using cYo.Common.Windows.Forms.Theme;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public static class BorderUtility
	{
		public static void DrawBorder(Graphics g, Rectangle bounds, ExtendedBorderStyle style)
		{
            Border3DStyle style2;
			switch (style)
			{
			default:
				return;
			case ExtendedBorderStyle.Flat:
				style2 = Border3DStyle.Flat;
				break;
			case ExtendedBorderStyle.Sunken:
				style2 = Border3DStyle.Sunken;
				break;
			case ExtendedBorderStyle.Raised:
				style2 = Border3DStyle.Raised;
				break;
			}
            //ControlPaint.DrawBorder3D(g, bounds, style2);
            ControlPaintEx.DrawBorder3D(g, bounds, style2);
		}

        public static Rectangle AdjustBorder(Rectangle bounds, ExtendedBorderStyle style, bool inwards)
		{
			int num = ((!inwards) ? 1 : (-1));
			switch (style)
			{
			case ExtendedBorderStyle.Flat:
				bounds.Inflate(num * 2, num * 2);
				break;
			case ExtendedBorderStyle.Sunken:
			case ExtendedBorderStyle.Raised:
				bounds.Inflate(num * 4, num * 4);
				break;
			}
			return bounds;
		}

		public static Rectangle AdjustBorder(Rectangle bounds, ExtendedBorderStyle style)
		{
			return AdjustBorder(bounds, style, inwards: true);
		}
	}
}
