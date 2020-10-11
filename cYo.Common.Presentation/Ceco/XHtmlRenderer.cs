using System;
using System.Drawing;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Presentation.Ceco.Builders;

namespace cYo.Common.Presentation.Ceco
{
	public static class XHtmlRenderer
	{
		private class BodyKey
		{
			public Font Font
			{
				get;
				set;
			}

			public string Text
			{
				get;
				set;
			}

			public override bool Equals(object obj)
			{
				BodyKey bodyKey = obj as BodyKey;
				if (bodyKey != null && bodyKey.Font == Font)
				{
					return bodyKey.Text == Text;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return Font.GetHashCode() ^ Text.GetHashCode();
			}
		}

		private static Cache<BodyKey, BodyBlock> bodyCache = new Cache<BodyKey, BodyBlock>(100);

		public static void DrawString(Graphics graphics, string s, Font font, Color foreColor, int x, int y)
		{
			DrawString(graphics, s, font, foreColor, new Point(x, y));
		}

		public static void DrawString(Graphics graphics, string s, Font font, Color foreColor, Point location)
		{
			DrawString(graphics, s, font, foreColor, new Rectangle(location, Size.Empty));
		}

		public static void DrawString(Graphics graphics, string s, Font font, Color foreColor, Rectangle layoutRectangle)
		{
			DrawString(graphics, s, font, foreColor, layoutRectangle, ContentAlignment.TopLeft);
		}

		public static void DrawString(Graphics graphics, string s, Font font, Color foreColor, Rectangle layoutRectangle, StringFormat sf)
		{
			DrawString(graphics, s, font, foreColor, layoutRectangle, EnumExtensions.FromAlignments(sf.Alignment, sf.LineAlignment));
		}

		public static void DrawString(Graphics graphics, string s, Font font, Color foreColor, Rectangle layoutRectangle, ContentAlignment align)
		{
			using (IItemLock<BodyBlock> itemLock = GetBody(s, font))
			{
				BodyBlock item = itemLock.Item;
				item.ForeColor = foreColor;
				if (layoutRectangle.Width <= 0)
				{
					layoutRectangle.Width = int.MaxValue;
				}
				if (layoutRectangle.Height <= 0)
				{
					layoutRectangle.Height = int.MaxValue;
				}
				item.Align = align.ToAlignment().ToHorizontalAlignment();
				VerticalAlignment verticalAlignment = align.ToLineAlignment().ToVerticalAlignment();
				if (verticalAlignment != 0 || verticalAlignment != VerticalAlignment.Top)
				{
					item.Measure(graphics, layoutRectangle.Width);
					switch (verticalAlignment)
					{
					case VerticalAlignment.Middle:
						layoutRectangle.Y += (layoutRectangle.Height - item.ActualSize.Height) / 2;
						break;
					case VerticalAlignment.Bottom:
						layoutRectangle.Y += layoutRectangle.Bottom - item.ActualSize.Height;
						break;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				item.Bounds = new Rectangle(Point.Empty, layoutRectangle.Size);
				item.Draw(graphics, layoutRectangle.Location);
			}
		}

		public static Size MeasureString(Graphics graphics, string s, Font font)
		{
			return MeasureString(graphics, s, font, 0);
		}

		public static Size MeasureString(Graphics graphics, string s, Font font, int width)
		{
			using (IItemLock<BodyBlock> itemLock = GetBody(s, font))
			{
				BodyBlock item = itemLock.Item;
				if (width <= 0)
				{
					width = int.MaxValue;
				}
				item.Measure(graphics, width);
				return item.ActualSize;
			}
		}

		private static IItemLock<BodyBlock> GetBody(string text, Font font)
		{
			return bodyCache.LockItem(new BodyKey
			{
				Text = text,
				Font = font
			}, delegate(BodyKey bk)
			{
				BodyBlock bodyBlock = new BodyBlock();
				bodyBlock.Inlines.AddRange(XHtmlParser.Parse(bk.Text).Inlines);
				bodyBlock.Font = font;
				return bodyBlock;
			});
		}

		public static HorizontalAlignment ToHorizontalAlignment(this StringAlignment align)
		{
			switch (align)
			{
			case StringAlignment.Near:
				return HorizontalAlignment.Left;
			case StringAlignment.Center:
				return HorizontalAlignment.Center;
			case StringAlignment.Far:
				return HorizontalAlignment.Right;
			default:
				return HorizontalAlignment.None;
			}
		}

		public static VerticalAlignment ToVerticalAlignment(this StringAlignment align)
		{
			switch (align)
			{
			case StringAlignment.Near:
				return VerticalAlignment.Top;
			case StringAlignment.Center:
				return VerticalAlignment.Middle;
			case StringAlignment.Far:
				return VerticalAlignment.Bottom;
			default:
				return VerticalAlignment.None;
			}
		}
	}
}
