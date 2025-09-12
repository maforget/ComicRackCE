using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Runtime;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.IO.Cache;

namespace cYo.Projects.ComicRack.Engine.Display
{
	public class ComicDisplay : DisposableObject, IComicDisplay, IComicDisplayConfig, IEditBookmark, IGetBookList, IRefreshDisplay, IEditPage
	{
		[Flags]
		public enum PagingMode
		{
			None = 0x0,
			Double = 0x1,
			Walled = 0x2
		}

		private enum WallState
		{
			Initial,
			Pending
		}

		public const int DefaultPageWallTicks = 300;

		public const int AnimationTime = 300;

		private readonly IComicDisplay display;

		private ContainerControl control;

		private float scrollLines = 1f;

		private KeySearch pageKeys;

		private long lastPartNavigation;

		private long transitionStart;

		private WallState wallState;

		private long lastPaging;

		private bool fullScreen;

		private Rectangle orgRect = Rectangle.Empty;

		private FormBorderStyle borderStyleOld = FormBorderStyle.Fixed3D;

		private FormWindowState oldFormState;

		private bool hideCursorFullScreen = true;

		private KeyboardShortcuts keyboardMap = new KeyboardShortcuts();

		private long pageWallTicks = 300L;

		private bool scrollingDoesBrowse = true;

		private bool autoScrolling = true;

		private ComicPageType pageFilter = ComicPageType.All;

		private volatile bool mouseClickEnabled = true;

		private volatile bool resetZoomOnPageChange;

		private volatile bool zoomInOutOnPageChange;

		private float oldZoom;

		private float mouseWheelSpeed = 2f;

		public bool TwoPageDisplay => PageLayout != PageLayoutMode.Single;

		public bool SupressContextMenu
		{
			get;
			set;
		}

		public bool FullScreen
		{
			get
			{
				return fullScreen;
			}
			set
			{
				if (control == null)
				{
					return;
				}
				Form parentForm = control.ParentForm;
				if (parentForm != null && fullScreen != value)
				{
					fullScreen = value;
					if (fullScreen)
					{
						oldFormState = parentForm.WindowState;
						borderStyleOld = parentForm.FormBorderStyle;
						parentForm.WindowState = FormWindowState.Normal;
						parentForm.FormBorderStyle = FormBorderStyle.None;
						orgRect = parentForm.Bounds;
						parentForm.Bounds = Screen.GetBounds(parentForm);
						parentForm.TopMost = true;
						display.AutoHideCursor = HideCursorFullScreen;
					}
					else
					{
						parentForm.TopMost = false;
						parentForm.Bounds = orgRect;
						parentForm.FormBorderStyle = borderStyleOld;
						parentForm.WindowState = oldFormState;
						display.AutoHideCursor = false;
					}
					OnFullScreenChanged();
				}
			}
		}

		public bool HideCursorFullScreen
		{
			get
			{
				return hideCursorFullScreen;
			}
			set
			{
				hideCursorFullScreen = value;
			}
		}

		public KeyboardShortcuts KeyboardMap
		{
			get
			{
				return keyboardMap;
			}
			set
			{
				keyboardMap = value;
			}
		}

		public long PageWallTicks
		{
			get
			{
				return pageWallTicks;
			}
			set
			{
				pageWallTicks = value;
			}
		}

		public bool ScrollingDoesBrowse
		{
			get
			{
				return scrollingDoesBrowse;
			}
			set
			{
				scrollingDoesBrowse = value;
			}
		}

		public bool AutoScrolling
		{
			get
			{
				return autoScrolling;
			}
			set
			{
				autoScrolling = value;
			}
		}

		public ComicPageType PageFilter
		{
			get
			{
				return pageFilter;
			}
			set
			{
				pageFilter = value;
				if (Book != null)
				{
					Book.PageFilter = value;
				}
			}
		}

		public bool MouseClickEnabled
		{
			get
			{
				return mouseClickEnabled;
			}
			set
			{
				mouseClickEnabled = value;
			}
		}

		public bool ResetZoomOnPageChange
		{
			get
			{
				return resetZoomOnPageChange;
			}
			set
			{
				resetZoomOnPageChange = value;
			}
		}

		public bool ZoomInOutOnPageChange
		{
			get
			{
				return zoomInOutOnPageChange;
			}
			set
			{
				zoomInOutOnPageChange = value;
			}
		}

		public float MouseWheelSpeed
		{
			get
			{
				return mouseWheelSpeed;
			}
			set
			{
				mouseWheelSpeed = value;
			}
		}

		public InfoOverlays VisibleInfoOverlays
		{
			get
			{
				return display.VisibleInfoOverlays;
			}
			set
			{
				display.VisibleInfoOverlays = value;
			}
		}

		public int ImagePartCount => display.ImagePartCount;

		public bool NavigationOverlayVisible
		{
			get
			{
				return display.NavigationOverlayVisible;
			}
			set
			{
				display.NavigationOverlayVisible = value;
			}
		}

		public int CurrentPage => display.CurrentPage;

		public int CurrentMousePage => display.CurrentMousePage;

