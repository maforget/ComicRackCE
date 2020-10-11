using System;
using System.Drawing;

namespace cYo.Common.Drawing
{
	public static class SizeExtensions
	{
		public static bool IsEmpty(this Size size)
		{
			if (size.Width != 0)
			{
				return size.Height == 0;
			}
			return true;
		}

		public static bool IsEmpty(this SizeF size)
		{
			if (size.Width != 0f)
			{
				return size.Height == 0f;
			}
			return true;
		}

		public static float GetScale(this SizeF size, SizeF targetSize, ScaleMode scaleMode = ScaleMode.FitAll, bool allowOverSize = true)
		{
			if (scaleMode == ScaleMode.None)
			{
				return 1f;
			}
			float num = targetSize.Width / size.Width;
			float num2 = targetSize.Height / size.Height;
			float num3;
			if (num == 0f)
			{
				num3 = num2;
			}
			else if (num2 == 0f)
			{
				num3 = num;
			}
			else
			{
				switch (scaleMode)
				{
				case ScaleMode.FitWidth:
					num3 = num;
					break;
				case ScaleMode.FitHeight:
					num3 = num2;
					break;
				case ScaleMode.Center:
					num3 = ((num2 >= num) ? num2 : num);
					break;
				case ScaleMode.Fill:
					num3 = ((num2 > num) ? num2 : num);
					break;
				default:
					num3 = ((num2 < num) ? num2 : num);
					break;
				}
			}
			if (!allowOverSize)
			{
				return Math.Min(1f, num3);
			}
			return num3;
		}

		public static float GetScale(this Size size, Size targetSize, ScaleMode scaleMode = ScaleMode.FitAll, bool allowOversize = true)
		{
			return ((SizeF)size).GetScale((SizeF)targetSize, scaleMode, allowOverSize: true);
		}

		public static Size Scale(this Size size, float scale)
		{
			return new Size((int)((float)size.Width * scale), (int)((float)size.Height * scale));
		}

		public static Size Scale(this Size size, float scaleX, float scaleY)
		{
			return new Size((int)((float)size.Width * scaleX), (int)((float)size.Height * scaleY));
		}

		public static Rectangle ToRectangle(this Size size)
		{
			return new Rectangle(Point.Empty, size);
		}

		public static RectangleF ToRectangle(this SizeF size)
		{
			return new RectangleF(PointF.Empty, size);
		}

		public static RectangleF ToRectangle(this SizeF size, SizeF targetSize, RectangleScaleMode mode = RectangleScaleMode.Center)
		{
			if (size.IsEmpty())
			{
				return Rectangle.Empty;
			}
			if (size == targetSize)
			{
				return new RectangleF(0f, 0f, size.Width, size.Height);
			}
			bool flag = (mode & RectangleScaleMode.Center) != 0;
			bool flag2 = (mode & RectangleScaleMode.OnlyShrink) != 0;
			float num = size.GetScale(targetSize);
			if (num > 1f && flag2)
			{
				num = 1f;
			}
			RectangleF result = new RectangleF(0f, 0f, size.Width * num, size.Height * num);
			if (flag)
			{
				if (targetSize.Width < 1f)
				{
					targetSize.Width = result.Width;
				}
				if (targetSize.Height < 1f)
				{
					targetSize.Height = result.Height;
				}
				result.Offset((targetSize.Width - result.Width) / 2f, (targetSize.Height - result.Height) / 2f);
			}
			return result;
		}

		public static RectangleF ToRectangle(this SizeF size, RectangleF targetBounds, RectangleScaleMode mode = RectangleScaleMode.Center)
		{
			RectangleF result = size.ToRectangle(targetBounds.Size, mode);
			result.Offset(targetBounds.Location);
			return result;
		}

		public static Rectangle ToRectangle(this Size size, Size targetSize, RectangleScaleMode mode = RectangleScaleMode.Center)
		{
			return Rectangle.Truncate(((SizeF)size).ToRectangle((SizeF)targetSize, mode));
		}

		public static Rectangle ToRectangle(this Size size, Rectangle targetBounds, RectangleScaleMode mode = RectangleScaleMode.Center)
		{
			return Rectangle.Truncate(((SizeF)size).ToRectangle((RectangleF)targetBounds, mode));
		}

		public static Rectangle Align(this Size size, Rectangle bounds, ContentAlignment alignment)
		{
			return size.ToRectangle().Align(bounds, alignment);
		}

		public static Size Rotate(this Size size, ImageRotation rotation)
		{
			if (rotation == ImageRotation.Rotate270 || rotation == ImageRotation.Rotate90)
			{
				return new Size(size.Height, size.Width);
			}
			return size;
		}
	}
}
