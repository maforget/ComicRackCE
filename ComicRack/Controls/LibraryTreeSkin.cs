using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class LibraryTreeSkin : NiceTreeSkin
	{
		private static readonly Image deviceIcon = Resources.DeviceSync.Scale(8, 8);

		public bool DisableDeviceIcon
		{
			get;
			set;
		}

		public Func<TreeNode, ComicListItem> GetNodeItem
		{
			get;
			set;
		}

		protected override void DrawNodeIcon(TreeViewSkinnerDrawInfo di, Image image, Rectangle bounds)
		{
			base.DrawNodeIcon(di, image, bounds);
			ComicListItem cli = ((GetNodeItem == null) ? (di.Node.Tag as ComicListItem) : GetNodeItem(di.Node));
			if (!DisableDeviceIcon && cli != null && !Program.Settings.Devices.SelectMany((DeviceSyncSettings d) => d.Lists.Where((DeviceSyncSettings.SharedList l) => l.ListId == cli.Id)).IsEmpty())
			{
				di.Graphics.DrawImage(deviceIcon, bounds.X, bounds.Y);
			}
		}

		protected override void DrawNodeLabel(TreeViewSkinnerDrawInfo di)
		{
			ComicListItem comicListItem = ((GetNodeItem == null) ? (di.Node.Tag as ComicListItem) : GetNodeItem(di.Node));
			base.DrawNodeLabel(di);
			if (comicListItem == null || !comicListItem.CacheEnabled || !Program.Settings.DisplayLibraryGauges || (comicListItem.BookCount <= 0 && comicListItem.NewBookCount <= 0 && comicListItem.UnreadBookCount <= 0))
			{
				return;
			}
			Graphics graphics = di.Graphics;
			Font font = FC.Get(di.Font, di.Font.Size * 0.75f);
			Rectangle rc = new Rectangle(di.LabelBounds.Right + 4, di.ItemBounds.Top, di.ItemBounds.Right - di.LabelBounds.Right - 4, di.ItemBounds.Height);
			int num = (Program.Settings.LibraryGaugesFormat.HasFlag(LibraryGauges.Numeric) ? DrawMarkers(graphics, rc, font, comicListItem, 0, onlyMeasure: true) : int.MaxValue);
			if (num < rc.Width)
			{
				DrawMarkers(graphics, rc, font, comicListItem);
				return;
			}
			int num2 = Math.Min(rc.Width / 3, 6);
			if (num2 > 2)
			{
				DrawMarkers(graphics, rc, font, comicListItem, num2);
			}
		}

		public int DrawMarkers(Graphics gr, Rectangle rc, Font font, ComicListItem cli, int totalSize = 0, bool onlyMeasure = false)
		{
			int num = 0;
			bool flag = Program.Settings.LibraryGaugesFormat.HasFlag(LibraryGauges.Total) && cli.BookCount != 0;
			bool flag2 = Program.Settings.LibraryGaugesFormat.HasFlag(LibraryGauges.Unread) && cli.UnreadBookCount != 0;
			bool flag3 = Program.Settings.LibraryGaugesFormat.HasFlag(LibraryGauges.New) && cli.NewBookCount != 0;
			if (flag)
			{
				int num2 = DrawNumberMarker(gr, cli.BookCount, ThemeColors.LibraryTree.TotalBack, ThemeColors.LibraryTree.TotalText, font, rc, !(flag2 && flag3), roundRight: true, onlyMeasure, totalSize);
				rc.Width -= num2;
				num += num2;
			}
			if (flag2)
			{
				int num3 = cli.UnreadBookCount;
				if (!flag3)
				{
					num3 += cli.NewBookCount;
				}
				int num4 = DrawNumberMarker(gr, num3, ThemeColors.LibraryTree.UnreadBack, ThemeColors.LibraryTree.UnreadText, font, rc, !flag3, !flag, onlyMeasure, totalSize);
				rc.Width -= num4;
				num += num4;
			}
			if (flag3)
			{
				int num5 = DrawNumberMarker(gr, cli.NewBookCount, ThemeColors.LibraryTree.NewBack, ThemeColors.LibraryTree.NewText, font, rc, roundLeft: true, !(flag2 && flag), onlyMeasure, totalSize);
				num += num5;
			}
			return num;
		}

		public int DrawNumberMarker(Graphics gr, int n, Color backColor, Color textColor, Font font, Rectangle bounds, bool roundLeft, bool roundRight, bool onlyMeasure, int fixedSize = 0)
		{
			if (n == 0)
			{
				return 0;
			}
			string text = n.ToString();
			int num = ((fixedSize == 0) ? ((int)gr.MeasureString(text, font, 200).Width + 4) : fixedSize);
			Rectangle rectangle = new Rectangle(bounds.Right - num, bounds.Y + 1, num, bounds.Height - 2);
			if (!onlyMeasure)
			{
				int num2 = (roundLeft ? 1 : 0);
				int num3 = (roundRight ? 1 : 0);
				using (Brush brush = new LinearGradientBrush(rectangle, backColor.Transparent(192), backColor.Transparent(128), 270f))
				{
					using (GraphicsPath path = rectangle.ConvertToPath(num2, num2, num3, num3, num3 * 4, num3 * 4, num2, num2 * 2))
					{
						gr.FillPath(brush, path);
					}
				}
				if (fixedSize == 0)
				{
					using (Brush brush2 = new SolidBrush(textColor))
					{
						using (StringFormat format = new StringFormat
						{
							Alignment = StringAlignment.Center,
							LineAlignment = StringAlignment.Center
						})
						{
							gr.DrawString(text, font, brush2, rectangle, format);
						}
					}
				}
			}
			return rectangle.Width + 1;
		}
	}
}
