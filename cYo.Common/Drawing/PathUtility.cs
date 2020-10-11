using System.Drawing;
using System.Drawing.Drawing2D;

namespace cYo.Common.Drawing
{
	public static class PathUtility
	{
		public static GraphicsPath GetArrowPath(Rectangle bounds, bool up)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			if (up)
			{
				graphicsPath.AddLine(bounds.Left, bounds.Bottom, bounds.Left + bounds.Width / 2, bounds.Top);
				graphicsPath.AddLine(bounds.Left + bounds.Width / 2, bounds.Top, bounds.Right, bounds.Bottom);
			}
			else
			{
				graphicsPath.AddLine(bounds.Left, bounds.Top, bounds.Left + bounds.Width / 2, bounds.Bottom);
				graphicsPath.AddLine(bounds.Left + bounds.Width / 2, bounds.Bottom, bounds.Right, bounds.Top);
			}
			graphicsPath.CloseFigure();
			return graphicsPath;
		}

		public static GraphicsPath ConvertToPath(this Rectangle bounds, int roundedWidthTopLeft, int roundedHeightTopLeft, int roundedWidthTopRight, int roundedHeightTopRight, int roundedWidthBottomRight, int roundedHeightBottomRight, int roundedWidthBottomLeft, int roundedHeightBottomLeft)
		{
			return CreatePath(bounds.X, bounds.Y, bounds.Width, bounds.Height, roundedWidthTopLeft, roundedHeightTopLeft, roundedWidthTopRight, roundedHeightTopRight, roundedWidthBottomRight, roundedHeightBottomRight, roundedWidthBottomLeft, roundedHeightBottomLeft);
		}

		public static GraphicsPath ConvertToPath(this Rectangle bounds, int roundedWidth, int roundedHeight)
		{
			return CreatePath(bounds.X, bounds.Y, bounds.Width, bounds.Height, roundedWidth, roundedHeight, roundedWidth, roundedHeight, roundedWidth, roundedHeight, roundedWidth, roundedHeight);
		}

		public static GraphicsPath CreatePath(int left, int top, int width, int height, int roundedWidthTopLeft, int roundedHeightTopLeft, int roundedWidthTopRight, int roundedHeightTopRight, int roundedWidthBottomRight, int roundedHeightBottomRight, int roundedWidthBottomLeft, int roundedHeightBottomLeft)
		{
			GraphicsPath graphicsPath = new GraphicsPath();
			int num = left + width;
			int num2 = top + height;
			if (roundedWidthTopLeft == 0 || roundedHeightTopLeft == 0)
			{
				graphicsPath.AddLine(left, top, left + 1, top);
			}
			else
			{
				graphicsPath.AddArc(left, top, roundedWidthTopLeft * 2, roundedHeightTopLeft * 2, 180f, 90f);
			}
			if (roundedWidthTopRight == 0 || roundedHeightTopRight == 0)
			{
				graphicsPath.AddLine(num, top, num, top + 1);
			}
			else
			{
				graphicsPath.AddArc(num - roundedWidthTopRight * 2, top, roundedWidthTopRight * 2, roundedHeightTopRight * 2, 270f, 90f);
			}
			if (roundedWidthBottomRight == 0 || roundedHeightBottomRight == 0)
			{
				graphicsPath.AddLine(num, num2, num - 1, num2);
			}
			else
			{
				graphicsPath.AddArc(num - roundedWidthBottomRight * 2, num2 - roundedHeightBottomRight * 2, roundedWidthBottomRight * 2, roundedHeightBottomRight * 2, 0f, 90f);
			}
			if (roundedWidthBottomLeft == 0 || roundedHeightBottomLeft == 0)
			{
				graphicsPath.AddLine(left, num2, left, num2 - 1);
			}
			else
			{
				graphicsPath.AddArc(left, num2 - roundedHeightBottomLeft * 2, roundedWidthBottomLeft * 2, roundedHeightBottomLeft * 2, 90f, 90f);
			}
			graphicsPath.CloseFigure();
			return graphicsPath;
		}
	}
}