		public ImageRotation CurrentImageRotation => display.CurrentImageRotation;

		public ComicBookNavigator Book
		{
			get
			{
				return display.Book;
			}
			set
			{
				display.Book = value;
			}
		}

		public bool IsValid => display.IsValid;

		public bool ShouldPagingBlend => display.ShouldPagingBlend;

		public bool IsDoubleImage => display.IsDoubleImage;

		public Size ImageSize => display.ImageSize;

		public ImagePartInfo ImageVisiblePart
		{
			get
			{
				return display.ImageVisiblePart;
			}
			set
			{
				display.ImageVisiblePart = value;
			}
		}

		public IPagePool PagePool
		{
			get
			{
				return display.PagePool;
			}
			set
			{
				display.PagePool = value;
			}
		}

		public IThumbnailPool ThumbnailPool
		{
			get
			{
				return display.ThumbnailPool;
			}
			set
			{
				display.ThumbnailPool = value;
			}
		}

		public bool IsHardwareRenderer => display.IsHardwareRenderer;

		public float DoublePageOverlap
		{
			get
			{
				return display.DoublePageOverlap;
			}
			set
			{
				display.DoublePageOverlap = value;
			}
		}

		public bool CanBookmark
		{
			get
			{
				if (Book != null && Book.Comic != null)
				{
					return Book.Comic.EditMode.CanEditPages();
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
					return string.Empty;
				}
				if (!string.IsNullOrEmpty(Bookmark))
				{
					return Bookmark;
				}
				return Book.CurrentPageAsText;
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
				return Book.CurrentPageInfo.Bookmark;
			}
			set
			{
				if (CanBookmark)
				{
					Book.Comic.UpdateBookmark(Book.CurrentPage, value);
				}
			}
		}

		public float MagnifierZoom
		{
			get
			{
				return display.MagnifierZoom;
			}
			set
			{
				display.MagnifierZoom = value;
			}
		}

		public float MagnifierOpacity
		{
			get
			{
				return display.MagnifierOpacity;
			}
			set
			{
				display.MagnifierOpacity = value;
			}
		}

		public Size MagnifierSize
		{
			get
			{
				return display.MagnifierSize;
			}
			set
			{
				display.MagnifierSize = value;
			}
		}

		public bool AutoHideMagnifier
		{
			get
			{
				return display.AutoHideMagnifier;
			}
			set
			{
				display.AutoHideMagnifier = value;
			}
		}

		public bool AutoMagnifier
		{
			get
			{
				return display.AutoMagnifier;
			}
			set
			{
				display.AutoMagnifier = value;
			}
		}

		public ImageDisplayOptions ImageDisplayOptions
		{
			get
			{
				return display.ImageDisplayOptions;
			}
			set
			{
				display.ImageDisplayOptions = value;
			}
		}

		public bool PageMargin
		{
			get
			{
				return display.PageMargin;
			}
			set
			{
				if (display.PageMargin != value)
				{
					Animate((Action)delegate
					{
						display.PageMargin = value;
					});
				}
			}
		}

		public float PageMarginPercentWidth
		{
			get
			{
				return display.PageMarginPercentWidth;
			}
			set
			{
				if (display.PageMarginPercentWidth != value)
				{
					Animate((Action)delegate
					{
						display.PageMarginPercentWidth = value;
					});
				}
			}
		}

		public Color BackColor
		{
			get
			{
				return display.BackColor;
			}
			set
			{
				display.BackColor = value;
			}
		}

		public string BackgroundTexture
		{
			get
			{
				return display.BackgroundTexture;
			}
			set
			{
				display.BackgroundTexture = value;
			}
		}

		public string PaperTexture
		{
			get
			{
				return display.PaperTexture;
			}
			set
			{
				display.PaperTexture = value;
			}
		}

		public float PaperTextureStrength
		{
			get
			{
				return display.PaperTextureStrength;
			}
			set
			{
				display.PaperTextureStrength = value;
			}
		}

		public ImageLayout PaperTextureLayout
		{
			get
			{
				return display.PaperTextureLayout;
			}
			set
			{
				display.PaperTextureLayout = value;
			}
		}

		public ImageLayout BackgroundImageLayout
		{
			get
			{
				return display.BackgroundImageLayout;
			}
			set
			{
				display.BackgroundImageLayout = value;
			}
		}

		public ImageBackgroundMode ImageBackgroundMode
		{
			get
			{
				return display.ImageBackgroundMode;
			}
			set
			{
				display.ImageBackgroundMode = value;
			}
		}

		public bool SmoothScrolling
		{
			get
			{
				return display.SmoothScrolling;
			}
			set
			{
				display.SmoothScrolling = value;
			}
		}

		public bool RealisticPages
		{
			get
			{
				return display.RealisticPages;
			}
			set
			{
				display.RealisticPages = value;
			}
		}

		public float InfoOverlayScaling
		{
			get
			{
				return display.InfoOverlayScaling;
			}
			set
			{
				display.InfoOverlayScaling = value;
			}
		}

