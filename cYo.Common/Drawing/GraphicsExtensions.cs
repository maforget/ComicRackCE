using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using cYo.Common.ComponentModel;

namespace cYo.Common.Drawing
{
	public static class GraphicsExtensions
	{
		public static IDisposable SaveState(this Graphics graphics)
		{
			if (graphics == null)
			{
				return null;
			}
			GraphicsState gs = graphics.Save();
			return new LeanDisposer(delegate
			{
				graphics.Restore(gs);
			});
		}

		public static IDisposable AntiAlias(this Graphics graphics, SmoothingMode mode = SmoothingMode.AntiAlias)
		{
			if (graphics == null || mode == graphics.SmoothingMode)
			{
				return null;
			}
			SmoothingMode sm = graphics.SmoothingMode;
			graphics.SmoothingMode = mode;
			return new LeanDisposer(delegate
			{
				graphics.SmoothingMode = sm;
			});
		}

		public static IDisposable TextRendering(this Graphics graphics, TextRenderingHint hint)
		{
			if (graphics == null || graphics.TextRenderingHint == hint)
			{
				return null;
			}
			TextRenderingHint oldHint = graphics.TextRenderingHint;
			graphics.TextRenderingHint = hint;
			return new LeanDisposer(delegate
			{
				graphics.TextRenderingHint = oldHint;
			});
		}

		public static IDisposable HighQuality(this Graphics graphics, bool enabled, float scale = 0f, bool forceHQ = false)
		{
			InterpolationMode oim = graphics.InterpolationMode;
			InterpolationMode interpolationMode = ((forceHQ || (enabled && scale < 0.5f)) ? InterpolationMode.HighQualityBicubic : InterpolationMode.Default);
			if (oim == interpolationMode)
			{
				return null;
			}
			graphics.InterpolationMode = interpolationMode;
			return new LeanDisposer(delegate
			{
				graphics.InterpolationMode = oim;
			});
		}

		public static IDisposable HighQuality(this Graphics graphics, bool enabled, SizeF dest, SizeF src, bool forceHQ = false)
		{
			dest = graphics.TransformRectangle(CoordinateSpace.Device, CoordinateSpace.World, dest.ToRectangle()).Size;
			return graphics.HighQuality(enabled, src.GetScale(dest), forceHQ);
		}

		public static IDisposable Fast(this Graphics graphics, bool ultraFast = false)
		{
			IDisposable result = graphics.SaveState();
			if (ultraFast)
			{
				graphics.CompositingMode = CompositingMode.SourceOver;
				graphics.CompositingQuality = CompositingQuality.HighSpeed;
				graphics.SmoothingMode = SmoothingMode.None;
				graphics.PixelOffsetMode = PixelOffsetMode.Half;
				graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			}
			else
			{
				graphics.CompositingMode = CompositingMode.SourceOver;
				graphics.CompositingQuality = CompositingQuality.HighSpeed;
				graphics.SmoothingMode = SmoothingMode.HighSpeed;
				graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;
				graphics.InterpolationMode = InterpolationMode.Low;
			}
			return result;
		}

		public static Point TransformPoint(this Graphics graphics, CoordinateSpace destSpace, CoordinateSpace srcSpace, Point pt)
		{
			Point[] array = new Point[1]
			{
				pt
			};
			graphics.TransformPoints(destSpace, srcSpace, array);
			return array[0];
		}

		public static RectangleF TransformRectangle(this Graphics graphics, CoordinateSpace destSpace, CoordinateSpace srcSpace, RectangleF rect)
		{
			PointF[] array = rect.ToPoints();
			graphics.TransformPoints(destSpace, srcSpace, array);
			return array.ToRectangle();
		}

		public static void DrawImage(this Graphics graphics, Image image, Rectangle bounds, Rectangle src = default(Rectangle), float opacity = 1f)
		{
			if (opacity <= 0.05f)
			{
				return;
			}
			if (src.IsEmpty())
			{
				src = new Rectangle(0, 0, image.Width, image.Height);
			}
			if (opacity >= 0.95f)
			{
				graphics.DrawImage(image, bounds, src, GraphicsUnit.Pixel);
				return;
			}
			using (ImageAttributes imageAttributes = new ImageAttributes())
			{
				ColorMatrix colorMatrix = new ColorMatrix
				{
					Matrix33 = Math.Max(0.05f, opacity)
				};
				imageAttributes.SetColorMatrix(colorMatrix);
				graphics.DrawImage(image, bounds, src.X, src.Y, src.Width, src.Height, GraphicsUnit.Pixel, imageAttributes);
			}
		}

		public static void DrawImage(this Graphics graphics, Image image, Rectangle bounds, float opacity = 1f)
		{
			graphics.DrawImage(image, bounds, Rectangle.Empty, opacity);
		}

		public static void DrawImage(this Graphics graphics, Image image, int x, int y, float opacity = 1f)
		{
			graphics.DrawImage(image, new Rectangle(new Point(x, y), image.Size), opacity);
		}

