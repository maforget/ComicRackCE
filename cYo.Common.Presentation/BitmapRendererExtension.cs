using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Presentation
{
	public static class BitmapRendererExtension
	{
		public static void DrawImage(this IBitmapRenderer gr, RendererImage image, RectangleF dest, RectangleF src)
		{
			gr.DrawImage(image, dest, src, BitmapAdjustment.Empty, 1f);
		}

		public static void DrawRectangle(this IBitmapRenderer gr, RectangleF r, Color color, float width)
		{
			gr.DrawLine(r.ToLineStrip(), color, width);
		}

		public static void DrawRectangle(this IBitmapRenderer gr, PointF a, PointF b, Color color, float width)
		{
			gr.DrawRectangle(RectangleExtensions.Create(a, b), color, width);
		}

		public static void DrawImage(this IBitmapRenderer gr, RendererImage image, RectangleF dest, RectangleF src, BitmapAdjustment transform, float opacity, RectangleF clip)
		{
			if (src.IsEmpty)
			{
				return;
			}
			if (!clip.IsEmpty && clip != dest)
			{
				float num = src.Width / dest.Width;
				float num2 = src.Height / dest.Height;
				RectangleF rectangleF = RectangleF.Intersect(dest, clip);
				if (rectangleF.IsEmpty)
				{
					return;
				}
				src.X += (rectangleF.X - dest.X) * num;
				src.Y += (rectangleF.Y - dest.Y) * num2;
				src.Width += (rectangleF.Width - dest.Width) * num;
				src.Height += (rectangleF.Height - dest.Height) * num2;
				dest = rectangleF;
			}
			gr.DrawImage(image, dest, src, transform, opacity);
		}

		public static void FillRectangle(this IBitmapRenderer gr, RendererImage image, ImageLayout id, RectangleF dest, RectangleF src, BitmapAdjustment transform, float opacity)
		{
			gr.FillRectangle(image, id, dest, src, transform, opacity, dest);
		}

		public static void FillRectangle(this IBitmapRenderer gr, RendererImage image, ImageLayout id, RectangleF dest, RectangleF src, BitmapAdjustment transform, float opacity, RectangleF clip)
		{
			switch (id)
			{
			case ImageLayout.None:
				dest.Size = src.Size;
				gr.DrawImage(image, dest, src, transform, opacity, clip);
				break;
			case ImageLayout.Center:
				gr.DrawImage(image, src.Align(dest, ContentAlignment.MiddleCenter), src, transform, opacity, clip);
				break;
			case ImageLayout.Stretch:
				gr.DrawImage(image, dest, src, transform, opacity, clip);
				break;
			case ImageLayout.Tile:
				foreach (RectangleF subRectangle in dest.GetSubRectangles(src, clip: true))
				{
					gr.DrawImage(image, subRectangle, subRectangle.Size.ToRectangle(), transform, opacity, clip);
				}
				break;
			case ImageLayout.Zoom:
				gr.DrawImage(image, src.Fit(dest, ScaleMode.Center), src, transform, opacity, clip);
				break;
			}
		}

		public static void DrawShadow(this IBitmapRenderer graphics, RectangleF rectangle, int depth, Color color, float opacity, BlurShadowType bst, BlurShadowParts parts)
		{
			using (Bitmap shadowBitmap = GraphicsExtensions.CreateShadowBitmap(bst, color, depth, opacity))
			{
				graphics.DrawShadow(rectangle, shadowBitmap, parts);
			}
		}

		public static void DrawShadow(this IBitmapRenderer graphics, RectangleF rectangle, Bitmap shadowBitmap, BlurShadowParts parts)
		{
			graphics.DrawShadow(rectangle, shadowBitmap, shadowBitmap.Width / 2, parts);
		}

		public static void DrawShadow(this IBitmapRenderer graphics, RectangleF rectangle, Bitmap shadowBitmap, int depth, BlurShadowParts parts)
		{
			int num = shadowBitmap.Width / 2;
			if ((parts & BlurShadowParts.Center) != 0)
			{
				graphics.FillRectangle(new RectangleF(rectangle.Left + (float)depth, rectangle.Top + (float)depth, rectangle.Width - (float)(2 * depth), rectangle.Height - (float)(2 * depth)), shadowBitmap.GetPixel(num, num));
			}
			if ((parts & BlurShadowParts.TopLeft) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Left, rectangle.Top, depth, depth), new RectangleF(0f, 0f, num, num));
			}
			if ((parts & BlurShadowParts.TopCenter) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Left + (float)depth, rectangle.Top, rectangle.Width - (float)(2 * depth), depth), new RectangleF(num, 0f, 1f, num));
			}
			if ((parts & BlurShadowParts.TopRight) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Right - (float)depth, rectangle.Top, depth, depth), new RectangleF(num, 0f, num, num));
			}
			if ((parts & BlurShadowParts.CenterRight) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Right - (float)depth, rectangle.Top + (float)depth, depth, rectangle.Height - (float)(2 * depth)), new RectangleF(num, num, num, 1f));
			}
			if ((parts & BlurShadowParts.BottomRight) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Right - (float)depth, rectangle.Bottom - (float)depth, depth, depth), new RectangleF(num, num, num, num));
			}
			if ((parts & BlurShadowParts.BottomCenter) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Left + (float)depth, rectangle.Bottom - (float)depth, rectangle.Width - (float)(2 * depth), depth), new RectangleF(num, num, 1f, num));
			}
			if ((parts & BlurShadowParts.BottomLeft) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Left, rectangle.Bottom - (float)depth, depth, depth), new RectangleF(0f, num, num, num));
			}
			if ((parts & BlurShadowParts.CenterLeft) != 0)
			{
				graphics.DrawImage(shadowBitmap, new RectangleF(rectangle.Left, rectangle.Top + (float)depth, depth, rectangle.Height - (float)(2 * depth)), new RectangleF(0f, num, num, 1f));
			}
		}
	}
}
