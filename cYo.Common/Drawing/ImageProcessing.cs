using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Localize;
using cYo.Common.Mathematics;

namespace cYo.Common.Drawing
{
	public static class ImageProcessing
	{
		private struct RGBA
		{
			public float R;

			public float G;

			public float B;
		}

		private const short blueIndex = 0;

		private const short greenIndex = 1;

		private const short redIndex = 2;

		private const short alphaIndex = 3;

		private const float grayRed = 0.3086f;

		private const float grayGreen = 0.6094f;

		private const float grayBlue = 0.082f;

		private static void CheckFormat(PixelFormat format)
		{
			if (format != PixelFormat.Format24bppRgb && format != PixelFormat.Format32bppArgb && format != PixelFormat.Format32bppRgb && format != PixelFormat.Canonical)
			{
				throw new ArgumentException("Invalid bitmap format");
			}
		}

		private static void CheckFormat(Bitmap bitmap)
		{
			CheckFormat(bitmap.PixelFormat);
		}

		public unsafe static Bitmap Copy(this Bitmap source, Rectangle clip, PixelFormat format)
		{
			CheckFormat(source);
			CheckFormat(format);
			clip.Intersect(source.Size.ToRectangle());
			Bitmap bitmap = new Bitmap(clip.Width, clip.Height, format);
			BitmapData bitmapData = source.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly, source.PixelFormat);
			BitmapData bitmapData2 = bitmap.LockBits(new Rectangle(0, 0, clip.Width, clip.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
			try
			{
				IntPtr scan = bitmapData.Scan0;
				IntPtr scanTarget = bitmapData2.Scan0;
				int strideSource = bitmapData.Stride;
				int pixelWidthSource = bitmapData.Stride / source.Width;
				int strideTarget = bitmapData2.Stride;
				int num = bitmapData2.Stride / bitmap.Width;
				int i = clip.Left;
				int w = clip.Width;
				int height = clip.Height;
				byte* p = (byte*)(void*)scan + strideSource * clip.Y;
				if (pixelWidthSource == 3)
				{
					if (num == 3)
					{
						Parallel.For(0, height, delegate(int y)
						{
							byte* ptr7 = p + y * strideSource + i * pixelWidthSource;
							byte* ptr8 = (byte*)(void*)scanTarget + y * strideTarget;
							for (int m = 0; m < w; m++)
							{
								*(ptr8++) = *(ptr7++);
								*(ptr8++) = *(ptr7++);
								*(ptr8++) = *(ptr7++);
							}
						});
					}
					else
					{
						Parallel.For(0, height, delegate(int y)
						{
							byte* ptr5 = p + y * strideSource + i * pixelWidthSource;
							byte* ptr6 = (byte*)(void*)scanTarget + y * strideTarget;
							for (int l = 0; l < w; l++)
							{
								*(ptr6++) = *(ptr5++);
								*(ptr6++) = *(ptr5++);
								*(ptr6++) = *(ptr5++);
								*(ptr6++) = byte.MaxValue;
							}
						});
					}
				}
				else if (num == 3)
				{
					Parallel.For(0, height, delegate(int y)
					{
						byte* ptr3 = p + y * strideSource + i * pixelWidthSource;
						byte* ptr4 = (byte*)(void*)scanTarget + y * strideTarget;
						for (int k = 0; k < w; k++)
						{
							*(ptr4++) = *(ptr3++);
							*(ptr4++) = *(ptr3++);
							*(ptr4++) = *(ptr3++);
							ptr3++;
						}
					});
				}
				else
				{
					Parallel.For(0, height, delegate(int y)
					{
						int* ptr = (int*)(p + y * strideSource + i * pixelWidthSource);
						int* ptr2 = (int*)((byte*)(void*)scanTarget + y * strideTarget);
						for (int j = 0; j < w; j++)
						{
							int* intPtr = ptr2;
							ptr2 = intPtr + 1;
							int* intPtr2 = ptr;
							ptr = intPtr2 + 1;
							*intPtr = *intPtr2;
						}
					});
				}
			}
			finally
			{
				source.UnlockBits(bitmapData);
				bitmap.UnlockBits(bitmapData2);
			}
			return bitmap;
		}

		public static Bitmap Copy(this Bitmap source, PixelFormat format)
		{
			return source.Copy(source.Size.ToRectangle(), PixelFormat.Format32bppArgb);
		}

		public static Bitmap Copy(this Bitmap source)
		{
			return source.Copy(source.Size.ToRectangle(), PixelFormat.Format32bppArgb);
		}

		public unsafe static void ChangeAlpha(this Bitmap bitmap, Rectangle clipRectangle, float alphaStart, float alphaEnd)
		{
			if (bitmap == null || bitmap.PixelFormat != PixelFormat.Format32bppArgb)
			{
				throw new ArgumentException("must be 32 bit bitmap");
			}
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			float num = (alphaEnd - alphaStart) / (float)clipRectangle.Height;
			float num2 = alphaStart * 256f;
			try
			{
				int num3 = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				int num4 = clipRectangle.Width * num3;
				int num5 = stride - num4;
				byte* ptr = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * num3;
				for (byte* ptr2 = ptr + clipRectangle.Height * stride; ptr < ptr2; ptr += num5)
				{
					byte* ptr3 = ptr + num4;
					byte b = (byte)num2;
					for (; ptr < ptr3; ptr += num3)
					{
						ptr[3] = (byte)(ptr[3] * b >> 8);
					}
					num2 += num;
				}
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void ChangeAlpha(this Bitmap bitmap, float alphaStart, float alphaEnd)
		{
			bitmap.ChangeAlpha(new Rectangle(0, 0, bitmap.Width, bitmap.Height), alphaStart, alphaEnd);
		}

		public static void ChangeAlpha(this Bitmap bitmap, float alpha)
		{
			bitmap.ChangeAlpha(alpha, alpha);
		}

		public static void ChangeAlpha(this Bitmap bitmap, byte alpha)
		{
			bitmap.ChangeAlpha((float)(int)alpha / 255f);
		}

		public static Rectangle GetTransparentBounds(this Bitmap bitmap, byte alpha = 16)
		{
			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
			{
				return new Rectangle(0, 0, bitmap.Width, bitmap.Height);
			}
			using (FastBitmap fastBitmap = new FastBitmap(bitmap))
			{
				int width = bitmap.Width;
				int height = bitmap.Height;
				int num = width;
				int num2 = -1;
				int num3 = 0;
				int num4 = 0;
				for (int i = 0; i < height; i++)
				{
					int j;
					for (j = 0; j < width && fastBitmap.GetPixel(j, i).A < alpha; j++)
					{
					}
					int num5 = width - 1;
					while (num5 > j && fastBitmap.GetPixel(num5, i).A < alpha)
					{
						num5--;
					}
					num = Math.Min(j, num);
					num2 = Math.Max(num5, num2);
					if (j < num5)
					{
						num4 = Math.Max(i, num4);
					}
					else if (num4 == 0)
					{
						num3 = Math.Max(i, num3);
					}
				}
				if (num >= num2 || num3 >= num4)
				{
					return Rectangle.Empty;
				}
				return new Rectangle(num, num3, num2 - num + 1, num4 - num3 + 1);
			}
		}

		public static Bitmap CropTransparent(this Bitmap bitmap, bool width = true, bool height = true, byte alpha = 16)
		{
			Rectangle transparentBounds = bitmap.GetTransparentBounds(alpha);
			if (!width)
			{
				transparentBounds.X = 0;
				transparentBounds.Width = bitmap.Width;
			}
			if (!height)
			{
				transparentBounds.Y = 0;
				transparentBounds.Height = bitmap.Height;
			}
			return bitmap.Crop(transparentBounds);
		}

		public unsafe static void Invert(this Bitmap bitmap, Rectangle clipRectangle)
		{
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					for (byte* ptr2 = ptr + clipScanLen; ptr < ptr2; ptr += pixelWidth)
					{
						*ptr = (byte)(255 - *ptr);
						ptr[1] = (byte)(255 - ptr[1]);
						ptr[2] = (byte)(255 - ptr[2]);
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void Invert(this Bitmap bitmap)
		{
			bitmap.Invert(Rectangle.Empty);
		}

		public unsafe static void ToGrayScale(this Bitmap bitmap, Rectangle clipRectangle)
		{
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					for (byte* ptr2 = ptr + clipScanLen; ptr < ptr2; ptr += pixelWidth)
					{
						byte* intPtr = ptr;
						byte* intPtr2 = ptr + 1;
						byte b;
						ptr[2] = (b = (byte)(0.299f * (float)(int)ptr[2] + 0.587f * (float)(int)ptr[1] + 0.114f * (float)(int)(*ptr)));
						*intPtr2 = (b = b);
						*intPtr = b;
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void ToGrayScale(this Bitmap bitmap)
		{
			bitmap.ToGrayScale(Rectangle.Empty);
		}

		public unsafe static void ChangeBrightness(this Bitmap bitmap, Rectangle clipRectangle, int brightness)
		{
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					for (byte* ptr2 = ptr + clipScanLen; ptr < ptr2; ptr += pixelWidth)
					{
						*ptr = (byte)Math.Min(Math.Max(*ptr + brightness, 0), 255);
						ptr[1] = (byte)Math.Min(Math.Max(ptr[1] + brightness, 0), 255);
						ptr[2] = (byte)Math.Min(Math.Max(ptr[2] + brightness, 0), 255);
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void ChangeBrightness(this Bitmap bitmap, int brightness)
		{
			bitmap.ChangeBrightness(Rectangle.Empty, brightness);
		}

		public unsafe static void ChangeContrast(this Bitmap bitmap, Rectangle clipRectangle, int contrast)
		{
			if (contrast < -100 || contrast > 100)
			{
				throw new ArgumentException("Must be in the range +/- 100");
			}
			float c = (100f + (float)contrast) / 100f;
			c *= c;
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					for (byte* ptr2 = ptr + clipScanLen; ptr < ptr2; ptr += pixelWidth)
					{
						*ptr = (byte)Math.Min(Math.Max((float)(*ptr - 128) * c + 128f, 0f), 255f);
						ptr[1] = (byte)Math.Min(Math.Max((float)(ptr[1] - 128) * c + 128f, 0f), 255f);
						ptr[2] = (byte)Math.Min(Math.Max((float)(ptr[2] - 128) * c + 128f, 0f), 255f);
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void ChangeContrast(this Bitmap bitmap, int contrast)
		{
			bitmap.ChangeContrast(Rectangle.Empty, contrast);
		}

		public unsafe static void ChangeGamma(this Bitmap bitmap, Rectangle clipRectangle, double red, double green, double blue)
		{
			CheckFormat(bitmap);
			red = red.Clamp(0.2, 5.0);
			green = green.Clamp(0.2, 5.0);
			blue = blue.Clamp(0.2, 5.0);
			byte[] redGamma = new byte[256];
			byte[] greenGamma = new byte[256];
			byte[] blueGamma = new byte[256];
			for (int i = 0; i < 256; i++)
			{
				redGamma[i] = (byte)Math.Min(255.0, 255.0 * Math.Pow((float)i / 255f, 1.0 / red) + 0.5);
				greenGamma[i] = (byte)Math.Min(255.0, 255.0 * Math.Pow((float)i / 255f, 1.0 / green) + 0.5);
				blueGamma[i] = (byte)Math.Min(255.0, 255.0 * Math.Pow((float)i / 255f, 1.0 / blue) + 0.5);
			}
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					for (byte* ptr2 = ptr + clipScanLen; ptr < ptr2; ptr += pixelWidth)
					{
						*ptr = blueGamma[*ptr];
						ptr[1] = greenGamma[ptr[1]];
						ptr[2] = redGamma[ptr[2]];
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void ChangeGamma(this Bitmap bitmap, double red, double green, double blue)
		{
			bitmap.ChangeGamma(Rectangle.Empty, red, green, blue);
		}

		public static void ChangeGamma(this Bitmap bitmap, float grayGamma)
		{
			bitmap.ChangeGamma(Rectangle.Empty, grayGamma, grayGamma, grayGamma);
		}

		public unsafe static void Colorize(this Bitmap bitmap, Rectangle clipRectangle, int red, int green, int blue, PixelOperation operation = PixelOperation.Multiply)
		{
			CheckFormat(bitmap);
			if (red < -255 || red > 255 || green < -255 || green > 255 || blue < -255 || blue > 255)
			{
				throw new ArgumentException("values must be in the range +/- 255");
			}
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					byte* ptr2 = ptr + clipScanLen;
					switch (operation)
					{
					case PixelOperation.Add:
						for (; ptr < ptr2; ptr += pixelWidth)
						{
							*ptr = (byte)Math.Min(Math.Max(*ptr + blue, 0), 255);
							ptr[1] = (byte)Math.Min(Math.Max(ptr[1] + green, 0), 255);
							ptr[2] = (byte)Math.Min(Math.Max(ptr[2] + red, 0), 255);
						}
						return;
					case PixelOperation.Replace:
						for (; ptr < ptr2; ptr += pixelWidth)
						{
							*ptr = (byte)blue;
							ptr[1] = (byte)green;
							ptr[2] = (byte)red;
						}
						return;
					}
					for (; ptr < ptr2; ptr += pixelWidth)
					{
						*ptr = (byte)Math.Min(Math.Max(*ptr * blue / 256, 0), 255);
						ptr[1] = (byte)Math.Min(Math.Max(ptr[1] * green / 256, 0), 255);
						ptr[2] = (byte)Math.Min(Math.Max(ptr[2] * red / 256, 0), 255);
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void Colorize(this Bitmap bitmap, int red, int green, int blue)
		{
			bitmap.Colorize(Rectangle.Empty, red, green, blue);
		}

		public static void Colorize(this Bitmap bitmap, Color color)
		{
			bitmap.Colorize(color.R, color.G, color.B);
		}

		public unsafe static void ApplyColorMatrix(this Bitmap bitmap, Rectangle clipRectangle, ColorMatrix matrix)
		{
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			float m0 = matrix[0, 0];
			float m = matrix[0, 1];
			float m2 = matrix[0, 2];
			float m3 = matrix[0, 3];
			float m4 = matrix[1, 0];
			float m5 = matrix[1, 1];
			float m6 = matrix[1, 2];
			float m7 = matrix[1, 3];
			float m8 = matrix[2, 0];
			float m9 = matrix[2, 1];
			float m10 = matrix[2, 2];
			float m11 = matrix[2, 3];
			float m12 = matrix[3, 0];
			float m13 = matrix[3, 1];
			float m14 = matrix[3, 2];
			float m15 = matrix[3, 3];
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					byte* ptr2 = ptr + clipScanLen;
					if (pixelWidth == 4)
					{
						for (; ptr < ptr2; ptr += pixelWidth)
						{
							float num = (int)(*ptr);
							float num2 = (int)ptr[1];
							float num3 = (int)ptr[2];
							float num4 = (int)ptr[3];
							ptr[2] = (byte)Math.Max(Math.Min(255f, num3 * m0 + num2 * m4 + num * m8 + num4 * m12), 0f);
							ptr[1] = (byte)Math.Max(Math.Min(255f, num3 * m + num2 * m5 + num * m9 + num4 * m13), 0f);
							*ptr = (byte)Math.Max(Math.Min(255f, num3 * m2 + num2 * m6 + num * m10 + num4 * m14), 0f);
							ptr[3] = (byte)Math.Max(Math.Min(255f, num3 * m3 + num2 * m7 + num * m11 + num4 * m15), 0f);
						}
					}
					else
					{
						for (; ptr < ptr2; ptr += pixelWidth)
						{
							float num5 = (int)(*ptr);
							float num6 = (int)ptr[1];
							float num7 = (int)ptr[2];
							ptr[2] = (byte)Math.Max(Math.Min(255f, num7 * m0 + num6 * m4 + num5 * m8 + m12), 0f);
							ptr[1] = (byte)Math.Max(Math.Min(255f, num7 * m + num6 * m5 + num5 * m9 + m13), 0f);
							*ptr = (byte)Math.Max(Math.Min(255f, num7 * m2 + num6 * m6 + num5 * m10 + m14), 0f);
						}
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
		}

		public static void ApplyColorMatrix(this Bitmap bitmap, ColorMatrix matrix)
		{
			bitmap.ApplyColorMatrix(Rectangle.Empty, matrix);
		}

		public unsafe static void Convolute(this Bitmap bitmap, Rectangle clipRectangle, ConvolutionMatrix m)
		{
			if (m.Divisor == 0)
			{
				throw new ArgumentException("factor of matrix can not be 0");
			}
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return;
			}
			using (Bitmap bitmap2 = bitmap.Clone(clipRectangle, bitmap.PixelFormat))
			{
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
				BitmapData bitmapData2 = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadWrite, bitmap2.PixelFormat);
				try
				{
					IntPtr scan = bitmapData.Scan0;
					IntPtr scan2 = bitmapData2.Scan0;
					int sourceStride = bitmapData2.Stride;
					int sourceStride2 = sourceStride * 2;
					int targetStride = bitmapData.Stride;
					int pixelWidth = sourceStride / bitmap.Width;
					int pixelWidth2 = 2 * pixelWidth;
					int num = bitmap.Width - 2;
					int toExclusive = bitmap.Height - 2;
					int clipScanLen = pixelWidth * num;
					int mtl = m.TopLeft;
					int mtm = m.TopMid;
					int mtr = m.TopRight;
					int mml = m.MidLeft;
					int mp = m.Pixel;
					int mmr = m.MidRight;
					int mbl = m.BottomLeft;
					int mbm = m.BottomMid;
					int mbr = m.BottomRight;
					byte* porg = (byte*)(void*)scan + clipRectangle.Top * targetStride + clipRectangle.Left * pixelWidth;
					byte* pSrcOrg = (byte*)(void*)scan2;
					Parallel.For(0, toExclusive, delegate(int h)
					{
						byte* ptr = porg + h * targetStride;
						byte* ptr2 = pSrcOrg + h * sourceStride;
						byte* ptr3 = ptr + clipScanLen;
						while (ptr < ptr3)
						{
							ptr[pixelWidth + 2 + targetStride] = (byte)Math.Min(Math.Max((ptr2[2] * mtl + ptr2[pixelWidth + 2] * mtm + ptr2[pixelWidth2 + 2] * mtr + ptr2[2 + sourceStride] * mml + ptr2[pixelWidth + 2 + sourceStride] * mp + ptr2[pixelWidth2 + 2 + sourceStride] * mmr + ptr2[2 + sourceStride2] * mbl + ptr2[pixelWidth + 2 + sourceStride2] * mbm + ptr2[pixelWidth2 + 2 + sourceStride2] * mbr) / m.Divisor + m.Offset, 0), 255);
							ptr[pixelWidth + 1 + targetStride] = (byte)Math.Min(Math.Max((ptr2[1] * mtl + ptr2[pixelWidth + 1] * mtm + ptr2[pixelWidth2 + 1] * mtr + ptr2[1 + sourceStride] * mml + ptr2[pixelWidth + 1 + sourceStride] * mp + ptr2[pixelWidth2 + 1 + sourceStride] * mmr + ptr2[1 + sourceStride2] * mbl + ptr2[pixelWidth + 1 + sourceStride2] * mbm + ptr2[pixelWidth2 + 1 + sourceStride2] * mbr) / m.Divisor + m.Offset, 0), 255);
							ptr[pixelWidth + targetStride] = (byte)Math.Min(Math.Max((*ptr2 * mtl + ptr2[pixelWidth] * mtm + ptr2[pixelWidth2] * mtr + ptr2[0 + sourceStride] * mml + ptr2[pixelWidth + sourceStride] * mp + ptr2[pixelWidth2 + sourceStride] * mmr + ptr2[0 + sourceStride2] * mbl + ptr2[pixelWidth + sourceStride2] * mbm + ptr2[pixelWidth2 + sourceStride2] * mbr) / m.Divisor + m.Offset, 0), 255);
							ptr += pixelWidth;
							ptr2 += pixelWidth;
						}
					});
				}
				finally
				{
					bitmap.UnlockBits(bitmapData);
					bitmap2.UnlockBits(bitmapData2);
				}
			}
		}

		public static void Convolute(this Bitmap bitmap, ConvolutionMatrix m)
		{
			bitmap.Convolute(Rectangle.Empty, m);
		}

		public static void Smooth(this Bitmap bitmap, Rectangle clipRectangle, int weight)
		{
			bitmap.Convolute(clipRectangle, new ConvolutionMatrix(1)
			{
				Pixel = weight,
				Divisor = weight + 8
			});
		}

		public static void Smooth(this Bitmap bitmap, int weight)
		{
			bitmap.Smooth(Rectangle.Empty, weight);
		}

		public static void Smooth(this Bitmap bitmap, Rectangle clipRectangle)
		{
			bitmap.Smooth(clipRectangle, 1);
		}

		public static void Smooth(this Bitmap bitmap)
		{
			bitmap.Smooth(Rectangle.Empty);
		}

		public static void GaussianBlur(this Bitmap bitmap, Rectangle clipRectangle, int weight)
		{
			ConvolutionMatrix convolutionMatrix = new ConvolutionMatrix(1);
			convolutionMatrix.Pixel = weight;
			ConvolutionMatrix m = convolutionMatrix;
			int num2 = (m.BottomMid = 2);
			int num4 = (m.MidRight = num2);
			int num7 = (m.TopMid = (m.MidLeft = num4));
			m.Divisor = weight + 12;
			bitmap.Convolute(clipRectangle, m);
		}

		public static void GaussianBlur(this Bitmap bitmap, int weight)
		{
			bitmap.GaussianBlur(Rectangle.Empty, weight);
		}

		public static void GaussianBlur(this Bitmap bitmap, Rectangle clipRectangle)
		{
			bitmap.GaussianBlur(clipRectangle, 4);
		}

		public static void GaussianBlur(this Bitmap bitmap)
		{
			bitmap.GaussianBlur(Rectangle.Empty);
		}

		public static void MeanRemoval(this Bitmap bitmap, Rectangle clipRectangle, int weight)
		{
			bitmap.Convolute(clipRectangle, new ConvolutionMatrix(-1)
			{
				Pixel = weight,
				Divisor = weight - 8
			});
		}

		public static void MeanRemoval(this Bitmap bitmap, int weight)
		{
			bitmap.MeanRemoval(Rectangle.Empty, weight);
		}

		public static void MeanRemoval(this Bitmap bitmap, Rectangle clipRectangle)
		{
			bitmap.MeanRemoval(clipRectangle, 9);
		}

		public static void MeanRemoval(this Bitmap bitmap)
		{
			bitmap.MeanRemoval(Rectangle.Empty);
		}

		public static void Sharpen(this Bitmap bitmap, Rectangle clipRectangle, int a, int b)
		{
			ConvolutionMatrix convolutionMatrix = new ConvolutionMatrix(0);
			convolutionMatrix.Pixel = a;
			ConvolutionMatrix m = convolutionMatrix;
			int num2 = (m.BottomMid = -b);
			int num4 = (m.MidRight = num2);
			int num7 = (m.TopMid = (m.MidLeft = num4));
			m.Divisor = a + 4 * -b;
			if (m.Divisor == 0)
			{
				m.Divisor = 1;
			}
			bitmap.Convolute(Rectangle.Empty, m);
		}

		public static void Sharpen(this Bitmap bitmap, int a, int b)
		{
			bitmap.Sharpen(Rectangle.Empty, a, b);
		}

		public static void Sharpen(this Bitmap bitmap, Rectangle clipRectangle)
		{
			bitmap.Sharpen(5, -1);
		}

		public static void Sharpen(this Bitmap bitmap)
		{
			bitmap.Sharpen(Rectangle.Empty);
		}

		public static void EmbossLaplacian(this Bitmap b, Rectangle clipRectangle)
		{
			ConvolutionMatrix m = new ConvolutionMatrix(-1);
			int num2 = (m.BottomMid = 0);
			int num4 = (m.MidRight = num2);
			int num7 = (m.TopMid = (m.MidLeft = num4));
			m.Pixel = 4;
			m.Offset = 127;
			b.Convolute(clipRectangle, m);
		}

		public static void EmbossLaplacian(this Bitmap b)
		{
			b.EmbossLaplacian(Rectangle.Empty);
		}

		public static void EdgeDetectQuick(this Bitmap bitmap, Rectangle clipRectangle)
		{
			ConvolutionMatrix m = default(ConvolutionMatrix);
			int num2 = (m.TopRight = -1);
			int num5 = (m.TopLeft = (m.TopMid = num2));
			num2 = (m.MidRight = 0);
			num5 = (m.MidLeft = (m.Pixel = num2));
			num2 = (m.BottomRight = 1);
			num5 = (m.BottomLeft = (m.BottomMid = num2));
			m.Offset = 127;
			bitmap.Convolute(clipRectangle, m);
		}

		public static void EdgeDetectQuick(this Bitmap bitmap)
		{
			bitmap.EdgeDetectQuick(Rectangle.Empty);
		}

		public unsafe static Bitmap ResizeFast(Bitmap source, int newWidth, int newHeight, PixelFormat format, ResizeFastInterpolation method)
		{
			Bitmap bitmap = source;

			//If format requested is anything other than 24 or 32 bit. Force format to be 32bit
			if (format != PixelFormat.Format32bppArgb && format != PixelFormat.Format24bppRgb)
				format = PixelFormat.Format32bppArgb;

            // get source image size
            int width = bitmap.Width;
			int height = bitmap.Height;

			//No need to resize, return a copy of the same image
			if (newWidth == width && newHeight == height)
				return bitmap.CreateCopy(format);

			//IF source image isn't 24bit or 32bit force create a copy with color
			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb && bitmap.PixelFormat != PixelFormat.Format24bppRgb)
				bitmap = bitmap.CreateCopy(PixelFormat.Format32bppArgb);

			BitmapData srcData = null;
			BitmapData dstData = null;
			Bitmap dstImage = null;

			try
			{
                // Reference: https://github.com/andrewkirillov/AForge.NET/blob/master/Sources/Imaging/Filters/Base%20classes/BaseTransformationFilter.cs#L61
				// Also: https://github.com/andrewkirillov/AForge.NET/blob/master/Sources/Imaging/Filters/Base%20classes/BaseTransformationFilter.cs#L103

                // lock source bitmap data
                srcData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				// create new image of required format
				dstImage = new Bitmap(newWidth, newHeight, format);
				// lock destination bitmap data
				dstData = dstImage.LockBits(new Rectangle(0, 0, newWidth, newHeight), ImageLockMode.ReadWrite, dstImage.PixelFormat); 

				int srcStride = srcData.Stride;
                int srcPixelSize = srcStride / width;
                //int srcOffset = srcStride - srcPixelSize * width;

                int dstStride = dstData.Stride;
				int dstPixelSize = dstStride / newWidth;
				//int dstOffset = dstStride - srcPixelSize * newWidth;

				int minPixelSize = Math.Min(srcPixelSize, dstPixelSize);
                int dstExtra = dstPixelSize - minPixelSize;
                float xFactor = (float)width / (float)newWidth;
				float yFactor = (float)height / (float)newHeight;

                // do the job
                byte* orgsrc = (byte*)srcData.Scan0.ToPointer();
				byte* orgdst = (byte*)dstData.Scan0.ToPointer();

				//Image has 4 bytesPerPixel (32 bit), so Add Alpha Channel
				if (dstPixelSize == 4)
					InitializeAlpha32(orgdst, newWidth, newHeight, dstStride);

				switch (method)
				{
						case ResizeFastInterpolation.NearestNeighbor:
                        // for each line
                        for (int y = 0; y < newHeight; y++)
                        {
							//Ref: https://github.com/andrewkirillov/AForge.NET/blob/master/Sources/Imaging/Filters/Transform/ResizeNearestNeighbor.cs#L81
							byte* dst = orgdst + dstStride * y;
							byte* src = orgsrc + srcStride * ((int)(y * yFactor));

							// for each pixel
							for (int x = 0; x < newWidth; x++)
                            {
                                byte* p = src + srcPixelSize * ((int)(x * xFactor));

                                for (int i = 0; i < minPixelSize; i++, dst++, p++)
                                {
                                    *dst = *p;
                                }
                                dst += dstExtra;
                            }
                        };
						break;
						case ResizeFastInterpolation.Bilinear:
                        {
							// Ref: https://github.com/andrewkirillov/AForge.NET/blob/master/Sources/Imaging/Filters/Transform/ResizeBilinear.cs#L78
                            // width and height decreased by 1
                            int ymax = height - 1;
                            int xmax = width - 1;

                            // for each line
                            for (int y = 0; y < newHeight; y++)
                            {
                                byte* dst = orgdst + y * dstStride;

                                // Y coordinates
                                float oy = (float)y * yFactor;
                                int oy1 = (int)oy;
                                int oy2 = (oy1 == ymax) ? oy1 : (oy1 + 1);
                                float dy1 = oy - (float)oy1;
                                float dy2 = 1f - dy1;

                                // get temp pointers
                                byte* tp1 = orgsrc + oy1 * srcStride;
                                byte* tp2 = orgsrc + oy2 * srcStride;

                                // for each pixel
                                for (int x = 0; x < newWidth; x++)
                                {
                                    // X coordinates
                                    float ox = (float)x * xFactor;
                                    int ox1 = (int)ox;
                                    int ox2 = (ox1 == xmax) ? ox1 : (ox1 + 1);
                                    float dx1 = ox - (float)ox1;
                                    float dx2 = 1f - dx1;

                                    // get four points
                                    byte* p1 = tp1 + ox1 * srcPixelSize;
                                    byte* p2 = tp1 + ox2 * srcPixelSize;
                                    byte* p3 = tp2 + ox1 * srcPixelSize;
                                    byte* p4 = tp2 + ox2 * srcPixelSize;

                                    for (int i = 0; i < minPixelSize; i++, dst++, p1++, p2++, p3++, p4++)
                                    {
                                        *dst = (byte)(
											dy2 * (dx2 * (*p1) + dx1 * (*p2)) +
											dy1 * (dx2 * (*p3) + dx1 * (*p4)));
                                    }
                                    dst += dstExtra;
                                }
                            };
                            break;
                        }
						case ResizeFastInterpolation.Bicubic:
                        {
							// Ref: https://github.com/andrewkirillov/AForge.NET/blob/master/Sources/Imaging/Filters/Transform/ResizeBicubic.cs#L79
                            // width and height decreased by 1
                            int ymax = height - 1;
                            int xmax = width - 1;

                            for (int y = 0; y < newHeight; y++)
                            {
                                byte* dst = orgdst + y * dstStride;

                                // Y coordinates
                                float oy = (float)y * yFactor - 0.5f;
                                int oy1 = (int)oy;
                                float dy = oy - (float)oy1;
                                float[] array = new float[4];

                                for (int x = 0; x < newWidth; x++)
                                {
                                    // X coordinates
                                    float ox = (float)x * xFactor - 0.5f;
                                    int ox1 = (int)ox;
                                    float dx = ox - (float)ox1;

                                    // initial pixel value
                                    for (int l = 0; l < minPixelSize; l++)
                                    {
                                        array[l] = 0f;
                                    }

                                    for (int n = -1; n < 3; n++)
                                    {
                                        // get Y coefficient
                                        float k1 = BiCubicKernel(dy - (float)n);

                                        int oy2 = oy1 + n;
                                        if (oy2 < 0)
                                            oy2 = 0;
                                        if (oy2 > ymax)
                                            oy2 = ymax;

                                        for (int m = -1; m < 3; m++)
                                        {
                                            // get X coefficient
                                            float k2 = k1 * BiCubicKernel((float)m - dx);

                                            int ox2 = ox1 + m;
                                            if (ox2 < 0)
                                                ox2 = 0;
                                            if (ox2 > xmax)
                                                ox2 = xmax;

                                            // temporary pointer
                                            byte* p = orgsrc + oy2 * srcStride + ox2 * srcPixelSize;
                                            for (int i = 0; i < minPixelSize; i++)
                                            {
                                                array[i] += k2 * (float)(int)p[i];
                                            }
                                        }
                                    }
                                    for (int i = 0; i < minPixelSize; i++)
                                    {
                                        byte* intPtr = dst + i;
                                        *intPtr = (byte)(*intPtr + (byte)array[i]);
                                    }
                                    dst += dstPixelSize;
                                }
                            };
                            break;
                        }
                }
				return dstImage;
			}
            catch
            {
                return null;
            }
            finally
			{
				if (dstData != null)
				{
                    // unlock destination image
                    dstImage.UnlockBits(dstData);
				}
				if (srcData != null)
				{
                    // unlock source image
                    bitmap.UnlockBits(srcData);
				}
				if (bitmap != source)
				{
					bitmap.Dispose();
				}
			}
		}

		private unsafe static void InitializeAlpha32(byte* scan, int width, int height, int strideLen)
		{
			for (int i = 0; i < height; i++)
			{
				uint* ptr = (uint*)(scan + i * strideLen);
				for (int j = 0; j < width; j++)
				{
					ptr[j] = 4278190080u;
				}
			}
		}

		private static float BiCubicKernel(float x)
		{
			if (x > 2f)
			{
				return 0f;
			}
			float num = x - 1f;
			float num2 = x + 1f;
			float num3 = x + 2f;
			float num4 = ((num3 <= 0f) ? 0f : (num3 * num3 * num3));
			float num5 = ((num2 <= 0f) ? 0f : (num2 * num2 * num2));
			float num6 = ((x <= 0f) ? 0f : (x * x * x));
			float num7 = ((num <= 0f) ? 0f : (num * num * num));
			return 355f / (678f * (float)Math.PI) * (num4 - 4f * num5 + 6f * num6 - 4f * num7);
		}

		public unsafe static Bitmap ResizeBiliniearHQ(Bitmap srcImg, int targetWidth, int targetHeight, PixelFormat targetFormat)
		{
			Bitmap bitmap = srcImg;
			int height = bitmap.Height;
			int width = bitmap.Width;
			if (targetFormat != PixelFormat.Format32bppArgb && targetFormat != PixelFormat.Format24bppRgb)
			{
				targetFormat = PixelFormat.Format32bppArgb;
			}
			if (targetWidth == width && targetHeight == height)
			{
				return srcImg.CreateCopy(targetFormat);
			}
			if (bitmap.PixelFormat != PixelFormat.Format32bppArgb && bitmap.PixelFormat != PixelFormat.Format24bppRgb)
			{
				bitmap = bitmap.CreateCopy(PixelFormat.Format32bppArgb);
			}
			Bitmap bitmap2 = new Bitmap(targetWidth, targetHeight, targetFormat);
			float num = ((width > targetWidth) ? ((float)width / (float)targetWidth) : ((float)(width - 1) / (float)targetWidth));
			float num2 = ((height > targetHeight) ? ((float)height / (float)targetHeight) : ((float)(height - 1) / (float)targetHeight));
			BitmapData bitmapData = null;
			BitmapData bitmapData2 = null;
			try
			{
				bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				int stride = bitmapData.Stride;
				int pixelWidth = stride / width;
				bitmapData2 = bitmap2.LockBits(new Rectangle(0, 0, targetWidth, targetHeight), ImageLockMode.ReadWrite, bitmap2.PixelFormat);
				int stride2 = bitmapData2.Stride;
				int num3 = stride2 / targetWidth;
				int num4 = targetWidth * num3;
				float num5 = 1f / num;
				RGBA[] array = new RGBA[width];
				try
				{
					fixed (RGBA* ptr4 = array)
					{
						float num6 = 0f;
						byte* ptr = (byte*)(void*)bitmapData.Scan0;
						byte* ptr2 = (byte*)(void*)bitmapData2.Scan0;
						if (num3 == 4)
						{
							InitializeAlpha32(ptr2, targetWidth, targetHeight, stride2);
						}
						int num7 = 0;
						while (num7 < targetHeight)
						{
							float num8 = num6 - (float)(int)num6;
							float num10;
							if (num2 >= 1f)
							{
								float num9 = num6 + num2;
								byte* ptr3 = ptr + (int)num6 * stride;
								ClearSpan(ptr4, width);
								AddSpan(ptr4, ptr3, width, pixelWidth, 1f - num8);
								for (int i = (int)num6; i < (int)num9 - 1; i++)
								{
									ptr3 += stride;
									AddSpan(ptr4, ptr3, width, pixelWidth, 1f);
								}
								AddSpan(ptr4, ptr3, width, pixelWidth, num9 - (float)(int)num9);
								num10 = 1f / num2;
							}
							else
							{
								byte* ptr5 = ptr + (int)num6 * stride;
								byte* psrc = ptr5 + stride;
								InterpolateSpan(ptr4, ptr5, psrc, width, pixelWidth, num8);
								num10 = 1f;
							}
							byte* ptr6 = ptr2 + num7 * stride2;
							byte* ptr7 = ptr6 + num4;
							float num11 = 0f;
							if (num >= 1f)
							{
								RGBA* ptr8 = ptr4;
								int num12 = 0;
								float num13 = num5 * num10;
								while (ptr6 < ptr7)
								{
									int j = num12 + 1;
									float num14 = (float)j - num11;
									num11 += num;
									num12 = (int)num11;
									float num15 = num11 - (float)num12;
									float num16 = ptr8->R * num14;
									float num17 = ptr8->G * num14;
									float num18 = ptr8->B * num14;
									for (; j < num12; j++)
									{
										ptr8++;
										num16 += ptr8->R;
										num17 += ptr8->G;
										num18 += ptr8->B;
									}
									num16 += ptr8->R * num15;
									num17 += ptr8->G * num15;
									num18 += ptr8->B * num15;
									*ptr6 = (byte)(num16 * num13);
									ptr6[1] = (byte)(num17 * num13);
									ptr6[2] = (byte)(num18 * num13);
									ptr6 += num3;
									ptr8++;
								}
							}
							else
							{
								while (ptr6 < ptr7)
								{
									float num19 = num11 - (float)(int)num11;
									RGBA* ptr9 = ptr4 + (int)num11;
									RGBA* ptr10 = ptr9 + 1;
									*ptr6 = (byte)((ptr9->R + (ptr10->R - ptr9->R) * num19) * num10);
									ptr6[1] = (byte)((ptr9->G + (ptr10->G - ptr9->G) * num19) * num10);
									ptr6[2] = (byte)((ptr9->B + (ptr10->B - ptr9->B) * num19) * num10);
									ptr6 += num3;
									num11 += num;
								}
							}
							num7++;
							num6 += num2;
						}
					}
				}
				finally
				{
				}
				return bitmap2;
			}
            catch
            {
                return null;
            }
            finally
			{
				if (bitmapData != null)
				{
					bitmap.UnlockBits(bitmapData);
				}
				if (bitmapData2 != null)
				{
					bitmap2.UnlockBits(bitmapData2);
				}
				if (bitmap != null && bitmap != srcImg)
				{
					bitmap.Dispose();
				}
			}
		}

		private unsafe static void ClearSpan(RGBA* pspan, int width)
		{
			int num = 0;
			while (num < width)
			{
				pspan->R = (pspan->G = (pspan->B = 0f));
				num++;
				pspan++;
			}
		}

		private unsafe static void AddSpan(RGBA* pspan, byte* psrc, int width, int pixelWidth, float fact)
		{
			if (fact == 0f)
			{
				return;
			}
			if (fact == 1f)
			{
				int num = 0;
				while (num < width)
				{
					pspan->R += (int)(*psrc);
					pspan->G += (int)psrc[1];
					pspan->B += (int)psrc[2];
					num++;
					psrc += pixelWidth;
					pspan++;
				}
			}
			else
			{
				int num2 = 0;
				while (num2 < width)
				{
					pspan->R += (float)(int)(*psrc) * fact;
					pspan->G += (float)(int)psrc[1] * fact;
					pspan->B += (float)(int)psrc[2] * fact;
					num2++;
					psrc += pixelWidth;
					pspan++;
				}
			}
		}

		private unsafe static void InterpolateSpan(RGBA* pspan, byte* psrc, byte* psrc2, int width, int pixelWidth, float fact)
		{
			if (fact == 0f)
			{
				int num = 0;
				while (num < width)
				{
					pspan->R = (int)(*psrc);
					pspan->G = (int)psrc[1];
					pspan->B = (int)psrc[2];
					num++;
					psrc += pixelWidth;
					pspan++;
				}
				return;
			}
			int num2 = 0;
			while (num2 < width)
			{
				pspan->R = (float)(int)(*psrc) + (float)(*psrc2 - *psrc) * fact;
				pspan->G = (float)(int)psrc[1] + (float)(psrc2[1] - psrc[1]) * fact;
				pspan->B = (float)(int)psrc[2] + (float)(psrc2[2] - psrc[2]) * fact;
				num2++;
				psrc += pixelWidth;
				psrc2 += pixelWidth;
				pspan++;
			}
		}

		public static Bitmap ResizeGdi(Bitmap bitmap, int width, int height, PixelFormat pixelFormat, bool highQuality = false)
		{
			Rectangle rectangle = new Rectangle(0, 0, width, height);
			if (rectangle.IsEmpty())
			{
				return null;
			}
			Bitmap bitmap2 = new Bitmap(width, height, pixelFormat);
			try
			{
				using (Graphics graphics = Graphics.FromImage(bitmap2))
				{
					using (graphics.HighQuality(enabled: true, rectangle.Size, bitmap2.Size, highQuality))
					{
						graphics.DrawImage(bitmap, rectangle, 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
					}
				}
				return bitmap2;
			}
			catch
			{
				bitmap2.SafeDispose();
				return null;
			}
		}

		public unsafe static Histogram GetHistogram(this Bitmap bitmap, Rectangle clipRectangle)
		{
			CheckFormat(bitmap);
			Rectangle rectangle = bitmap.Size.ToRectangle();
			if (clipRectangle.IsEmpty)
			{
				clipRectangle = rectangle;
			}
			else
			{
				clipRectangle.Intersect(rectangle);
			}
			if (clipRectangle.Width == 0 || clipRectangle.Height == 0)
			{
				return Histogram.Empty;
			}
			int[] reds = new int[256];
			int[] greens = new int[256];
			int[] blues = new int[256];
			int[] grays = new int[256];
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
			try
			{
				int pixelWidth = bitmapData.Stride / bitmap.Width;
				int stride = bitmapData.Stride;
				byte* pbase = (byte*)(void*)bitmapData.Scan0 + clipRectangle.Top * stride + clipRectangle.Left * pixelWidth;
				int clipScanLen = clipRectangle.Width * pixelWidth;
				Parallel.For(0, clipRectangle.Height, delegate(int h)
				{
					byte* ptr = pbase + stride * h;
					for (byte* ptr2 = ptr + clipScanLen; ptr < ptr2; ptr += pixelWidth)
					{
						int num = *ptr;
						int num2 = ptr[1];
						int num3 = ptr[2];
						reds[num3]++;
						greens[num2]++;
						blues[num]++;
						int num4 = (int)((float)num3 * grayRed + (float)num2 * grayGreen + (float)num * grayBlue);
						grays[num4]++;
					}
				});
			}
			finally
			{
				bitmap.UnlockBits(bitmapData);
			}
			return new Histogram(reds, greens, blues, grays, clipRectangle.Width * clipRectangle.Height);
		}

		public static Histogram GetHistogram(this Bitmap bitmap)
		{
			return bitmap.GetHistogram(Rectangle.Empty);
		}

		public static Matrix CreateColorScaleMatrix(float scale, float offset)
		{
			Matrix matrix = new Matrix(5, 5, 1.0);
			double num2 = (matrix[2, 2] = scale);
			double num5 = (matrix[0, 0] = (matrix[1, 1] = num2));
			num2 = (matrix[3, 2] = offset);
			num5 = (matrix[3, 0] = (matrix[3, 1] = num2));
			return matrix;
		}

		public static Matrix CreateColorSaturationMatrix(float sat)
		{
			Matrix matrix = new Matrix(5, 5, 1.0);
			matrix[0, 0] = (1f - sat) * grayRed + sat;
			double num3 = (matrix[0, 1] = (matrix[0, 2] = (1f - sat) * grayRed));
			matrix[1, 1] = (1f - sat) * grayGreen + sat;
			num3 = (matrix[1, 0] = (matrix[1, 2] = (1f - sat) * grayGreen));
			matrix[2, 2] = (1f - sat) * grayBlue + sat;
			num3 = (matrix[2, 0] = (matrix[2, 1] = (1f - sat) * grayBlue));
			return matrix;
		}

		public static Matrix CreateColorWhitePointMatrix(Color whitePoint)
		{
			try
			{
				Matrix matrix = new Matrix(5, 5, 1.0);
				byte r = whitePoint.R;
				byte g = whitePoint.G;
				byte b = whitePoint.B;
				byte b2 = (byte)(grayRed * (float)(int)r + grayGreen * (float)(int)g + grayBlue * (float)(int)b);
				matrix[3, 0] = (float)(b2 - r) / 256f;
				matrix[3, 1] = (float)(b2 - g) / 256f;
				matrix[3, 2] = (float)(b2 - b) / 256f;
				matrix[0, 0] = 1.0 / (1.0 - matrix[3, 0]);
				matrix[1, 1] = 1.0 / (1.0 - matrix[3, 1]);
				matrix[2, 2] = 1.0 / (1.0 - matrix[3, 1]);
				return matrix;
			}
			catch
			{
				Matrix matrix2 = new Matrix(5, 5, 1.0);
				double num2 = (matrix2[2, 2] = 1.0);
				double num5 = (matrix2[0, 0] = (matrix2[1, 1] = num2));
				return matrix2;
			}
		}

		public static ColorMatrix CreateColorMatrix(float blackLevel, float whiteLevel, float contrast, float brightness, float saturation, Color whitePointColor)
		{
			Matrix matrix = CreateColorScaleMatrix((contrast + 1f) / (whiteLevel - blackLevel), brightness - blackLevel) * CreateColorSaturationMatrix(saturation + 1f) * CreateColorWhitePointMatrix(whitePointColor);
			double num2 = (matrix[4, 2] = 0.0010000000474974513);
			double num5 = (matrix[4, 0] = (matrix[4, 1] = num2));
			float[][] array = new float[5][];
			for (int i = 0; i < 5; i++)
			{
				array[i] = new float[5];
				for (int j = 0; j < 5; j++)
				{
					array[i][j] = (float)matrix[i, j];
				}
			}
			return new ColorMatrix(array);
		}

		public static void GetBlackWhitePoint(this Bitmap bitmap, out float bp, out float wp)
		{
			try
			{
				Histogram histogram = bitmap.GetHistogram();
				bp = histogram.GetBlackPointNormalized();
				wp = histogram.GetWhitePointNormalized();
			}
			catch
			{
				wp = 1f;
				bp = 0f;
			}
		}

		public static void ApplyAdjustment(this Bitmap bitmap, Rectangle clipRectangle, BitmapAdjustment adjustment)
		{
			CheckFormat(bitmap);
			float wp = 1f;
			float bp = 0f;
			if (adjustment.HasAutoContrast)
			{
				bitmap.GetBlackWhitePoint(out bp, out wp);
			}
			if (adjustment.HasColorTransformations || wp < 0.95f || bp > 0.05f)
			{
				bitmap.ApplyColorMatrix(clipRectangle, CreateColorMatrix(bp, wp, adjustment.Contrast, adjustment.Brightness, adjustment.Saturation, adjustment.WhitePointColor));
			}
			if (adjustment.HasGamma)
			{
				bitmap.ChangeGamma(1f + adjustment.Gamma);
			}
			if (adjustment.Sharpen != 0)
			{
				bitmap.Sharpen(clipRectangle, (4 - adjustment.Sharpen) * 5, 1);
			}
		}

		public static void ApplyAdjustment(this Bitmap bitmap, BitmapAdjustment adjustment)
		{
			bitmap.ApplyAdjustment(Rectangle.Empty, adjustment);
		}

		public static Bitmap CreateAdjustedBitmap(this Bitmap bitmap, BitmapAdjustment adjustment, PixelFormat pixelFormat, bool alwaysClone)
		{
			if (bitmap == null)
			{
				return null;
			}
			float wp = 1f;
			float bp = 0f;
			if (adjustment.HasAutoContrast)
			{
				bitmap.GetBlackWhitePoint(out bp, out wp);
			}
			if (!alwaysClone && !adjustment.HasColorTransformations && wp >= 0.95f && bp <= 0.05f && !adjustment.HasSharpening && !adjustment.HasGamma)
			{
				return bitmap;
			}
			Bitmap bitmap2 = null;
			try
			{
				if (!alwaysClone && !adjustment.HasColorTransformations && wp >= 0.95f && bp <= 0.05f)
				{
					bitmap2 = bitmap;
				}
				else
				{
					bitmap2 = bitmap.Copy(pixelFormat);
					bitmap2.ApplyColorMatrix(CreateColorMatrix(bp, wp, adjustment.Contrast, adjustment.Brightness, adjustment.Saturation, adjustment.WhitePointColor));
				}
				if (adjustment.HasGamma)
				{
					bitmap2.ChangeGamma(1f + adjustment.Gamma);
				}
				if (adjustment.Sharpen != 0)
				{
					bitmap2.Sharpen((4 - adjustment.Sharpen) * 5, 1);
				}
				return bitmap2;
			}
			catch
			{
				if (bitmap2 != null && bitmap != bitmap2)
				{
					bitmap2.Dispose();
				}
				return null;
			}
		}

		public static Bitmap CreateAdjustedBitmap(this Bitmap bitmap, BitmapAdjustment adjustment, bool alwaysClone)
		{
			return bitmap.CreateAdjustedBitmap(adjustment, PixelFormat.Format32bppArgb, alwaysClone);
		}

		public static void Process(this Bitmap bitmap, IEnumerable<ImageAction> processingStack)
		{
			processingStack.ForEach(delegate(ImageAction a)
			{
				a(bitmap);
			});
		}
	}
}