		public RightToLeftReadingMode RightToLeftReadingMode
		{
			get
			{
				return display.RightToLeftReadingMode;
			}
			set
			{
				if (display.RightToLeftReadingMode != value)
				{
					display.RightToLeftReadingMode = value;
				}
			}
		}

		public bool LeftRightMovementReversed
		{
			get
			{
				return display.LeftRightMovementReversed;
			}
			set
			{
				display.LeftRightMovementReversed = value;
			}
		}

		public bool TwoPageNavigation
		{
			get
			{
				return display.TwoPageNavigation;
			}
			set
			{
				display.TwoPageNavigation = value;
			}
		}

		public bool AutoHideCursor
		{
			get
			{
				return display.AutoHideCursor;
			}
			set
			{
				display.AutoHideCursor = value;
			}
		}

		public bool RightToLeftReading
		{
			get
			{
				return display.RightToLeftReading;
			}
			set
			{
				if (display.RightToLeftReading == value)
				{
					return;
				}
				if (TwoPageDisplay)
				{
					Animate(delegate(float p)
					{
						display.DoublePageOverlap = p;
					});
				}
				display.RightToLeftReading = value;
				if (TwoPageDisplay)
				{
					Animate(delegate(float p)
					{
						display.DoublePageOverlap = 1f - p;
					});
				}
			}
		}

		public bool ImageAutoRotate
		{
			get
			{
				return display.ImageAutoRotate;
			}
			set
			{
				display.ImageAutoRotate = value;
			}
		}

		public ImageRotation ImageRotation
		{
			get
			{
				return display.ImageRotation;
			}
			set
			{
				display.ImageRotation = value;
			}
		}

		public PageLayoutMode PageLayout
		{
			get
			{
				return display.PageLayout;
			}
			set
			{
				if (display.PageLayout == value)
				{
					return;
				}
				bool imageAutoRotate = display.ImageAutoRotate;
				ImageRotation imageRotation = display.ImageRotation;
				display.ImageAutoRotate = false;
				display.ImageRotation = display.CurrentImageRotation;
				switch (value)
				{
				default:
					Animate(delegate(float p)
					{
						display.DoublePageOverlap = p;
					});
					break;
				case PageLayoutMode.Double:
					if (!TwoPageDisplay || (TwoPageDisplay && !IsDoubleImage))
					{
						display.PageLayout = value;
						Animate(delegate(float p)
						{
							display.DoublePageOverlap = 1f - p;
						});
					}
					break;
				case PageLayoutMode.DoubleAdaptive:
					if (TwoPageDisplay)
					{
						if (!IsDoubleImage)
						{
							Animate(delegate(float p)
							{
								display.DoublePageOverlap = p;
							});
						}
					}
					else
					{
						display.PageLayout = value;
						Animate(delegate(float p)
						{
							display.DoublePageOverlap = 1f - p;
						});
					}
					break;
				}
				display.PageLayout = value;
				display.DoublePageOverlap = 0f;
				display.ImageRotation = imageRotation;
				display.ImageAutoRotate = imageAutoRotate;
			}
		}

		public bool ImageFitOnlyIfOversized
		{
			get
			{
				return display.ImageFitOnlyIfOversized;
			}
			set
			{
				if (display.ImageFitOnlyIfOversized != value)
				{
					Animate((Action)delegate
					{
						display.ImageFitOnlyIfOversized = value;
					});
				}
			}
		}

		public bool MagnifierVisible
		{
			get
			{
				return display.MagnifierVisible;
			}
			set
			{
				display.MagnifierVisible = value;
			}
		}

		public float ImageZoom
		{
			get
			{
				return display.ImageZoom;
			}
			set
			{
				if (display.ImageZoom != value)
				{
					float num = 1f;
					Animate((Action)delegate
					{
						display.ImageZoom = value;
					}, (int)((float)EngineConfiguration.Default.AnimationDuration * num));
				}
			}
		}

		public ImageFitMode ImageFitMode
		{
			get
			{
				return display.ImageFitMode;
			}
			set
			{
				if (display.ImageFitMode != value)
				{
					Animate((Action)delegate
					{
						display.ImageFitMode = value;
					});
				}
			}
		}

		public PageTransitionEffect PageTransitionEffect
		{
			get
			{
				return display.PageTransitionEffect;
			}
			set
			{
				display.PageTransitionEffect = value;
			}
		}

		public bool DisplayChangeAnimation
		{
			get
			{
				return display.DisplayChangeAnimation;
			}
			set
			{
				display.DisplayChangeAnimation = value;
			}
		}

		public bool FlowingMouseScrolling
		{
			get
			{
				return display.FlowingMouseScrolling;
			}
			set
			{
				display.FlowingMouseScrolling = value;
			}
		}

		public bool SoftwareFiltering
		{
			get
			{
				return display.SoftwareFiltering;
			}
			set
			{
				display.SoftwareFiltering = value;
			}
		}

		public bool HardwareFiltering
		{
			get
			{
				return display.HardwareFiltering;
			}
			set
			{
				display.HardwareFiltering = value;
			}
		}

		public bool IsMovementFlipped => display.IsMovementFlipped;

