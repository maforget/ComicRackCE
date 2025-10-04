using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public partial class PagesView : UserControlEx, IEditBookmark, IEditPage
	{
		private volatile bool listDirty;

		private readonly CommandMapper command = new CommandMapper();

		private EnumMenuUtility pageMenu;

		private EnumMenuUtility rotateMenu;

		private bool createBackdrop = true;

		private ComicBookNavigator book;

		private ComicPageType pageFilter = ComicPageType.All;

		private ComicPageInfo[] dragPages;

		private IBitmapCursor dragCursor;

		public ItemView ItemView => itemView;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemViewConfig ViewConfig
		{
			get
			{
				return itemView.ViewConfig;
			}
			set
			{
				int itemRowHeight = itemView.ItemRowHeight;
				itemView.ViewConfig = value;
				itemView.ItemRowHeight = itemRowHeight;
			}
		}

		[DefaultValue(true)]
		public bool CreateBackdrop
		{
			get
			{
				return createBackdrop;
			}
			set
			{
				createBackdrop = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicBookNavigator Book
		{
			get
			{
				return book;
			}
			set
			{
				if (book == value)
				{
					return;
				}
				if (book != null)
				{
					book.IndexOfPageReady -= Book_PageReady;
					book.IndexRetrievalCompleted -= Book_IndexRetrievalCompleted;
				}
				Image backgroundImage = itemView.BackgroundImage;
				itemView.BackgroundImage = null;
				backgroundImage?.Dispose();
				itemView.Items.Clear();
				book = value;
				if (book == null)
				{
					return;
				}
				using (ItemMonitor.Lock(book))
				{
					if (book.ProviderStatus != Engine.IO.Provider.ImageProviderStatus.NotStarted)
					{
						for (int i = 0; i < book.Count; i++)
						{
							ComicPageInfo page = book.Comic.GetPage(i);
							Book_PageReady(book, new BookPageEventArgs(book.Comic, i, i, page, book.GetImageName(page.ImageIndex)));
						}
					}
					book.IndexOfPageReady += Book_PageReady;
					book.IndexRetrievalCompleted += Book_IndexRetrievalCompleted;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicPageType PageFilter
		{
			get
			{
				return pageFilter;
			}
			set
			{
				if (pageFilter != value)
				{
					pageFilter = value;
					UpdateList(now: false);
				}
			}
		}

		public bool CanBookmark
		{
			get
			{
				if (Book != null && Book.Comic != null && Book.Comic.EditMode.CanEditPages())
				{
					return itemView.SelectedCount == 1;
				}
				return false;
			}
		}

		public string BookmarkProposal
		{
			get
			{
				if (!CanBookmark)
				{
					return null;
				}
				if (!string.IsNullOrEmpty(Bookmark))
				{
					return Bookmark;
				}
				return string.Format("{0} {1}", TR.Default["Page", "Page"], itemView.SelectedItems.First().Text);
			}
		}

		public string Bookmark
		{
			get
			{
				if (!CanBookmark)
				{
					return null;
				}
				return GetSelectedPages().First().Bookmark;
			}
			set
			{
				if (CanBookmark)
				{
					int page = Book.Comic.TranslateImageIndexToPage(GetSelectedPages().First().ImageIndex);
					Book.Comic.UpdateBookmark(page, value ?? string.Empty);
				}
			}
		}

		private bool HasValidPages
		{
			get
			{
				if (Book != null && Book.Comic != null && Book.Comic.EditMode.CanEditPages())
				{
					return itemView.SelectedCount > 0;
				}
				return false;
			}
		}

		bool IEditPage.IsValid => HasValidPages;

		ComicPageType IEditPage.PageType
		{
			get
			{
				if (!HasValidPages)
				{
					return ComicPageType.Other;
				}
				return (from pvi in itemView.SelectedItems.OfType<PageViewItem>()
					select pvi.PageInfo).FirstOrDefault().PageType;
			}
			set
			{
				if (HasValidPages)
				{
					GetSelectedPages().ForEach(delegate(ComicPageInfo pi)
					{
						Book.Comic.UpdatePageType(pi, value);
					});
				}
			}
		}

		ImageRotation IEditPage.Rotation
		{
			get
			{
				if (!HasValidPages)
				{
					return ImageRotation.None;
				}
				return (from pvi in itemView.SelectedItems.OfType<PageViewItem>()
					select pvi.PageInfo).FirstOrDefault().Rotation;
			}
			set
			{
				if (HasValidPages)
				{
					GetSelectedPages().ForEach(delegate(ComicPageInfo pi)
					{
						Book.Comic.UpdatePageRotation(pi, value);
					});
				}
			}
		}

		public PagesView()
		{
			InitializeComponent();
			components.Add(command);
			LocalizeUtility.Localize(this, components);
			itemView.ScrollResizeRefresh = Program.ExtendedSettings.OptimizedListScrolling;
			itemView.Columns.Add(new ItemViewColumn(0, "Page", 40, new ComicListField("Page", "The page number of the image"), new PageViewItemPageComparer(), null, visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(1, "Thumbnail", 80, new ComicListField("Thumbnail", "The thumbnail image of the page")));
			itemView.Columns.Add(new ItemViewColumn(2, "Type", 80, new ComicListField("PageTypeAsText", "The type of the page (story, cover, etc.)"), new PageViewItemComparer<ComicPageInfoTypeComparer>()));
			itemView.Columns.Add(new ItemViewColumn(3, "Size", 80, new ComicListField("ImageFileSizeAsText", "Size of the page in bytes"), new PageViewItemComparer<ComicPageInfoImageSizeComparer>(), new PageViewItemGrouper<ComicPageInfoGroupImageSize>(), visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(4, "Width", 60, new ComicListField("ImageWidthAsText", "Width of the page in pixels"), new PageViewItemComparer<ComicPageInfoImageWidthComparer>(), null, visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(5, "Height", 60, new ComicListField("ImageHeightAsText", "Height of the page in pixels"), new PageViewItemComparer<ComicPageInfoImageHeightComparer>(), null, visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(6, "Name", 150, new ComicListField("Key", "Unique key for this page in the Book"), new PageViewItemKeyComparer()));
			itemView.Columns.Add(new ItemViewColumn(7, "Bookmark", 150, new ComicListField("Bookmark", "Bookmark Description"), new PageViewItemComparer<ComicPageBookmarkComparer>(), new PageViewBookmarkGrouper()));
			itemView.Columns.Add(new ItemViewColumn(8, "Rotation", 60, new ComicListField("RotationAsText", "Permanent rotation of this page"), new PageViewItemComparer<ComicPageRotationComparer>(), null, visible: true, StringAlignment.Far));
			itemView.Columns.Add(new ItemViewColumn(9, "Position", 60, new ComicListField("PagePositionAsText", "Layout Position of this page"), new PageViewItemComparer<ComicPagePositionComparer>()));
			foreach (ItemViewColumn column in itemView.Columns)
			{
				column.Width = FormUtility.ScaleDpiX(column.Width);
			}
			itemView.SortColumn = itemView.Columns[0];
			itemView.Font = SystemFonts.IconTitleFont;
			itemView.ItemRowHeight = itemView.Font.Height + FormUtility.ScaleDpiY(6);
			itemView.ItemThumbSize = itemView.ItemThumbSize.ScaleDpi();
			itemView.ColumnHeaderHeight = itemView.ItemRowHeight;
			itemView.MouseWheel += ItemViewMouseWheel;
			IdleProcess.Idle += Application_Idle;
			KeySearch.Create(itemView);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!base.DesignMode)
			{
				ContextMenuStrip autoViewContextMenuStrip = itemView.AutoViewContextMenuStrip;
				autoViewContextMenuStrip.Items.Add(new ToolStripSeparator());
				ToolStripMenuItem toolStripMenuItem;
				autoViewContextMenuStrip.Items.Add(toolStripMenuItem = miSelectAll.Clone());
				command.Add(itemView.SelectAll, miSelectAll, toolStripMenuItem);
				command.Add(itemView.InvertSelection, miInvertSelection);
				command.Add(CopyPage, miCopy);
				command.Add(MergePages, miMergePages);
                command.Add(RefreshDisplay, miRefreshThumbnail);
				command.Add(MarkAsDeleted, miMarkDeleted);
				command.Add(SetBookmark, () => CanBookmark, miSetBookmark);
				command.Add(delegate
				{
					Bookmark = string.Empty;
				}, () => CanBookmark && !string.IsNullOrEmpty(Bookmark), miRemoveBookmark);
				command.Add(MoveSelectedPageStart, () => itemView.SelectedCount > 0, miMoveToTop);
				command.Add(MoveSelectedPageEnd, () => itemView.SelectedCount > 0, miMoveToBottom);
				command.Add(ResetPageOrder, () => itemView.SelectedCount > 0, miResetOriginalOrder);
				command.Add(delegate
				{
					SetPagePosition(ComicPagePosition.Default);
				}, () => itemView.SelectedCount > 0, miPagePositionDefault);
				command.Add(delegate
				{
					SetPagePosition(ComicPagePosition.Near);
				}, () => itemView.SelectedCount > 0, miPagePositionNear);
				command.Add(delegate
				{
					SetPagePosition(ComicPagePosition.Far);
				}, () => itemView.SelectedCount > 0, miPagePositionFar);
				pageMenu = new EnumMenuUtility(miPageType, typeof(ComicPageType), flagsMode: false, null, Keys.A | Keys.Shift | Keys.Alt);
				pageMenu.ValueChanged += PageMenuValueChanged;
				Dictionary<int, Image> dictionary = new Dictionary<int, Image>();
				dictionary.Add(0, Resources.Rotate0Permanent);
				dictionary.Add(1, Resources.Rotate90Permanent);
				dictionary.Add(2, Resources.Rotate180Permanent);
				dictionary.Add(3, Resources.Rotate270Permanent);
				rotateMenu = new EnumMenuUtility(cmPageRotate, typeof(ImageRotation), flagsMode: false, dictionary, Keys.D6 | Keys.Shift | Keys.Alt);
				rotateMenu.ValueChanged += RotateMenuValueChanged;
			}
		}

		private void PageMenuValueChanged(object sender, EventArgs e)
		{
			if (pageMenu.Value == -1)
			{
				return;
			}
			foreach (PageViewItem selectedItem in itemView.SelectedItems)
			{
				selectedItem.SetPageType((ComicPageType)pageMenu.Value);
			}
		}

		private void RotateMenuValueChanged(object sender, EventArgs e)
		{
			if (rotateMenu.Value == -1)
			{
				return;
			}
			foreach (PageViewItem selectedItem in itemView.SelectedItems)
			{
				selectedItem.SetPageRotation((ImageRotation)rotateMenu.Value);
			}
		}

        private void contextPages_Opening(object sender, CancelEventArgs e)
        {
            miMergePages.Visible = false;
            miPageType.Visible = tsPageTypeSeparator.Visible = book.Comic.EditMode.CanEditPages();
            miSetBookmark.Visible = miRemoveBookmark.Visible = tsBookmarkSeparator.Visible = book.Comic.EditMode.CanEditPages();
            miMoveToTop.Visible = miMoveToBottom.Visible = miResetOriginalOrder.Visible = tsMovePagesSeparator.Visible = book.Comic.EditMode.CanEditPages();
            miCopy.Enabled = itemView.FocusedItem != null;

            ComicPageType? pageType = null;
            foreach (ComicPageInfo selectedPage in GetSelectedPages())
            {
                if (pageType is null)
                    pageType = selectedPage.PageType;
                else if (pageType != selectedPage.PageType)
                {
					pageType = null;
                    break;
                }
            }
            pageMenu.Value = pageType.HasValue ? (int)pageType.Value : -1;

            ImageRotation? imageRotation = null;
            foreach (ComicPageInfo selectedPage in GetSelectedPages())
            {
                if (imageRotation == null)
                    imageRotation = selectedPage.Rotation;
                else if (imageRotation != selectedPage.Rotation)
                {
                    imageRotation = null;
                    break;
                }
            }
            rotateMenu.Value = imageRotation.HasValue ? (int)imageRotation.Value : -1;

            ComicPagePosition? pageInfo = null;
            foreach (ComicPageInfo selectedPage in GetSelectedPages())
            {
                if (pageInfo == null)
                    pageInfo = selectedPage.PagePosition;
                else if (pageInfo != selectedPage.PagePosition)
                {
                    pageInfo = null;
                    break;
                }
            }
            miPagePositionDefault.Checked = pageInfo == ComicPagePosition.Default;
            miPagePositionNear.Checked = pageInfo == ComicPagePosition.Near;
            miPagePositionFar.Checked = pageInfo == ComicPagePosition.Far;

            IEnumerable<ComicPageInfo> selectedPages = GetSelectedPages();
            if (selectedPages?.Count() == 2)
                miMergePages.Visible = true;
        }

        private void ItemViewMouseWheel(object sender, MouseEventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Control) != 0 && itemView.ItemViewMode != ItemViewMode.Detail)
			{
				SetItemSize(itemView.ItemThumbSize.Height + e.Delta / SystemInformation.MouseWheelScrollDelta * 16);
			}
		}

		private void Application_Idle(object sender, EventArgs e)
		{
			while (listDirty)
			{
				listDirty = false;
				FillList(book, null);
			}
		}

		private void itemView_PostPaint(object sender, PaintEventArgs e)
		{
			Rectangle displayRectangle = itemView.DisplayRectangle;
			e.Graphics.DrawShadow(displayRectangle, 8, Color.Black, 0.125f, BlurShadowType.Inside, BlurShadowParts.Edges);
		}

		private void Book_PageReady(object sender, BookPageEventArgs e)
		{
			ComicBookNavigator comicBookNavigator = sender as ComicBookNavigator;
			if (e.PageInfo.IsFrontCover && createBackdrop)
			{
				CreateBackground();
			}
			if (e.PageInfo.IsTypeOf(PageFilter))
			{
				PageViewItem pageViewItem = itemView.Items.Cast<PageViewItem>().FirstOrDefault((PageViewItem iv) => iv.ImageIndex == e.PageInfo.ImageIndex);
				if (pageViewItem == null)
				{
					itemView.Items.Add(new PageViewItem(comicBookNavigator, e.PageInfo.ImageIndex, e.PageKey));
				}
				else
				{
					pageViewItem.Key = e.PageKey;
				}
			}
		}

		private void Book_IndexRetrievalCompleted(object sender, EventArgs e)
		{
		}

		private void FillList(ComicBookNavigator nav, IEnumerable<ComicPageInfo> selectedPages)
		{
			itemView.BeginUpdate();
			try
			{
				itemView.Items.Clear();
				try
				{
					int num = (nav.IsIndexRetrievalCompleted ? Math.Max(nav.Count, nav.Comic.PageCount) : nav.Count);
					for (int i = 0; i < num; i++)
					{
						ComicPageInfo cpi = nav.Comic.GetPage(i);
						if (cpi.IsTypeOf(PageFilter))
						{
							PageViewItem pageViewItem = new PageViewItem(nav, cpi.ImageIndex);
							itemView.Items.Add(pageViewItem);
							if (selectedPages != null && selectedPages.FindIndex((ComicPageInfo c) => c.ImageIndex == cpi.ImageIndex) != -1)
							{
								pageViewItem.Selected = true;
							}
						}
					}
				}
				catch
				{
				}
			}
			finally
			{
				itemView.EndUpdate();
			}
		}

		public IEnumerable<PageViewItem> GetItems()
		{
			return itemView.Items.OfType<PageViewItem>();
		}

		public IEnumerable<ComicPageInfo> GetSelectedPages()
		{
			return from PageViewItem pvi in itemView.SelectedItems
				select pvi.PageInfo;
		}

		public void SetItemSize(int height)
		{
			switch (itemView.ItemViewMode)
			{
			case ItemViewMode.Thumbnail:
				height = height.Clamp(FormUtility.ScaleDpiY(Program.MinThumbHeight), FormUtility.ScaleDpiY(Program.MaxThumbHeight));
				itemView.ItemThumbSize = new Size(height, height);
				break;
			case ItemViewMode.Tile:
				height = height.Clamp(FormUtility.ScaleDpiY(Program.MinTileHeight), FormUtility.ScaleDpiY(Program.MaxTileHeight));
				itemView.ItemTileSize = new Size(height * 2, height);
				break;
			case ItemViewMode.Detail:
				height = height.Clamp(FormUtility.ScaleDpiY(Program.MinRowHeight), FormUtility.ScaleDpiY(Program.MaxRowHeight));
				itemView.ItemRowHeight = height;
				break;
			}
		}

		public void CopyPage()
		{
			using (new WaitCursor())
			{
				try
				{
					Clipboard.Clear();
					Clipboard.SetDataObject(CreateDataObjectFromPages(GetSelectedPages()));
				}
				catch (Exception)
				{
				}
			}
		}

		public void UpdateList(bool now)
		{
			if (now)
			{
				FillList(book, null);
			}
			else
			{
				listDirty = true;
			}
		}

		public void RefreshDisplay()
		{
			IViewableItem[] array = itemView.SelectedItems.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				PageViewItem pageViewItem = (PageViewItem)array[i];
				pageViewItem.RefreshImage();
			}
		}

		public void MarkAsDeleted()
		{
			IViewableItem[] array = itemView.SelectedItems.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				PageViewItem pageViewItem = (PageViewItem)array[i];
				pageViewItem.SetPageType((pageViewItem.PageInfo.PageType == ComicPageType.Deleted) ? ComicPageType.Story : ComicPageType.Deleted);
			}
		}

		public void RotatePage(ImageRotation rotation)
		{
			IViewableItem[] array = itemView.SelectedItems.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				PageViewItem pageViewItem = (PageViewItem)array[i];
				pageViewItem.SetPageRotation(rotation);
			}
		}

		public void SetPagePosition(ComicPagePosition position)
		{
			IViewableItem[] array = itemView.SelectedItems.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				PageViewItem pageViewItem = (PageViewItem)array[i];
				pageViewItem.SetPagePosition(position);
			}
		}

		public void MoveSelectedPageStart()
		{
			MovePages(0, GetSelectedPages());
		}

		public void MoveSelectedPageEnd()
		{
			ComicPageInfo[] array = GetSelectedPages().ToArray();
			if (array.All((ComicPageInfo p) => p.PageType == ComicPageType.FrontCover))
			{
				ComicPageInfo[] array2 = array;
				foreach (ComicPageInfo cpi in array2)
				{
					book.Comic.UpdatePageType(cpi, ComicPageType.Other);
				}
				if (array.Length < book.Comic.Pages.Count)
				{
					book.Comic.UpdatePageType(array.Length, ComicPageType.FrontCover);
				}
			}
			MovePages(book.Comic.PageCount, GetSelectedPages());
		}

		public void ResetPageOrder()
		{
			try
			{
				book.Comic.ResetPageSequence();
				FillList(book, GetSelectedPages());
			}
			catch
			{
			}
		}

		private void SetBookmark()
		{
			if (CanBookmark)
			{
				string name = SelectItemDialog.GetName(this, TR.Default["Bookmark", "Bookmark"], BookmarkProposal);
				if (!string.IsNullOrEmpty(name))
				{
					Bookmark = name;
				}
			}
		}

		private void MovePages(int position, IEnumerable<ComicPageInfo> pages)
		{
			try
			{
				ComicInfo comic = book.Comic;
				comic.MovePages(position, pages);
				FillList(book, pages);
			}
			catch
			{
			}
		}

		private void CreateBackground()
		{
			try
			{
				Image backgroundImage = itemView.BackgroundImage;
				itemView.BackgroundImage = null;
				backgroundImage?.Dispose();
				ComicBookNavigator newNav = book;
				Bitmap bmp = default(Bitmap);
				ThreadUtility.RunInBackground("Create pages backdrop", delegate
				{
					try
					{
						if (!base.IsDisposed && newNav != null && !newNav.IsDisposed)
						{
							using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(newNav.Comic.GetFrontCoverThumbnailKey(), newNav, onErrorThrowException: true))
							{
								bmp = ComicBox3D.CreateDefaultBook(itemLock.Item.Bitmap, null, EngineConfiguration.Default.ListCoverSize.ScaleDpi(), newNav.Comic.PageCount);
								bmp.ChangeAlpha(EngineConfiguration.Default.ListCoverAlpha);
								itemView.BeginInvoke(delegate
								{
									Image backgroundImage2 = itemView.BackgroundImage;
									itemView.BackgroundImage = bmp;
									backgroundImage2.SafeDispose();
								});
							}
						}
					}
					catch
					{
					}
				});
			}
			catch
			{
			}
		}

		private bool IsPageSorted()
		{
			if (itemView.SortColumn != null && itemView.SortColumn.Id == 0 && itemView.ItemSortOrder == SortOrder.Ascending)
			{
				return itemView.GroupColumn == null;
			}
			return false;
		}

		private void itemView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (itemView.SelectedItems.IsEmpty())
			{
				return;
			}
			dragCursor = itemView.GetDragCursor(Program.ExtendedSettings.DragDropCursorAlpha);
			bool flag = IsPageSorted();
			try
			{
				dragPages = GetSelectedPages().ToArray();
				itemView.AllowDrop = true;
				itemView.DoDragDrop(CreateDataObjectFromPages(dragPages), (!flag) ? DragDropEffects.Copy : (DragDropEffects.Copy | DragDropEffects.Move));
			}
			finally
			{
				itemView.AllowDrop = false;
				if (dragCursor != null)
				{
					dragCursor.Dispose();
				}
				dragCursor = null;
				dragPages = null;
			}
		}

		private DataObjectEx CreateDataObjectFromPages(IEnumerable<ComicPageInfo> dragPages)
		{
			DataObjectEx dataObjectEx = new DataObjectEx();
			ComicBook comic = Book.Comic;
			foreach (ComicPageInfo dragPage in dragPages)
			{
				int num = comic.TranslateImageIndexToPage(dragPage.ImageIndex);
				string fileName = FileUtility.MakeValidFilename(StringUtility.Format("{0} - {1} {2}.jpg", comic.Caption, TR.Default["Page", "Page"], num + 1));
				PageKey key = Book.GetPageKey(num);
				dataObjectEx.SetFile(fileName, delegate(Stream s)
				{
					try
					{
						using (IItemLock<PageImage> itemLock2 = Program.ImagePool.GetPage(key, comic))
						{
							itemLock2.Item.Bitmap.Save(s, ImageFormat.Jpeg);
						}
					}
					catch
					{
					}
				});
			}
			int page = comic.TranslateImageIndexToPage(dragPages.First().ImageIndex);
			using (IItemLock<PageImage> itemLock = Program.ImagePool.GetPage(book.GetPageKey(page), Book))
			{
				if (itemLock != null)
				{
					if (itemLock.Item != null)
					{
						if (itemLock.Item.Bitmap != null)
						{
							dataObjectEx.SetImage(itemLock.Item.Bitmap);
							return dataObjectEx;
						}
						return dataObjectEx;
					}
					return dataObjectEx;
				}
				return dataObjectEx;
			}
		}

		private void itemView_DragDrop(object sender, DragEventArgs e)
		{
			if (itemView.MarkerItem != null)
			{
				MovePages(((PageViewItem)itemView.MarkerItem).Page, dragPages);
			}
		}

		private void itemView_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = ((dragPages != null && IsPageSorted()) ? DragDropEffects.Move : DragDropEffects.None);
		}

		private void itemView_DragLeave(object sender, EventArgs e)
		{
			itemView.MarkerVisible = false;
		}

		private void itemView_DragOver(object sender, DragEventArgs e)
		{
			Point pt = itemView.PointToClient(new Point(e.X, e.Y));
			itemView.MarkerItem = itemView.ItemHitTest(pt) as PageViewItem;
			itemView.MarkerVisible = e.Effect == DragDropEffects.Move;
		}

		private void itemView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			if (dragCursor != null && !(dragCursor.Cursor == null))
			{
				e.UseDefaultCursors = false;
				dragCursor.OverlayCursor = ((e.Effect == DragDropEffects.None) ? Cursors.No : Cursors.Default);
				dragCursor.OverlayEffect = ((e.Effect == DragDropEffects.Copy) ? BitmapCursorOverlayEffect.Plus : BitmapCursorOverlayEffect.None);
				Cursor.Current = dragCursor.Cursor;
			}
		}

        private void MergePages()
        {
			//If any other quantity than 2 or any of the pages are marked as Deleted, abort
			List<PageViewItem> selectedPages = itemView.SelectedItems.Cast<PageViewItem>().ToList();
            if (selectedPages?.Count() != 2 && selectedPages.Any((PageViewItem p) => p.PageInfo.PageType == ComicPageType.Deleted))
				return;

            //Check the reading direction to determine the first and second page.
            bool reversed = this.Book.RightToLeftReading == YesNo.Yes || this.Book.Comic.Manga == MangaYesNo.YesAndRightToLeft;
			//If the sort order is descending the pages will be reversed already, so no need to reverse the order
			reversed = reversed && itemView.ItemSorter != null && itemView.ItemSortOrder == SortOrder.Descending ? false : reversed;
            PageViewItem firstPage = (reversed ? selectedPages[^1] : selectedPages[0]);
            PageViewItem secondPage = (reversed ? selectedPages[0] : selectedPages[^1]);

			//Get the bitmaps and merge them
			int firstImageIndex = firstPage.ImageIndex;
			Bitmap firstImage = book.GetImage(firstImageIndex);
			Bitmap secondImage = book.GetImage(secondPage.ImageIndex);
            Bitmap mergedImage = BitmapExtensions.Merge(firstImage, secondImage);

            //Get the PageKey (includes color ajustements) & ThumbnailKey for future reference
            PageKey pageKey = Book.GetPageKey(firstImageIndex);
			ThumbnailKey thumbKey = Book.GetThumbnailKey(firstImageIndex);

			//Remove the original cached version 
            Program.ImagePool.Pages.RefreshImage(pageKey);
            Program.ImagePool.Thumbs.RefreshImage(thumbKey);

            //Add pages to the ImagePool because if they aren't the Export will retrieve the original image instead
            using IItemLock<PageImage> itemLock = Program.ImagePool.Pages.AddImage(pageKey, p => PageImage.CreateFromMerged(mergedImage));
            using IItemLock<ThumbnailImage> itemLock2 = Program.ImagePool.Thumbs.AddImage(thumbKey, p => ThumbnailImage.CreateFrom(mergedImage, mergedImage.Size));

			//Mark the second image as Deleted
			secondPage.SetPageType(ComicPageType.Deleted);

			//Update UI
			UpdateList(true);//will update the pages lists so if Deleted are uncheck, they will dissapear
			RefreshDisplay();
		}
    }
}
