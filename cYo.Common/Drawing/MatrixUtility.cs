using System.Drawing;
using System.Drawing.Drawing2D;

namespace cYo.Common.Drawing
{
	public static class MatrixUtility
	{
		public static Matrix GetRotationMatrix(Point anchor, int pageRotation)
		{
			Matrix matrix = new Matrix();
			if (pageRotation % 360 == 0)
			{
				return matrix;
			}
			matrix.Translate(-anchor.X, -anchor.Y, MatrixOrder.Append);
			matrix.Rotate(pageRotation, MatrixOrder.Append);
			matrix.Translate(anchor.X, anchor.Y, MatrixOrder.Append);
			return matrix;
		}

		public static Matrix GetRotationMatrix(Size page, int pageRotation)
		{
			return GetRotationMatrix(new Point(page.Width / 2, page.Height / 2), pageRotation);
		}
	}
}