		public bool BlendWhilePaging
		{
			get
			{
				return display.BlendWhilePaging;
			}
			set
			{
				display.BlendWhilePaging = value;
			}
		}

		public MagnifierStyle MagnifierStyle
		{
			get
			{
				return display.MagnifierStyle;
			}
			set
			{
				display.MagnifierStyle = value;
			}
		}

		bool IEditPage.IsValid
		{
			get
			{
				if (IsValid)
				{
					return Book.Comic.EditMode.CanEditPages();
				}
				return false;
			}
		}

		ComicPageType IEditPage.PageType
		{
			get
			{
				if (!IsValid)
				{
					return ComicPageType.Other;
				}
				if (CurrentMousePage != -1)
				{
					return Book.Comic.GetPage(CurrentMousePage).PageType;
				}
				return Book.Comic.GetPage(CurrentPage).PageType;
			}
			set
			{
				if (IsValid)
				{
					if (CurrentMousePage != -1)
					{
						Book.Comic.UpdatePageType(CurrentMousePage, value);
					}
					else
					{
						Book.Comic.UpdatePageType(CurrentPage, value);
					}
				}
			}
		}

		ImageRotation IEditPage.Rotation
		{
			get
			{
				if (!IsValid)
				{
					return ImageRotation.None;
				}
				if (CurrentMousePage != -1)
				{
					return Book.Comic.GetPage(CurrentMousePage).Rotation;
				}
				return Book.Comic.GetPage(CurrentPage).Rotation;
			}
			set
			{
				if (IsValid)
				{
					if (CurrentMousePage != -1)
					{
						Book.Comic.UpdatePageRotation(CurrentMousePage, value);
					}
					else
					{
						Book.Comic.UpdatePageRotation(CurrentPage, value);
					}
				}
			}
		}

		public event EventHandler FirstPageReached;

		public event EventHandler LastPageReached;

		public event EventHandler FullScreenChanged;

		public event EventHandler BookChanged
		{
			add
			{
				display.BookChanged += value;
			}
			remove
			{
				display.BookChanged -= value;
			}
		}

		public event EventHandler<GestureEventArgs> Gesture
		{
			add
			{
				display.Gesture += value;
			}
			remove
			{
				display.Gesture -= value;
			}
		}

		public event EventHandler VisibleInfoOverlaysChanged
		{
			add
			{
				display.VisibleInfoOverlaysChanged += value;
			}
			remove
			{
				display.VisibleInfoOverlaysChanged -= value;
			}
		}

		public event EventHandler<GestureEventArgs> PreviewGesture
		{
			add
			{
				display.PreviewGesture += value;
			}
			remove
			{
				display.PreviewGesture -= value;
			}
		}

		public event EventHandler<BookPageEventArgs> PageChange
		{
			add
			{
				display.PageChange += value;
			}
			remove
			{
				display.PageChange -= value;
			}
		}

		public event EventHandler<BookPageEventArgs> PageChanged
		{
			add
			{
				display.PageChanged += value;
			}
			remove
			{
				display.PageChanged -= value;
			}
		}

		public event EventHandler DrawnPageCountChanged
		{
			add
			{
				display.DrawnPageCountChanged += value;
			}
			remove
			{
				display.DrawnPageCountChanged -= value;
			}
		}

		public event EventHandler<BrowseEventArgs> Browse
		{
			add
			{
				display.Browse += value;
			}
			remove
			{
				display.Browse -= value;
			}
		}

		public ComicDisplay(IComicDisplay display)
		{
			this.display = display;
			this.display.BookChanged += display_BookChanged;
			this.display.PageChange += display_PageChange;
			this.display.PageChanged += display_PageChanged;
			this.display.Browse += display_Browse;
			this.display.PreviewGesture += display_PreviewGesture;
			this.display.Gesture += display_Gesture;
			control = display as ContainerControl;
			if (control != null)
			{
				control.KeyDown += display_KeyDown;
				control.MouseDown += display_MouseDown;
				control.KeyUp += control_KeyUp;
				control.MouseWheel += display_MouseWheel;
			}
			IMouseHWheel mouseHWheel = display as IMouseHWheel;
			if (mouseHWheel != null)
			{
				mouseHWheel.MouseHWheel += display_MouseHWheel;
			}
			pageKeys = new KeySearch((string s) => Book != null && int.TryParse(s, out var result) && Book.Navigate(result - 1, PageSeekOrigin.Beginning))
			{
				SearchDelay = 1000
			};
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				display.BookChanged -= display_BookChanged;
				display.PageChange -= display_PageChange;
				display.PageChanged -= display_PageChanged;
				display.Browse -= display_Browse;
				display.Gesture -= display_Gesture;
				control = display as ContainerControl;
				if (control != null)
				{
					control.KeyDown -= display_KeyDown;
					control.MouseDown -= display_MouseDown;
					control.KeyUp -= control_KeyUp;
					control.MouseWheel -= display_MouseWheel;
				}
				IMouseHWheel mouseHWheel = display as IMouseHWheel;
				if (mouseHWheel != null)
				{
					mouseHWheel.MouseHWheel -= display_MouseHWheel;
				}
				pageKeys.Dispose();
			}
			base.Dispose(disposing);
		}

