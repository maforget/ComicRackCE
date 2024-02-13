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
	public partial class PagesView : UserControl, IEditBookmark, IEditPage
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
					if (book.ProviderStatus != 0)
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
			ToolStripMenuItem toolStripMenuItem = miPageType;
			bool visible = (tsPageTypeSeparator.Visible = book.Comic.EditMode.CanEditPages());
			toolStripMenuItem.Visible = visible;
			ToolStripMenuItem toolStripMenuItem2 = miSetBookmark;
			ToolStripMenuItem toolStripMenuItem3 = miRemoveBookmark;
			bool flag3 = (tsBookmarkSeparator.Visible = book.Comic.EditMode.CanEditPages());
			visible = (toolStripMenuItem3.Visible = flag3);
			toolStripMenuItem2.Visible = visible;
			ToolStripMenuItem toolStripMenuItem4 = miMoveToTop;
			ToolStripMenuItem toolStripMenuItem5 = miMoveToBottom;
			ToolStripMenuItem toolStripMenuItem6 = miResetOriginalOrder;
			bool flag6 = (tsMovePagesSeparator.Visible = book.Comic.EditMode.CanEditPages());
			flag3 = (toolStripMenuItem6.Visible = flag6);
			visible = (toolStripMenuItem5.Visible = flag3);
			toolStripMenuItem4.Visible = visible;
			miCopy.Enabled = itemView.FocusedItem != null;
			int num = -1;
			foreach (ComicPageInfo selectedPage in GetSelectedPages())
			{
				if (num == -1)
				{
					num = (int)selectedPage.PageType;
				}
				else if (num != (int)selectedPage.PageType)
				{
					num = -1;
					break;
				}
			}
			pageMenu.Value = num;
			int num2 = -1;
			foreach (ComicPageInfo selectedPage2 in GetSelectedPages())
			{
				if (num2 == -1)
				{
					num2 = (int)selectedPage2.Rotation;
				}
				else if (num2 != (int)selectedPage2.Rotation)
				{
					num2 = -1;
					break;
				}
			}
			rotateMenu.Value = num2;
			int num3 = -1;
			foreach (ComicPageInfo selectedPage3 in GetSelectedPages())
			{
				if (num3 == -1)
				{
					num3 = (int)selectedPage3.PagePosition;
				}
				else if (num3 != (int)selectedPage3.PagePosition)
				{
					num3 = -1;
					break;
				}
			}
			miPagePositionDefault.Checked = num3 == 0;
			miPagePositionNear.Checked = num3 == 1;
			miPagePositionFar.Checked = num3 == 2;
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
	}
}
