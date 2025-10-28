using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class FolderViewItem : ThumbnailViewItem
	{
		public override ThumbnailKey ThumbnailKey => new ThumbnailKey(this, Text, 0, ImageRotation.None);

		public override ItemViewStates GetOwnerDrawnStates(ItemViewMode displayType)
		{
			return ItemViewStates.Selected | ItemViewStates.Hot;
		}

		protected override Size MeasureItem(Graphics graphics, Size defaultSize, ItemViewMode displayType)
		{
			base.MeasureItem(graphics, defaultSize, displayType);
			return defaultSize;
		}

		public override void OnDraw(ItemDrawInformation drawInfo)
		{
			base.OnDraw(drawInfo);
			Rectangle bounds = drawInfo.Bounds;
			Font font = base.View.Font;
			Color foreColor = base.View.ForeColor;
			ThumbnailDrawingOptions thumbnailDrawingOptions = ThumbnailDrawingOptions.EnableShadow | ThumbnailDrawingOptions.EnableBorder | ThumbnailDrawingOptions.EnableRating | ThumbnailDrawingOptions.EnableVerticalBookmarks | ThumbnailDrawingOptions.EnableBackground | ThumbnailDrawingOptions.EnableStates | ThumbnailDrawingOptions.EnableBowShadow;
			Color foreColor2 = (((drawInfo.State & ItemViewStates.Selected) != 0) ? ThemeColors.ThumbnailViewItem.HighlightText : base.View.ForeColor);
			if (base.Selected)
			{
				thumbnailDrawingOptions |= ThumbnailDrawingOptions.Selected;
			}
			if (base.Hot)
			{
				thumbnailDrawingOptions |= ThumbnailDrawingOptions.Hot;
			}
			if (base.Focused)
			{
				thumbnailDrawingOptions |= ThumbnailDrawingOptions.Focused;
			}
			using (IItemLock<ThumbnailImage> itemLock = GetThumbnail(memoryOnly: false))
			{
				ThumbTileRenderer thumbTileRenderer = new ThumbTileRenderer(itemLock?.Item.GetThumbnail(bounds.Height), thumbnailDrawingOptions);
				using (StringFormat format = new StringFormat
				{
					Trimming = StringTrimming.EllipsisPath
				})
				{
					thumbTileRenderer.Font = font;
					thumbTileRenderer.Border = new Size(2, 2);
					thumbTileRenderer.ForeColor = foreColor;
					thumbTileRenderer.BackColor = ThemeColors.ThumbnailViewItem.Back;
                    thumbTileRenderer.SelectionBackColor = StyledRenderer.GetSelectionColor(drawInfo.ControlFocused);
					thumbTileRenderer.TextLines.Add(new TextLine(FileUtility.GetSafeFileName(Text), FC.GetRelative(font, 1.2f, FontStyle.Bold), foreColor2, (StringFormatFlags)0, StringAlignment.Near, 0, 2));
					thumbTileRenderer.TextLines.Add(new TextLine(Text, FC.GetRelative(font, 0.8f), foreColor2, format, 0, 5));
					thumbTileRenderer.DrawTile(drawInfo.Graphics, bounds);
					thumbTileRenderer.DisposeTextLines();
				}
			}
		}

		protected override void CreateThumbnail(ThumbnailKey key)
		{
			Bitmap bmp = GetFolderImage(Text, new Size(341, 512), 3, 4);
			try
			{
				using (Program.ImagePool.Thumbs.AddImage(key, (ImageKey k) => ThumbnailImage.CreateFrom(bmp, bmp.Size)))
				{
				}
			}
			finally
			{
				if (bmp != null)
				{
					((IDisposable)bmp).Dispose();
				}
			}
		}

		private static Bitmap GetFolderImage(string folder, Size sz, int dx, int dy)
		{
			List<Bitmap> list = new List<Bitmap>();
			int count = dx * dy;
			try
			{
				IList<string> list2 = FileUtility.GetFiles(folder, SearchOption.AllDirectories, Providers.Readers.GetFileExtensions().ToArray()).Take(count).ToArray()
					.Randomize();
				foreach (string item in list2)
				{
					ComicBook comicBook = Program.BookFactory.Create(item, CreateBookOption.DoNotAdd);
					if (comicBook != null)
					{
						using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(comicBook))
						{
							list.Add(itemLock.Item.Bitmap.Clone() as Bitmap);
						}
					}
				}
				return list.CreateMosaicImage(dx, sz, Color.Black);
			}
			finally
			{
				list.Dispose();
			}
		}

		public static FolderViewItem Create(string folder)
		{
			return new FolderViewItem
			{
				Text = folder,
				TooltipText = folder
			};
		}
	}
}
