using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Common.Drawing
{
	public static class RectangleExtensions
	{
		public static Rectangle Create(Point firstCorner, Point secondCorner)
		{
			int left = Math.Min(firstCorner.X, secondCorner.X);
			int top = Math.Min(firstCorner.Y, secondCorner.Y);
			int right = Math.Max(firstCorner.X, secondCorner.X);
			int bottom = Math.Max(firstCorner.Y, secondCorner.Y);
			return Rectangle.FromLTRB(left, top, right, bottom);
		}

		public static Rectangle Create(Point center, int radiusX, int radiusY)
		{
			int left = center.X - radiusX;
			int top = center.Y - radiusY;
			int right = center.X + radiusX;
			int bottom = center.Y + radiusY;
			return Rectangle.FromLTRB(left, top, right, bottom);
		}

		public static RectangleF Create(PointF firstCorner, PointF secondCorner)
		{
			float left = Math.Min(firstCorner.X, secondCorner.X);
			float top = Math.Min(firstCorner.Y, secondCorner.Y);
			float right = Math.Max(firstCorner.X, secondCorner.X);
			float bottom = Math.Max(firstCorner.Y, secondCorner.Y);
			return RectangleF.FromLTRB(left, top, right, bottom);
		}

		public static bool IsEmpty(this Rectangle rc)
		{
			if (rc.Width != 0)
			{
				return rc.Height == 0;
			}
			return true;
		}

		public static Point GetCenter(this Rectangle rc)
		{
			return new Point(rc.X + rc.Width / 2, rc.Y + rc.Height / 2);
		}

		public static PointF GetCenter(this RectangleF rc)
		{
			return new PointF(rc.X + rc.Width / 2f, rc.Y + rc.Height / 2f);
		}

		public static int IndexOfBestFit(this IEnumerable<Rectangle> rectangles, Rectangle test)
		{
			int result = -1;
			int num = 0;
			int num2 = 0;
			foreach (Rectangle rectangle2 in rectangles)
			{
				Rectangle rectangle = Rectangle.Intersect(rectangle2, test);
				int num3 = rectangle.Width * rectangle.Height;
				if (num3 > num2)
				{
					num2 = num3;
					result = num;
				}
				num++;
			}
			return result;
		}

		public static PointF TopLeft(this RectangleF rect)
		{
			return rect.Location;
		}

		public static PointF TopRight(this RectangleF rect)
		{
			return new PointF(rect.Right, rect.Top);
		}

		public static PointF BottomLeft(this RectangleF rect)
		{
			return new PointF(rect.Left, rect.Bottom);
		}

		public static PointF BottomRight(this RectangleF rect)
		{
			return new PointF(rect.Right, rect.Bottom);
		}

		public static Point TopLeft(this Rectangle rect)
		{
			return rect.Location;
		}

		public static Point TopRight(this Rectangle rect)
		{
			return new Point(rect.Right, rect.Top);
		}

		public static Point BottomLeft(this Rectangle rect)
		{
			return new Point(rect.Left, rect.Bottom);
		}

		public static Point BottomRight(this Rectangle rect)
		{
			return new Point(rect.Right, rect.Bottom);
		}

		public static Rectangle Pad(this Rectangle rc, Padding margin)
		{
			rc.X += margin.Left;
			rc.Y += margin.Top;
			rc.Width -= margin.Horizontal;
			rc.Height -= margin.Vertical;
			return rc;
		}

		public static Rectangle Pad(this Rectangle rc, int all)
		{
			return rc.Pad(new Padding(all));
		}

		public static RectangleF Pad(this RectangleF rc, Padding margin)
		{
			rc.X += margin.Left;
			rc.Y += margin.Top;
			rc.Width -= margin.Horizontal;
			rc.Height -= margin.Vertical;
			return rc;
		}

		public static RectangleF Pad(this RectangleF rc, int all)
		{
			return rc.Pad(new Padding(all));
		}

		public static Rectangle Pad(this Rectangle rc, int left, int top, int right = 0, int bottom = 0)
		{
			return rc.Pad(new Padding(left, top, right, bottom));
		}

		public static Padding GetPadding(this Rectangle inner, Rectangle outer)
		{
			return new Padding(inner.Left - outer.Left, inner.Top - outer.Top, outer.Right - inner.Right, outer.Bottom - inner.Bottom);
		}

		public static Padding GetPadding(this Rectangle inner, Size outerSize)
		{
			return inner.GetPadding(new Rectangle(Point.Empty, outerSize));
		}

		public static RectangleF Grow(this RectangleF rect, float width, float height)
		{
			rect.Inflate(width, height);
			return rect;
		}

		public static RectangleF Grow(this RectangleF rect, float n)
		{
			return rect.Grow(n, n);
		}

		public static IEnumerable<RectangleF> GetBorderRectangles(RectangleF rd, RectangleF rs, Rectangle ir)
		{
			if (rs.Width == 0f || rs.Height == 0f)
			{
				return new RectangleF[0];
			}
			float num = rd.Width / rs.Width;
			float num2 = rd.Height / rs.Height;
			RectangleF item = RectangleF.Empty;
			RectangleF item2 = RectangleF.Empty;
			RectangleF item3 = RectangleF.Empty;
			RectangleF item4 = RectangleF.Empty;
			if (rs.Bottom > (float)ir.Bottom)
			{
				float num3 = rs.Bottom - (float)ir.Bottom;
				item3 = new RectangleF(rd.Left, rd.Bottom - num2 * num3, rd.Height, num2 * num3);
			}
			if (rs.Top < (float)ir.Top)
			{
				float num4 = (float)ir.Top - rs.Top;
				item = new RectangleF(rd.Left, rd.Top, rd.Width, num4 * num2);
			}
			if (rs.Right > (float)ir.Right)
			{
				float num5 = rs.Right - (float)ir.Right;
				item4 = new RectangleF(rd.Right - num * num5, rd.Top, num * num5, rd.Height);
			}
			if (rs.Left < (float)ir.Left)
			{
				float num6 = (float)ir.Left - rs.Left;
				item2 = new RectangleF(rd.Left, rd.Top, num * num6, rd.Height);
			}
			if (!item.IsEmpty)
			{
				float num7 = Math.Max(rd.Top, item.Bottom);
				if (!item4.IsEmpty)
				{
					item4.Y = num7;
					item4.Height = rd.Height - (rd.Top - num7);
				}
				if (!item2.IsEmpty)
				{
					item2.Y = num7;
					item2.Height = rd.Height - (rd.Top - num7);
				}
			}
			if (!item3.IsEmpty)
			{
				float height = Math.Min(rd.Bottom, item3.Top);
				if (!item4.IsEmpty)
				{
					item4.Height = height;
				}
				if (!item2.IsEmpty)
				{
					item2.Height = height;
				}
			}
			List<RectangleF> list = new List<RectangleF>(4);
			if (!item2.IsEmpty)
			{
				list.Add(item2);
			}
			if (!item4.IsEmpty)
			{
				list.Add(item4);
			}
			if (!item.IsEmpty)
			{
				list.Add(item);
			}
			if (!item3.IsEmpty)
			{
				list.Add(item3);
			}
			return list.ToArray();
		}

		public static IEnumerable<RectangleF> GetSubRectangles(this RectangleF rect, RectangleF sub, bool clip)
		{
			int nx = (int)((rect.Width + sub.Width - 1f) / sub.Width);
			int ny = (int)((rect.Height + sub.Height - 1f) / sub.Height);
			RectangleF r = new RectangleF(rect.Location, sub.Size);
			for (int x = 0; x < nx; x++)
			{
				for (int y = 0; y < ny; y++)
				{
					yield return clip ? RectangleF.Intersect(rect, r) : r;
					r.Y += sub.Height;
				}
				r.X += sub.Width;
				r.Y = rect.Y;
			}
		}

		public static Rectangle Align(this Rectangle rectangle, Rectangle bounds, ContentAlignment alignment)
		{
			switch (alignment)
			{
			case ContentAlignment.TopLeft:
			case ContentAlignment.TopCenter:
			case ContentAlignment.TopRight:
				rectangle.Y = bounds.Y;
				break;
			case ContentAlignment.MiddleLeft:
			case ContentAlignment.MiddleCenter:
			case ContentAlignment.MiddleRight:
				rectangle.Y = bounds.Y + (bounds.Height - rectangle.Height) / 2;
				break;
			case ContentAlignment.BottomLeft:
			case ContentAlignment.BottomCenter:
			case ContentAlignment.BottomRight:
				rectangle.Y = bounds.Y + bounds.Height - rectangle.Height;
				break;
			}
			switch (alignment)
			{
			case ContentAlignment.TopLeft:
			case ContentAlignment.MiddleLeft:
			case ContentAlignment.BottomLeft:
				rectangle.X = bounds.X;
				break;
			case ContentAlignment.TopCenter:
			case ContentAlignment.MiddleCenter:
			case ContentAlignment.BottomCenter:
				rectangle.X = bounds.X + (bounds.Width - rectangle.Width) / 2;
				break;
			case ContentAlignment.TopRight:
			case ContentAlignment.MiddleRight:
			case ContentAlignment.BottomRight:
				rectangle.X = bounds.X + bounds.Width - rectangle.Width;
				break;
			}
			return rectangle;
		}

		public static Rectangle Center(this Rectangle rectangle, Rectangle bounds)
		{
			return rectangle.Align(bounds, ContentAlignment.MiddleCenter);
		}

		public static Rectangle AlignHorizontal(this Rectangle rectangle, int offset, StringAlignment alignment)
		{
			switch (alignment)
			{
			default:
				return new Rectangle(offset - rectangle.Width / 2, rectangle.Y, rectangle.Width, rectangle.Height);
			case StringAlignment.Far:
				return new Rectangle(offset, rectangle.Y, rectangle.Width, rectangle.Height);
			case StringAlignment.Near:
				return new Rectangle(offset - rectangle.Width, rectangle.Y, rectangle.Width, rectangle.Height);
			}
		}

		public static RectangleF Align(this RectangleF rectangle, RectangleF bounds, ContentAlignment alignment)
		{
			switch (alignment)
			{
			case ContentAlignment.TopLeft:
			case ContentAlignment.TopCenter:
			case ContentAlignment.TopRight:
				rectangle.Y = bounds.Y;
				break;
			case ContentAlignment.MiddleLeft:
			case ContentAlignment.MiddleCenter:
			case ContentAlignment.MiddleRight:
				rectangle.Y = bounds.Y + (bounds.Height - rectangle.Height) / 2f;
				break;
			case ContentAlignment.BottomLeft:
			case ContentAlignment.BottomCenter:
			case ContentAlignment.BottomRight:
				rectangle.Y = bounds.Y + bounds.Height - rectangle.Height;
				break;
			}
			switch (alignment)
			{
			case ContentAlignment.TopLeft:
			case ContentAlignment.MiddleLeft:
			case ContentAlignment.BottomLeft:
				rectangle.X = bounds.X;
				break;
			case ContentAlignment.TopCenter:
			case ContentAlignment.MiddleCenter:
			case ContentAlignment.BottomCenter:
				rectangle.X = bounds.X + (bounds.Width - rectangle.Width) / 2f;
				break;
			case ContentAlignment.TopRight:
			case ContentAlignment.MiddleRight:
			case ContentAlignment.BottomRight:
				rectangle.X = bounds.X + bounds.Width - rectangle.Width;
				break;
			}
			return rectangle;
		}

		public static RectangleF Center(this RectangleF rectangle, RectangleF bounds)
		{
			return rectangle.Align(bounds, ContentAlignment.MiddleCenter);
		}

		public static RectangleF AlignHorizontal(this RectangleF rectangle, float offset, StringAlignment alignment)
		{
			switch (alignment)
			{
			default:
				return new RectangleF(offset - rectangle.Width / 2f, rectangle.Y, rectangle.Width, rectangle.Height);
			case StringAlignment.Far:
				return new RectangleF(offset, rectangle.Y, rectangle.Width, rectangle.Height);
			case StringAlignment.Near:
				return new RectangleF(offset - rectangle.Width, rectangle.Y, rectangle.Width, rectangle.Height);
			}
		}

		public static Rectangle Scale(this Rectangle rect, float scaleX, float scaleY)
		{
			rect.X = (int)((float)rect.X * scaleX);
			rect.Y = (int)((float)rect.Y * scaleY);
			rect.Width = (int)((float)rect.Width * scaleX);
			rect.Height = (int)((float)rect.Height * scaleY);
			return rect;
		}

		public static Rectangle Scale(this Rectangle rect, float scale)
		{
			return rect.Scale(scale, scale);
		}

		public static Rectangle Fit(this Rectangle rect, Rectangle fit, ScaleMode scaleMode = ScaleMode.FitAll)
		{
			return rect.Scale(rect.Size.GetScale(fit.Size, scaleMode)).Align(fit, ContentAlignment.MiddleCenter);
		}

		public static RectangleF Scale(this RectangleF rect, float scaleX, float scaleY)
		{
			rect.X *= scaleX;
			rect.Y *= scaleY;
			rect.Width *= scaleX;
			rect.Height *= scaleY;
			return rect;
		}

		public static RectangleF Scale(this RectangleF rect, float scale)
		{
			return rect.Scale(scale, scale);
		}

		public static RectangleF Fit(this RectangleF rect, RectangleF fit, ScaleMode scaleMode = ScaleMode.FitAll)
		{
			return rect.Scale(rect.Size.GetScale(fit.Size, scaleMode)).Align(fit, ContentAlignment.MiddleCenter);
		}

		public static Rectangle Round(this RectangleF rect)
		{
			return Rectangle.Round(rect);
		}

		public static RectangleF Add(this RectangleF r, float x, float y)
		{
			r.Offset(x, y);
			return r;
		}

		public static Rectangle Add(this Rectangle r, int x, int y)
		{
			r.Offset(x, y);
			return r;
		}

		public static Rectangle Rotate(this Rectangle rectangle, Matrix rotationMatrix)
		{
			if (rotationMatrix == null)
			{
				throw new ArgumentNullException();
			}
			Point[] array = new Point[4]
			{
				new Point(rectangle.X, rectangle.Y),
				new Point(rectangle.Right, rectangle.Y),
				new Point(rectangle.Right, rectangle.Bottom),
				new Point(rectangle.X, rectangle.Bottom)
			};
			rotationMatrix.TransformPoints(array);
			return array.GetBounds();
		}

		public static Rectangle Rotate(this Rectangle rectangle, int angle)
		{
			return rectangle.Rotate(MatrixUtility.GetRotationMatrix(rectangle.Size, angle));
		}

		public static Rectangle Rotate(this Rectangle rectangle, ImageRotation rotation)
		{
			return rectangle.Rotate(rotation.ToDegrees());
		}

		public static PointF[] ToPoints(this RectangleF rect)
		{
			return new PointF[4]
			{
				rect.TopLeft(),
				rect.TopRight(),
				rect.BottomLeft(),
				rect.BottomRight()
			};
		}

		public static PointF[] ToLineStrip(this RectangleF rect)
		{
			return new PointF[5]
			{
				rect.TopLeft(),
				rect.TopRight(),
				rect.BottomRight(),
				rect.BottomLeft(),
				rect.TopLeft()
			};
		}

		public static RectangleF ToRectangle(this IEnumerable<PointF> points)
		{
			return RectangleF.FromLTRB(points.Min((PointF pt) => pt.X), points.Min((PointF pt) => pt.Y), points.Max((PointF pt) => pt.X), points.Max((PointF pt) => pt.Y));
		}

		public static Point[] ToPoints(this Rectangle rect)
		{
			return new Point[4]
			{
				rect.TopLeft(),
				rect.TopRight(),
				rect.BottomLeft(),
				rect.BottomRight()
			};
		}

		public static Rectangle ToRectangle(this IEnumerable<Point> points)
		{
			return Rectangle.FromLTRB(points.Min((Point pt) => pt.X), points.Min((Point pt) => pt.Y), points.Max((Point pt) => pt.X), points.Max((Point pt) => pt.Y));
		}

		public static RectangleF Union(this RectangleF a, RectangleF b)
		{
			if (!a.IsEmpty)
			{
				return RectangleF.Union(a, b);
			}
			return b;
		}

		public static Rectangle Union(this Rectangle a, Rectangle b)
		{
			if (!a.IsEmpty)
			{
				return Rectangle.Union(a, b);
			}
			return b;
		}

		public static Rectangle Subtract(this Rectangle a, Rectangle b)
		{
			Rectangle rectangle = Rectangle.Intersect(a, b);
			if (rectangle.IsEmpty)
			{
				return a;
			}
			if (rectangle.Top == a.Top)
			{
				a = a.Pad(0, rectangle.Height);
			}
			else if (rectangle.Bottom == a.Bottom)
			{
				a = a.Pad(0, 0, 0, rectangle.Height);
			}
			if (rectangle.Left == a.Left)
			{
				a = a.Pad(rectangle.Width, 0);
			}
			else if (rectangle.Right == a.Right)
			{
				a = a.Pad(0, 0, rectangle.Width);
			}
			return a;
		}
	}
}
