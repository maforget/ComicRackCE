using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Presentation
{
	public class ScalableBitmap
	{
		public Bitmap Bitmap
		{
			get;
			set;
		}

		public Padding Margin
		{
			get;
			set;
		}

		public Rectangle Inner
		{
			get
			{
				return new Rectangle(Point.Empty, Bitmap.Size).Pad(Margin);
			}
			set
			{
				Margin = value.GetPadding(Bitmap.Size);
			}
		}

		public ScalableBitmap(Bitmap bitmap, Padding margin)
		{
			Bitmap = bitmap;
			Margin = margin;
		}

		public ScalableBitmap(Bitmap bitmap, int left, int top, int right, int bottom)
			: this(bitmap, new Padding(left, top, right, bottom))
		{
		}

		public ScalableBitmap(Bitmap bitmap, Rectangle inner)
			: this(bitmap, inner.GetPadding(bitmap.Size))
		{
		}

		public ScalableBitmap(Bitmap bitmap)
			: this(bitmap, Padding.Empty)
		{
		}

		public RectangleF Draw(IBitmapRenderer gr, RectangleF dest, RectangleF src, BitmapAdjustment itf, float opacity)
		{
			return Draw(gr, Bitmap, dest, src, Margin, itf, opacity);
		}

		public RectangleF Draw(IBitmapRenderer gr, RectangleF dest, BitmapAdjustment itf, float opacity)
		{
			return Draw(gr, dest, new RectangleF(PointF.Empty, Bitmap.Size), itf, opacity);
		}

		public RectangleF Draw(IBitmapRenderer gr, RectangleF dest, float opacity)
		{
			return Draw(gr, dest, BitmapAdjustment.Empty, opacity);
		}

		public RectangleF Draw(IBitmapRenderer gr, RectangleF dest)
		{
			return Draw(gr, dest, 1f);
		}

		public static RectangleF Draw(IBitmapRenderer gr, Bitmap bmp, RectangleF dest, RectangleF src, Padding margin, BitmapAdjustment itf, float opacity)
		{
			if (margin.All == 0)
			{
				gr?.DrawImage(bmp, dest, src, itf, opacity);
				return dest;
			}
			float num = margin.Left;
			float num2 = margin.Right;
			float num3 = margin.Top;
			float num4 = margin.Bottom;
			RectangleF src2 = new RectangleF(src.Left + (float)margin.Left, src.Top + (float)margin.Top, src.Width - (float)margin.Horizontal, src.Height - (float)margin.Vertical);
			RectangleF rectangleF = new RectangleF(dest.Left + num, dest.Top + num3, dest.Width - num - num2, dest.Height - num3 - num4);
			if (gr != null)
			{
				gr.DrawImage(bmp, new RectangleF(dest.Left, dest.Top, num, num3), new RectangleF(src.Left, src.Top, margin.Left, margin.Top), itf, opacity);
				gr.DrawImage(bmp, new RectangleF(rectangleF.Left, dest.Top, rectangleF.Width, num3), new RectangleF(src2.Left, src.Top, src2.Width, margin.Top), itf, opacity);
				gr.DrawImage(bmp, new RectangleF(rectangleF.Right, dest.Top, num2, num3), new RectangleF(src2.Right, src.Top, margin.Right, margin.Top), itf, opacity);
				gr.DrawImage(bmp, new RectangleF(rectangleF.Right, rectangleF.Top, num2, rectangleF.Height), new RectangleF(src2.Right, src2.Top, margin.Right, src2.Height), itf, opacity);
				gr.DrawImage(bmp, new RectangleF(rectangleF.Right, rectangleF.Bottom, num2, num4), new RectangleF(src2.Right, src2.Bottom, margin.Right, margin.Bottom), itf, opacity);
				gr.DrawImage(bmp, new RectangleF(rectangleF.Left, rectangleF.Bottom, rectangleF.Width, num4), new RectangleF(src2.Left, src2.Bottom, src2.Width, margin.Bottom), itf, opacity);
				gr.DrawImage(bmp, new RectangleF(dest.Left, rectangleF.Bottom, num, num4), new RectangleF(src.Left, src2.Bottom, margin.Left, margin.Bottom), itf, opacity);
				gr.DrawImage(bmp, new RectangleF(dest.Left, rectangleF.Top, num, rectangleF.Height), new RectangleF(src.Left, src2.Top, margin.Left, src2.Height), itf, opacity);
				gr.DrawImage(bmp, rectangleF, src2, itf, opacity);
			}
			return rectangleF;
		}

		public static RectangleF Draw(IBitmapRenderer gr, Bitmap bmp, RectangleF dest, RectangleF src, Padding margin, float opacity)
		{
			return Draw(gr, bmp, dest, src, margin, BitmapAdjustment.Empty, opacity);
		}

		public static RectangleF Draw(IBitmapRenderer gr, Bitmap bmp, RectangleF dest, Padding margin, float opacity)
		{
			return Draw(gr, bmp, dest, new RectangleF(Point.Empty, bmp.Size), margin, opacity);
		}

		public static implicit operator ScalableBitmap(Bitmap bitmap)
		{
			return new ScalableBitmap(bitmap);
		}
	}
}
