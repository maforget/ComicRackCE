using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Presentation;
using cYo.Common.Presentation.Panels;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Display.Forms.Properties;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.Display.Forms
{
	public class ComicDisplayControl : ImageDisplayControl, IComicDisplay, IComicDisplayConfig
	{
		public enum BlendAnimationMode
		{
			Default,
			CurrentAsNew,
			CurrentAsOld
		}

		public class ImageInfo
		{
			public int Width
			{
				get;
				set;
			}

			public int Height
			{
				get;
				set;
			}

			public int ImageCount
			{
				get;
				set;
			}

			public bool IsForcedDoublePage
			{
				get;
				set;
			}

			public bool IsSingleImage => ImageCount == 1;

			public bool IsDoubleImage => ImageCount > 1;

			public bool IsDoublePage => Width > Height;

			public bool IsValid => !Size.IsEmpty;

			public Size Size
			{
				get
				{
					return new Size(Width, Height);
				}
				set
				{
					Width = value.Width;
					Height = value.Height;
				}
			}
		}

		public delegate void BlendAnimationHandler(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent);

		private class ScaledPageKey
		{
			private WeakReference<PageImage> wrf;

			public float ScaleX
			{
				get;
				set;
			}

			public float ScaleY
			{
				get;
				set;
			}

			public PageImage Bitmap
			{
				get
				{
					return wrf.GetData();
				}
				set
				{
					wrf = new WeakReference<PageImage>(value);
				}
			}

			public override bool Equals(object obj)
			{
				ScaledPageKey scaledPageKey = obj as ScaledPageKey;
				if (scaledPageKey != null && scaledPageKey.ScaleX == ScaleX && scaledPageKey.ScaleY == ScaleY)
				{
					return scaledPageKey.Bitmap == Bitmap;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return ScaleX.GetHashCode() ^ ScaleY.GetHashCode();
			}
		}

		private class ScaledPageItem : MemoryOptimizedImage
		{
			public long Ticks
			{
				get;
				set;
			}

			public ScaledPageItem()
				: base((Bitmap)null)
			{
			}
		}

		private struct Magnifier
		{
			public Bitmap Bitmap;

			public Rectangle Inner;

			public Rectangle Outer;
		}

		private class MoveAnimator : Animator
		{
			public MoveAnimator(int time, Point fromPoint, Point toPoint, bool outTransition)
			{
				if (!outTransition)
				{
					base.Delay = 500;
				}
				base.Span = time;
				base.AnimationValueGenerator = Animator.SinusRise;
				Point dp = new Point(toPoint.X - fromPoint.X, toPoint.Y - fromPoint.Y);
				base.AnimationHandler = delegate(OverlayPanel p, float t, float d)
				{
					p.X = fromPoint.X + (int)(t * (float)dp.X);
					p.Y = fromPoint.Y + (int)(t * (float)dp.Y);
					if (outTransition && t >= 1f)
					{
						p.Visible = false;
					}
				};
			}
		}

		private class SizeAnimator : Animator
		{
			public SizeAnimator(int time, Point toPoint, bool outTransition)
			{
				base.Span = time;
				base.AnimationValueGenerator = Animator.LinearRise;
				base.AnimationHandler = delegate(OverlayPanel p, float t, float d)
				{
					p.Scale = (outTransition ? (1f - t) : t);
					Rectangle bounds = p.Bounds;
					Rectangle physicalBounds = p.PhysicalBounds;
					p.X = toPoint.X + (physicalBounds.Width - bounds.Width) / 2;
					p.Y = toPoint.Y + physicalBounds.Height - bounds.Height;
					if (outTransition && t >= 1f)
					{
						p.Visible = false;
					}
				};
			}
		}

		public const int DefaultFadeTime = 100;

		private static readonly int NavigationOverlayDefaultHeight = FormUtility.ScaleDpiY(200);

		private static readonly Size pageInfoSize = new Size(150, 25).ScaleDpi();

		private static readonly Size partInfoSize = new Size(200, 200).ScaleDpi();

		private static readonly Size loadInfoSize = new Size(200, 30).ScaleDpi();

		private static readonly Size messageSize = new Size(300, 30).ScaleDpi();

		public static readonly Cursor EmptyCursor = new Cursor(new MemoryStream(Resources.EmptyCursor));

		private readonly OverlayManager overlayManager;

		private readonly TextOverlay currentPageOverlay;

		private readonly TextOverlay loadPageOverlay;

		private readonly TextOverlay messageOverlay;

		private readonly OverlayPanel visiblePartOverlay;

		private readonly OverlayPanel magnifierOverlay;

		private readonly NavigationOverlay navigationOverlay;

		private readonly OverlayPanel gestureOverlay;

		private bool firstPageHasBeenLoaded = true;

		private bool preCache = true;

		private bool navigationOverlayVisible;

		private volatile IPagePool pagePool;

		private ComicBookNavigator book;

		private readonly Color blindOutColor = Color.FromArgb(192, Color.Black);

		private bool blindOut;

		private bool showStatusMessage;

		private bool leftRightMovementReversed;

		private volatile PageLayoutMode pageLayout;

		private volatile float doublePageOverlap;

		private float magnifierZoom = 2f;

		private bool magnifierVisible;

		private bool autoHideMagnifier = true;

		private bool autoMagnifier = true;

		private bool realisticPages = true;

		private float infoOverlayScaling = 1f;

		private InfoOverlays visibleInfoOverlays;

		private PageTransitionEffect pageTransitionEffect = PageTransitionEffect.Fade;

		private bool disableBlending;

		private bool softwareFiltering = true;

		private MagnifierStyle magnifierStyle;

		private Bitmap paperTextureBitmap;

		private ImageLayout paperTextureLayout;

		private float paperTextureStrength = 1f;

		private string paperTexture;

		private int displayHash;

		private volatile int currentPage;

		private int currentMousePage = -1;

		private Bitmap workingPaperTexture;

		private static readonly Bitmap pageBowLeft = PageRendering.CreatePageBow(new Size(256, 256), 180f);

		private static readonly Bitmap pageBowRight = PageRendering.CreatePageBow(new Size(256, 256), 0f);

		private Bitmap shadowBitmap;

		private float innerBowLeftOffsetInPercent;

		private float innerBowRightOffsetInPercent;

		private int[] displayedPages = new int[0];

		private Rectangle[] displayedPageAreas = new Rectangle[0];

		private RectangleF displayedPageBounds;

		private PageKey lastValidKey;

		private bool drawBlankPagesOverride;

		private Cache<ScaledPageKey, ScaledPageItem> scaledCache;

		private Point mouseDown;

		private bool panMagnifier;

		private int cachedPartOverlay = -1;

		private Point cachedPartOffset;

		private int currentPageOverlayHash;

		private static Magnifier[] magnifiers;

		private RectangleF partRect;

		private int currentPartHash;

		private Bitmap smallBitmap;

		private long lastBlend;

		private const bool moveAnim = true;

		private bool inBlendAnmation;

		private IContainer components;

		private System.Windows.Forms.Timer imageScaleTimer;

		private System.Windows.Forms.Timer longClickTimer;

		private System.Windows.Forms.Timer cacheUpdateTimer;

		[DefaultValue(true)]
		public bool PreCache
		{
			get
			{
				return preCache;
			}
			set
			{
				preCache = value;
			}
		}

		[DefaultValue(false)]
		public bool NavigationOverlayVisible
		{
			get
			{
				return navigationOverlayVisible;
			}
			set
			{
				if (navigationOverlayVisible != value)
				{
					navigationOverlayVisible = value;
					UpdateNavigationOverlay();
				}
			}
		}

		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IPagePool PagePool
		{
			get
			{
				return pagePool;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (pagePool != value)
				{
					if (pagePool != null)
					{
						pagePool.PageCached -= MemoryPageCache_ItemAdded;
					}
					pagePool = value;
					value.PageCached += MemoryPageCache_ItemAdded;
				}
			}
		}

		[DefaultValue(null)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IThumbnailPool ThumbnailPool
		{
			get
			{
				return navigationOverlay.Pool;
			}
			set
			{
				navigationOverlay.Pool = value;
			}
		}

		[DefaultValue(null)]
		public ComicBookNavigator Book
		{
			get
			{
				return book;
			}
			set
			{
				ComicBookNavigator comicBookNavigator = Book;
				if (comicBookNavigator == value)
				{
					return;
				}
				StopPendingImageCacheUpdate();
				if (comicBookNavigator != null)
				{
					comicBookNavigator.Disposing -= book_Disposing;
					comicBookNavigator.Navigation -= book_Navigation;
					comicBookNavigator.IndexOfPageReady -= book_IndexOfPageReady;
					comicBookNavigator.ColorAdjustmentChanged -= book_ColorAdjustmentChanged;
					comicBookNavigator.RightToLeftReadingChanged -= book_RightToLeftReadingChanged;
					comicBookNavigator.IndexRetrievalCompleted -= book_IndexRetrievalCompleted;
					comicBookNavigator.PageFilterChanged -= book_PageFilterOrPagesChanged;
					comicBookNavigator.PagesChanged -= book_PageFilterOrPagesChanged;
					comicBookNavigator.Comic.BookChanged -= Comic_BookChanged;
					comicBookNavigator.PagePart = base.ImageVisiblePart;
					comicBookNavigator.RightToLeftReading = (base.RightToLeftReading ? YesNo.Yes : YesNo.No);
				}
				book = value;
				OnBookChanged();
				lastValidKey = null;
				if (value != null)
				{
					value.Disposing += book_Disposing;
					value.Navigation += book_Navigation;
					value.ColorAdjustmentChanged += book_ColorAdjustmentChanged;
					value.RightToLeftReadingChanged += book_RightToLeftReadingChanged;
					value.IndexRetrievalCompleted += book_IndexRetrievalCompleted;
					value.IndexOfPageReady += book_IndexOfPageReady;
					value.PageFilterChanged += book_PageFilterOrPagesChanged;
					value.PagesChanged += book_PageFilterOrPagesChanged;
					value.Comic.BookChanged += Comic_BookChanged;
					CurrentPage = value.CurrentPage;
					base.ImageVisiblePart = value.PagePart;
					if (value.RightToLeftReading != YesNo.Unknown)
					{
						base.RightToLeftReading = value.RightToLeftReading == YesNo.Yes;
					}
				}
				navigationOverlay.Provider = value;
				navigationOverlay.ImageKeyProvider = value;
				UpdateNavigationOverlay(redraw: true);
				Invalidate();
			}
		}

		[DefaultValue(typeof(Color), "192, 0, 0, 0")]
		public Color BlindOutColor
		{
			get
			{
				return blindOutColor;
			}
			set
			{
				if (!(blindOutColor == value) && blindOut)
				{
					Invalidate();
				}
			}
		}

		[DefaultValue(false)]
		public bool BlindOut
		{
			get
			{
				return blindOut;
			}
			set
			{
				if (blindOut != value)
				{
					blindOut = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(false)]
		public bool ShowStatusMessage
		{
			get
			{
				return showStatusMessage;
			}
			set
			{
				if (showStatusMessage != value)
				{
					showStatusMessage = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(false)]
		public bool LeftRightMovementReversed
		{
			get
			{
				return leftRightMovementReversed;
			}
			set
			{
				if (leftRightMovementReversed != value)
				{
					leftRightMovementReversed = value;
					OnReadingModeChanged();
				}
			}
		}

		[DefaultValue(PageLayoutMode.Single)]
		public PageLayoutMode PageLayout
		{
			get
			{
				return pageLayout;
			}
			set
			{
				if (pageLayout != value)
				{
					pageLayout = value;
					OnDisplayChanged();
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		[DefaultValue(0f)]
		public float DoublePageOverlap
		{
			get
			{
				return doublePageOverlap;
			}
			set
			{
				if (doublePageOverlap != value)
				{
					doublePageOverlap = value;
				}
			}
		}

		[DefaultValue(2f)]
		public float MagnifierZoom
		{
			get
			{
				return magnifierZoom;
			}
			set
			{
				if (magnifierZoom != value)
				{
					magnifierZoom = value;
					if (MagnifierVisible)
					{
						Invalidate();
					}
				}
			}
		}

		[DefaultValue(false)]
		public bool MagnifierVisible
		{
			get
			{
				return magnifierVisible;
			}
			set
			{
				if (magnifierVisible != value)
				{
					magnifierVisible = value;
					UpdateMagnifierVisibility();
				}
			}
		}

		[DefaultValue(1f)]
		public float MagnifierOpacity
		{
			get
			{
				return magnifierOverlay.Opacity;
			}
			set
			{
				magnifierOverlay.Opacity = value;
			}
		}

		[DefaultValue(typeof(Size), "200, 200")]
		public Size MagnifierSize
		{
			get
			{
				return magnifierOverlay.Size;
			}
			set
			{
				magnifierOverlay.Size = value;
			}
		}

		[DefaultValue(true)]
		public bool AutoHideMagnifier
		{
			get
			{
				return autoHideMagnifier;
			}
			set
			{
				if (autoHideMagnifier != value)
				{
					autoHideMagnifier = value;
					UpdateMagnifierVisibility();
				}
			}
		}

		[DefaultValue(true)]
		public bool AutoMagnifier
		{
			get
			{
				return autoMagnifier;
			}
			set
			{
				if (autoMagnifier != value)
				{
					autoMagnifier = value;
					UpdateMagnifierVisibility();
				}
			}
		}

		[DefaultValue(true)]
		public bool RealisticPages
		{
			get
			{
				return realisticPages;
			}
			set
			{
				if (realisticPages != value)
				{
					realisticPages = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(1f)]
		public float InfoOverlayScaling
		{
			get
			{
				return infoOverlayScaling;
			}
			set
			{
				if (infoOverlayScaling != value)
				{
					infoOverlayScaling = value;
					TextOverlay textOverlay = currentPageOverlay;
					OverlayPanel overlayPanel = visiblePartOverlay;
					TextOverlay textOverlay2 = loadPageOverlay;
					float num2 = (messageOverlay.Scale = infoOverlayScaling);
					float num4 = (textOverlay2.Scale = num2);
					float num7 = (textOverlay.Scale = (overlayPanel.Scale = num4));
					navigationOverlay.Size = CalcNavigationOverlaySize();
				}
			}
		}

		[DefaultValue(InfoOverlays.None)]
		public InfoOverlays VisibleInfoOverlays
		{
			get
			{
				return visibleInfoOverlays;
			}
			set
			{
				if (visibleInfoOverlays != value)
				{
					visibleInfoOverlays = value;
					UpdateNavigationOverlay();
					OnVisibleInfoOverlaysChanged();
				}
			}
		}

		[DefaultValue(PageTransitionEffect.Fade)]
		public PageTransitionEffect PageTransitionEffect
		{
			get
			{
				return pageTransitionEffect;
			}
			set
			{
				pageTransitionEffect = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public BlendAnimationHandler Blender
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool DisableBlending
		{
			get
			{
				return disableBlending;
			}
			set
			{
				disableBlending = value;
			}
		}

		[DefaultValue(true)]
		public bool SoftwareFiltering
		{
			get
			{
				return softwareFiltering;
			}
			set
			{
				softwareFiltering = value;
			}
		}

		public bool IsFlipped
		{
			get
			{
				if (base.RightToLeftReading)
				{
					return base.RightToLeftReadingMode == RightToLeftReadingMode.FlipParts;
				}
				return false;
			}
		}

		public bool IsMovementFlipped
		{
			get
			{
				if (LeftRightMovementReversed)
				{
					return IsFlipped;
				}
				return false;
			}
		}

		[DefaultValue(false)]
		public bool BlendWhilePaging
		{
			get;
			set;
		}

		[DefaultValue(MagnifierStyle.Glass)]
		public MagnifierStyle MagnifierStyle
		{
			get
			{
				return magnifierStyle;
			}
			set
			{
				magnifierStyle = value;
			}
		}

		public Color BlankPageColor => EngineConfiguration.Default.BlankPageColor;

		[Category("Appearance")]
		[Description("Paper overlay texture")]
		[DefaultValue(null)]
		public Bitmap PaperTextureBitmap
		{
			get
			{
				return paperTextureBitmap;
			}
			set
			{
				if (paperTextureBitmap != value)
				{
					paperTextureBitmap = value;
					CreateWorkingPaperTexture();
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("Paper texture layout")]
		[DefaultValue(null)]
		public ImageLayout PaperTextureLayout
		{
			get
			{
				return paperTextureLayout;
			}
			set
			{
				if (paperTextureLayout != value)
				{
					paperTextureLayout = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("Paper texture alpha value")]
		[DefaultValue(1f)]
		public float PaperTextureStrength
		{
			get
			{
				return paperTextureStrength;
			}
			set
			{
				if (value != paperTextureStrength)
				{
					paperTextureStrength = value;
					CreateWorkingPaperTexture();
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[Description("Paper texture file")]
		[DefaultValue(null)]
		public string PaperTexture
		{
			get
			{
				return paperTexture;
			}
			set
			{
				if (!(paperTexture == value))
				{
					paperTexture = value;
					Image obj = PaperTextureBitmap;
					try
					{
						PaperTextureBitmap = (Bitmap)Image.FromFile(paperTexture);
					}
					catch (Exception)
					{
						PaperTextureBitmap = null;
					}
					obj.SafeDispose();
					OnImageDisplayOptionsChanged();
					Invalidate();
				}
			}
		}

		public override bool IsValid
		{
			get
			{
				if (Book != null)
				{
					return PagePool != null;
				}
				return false;
			}
		}

		public int DisplayHash => displayHash;

		public int CurrentPage
		{
			get
			{
				return currentPage;
			}
			protected set
			{
				currentPage = value;
			}
		}

		public int CurrentMousePage => currentMousePage;

		protected int NextPage
		{
			get
			{
				if (!TwoPageDisplay)
				{
					return -1;
				}
				return SeekPage(CurrentPage, 1);
			}
		}

		public bool TwoPageDisplay => PageLayout != PageLayoutMode.Single;

		public bool ShouldPagingBlend
		{
			get;
			private set;
		}

		protected override bool MouseHandled
		{
			get
			{
				if (!overlayManager.MouseHandled)
				{
					return base.MouseActionHappened;
				}
				return true;
			}
		}

		private int[] DisplayedPages => displayedPages;

		private Rectangle[] DisplayedPageAreas => displayedPageAreas;

		private RectangleF DisplayedPageBounds => displayedPageBounds;

		private bool IsCurrentPageOverlayEnabled => (visibleInfoOverlays & InfoOverlays.CurrentPage) != 0;

		private bool IsPartInfoOverlayEnabled => (visibleInfoOverlays & InfoOverlays.PartInfo) != 0;

		private bool IsLoadPageOverlayEnabled => (visibleInfoOverlays & InfoOverlays.LoadPage) != 0;

		private bool IsNavigationOverlayEnabled => (visibleInfoOverlays & InfoOverlays.PageBrowser) != 0;

		private bool IsPageBrowsersOnTop => (visibleInfoOverlays & InfoOverlays.PageBrowserOnTop) != 0;

		private bool CurrentPageShowsName => (visibleInfoOverlays & InfoOverlays.CurrentPageShowsName) != 0;

		public override int PageScrollingTime
		{
			get
			{
				return EngineConfiguration.Default.PageScrollingDuration;
			}
			set
			{
				base.PageScrollingTime = value;
			}
		}

		public override bool IsDoubleImage => GetImageInfo().IsDoubleImage;

		private int NavigationOverlayVisibleY
		{
			get
			{
				Rectangle clientRectangle = base.ClientRectangle;
				Rectangle bounds = navigationOverlay.Bounds;
				if (!IsPageBrowsersOnTop)
				{
					return clientRectangle.Height - bounds.Height - base.Margin.Bottom;
				}
				return base.Margin.Top;
			}
		}

		public event EventHandler BookChanged;

		public event EventHandler DrawnPageCountChanged;

		public event EventHandler<BrowseEventArgs> Browse;

		public event EventHandler<BookPageEventArgs> PageChange;

		public event EventHandler<BookPageEventArgs> PageChanged;

		public event EventHandler VisibleInfoOverlaysChanged;

		public ComicDisplayControl()
		{
			InitializeComponent();
			cacheUpdateTimer.Interval = EngineConfiguration.Default.PageCachingDelay;
			overlayManager = new OverlayManager(this)
			{
				AnimationEnabled = true
			};
			components.Add(overlayManager);
			Size size = new Size(300, 200).ScaleDpi();
			magnifierOverlay = new OverlayPanel(size)
			{
				Opacity = 1f,
				Visible = false,
				Enabled = false
			};
			magnifierOverlay.RenderSurface += magnifierOverlay_RenderSurface;
			overlayManager.Panels.Add(magnifierOverlay);
			visiblePartOverlay = new OverlayPanel(partInfoSize.Width, partInfoSize.Height, ContentAlignment.BottomRight, new Animator[1]
			{
				new FadeAnimator(DefaultFadeTime, 2000, DefaultFadeTime)
			})
			{
				Opacity = 0f,
				AutoAlign = true,
				Enabled = true,
				HitTestType = HitTestType.Disabled
			};
			visiblePartOverlay.Drawing += visiblePartOverlay_Drawing;
			visiblePartOverlay.RenderSurface += visiblePartOverlay_RenderSurface;
			if (!EngineConfiguration.Default.HideVisiblePartOverlayClose)
			{
				SimpleButtonPanel simpleButtonPanel = new SimpleButtonPanel(new Size(16, 16).ScaleDpi())
				{
					Margin = Padding.Empty,
					Alignment = ContentAlignment.TopRight,
					Icon = Resources.Close,
					AutoAlign = true,
					AlignmentOffset = new Point(-3, -2).ScaleDpi()
				};
				simpleButtonPanel.Click += delegate
				{
					visiblePartOverlay.Opacity = 0f;
					VisibleInfoOverlays &= ~InfoOverlays.PartInfo;
				};
				visiblePartOverlay.Panels.Add(simpleButtonPanel);
			}
			overlayManager.Panels.Add(visiblePartOverlay);
			currentPageOverlay = new TextOverlay(pageInfoSize.Width, pageInfoSize.Height, ContentAlignment.TopRight, Font)
			{
				Enabled = false,
				Opacity = 0f,
				AutoAlign = true,
				Html = true
			};
			currentPageOverlay.Animators.Add(new FadeAnimator(100, 2000, 100));
			overlayManager.Panels.Add(currentPageOverlay);
			loadPageOverlay = new TextOverlay(loadInfoSize.Width, loadInfoSize.Height, ContentAlignment.MiddleCenter, Font)
			{
				Enabled = false,
				AutoAlign = true,
				Visible = false
			};
			overlayManager.Panels.Add(loadPageOverlay);
			messageOverlay = new TextOverlay(messageSize.Width, messageSize.Height, ContentAlignment.MiddleCenter, Font)
			{
				Enabled = false,
				AutoAlign = true,
				Visible = false
			};
			overlayManager.Panels.Add(messageOverlay);
			navigationOverlay = new NavigationOverlay(new Size(500, NavigationOverlayDefaultHeight).ScaleDpi())
			{
				Visible = false
			};
			navigationOverlay.Browse += delegate(object x, BrowseEventArgs y)
			{
				OnBrowse(y);
			};
			overlayManager.Panels.Add(navigationOverlay);
			gestureOverlay = new GestureOverlay
			{
				Enabled = false,
				Opacity = 0f,
				AutoAlign = true,
				IgnoreParentMargin = true
			};
			gestureOverlay.Animators.Add(new FadeAnimator(0, 500, DefaultFadeTime));
			overlayManager.Panels.Add(gestureOverlay);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Book = null;
			}
			overlayManager.Panels.Dispose();
			overlayManager.Dispose();
			base.Dispose(disposing);
		}

		protected virtual void OnBookChanged()
		{
			if (this.BookChanged != null)
			{
				this.BookChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnDrawnPageCountChanged()
		{
			UpdateCurrentPageOverlay();
			if (this.DrawnPageCountChanged != null)
			{
				this.DrawnPageCountChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnBrowse(BrowseEventArgs e)
		{
			if (this.Browse != null)
			{
				this.Browse(this, e);
			}
		}

		protected virtual void OnPageChange(BookPageEventArgs e)
		{
			if (this.PageChange != null)
			{
				this.PageChange(this, e);
			}
		}

		protected virtual void OnPageChanged(BookPageEventArgs e)
		{
			currentMousePage = -1;
			if (this.PageChanged != null)
			{
				this.PageChanged(this, e);
			}
		}

		protected virtual void OnVisibleInfoOverlaysChanged()
		{
			if (this.VisibleInfoOverlaysChanged != null)
			{
				this.VisibleInfoOverlaysChanged(this, EventArgs.Empty);
			}
		}

		private void CreateWorkingPaperTexture()
		{
			workingPaperTexture.SafeDispose();
			workingPaperTexture = null;
			if (paperTextureBitmap != null && !(paperTextureStrength < 0.05f))
			{
				workingPaperTexture = new Bitmap(paperTextureBitmap.Width, paperTextureBitmap.Height);
				using (Graphics graphics = Graphics.FromImage(workingPaperTexture))
				{
					graphics.Clear(Color.White);
					graphics.DrawImage(PaperTextureBitmap, 0, 0, paperTextureStrength);
				}
			}
		}

		private int GetPageFromPoint(Point pt)
		{
			pt = ClientToImage(pt, withOffset: false);
			for (int i = 0; i < DisplayedPageAreas.Length; i++)
			{
				Rectangle rectangle = DisplayedPageAreas[i];
				if (rectangle.Contains(pt))
				{
					return DisplayedPages[i];
				}
			}
			return CurrentPage;
		}

		private Size CalcNavigationOverlaySize()
		{
			float num = (EngineConfiguration.Default.NavigationPanelWidth * infoOverlayScaling).Clamp(0.2f, 1f);
			return new Size(Math.Max(200, (int)((float)base.ClientRectangle.Width * num)), Math.Max((int)((float)NavigationOverlayDefaultHeight * infoOverlayScaling), 120));
		}

		private static void DrawPageBow(IBitmapRenderer gr, RectangleF rect, bool rightSide)
		{
			rect.Inflate(2f, 0f);
			gr.DrawImage(rightSide ? pageBowRight : pageBowLeft, rect, new RectangleF(0f, 0f, pageBowLeft.Width - 1, pageBowLeft.Height - 1), BitmapAdjustment.Empty, 1f);
		}

		private RectangleF DrawPageOrnaments(IBitmapRenderer gr, Rectangle destination, Rectangle source, RectangleF ri1, RectangleF ri2, bool leftOk, bool rightOk, bool fillLeft, bool fillRight)
		{
			float pageBowWidth = EngineConfiguration.Default.PageBowWidth;
			bool pageBowBorder = EngineConfiguration.Default.PageBowBorder;
			bool pageBowCenter = EngineConfiguration.Default.PageBowCenter;
			float scaleX = (float)destination.Width / (float)source.Width;
			float scaleY = (float)destination.Height / (float)source.Height;
			float num = ri1.Top - (float)source.Y;
			float height = ri1.Height;
			RectangleF rectangleF = Rectangle.Empty;
			if (leftOk)
			{
				RectangleF rectangleF2 = new RectangleF((float)destination.X + ri1.Left - (float)source.X, (float)destination.Y + num, ri1.Width, ri1.Height).Scale(scaleX, scaleY);
				rectangleF = rectangleF.Union(rectangleF2);
				if (fillLeft)
				{
					gr.FillRectangle(rectangleF2, BlankPageColor);
				}
				if (RealisticPages)
				{
					if (pageBowBorder)
					{
						DrawPageBow(gr, new RectangleF((float)destination.X + ri1.Left - (float)source.X, (float)destination.Y + num, ri1.Width * pageBowWidth, height).Scale(scaleX, scaleY), rightSide: false);
					}
					if (pageBowCenter || !rightOk)
					{
						DrawPageBow(gr, new RectangleF((float)destination.X + (ri1.Right - (float)source.X) * (1f - innerBowLeftOffsetInPercent) - ri1.Width * pageBowWidth, (float)destination.Y + num, ri1.Width * pageBowWidth, height).Scale(scaleX, scaleY), rightSide: true);
					}
					gr.DrawRectangle(rectangleF2, Color.Black, 1f);
				}
			}
			if (rightOk)
			{
				RectangleF rectangleF3 = new RectangleF((float)destination.X + ri1.Right + ri2.Right - ri2.Width - (float)source.X, (float)destination.Y + num, ri2.Width, ri1.Height).Scale(scaleX, scaleY);
				rectangleF = rectangleF.Union(rectangleF3);
				if (fillRight)
				{
					gr.FillRectangle(rectangleF3, BlankPageColor);
				}
				if (RealisticPages)
				{
					if (pageBowBorder)
					{
						DrawPageBow(gr, new RectangleF((float)destination.X + ri1.Right + ri2.Right - ri2.Width * pageBowWidth - (float)source.X, (float)destination.Y + num, ri2.Width * pageBowWidth, height).Scale(scaleX, scaleY), rightSide: true);
					}
					if (pageBowCenter || !leftOk)
					{
						float num2 = ri2.Width * pageBowWidth;
						float num3 = ri2.Width * innerBowRightOffsetInPercent;
						if (ri1.Width - num3 < num2)
						{
							num3 = 0f;
						}
						RectangleF rect = new RectangleF((float)destination.X + ri1.Right - (float)source.X + num3, (float)destination.Y + num, num2, height).Scale(scaleX, scaleY);
						DrawPageBow(gr, rect, rightSide: false);
					}
					gr.DrawRectangle(rectangleF3, Color.Black, 1f);
				}
			}
			if (RealisticPages && !rectangleF.IsEmpty)
			{
				int num4 = (int)(Math.Min(rectangleF.Width, rectangleF.Height) * EngineConfiguration.Default.PageShadowWidthPercentage / 100f).Clamp(0f, 255f);
				if (num4 != 0)
				{
					float maxOpacity = EngineConfiguration.Default.PageShadowOpacity.Clamp(0f, 1f);
					if (shadowBitmap == null)
					{
						shadowBitmap = GraphicsExtensions.CreateShadowBitmap(BlurShadowType.Outside, Color.Black, 64, maxOpacity);
					}
					gr.DrawShadow(rectangleF.Pad(-num4), shadowBitmap, num4, BlurShadowParts.Edges);
				}
			}
			return rectangleF;
		}

		private ImageInfo GetImageInfo(int page, PageImage image1, PageImage image2)
		{
			ImageInfo imageInfo = new ImageInfo();
			if (!IsValid)
			{
				return imageInfo;
			}
			try
			{
				int num = SeekPage(page, 1);
				if (!TwoPageDisplay || IsPageSingleType(page) || num == -1 || (image1 != null && image1.Width > image1.Height) || image2 == null || image2.Width > image2.Height || IsPageSingleType(num))
				{
					if (image1 != null)
					{
						Size size = image1.Size;
						if (PageLayout == PageLayoutMode.Double && size.Height > size.Width)
						{
							imageInfo.IsForcedDoublePage = true;
							size.Width += (int)((float)size.Width * (1f - DoublePageOverlap));
						}
						imageInfo.ImageCount = 1;
						imageInfo.Size = size;
						return imageInfo;
					}
					return imageInfo;
				}
				if (image1 != null)
				{
					if (image2 != null)
					{
						int num2 = Math.Max(image1.Height, image2.Height);
						int num3 = image1.Width * num2 / image1.Height;
						int num4 = image2.Width * num2 / image2.Height;
						imageInfo.ImageCount = 2;
						imageInfo.Size = new Size(num3 + num4 - (int)(DoublePageOverlap * (float)num3), num2);
						return imageInfo;
					}
					return imageInfo;
				}
				return imageInfo;
			}
			catch
			{
				return imageInfo;
			}
		}

		private ImageInfo GetImageInfo(int page)
		{
			using (IItemLock<PageImage> itemLock3 = GetImage(page))
			{
				IItemLock<PageImage> itemLock2;
				if (!TwoPageDisplay)
				{
					IItemLock<PageImage> itemLock = new ItemLock<PageImage>(null);
					itemLock2 = itemLock;
				}
				else
				{
					itemLock2 = GetImage(SeekPage(page, 1));
				}
				using (IItemLock<PageImage> itemLock4 = itemLock2)
				{
					return GetImageInfo(page, itemLock3.Item, itemLock4.Item);
				}
			}
		}

		private ImageInfo GetImageInfo()
		{
			return GetImageInfo(CurrentPage);
		}

		private bool IsPageSingleType(int page)
		{
			if (!IsValid)
			{
				return false;
			}
			try
			{
				if (page >= Book.Comic.PageCount)
				{
					return false;
				}
				return Book.Comic.GetPage(page).IsSinglePageType;
			}
			catch
			{
				return false;
			}
		}

		private bool IsPageSingleRightType(int page)
		{
			if (!IsValid)
			{
				return false;
			}
			try
			{
				if (page >= Book.Comic.PageCount)
				{
					return false;
				}
				return Book.Comic.GetPage(page).IsSingleRightPageType;
			}
			catch
			{
				return false;
			}
		}

		private void PositionMagnifier(Point location)
		{
			magnifierOverlay.CenterLocation = location;
			UpdateMagnifierVisibility();
		}

		private void UpdateMagnifierVisibility()
		{
			if (true.Equals(magnifierOverlay.Tag))
			{
				magnifierOverlay.Visible = MagnifierVisible;
			}
			else
			{
				Rectangle clientRectangle = base.ClientRectangle;
				clientRectangle.Inflate(-32, -32);
				magnifierOverlay.Visible = MagnifierVisible && (!autoHideMagnifier || clientRectangle.Contains(magnifierOverlay.CenterLocation));
			}
			if (magnifierOverlay.Visible)
			{
				navigationOverlayVisible = false;
			}
		}

		private void PositionMagnifier()
		{
			PositionMagnifier(PointToClient(Cursor.Position));
		}

		private PageKey GetPageKey(int page)
		{
			if (!IsValid)
			{
				return null;
			}
			PageKey pageKey = Book.GetPageKey(page);
			pageKey.Source = this;
			return pageKey;
		}

		private bool IsPageInCache(int page, int offset = 0, bool fastMem = true, bool putInCache = true)
		{
			int num = SeekPage(page, offset);
			if (num == -1 || num == page)
			{
				return true;
			}
			PageKey pageKey = GetPageKey(num);
			using (IItemLock<PageImage> itemLock = pagePool.GetPage(pageKey, fastMem))
			{
				if (itemLock != null && itemLock.Item != null)
				{
					return true;
				}
			}
			if (putInCache && Book != null)
			{
				pagePool.CachePage(pageKey, fastMem, Book, bottom: false);
			}
			return false;
		}

		private int CachePage(int page, int offset, bool fastMem, bool bottom)
		{
			int num = SeekPage(page, offset);
			if (num == page)
			{
				num = -1;
			}
			if (num != -1)
			{
				pagePool.CachePage(GetPageKey(num), fastMem, Book, bottom);
			}
			return num;
		}

		private bool CacheBackPage(ref int page, int offset)
		{
			int num = CachePage(page, offset, fastMem: true, bottom: true);
			if (num == -1)
			{
				return false;
			}
			page = num;
			return true;
		}

		private int SeekPage(int page, int offset)
		{
			if (Book == null)
			{
				return -1;
			}
			return Book.SeekNextPage(page, Math.Abs(offset), Math.Sign(offset));
		}

		public IItemLock<PageImage> GetImage(int page, bool withCaching = false)
		{
			bool flag = PreCache && withCaching;
			if (!IsValid || page < 0 || page > Book.ProviderPageCount)
			{
				return new ItemLock<PageImage>(null);
			}
			PageKey pageKey = GetPageKey(page);
			IItemLock<PageImage> page2 = pagePool.GetPage(pageKey, onlyMemory: true);
			if (page2 == null)
			{
				PagePool.CachePage(pageKey, fastMem: true, book, bottom: false);
			}
			if (flag)
			{
				CachePage(page, 1, fastMem: true, bottom: false);
				InvalidatePendingImageCacheUpdate();
			}
			if (page2 != null)
			{
				lastValidKey = pageKey;
				firstPageHasBeenLoaded = true;
				page2.Tag = true;
				return page2;
			}
			if (lastValidKey != null)
			{
				page2 = pagePool.GetPage(lastValidKey, onlyMemory: false);
			}
			return page2 ?? new ItemLock<PageImage>(null);
		}

		private void InvalidatePendingImageCacheUpdate()
		{
			cacheUpdateTimer.Stop();
			cacheUpdateTimer.Start();
		}

		private void StopPendingImageCacheUpdate()
		{
			cacheUpdateTimer.Stop();
		}

		private void cacheUpdateTimer_Tick(object sender, EventArgs e)
		{
			cacheUpdateTimer.Stop();
			if (!IsValid)
			{
				return;
			}
			try
			{
				int num = CurrentPage;
				int num2 = (pagePool.MaximumMemoryItems - 15) / 2;
				int page = CachePage(num, 1, fastMem: true, bottom: false);
				int page2 = num;
				bool flag = page != -1;
				bool flag2 = true;
				while (num2 > 0 && (!flag || !(flag = CacheBackPage(ref page, 1)) || --num2 != 0) && (!flag || !(flag = CacheBackPage(ref page, 1)) || --num2 != 0) && (!flag2 || !(flag2 = CacheBackPage(ref page2, -1)) || --num2 != 0) && (flag || flag2))
				{
				}
			}
			catch (Exception)
			{
			}
		}

		private Matrix4 GetMatrix(System.Drawing.Drawing2D.Matrix matrix)
		{
			float[] elements = matrix.Elements;
			return new Matrix4(elements[0], elements[2], 0f, elements[4], elements[1], elements[3], 0f, elements[5], 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f);
		}

		private System.Drawing.Drawing2D.Matrix GetMatrix(Matrix4 matrix)
		{
			return new System.Drawing.Drawing2D.Matrix(matrix[0], matrix[1], matrix[4], matrix[5], matrix[12], matrix[13]);
		}

		private bool DrawPage(IBitmapRenderer gr, IItemLock<PageImage> il, RectangleF rd, RectangleF rs)
		{
			float opacity = 1f;
			if (il.Tag is bool && (bool)il.Tag)
			{
				if (drawBlankPagesOverride)
				{
					gr.FillRectangle(rd, EngineConfiguration.Default.BlankPageColor);
					opacity = 0.1f;
				}
				try
				{
					bool flag = false;
					if (gr.IsHardware && SoftwareFiltering && !inBlendAnmation)
					{
						Matrix4 matrix = GetMatrix(gr.Transform);
						Matrix4.Decompose(matrix, out var _, out var scaling, out var _);
						float num = (float)Math.Round(scaling[0], 4);
						float num2 = (float)Math.Round(scaling[1], 4);
						float softwareFilterMinScale = EngineConfiguration.Default.SoftwareFilterMinScale;
						if (num >= softwareFilterMinScale && num2 >= softwareFilterMinScale && (num < 1f || num2 < 1f))
						{
							using (IItemLock<ScaledPageItem> itemLock = GetScaledPage(il.Item, num, num2))
							{
								if (itemLock != null && itemLock.Item.IsValid)
								{
									ScaledPageItem item = itemLock.Item;
									Size size = il.Item.Size;
									num = (float)item.Width / (float)size.Width;
									num2 = (float)item.Height / (float)size.Height;
									rs.X *= num;
									rs.Y *= num2;
									rs.Width *= num;
									rs.Height *= num2;
									rd.Width *= num / scaling[0];
									rd.Height *= num2 / scaling[1];
									((IHardwareRenderer)gr).OptimizedTextures = true;
									try
									{
										gr.DrawImage(item, rd, rs, BitmapAdjustment.Empty, opacity);
									}
									finally
									{
										((IHardwareRenderer)gr).OptimizedTextures = false;
									}
									flag = true;
								}
							}
						}
					}
					if (!flag)
					{
						gr.DrawImage(il.Item, rd, rs, BitmapAdjustment.Empty, opacity);
					}
				}
				catch (Exception)
				{
				}
				return true;
			}
			gr.FillRectangle(rd, Color.FromArgb(128, BackColor));
			return false;
		}

		private IItemLock<ScaledPageItem> GetScaledPage(PageImage bmp, float sx, float sy)
		{
			if (bmp == null)
			{
				return null;
			}
			if (scaledCache == null)
			{
				scaledCache = new Cache<ScaledPageKey, ScaledPageItem>(2);
				scaledCache.MinimalTimeInCache = 0;
				scaledCache.ItemRemoved += delegate(object s, CacheItemEventArgs<ScaledPageKey, ScaledPageItem> e)
				{
					e.Item.Dispose();
				};
			}
			ScaledPageKey key = new ScaledPageKey
			{
				ScaleX = sx,
				ScaleY = sy,
				Bitmap = bmp
			};
			IItemLock<ScaledPageItem> itemLock = scaledCache.LockItem(key, (ScaledPageKey b) => new ScaledPageItem());
			int num = EngineConfiguration.Default.SoftwareFilterDelay.Clamp(100, 5000);
			long ticks = Machine.Ticks;
			if (!itemLock.Item.IsValid)
			{
				if (itemLock.Item.Ticks != 0L && ticks - itemLock.Item.Ticks > num)
				{
					Size size = new Size((int)Math.Round((float)bmp.Width * sx), (int)Math.Round((float)bmp.Height * sy));
					itemLock.Item.Optimized = PageImage.MemoryOptimized;
					itemLock.Item.Bitmap = bmp.Bitmap.Resize(size, EngineConfiguration.Default.SoftwareFilter);
				}
				else
				{
					itemLock.Item.Ticks = ticks;
					imageScaleTimer.Interval = num + 100;
					imageScaleTimer.Stop();
					imageScaleTimer.Start();
				}
			}
			return itemLock;
		}

		private void imageScaleTimer_Tick(object sender, EventArgs e)
		{
			imageScaleTimer.Stop();
			Invalidate();
		}

		public override Bitmap CreatePageImage()
		{
			bool flag = RealisticPages;
			RealisticPages = false;
			try
			{
				return base.CreatePageImage();
			}
			finally
			{
				RealisticPages = flag;
			}
		}

		protected override void DrawImage(IBitmapRenderer gr, Rectangle destination, Rectangle source, bool clipToDestination)
		{
			if (!IsValid)
			{
				return;
			}
			int num = displayHash;
			int num2 = CurrentPage;
			int nextPage = NextPage;
			int[] array = new int[0];
			Rectangle[] array2 = new Rectangle[0];
			using (IItemLock<PageImage> itemLock = GetImage(num2, withCaching: true))
			{
				using (IItemLock<PageImage> itemLock2 = GetImage(nextPage, withCaching: true))
				{
					if (itemLock.Item == null)
					{
						displayHash = 0;
					}
					else
					{
						ImageInfo imageInfo = GetImageInfo(num2, itemLock.Item, itemLock2.Item);
						SizeF sizeF = imageInfo.Size;
						if (!clipToDestination)
						{
							float num3 = (float)source.Width / (float)destination.Width;
							float num4 = (float)source.Height / (float)destination.Height;
							destination = destination.Pad(-(int)(num3 * (float)source.X), -(int)(num4 * (float)source.Top), -(int)(num3 * (sizeF.Width - (float)source.Right)), -(int)(num4 * (sizeF.Height - (float)source.Bottom)));
							source = new Rectangle(Point.Empty, sizeF.ToSize());
						}
						if (imageInfo.IsSingleImage && !imageInfo.IsForcedDoublePage)
						{
							DrawPage(gr, itemLock, destination, source);
							if (RealisticPages)
							{
								if (imageInfo.IsDoublePage)
								{
									RectangleF rectangleF = new RectangleF(0f, 0f, (float)itemLock.Item.Width / 2f, itemLock.Item.Height);
									displayedPageBounds = DrawPageOrnaments(gr, destination, source, rectangleF, rectangleF, leftOk: true, rightOk: true, fillLeft: false, fillRight: false);
								}
								else
								{
									RectangleF rectangleF2 = new RectangleF(0f, 0f, itemLock.Item.Width, itemLock.Item.Height);
									displayedPageBounds = DrawPageOrnaments(gr, destination, source, rectangleF2, rectangleF2, leftOk: true, rightOk: false, fillLeft: false, fillRight: false);
								}
							}
							displayHash = itemLock.Item.GetHashCode();
							array = new int[1]
							{
								num2
							};
							array2 = new Rectangle[1]
							{
								destination
							};
						}
						else
						{
							bool flag = base.RightToLeftReading && base.RightToLeftReadingMode == RightToLeftReadingMode.FlipPages;
							bool flag2 = IsPageSingleType(num2) && nextPage == -1;
							bool flag3 = ((num2 != 0 && !IsPageSingleRightType(num2) && flag) || flag2) ^ IsFlipped;
							bool a = !imageInfo.IsSingleImage;
							bool b = true;
							bool flag4 = false;
							bool flag5 = false;
							IItemLock<PageImage> a2 = itemLock;
							IItemLock<PageImage> b2 = itemLock2;
							if (imageInfo.IsSingleImage)
							{
								b2 = a2;
							}
							if (flag3)
							{
								CloneUtility.Swap(ref a2, ref b2);
								CloneUtility.Swap(ref a, ref b);
							}
							Size size = a2.Item.Size;
							Size size2 = b2.Item.Size;
							float num5 = sizeF.Height / (float)size.Height;
							float num6 = sizeF.Height / (float)size2.Height;
							RectangleF ri = new RectangleF(0f, 0f, size.Width, size.Height).Scale(num5);
							RectangleF ri2 = new RectangleF(0f, 0f, size2.Width, size2.Height).Scale(num6);
							float num7 = DoublePageOverlap * ri.Width;
							if (num2 == 0 || flag2)
							{
								flag3 = !flag3;
							}
							if (flag3)
							{
								ri.Width -= num7;
							}
							else
							{
								ri2.Width -= num7;
							}
							RectangleF rect = new RectangleF(source.X, source.Y, Math.Min(ri.Right, source.Right) - (float)source.X, source.Height);
							RectangleF rect2 = new RectangleF(rect.Right - ri.Width, rect.Y, (float)source.Right - rect.Right, rect.Height);
							if (!flag3)
							{
								rect2.X += num7;
							}
							RectangleF rect3 = new RectangleF((float)destination.X + rect.Left / (float)source.Width, destination.Y, (float)destination.Width * rect.Width / (float)source.Width, destination.Height);
							RectangleF rect4 = new RectangleF(rect3.Right, destination.Y, (float)destination.Right - rect3.Right, destination.Height);
							rect = rect.Scale(1f / num5);
							rect2 = rect2.Scale(1f / num6);
							using (gr.SaveState())
							{
								if (a)
								{
									gr.ScaleTransform(num5, num5);
									a = DrawPage(gr, a2, rect3.Scale(1f / num5), rect);
								}
								else
								{
									flag4 = num2 != 0 && !flag2;
								}
							}
							using (gr.SaveState())
							{
								if (b)
								{
									gr.ScaleTransform(num6, num6);
									b = DrawPage(gr, b2, rect4.Scale(1f / num6), rect2);
								}
								else
								{
									flag5 = num2 != 0 && !flag2;
								}
							}
							displayedPageBounds = DrawPageOrnaments(gr, destination, source, ri, ri2, a || flag4, b || flag5, flag4, flag5);
							displayHash = a2.Item.GetHashCode() ^ (b2.Item.GetHashCode() << 1);
							List<int> list = new List<int>();
							List<Rectangle> list2 = new List<Rectangle>();
							if (a)
							{
								list.Add(num2);
								list2.Add(rect3.Round());
							}
							if (b)
							{
								list.Add(a ? nextPage : num2);
								list2.Add(rect4.Round());
							}
							array = list.ToArray();
							array2 = list2.ToArray();
							if (flag3)
							{
								Array.Reverse(array);
							}
						}
					}
				}
			}
			if (num != displayHash)
			{
				OnDisplayChanged();
			}
			ComicBookNavigator comicBookNavigator = Book;
			if (comicBookNavigator != null)
			{
				int num8 = ((nextPage != -1) ? nextPage : num2);
				if (num8 > comicBookNavigator.LastPageRead)
				{
					comicBookNavigator.LastPageRead = num8;
				}
			}
			displayedPages = array;
			displayedPageAreas = array2;
		}

		protected override void RenderImageEffect(IBitmapRenderer bitmapRenderer, DisplayOutput display)
		{
			base.RenderImageEffect(bitmapRenderer, display);
			if (bitmapRenderer.IsHardware && workingPaperTexture != null)
			{
				IHardwareRenderer hardwareRenderer = bitmapRenderer as IHardwareRenderer;
				hardwareRenderer.BlendingOperation = BlendingOperation.Multiply;
				try
				{
					hardwareRenderer.FillRectangle(workingPaperTexture, PaperTextureLayout, displayedPageBounds, paperTextureBitmap.Size.ToRectangle(), BitmapAdjustment.Empty, 1f);
				}
				catch (Exception)
				{
				}
				hardwareRenderer.BlendingOperation = BlendingOperation.Blend;
			}
		}

		protected virtual void OnDisplayChanged()
		{
			if (!base.DisplayEventsDisabled)
			{
				UpdatePartOverlay(always: true);
				navigationOverlay.IsDoublePage = IsDoubleImage;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (e.Button == MouseButtons.Left && IsMouseOk(e.Location))
			{
				if (!MagnifierVisible)
				{
					mouseDown = e.Location;
					longClickTimer.Start();
				}
				if (NavigationOverlayVisible)
				{
					NavigationOverlayVisible = false;
					base.MouseActionHappened = true;
				}
				ShowGestureIndicator(e.Location);
			}
			currentMousePage = GetPageFromPoint(e.Location);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (!mouseDown.IsEmpty)
			{
				mouseDown = Point.Empty;
				longClickTimer.Stop();
			}
			if (true.Equals(magnifierOverlay.Tag))
			{
				MagnifierVisible = false;
				magnifierOverlay.Tag = false;
				base.DisableScrolling = false;
			}
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!mouseDown.IsEmpty && (Math.Abs(mouseDown.X - e.X) > 5 || Math.Abs(mouseDown.Y - e.Y) > 5))
			{
				longClickTimer.Stop();
				mouseDown = Point.Empty;
			}
			if (magnifierOverlay.Visible)
			{
				Cursor = (Cursor.Current = EmptyCursor);
			}
			else
			{
				Cursor = (Cursor.Current = Cursors.Default);
			}
			UpdateNavigationOverlay();
			PositionMagnifier(e.Location);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			longClickTimer.Stop();
			UpdateNavigationOverlay(new Point(-1, -1));
			PositionMagnifier();
		}

		protected override void OnGestureStart()
		{
			base.MouseActionHappened = false;
			base.OnGestureStart();
			if (magnifierOverlay.Visible && !magnifierOverlay.Bounds.Contains(base.GestureLocation))
			{
				MagnifierVisible = false;
			}
		}

		protected override void OnPanStart()
		{
			base.OnPanStart();
			panMagnifier = false;
			if (magnifierOverlay.Visible && magnifierOverlay.Bounds.Contains(base.GestureLocation))
			{
				panMagnifier = true;
				base.MouseActionHappened = true;
			}
		}

		protected override void OnPan()
		{
			base.OnPan();
			if (panMagnifier)
			{
				PositionMagnifier(base.PanLocation);
				base.MouseActionHappened = true;
			}
		}

		protected override void OnPageDisplayModeChanged()
		{
			base.OnPageDisplayModeChanged();
			UpdatePartOverlay(always: true);
		}

		protected override void OnVisiblePartChanged()
		{
			base.OnVisiblePartChanged();
			UpdatePartOverlay(always: false);
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			navigationOverlay.Size = CalcNavigationOverlaySize();
			if (navigationOverlay.Visible)
			{
				navigationOverlay.Y = NavigationOverlayVisibleY;
				navigationOverlay.X = (base.ClientRectangle.Width - navigationOverlay.Width) / 2;
			}
			UpdatePartOverlay(always: true);
		}

		protected override void OnRenderImageOverlay(RenderEventArgs e)
		{
			base.OnRenderImageOverlay(e);
			UpdateCurrentPageOverlay();
			UpdateMessageOverlay();
			if (messageOverlay.Visible)
			{
				loadPageOverlay.Visible = false;
			}
			else
			{
				UpdateLoadPageOverlay();
			}
			overlayManager.Draw(e.Graphics);
			if (blindOut)
			{
				e.Graphics.FillRectangle(base.ClientRectangle, blindOutColor);
			}
		}

		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
			case Keys.Tab:
			case Keys.End:
			case Keys.Home:
			case Keys.Left:
			case Keys.Up:
			case Keys.Right:
			case Keys.Down:
			case Keys.Tab | Keys.Shift:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		protected override Color GetAutoBackgroundColor()
		{
			try
			{
				if (IsValid)
				{
					using (IItemLock<PageImage> itemLock = GetImage(CurrentPage))
					{
						if (itemLock.Item.BackgrounColor.IsEmpty)
						{
							Bitmap bitmap = ((itemLock.Item != null) ? itemLock.Item.Bitmap : null);
							if (bitmap != null)
							{
								Color[] array = new Color[4]
								{
									bitmap.GetAverageColor(2, 2, 4),
									bitmap.GetAverageColor(bitmap.Width - 2 - 4, 2, 4),
									bitmap.GetAverageColor(bitmap.Width - 2 - 4, bitmap.Height - 2 - 4, 4),
									bitmap.GetAverageColor(2, bitmap.Height - 2 - 4, 4)
								};
								if (array.GetAverage().GetBrightness() < 0.5f)
								{
									itemLock.Item.BackgrounColor = array.Max((Color a, Color b) => a.GetBrightness().CompareTo(b.GetBrightness()));
								}
								else
								{
									itemLock.Item.BackgrounColor = array.Max((Color a, Color b) => b.GetBrightness().CompareTo(a.GetBrightness()));
								}
							}
						}
						return itemLock.Item.BackgrounColor;
					}
				}
			}
			catch
			{
			}
			return Color.Empty;
		}

		protected override bool IsImageValid()
		{
			return GetImageInfo().IsValid;
		}

		protected override Size GetImageSize()
		{
			return GetImageInfo().Size;
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			if (!MouseHandled)
			{
				base.OnDoubleClick(e);
			}
		}

		protected override bool IsMouseOk(Point point)
		{
			return overlayManager.Panels.Find((OverlayPanel x) => x.HasMouse) == null;
		}

		protected override void OnImageDisplayOptionsChanged()
		{
			UpdateNavigationOverlay(redraw: false);
		}

		protected override void OnReadingModeChanged()
		{
			base.OnReadingModeChanged();
			navigationOverlay.Mirror = IsMovementFlipped;
		}

		private void ShowGestureIndicator(Point pt)
		{
			if (!EngineConfiguration.Default.ShowGestureHint)
			{
				return;
			}
			GestureArea gestureArea = GestureHitTest(pt);
			if (gestureArea != null)
			{
				GestureEventArgs gestureEventArgs = new GestureEventArgs(GestureType.Touch)
				{
					Area = gestureArea.Alignment,
					AreaBounds = gestureArea.Area,
					Double = false
				};
				OnPreviewGesture(gestureEventArgs);
				if (!gestureEventArgs.Handled)
				{
					gestureEventArgs.Double = true;
					OnPreviewGesture(gestureEventArgs);
				}
				if (gestureEventArgs.Handled)
				{
					gestureOverlay.Alignment = gestureArea.Alignment;
					gestureOverlay.Size = gestureArea.Area.Size;
					gestureOverlay.Opacity = 1f;
					Update();
					gestureOverlay.Animators[0].Start();
				}
			}
		}

		public void DisplayOpenMessage()
		{
			firstPageHasBeenLoaded = false;
		}

		private void UpdateNavigationOverlay(bool redraw)
		{
			if (IsValid)
			{
				navigationOverlay.Pages = Book.GetPages().ToArray();
				if ((Control.MouseButtons & MouseButtons.Left) == 0)
				{
					navigationOverlay.DisplayedPageIndex = ((IList)navigationOverlay.Pages).IndexOf((object)CurrentPage);
				}
				navigationOverlay.IsDoublePage = IsDoubleImage;
				navigationOverlay.Caption = book.Comic.Caption;
				if (redraw)
				{
					navigationOverlay.Invalidate();
				}
			}
		}

		private void UpdatePartOverlay(bool always)
		{
			int num = CurrentPage * 100 + base.ImageVisiblePart.Part;
			Point offset = base.ImageVisiblePart.Offset;
			if (num == cachedPartOverlay && cachedPartOffset == offset && !always)
			{
				return;
			}
			using (ItemMonitor.Lock(visiblePartOverlay))
			{
				if (ImagePartCount != 1 || visiblePartOverlay.IsVisible)
				{
					if (!IsImageValid())
					{
						smallBitmap.SafeDispose();
						smallBitmap = null;
					}
					else if (IsPartInfoOverlayEnabled)
					{
						visiblePartOverlay.Animators[0].Start();
					}
				}
			}
			cachedPartOverlay = num;
			cachedPartOffset = offset;
		}

		private void UpdateCurrentPageOverlay()
		{
			UpdateCurrentPageOverlay(DisplayedPages);
		}

		private void UpdateCurrentPageOverlay(IEnumerable<int> pageNumbers)
		{
			if (Book == null || pageNumbers == null)
			{
				return;
			}
			int[] array = pageNumbers.Where((int n) => n >= 0).ToArray();
			if (array.Length == 0 || currentPageOverlayHash == DisplayHash)
			{
				return;
			}
			currentPageOverlayHash = DisplayHash;
			string number = ((array.Length == 1) ? (array[0] + 1).ToString() : $"{array[0] + 1}/{array[1] + 1}");
			string text = ComicBook.FormatNumber(number, Book.IsIndexRetrievalCompleted ? Book.ProviderPageCount : (-1));
			if (CurrentPageShowsName)
			{
				text += "<small>";
				text = text.AppendWithSeparator("<br/>", Book.GetImageName(Book.Comic.TranslatePageToImageIndex(array[0]), noPath: true).ToXmlString());
				if (array.Length > 1)
				{
					text = text.AppendWithSeparator("<br/>", Book.GetImageName(Book.Comic.TranslatePageToImageIndex(array[1]), noPath: true).ToXmlString());
				}
				text += "</small>";
			}
			currentPageOverlay.Text = text;
			if (IsCurrentPageOverlayEnabled)
			{
				currentPageOverlay.Animators[0].Start();
			}
		}

		private void UpdateLoadPageOverlay()
		{
			if (!IsLoadPageOverlayEnabled || !IsValid || CurrentPage < 0)
			{
				loadPageOverlay.Visible = false;
				return;
			}
			int page = CurrentPage;
			int nextPage = NextPage;
			bool flag = false;
			bool flag2 = false;
			using (IItemLock<PageImage> itemLock = GetImage(page))
			{
				using (IItemLock<PageImage> itemLock2 = GetImage(NextPage))
				{
					ImageInfo imageInfo = GetImageInfo(page, itemLock.Item, itemLock2.Item);
					flag = itemLock.Item == null || !true.Equals(itemLock.Tag);
					if (!imageInfo.IsSingleImage)
					{
						flag2 = itemLock2.Item == null || !true.Equals(itemLock2.Tag);
					}
				}
			}
			if (!flag && !flag2)
			{
				loadPageOverlay.Visible = false;
				return;
			}
			string text = string.Empty;
			if (flag)
			{
				text = (CurrentPage + 1).ToString();
			}
			if (flag2 && nextPage != -1)
			{
				text = text.AppendWithSeparator(", ", (nextPage + 1).ToString());
			}
			if (!string.IsNullOrEmpty(text))
			{
				UpdateLoadPageOverlay(text);
			}
		}

		private void UpdateLoadPageOverlay(string pageText)
		{
			if (Book != null && !string.IsNullOrEmpty(pageText))
			{
				loadPageOverlay.Text = string.Format(TR.Messages["LoadingPage", "Loading Page {0}..."], pageText);
				loadPageOverlay.Visible = true;
			}
		}

		private void UpdateMessageOverlay()
		{
			string text = null;
			Bitmap bitmap = null;
			if (Book == null)
			{
				text = TR.Messages["NoComicOpen", "No book is open"];
			}
			else
			{
				if (Book.ProviderStatus == ImageProviderStatus.Error)
				{
					text = StringUtility.Format(TR.Messages["OpenError", "Could not open the book '{0}'!"], Book.Comic.DisplayFileLocation);
				}
				else if (!firstPageHasBeenLoaded)
				{
					text = StringUtility.Format(TR.Messages["OpeningComic", "Opening the book '{0}'..."], Book.Comic.DisplayFileLocation);
				}
				if (text != null && ThumbnailPool != null)
				{
					IItemLock<ThumbnailImage> thumbnail;
					using (thumbnail = ThumbnailPool.GetThumbnail(Book.Comic.GetFrontCoverThumbnailKey(), onlyMemory: true))
					{
						if (thumbnail != null && thumbnail.Item != null)
						{
							if (messageOverlay.Tag == thumbnail.Item && messageOverlay.Icon != null)
							{
								bitmap = messageOverlay.Icon;
							}
							else
							{
								messageOverlay.Tag = thumbnail.Item;
								bitmap = ComicBox3D.CreateDefaultBook(thumbnail.Item.GetThumbnail(128), null, new Size(128, 128), Book.Comic.PageCount);
							}
						}
					}
				}
			}
			if (messageOverlay.Icon != bitmap)
			{
				Bitmap icon = messageOverlay.Icon;
				messageOverlay.Icon = bitmap;
				icon?.Dispose();
			}
			messageOverlay.Text = text;
			messageOverlay.Visible = !string.IsNullOrEmpty(text) && showStatusMessage;
		}

		private void DrawMagnifier(IBitmapRenderer gr, Point location, Rectangle mrc, float zoom)
		{
			DisplayOutput displayOutput = base.LastRenderedDisplay ?? base.Display;
			Rectangle rectangle = mrc;
			int num3 = (mrc.Width = (mrc.Height = Math.Max(mrc.Height, mrc.Width)));
			Rectangle source = mrc;
			source.Width = (int)((float)source.Width / displayOutput.Scale.Width / zoom);
			source.Height = (int)((float)source.Height / displayOutput.Scale.Height / zoom);
			source.Offset(ClientToImage(displayOutput, location));
			source.Offset(-source.Width / 2, -source.Height / 2);
			using (gr.SaveState())
			{
				using (gr.SaveState())
				{
					gr.TranslateTransform((float)rectangle.Width / 2f, (float)rectangle.Height / 2f);
					gr.ScaleTransform(zoom, zoom);
					gr.TranslateTransform(-location.X, -location.Y);
					RenderImageBackground(gr, null);
				}
				gr.TranslateTransform((float)mrc.Width / 2f - (float)(mrc.Width - rectangle.Width) / 2f, (float)mrc.Height / 2f - (float)(mrc.Height - rectangle.Height) / 2f);
				gr.RotateTransform(displayOutput.Config.Rotation.ToDegrees());
				gr.TranslateTransform((float)(-mrc.Width) / 2f, (float)(-mrc.Height) / 2f);
				DrawImage(gr, mrc, source, clipToDestination: true);
				RenderImageEffect(gr, null);
			}
		}

		private void magnifierOverlay_RenderSurface(object sender, PanelRenderEventArgs e)
		{
			Magnifier magnifier = magnifiers[(int)MagnifierStyle];
			IBitmapRenderer renderer = e.Renderer;
			Padding padding = magnifier.Outer.GetPadding(magnifier.Bitmap.Size);
			Padding padding2 = magnifier.Inner.GetPadding(magnifier.Bitmap.Size);
			Rectangle rectangle = magnifierOverlay.ClientRectangle.Pad(padding);
			Point location = magnifierOverlay.Location;
			location.Offset(rectangle.Location);
			location.Offset(rectangle.Width / 2, rectangle.Height / 2);
			location = location.Clip(base.ClientRectangle);
			float zoom = MagnifierZoom;
			renderer.Opacity = MagnifierOpacity;
			RectangleF clip = renderer.Clip;
			renderer.Clip = rectangle;
			DrawMagnifier(renderer, location, rectangle, zoom);
			renderer.Clip = clip;
			renderer.Opacity = 1f;
			ScalableBitmap.Draw(renderer, magnifier.Bitmap, magnifierOverlay.ClientRectangle, padding2, 1f);
		}

		private void visiblePartOverlay_Drawing(object sender, EventArgs e)
		{
			Size size = base.ImageSize.ToRectangle(partInfoSize, RectangleScaleMode.None).Size;
			size.Width += 16;
			size.Height += 16;
			if (!(visiblePartOverlay.Size == size))
			{
				visiblePartOverlay.Size = size;
				using (PanelSurface panelSurface = visiblePartOverlay.CreateSurface())
				{
					panelSurface.Graphics.Clear(Color.Transparent);
					partRect = PanelRenderer.DrawGraphics(panelSurface.Graphics, new Rectangle(Point.Empty, size), 1f);
				}
			}
		}

		private void visiblePartOverlay_RenderSurface(object sender, PanelRenderEventArgs e)
		{
			IBitmapRenderer renderer = e.Renderer;
			Size targetSize = partRect.Size.ToSize();
			Size imageSize = GetImageSize();
			if (imageSize.IsEmpty)
			{
				return;
			}
			Rectangle r = imageSize.ToRectangle(targetSize);
			r.Offset((int)partRect.Left, (int)partRect.Top);
			if (DoublePageOverlap == 0f && (currentPartHash != displayHash || smallBitmap == null))
			{
				Image image = smallBitmap;
				try
				{
					smallBitmap = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
					Bitmap bitmap = (((base.ImageDisplayOptions & ImageDisplayOptions.HighQuality) != 0) ? new Bitmap(smallBitmap.Width * 2, smallBitmap.Height * 2) : smallBitmap);
					using (Graphics graphics = Graphics.FromImage(bitmap))
					{
						graphics.ScaleTransform((float)bitmap.Width / (float)imageSize.Width, (float)bitmap.Height / (float)imageSize.Height);
						DrawImage(new BitmapGdiRenderer(graphics)
						{
							LowQualityInterpolation = InterpolationMode.Low
						}, imageSize.ToRectangle(), imageSize.ToRectangle());
					}
					if (bitmap != smallBitmap)
					{
						using (Graphics graphics2 = Graphics.FromImage(smallBitmap))
						{
							graphics2.InterpolationMode = InterpolationMode.HighQualityBicubic;
							graphics2.DrawImage(bitmap, smallBitmap.Size.ToRectangle());
						}
						bitmap.Dispose();
					}
				}
				catch (Exception)
				{
				}
				image?.Dispose();
				currentPartHash = displayHash;
			}
			Rectangle pagePartBounds = base.PagePartBounds;
			Rectangle rectangle = pagePartBounds.Scale(imageSize.GetScale(targetSize));
			Rectangle r2 = rectangle;
			r2.Offset(r.Location);
			renderer.Opacity = visiblePartOverlay.Opacity;
			if (smallBitmap != null)
			{
				try
				{
					renderer.DrawImage(smallBitmap, r, new Rectangle(0, 0, smallBitmap.Width, smallBitmap.Height), BitmapAdjustment.Empty, 0.3f);
					renderer.DrawImage(smallBitmap, r2, rectangle, BitmapAdjustment.Empty, 1f);
				}
				catch (Exception)
				{
				}
			}
			renderer.Opacity = 1f;
		}

		private void MemoryPageCache_ItemAdded(object sender, CacheItemEventArgs<ImageKey, PageImage> e)
		{
			if (IsValid && (object.Equals(e.Key, GetPageKey(CurrentPage)) || (TwoPageDisplay && object.Equals(e.Key, GetPageKey(NextPage))) || !DisplayedPages.Where((int dp) => object.Equals(e.Key, GetPageKey(dp))).IsEmpty()))
			{
				Invalidate();
			}
		}

		private void book_Disposing(object sender, EventArgs e)
		{
			Book = null;
		}

		private void book_Navigation(object sender, BookPageEventArgs e)
		{
			bool flag = e.OldPage < e.Page;
			if (IsFlipped)
			{
				flag = !flag;
			}
			switch (PageTransitionEffect)
			{
			default:
				Blender = null;
				break;
			case PageTransitionEffect.Fade:
				Blender = FadeInBlending;
				break;
			case PageTransitionEffect.LeftRight:
				if (flag)
				{
					Blender = ScrollToLeftBlending;
				}
				else
				{
					Blender = ScrollToRightBlending;
				}
				break;
			case PageTransitionEffect.TopDown:
				if (flag)
				{
					Blender = ScrollToTopBlending;
				}
				else
				{
					Blender = ScrollToBottomBlending;
				}
				break;
			case PageTransitionEffect.Paging:
				if (flag)
				{
					Blender = PageForward;
				}
				else
				{
					Blender = PageBackward;
				}
				break;
			}
			ShouldPagingBlend = !base.InvokeRequired && (BlendWhilePaging || Machine.Ticks - lastBlend > 100);
			OnPageChange(e);
			int oldPage = CurrentPage;
			DisplayOutputConfig displayConfig = base.DisplayConfig;
			currentPage = Book.CurrentPage;
			int part = 0;
			int num = ((!TwoPageDisplay) ? 1 : 2);
			if (e.OldPage != -1 && Math.Abs(e.OldPage - e.Page) <= num)
			{
				part = ((e.OldPage >= e.Page) ? (ImagePartCount - 1) : 0);
			}
			base.ImageVisiblePart = new ImagePartInfo(part);
			if (ShouldPagingBlend)
			{
				BlendAnimation(oldPage, displayConfig);
			}
			else
			{
				Invalidate();
			}
			OnPageChanged(e);
			UpdateNavigationOverlay(redraw: false);
			Update();
			lastBlend = Machine.Ticks;
		}

		private void book_PageFilterOrPagesChanged(object sender, EventArgs e)
		{
			Invalidate();
			UpdateNavigationOverlay(redraw: true);
		}

		private void Comic_BookChanged(object sender, BookChangedEventArgs e)
		{
			navigationOverlay.Caption = book.Comic.Caption;
		}

		private void book_IndexOfPageReady(object sender, BookPageEventArgs e)
		{
			try
			{
				UpdateNavigationOverlay(redraw: true);
				if (e.Page == CurrentPage)
				{
					UpdateCurrentPageOverlay();
					Invalidate();
				}
			}
			catch (Exception)
			{
			}
		}

		private void book_IndexRetrievalCompleted(object sender, EventArgs e)
		{
			UpdateNavigationOverlay(redraw: true);
			Invalidate();
		}

		private void book_ColorAdjustmentChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void book_RightToLeftReadingChanged(object sender, EventArgs e)
		{
			if (Book.RightToLeftReading != YesNo.Unknown)
			{
				base.RightToLeftReading = Book.RightToLeftReading == YesNo.Yes;
			}
		}

		private void longClickTimer_Tick(object sender, EventArgs e)
		{
			if (AutoMagnifier)
			{
				magnifierOverlay.Tag = true;
				MagnifierVisible = true;
				base.MouseActionHappened = true;
				base.DisableScrolling = true;
			}
			longClickTimer.Stop();
		}

		private void UpdateNavigationOverlay()
		{
			UpdateNavigationOverlay(PointToClient(Cursor.Position));
		}

		private void UpdateNavigationOverlay(Point pt)
		{
			if (!IsValid)
			{
				navigationOverlay.Visible = false;
				return;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle bounds = navigationOverlay.Bounds;
			Size size = new Size(500, 50);
			if (IsPageBrowsersOnTop)
			{
				UpdateNavigationOverlay(pt, navigationOverlay, new Point((clientRectangle.Width - bounds.Width) / 2, -bounds.Height), new Point((clientRectangle.Width - bounds.Width) / 2, NavigationOverlayVisibleY), new Rectangle((clientRectangle.Width - size.Width) / 2, 0, size.Width, size.Height));
			}
			else
			{
				UpdateNavigationOverlay(pt, navigationOverlay, new Point((clientRectangle.Width - bounds.Width) / 2, clientRectangle.Height), new Point((clientRectangle.Width - bounds.Width) / 2, NavigationOverlayVisibleY), new Rectangle((clientRectangle.Width - size.Width) / 2, clientRectangle.Height - size.Height, size.Width, size.Height));
			}
		}

		private void UpdateNavigationOverlay(Point pt, OverlayPanel panel, Point start, Point end, Rectangle hotBounds)
		{
			bool flag = IsImageValid() && (NavigationOverlayVisible || (IsNavigationOverlayEnabled && (panel.HasMouse || (Control.MouseButtons == MouseButtons.None && !magnifierOverlay.Visible && hotBounds.Contains(pt)))));
			if (!flag.Equals(panel.Tag) && (flag || panel.Visible))
			{
				overlayManager.AnimationEnabled = true;
				panel.Animators.Clear();
				if (!flag)
				{
					CloneUtility.Swap(ref start, ref end);
				}
				if (!panel.Visible)
				{
					panel.Location = start;
					panel.Visible = true;
				}
				panel.Animators.Add(new MoveAnimator(flag ? 300 : 200, panel.Location, end, !flag));
				panel.Tag = flag;
				panel.Animators.Start();
			}
		}

		public void BlendAnimation(int oldPage, DisplayOutputConfig oldConfig, BlendAnimationHandler blender, BlendAnimationMode mode = BlendAnimationMode.Default)
		{
			if (renderer != null && renderer.IsHardware && !disableBlending && blender != null && base.Visible)
			{
				base.DisplayEventsDisabled = true;
				try
				{
					if (mode != 0)
					{
						oldPage = currentPage;
					}
					int num = 50;
					while (!IsPageInCache(oldPage) || !IsPageInCache(oldPage, 1) || !IsPageInCache(currentPage) || !IsPageInCache(currentPage, 1))
					{
						if (--num < 0)
						{
							return;
						}
						Thread.Sleep(50);
					}
					DisplayOutputConfig displayConfig = base.DisplayConfig;
					displayConfig.Rotation = base.LastRenderedDisplay.Config.Rotation;
					DisplayOutput display = DisplayOutput.Create(displayConfig, base.CurrentAnamorphicTolerance);
					DisplayOutput oldOut = (oldConfig.IsEmpty ? display : DisplayOutput.Create(oldConfig, base.CurrentAnamorphicTolerance));
					switch (mode)
					{
					case BlendAnimationMode.CurrentAsNew:
						oldOut = null;
						break;
					case BlendAnimationMode.CurrentAsOld:
						oldOut = display;
						display = null;
						break;
					}
					inBlendAnmation = true;
					renderer.BeginScene(null);
					try
					{
						using (renderer.SaveState())
						{
							blender(renderer, oldPage, oldOut, display, 0f);
						}
						RenderImageOverlay(renderer, display ?? oldOut);
					}
					finally
					{
						renderer.EndScene();
					}
					ThreadUtility.Animate(EngineConfiguration.Default.BlendDuration, delegate(float x)
					{
						IBitmapRenderer bitmapRenderer = renderer;
						try
						{
							bitmapRenderer.BeginScene(null);
							using (bitmapRenderer.SaveState())
							{
								blender(bitmapRenderer, oldPage, oldOut, display, x);
							}
							RenderImageOverlay(bitmapRenderer, display ?? oldOut);
						}
						catch (Exception e2)
						{
							if (HandleRendererError(e2))
							{
								bitmapRenderer = null;
							}
						}
						finally
						{
							try
							{
								bitmapRenderer.EndScene();
							}
							catch
							{
							}
						}
					});
				}
				catch (Exception e)
				{
					HandleRendererError(e);
				}
				finally
				{
					base.DisplayEventsDisabled = false;
					inBlendAnmation = false;
				}
			}
			Invalidate();
		}

		public void BlendAnimation(int oldPage, DisplayOutputConfig oldConfig)
		{
			BlendAnimation(oldPage, oldConfig, Blender);
		}

		private void RenderImageBackground(IBitmapRenderer bitmapRenderer, DisplayOutput output, int page = -1)
		{
			int num = currentPage;
			if (page != -1)
			{
				currentPage = page;
			}
			try
			{
				base.RenderImageBackground(bitmapRenderer, output);
			}
			finally
			{
				currentPage = num;
			}
		}

		public void FadeInBlending(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent)
		{
			float opacity = hr.Opacity;
			float num = percent.Clamp(0.05f, 1f);
			RectangleF clip = hr.Clip;
			if (currentPage < oldPage)
			{
				RenderImageBackground(hr, display);
				if (!base.IsConstantBackground)
				{
					hr.Opacity = 1f - num;
					RenderImageBackground(hr, oldOut, oldPage);
				}
				hr.Opacity = num;
				RenderImageSafe(hr, display, withBackground: false);
				hr.Opacity = 1f;
				hr.Clip = oldOut.OutputBoundsScreen;
				RenderImageSafe(hr, display, withBackground: false);
				hr.Clip = clip;
				hr.Opacity = 1f - num;
				RenderImageSafe(hr, oldOut, oldPage, withBackground: false);
			}
			else
			{
				RenderImageBackground(hr, oldOut, oldPage);
				if (!base.IsConstantBackground)
				{
					hr.Opacity = num;
					RenderImageBackground(hr, display);
				}
				hr.Opacity = 1f - num;
				RenderImageSafe(hr, oldOut, oldPage, withBackground: false);
				hr.Opacity = 1f;
				hr.Clip = display.OutputBoundsScreen;
				RenderImageSafe(hr, oldOut, oldPage, withBackground: false);
				hr.Clip = clip;
				hr.Opacity = num;
				RenderImageSafe(hr, display, withBackground: false);
			}
			hr.Opacity = opacity;
		}

		public void PageForward(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent)
		{
			bool flag = true;
			bool flag2 = oldOut.OutputBoundsScreen.Width >= oldOut.OutputBoundsScreen.Height;
			bool flag3 = oldOut.Config.Rotation != 0 || display.Config.Rotation != ImageRotation.None;
			bool flag4 = !flag2 || flag3;
			Rectangle rectangle;
			if (!base.IsConstantBackground || flag3)
			{
				rectangle = base.ClientRectangle;
			}
			else
			{
				rectangle = Rectangle.Union(display.OutputBoundsScreen, oldOut.OutputBoundsScreen).Pad(-10);
				RenderImageBackground(hr, null);
				flag = false;
				hr.Clip = rectangle;
			}
			Rectangle rectangle2 = rectangle;
			float num = (float)rectangle2.Width * percent;
			RenderImageSafe(hr, oldOut, oldPage, flag ? RenderType.Default : RenderType.WithoutBackground);
			if (flag4)
			{
				num *= 2f;
				drawBlankPagesOverride = true;
			}
			innerBowLeftOffsetInPercent = 1f - percent;
			hr.TranslateTransform((float)rectangle2.Width - num, 0f);
			RenderImageSafe(hr, display, flag);
			hr.TranslateTransform(0f - ((float)rectangle2.Width - num), 0f);
			innerBowLeftOffsetInPercent = 0f;
			drawBlankPagesOverride = false;
			if (num > 5f)
			{
				hr.Clip = new RectangleF((float)rectangle2.Right - num / 2f, rectangle2.Top, num / 2f + 1f, rectangle2.Height + 1);
				RenderImageSafe(hr, display);
			}
			hr.Clip = Rectangle.Empty;
		}

		public void PageBackward(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent)
		{
			if (EngineConfiguration.Default.MirroredPageTurnAnimation)
			{
				int oldPage2 = currentPage;
				currentPage = oldPage;
				PageForward(hr, oldPage2, display, oldOut, 1f - percent);
				currentPage = oldPage2;
				return;
			}
			bool withBackground = true;
			bool flag = oldOut.OutputBoundsScreen.Width >= oldOut.OutputBoundsScreen.Height;
			bool flag2 = oldOut.Config.Rotation != 0 || display.Config.Rotation != ImageRotation.None;
			bool flag3 = !flag || flag2;
			Rectangle rectangle;
			if (!base.IsConstantBackground || flag2)
			{
				rectangle = base.ClientRectangle;
			}
			else
			{
				rectangle = Rectangle.Union(display.OutputBoundsScreen, oldOut.OutputBoundsScreen).Pad(-10);
				RenderImageBackground(hr, null);
				hr.Clip = rectangle;
				withBackground = false;
			}
			Rectangle rectangle2 = rectangle;
			float num = (float)rectangle2.Width * percent;
			RenderImageSafe(hr, oldOut, oldPage, withBackground);
			if (flag3)
			{
				num *= 2f;
				drawBlankPagesOverride = true;
			}
			innerBowRightOffsetInPercent = 1f - percent;
			hr.TranslateTransform(0f - ((float)rectangle2.Width - num), 0f);
			RenderImageSafe(hr, display, withBackground);
			hr.TranslateTransform((float)rectangle2.Width - num, 0f);
			drawBlankPagesOverride = false;
			innerBowRightOffsetInPercent = 0f;
			if (num > 5f)
			{
				hr.Clip = new RectangleF(rectangle2.Left, rectangle2.Top, num / 2f + 1f, rectangle2.Height + 1);
				RenderImageSafe(hr, display);
			}
			hr.Clip = Rectangle.Empty;
		}

		public void ScrollToLeftBlending(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent)
		{
			RenderImageBackground(hr, null);
			hr.TranslateTransform((float)(-base.ClientRectangle.Width) * percent, 0f);
			RenderImageSafe(hr, oldOut, oldPage, !base.IsConstantBackground);
			hr.TranslateTransform(base.ClientRectangle.Width, 0f);
			RenderImageSafe(renderer, display, !base.IsConstantBackground);
		}

		public void ScrollToRightBlending(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent)
		{
			RenderImageBackground(hr, null);
			hr.TranslateTransform((float)base.ClientRectangle.Width * percent, 0f);
			RenderImageSafe(hr, oldOut, oldPage, !base.IsConstantBackground);
			hr.TranslateTransform(-base.ClientRectangle.Width, 0f);
			RenderImageSafe(renderer, display, !base.IsConstantBackground);
		}

		public void ScrollToBottomBlending(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent)
		{
			RenderImageBackground(hr, null);
			hr.TranslateTransform(0f, (float)base.ClientRectangle.Height * percent);
			RenderImageSafe(hr, oldOut, oldPage, !base.IsConstantBackground);
			hr.TranslateTransform(0f, -base.ClientRectangle.Height);
			RenderImageSafe(renderer, display, !base.IsConstantBackground);
		}

		public void ScrollToTopBlending(IBitmapRenderer hr, int oldPage, DisplayOutput oldOut, DisplayOutput display, float percent)
		{
			RenderImageBackground(hr, null);
			hr.TranslateTransform(0f, (float)(-base.ClientRectangle.Height) * percent);
			RenderImageSafe(hr, oldOut, oldPage, !base.IsConstantBackground);
			hr.TranslateTransform(0f, base.ClientRectangle.Height);
			RenderImageSafe(renderer, display, !base.IsConstantBackground);
		}

		private void RenderImageSafe(IBitmapRenderer bitmapRenderer, DisplayOutput output, int page, RenderType renderType = RenderType.Default)
		{
			if (output != null)
			{
				int num = currentPage;
				currentPage = page;
				try
				{
					RenderImageSafe(bitmapRenderer, output, renderType);
				}
				finally
				{
					currentPage = num;
				}
			}
		}

		private void RenderImageSafe(IBitmapRenderer bitmapRenderer, DisplayOutput output, int page, bool withBackground)
		{
			RenderImageSafe(bitmapRenderer, output, page, withBackground ? RenderType.Default : RenderType.WithoutBackground);
		}

		private void RenderImageSafe(IBitmapRenderer bitmapRenderer, DisplayOutput output, bool withBackground)
		{
			RenderImageSafe(bitmapRenderer, output, withBackground ? RenderType.Default : RenderType.WithoutBackground);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			imageScaleTimer = new System.Windows.Forms.Timer(components);
			longClickTimer = new System.Windows.Forms.Timer(components);
			cacheUpdateTimer = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			imageScaleTimer.Interval = 500;
			imageScaleTimer.Tick += new System.EventHandler(imageScaleTimer_Tick);
			longClickTimer.Interval = 500;
			longClickTimer.Tick += new System.EventHandler(longClickTimer_Tick);
			cacheUpdateTimer.Interval = 2000;
			cacheUpdateTimer.Tick += new System.EventHandler(cacheUpdateTimer_Tick);
			ResumeLayout(false);
		}

		static ComicDisplayControl()
		{
			Magnifier[] array = new Magnifier[2];
			Magnifier magnifier = new Magnifier
			{
				Bitmap = Resources.Magnifier,
				Inner = new Rectangle(20, 20, 573, 327),
				Outer = new Rectangle(5, 5, 592, 350)
			};
			array[0] = magnifier;
			magnifier = new Magnifier
			{
				Bitmap = Resources.MagnifierLight,
				Inner = new Rectangle(6, 5, 102, 56),
				Outer = new Rectangle(3, 2, 108, 61)
			};
			array[1] = magnifier;
			magnifiers = array;
		}
	}
}
