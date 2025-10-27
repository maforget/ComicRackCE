using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Projects.ComicRack.Engine.Controls
{
	public static class ListStyles
	{
		private static Rectangle oldBounds;

		public static void SetOwnerDrawn(ListView lv)
		{
			lv.OwnerDraw = true;
			lv.DrawItem += DrawItem;
			lv.DrawColumnHeader += DrawColumnHeader;
			lv.DrawSubItem += DrawSubItem;
			lv.MouseMove += MouseMove;
		}

		private static void MouseMove(object sender, MouseEventArgs e)
		{
			ListView listView = sender as ListView;
			ListViewItem itemAt = listView.GetItemAt(e.X, e.Y);
			if (itemAt != null && itemAt.Bounds != oldBounds)
			{
				listView.Invalidate(itemAt.Bounds);
				oldBounds = itemAt.Bounds;
			}
		}

		private static void DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			e.DrawDefault = false;
			TextFormatFlags textFormatFlags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.VerticalCenter;
			switch (e.Header.TextAlign)
			{
			case HorizontalAlignment.Right:
				textFormatFlags |= TextFormatFlags.Right;
				break;
			case HorizontalAlignment.Center:
				textFormatFlags |= TextFormatFlags.HorizontalCenter;
				break;
			}
			using (e.Graphics.SaveState())
			{
				e.Graphics.SetClip(e.SubItem.Bounds, CombineMode.Intersect);
				e.DrawText(textFormatFlags);
			}
		}

		private static void DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
			=> ThemeExtensions.ListView_DrawColumnHeader(sender, e); // handles dark text on dark background in Dark Mode

        private static void DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			e.DrawDefault = false;
			e.DrawBackground();
			using (e.Graphics.SaveState())
			{
				e.Graphics.SetClip(e.Item.Bounds, CombineMode.Intersect);
				StyledRenderer.AlphaStyle alphaStyle = StyledRenderer.GetAlphaStyle(e.Item.Selected, hot: false, e.Item.Focused);
				Color selectionColor = StyledRenderer.GetSelectionColor(e.Item.Focused);
				if (alphaStyle != 0)
				{
					e.Graphics.DrawStyledRectangle(e.Bounds.Pad(0, 0, 1, 1), alphaStyle, selectionColor);
				}
			}
		}
	}
}