		protected virtual void OnFullScreenChanged()
		{
			if (this.FullScreenChanged != null)
			{
				this.FullScreenChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnFirstPageReached()
		{
			if (this.FirstPageReached != null)
			{
				this.FirstPageReached(this, EventArgs.Empty);
			}
		}

		protected virtual void OnLastPageReached()
		{
			if (this.LastPageReached != null)
			{
				this.LastPageReached(this, EventArgs.Empty);
			}
		}

		public void DisplayNextPageOrPart(bool forceNewPage = false)
		{
			if (!EatScrolling() && (forceNewPage || !DisplayPart(PartPageToDisplay.Next)))
			{
				DisplayNextPage(PagingMode.Double | PagingMode.Walled);
			}
		}

		public void DisplayPreviousPageOrPart(bool forceNewPage = false)
		{
			if (!EatScrolling() && (forceNewPage || !DisplayPart(PartPageToDisplay.Previous)))
			{
				DisplayPreviousPage(PagingMode.Double | PagingMode.Walled);
			}
		}

		public void DisplayNextPage(PagingMode mode)
		{
			bool flag = (mode & PagingMode.Double) == 0;
			bool flag2 = (mode & PagingMode.Walled) != 0;
			if (!IsValid || (flag2 && IsPageChangeWalled()))
			{
				return;
			}
			int num = ((flag || !IsDoubleImage) ? 1 : 2);
			if (num == 2)
			{
				int page = Book.SeekNewPage(1, PageSeekOrigin.Current);
				if (Book.Comic.GetPage(page).PagePosition == ComicPagePosition.Near)
				{
					num = 1;
				}
			}
			if (!Book.Navigate(num))
			{
				OnLastPageReached();
			}
			lastPaging = Machine.Ticks;
		}

		public void DisplayPreviousPage(PagingMode mode)
		{
			bool flag = (mode & PagingMode.Double) == 0;
			bool flag2 = (mode & PagingMode.Walled) != 0;
			if (IsValid && (!flag2 || !IsPageChangeWalled()))
			{
				int page = Book.SeekNewPage(-1, PageSeekOrigin.Current);
				int num = Book.SeekNewPage(-2, PageSeekOrigin.Current);
				int offset;
				if (!TwoPageDisplay || flag || num == -1)
				{
					offset = -1;
				}
				else
				{
					ComicPageInfo page2 = Book.Comic.GetPage(page);
					ComicPageInfo page3 = Book.Comic.GetPage(num);
					offset = ((page2.IsSinglePageType || page2.IsDoublePage || page3.IsSinglePageType || page3.IsDoublePage || (page2.PagePosition == ComicPagePosition.Near && page3.PagePosition != ComicPagePosition.Far)) ? (-1) : (-2));
				}
				if (!Book.Navigate(offset))
				{
					OnFirstPageReached();
				}
				lastPaging = Machine.Ticks;
			}
		}

		public void DisplayFirstPage()
		{
			if (IsValid)
			{
				Book.Navigate(PageSeekOrigin.Beginning);
			}
		}

		public void DisplayLastPage()
		{
			if (IsValid)
			{
				Book.Navigate(PageSeekOrigin.End);
			}
		}

		public void DisplayLastPageRead()
		{
			if (IsValid)
			{
				ImageVisiblePart = ImagePartInfo.Empty;
				Book.Navigate(Book.Comic.LastPageRead, PageSeekOrigin.Beginning);
			}
		}

		public void DisplayPreviousBookmarkedPage()
		{
			if (IsValid)
			{
				ImageVisiblePart = ImagePartInfo.Empty;
				Book.NavigateBookmark(-1);
			}
		}

		public void DisplayNextBookmarkedPage()
		{
			if (IsValid)
			{
				ImageVisiblePart = ImagePartInfo.Empty;
				Book.NavigateBookmark(1);
			}
		}

		public void ScrollLeft(float lines)
		{
			if (!EatScrolling())
			{
				if (!AutoScrolling)
				{
					MovePart(new Point((int)(lines * (float)(-GetLineSize().Width)), 0));
				}
				else if (IsMovementFlipped)
				{
					DisplayNextPageOrPart();
				}
				else
				{
					DisplayPreviousPageOrPart();
				}
			}
		}

		public void ScrollLeft()
		{
			ScrollLeft(scrollLines);
		}

		public void ScrollRight(float lines)
		{
			if (!EatScrolling())
			{
				if (!AutoScrolling)
				{
					MovePart(new Point((int)(lines * (float)GetLineSize().Width), 0));
				}
				else if (IsMovementFlipped)
				{
					DisplayPreviousPageOrPart();
				}
				else
				{
					DisplayNextPageOrPart();
				}
			}
		}

		public void ScrollRight()
		{
			ScrollRight(scrollLines);
		}

		public void ScrollUp(float lines)
		{
			if (!EatScrolling())
			{
				if (AutoScrolling)
				{
					DisplayPreviousPageOrPart();
				}
				else
				{
					ScrollUp(lines, ScrollingDoesBrowse);
				}
			}
		}

		public void ScrollUp()
		{
			ScrollUp(scrollLines);
		}

		public void ScrollDown(float lines)
		{
			if (!EatScrolling())
			{
				if (AutoScrolling)
				{
					DisplayNextPageOrPart();
				}
				else
				{
					ScrollDown(lines, ScrollingDoesBrowse);
				}
			}
		}

		public void ScrollDown()
		{
			ScrollDown(scrollLines);
		}

		public void ToggleNavigationOverlay()
		{
			NavigationOverlayVisible = !NavigationOverlayVisible;
		}

		public void TogglePageFit()
		{
			ImageFitMode = (ImageFitMode)((int)(ImageFitMode + 1) % 6);
		}

		public void CopyPageToClipboard()
		{
			try
			{
				Clipboard.SetImage(CreatePageImage());
			}
			catch
			{
			}
		}

		public void ToggleFullScreen()
		{
			FullScreen = !FullScreen;
		}

		public void TogglePageLayout()
		{
			switch (PageLayout)
			{
			case PageLayoutMode.Single:
				PageLayout = PageLayoutMode.Double;
				break;
			case PageLayoutMode.Double:
				PageLayout = PageLayoutMode.DoubleAdaptive;
				break;
			case PageLayoutMode.DoubleAdaptive:
				PageLayout = PageLayoutMode.Single;
				break;
			}
		}

		public void ToogleRealisticPages()
		{
			RealisticPages = !RealisticPages;
		}

		public void ToggleFitOnlyIfOversized()
		{
			ImageFitOnlyIfOversized = !ImageFitOnlyIfOversized;
		}

		public void ToggleMagnifier()
		{
			MagnifierVisible = !MagnifierVisible;
		}

		public void SetPageOriginal()
		{
			ImageFitMode = ImageFitMode.Original;
		}

		public void SetPageFitAll()
		{
			ImageFitMode = ImageFitMode.Fit;
		}

		public void SetPageFitWidth()
		{
			ImageFitMode = ImageFitMode.FitWidth;
		}

		public void SetPageFitWidthAdaptive()
		{
			ImageFitMode = ImageFitMode.FitWidthAdaptive;
		}

		public void SetPageFitHeight()
		{
			ImageFitMode = ImageFitMode.FitHeight;
		}

		public void SetPageBestFit()
		{
			ImageFitMode = ImageFitMode.BestFit;
		}

		public bool IsPageFitHeight()
		{
			return ImageFitMode == ImageFitMode.FitHeight;
		}

		public bool IsPageFitWidth()
		{
			return ImageFitMode == ImageFitMode.FitWidth;
		}

		public bool IsPageFitWidthAdaptive()
		{
			return ImageFitMode == ImageFitMode.FitWidthAdaptive;
		}

		public bool IsPageFitBest()
		{
			return ImageFitMode == ImageFitMode.BestFit;
		}

		public void SetInfoOverlays(InfoOverlays overlays, bool enable)
		{
			if (enable)
			{
				VisibleInfoOverlays |= overlays;
			}
			else
			{
				VisibleInfoOverlays &= ~overlays;
			}
		}

		public bool GetInfoOverays(InfoOverlays overlays)
		{
			return VisibleInfoOverlays.HasFlag(overlays);
		}

		private bool IsPageChangeWalled()
		{
			if (pageWallTicks == 0L || ImagePartCount == 1)
			{
				return false;
			}
			long ticks = Machine.Ticks;
			if (ticks - lastPartNavigation > pageWallTicks)
			{
				this.wallState = WallState.Initial;
				return false;
			}
			WallState wallState = this.wallState;
			if (wallState == WallState.Initial || wallState != WallState.Pending)
			{
				transitionStart = ticks;
				this.wallState = WallState.Pending;
				return true;
			}
			if (ticks - transitionStart < pageWallTicks)
			{
				return true;
			}
			this.wallState = WallState.Initial;
			transitionStart = ticks;
			return false;
		}

		private bool EatScrolling()
		{
			if (ImagePartCount == 1)
			{
				return false;
			}
			long ticks = Machine.Ticks;
			if (ticks - lastPaging < pageWallTicks)
			{
				return true;
			}
			return false;
		}

		private bool ScrollUp(float lines, bool withPageChange)
		{
			if (MovePart(new Point(0, (int)((0f - lines) * (float)GetLineSize().Height))))
			{
				return true;
			}
			if (!withPageChange)
			{
				return false;
			}
			if (EatScrolling())
			{
				return false;
			}
			DisplayPreviousPageOrPart(forceNewPage: true);
			return true;
		}

		private bool ScrollDown(float lines, bool withPageChange)
		{
			if (MovePart(new Point(0, (int)(lines * (float)GetLineSize().Height))))
			{
				return true;
			}
			if (!withPageChange)
			{
				return false;
			}
			DisplayNextPageOrPart(forceNewPage: true);
			return true;
		}

		private Size GetLineSize()
		{
			bool isDoubleImage = IsDoubleImage;
			Size imageSize = ImageSize;
			return new Size(imageSize.Width / (isDoubleImage ? 32 : 16), imageSize.Height / 32);
		}

		private void display_MouseDown(object sender, MouseEventArgs e)
		{
			scrollLines = 1f;
		}

		private void display_BookChanged(object sender, EventArgs e)
		{
			if (Book != null)
			{
				Book.PageFilter = PageFilter;
			}
			if (resetZoomOnPageChange)
			{
				ImageZoom = 1f;
			}
		}

		private void display_PageChange(object sender, BookPageEventArgs e)
		{
			oldZoom = 0f;
			if (ImageZoom != 1f)
			{
				if (resetZoomOnPageChange)
				{
					ImageZoom = 1f;
				}
				else if (zoomInOutOnPageChange && IsHardwareRenderer && ShouldPagingBlend)
				{
					oldZoom = ImageZoom;
					ImageZoom = 1f;
				}
			}
		}

		private void display_PageChanged(object sender, BookPageEventArgs e)
		{
			if (zoomInOutOnPageChange && oldZoom != 0f)
			{
				bool goingForward = e.OldPage < e.Page;
				bool RTL = RightToLeftReading && RightToLeftReadingMode == RightToLeftReadingMode.FlipParts;
				int corner = RTL ? -1 : 1;

				if (goingForward)
					ZoomTo(new Point(corner * -500000, -500000), oldZoom); //Next page: Arrive on Upper Left Corner, Upper Right with RTL
				else
					ZoomTo(new Point(corner * 500000, 500000), oldZoom); //Previous Page: Arrive on Bottom Right Corner, Bottom Left with RTL
			}
			oldZoom = 0f;
		}

		private CommandKey TranslateTouchGesture(GestureEventArgs e)
		{
			switch (e.Area)
			{
			case ContentAlignment.TopLeft:
				if (!e.Double)
				{
					return CommandKey.Gesture1;
				}
				return CommandKey.GestureDouble1;
			case ContentAlignment.TopCenter:
				if (!e.Double)
				{
					return CommandKey.Gesture2;
				}
				return CommandKey.GestureDouble2;
			case ContentAlignment.TopRight:
				if (!e.Double)
				{
					return CommandKey.Gesture3;
				}
				return CommandKey.GestureDouble3;
			case ContentAlignment.MiddleLeft:
				if (!e.Double)
				{
					return CommandKey.Gesture4;
				}
				return CommandKey.GestureDouble4;
			case ContentAlignment.MiddleCenter:
				if (!e.Double)
				{
					return CommandKey.Gesture5;
				}
				return CommandKey.GestureDouble5;
			case ContentAlignment.MiddleRight:
				if (!e.Double)
				{
					return CommandKey.Gesture6;
				}
				return CommandKey.GestureDouble6;
			case ContentAlignment.BottomLeft:
				if (!e.Double)
				{
					return CommandKey.Gesture7;
				}
				return CommandKey.GestureDouble7;
			case ContentAlignment.BottomCenter:
				if (!e.Double)
				{
					return CommandKey.Gesture8;
				}
				return CommandKey.GestureDouble8;
			case ContentAlignment.BottomRight:
				if (!e.Double)
				{
					return CommandKey.Gesture9;
				}
				return CommandKey.GestureDouble9;
			default:
				return CommandKey.None;
			}
		}

		private void display_PreviewGesture(object sender, GestureEventArgs e)
		{
			if (e.Gesture == GestureType.Touch)
			{
				e.Handled = keyboardMap.Commands.Any((KeyboardCommand c) => c.Handles(TranslateTouchGesture(e)));
			}
		}

		private void display_Gesture(object sender, GestureEventArgs e)
		{
			switch (e.Gesture)
			{
			case GestureType.Click:
				if (MouseClickEnabled)
				{
					e.Handled = keyboardMap.HandleKey(e.MouseButton, e.Double, e.IsTouch);
				}
				break;
			case GestureType.PressAndTap:
				e.Handled = keyboardMap.HandleKey(CommandKey.TouchPressAndTap);
				break;
			case GestureType.TwoFingerTap:
				e.Handled = keyboardMap.HandleKey(CommandKey.TouchTwoFingerTap);
				break;
			case GestureType.Touch:
				e.Handled = keyboardMap.HandleKey(TranslateTouchGesture(e));
				break;
			case GestureType.FlickLeft:
				e.Handled = keyboardMap.HandleKey(CommandKey.FlickLeft);
				break;
			case GestureType.FlickRight:
				e.Handled = keyboardMap.HandleKey(CommandKey.FlickRight);
				break;
			}
		}

		private void display_KeyDown(object sender, KeyEventArgs e)
		{
			scrollLines = 1f;
			e.Handled = keyboardMap.HandleKey(e.KeyCode | e.Modifiers);
			if (!e.Handled)
			{
				char numberFromKey = GetNumberFromKey(e.KeyCode);
				e.Handled = numberFromKey != 0 && pageKeys.Select(numberFromKey);
			}
		}

		private char GetNumberFromKey(Keys key)
		{
			if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
			{
				return (char)(48 + (key - 96));
			}
			if (key >= Keys.D0 && key <= Keys.D9)
			{
				return (char)(48 + (key - 48));
			}
			return '\0';
		}

		private void display_MouseWheel(object sender, MouseEventArgs e)
		{
			if (Control.MouseButtons == MouseButtons.Right)
			{
				keyboardMap.HandleKey((e.Delta > 0) ? CommandKey.Tab : (CommandKey.Tab | CommandKey.Shift));
				SupressContextMenu = true;
			}
			else
			{
				scrollLines = (float)Math.Abs(e.Delta / SystemInformation.MouseWheelScrollDelta) * MouseWheelSpeed;
				keyboardMap.HandleKey((e.Delta > 0) ? CommandKey.MouseWheelUp : CommandKey.MouseWheelDown, Control.ModifierKeys);
			}
		}

		private void display_MouseHWheel(object sender, MouseEventArgs e)
		{
			keyboardMap.HandleKey((e.Delta < 0) ? CommandKey.MouseTiltLeft : CommandKey.MouseTiltRight, Control.ModifierKeys);
		}

		private void display_Browse(object sender, BrowseEventArgs e)
		{
			PageSeekOrigin pageSeekOrigin = e.SeekOrigin;
			int num = e.Offset;
			if (IsMovementFlipped && pageSeekOrigin != PageSeekOrigin.Absolute)
			{
				num = -num;
				if (pageSeekOrigin == PageSeekOrigin.Beginning || pageSeekOrigin == PageSeekOrigin.End)
				{
					pageSeekOrigin = ((pageSeekOrigin == PageSeekOrigin.Beginning) ? PageSeekOrigin.End : PageSeekOrigin.Beginning);
				}
			}
			switch (pageSeekOrigin)
			{
			case PageSeekOrigin.Beginning:
				DisplayFirstPage();
				break;
			case PageSeekOrigin.End:
				DisplayLastPage();
				break;
			case PageSeekOrigin.Current:
				if (num < 0)
				{
					DisplayPreviousPage(PagingMode.Double);
				}
				else
				{
					DisplayNextPage(PagingMode.Double);
				}
				break;
			case PageSeekOrigin.Absolute:
				Book.Navigate(num, pageSeekOrigin);
				break;
			}
		}

		private void control_KeyUp(object sender, KeyEventArgs e)
		{
			lastPaging = 0L;
		}

		public void DisplayOpenMessage()
		{
			display.DisplayOpenMessage();
		}

		public Bitmap CreatePageImage()
		{
			return display.CreatePageImage();
		}

		public bool DisplayPart(PartPageToDisplay ptd)
		{
			bool flag = display.DisplayPart(ptd);
			if (flag)
			{
				wallState = WallState.Initial;
				lastPartNavigation = Machine.Ticks;
			}
			return flag;
		}

		public bool MovePart(Point offset)
		{
			bool flag = display.MovePart(offset);
			if (flag)
			{
				wallState = WallState.Initial;
				lastPartNavigation = Machine.Ticks;
			}
			return flag;
		}

		public void MovePartDown(float percent)
		{
			display.MovePartDown(percent);
		}

		public bool Focus()
		{
			if (control != null)
			{
				return control.Focus();
			}
			return false;
		}

		public bool SetRenderer(bool hardware)
		{
			return display.SetRenderer(hardware);
		}

		public object GetState()
		{
			return display.GetState();
		}

		public void Animate(object a, object b, int time)
		{
			display.Animate(a, b, time);
		}

		public void Animate(Action<float> animate, int time)
		{
			display.Animate(animate, time);
		}

		public void ZoomTo(Point location, float zoom)
		{
			if (display.ImageZoom != zoom)
			{
				float num = 1f;
				Animate((Action)delegate
				{
					display.ZoomTo(location, zoom);
				}, (int)((float)EngineConfiguration.Default.AnimationDuration * num));
			}
		}

		public void RefreshDisplay()
		{
			if (IsValid)
			{
				display.PagePool.RefreshPage(Book.GetPageKey(Book.CurrentPage));
				if (TwoPageDisplay)
				{
					display.PagePool.RefreshPage(Book.GetPageKey(Book.NextPage));
				}
			}
			if (control != null)
			{
				control.Invalidate();
			}
		}

		private IEnumerable<ComicBook> GetBookAsList()
		{
			if (Book != null && Book.Comic != null)
			{
				yield return Book.Comic;
			}
		}

		public IEnumerable<ComicBook> GetBookList(ComicBookFilterType cbft)
		{
			return ComicBookCollection.Filter(cbft, GetBookAsList());
		}

		public void Animate(Action m, int time)
		{
			if (!DisplayChangeAnimation)
			{
				m();
				return;
			}
			object state = display.GetState();
			m();
			object state2 = display.GetState();
			display.Animate(state, state2, time);
		}

		public void Animate(Action m)
		{
			Animate(m, EngineConfiguration.Default.AnimationDuration);
		}

		public void Animate(Action<float> action)
		{
			Animate(action, EngineConfiguration.Default.AnimationDuration);
		}
	}
}
