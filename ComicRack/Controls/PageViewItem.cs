using System.Collections.Generic;
using System.Drawing;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Text;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class PageViewItem : ThumbnailViewItem
	{
		private readonly ComicBookNavigator book;

		private readonly ComicBook comic;

		private readonly int imageIndex;

		private bool isCurrentPage;

		private string key = string.Empty;

		public ComicBookNavigator Book => book;

		public ComicBook Comic => comic;

		public int ImageIndex => imageIndex;

		public ComicPageInfo PageInfo => comic.GetPageByImageIndex(imageIndex);

		public bool IsCurrentPage
		{
			get
			{
				return isCurrentPage;
			}
			set
			{
				if (isCurrentPage != value)
				{
					isCurrentPage = value;
					Update();
				}
			}
		}

		public int Page => comic.TranslateImageIndexToPage(imageIndex);

		public string PageAsText => (Page + 1).ToString();

		public string Key
		{
			get
			{
				return key;
			}
			set
			{
				if (!(key == value))
				{
					key = value;
					Update();
				}
			}
		}

		public override ThumbnailKey ThumbnailKey => book.Comic.GetThumbnailKey(Page);

		public PageViewItem(ComicBookNavigator book, int imageIndex, string key)
		{
			this.book = book;
			comic = book.Comic;
			this.key = key;
			this.imageIndex = imageIndex;
			UpdateInfo();
			comic.BookChanged += comic_BookChanged;
			this.book.Navigation += book_Navigation;
		}

		public PageViewItem(ComicBookNavigator book, int imageIndex)
			: this(book, imageIndex, book.GetImageName(imageIndex))
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				comic.BookChanged -= comic_BookChanged;
				book.Navigation -= book_Navigation;
			}
			base.Dispose(disposing);
		}

		public void SetPageType(ComicPageType pageType)
		{
			book.Comic.UpdatePageType(Page, pageType);
		}

		public void SetPageRotation(ImageRotation rotation)
		{
			book.Comic.UpdatePageRotation(Page, rotation);
		}

		public void SetPagePosition(ComicPagePosition position)
		{
			book.Comic.UpdatePagePosition(Page, position);
		}

		protected override Size GetEstimatedSize(Size canvasSize)
		{
			ComicPageInfo pageInfo = PageInfo;
			Size imageSize = new Size(pageInfo.ImageWidth, pageInfo.ImageHeight);
			if (imageSize.Width <= 0 || imageSize.Height <= 0)
			{
				return base.GetEstimatedSize(canvasSize);
			}
			return ThumbRenderer.GetSafeScaledImageSize(imageSize, canvasSize);
		}

		public override ItemViewStates GetOwnerDrawnStates(ItemViewMode displayType)
		{
			if (displayType == ItemViewMode.Tile)
			{
				return ItemViewStates.Selected | ItemViewStates.Hot;
			}
			return base.GetOwnerDrawnStates(displayType);
		}

		protected override Size MeasureItem(Graphics graphics, Size defaultSize, ItemViewMode displayType)
		{
			defaultSize = base.MeasureItem(graphics, defaultSize, displayType);
			if (displayType == ItemViewMode.Thumbnail)
			{
				return AddBorder(GetThumbnailSizeSafe(defaultSize));
			}
			return defaultSize;
		}

		protected override Size MeasureColumn(Graphics graphics, IColumn header, Size defaultSize)
		{
			defaultSize = base.MeasureColumn(graphics, header, defaultSize);
			ComicListField comicListField = (ComicListField)header.Tag;
			string displayProperty = comicListField.DisplayProperty;
			if (displayProperty == "Thumbnail")
			{
				defaultSize.Width = FormUtility.ScaleDpiX(128);
			}
			else
			{
				string stringValue = GetStringValue(comicListField.DisplayProperty);
				defaultSize = graphics.MeasureString(stringValue, base.View.Font).ToSize();
				defaultSize.Width += 8;
			}
			return defaultSize;
		}

		public override void OnDraw(ItemDrawInformation drawInfo)
		{
			base.OnDraw(drawInfo);
			Color textColor = drawInfo.TextColor;
			Rectangle bounds = drawInfo.Bounds;
			Font font = base.View.Font;
			ComicListField comicListField = ((drawInfo.Header != null) ? (drawInfo.Header.Tag as ComicListField) : null);
			List<Image> list = null;
			if (PageInfo.IsDeleted)
			{
				list = list.SafeAdd(ThumbnailViewItem.DeletedStateImage);
			}
			using (StringFormat stringFormat = new StringFormat())
			{
				using (IItemLock<ThumbnailImage> itemLock = ((comicListField == null || comicListField.DisplayProperty == "Thumbnail") ? GetThumbnail(drawInfo) : null))
				{
					if (itemLock != null)
					{
						Comic.UpdatePageSize(Page, itemLock.Item.OriginalSize.Width, itemLock.Item.OriginalSize.Height);
					}
					int height = ((drawInfo.DisplayType == ItemViewMode.Detail) ? 256 : bounds.Height);
					Image image = itemLock?.Item.GetThumbnail(height);
					ThumbnailDrawingOptions thumbnailDrawingOptions = ThumbnailDrawingOptions.EnableShadow | ThumbnailDrawingOptions.EnableBorder | ThumbnailDrawingOptions.EnableRating | ThumbnailDrawingOptions.EnableVerticalBookmarks | ThumbnailDrawingOptions.EnableBackground | ThumbnailDrawingOptions.EnableStates | ThumbnailDrawingOptions.EnableBowShadow;
					if (base.Selected | IsCurrentPage)
					{
						thumbnailDrawingOptions |= ThumbnailDrawingOptions.Selected;
					}
					if (base.Hot | IsCurrentPage)
					{
						thumbnailDrawingOptions |= ThumbnailDrawingOptions.Hot;
					}
					if (base.View.InScrollOrResize)
					{
						thumbnailDrawingOptions |= ThumbnailDrawingOptions.FastMode;
					}
					switch (drawInfo.DisplayType)
					{
					case ItemViewMode.Thumbnail:
					{
						Animate(image);
						ThumbRenderer thumbRenderer = new ThumbRenderer(image, thumbnailDrawingOptions | ThumbnailDrawingOptions.EnablePageNumber)
						{
							PageNumber = Page + 1,
							ImageOpacity = base.Opacity,
							SelectionBackColor = StyledRenderer.GetSelectionColor(drawInfo.ControlFocused),
							Bookmarked = PageInfo.IsBookmark
						};
						bounds.Inflate(-base.Border.Width, -base.Border.Height);
						if (list != null)
						{
							thumbRenderer.StateImages.AddRange(list);
						}
						thumbRenderer.DrawThumbnail(drawInfo.Graphics, bounds);
						break;
					}
					case ItemViewMode.Tile:
					{
						Animate(image);
						ThumbTileRenderer thumbTileRenderer = new ThumbTileRenderer(image, thumbnailDrawingOptions)
						{
							Font = font,
							Border = base.Border,
							ForeColor = textColor,
							BackColor = ThemeColors.ThumbnailViewItem.Back,
							SelectionBackColor = StyledRenderer.GetSelectionColor(drawInfo.ControlFocused),
							ImageOpacity = base.Opacity,
							Bookmarked = PageInfo.IsBookmark
						};
						if (list != null)
						{
							thumbTileRenderer.StateImages.AddRange(list);
						}
						Font font2 = FC.Get(font, ((float)bounds.Height * 0.07f).Clamp(font.Size * 0.8f, font.Size * 1f));
						thumbTileRenderer.TextLines.AddRange(ComicTextBuilder.GetTextBlocks(PageInfo, Page, font2, textColor, ComicTextElements.DefaultPage));
						thumbTileRenderer.DrawTile(drawInfo.Graphics, bounds);
						thumbTileRenderer.DisposeTextLines();
						break;
					}
					case ItemViewMode.Detail:
						if (comicListField != null)
						{
							string displayProperty = comicListField.DisplayProperty;
							if (displayProperty == "Thumbnail")
							{
								Animate(image);
								if (image != null)
								{
									bounds.Height = bounds.Width * image.Height / image.Width;
									bounds.Inflate(-2, -2);
									drawInfo.Graphics.IntersectClip(bounds);
									if (PageInfo.IsBookmark)
									{
										thumbnailDrawingOptions |= ThumbnailDrawingOptions.Bookmarked;
									}
									ThumbRenderer.DrawThumbnail(drawInfo.Graphics, image, bounds, thumbnailDrawingOptions, null, base.Opacity);
								}
							}
							else
							{
								stringFormat.Alignment = drawInfo.Header.Alignment;
								stringFormat.LineAlignment = StringAlignment.Center;
								stringFormat.Trimming = StringTrimming.EllipsisCharacter;
								stringFormat.FormatFlags = StringFormatFlags.LineLimit;
								string stringValue = GetStringValue(comicListField.DisplayProperty);
								bounds.Inflate(-2, 0);
								using (Brush brush = new SolidBrush(textColor))
								{
									drawInfo.Graphics.DrawString(stringValue, font, brush, bounds, stringFormat);
								}
							}
						}
						else if (drawInfo.GroupItem % 2 == 0 && (drawInfo.State & ItemViewStates.Selected) == 0)
						{
							drawInfo.Graphics.FillRectangle(ThemeBrushes.DetailView.RowHighlight, bounds);
						}
						break;
					}
				}
			}
		}

		protected override void CreateThumbnail(ThumbnailKey key)
		{
			using (Program.ImagePool.GetThumbnail(key, book, book.Comic))
			{
			}
		}

		private void comic_BookChanged(object sender, BookChangedEventArgs e)
		{
			if (e.Page != -1 && comic.TranslatePageToImageIndex(e.Page) == imageIndex)
			{
				UpdateInfo();
				Update(sizeChanged: true);
			}
		}

		private void book_Navigation(object sender, BookPageEventArgs e)
		{
			IsCurrentPage = PageInfo.Equals(e.PageInfo);
			if (IsCurrentPage)
			{
				EnsureVisible();
			}
		}

		private string GetStringValue(string property)
		{
			if (!(property == "Key"))
			{
				if (property == "Page")
				{
					return PageAsText;
				}
				return PageInfo.GetStringValue(property);
			}
			return Key;
		}

		private void UpdateInfo()
		{
			Text = PageAsText;
			base.TooltipText = StringUtility.Format("{0} #{1}", TR.Default["Page", "Page"], Text);
		}
	}
}
