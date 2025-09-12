using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.Drawing3D;
using cYo.Common.Mathematics;
using cYo.Projects.ComicRack.Engine.Properties;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public class ComicBox3D
	{
		private const float DefaultRotateX = 54f;

		private const float DefaultRotateY = -28f;

		private const float DefaultRotateZ = 12f;

		private const float DefaultDistance = 1.5f;

		private static readonly Bitmap shadowMap = Resources.ShadowMap;

		public static Bitmap CreateDefaultBook(Bitmap cover, Bitmap coverBack, Size size, int pages, ComicBox3DOptions options = ComicBox3DOptions.Default)
		{
			return Create(cover, coverBack, size, (float)pages / 25f * 0.025f, DefaultDistance, DefaultRotateX, DefaultRotateY, DefaultRotateZ, options);
		}

		public static Bitmap Create(Bitmap cover, Bitmap coverBack, Size size, float thickness, float distance, float rx, float ry, float rz, ComicBox3DOptions options = ComicBox3DOptions.Default)
		{
			CyoGl cyoGl = new CyoGl();
			cyoGl.Lights.Add(new Light
			{
				Position = new Vector3(0.5f, 3f, -5f),
				LightType = LightType.Point,
				DistanceFallOff = true,
				DiffusePower = 80f
			});
			Size size2 = size;
			float num = 2f / 3f;
			float num2 = 1f;
			float num3 = thickness.Clamp(0.02f, 0.15f);
			if (num > 1f)
			{
				num = 1f;
				num2 = (float)cover.Height / (float)cover.Width;
			}
			cyoGl.Projection = Matrix4.PerspectiveFOVInfinity(Numeric.DegToRad(35f), 1f, 0f - distance);
			Matrix4 identity = Matrix4.Identity;
			identity *= Matrix4.Translation((0f - num) / 2f, (0f - num2) / 2f, (0f - num3) / 2f);
			identity *= Matrix4.RotationX(Numeric.DegToRad(rx));
			identity *= Matrix4.RotationY(Numeric.DegToRad(ry));
			identity *= Matrix4.RotationZ(Numeric.DegToRad(rz));
			cyoGl.ModelView *= identity;
			Rectangle rectangle = cover.Size.ToRectangle();
			Rectangle clip = coverBack?.Size.ToRectangle() ?? rectangle;
			if (options.HasFlag(ComicBox3DOptions.SplitDoublePages))
			{
				if (rectangle.Width > rectangle.Height)
				{
					int width = rectangle.Width;
					rectangle.Width = Math.Min(width, rectangle.Height * cover.Height / cover.Width);
					rectangle.X = width - rectangle.Width;
				}
				if (clip.Width > clip.Height)
				{
					int width2 = clip.Width;
					clip.Width = Math.Min(width2, clip.Height * cover.Height / cover.Width);
					clip.X = width2 - clip.Width;
				}
			}
			Size size3 = shadowMap.Size;
			Vertex[] array = new Vertex[4]
			{
				new Vertex(0f, num2, 0f, 0f, 0f),
				new Vertex(num, num2, 0f, rectangle.Width - 1, 0f),
				new Vertex(num, 0f, 0f, rectangle.Width - 1, rectangle.Height - 1),
				new Vertex(0f, 0f, 0f, 0f, rectangle.Height - 1)
			};
			Vertex[] array2 = new Vertex[4]
			{
				new Vertex(0f, 0f, num3, 0f, clip.Height - 1),
				new Vertex(num, 0f, num3, clip.Width - 1, clip.Height - 1),
				new Vertex(num, num2, num3, clip.Width - 1, 0f),
				new Vertex(0f, num2, num3, 0f, 0f)
			};
			RectangleF rectangleF = new RectangleF((float)rectangle.Width * 0.1f, (float)rectangle.Height * 0.05f, (float)rectangle.Width * 0.8f, (float)rectangle.Height * num3);
			Vertex[] array3 = new Vertex[4]
			{
				new Vertex(0f, num2, 0f, rectangleF.Left, rectangleF.Top),
				new Vertex(0f, 0f, 0f, rectangleF.Right, rectangleF.Top),
				new Vertex(0f, 0f, num3, rectangleF.Right, rectangleF.Bottom),
				new Vertex(0f, num2, num3, rectangleF.Left, rectangleF.Bottom)
			};
			Vertex[] array4 = new Vertex[4]
			{
				new Vertex(0f, 0f, 0f),
				new Vertex(num, 0f, 0f),
				new Vertex(num, 0f, num3),
				new Vertex(0f, 0f, num3)
			};
			Vertex[] array5 = new Vertex[4]
			{
				new Vertex(0f, num2, 0f),
				new Vertex(0f, num2, num3),
				new Vertex(num, num2, num3),
				new Vertex(num, num2, 0f)
			};
			Vertex[] array6 = new Vertex[4]
			{
				new Vertex(num, num2, 0f),
				new Vertex(num, num2, num3),
				new Vertex(num, 0f, num3),
				new Vertex(num, 0f, 0f)
			};
			IEnumerable<Vertex> enumerable = array.Concat(array2).Concat(array3).Concat(array4)
				.Concat(array5)
				.Concat(array6);
			Vertex[] array7 = null;
			if (options.HasFlag(ComicBox3DOptions.SimpleShadow))
			{
				float num4 = 0.015f + num3 / 4f;
				array7 = new Vertex[4]
				{
					new Vertex(0f - num4, num2 + num4, num3, 0f, 0f),
					new Vertex(num + num4, num2 + num4, num3, size3.Width - 1, 0f),
					new Vertex(num + num4, 0f - num4, num3, size3.Width - 1, size3.Height - 1),
					new Vertex(0f - num4, 0f - num4, num3, 0f, size3.Height - 1)
				};
				enumerable = enumerable.Concat(array7);
			}
			if (options.HasFlag(ComicBox3DOptions.Wireless))
			{
				enumerable.ForEach(delegate(Vertex p)
				{
					p.SetColor(Color.Black);
				});
			}
			RectangleF viewportExtent = cyoGl.GetViewportExtent(enumerable, toFrameBuffer: false);
			float num5 = Math.Max(viewportExtent.Width, viewportExtent.Height);
			viewportExtent.X += (viewportExtent.Width - num5) / 2f;
			viewportExtent.Y += (viewportExtent.Height - num5) / 2f;
			float num8 = (viewportExtent.Width = (viewportExtent.Height = num5));
			cyoGl.Viewport = viewportExtent;
			if (options.HasFlag(ComicBox3DOptions.Filter))
			{
				size = size.Scale(1.5f);
			}
			Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
			try
			{
				using (BitmapFrameBuffer frameBuffer = new BitmapFrameBuffer(bitmap))
				{
					using (BitmapFrameBuffer texture = new BitmapFrameBuffer(shadowMap))
					{
						using (BitmapFrameBuffer bitmapFrameBuffer = new BitmapFrameBuffer(cover, rectangle))
						{
							using (BitmapFrameBuffer texture2 = ((coverBack != null) ? new BitmapFrameBuffer(coverBack, clip) : bitmapFrameBuffer))
							{
								cyoGl.FrameBuffer = frameBuffer;
								cyoGl.Wireless = options.HasFlag(ComicBox3DOptions.Wireless);
								cyoGl.BacksideCulling = true;
								cyoGl.ShadingModel = ShadingModel.Gourard;
								if (array7 != null)
								{
									cyoGl.Texture = texture;
									cyoGl.DrawQuad(array7);
								}
								cyoGl.Texture = bitmapFrameBuffer;
								cyoGl.DrawQuad(array);
								cyoGl.DrawQuad(array3);
								cyoGl.Texture = texture2;
								cyoGl.DrawQuad(array2);
								cyoGl.Texture = null;
								cyoGl.DrawQuad(array4);
								cyoGl.DrawQuad(array5);
								cyoGl.DrawQuad(array6);
							}
						}
					}
				}
				Rectangle rectangle2 = Rectangle.Truncate(cyoGl.GetViewportExtent(enumerable));
				if (options.HasFlag(ComicBox3DOptions.Filter))
				{
					rectangle2 = rectangle2.Scale(2f / 3f);
					using (Bitmap bmp = bitmap)
					{
						bitmap = bmp.Scale(size2, BitmapResampling.GdiPlusHQ);
					}
				}
				if (!options.HasFlag(ComicBox3DOptions.Trim))
				{
					Bitmap result = bitmap;
					bitmap = null;
					return result;
				}
				if (rectangle2.Width == 0 || rectangle2.Height == 0)
				{
					return null;
				}
				return bitmap.CreateCopy(rectangle2);
			}
			finally
			{
				bitmap?.Dispose();
			}
		}
	}
}
