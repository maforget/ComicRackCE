using System.Drawing;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public class ThumbIconRenderer : ViewItemRenderer
	{
		public int TextHeight
		{
			get;
			set;
		}

		public ThumbIconRenderer(Image image, ThumbnailDrawingOptions flags)
		{
			TextHeight = 45;
			base.Image = image;
			base.Options = flags;
		}

		public Rectangle Draw(Graphics graphics, Rectangle bounds)
		{
			Rectangle rectangle = bounds;
			rectangle.Inflate(-base.Border.Width, -base.Border.Height);
			Rectangle rectangle2 = rectangle;
			rectangle2.Height -= TextHeight;
			Point location = new Point(rectangle2.Left, rectangle2.Top);
			Rectangle rectangle3 = DrawThumbnail(graphics, new Rectangle(location, rectangle2.Size));
			if (base.TextLines.Count == 0)
			{
				return rectangle3;
			}
			Rectangle rectangle4 = new Rectangle(rectangle.X, rectangle2.Bottom, rectangle.Width, TextHeight);
			Rectangle rect = rectangle4;
			rect.Inflate(-2, -2);
			if (base.Selected || base.Hot || base.Focused)
			{
				Rectangle rc = SimpleTextRenderer.MeasureText(graphics, base.TextLines, rect);
				rc.Inflate(2, 2);
				rc.Intersect(rectangle4);
				graphics.DrawStyledRectangle(rc, base.SelectionAlphaState, base.SelectionBackColor);
			}
			using (graphics.Fast())
			{
				return Rectangle.Union(rectangle3, SimpleTextRenderer.DrawText(graphics, base.TextLines, rect));
			}
		}
	}
}
