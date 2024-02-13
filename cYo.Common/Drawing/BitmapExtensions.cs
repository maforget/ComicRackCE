using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using cYo.Common.ComponentModel;
using cYo.Common.Runtime;

namespace cYo.Common.Drawing
{
	public static class BitmapExtensions
	{
		private const float Pi = (float)Math.PI;

		public static Bitmap Resize(this Bitmap bmp, Size size, BitmapResampling resampling = BitmapResampling.BilinearHQ, PixelFormat format = PixelFormat.Format32bppArgb)
		{
			if (bmp == null || size.IsEmpty())
			{
				return null;
			}
			if (bmp.Size.IsEmpty())
			{
				return bmp;
			}
			switch (resampling)
			{
			case BitmapResampling.FastAndUgly:
				return ImageProcessing.ResizeFast(bmp, size.Width, size.Height, format, ResizeFastInterpolation.NearestNeighbor);
			case BitmapResampling.FastBilinear:
				return ImageProcessing.ResizeFast(bmp, size.Width, size.Height, format, ResizeFastInterpolation.Bilinear);
			case BitmapResampling.FastBicubic:
				return ImageProcessing.ResizeFast(bmp, size.Width, size.Height, format, ResizeFastInterpolation.Bicubic);
			case BitmapResampling.BilinearHQ:
				return ImageProcessing.ResizeBiliniearHQ(bmp, size.Width, size.Height, format);
			case BitmapResampling.GdiPlus:
				return ImageProcessing.ResizeGdi(bmp, size.Width, size.Height, format);
			case BitmapResampling.GdiPlusHQ:
				return ImageProcessing.ResizeGdi(bmp, size.Width, size.Height, format, highQuality: true);
			default:
				throw new ArgumentOutOfRangeException("resampling");
			}
		}

		public static Bitmap Resize(this Bitmap bmp, int width, int height)
		{
			return bmp.Resize(new Size(width, height));
		}

		public static Bitmap Scale(this Bitmap bmp, Size size, BitmapResampling resampling = BitmapResampling.BilinearHQ, PixelFormat format = PixelFormat.Format32bppArgb)
		{
			if (bmp == null)
			{
				return null;
			}
			if (bmp.Size.IsEmpty())
			{
				return bmp;
			}
			return bmp.Resize(bmp.Size.ToRectangle(size).Size, resampling, format);
		}

		public static Bitmap Scale(this Bitmap bmp, int width, int height)
		{
			return bmp.Scale(new Size(width, height));
		}

		public static Bitmap Scale(this Bitmap bmp, float factor)
		{
			return bmp.Resize(new Size((int)((float)bmp.Width * factor), (int)((float)bmp.Height * factor)), BitmapResampling.GdiPlus);
		}

		private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
		{
			return ImageCodecInfo.GetImageEncoders().FirstOrDefault((ImageCodecInfo ici) => ici.FormatID == format.Guid);
		}

		public static void SaveJpeg(this Image image, Stream s, int quality = -1)
		{
			image.SaveImage(s, ImageFormat.Jpeg, -1, quality);
		}

		public static void SaveJpeg(this Image image, string file, int quality = -1)
		{
			image.SaveImage(file, ImageFormat.Jpeg, -1, quality);
		}

		public static void SaveImage(this Image image, string file, ImageFormat imageFormat, int colorDepth = -1, int quality = -1)
		{
			using (Stream ms = File.Create(file))
			{
				image.SaveImage(ms, imageFormat, colorDepth, quality);
			}
		}

		public static void SaveImage(this Image image, Stream ms, ImageFormat imageFormat, int colorDepth = -1, int quality = -1)
		{
			int num = 0;
			if (colorDepth > 0)
			{
				num++;
			}
			if (quality > -1)
			{
				num++;
			}
			if (num == 0)
			{
				image.Save(ms, imageFormat);
				return;
			}
			using (EncoderParameters encoderParameters = new EncoderParameters(num))
			{
				ImageCodecInfo encoderInfo = GetEncoderInfo(imageFormat);
				if (quality >= 0)
				{
					encoderParameters.Param[--num] = new EncoderParameter(Encoder.Quality, quality);
				}
				if (colorDepth > 0)
				{
					encoderParameters.Param[--num] = new EncoderParameter(Encoder.ColorDepth, colorDepth);
				}
				image.Save(ms, encoderInfo, encoderParameters);
			}
		}