		public static void DrawImage(this Graphics gr, Bitmap bitmap, Rectangle destination, int x, int y, int width, int height, BitmapAdjustment adjustment, float opacity)
		{
			if (opacity < 0.05f)
			{
				return;
			}
			if (!adjustment.HasSharpening && !adjustment.HasGamma)
			{
				float wp = 1f;
				float bp = 0f;
				if (adjustment.HasAutoContrast)
				{
					bitmap.GetBlackWhitePoint(out bp, out wp);
				}
				if (adjustment.IsEmpty && wp >= 0.95f && bp <= 0.05f && opacity >= 0.95f)
				{
					gr.DrawImage(bitmap, destination, x, y, width, height, GraphicsUnit.Pixel);
					return;
				}
				using (ImageAttributes imageAttributes = new ImageAttributes())
				{
					ColorMatrix colorMatrix = ImageProcessing.CreateColorMatrix(bp, wp, adjustment.Contrast, adjustment.Brightness, adjustment.Saturation, adjustment.WhitePointColor);
					colorMatrix.Matrix33 = Math.Max(0.05f, opacity);
					imageAttributes.SetColorMatrix(colorMatrix);
					gr.DrawImage(bitmap, destination, x, y, width, height, GraphicsUnit.Pixel, imageAttributes);
				}
			}
			else
			{
				using (Bitmap image = bitmap.CreateAdjustedBitmap(adjustment, PixelFormat.Format32bppArgb, alwaysClone: true))
				{
					gr.DrawImage(image, destination, new Rectangle(x, y, width, height), opacity);
				}
			}
		}

		public static void DrawImage(this Graphics gr, Bitmap bitmap, Rectangle destination, int x, int y, int width, int height, BitmapAdjustment adjustment)
		{
			gr.DrawImage(bitmap, destination, x, y, width, height, adjustment, 1f);
		}

		public static void DrawImage(this Graphics gr, Bitmap bitmap, Rectangle destination, BitmapAdjustment it, float opacity)
		{
			gr.DrawImage(bitmap, destination, 0, 0, bitmap.Width, bitmap.Height, it, opacity);
		}

		public static void DrawImage(this Graphics gr, Bitmap bitmap, Rectangle destination, BitmapAdjustment adjustment)
		{
			gr.DrawImage(bitmap, destination, adjustment, 1f);
		}

		public static void DrawShadow(this Graphics graphics, Rectangle rectangle, int depth, Color color, float opacity)
		{
			if (graphics == null)
			{
				throw new ArgumentNullException();
			}
			Rectangle rect = new Rectangle(rectangle.Right, rectangle.Top + depth, depth, rectangle.Height);
			Rectangle rect2 = new Rectangle(rectangle.Left + depth, rectangle.Bottom, rectangle.Width - depth, depth);
			using (Brush brush = new SolidBrush(Color.FromArgb((int)(255f * opacity), color)))
			{
				graphics.FillRectangle(brush, rect);
				graphics.FillRectangle(brush, rect2);
			}
		}

		public static void DrawShadow(this Graphics graphics, Rectangle rectangle, int depth, Color color, float opacity, BlurShadowType bst, BlurShadowParts parts)
		{
			using (Bitmap image = CreateShadowBitmap(bst, color, depth, opacity))
			{
				if ((parts & BlurShadowParts.Center) != 0)
				{
					using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255f * opacity), color)))
					{
						graphics.FillRectangle(brush, rectangle.Left + depth, rectangle.Top + depth, rectangle.Width - 2 * depth, rectangle.Height - 2 * depth);
					}
				}
				if ((parts & BlurShadowParts.TopLeft) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Left, rectangle.Top, depth, depth), 0, 0, depth, depth, GraphicsUnit.Pixel);
				}
				if ((parts & BlurShadowParts.TopCenter) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Left + depth, rectangle.Top, rectangle.Width - 2 * depth, depth), depth, 0, 1, depth, GraphicsUnit.Pixel);
				}
				if ((parts & BlurShadowParts.TopRight) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Right - depth, rectangle.Top, depth, depth), depth, 0, depth, depth, GraphicsUnit.Pixel);
				}
				if ((parts & BlurShadowParts.CenterRight) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Right - depth, rectangle.Top + depth, depth, rectangle.Height - 2 * depth), depth, depth, depth, 1, GraphicsUnit.Pixel);
				}
				if ((parts & BlurShadowParts.BottomRight) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Right - depth, rectangle.Bottom - depth, depth, depth), depth, depth, depth, depth, GraphicsUnit.Pixel);
				}
				if ((parts & BlurShadowParts.BottomCenter) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Left + depth, rectangle.Bottom - depth, rectangle.Width - 2 * depth, depth), depth, depth, 1, depth, GraphicsUnit.Pixel);
				}
				if ((parts & BlurShadowParts.BottomLeft) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Left, rectangle.Bottom - depth, depth, depth), 0, depth, depth, depth, GraphicsUnit.Pixel);
				}
				if ((parts & BlurShadowParts.CenterLeft) != 0)
				{
					graphics.DrawImage(image, new Rectangle(rectangle.Left, rectangle.Top + depth, depth, rectangle.Height - 2 * depth), 0, depth, depth, 1, GraphicsUnit.Pixel);
				}
			}
		}

		public static Bitmap CreateShadowBitmap(BlurShadowType bst, Color shadowColor, int depth, float maxOpacity)
		{
			Color color = Color.FromArgb((int)(255f * maxOpacity), shadowColor);
			Color color2 = Color.FromArgb(0, shadowColor);
			if (bst == BlurShadowType.Inside)
			{
				Color color3 = color;
				color = color2;
				color2 = color3;
			}
			depth *= 2;
			using (GraphicsPath graphicsPath = new GraphicsPath())
			{
				graphicsPath.AddEllipse(0, 0, depth, depth);
				using (PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath))
				{
					pathGradientBrush.CenterColor = color;
					pathGradientBrush.SurroundColors = new Color[1]
					{
						color2
					};
					Bitmap bitmap = new Bitmap(depth, depth);
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.Clear(color2);
						graphics.CompositingMode = CompositingMode.SourceCopy;
						graphics.FillEllipse(pathGradientBrush, 0, 0, depth, depth);
					}
					return bitmap;
				}
			}
		}
	}
}
