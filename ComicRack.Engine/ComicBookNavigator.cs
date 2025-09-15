using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookNavigator : DisposableObject, IImageProvider, IDisposable, IImageKeyProvider
	{
		public static bool TrackCurrentPage = true;

		private int initialPage;

		private ImageProvider provider;

		private readonly ComicBook comic;

		private BitmapAdjustment baseColorAdjustment = BitmapAdjustment.Empty;

		private ComicPageType pageFilter = ComicPageType.All;

		private ImagePartInfo pagePart = ImagePartInfo.Empty;

		private YesNo rightToLeftReading = YesNo.Unknown;

		private volatile bool updateCurrentPageEnabled = true;

		private volatile Bitmap thumbnail;

		private volatile int currentPage;

		private volatile int lastPageRead;

		public ComicBook Comic => comic;

		public string Source => Comic.FilePath;

		public string Caption => Comic.Caption;

		public int ProviderPageCount
		{
			get
			{
				CheckDisposed();
				return provider.Count;
			}
		}

		public BitmapAdjustment BaseColorAdjustment
		{
			get
			{
				CheckDisposed();
				using (ItemMonitor.Lock(this))
				{
					return baseColorAdjustment;
				}
			}
			set
			{
				CheckDisposed();
				using (ItemMonitor.Lock(this))
				{
					if (baseColorAdjustment == value)
					{
						return;
					}
					baseColorAdjustment = value;
				}
				OnColorAdjustmentChanged();
			}
		}

		public BitmapAdjustment ColorAdjustment
		{
			get
			{
				CheckDisposed();
				return BitmapAdjustment.Add(BaseColorAdjustment, Comic.ColorAdjustment);
			}
		}

		public ComicPageType PageFilter
		{
			get
			{
				CheckDisposed();
				return pageFilter;
			}
			set
			{
				CheckDisposed();
				if (pageFilter != value)
				{
					pageFilter = value;
					OnPageFilterChanged();
				}
			}
		}

		public ImagePartInfo PagePart
		{
			get
			{
				return pagePart;
			}
			set
			{
				pagePart = value;
			}
		}

		[DefaultValue(false)]
		public YesNo RightToLeftReading
		{
			get
			{
				return rightToLeftReading;
			}
			set
			{
				if (rightToLeftReading != value)
				{
					rightToLeftReading = value;
					OnRightToLeftReadingChanged();
				}
			}
		}

		[DefaultValue(true)]
		public bool UpdateCurrentPageEnabled
		{
			get
			{
				return updateCurrentPageEnabled;
			}
			set
			{
				updateCurrentPageEnabled = value;
			}
		}

		public Bitmap Thumbnail
		{
			get
			{
				return thumbnail;
			}
			set
			{
				if (value != thumbnail)
				{
					Bitmap obj = thumbnail;
					thumbnail = value;
					obj.SafeDispose();
				}
			}
		}

		public int IndexPagesRetrieved
		{
			get;
			private set;
		}

		public ImageProviderStatus ProviderStatus
		{
			get
			{
				CheckDisposed();
				return provider.Status;
			}
		}

		public bool IsIndexRetrievalCompleted
		{
			get
			{
				CheckDisposed();
				return provider.IsIndexRetrievalCompleted;
			}
		}

		public int CurrentPage
		{
			get
			{
				return currentPage;
			}
			set
			{
                //This sets the CurrentPage to the last page if the value exceeds the total number of pages.
                //HACK: Added a check to prevent this adjustment when the book contains only one page, ensuring it is marked as read.
                if (IsIndexRetrievalCompleted && ProviderPageCount != 1 && value >= ProviderPageCount)
				{
					value = ProviderPageCount - 1;
				}
				if (value < 0)
				{
					value = 0;
				}
				int oldPage = currentPage;
				if (value != currentPage)
				{
					currentPage = value;
					if (updateCurrentPageEnabled && TrackCurrentPage)
					{
						Comic.CurrentPage = CurrentPage;
					}
					if (this.Navigation != null)
					{
						this.Navigation(this, new BookPageEventArgs(Comic, oldPage, CurrentPage, CurrentPageInfo, CurrentPageName));
					}
				}
			}
		}

		public int LastPageRead
		{
			get
			{
				return lastPageRead;
			}
			set
			{
				if (value != LastPageRead)
				{
					lastPageRead = value;
					if (value != LastPageRead && updateCurrentPageEnabled && TrackCurrentPage)
					{
						Comic.LastPageRead = lastPageRead;
					}
				}
			}
		}

		public int NextPage => SeekNextPage(CurrentPage, 1, 1);

		public ComicPageInfo CurrentPageInfo => Comic.GetPage(CurrentPage);

		public string CurrentPageAsText => string.Format("{0} {1}", TR.Default["Page", "Page"], CurrentPage + 1);

		public string CurrentPageName
		{
			get
			{
				try
				{
					return GetImageInfo(CurrentPageInfo.ImageIndex).Name;
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		public int Count => ProviderPageCount;

		public bool IsSlow
		{
			get
			{
				if (provider != null)
				{
					return provider.IsSlow;
				}
				return false;
			}
		}

		public event EventHandler IndexRetrievalStarted;

		public event EventHandler<BookPageEventArgs> IndexOfPageReady;

		public event EventHandler IndexRetrievalCompleted;

		public event EventHandler<BookPageEventArgs> Navigation;

		public event EventHandler ErrorOpening;

		public event EventHandler Opened;

		public event EventHandler ColorAdjustmentChanged;

		public event EventHandler RightToLeftReadingChanged;

		public event EventHandler PageFilterChanged;

		public event EventHandler PagesChanged;

		public ComicBookNavigator(ComicBook comic)
		{
			this.comic = comic;
			provider = comic.CreateImageProvider();
			if (provider == null)
			{
				throw new ArgumentException("No valid comic book");
			}
			provider.ImageReady += ProviderImageIndexReady;
			provider.IndexRetrievalCompleted += ProviderIndexRetrievalCompleted;
			this.comic.FileRenamed += ComicFileRenamed;
			this.comic.BookChanged += ComicBookChanged;
		}

		public void Open(bool async, int initialPage)
		{
			try
			{
				OnIndexRetrievalStarted();
				provider.Open(Source, async);
				this.initialPage = initialPage;
			}
			catch (Exception)
			{
				OnIndexRetrievalCompleted();
				OnErrorOpening();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Thumbnail = null;
				comic.FileRenamed -= ComicFileRenamed;
				comic.BookChanged -= ComicBookChanged;
				if (provider != null)
				{
					provider.ImageReady -= ProviderImageIndexReady;
					provider.IndexRetrievalCompleted -= ProviderIndexRetrievalCompleted;
					IDisposable disposable = provider;
					ThreadUtility.RunInBackground("Background Provider Dispose", disposable.Dispose);
				}
				provider = null;
			}
			base.Dispose(disposing);
		}

		public string GetImageName(int imageIndex)
		{
			ProviderImageInfo imageInfo = GetImageInfo(imageIndex);
			if (imageInfo != null && imageInfo.Name != null)
			{
				return imageInfo.Name;
			}
			return string.Empty;
		}

		public string GetImageName(int imageIndex, bool noPath)
		{
			string text = GetImageName(imageIndex);
			if (!string.IsNullOrEmpty(text) && noPath)
			{
				text = Path.GetFileName(text);
			}
			return text;
		}

		public int SeekNextPage(int page, int count, int direction, bool noFilter = false)
		{
			int num = Math.Abs(count);
			int num2 = CurrentPage;
			while (page >= 0 && page < Comic.PageCount)
			{
				if (noFilter || comic.GetPage(page).IsTypeOf(pageFilter) || page == num2)
				{
					if (num == 0)
					{
						return page;
					}
					num--;
				}
				if (direction == 0)
				{
					break;
				}
				page += direction;
			}
			return -1;
		}

		public int SeekNewPage(int offset, PageSeekOrigin pageSeekOrigin, bool noFilter = false)
		{
			int count = Math.Abs(offset);
			int page;
			int direction;
			switch (pageSeekOrigin)
			{
			default:
				page = 0;
				direction = 1;
				break;
			case PageSeekOrigin.Current:
				page = CurrentPage;
				direction = ((offset == 0) ? 1 : Math.Sign(offset));
				break;
			case PageSeekOrigin.End:
				page = ProviderPageCount - 1;
				direction = -1;
				break;
			case PageSeekOrigin.Absolute:
				return offset.Clamp(0, comic.PageCount - 1);
			}
			return SeekNextPage(page, count, direction, noFilter);
		}

		public ComicPageInfo GetPageInfo(int offset, PageSeekOrigin pageSeekOrigin)
		{
			return Comic.GetPage(SeekNewPage(offset, pageSeekOrigin));
		}

		public IEnumerable<int> GetPages(bool noFilter = false)
		{
			for (int p = SeekNextPage(0, 0, 1, noFilter); p != -1; p = SeekNextPage(p, 1, 1, noFilter))
			{
				yield return p;
			}
		}

		public IEnumerable<ComicPageInfo> GetPageInfos(bool noFilter = false)
		{
			return (from page in GetPages(noFilter)
				select Comic.GetPage(page)).ToArray();
		}

		public bool Navigate(int offset, PageSeekOrigin pageSeekOrigin, bool noFilter)
		{
			int num = SeekNewPage(offset, pageSeekOrigin, noFilter);
			if (num == -1)
			{
                //HACK: If a book contains only one page, set the CurrentPage to 1 (Page 2), so it is marked as read
                if (ProviderPageCount == 1)
                    CurrentPage = 1;

                return false;
			}
			CurrentPage = num;
			return true;
		}

		public bool Navigate(int offset, PageSeekOrigin pageSeekOrigin)
		{
			return Navigate(offset, pageSeekOrigin, noFilter: false);
		}

		public bool Navigate(int offset)
		{
			return Navigate(offset, PageSeekOrigin.Current);
		}

		public bool Navigate(PageSeekOrigin pageSeekOrigin)
		{
			return Navigate(0, pageSeekOrigin);
		}

		public bool CanNavigate(int offset, PageSeekOrigin pageSeekOrigin)
		{
			return SeekNewPage(offset, pageSeekOrigin) != -1;
		}

		public bool CanNavigate(int offset)
		{
			return CanNavigate(offset, PageSeekOrigin.Current);
		}

		public int SeekBookmark(int page, int count)
		{
			int num = Math.Sign(count);
			count = Math.Abs(count);
			while (count-- > 0 && (page = Comic.Pages.SeekBookmark(page + num, num)) != -1)
			{
			}
			return page;
		}

		public bool NavigateBookmark(int count)
		{
			int num = SeekBookmark(CurrentPage, count);
			bool flag = num != -1;
			if (flag)
			{
				CurrentPage = num;
			}
			return flag;
		}

		public bool CanNavigateBookmark(int count)
		{
			return SeekBookmark(CurrentPage, count) != -1;
		}

		protected virtual void OnPageFilterChanged()
		{
			if (!base.IsDisposed && this.PageFilterChanged != null)
			{
				this.PageFilterChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnRightToLeftReadingChanged()
		{
			if (!base.IsDisposed && this.RightToLeftReadingChanged != null)
			{
				this.RightToLeftReadingChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnColorAdjustmentChanged()
		{
			if (!base.IsDisposed && this.ColorAdjustmentChanged != null)
			{
				this.ColorAdjustmentChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnPagesChanged()
		{
			if (!base.IsDisposed && this.PagesChanged != null)
			{
				this.PagesChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnIndexRetrievalStarted()
		{
			if (!base.IsDisposed && this.IndexRetrievalStarted != null)
			{
				this.IndexRetrievalStarted(this, EventArgs.Empty);
			}
		}

		protected virtual void OnIndexRetrievalCompleted()
		{
			if (!base.IsDisposed && this.IndexRetrievalCompleted != null)
			{
				this.IndexRetrievalCompleted(this, EventArgs.Empty);
			}
		}

		protected virtual void OnIndexOfPageReady(BookPageEventArgs bpea)
		{
			if (!base.IsDisposed && this.IndexOfPageReady != null)
			{
				this.IndexOfPageReady(this, bpea);
			}
		}

		protected virtual void OnErrorOpening()
		{
			if (!base.IsDisposed && this.ErrorOpening != null)
			{
				this.ErrorOpening(this, EventArgs.Empty);
			}
		}

		protected virtual void OnOpened()
		{
			if (!base.IsDisposed && this.Opened != null)
			{
				this.Opened(this, EventArgs.Empty);
			}
		}

		private void ProviderImageIndexReady(object sender, ImageIndexReadyEventArgs e)
		{
			if (!base.IsDisposed)
			{
				int num = comic.TranslateImageIndexToPage(e.ImageNumber);
				ComicPageInfo page = comic.GetPage(num);
				IndexPagesRetrieved = num;
				comic.PageCount = Math.Max(comic.PageCount, num);
				if (e.ImageInfo.Size > 0)
				{
					comic.UpdatePageFileSize(num, (int)e.ImageInfo.Size);
				}
				if (!string.IsNullOrEmpty(e.ImageInfo.Name) && page.IsFrontCover && !ComicInfo.IsValidCoverKey(e.ImageInfo.Name))
				{
					comic.UpdatePageType(num, ComicPageType.Other);
				}
				OnIndexOfPageReady(new BookPageEventArgs(Comic, num, num, comic.GetPage(num), e.ImageInfo.Name));
				if (initialPage != 0 && initialPage == num)
				{
					CurrentPage = initialPage;
				}
			}
		}

		private void ProviderIndexRetrievalCompleted(object sender, IndexRetrievalCompletedEventArgs e)
		{
			if (base.IsDisposed)
			{
				return;
			}
			OnIndexRetrievalCompleted();
			if (e.Status != ImageProviderStatus.Error)
			{
				Comic.PageCount = ProviderPageCount;
				Comic.TrimExcessPageInfo();
				if (CurrentPage >= ProviderPageCount)
				{
					CurrentPage = ProviderPageCount - 1;
				}
				LastPageRead = Comic.LastPageRead;
				if (LastPageRead >= ProviderPageCount)
				{
					LastPageRead = ProviderPageCount - 1;
				}
				OnOpened();
			}
			else
			{
				OnErrorOpening();
			}
		}

		private void ComicFileRenamed(object sender, ComicBookFileRenameEventArgs e)
		{
			if (!base.IsDisposed)
			{
				provider.ChangeSourceLocation(e.NewFile);
			}
		}

		private void ComicBookChanged(object sender, PropertyChangedEventArgs e)
		{
			if (base.IsDisposed)
			{
				return;
			}
			string propertyName = e.PropertyName;
			if (!(propertyName == "ColorAdjustment"))
			{
				if (propertyName == "Pages")
				{
					OnPagesChanged();
				}
			}
			else
			{
				OnColorAdjustmentChanged();
			}
		}

		public Bitmap GetImage(int index)
		{
			if (provider == null)
			{
				return null;
			}
			return provider.GetImage(index);
		}

		public byte[] GetByteImage(int index)
		{
			if (provider == null)
			{
				return null;
			}
			return provider.GetByteImage(index);
		}

		public ExportImageContainer GetByteImageForExport(int index)
		{
			if (provider == null)
			{
				return null;
			}
			return provider.GetByteImageForExport(index);
		}

		public ProviderImageInfo GetImageInfo(int index)
		{
			if (provider == null)
			{
				return null;
			}
			return provider.GetImageInfo(index);
		}

		public ThumbnailImage GetThumbnail(int index)
		{
			return provider.GetThumbnail(index);
		}

		public PageKey GetPageKey(int page)
		{
			return GetPageKey(page, ColorAdjustment);
		}

		public PageKey GetPageKey()
		{
			return GetPageKey(CurrentPage);
		}

		public PageKey GetPageKey(int page, BitmapAdjustment colorAdjustment)
		{
			return Comic.GetPageKey(page, colorAdjustment);
		}

		public ThumbnailKey GetThumbnailKey(int page)
		{
			return Comic.GetThumbnailKey(page);
		}

		public ImageKey GetImageKey(int page)
		{
			return GetPageKey(page);
		}
	}
}