		public static byte[] ImageToJpegBytes(this Image image, int quality = -1)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				image.SaveJpeg(memoryStream, quality);
				return memoryStream.ToArray();
			}
		}

		public static byte[] ImageToBytes(this Image image, ImageFormat imageFormat, int colorDepth = -1, int quality = -1)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				image.SaveImage(memoryStream, imageFormat, colorDepth, quality);
				return memoryStream.ToArray();
			}
		}

		public static Bitmap LoadIcon(Stream stream, Color backColor)
		{
			using (Icon icon = new Icon(stream, 1024, 1024))
			{
				Bitmap bitmap = new Bitmap(icon.Width, icon.Height, PixelFormat.Format32bppArgb);
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.Clear(backColor);
					graphics.DrawIcon(icon, new Rectangle(0, 0, icon.Width, icon.Height));
				}
				return bitmap;
			}
		}

		public static Bitmap LoadIcon(string file, Color backColor)
		{
			using (FileStream stream = File.OpenRead(file))
			{
				return LoadIcon(stream, backColor);
			}
		}

		public static Bitmap BitmapFromStream(Stream stream, PixelFormat pixelFormat, bool alwaysCreateCopy)
		{
			Bitmap bitmap = null;
			try
			{
				bitmap = LoadBitmap32BitFix(stream);
				if (pixelFormat == PixelFormat.Undefined)
				{
					pixelFormat = bitmap?.PixelFormat ?? PixelFormat.Format32bppArgb;
				}
				if (alwaysCreateCopy || bitmap == null || bitmap.PixelFormat != pixelFormat)
				{
					return bitmap.CreateCopy(pixelFormat, alwaysTrueCopy: true);
				}
				return bitmap;
			}
			catch
			{
				bitmap?.Dispose();
				throw;
			}
		}

		private static Bitmap LoadBitmap32BitFix(Stream s)
		{
			if (Machine.Is64Bit)
			{
				return Image.FromStream(s, useEmbeddedColorManagement: false, validateImageData: false) as Bitmap;
			}
			try
			{
				Bitmap bitmap = Image.FromStream(s, useEmbeddedColorManagement: false, validateImageData: false) as Bitmap;
				if (bitmap == null)
				{
					return bitmap;
				}
				if (bitmap.Width == 0 || bitmap.Height == 0)
				{
					throw new IOException();
				}
				return bitmap;
			}
			catch (Exception)
			{
				MemoryStream memoryStream = new MemoryStream();
				s.Position = 0L;
				if (!JpegFile.RemoveExif(s, memoryStream))
				{
					return null;
				}
				memoryStream.Position = 0L;
				return Image.FromStream(memoryStream, useEmbeddedColorManagement: false, validateImageData: false) as Bitmap;
			}
		}

		public static Bitmap BitmapFromStream(Stream stream, bool alwaysCreateCopy = false)
		{
			return BitmapFromStream(stream, PixelFormat.Format32bppArgb, alwaysCreateCopy);
		}

		public static Bitmap BitmapFromBytes(byte[] data, PixelFormat pixelFormat)
		{
			return BitmapFromStream(new MemoryStream(data), pixelFormat, alwaysCreateCopy: false);
		}

		public static Bitmap BitmapFromBytes(byte[] data)
		{
			return BitmapFromStream(new MemoryStream(data));
		}

		public static Bitmap BitmapFromFile(string file, bool alwaysCreateCopy = true)
		{
			using (FileStream stream = File.OpenRead(file))
			{
				return BitmapFromStream(stream, alwaysCreateCopy);
			}
		}

		public static Color GetAverageColor(this Bitmap bitmap, Rectangle rectangle)
		{
			if (bitmap == null)
			{
				return Color.Empty;
			}
			rectangle = Rectangle.Intersect(rectangle, new Rectangle(Point.Empty, bitmap.Size));
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = rectangle.Width * rectangle.Height;
			for (int i = rectangle.Left; i < rectangle.Right; i++)
			{
				for (int j = rectangle.Top; j < rectangle.Bottom; j++)
				{
					Color pixel = bitmap.GetPixel(i, j);
					num += pixel.R;
					num2 += pixel.G;
					num3 += pixel.B;
				}
			}
			return Color.FromArgb(num / num4, num2 / num4, num3 / num4);
		}

		public static Color GetAverageColor(this Bitmap bitmap, Point location, Size size)
		{
			return bitmap.GetAverageColor(new Rectangle(location, size));
		}

		public static Color GetAverageColor(this Bitmap bitmap, int x, int y, int size)
		{
			return bitmap.GetAverageColor(new Point(x, y), new Size(size, size));
		}

		public static Bitmap Clone(this Bitmap bmp, PixelFormat format, bool alwaysClone = false)
		{
			if (bmp == null || (bmp.PixelFormat == format && !alwaysClone))
			{
				return bmp;
			}
			return bmp.Clone(bmp.Size.ToRectangle(), format);
		}

		public static Bitmap ToOptimized(this Bitmap bmp, bool disposeOriginal = true)
		{
			Bitmap bitmap = bmp.Clone(PixelFormat.Format32bppPArgb);
			if (bmp != bitmap)
			{
				bmp.SafeDispose();
			}
			return bitmap;
		}

		public static Bitmap CreateCopy(this Image image, Rectangle clip, PixelFormat format, bool alwaysTrueCopy = false)
		{
			Bitmap bitmap = null;
			try
			{
				if (image == null)
				{
					return null;
				}
				if (format == PixelFormat.Undefined)
				{
					format = image.PixelFormat;
				}
				Bitmap bitmap2 = image as Bitmap;
				if (bitmap2 != null)
				{
					if (!alwaysTrueCopy && image.Size == clip.Size && image.PixelFormat == format)
					{
						return (Bitmap)bitmap2.Clone();
					}
					if (format == PixelFormat.Format32bppArgb || format == PixelFormat.Format24bppRgb)
					{
						try
						{
							return bitmap2.Copy(clip, format);
						}
						catch
						{
						}
					}
				}
				bitmap = new Bitmap(clip.Width, clip.Height, format);
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.SmoothingMode = SmoothingMode.None;
					graphics.InterpolationMode = InterpolationMode.Low;
					graphics.DrawImage(image, new Rectangle(0, 0, clip.Width, clip.Height), clip, GraphicsUnit.Pixel);
				}
				return bitmap;
			}
			catch
			{
				bitmap?.Dispose();
				return null;
			}
		}

		public static Bitmap CreateCopy(this Image image, Rectangle clip, bool alwaysTrueCopy = false)
		{
			return image.CreateCopy(clip, PixelFormat.Format32bppArgb, alwaysTrueCopy);
		}

		public static Bitmap CreateCopy(this Image image, PixelFormat pixelFormat, bool alwaysTrueCopy = false)
		{
			return image.CreateCopy(new Rectangle(0, 0, image.Width, image.Height), pixelFormat, alwaysTrueCopy);
		}

		public static Bitmap CreateCopy(this Image image, bool alwaysTrueCopy = false)
		{
			return image.CreateCopy(PixelFormat.Format32bppArgb, alwaysTrueCopy);
		}

		public static Bitmap Crop(this Image image, Rectangle clip, bool alwaysTrueCopy = false)
		{
			return image.CreateCopy(clip, image.PixelFormat, alwaysTrueCopy);
		}

		public static Bitmap Rotate(this Bitmap image, ImageRotation rotation)
		{
			if (image == null)
			{
				return null;
			}
			Bitmap bitmap = (Bitmap)image.Clone();
			switch (rotation)
			{
			case ImageRotation.Rotate90:
				bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
				break;
			case ImageRotation.Rotate180:
				bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
				break;
			case ImageRotation.Rotate270:
				bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
				break;
			}
			return bitmap;
		}

		public static Bitmap Distort(this Bitmap baseBitmap, Point topleft, Point topright, Point bottomleft, Point bottomright)
		{
			Point[] array = new Point[4]
			{
				topleft,
				topright,
				bottomright,
				bottomleft
			};
			int val = int.MaxValue;
			int num = int.MinValue;
			int val2 = int.MaxValue;
			int num2 = int.MinValue;
			Point[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Point point = array2[i];
				val = Math.Min(val, point.X);
				num = Math.Max(num, point.X);
				val2 = Math.Min(val2, point.Y);
				num2 = Math.Max(num2, point.Y);
			}
			Rectangle rectangle = new Rectangle(0, 0, num, num2);
			using (Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height))
			{
				using (Bitmap inputBitmap = new Bitmap(baseBitmap, rectangle.Width, rectangle.Height))
				{
					PointF pointF = topleft;
					PointF pointF2 = topright;
					PointF pointF3 = bottomright;
					PointF pointF4 = bottomleft;
					float angularCoefficient = GetAngularCoefficient(pointF, pointF2);
					float angularCoefficient2 = GetAngularCoefficient(pointF4, pointF3);
					float angularCoefficient3 = GetAngularCoefficient(pointF, pointF4);
					float angularCoefficient4 = GetAngularCoefficient(pointF3, pointF2);
					PointF? intersection = GetIntersection(pointF2, angularCoefficient, pointF3, angularCoefficient2);
					PointF? intersection2 = GetIntersection(pointF, angularCoefficient3, pointF2, angularCoefficient4);
					using (FastBitmap fastBitmap2 = new FastBitmap(bitmap))
					{
						using (FastBitmap fastBitmap = new FastBitmap(inputBitmap))
						{
							for (int j = 0; j < rectangle.Height; j++)
							{
								for (int k = 0; k < rectangle.Width; k++)
								{
									PointF pointF5 = new PointF(k, j);
									float m = ((!intersection.HasValue) ? angularCoefficient : GetAngularCoefficient(intersection.Value, pointF5));
									float m2 = ((!intersection2.HasValue) ? angularCoefficient4 : GetAngularCoefficient(intersection2.Value, pointF5));
									PointF? intersection3 = GetIntersection(pointF5, m, pointF, angularCoefficient3);
									PointF a = (intersection3.HasValue ? intersection3.Value : pointF);
									PointF? intersection4 = GetIntersection(pointF5, m, pointF2, angularCoefficient4);
									PointF a2 = ((!intersection4.HasValue) ? pointF3 : intersection4.Value);
									PointF? intersection5 = GetIntersection(pointF5, m2, pointF, angularCoefficient);
									PointF a3 = (intersection5.HasValue ? intersection5.Value : pointF2);
									PointF? intersection6 = GetIntersection(pointF5, m2, pointF4, angularCoefficient2);
									PointF a4 = (intersection6.HasValue ? intersection6.Value : pointF4);
									float distance = GetDistance(a, pointF5);
									float distance2 = GetDistance(a2, pointF5);
									float distance3 = GetDistance(a3, pointF5);
									float distance4 = GetDistance(a4, pointF5);
									int num3 = (int)Math.Round((float)rectangle.Width * distance / (distance + distance2));
									int num4 = (int)Math.Round((float)rectangle.Height * distance3 / (distance3 + distance4));
									if (num4 >= 0 && num3 >= 0 && num4 < rectangle.Height && num3 < rectangle.Width)
									{
										Color pixel = fastBitmap.GetPixel(num3, num4);
										fastBitmap2.SetPixel(k, j, pixel);
									}
								}
							}
						}
					}
					Bitmap bitmap2 = new Bitmap(rectangle.Width, rectangle.Height);
					using (Graphics graphics = Graphics.FromImage(bitmap2))
					{
						using (GraphicsPath graphicsPath = new GraphicsPath())
						{
							graphicsPath.AddLines(new PointF[4]
							{
								pointF,
								pointF2,
								pointF3,
								pointF4
							});
							graphicsPath.CloseFigure();
							graphics.Clip = new Region(graphicsPath);
							graphics.DrawImage(bitmap, 0, 0);
						}
					}
					return bitmap2;
				}
			}
		}

		public static Bitmap CreateMosaicImage(this IEnumerable<Bitmap> images, int imagesInEachRow, Size size, Color backColor)
		{
			Bitmap bitmap = new Bitmap(size.Width, size.Height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				Rectangle rect = new Rectangle(Point.Empty, size);
				using (Brush brush = new SolidBrush(backColor))
				{
					graphics.FillRectangle(brush, rect);
				}
				float num = (float)size.Width / (float)imagesInEachRow;
				float num2 = float.MaxValue;
				PointF pointF = new PointF(0f, 0f);
				foreach (Bitmap image in images)
				{
					float num3 = num / (float)image.Width;
					float val = (float)image.Height * num3;
					num2 = Math.Min(val, num2);
					if (!(pointF.Y > (float)size.Height))
					{
						graphics.DrawImage(image, new RectangleF(pointF.X, pointF.Y, num, (float)image.Height * num3));
						pointF.X += num;
						if (pointF.X >= (float)size.Width)
						{
							pointF.X = 0f;
							pointF.Y += num2;
							num2 = float.MaxValue;
						}
						continue;
					}
					return bitmap;
				}
				return bitmap;
			}
		}

		private static float GetDistance(PointF a, PointF b)
		{
			float num = a.X - b.X;
			float num2 = a.Y - b.Y;
			return (float)Math.Sqrt(num * num + num2 * num2);
		}

		private static PointF? GetIntersection(PointF u, float m1, PointF v, float m2)
		{
			if (u == v)
			{
				return u;
			}
			if (m1 == m2)
			{
				return null;
			}
			float num = float.Epsilon;
			float y = float.Epsilon;
			if (float.IsInfinity(m1))
			{
				num = u.X;
				y = v.Y + m2 * (0f - v.X + u.X);
			}
			if (float.IsInfinity(m2))
			{
				num = v.X;
				y = u.Y + m1 * (0f - u.X + v.X);
			}
			if (num == float.Epsilon)
			{
				float num2 = u.Y - m1 * u.X;
				float num3 = v.Y - m2 * v.X;
				num = (num2 - num3) / (m2 - m1);
				y = m1 * num + num2;
			}
			return new PointF(num, y);
		}

		private static float GetAngularCoefficient(PointF u, PointF v)
		{
			float angularCoefficientRads = GetAngularCoefficientRads(u, v);
			if (angularCoefficientRads % Pi == Pi / 2f)
			{
				return float.PositiveInfinity;
			}
			if (angularCoefficientRads % Pi == -Pi / 2f)
			{
				return float.NegativeInfinity;
			}
			return (float)Math.Tan(angularCoefficientRads);
		}

		private static float GetAngularCoefficientRads(PointF from, PointF to)
		{
			if (to.Y == from.Y)
			{
				if (!(from.X > to.X))
				{
					return 0f;
				}
				return Pi;
			}
			if (to.X == from.X)
			{
				if (!(to.Y < from.Y))
				{
					return Pi / 2f;
				}
				return -Pi / 2f;
			}
			float num = (float)Math.Atan((to.Y - from.Y) / (to.X - from.X));
			if (to.X < 0f)
			{
				if (num > 0f)
				{
					num += Pi / 2f;
				}
				else if (num < 0f)
				{
					num -= Pi;
				}
			}
			return num;
		}
	}
}
