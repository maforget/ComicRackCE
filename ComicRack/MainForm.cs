using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Net;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Engine.Display.Forms;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Network;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Plugins;
using cYo.Projects.ComicRack.Plugins.Automation;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Menus;
using cYo.Projects.ComicRack.Viewer.Properties;
using cYo.Projects.ComicRack.Viewer.Views;
using Microsoft.Win32;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace cYo.Projects.ComicRack.Viewer
{
	[ComVisible(true)]
	public partial class MainForm : Form, IMain, IContainerControl, IPluginConfig, IApplication, IBrowser
	{
		public partial class ComicReaderTab : TabBar.TabBarItem
		{
			private readonly ComicBookNavigator nav;

			private readonly Font font;

			private readonly string shortcut;

			public ComicReaderTab(string text, ComicBookNavigator nav, Font font, string shortcut)
				: base(text)
			{
				this.nav = nav;
				this.font = font;
				this.shortcut = shortcut;
				ToolTipSize = new Size(400, 200).ScaleDpi();
			}

			public override bool ShowToolTip()
			{
				return nav != null;
			}

			public override void DrawTooltip(Graphics gr, Rectangle rc)
			{
				base.DrawTooltip(gr, rc);
				if (nav == null)
				{
					return;
				}
				try
				{
					ComicBook comic = nav.Comic;
					using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(comic.GetFrontCoverThumbnailKey(), nav, onErrorThrowException: false))
					{
						rc.Inflate(-10, -5);
						if (!string.IsNullOrEmpty(shortcut))
						{
							using (Brush brush = new SolidBrush(Color.FromArgb(128, SystemColors.InfoText)))
							{
								using (StringFormat format = new StringFormat
								{
									Alignment = StringAlignment.Far,
									LineAlignment = StringAlignment.Far
								})
								{
									gr.DrawString(shortcut, FC.Get(font, 6f), brush, rc, format);
								}
							}
							rc.Height -= 10;
						}
						ThumbTileRenderer.DrawTile(gr, rc, itemLock.Item.GetThumbnail(rc.Height), comic, font, SystemColors.InfoText, Color.Transparent, ThumbnailDrawingOptions.DefaultWithoutBackground, ComicTextElements.DefaultComic, threeD: false, comic.GetIcons());
					}
				}
				catch
				{
				}
			}
		}

		public class RatingEditor : IEditRating
		{
			private IEnumerable<ComicBook> books;

			private IWin32Window parent;

			public RatingEditor(IWin32Window parent, IEnumerable<ComicBook> books)
			{
				this.parent = parent;
				this.books = books;
			}

			public bool IsValid()
			{
				if (books != null)
				{
					return !books.IsEmpty();
				}
				return false;
			}

			public void SetRating(float rating)
			{
				if (IsValid())
				{
					Program.Database.Undo.SetMarker(TR.Messages["UndoRating", "Change Rating"]);
					books.ForEach((ComicBook cb) =>
                    {
                        cb.Rating = rating;
                    });
				}
			}

			public float GetRating()
			{
				float num = -1f;
				if (IsValid())
				{
					foreach (ComicBook book in books)
					{
						if (num == -1f)
						{
							num = book.Rating;
						}
						else if (num != book.Rating)
						{
							return -1f;
						}
					}
					return num;
				}
				return num;
			}

			public bool QuickRatingAndReview()
			{
				Program.Database.Undo.SetMarker(TR.Messages["QuickRating", "Quick Rating"]);
				return QuickRatingDialog.Show(parent, books.FirstOrDefault());
			}
		}

		public class PageEditorWrapper : IEditPage
		{
			private IEditPage editor;

			public bool IsValid
			{
				get
				{
					if (editor != null)
					{
						return editor.IsValid;
					}
					return false;
				}
			}

			public ComicPageType PageType
			{
				get
				{
					if (!IsValid)
					{
						return ComicPageType.Story;
					}
					return editor.PageType;
				}
				set
				{
					if (IsValid)
					{
						editor.PageType = value;
					}
				}
			}

			public ImageRotation Rotation
			{
				get
				{
					if (!IsValid)
					{
						return ImageRotation.None;
					}
					return editor.Rotation;
				}
				set
				{
					if (IsValid)
					{
						editor.Rotation = value;
					}
				}
			}

			public PageEditorWrapper(IEditPage editor)
			{
				this.editor = editor;
			}
		}

		public class BookmarkEditorWrapper : IEditBookmark
		{
			private IEditBookmark editor;

			public bool CanBookmark
			{
				get
				{
					if (editor != null)
					{
						return editor.CanBookmark;
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
					return editor.BookmarkProposal;
				}
			}

			public string Bookmark
			{
				get
				{
					if (!CanBookmark)
					{
						return string.Empty;
					}
					return editor.Bookmark;
				}
				set
				{
					if (CanBookmark)
					{
						editor.Bookmark = value;
					}
				}
			}

			public BookmarkEditorWrapper(IEditBookmark editor)
			{
				this.editor = editor;
			}
		}

		private readonly CommandMapper commands = new CommandMapper();

		private readonly ToolStripThumbSize thumbSize = new ToolStripThumbSize();

		private string[] recentFiles = new string[0];

		private readonly VisibilityAnimator mainMenuStripVisibility;

		private readonly VisibilityAnimator fileTabsVisibility;

		private readonly VisibilityAnimator statusStripVisibility;

		private EnumMenuUtility pageTypeContextMenu;

		private EnumMenuUtility pageTypeEditMenu;

		private EnumMenuUtility pageRotationContextMenu;

		private EnumMenuUtility pageRotationEditMenu;

		private readonly KeyboardShortcuts mainKeys = new KeyboardShortcuts();

		private bool menuDown;

		private bool autoHideMainMenu;

		private bool showMainMenuNoComicOpen = true;

		private bool menuClose;

		private ComicBook[] lastRandomList = new ComicBook[0];

		private List<ComicBook> randomSelectedComics;

		private float lastZoom = 2f;

		private string lastWorkspaceName;

		private WorkspaceType lastWorkspaceType = WorkspaceType.Default;

		private ReaderForm readerForm;

		private DockStyle savedBrowserDockStyle;

		private bool savedBrowserVisible;

		private Rectangle undockedReaderBounds;

        private FormWindowState undockedReaderState;

        private bool shieldReaderFormClosing;

		private Image addTabImage = Resources.AddTab;

		private Image emptyTabImage = Resources.Original;

		private TasksDialog taskDialog;

		private static Image SinglePageRtl = Resources.SinglePageRtl;

		private static Image TwoPagesRtl = Resources.TwoPageForcedRtl;

		private static Image TwoPagesAdaptiveRtl = Resources.TwoPageRtl;

		private bool enableAutoHideMenu = true;

		private long menuAutoClosed;

		private static readonly string None = TR.Default["None", "None"];

		private static readonly string NotAvailable = TR.Default["NotAvailable", "NA"];

		private static readonly string ExportingComics = TR.Load(typeof(MainForm).Name)["ExportingComics", "Exporting Books: {0} queued"];

		private static readonly string ExportingErrors = TR.Load(typeof(MainForm).Name)["ExportingErrors", "{0} errors. Click for details"];

		private static readonly string DeviceSyncing = TR.Load(typeof(MainForm).Name)["DeviceSyncing", "Syncing Devices: {0} queued"];

		private static readonly string DeviceSyncingErrors = TR.Load(typeof(MainForm).Name)["DeviceSyncingErrors", "{0} errors. Click for details"];

		private static readonly Image exportErrorAnimation = Resources.ExportAnimationWithError;

		private static readonly Image exportAnimation = Resources.ExportAnimation;

		private static readonly Image exportError = Resources.ExportError;

		private static readonly Image deviceSyncErrorAnimation = Resources.DeviceSyncAnimationWithError;

		private static readonly Image deviceSyncAnimation = Resources.DeviceSyncAnimation;

		private static readonly Image deviceSyncError = Resources.DeviceSyncError;

		private static readonly Image zoomImage = Resources.Zoom;

		private static readonly Image zoomClearImage = Resources.ZoomClear;

		private static readonly Image updatePages = Resources.UpdatePages;

		private static readonly Image greenLight = Resources.GreenLight;

		private static readonly Image grayLight = Resources.GrayLight;

		private static readonly Image trackPagesLockedImage = Resources.Locked;

		private static readonly Image datasourceConnected = Resources.DataSourceConnected;

		private static readonly Image datasourceDisconnected = Resources.DataSourceDisconnected;

		private bool maximized;

		private bool shieldTray;

		private bool minimalGui;

		private bool quickUpdateRegistered;

		private IEnumerable<ShareableComicListItem> defaultQuickOpenLists;

		private bool quickListDirty;

		[DefaultValue(null)]
		public ComicDisplay ComicDisplay
		{
			get
			{
				if (comicDisplay == null)
				{
					comicDisplay = CreateComicDisplay();
				}
				return comicDisplay;
			}
		}

		[DefaultValue(false)]
		public bool AutoHideMainMenu
		{
			get
			{
				return autoHideMainMenu;
			}
			set
			{
				if (autoHideMainMenu != value)
				{
					autoHideMainMenu = value;
					OnGuiVisibilities();
				}
			}
		}

		[DefaultValue(true)]
		public bool ShowMainMenuNoComicOpen
		{
			get
			{
				return showMainMenuNoComicOpen;
			}
			set
			{
				if (showMainMenuNoComicOpen != value)
				{
					showMainMenuNoComicOpen = value;
					OnGuiVisibilities();
				}
			}
		}

		[Browsable(false)]
		public bool IsInitialized
		{
			get;
			private set;
		}

		public bool ReaderUndocked
		{
			get
			{
				return readerForm != null;
			}
			set
			{
				if (value == ReaderUndocked)
				{
					return;
				}
				if (value)
				{
					savedBrowserDockStyle = BrowserDock;
					savedBrowserVisible = BrowserVisible;
					BrowserVisible = true;
					BrowserDock = DockStyle.Fill;
					panelReader.Controls.Remove(readerContainer);
					readerForm = new ReaderForm(ComicDisplay);
					readerForm.FormClosing += ReaderFormFormClosing;
					readerForm.KeyDown += ReaderFormKeyDown;
					readerForm.Controls.Add(readerContainer);
					readerForm.WindowState = undockedReaderState;
					if (undockedReaderBounds.IsEmpty)
					{
						readerForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
					}
					else
					{
						readerForm.StartPosition = FormStartPosition.Manual;
						readerForm.SafeBounds = undockedReaderBounds;
					}
					mainView.ShowLibrary();
					if (OpenBooks.OpenCount > 0)
					{
						readerForm.Show();
					}
				}
				else
				{
					undockedReaderState = readerForm.WindowState;
					undockedReaderBounds = readerForm.SafeBounds;
					readerForm.Controls.Remove(readerContainer);
					readerForm.Dispose();
					panelReader.Controls.Add(readerContainer);
					readerForm = null;
					BrowserDock = savedBrowserDockStyle;
					BrowserVisible = savedBrowserVisible;
				}
				OnGuiVisibilities();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle UndockedReaderBounds
		{
			get
			{
				if (!ReaderUndocked)
				{
					return undockedReaderBounds;
				}
				return readerForm.SafeBounds;
			}
			set
			{
				if (ReaderUndocked)
				{
					readerForm.SafeBounds = value;
				}
				else
				{
					undockedReaderBounds = value;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FormWindowState UndockedReaderState
		{
			get
			{
				if (!ReaderUndocked)
				{
					return undockedReaderState;
				}
				return readerForm.WindowState;
			}
			set
			{
				if (ReaderUndocked)
				{
					readerForm.WindowState = value;
				}
				else
				{
					undockedReaderState = value;
				}
			}
		}

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ScriptOutputBounds
        {
            get
            {
				return ScriptConsole?.SafeBounds ?? Rectangle.Empty;
            }
            set
            {
				if (ScriptConsole != null)
				{
					if (value.IsEmpty)
					{
						ScriptConsole.StartPosition = FormStartPosition.WindowsDefaultLocation;
					}
					else
					{
						ScriptConsole.SafeBounds = value;
					} 
				}
            }
        }

        private bool MainToolStripVisible
		{
			get
			{
				return fileTabs.Controls.Contains(mainToolStrip);
			}
			set
			{
				bool mainToolStripVisible = MainToolStripVisible;
				if (value != mainToolStripVisible)
				{
					if (mainToolStripVisible)
					{
						mainView.TabBar.Controls.Remove(mainToolStrip);
					}
					else
					{
						fileTabs.Controls.Remove(mainToolStrip);
					}
					if (value)
					{
						fileTabs.Controls.Add(mainToolStrip);
						return;
					}
					mainView.TabBar.Controls.Add(mainToolStrip);
					mainView.TabBar.Controls.SetChildIndex(mainToolStrip, 0);
				}
			}
		}

		public DockStyle ViewDock
		{
			get
			{
				return mainViewContainer.Dock;
			}
			set
			{
				mainViewContainer.Dock = value;
			}
		}

		public bool Maximized => maximized;

		public Rectangle SafeBounds
		{
			get;
			set;
		}

		public bool MinimizedToTray => notifyIcon.Visible;

		public Control Control => this;

		public bool IsComicVisible
		{
			get
			{
				if (!ReaderUndocked && BrowserDock == DockStyle.Fill)
				{
					return mainView.IsComicVisible;
				}
				return true;
			}
		}

		public bool BrowserVisible
		{
			get
			{
				if (!ReaderUndocked && BrowserDock != DockStyle.Fill)
				{
					return mainViewContainer.Expanded;
				}
				return savedBrowserVisible;
			}
			set
			{
				if (!ReaderUndocked && BrowserDock != DockStyle.Fill)
				{
					mainViewContainer.Expanded = value;
					return;
				}
				savedBrowserVisible = value;
				mainViewContainer.Expanded = true;
			}
		}

		public DockStyle BrowserDock
		{
			get
			{
				if (!ReaderUndocked)
				{
					return mainViewContainer.Dock;
				}
				return savedBrowserDockStyle;
			}
			set
			{
				if (ReaderUndocked)
				{
					savedBrowserDockStyle = value;
					mainViewContainer.Dock = DockStyle.Fill;
				}
				else
				{
					mainViewContainer.Dock = value;
				}
			}
		}

		public NavigatorManager OpenBooks => books;

		[Browsable(false)]
		[DefaultValue(false)]
		public bool MinimalGui
		{
			get
			{
				return minimalGui;
			}
			set
			{
				if (minimalGui != value)
				{
					minimalGui = value;
					OnGuiVisibilities();
				}
			}
		}

        public static ScriptOutputForm ScriptConsole
        {
            get => Program.ScriptConsole != null ? Program.ScriptConsole : null;
        }

        public IEnumerable<string> LibraryPaths => Program.Settings.ScriptingLibraries.Replace("\n", "").Replace("\r", "").Split(';', StringSplitOptions.RemoveEmptyEntries);

		public MainForm()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			base.Size = base.Size.ScaleDpi();
			statusStrip.Height = (int)tsText.Font.GetHeight() + FormUtility.ScaleDpiY(8);
			SystemEvents.DisplaySettingsChanging += delegate
			{
				StoreWorkspace();
			};
			SystemEvents.DisplaySettingsChanged += delegate
			{
				SetWorkspaceDisplayOptions(Program.Settings.CurrentWorkspace);
			};
			if (Program.ExtendedSettings.DisableFoldersView)
			{
				miViewFolders.GetCurrentParent().Items.Remove(miViewFolders);
			}
			notifyIcon.MouseDoubleClick += NotifyIconMouseDoubleClick;
			FormUtility.EnableRightClickSplitButtons(mainToolStrip.Items);
			AllowDrop = true;
			base.DragDrop += BookDragDrop;
			base.DragEnter += BookDragEnter;
			ComicDisplay.FirstPageReached += viewer_FirstPageReached;
			ComicDisplay.LastPageReached += viewer_LastPageReached;
			ComicDisplay.FullScreenChanged += ViewerFullScreenChanged;
			ComicDisplay.PageChanged += ComicDisplay_PageChanged;
			books = new NavigatorManager(ComicDisplay);
			books.BookOpened += OnBookOpened;
			books.BookClosed += OnBookClosed;
			books.BookClosing += OnBookClosing;
			books.Slots.Changed += OpenBooks_SlotsChanged;
			books.CurrentSlotChanged += OpenBooks_CurrentSlotChanged;
			books.OpenComicsChanged += OpenBooks_CaptionsChanged;
			components.Add(commands);
			tbZoom.Width = 60;
			fileTabs.Visible = false;
			DropDownHost<MagnifySetupControl> dropDownHost = new DropDownHost<MagnifySetupControl>();
			ComicDisplay.MagnifierOpacity = (dropDownHost.Control.MagnifyOpaque = Program.Settings.MagnifyOpaque);
			ComicDisplay.MagnifierSize = (dropDownHost.Control.MagnifySize = Program.Settings.MagnifySize);
			ComicDisplay.MagnifierZoom = (dropDownHost.Control.MagnifyZoom = Program.Settings.MagnifyZoom);
			ComicDisplay.MagnifierStyle = (dropDownHost.Control.MagnifyStyle = Program.Settings.MagnifyStyle);
			ComicDisplay.AutoMagnifier = (dropDownHost.Control.AutoMagnifier = Program.Settings.AutoMagnifier);
			ComicDisplay.AutoHideMagnifier = (dropDownHost.Control.AutoHideMagnifier = Program.Settings.AutoHideMagnifier);
			dropDownHost.Control.ValuesChanged += MagnifySetupChanged;
			tbMagnify.DropDown = dropDownHost;
			mainMenuStripVisibility = new VisibilityAnimator(components, mainMenuStrip);
			fileTabsVisibility = new VisibilityAnimator(components, fileTabs);
			statusStripVisibility = new VisibilityAnimator(components, statusStrip);
			LocalizeUtility.Localize(this, components);
			quickOpenView.Caption = TR.Load(base.Name)[quickOpenView.Name, quickOpenView.Caption];
			Program.StartupProgress(TR.Messages["InitScripts", "Initializing Scripts"], 70);
			if (ScriptUtility.Initialize(this, this, this, ComicDisplay, this, OpenBooks))
			{
				miFileAutomation.DropDownItems.AddRange(ScriptUtility.CreateToolItems<ToolStripMenuItem>(this, PluginEngine.ScriptTypeLibrary, () => Program.Database.Books).ToArray());
				miFileAutomation.Visible = miFileAutomation.DropDownItems.Count != 0;
				int num = fileMenu.DropDownItems.IndexOf(miNewComic);
				ToolStripMenuItem[] array = ScriptUtility.CreateToolItems<ToolStripMenuItem>(this, PluginEngine.ScriptTypeNewBooks, () => Program.Database.Books).ToArray();
				foreach (ToolStripMenuItem value in array)
				{
					fileMenu.DropDownItems.Insert(++num, value);
				}
				foreach (Command sc in ScriptUtility.Scripts.GetCommands(PluginEngine.ScriptTypeDrawThumbnailOverlay))
				{
					sc.PreCompile();
					CoverViewItem.DrawCustomThumbnailOverlay += (ComicBook comic, Graphics graphics, Rectangle bounds, int flags) =>
                    {
                        sc.Invoke(new object[4]
                        {
                            comic,
                            graphics,
                            bounds,
                            flags
                        }, catchErrors: true);
                    };
				}
			}
			Program.StartupProgress(TR.Messages["InitGUI", "Initializing User Interface"], 80);
		}

		private void UpdateSettings()
		{
			ComicDisplay.MouseWheelSpeed = Program.Settings.MouseWheelSpeed;
			ComicDisplay.ImageDisplayOptions = Program.Settings.PageImageDisplayOptions;
			ComicDisplay.SmoothScrolling = Program.Settings.SmoothScrolling;
			ComicDisplay.BlendWhilePaging = Program.Settings.BlendWhilePaging;
			ComicDisplay.InfoOverlayScaling = (float)Program.Settings.OverlayScaling / 100f;
			ComicDisplay.SetInfoOverlays(InfoOverlays.PartInfo, Program.Settings.ShowVisiblePagePartOverlay);
			ComicDisplay.SetInfoOverlays(InfoOverlays.CurrentPage, Program.Settings.ShowCurrentPageOverlay);
			ComicDisplay.SetInfoOverlays(InfoOverlays.LoadPage, Program.Settings.ShowStatusOverlay);
			ComicDisplay.SetInfoOverlays(InfoOverlays.PageBrowser, Program.Settings.ShowNavigationOverlay);
			ComicDisplay.SetInfoOverlays(InfoOverlays.PageBrowserOnTop, Program.Settings.NavigationOverlayOnTop);
			ComicDisplay.SetInfoOverlays(InfoOverlays.CurrentPageShowsName, Program.Settings.CurrentPageShowsName);
			ComicDisplay.HideCursorFullScreen = Program.Settings.HideCursorFullScreen;
			ComicDisplay.AutoScrolling = Program.Settings.AutoScrolling;
			ComicDisplay.PageWallTicks = (Program.Settings.PageChangeDelay ? 300 : 0);
			ComicDisplay.ScrollingDoesBrowse = Program.Settings.ScrollingDoesBrowse;
			ComicDisplay.ResetZoomOnPageChange = Program.Settings.ResetZoomOnPageChange;
			ComicDisplay.ZoomInOutOnPageChange = Program.Settings.ZoomInOutOnPageChange;
			ComicDisplay.RightToLeftReadingMode = Program.Settings.RightToLeftReadingMode;
			ComicDisplay.LeftRightMovementReversed = Program.Settings.LeftRightMovementReversed;
			ComicDisplay.DisplayChangeAnimation = Program.Settings.DisplayChangeAnimation;
			ComicDisplay.FlowingMouseScrolling = Program.Settings.FlowingMouseScrolling;
			ComicDisplay.SoftwareFiltering = Program.Settings.SoftwareFiltering;
			ComicDisplay.HardwareFiltering = Program.Settings.HardwareFiltering;
			ComicDisplay.SetRenderer(Program.Settings.HardwareAcceleration);
			foreach (ComicBookNavigator slot in OpenBooks.Slots)
			{
				if (slot != null)
				{
					slot.BaseColorAdjustment = Program.Settings.GlobalColorAdjustment;
				}
			}
			AutoHideMainMenu = Program.Settings.AutoHideMainMenu;
			ShowMainMenuNoComicOpen = Program.Settings.ShowMainMenuNoComicOpen;
			quickOpenView.ThumbnailSize = Program.Settings.QuickOpenThumbnailSize;
			ComicBookNavigator.TrackCurrentPage = Program.Settings.TrackCurrentPage;
			tsCurrentPage.Image = (ComicBookNavigator.TrackCurrentPage ? null : Resources.Locked);
			CoverViewItem.ThumbnailSizing = (Program.Settings.CoverThumbnailsSameSize ? CoverThumbnailSizing.Fit : CoverThumbnailSizing.None);
			ComicBook.NewBooksChecked = Program.Settings.NewBooksChecked;
			DeviceSyncFactory.SetExtraWifiDeviceAddresses(EngineConfiguration.Default.ExtraWifiDeviceAddresses + "," + Program.Settings.ExtraWifiDeviceAddresses);
		}

		private void SettingsChanged(object sender, EventArgs e)
		{
			UpdateSettings();
		}

		private void OnBookOpened(object sender, BookEventArgs e)
		{
			if (Program.Settings.TrackCurrentPage)
			{
				e.Book.OpenedTime = DateTime.Now;
			}
			e.Book.NewPages = 0;
			recentFiles = Program.Database.GetRecentFiles(Settings.RecentFileCount).ToArray();
			if (e.Book.EditMode.IsLocalComic())
			{
				Win7.UpdateRecent(e.Book.FilePath);
			}
			UpdateBrowserVisibility();
			ScriptUtility.Invoke(PluginEngine.ScriptTypeBookOpened, e.Book);
			string url = e.Book.FilePath;
			Win7.AddTabbedThumbnail(this, e.Book.FilePath, delegate
			{
				books.CurrentSlot = books.Slots.FindIndex((ComicBookNavigator s) => s.Comic.FilePath == url);
			}, delegate
			{
				books.Close(books.Slots.FindIndex((ComicBookNavigator s) => s.Comic.FilePath == url));
			}, delegate
			{
				ComicBookNavigator comicBookNavigator = books.Slots.FirstOrDefault((ComicBookNavigator s) => s.Comic.FilePath == url);
				return (comicBookNavigator == books.CurrentBook) ? ComicDisplay.CreateThumbnail() : comicBookNavigator.Thumbnail;
			});
		}

		private void OnBookClosing(object sender, BookEventArgs e)
		{
			if (Program.Settings.AutoShowQuickReview && e.Book != null && e.Book.HasBeenRead && e.Book.Rating == 0f)
			{
				new RatingEditor(Form.ActiveForm ?? this, ListExtensions.AsEnumerable<ComicBook>(e.Book)).QuickRatingAndReview();
			}
		}

		private void OnBookClosed(object sender, BookEventArgs e)
		{
			Program.ImagePool.SlowPageQueue.RemoveItems((PageKey k) => k.Location == e.Book.FilePath);
			Program.ImagePool.FastPageQueue.RemoveItems((PageKey k) => k.Location == e.Book.FilePath);
			Program.QueueManager.AddBookToFileUpdate(e.Book);
			Win7.RemoveThumbnail(e.Book.FilePath);
		}

		private void ComicDisplay_PageChanged(object sender, BookPageEventArgs e)
		{
			Win7.InvalidateThumbnail(OpenBooks.CurrentBook.Comic.FilePath);
		}

		private void UpdateBrowserVisibility()
		{
			if (!ReaderUndocked)
			{
				if (BrowserDock == DockStyle.Fill)
				{
					mainView.ShowView(books.CurrentSlot);
				}
				else if (Program.Settings.CloseBrowserOnOpen)
				{
					BrowserVisible = false;
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 28 && m.WParam.ToInt32() == 0)
			{
				ComicDisplay.FullScreen = false;
			}
			base.WndProc(ref m);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			Win7.Initialize();
			if (!string.IsNullOrEmpty(Program.ExtendedSettings.InstallPlugin))
			{
				ShowPreferences(Program.ExtendedSettings.InstallPlugin);
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Program.Database.BookChanged += WatchedBookHasChanged;
			Program.BookFactory.TemporaryBookChanged += WatchedBookHasChanged;
			Program.Database.Books.ForEach(Program.QueueManager.AddBookToFileUpdate);
			if (Program.Settings.ScanStartup)
			{
				Program.Scanner.ScanFilesOrFolders(Program.Database.WatchFolders.Folders, all: true, Program.Settings.RemoveMissingFilesOnFullScan);
			}
			if (Program.Settings.UpdateWebComicsStartup)
			{
				UpdateWebComics();
			}
			SuspendLayout();
			base.Icon = Resources.ComicRackAppSmall;
			notifyIcon.Icon = base.Icon;
			notifyIcon.Text = LocalizeUtility.GetText(this, "NotifyIconText", notifyIcon.Text);
			miFileAutomation.Visible = miFileAutomation.DropDownItems.Count != 0;
			mainMenuStrip.SendToBack();
			statusStrip.Items.Insert(statusStrip.Items.Count - 1, thumbSize);
			thumbSize.TrackBar.Scroll += TrackBar_Scroll;
			ThumbRenderer.DefaultRatingImage1 = Resources.StarYellow.ToOptimized();
			ThumbRenderer.DefaultRatingImage2 = Resources.StarBlue.ToOptimized();
			ThumbRenderer.DefaultTagRatingImage1 = Resources.RatingYellow.ToOptimized();
			ThumbRenderer.DefaultTagRatingImage2 = Resources.RatingBlue.ToOptimized();
			ComicDisplay.PagePool = Program.ImagePool;
			ComicDisplay.ThumbnailPool = Program.ImagePool;
			ComicDisplay.PageFilter = Program.Settings.PageFilter;
			mainView.Main = this;
			miOpenRecent.DropDownItems.Add(new ToolStripMenuItem("dummy"));
			IdleProcess.Idle += Application_Idle;
			Program.Settings.SettingsChanged += SettingsChanged;
			recentFiles = Program.Database.GetRecentFiles(Settings.RecentFileCount).ToArray();
			InitializeCommands();
			InitializeKeyboard();
			InitializeHelp(Program.Settings.HelpSystem);
			Program.Settings.HelpSystemChanged += delegate
			{
				InitializeHelp(Program.Settings.HelpSystem);
			};
			InitializePluginHelp();
			UpdateSettings();
			UpdateWorkspaceMenus();
			SetWorkspace(Program.Settings.GetWorkspace(Program.ExtendedSettings.Workspace) ?? Program.Settings.CurrentWorkspace, remember: false);
			UpdateListConfigMenus();
			pageTypeContextMenu = new EnumMenuUtility(cmPageType, typeof(ComicPageType), flagsMode: false, null, Keys.A | Keys.Shift | Keys.Alt);
			pageTypeEditMenu = new EnumMenuUtility(miPageType, typeof(ComicPageType), flagsMode: false, null, Keys.A | Keys.Shift | Keys.Alt);
			pageTypeContextMenu.ValueChanged += delegate
			{
				GetPageEditor().PageType = (ComicPageType)pageTypeContextMenu.Value;
			};
			pageTypeEditMenu.ValueChanged += delegate
			{
				GetPageEditor().PageType = (ComicPageType)pageTypeEditMenu.Value;
			};
			Dictionary<int, Image> images = new Dictionary<int, Image>
			{
				{
					0,
					Resources.Rotate0Permanent
				},
				{
					1,
					Resources.Rotate90Permanent
				},
				{
					2,
					Resources.Rotate180Permanent
				},
				{
					3,
					Resources.Rotate270Permanent
				}
			};
			pageRotationContextMenu = new EnumMenuUtility(cmPageRotate, typeof(ImageRotation), flagsMode: false, images, Keys.D6 | Keys.Shift | Keys.Alt);
			pageRotationEditMenu = new EnumMenuUtility(miPageRotate, typeof(ImageRotation), flagsMode: false, images, Keys.D6 | Keys.Shift | Keys.Alt);
			pageRotationContextMenu.ValueChanged += delegate
			{
				GetPageEditor().Rotation = (ImageRotation)pageRotationContextMenu.Value;
			};
			pageRotationEditMenu.ValueChanged += delegate
			{
				GetPageEditor().Rotation = (ImageRotation)pageRotationEditMenu.Value;
			};
			ResumeLayout(performLayout: true);
			contextRating.Items.Insert(contextRating.Items.Count - 2, new ToolStripSeparator());
			RatingControl.InsertRatingControl(contextRating, contextRating.Items.Count - 2, Resources.StarYellow, GetRatingEditor);
			contextRating2.Items.Insert(contextRating2.Items.Count - 2, new ToolStripSeparator());
			RatingControl.InsertRatingControl(contextRating2, contextRating2.Items.Count - 2, Resources.StarYellow, GetRatingEditor);
			contextRating.Renderer = new MenuRenderer(Resources.StarYellow);
			contextRating2.Renderer = new MenuRenderer(Resources.StarYellow);
			IdleProcess.CancelIdle += (object a, CancelEventArgs b) =>
            {
                b.Cancel = !IdleProcess.ShouldProcess(this) && !IdleProcess.ShouldProcess(readerForm);
            };
			Program.StartupProgress(TR.Messages["LoadComic", "Opening Files"], 90);
			Refresh();
			foreach (string commandLineFile in Program.CommandLineFiles)
			{
				if (File.Exists(commandLineFile))
				{
					OpenSupportedFile(commandLineFile, newSlot: false, 0, fromShell: true);
				}
			}
			if (books.OpenCount == 0 && Program.Settings.OpenLastFile)
			{
				List<string> files = new List<string>(Program.Settings.LastOpenFiles);
				books.Open(files, OpenComicOptions.NoIncreaseOpenedCount | OpenComicOptions.AppendNewSlots);
			}
			if (Program.Settings.ShowQuickManual)
			{
				Program.Settings.ShowQuickManual = false;
				books.Open(Program.QuickHelpManualFile, OpenComicOptions.OpenInNewSlot | OpenComicOptions.NoFileUpdate);
			}
			if (!string.IsNullOrEmpty(Program.ExtendedSettings.ImportList))
			{
				ImportComicList(Program.ExtendedSettings.ImportList);
			}
			Program.NetworkManager.BroadcastStart();
			VisibilityAnimator.EnableAnimation = Program.Settings.AnimatePanels && !Program.ExtendedSettings.DisableMenuHideShowAnimation;
			SizableContainer.EnableAnimation = Program.Settings.AnimatePanels;
			Program.Settings.AnimatePanelsChanged += delegate
			{
				SizableContainer.EnableAnimation = Program.Settings.AnimatePanels;
			};
			if (books.Slots.Count == 0)
			{
				RebuildBookTabs();
			}
			OnUpdateGui();
			IsInitialized = true;
			this.BeginInvoke(delegate
			{
				ScriptUtility.Invoke(PluginEngine.ScriptTypeStartup);
			});
		}

		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
			if (!mainViewContainer.Expanded)
			{
				ComicDisplay.Focus();
			}
			else
			{
				mainView.Focus();
			}
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);
			if (e.CloseReason == CloseReason.UserClosing && Program.Settings.CloseMinimizesToTray && !menuClose)
			{
				MinimizeToTray();
				if ((Program.Settings.HiddenMessageBoxes & HiddenMessageBoxes.ComicRackMinimized) == 0)
				{
					notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
					notifyIcon.BalloonTipText = TR.Messages["ComicRackMinimized", "You either close ComicRack with File/Exit or you can change this behavior in the Preferences Dialog.\nClick here to not show this message again"];
					notifyIcon.BalloonTipTitle = TR.Messages["ComicRackMinimizedTitle", "ComicRack is still running"];
					notifyIcon.Tag = HiddenMessageBoxes.ComicRackMinimized;
					notifyIcon.ShowBalloonTip(5000);
				}
				e.Cancel = true;
				return;
			}
			if (Program.Settings.UpdateComicFiles)
			{
				IEnumerable<ComicBook> dirtyTempList = Program.BookFactory.TemporaryBooks.Where((ComicBook cb) => cb.ComicInfoIsDirty);
				int dirtyCount = dirtyTempList.Count();
				if (dirtyCount != 0 && Program.AskQuestion(this, TR.Messages["AskDirtyItems", "Save changed information for Books that are not in the database?\nAll changes not saved now will be lost!"], TR.Default["Save", "Save"], HiddenMessageBoxes.AskDirtyItems, TR.Messages["AlwaysSaveDirty", "Always save changes"], TR.Default["No", "No"]))
				{
					AutomaticProgressDialog.Process(this, TR.Messages["SaveInfo", "Saving Book Information"], TR.Messages["SaveInfoText", "Please wait while all unsaved information is stored!"], 5000, delegate
					{
						int num = 0;
						foreach (ComicBook item in dirtyTempList)
						{
							if (AutomaticProgressDialog.ShouldAbort)
							{
								break;
							}
							AutomaticProgressDialog.Value = num++ * 100 / dirtyCount;
							Program.QueueManager.WriteInfoToFileWithCacheUpdate(item);
						}
					}, AutomaticProgressDialogOptions.EnableCancel);
				}
			}
			if (Program.QueueManager.IsActive && !QuestionDialog.Ask(this, TR.Messages["BackgroundConvert", "Files are still being updated/converted/synchronized in the background. If you close now, some information will not be written!"], TR.Messages["CloseComicRack", "Close ComicRack"]))
			{
				e.Cancel = true;
				return;
			}
			if (ScriptUtility.Enabled)
			{
				Program.Settings.PluginsStates = ScriptUtility.Scripts.CommandStates;
			}
			Program.Settings.LastOpenFiles.Clear();
			Program.Settings.LastOpenFiles.AddRange(books.OpenFiles);
			StoreWorkspace();
			Program.Settings.QuickOpenThumbnailSize = quickOpenView.ThumbnailSize;
			Program.Settings.MagnifySize = ComicDisplay.MagnifierSize;
			Program.Settings.MagnifyOpaque = ComicDisplay.MagnifierOpacity;
			Program.Settings.MagnifyZoom = ComicDisplay.MagnifierZoom;
			Program.Settings.MagnifyStyle = ComicDisplay.MagnifierStyle;
			Program.Settings.AutoHideMagnifier = ComicDisplay.AutoHideMagnifier;
			Program.Settings.AutoMagnifier = ComicDisplay.AutoMagnifier;
			Program.Settings.ThumbCacheEnabled = Program.ImagePool.Thumbs.DiskCache.Enabled;
			Program.Settings.PageFilter = ComicDisplay.PageFilter;
			Program.Settings.ReaderKeyboardMapping.Clear();
			Program.Settings.ReaderKeyboardMapping.AddRange(ComicDisplay.KeyboardMap.GetKeyMapping());
			Program.NetworkManager.BroadcastStop();
			if (readerForm != null)
			{
				readerForm.Dispose();
				readerForm = null;
			}
		}

		private void ConstraintMainView(bool always)
		{
			if ((base.Visible || always) && mainView.Dock != DockStyle.Fill && base.WindowState != FormWindowState.Minimized)
			{
				Rectangle displayRectangle = DisplayRectangle;
				if (fileTabsVisibility.Visible)
				{
					displayRectangle.Y = fileTabs.Bottom;
					displayRectangle.Height -= fileTabs.Bottom;
				}
				if (statusStrip.Visible)
				{
					displayRectangle.Height -= statusStrip.Height;
				}
				mainViewContainer.Bounds = Rectangle.Intersect(displayRectangle, mainViewContainer.Bounds);
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			ConstraintMainView(always: false);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (MinimizedToTray)
			{
				base.Visible = false;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			menuDown = e.KeyCode == Keys.Menu;
			if (!mainKeys.HandleKey(e.KeyCode | e.Modifiers))
			{
				base.OnKeyDown(e);
			}
		}

		private void InitializeCommands()
		{
			tbTools.DropDownOpening += delegate
			{
				mainToolStrip.DefaultDropDownDirection = ToolStripDropDownDirection.BelowLeft;
			};
			tbTools.DropDownClosed += delegate
			{
				mainToolStrip.DefaultDropDownDirection = ToolStripDropDownDirection.Default;
			};
			commands.Add(ShowOpenDialog, miOpenComic, cmOpenComic, tbOpenComic);
			commands.Add(OpenBooks.Close, () => OpenBooks.Slots.Count > 0, miCloseComic, cmClose, cmCloseComic);
			commands.Add(OpenBooks.CloseAll, () => OpenBooks.Slots.Count > 0, miCloseAllComics);
			commands.Add(OpenBooks.AddSlot, miAddTab);
			commands.Add(delegate
			{
				AddNewBook();
			}, miNewComic);
			commands.Add(delegate
			{
				OpenNextComic();
			}, miNextFromList, tbNextFromList, cmNextFromList);
			commands.Add(delegate
			{
				OpenPrevComic();
			}, miPrevFromList, tbPrevFromList, cmPrevFromList);
			commands.Add(delegate
			{
				OpenRandomComic();
			}, miRandomFromList, tbRandomFromList, cmRandomFromList);
			commands.Add(delegate
			{
				SyncBrowser();
			}, () => ComicDisplay.Book != null, miSyncBrowser, cmSyncBrowser);
			commands.Add(AddFolderToLibrary, miAddFolderToLibrary);
			commands.Add(StartFullScan, miScan, tbScan);
			commands.Add(UpdateComics, miUpdateAllComicFiles, tbUpdateAllComicFiles);
			commands.Add(MenuSynchronizeDevices, miSynchronizeDevices, tsSynchronizeDevices);
			commands.Add(UpdateWebComics, miUpdateWebComics, tbUpdateWebComics);
			commands.Add(delegate
			{
				ShowPendingTasks();
			}, miTasks);
			commands.Add(MenuRestart, miRestart);
			commands.Add(MenuClose, miExit, cmNotifyExit, tbExit);
			commands.Add(OpenRemoteLibrary, miOpenRemoteLibrary, tbOpenRemoteLibrary);
			commands.Add(ComicDisplay.DisplayFirstPage, () => ComicDisplay.Book != null && ComicDisplay.Book.CanNavigate(-1), miFirstPage, tbFirstPage);
			commands.Add(delegate
			{
				ComicDisplay.DisplayPreviousPage(ComicDisplay.PagingMode.Double);
			}, () => ComicDisplay.Book != null, miPrevPage, tbPrevPage);
			commands.Add(delegate
			{
				ComicDisplay.DisplayNextPage(ComicDisplay.PagingMode.Double);
			}, () => ComicDisplay.Book != null, miNextPage, tbNextPage);
			commands.Add(ComicDisplay.DisplayLastPage, () => ComicDisplay.Book != null && ComicDisplay.Book.CanNavigate(1), miLastPage, tbLastPage);
			commands.Add(ComicDisplay.DisplayPreviousBookmarkedPage, () => ComicDisplay.Book != null && ComicDisplay.Book.CanNavigateBookmark(-1), miPrevBookmark, tbPrevBookmark, cmPrevBookmark);
			commands.Add(ComicDisplay.DisplayNextBookmarkedPage, () => ComicDisplay.Book != null && ComicDisplay.Book.CanNavigateBookmark(1), miNextBookmark, tbNextBookmark, cmNextBookmark);
			commands.Add(SetBookmark, SetBookmarkAvailable, miSetBookmark, tbSetBookmark, cmSetBookmark);
			commands.Add(RemoveBookmark, RemoveBookmarkAvailable, miRemoveBookmark, tbRemoveBookmark, cmRemoveBookmark);
			commands.Add(ComicDisplay.DisplayLastPageRead, () => ComicDisplay.Book != null && ComicDisplay.Book.CurrentPage != ComicDisplay.Book.Comic.LastPageRead, miLastPageRead, tbLastPageRead, cmLastPageRead);
			commands.Add(OpenBooks.PreviousSlot, () => OpenBooks.Slots.Count > 1, miPrevTab);
			commands.Add(OpenBooks.NextSlot, () => OpenBooks.Slots.Count > 1, miNextTab);
			commands.AddService(this, (ILibraryBrowser s) =>
            {
                s.BrowseNext();
            }, (ILibraryBrowser s) => s.CanBrowseNext(), miNextList);
			commands.AddService(this, (ILibraryBrowser s) =>
            {
                s.BrowsePrevious();
            }, (ILibraryBrowser s) => s.CanBrowsePrevious(), miPreviousList);
			commands.Add(delegate
			{
				Program.Settings.AutoScrolling = !Program.Settings.AutoScrolling;
			}, true, () => Program.Settings.AutoScrolling, miAutoScroll, tbAutoScroll);
			commands.Add(delegate
			{
				ComicDisplay.TwoPageNavigation = !ComicDisplay.TwoPageNavigation;
			}, true, () => ComicDisplay.TwoPageNavigation, miDoublePageAutoScroll);
			commands.Add(delegate
			{
				ComicDisplay.RightToLeftReading = !ComicDisplay.RightToLeftReading;
			}, true, () => ComicDisplay.RightToLeftReading, miRightToLeft, tbRightToLeft, cmRightToLeft);
			commands.Add(ShowInfo, () => this.InvokeActiveService((IGetBookList bl) => !bl.GetBookList(ComicBookFilterType.Selected).IsEmpty(), defaultReturn: false), miShowInfo, tbShowInfo, cmShowInfo);
			commands.Add(Program.Database.Undo.Undo, () => Program.Database.Undo.CanUndo, miUndo);
			commands.Add(Program.Database.Undo.Redo, () => Program.Database.Undo.CanRedo, miRedo);
			commands.Add(ComicDisplay.CopyPageToClipboard, () => ComicDisplay.Book != null, miCopyPage, cmCopyPage);
			commands.Add(ExportCurrentImage, () => ComicDisplay.Book != null, miExportPage, cmExportPage);
			commands.Add(ToggleUndockReader, true, () => ReaderUndocked, miReaderUndocked, tbReaderUndocked);
			commands.Add(delegate
			{
				MinimalGui = !MinimalGui;
			}, true, () => MinimalGui, miMinimalGui, cmMinimalGui, tbMinimalGui);
			commands.Add(ComicDisplay.ToggleFullScreen, true, () => ComicDisplay.FullScreen, miFullScreen, tbFullScreen);
			commands.Add(ComicDisplay.TogglePageLayout, tbPageLayout);
			commands.Add(delegate
			{
				ComicDisplay.PageLayout = PageLayoutMode.Single;
			}, true, () => ComicDisplay.PageLayout == PageLayoutMode.Single, miSinglePage, tbSinglePage, cmSinglePage);
			commands.Add(delegate
			{
				ComicDisplay.PageLayout = PageLayoutMode.Double;
			}, true, () => ComicDisplay.PageLayout == PageLayoutMode.Double, miTwoPages, tbTwoPages, cmTwoPages);
			commands.Add(delegate
			{
				ComicDisplay.PageLayout = PageLayoutMode.DoubleAdaptive;
			}, true, () => ComicDisplay.PageLayout == PageLayoutMode.DoubleAdaptive, miTwoPagesAdaptive, tbTwoPagesAdaptive, cmTwoPagesAdaptive);
			commands.Add(ComicDisplay.TogglePageFit, tbFit);
			commands.Add(ComicDisplay.SetPageOriginal, true, () => ComicDisplay.ImageFitMode == ImageFitMode.Original, miOriginal, cmOriginal, tbOriginal);
			commands.Add(ComicDisplay.SetPageFitAll, true, () => ComicDisplay.ImageFitMode == ImageFitMode.Fit, miFitAll, tbFitAll, cmFitAll);
			commands.Add(ComicDisplay.SetPageFitWidth, true, ComicDisplay.IsPageFitWidth, miFitWidth, tbFitWidth, cmFitWidth);
			commands.Add(ComicDisplay.SetPageFitWidthAdaptive, true, ComicDisplay.IsPageFitWidthAdaptive, miFitWidthAdaptive, tbFitWidthAdaptive, cmFitWidthAdaptive);
			commands.Add(ComicDisplay.SetPageFitHeight, true, ComicDisplay.IsPageFitHeight, miFitHeight, tbFitHeight, cmFitHeight);
			commands.Add(ComicDisplay.SetPageBestFit, true, ComicDisplay.IsPageFitBest, miBestFit, tbBestFit, cmFitBest);
			commands.Add(ComicDisplay.ToggleFitOnlyIfOversized, true, () => ComicDisplay.ImageFitOnlyIfOversized, miOnlyFitOversized, tbOnlyFitOversized, cmOnlyFitOversized);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = Numeric.Select(ComicDisplay.ImageZoom, new float[4]
				{
					1f,
					1.25f,
					1.5f,
					2f
				}, wrap: true);
			}, tbZoom);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = (ComicDisplay.ImageZoom + 0.1f).Clamp(1f, 8f);
			}, () => ComicDisplay.ImageZoom < 8f, miZoomIn, tbZoomIn);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = (ComicDisplay.ImageZoom - 0.1f).Clamp(1f, 8f);
			}, () => ComicDisplay.ImageZoom > 1f, miZoomOut, tbZoomOut);
			commands.Add(delegate
			{
				ToggleZoom(CommandKey.None);
			}, miToggleZoom);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = 1f;
			}, miZoom100, tbZoom100);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = 1.25f;
			}, miZoom125, tbZoom125);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = 1.5f;
			}, miZoom150, tbZoom150);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = 2f;
			}, miZoom200, tbZoom200);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = 4f;
			}, miZoom400, tbZoom400);
			commands.Add(delegate
			{
				ComicDisplay.ImageZoom = ZoomDialog.Show(this, ComicDisplay.ImageZoom);
			}, miZoomCustom, tbZoomCustom);
			commands.Add(delegate
			{
				ComicDisplay.ImageRotation = ImageRotation.None;
			}, true, () => ComicDisplay.ImageRotation == ImageRotation.None, miRotate0, tbRotate0, cmRotate0);
			commands.Add(delegate
			{
				ComicDisplay.ImageRotation = ImageRotation.Rotate90;
			}, true, () => ComicDisplay.ImageRotation == ImageRotation.Rotate90, miRotate90, tbRotate90, cmRotate90);
			commands.Add(delegate
			{
				ComicDisplay.ImageRotation = ImageRotation.Rotate180;
			}, true, () => ComicDisplay.ImageRotation == ImageRotation.Rotate180, miRotate180, tbRotate180, cmRotate180);
			commands.Add(delegate
			{
				ComicDisplay.ImageRotation = ImageRotation.Rotate270;
			}, true, () => ComicDisplay.ImageRotation == ImageRotation.Rotate270, miRotate270, tbRotate270, cmRotate270);
			commands.Add(delegate
			{
				ComicDisplay.ImageRotation = ComicDisplay.ImageRotation.RotateLeft();
			}, miRotateLeft, tbRotateLeft);
			commands.Add(delegate
			{
				ComicDisplay.ImageRotation = ComicDisplay.ImageRotation.RotateRight();
			}, miRotateRight, tbRotateRight, tbRotate);
			commands.Add(delegate
			{
				ComicDisplay.ImageAutoRotate = !ComicDisplay.ImageAutoRotate;
			}, true, () => ComicDisplay.ImageAutoRotate, miAutoRotate, tbAutoRotate);
			commands.Add(ComicDisplay.ToggleMagnifier, true, () => ComicDisplay.MagnifierVisible, miMagnify, tbMagnify, cmMagnify);
			commands.Add(delegate
			{
				ShowPortableDevices();
			}, miDevices);
			commands.Add(delegate
			{
				ShowPreferences();
			}, miPreferences, tbPreferences);
			commands.Add(delegate
			{
				Program.Settings.AutoHideMainMenu = !Program.Settings.AutoHideMainMenu;
			}, true, () => !Program.Settings.AutoHideMainMenu, tbShowMainMenu);
			commands.Add(delegate
			{
				BrowserVisible = true;
				mainView.ShowLibrary();
			}, miViewLibrary);
			commands.Add(delegate
			{
				BrowserVisible = true;
				mainView.ShowFolders();
			}, miViewFolders);
			commands.Add(delegate
			{
				BrowserVisible = true;
				mainView.ShowPages();
			}, () => OpenBooks.CurrentBook != null, miViewPages);
			commands.Add(ToggleBrowser, true, () => BrowserVisible, miToggleBrowser);
			commands.Add(ToggleSidebar, CheckSidebarAvailable, CheckSidebarEnabled, miSidebar);
			commands.Add(ToggleSmallPreview, CheckSidebarAvailable, CheckSmallPreviewEnabled, miSmallPreview);
			commands.Add(ToggleSearchBrowser, CheckSearchAvailable, CheckSearchBrowserEnabled, miSearchBrowser);
			commands.Add(ToggleInfoPanel, CheckInfoPanelAvailable, CheckInfoPanelEnabled, miInfoPanel);
			commands.AddService(this, (IRefreshDisplay c) =>
            {
                c.RefreshDisplay();
            }, miViewRefresh);
			commands.Add(delegate
			{
				GetRatingEditor().SetRating(0f);
			}, () => GetRatingEditor().IsValid(), () => Math.Round(GetRatingEditor().GetRating()) == 0.0, miRate0, cmRate0);
			commands.Add(delegate
			{
				GetRatingEditor().SetRating(1f);
			}, () => GetRatingEditor().IsValid(), () => Math.Round(GetRatingEditor().GetRating()) == 1.0, miRate1, cmRate1);
			commands.Add(delegate
			{
				GetRatingEditor().SetRating(2f);
			}, () => GetRatingEditor().IsValid(), () => Math.Round(GetRatingEditor().GetRating()) == 2.0, miRate2, cmRate2);
			commands.Add(delegate
			{
				GetRatingEditor().SetRating(3f);
			}, () => GetRatingEditor().IsValid(), () => Math.Round(GetRatingEditor().GetRating()) == 3.0, miRate3, cmRate3);
			commands.Add(delegate
			{
				GetRatingEditor().SetRating(4f);
			}, () => GetRatingEditor().IsValid(), () => Math.Round(GetRatingEditor().GetRating()) == 4.0, miRate4, cmRate4);
			commands.Add(delegate
			{
				GetRatingEditor().SetRating(5f);
			}, () => GetRatingEditor().IsValid(), () => Math.Round(GetRatingEditor().GetRating()) == 5.0, miRate5, cmRate5);
			commands.Add(delegate
			{
				GetRatingEditor().QuickRatingAndReview();
			}, () => GetRatingEditor().IsValid(), miQuickRating, cmQuickRating);
			commands.Add(delegate
			{
				if (!Program.Help.Execute("HelpMain"))
				{
					Program.StartDocument(Program.DefaultWiki);
				}
			}, miWebHelp);
			commands.Add(delegate
			{
				books.Open(Program.QuickHelpManualFile, OpenComicOptions.OpenInNewSlot | OpenComicOptions.NoFileUpdate);
			}, miHelpQuickIntro);
			commands.Add(delegate
			{
				Program.StartDocument(Program.DefaultWebSite);
			}, miWebHome);
			commands.Add(delegate
			{
				Program.StartDocument(Program.DefaultUserForm);
			}, miWebUserForum);
			commands.Add(ShowAboutDialog, miAbout, tbAbout);
			commands.Add(ShowNews, miNews);
			commands.Add(SaveWorkspace, tsSaveWorkspace, miSaveWorkspace);
			commands.Add(EditWorkspace, () => Program.Settings.Workspaces.Count > 0, tsEditWorkspaces, miEditWorkspaces);
			commands.Add(EditWorkspaceDisplaySettings, miComicDisplaySettings, tbComicDisplaySettings);
			commands.Add(EditListLayout, CheckViewOptionsAvailable, miEditListLayout);
			commands.Add(SaveListLayout, miSaveListLayout);
			commands.Add(EditListLayouts, () => Program.Settings.ListConfigurations.Count > 0, miEditLayouts);
			commands.Add(delegate
			{
				SetListLayoutToAll();
			}, miSetAllListsSame);
			commands.Add(delegate
			{
				Program.Settings.TrackCurrentPage = !Program.Settings.TrackCurrentPage;
			}, true, () => Program.Settings.TrackCurrentPage, miTrackCurrentPage);
			commands.Add(ComicDisplay.RefreshDisplay, ComicDisplay.IsValid, cmRefreshPage);
			commands.Add(RestoreFromTray, cmNotifyRestore);
			commands.Add(OpenBooks.CloseAllButCurrent, () => OpenBooks.Slots.Count > 0, cmCloseAllButThis);
			commands.Add(OpenBooks.CloseAllToTheRight, () => OpenBooks.CurrentSlot < OpenBooks.Slots.Count - 1, cmCloseAllToTheRight);
			commands.Add(delegate
			{
				Clipboard.SetText(ComicDisplay.Book.Comic.FilePath);
			}, () => ComicDisplay.Book != null && ComicDisplay.Book.Comic.EditMode.IsLocalComic(), cmCopyPath);
			commands.Add(delegate
			{
				Program.ShowExplorer(ComicDisplay.Book.Comic.FilePath);
			}, () => ComicDisplay.Book != null && ComicDisplay.Book.Comic.EditMode.IsLocalComic(), cmRevealInExplorer);
		}

		private void InitializeKeyboard()
		{
			string group = "Library";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miNextFromList.Image, "NextComic", group, "Next Book", (Action)delegate
			{
				OpenNextComic();
			}, new CommandKey[1]
			{
				CommandKey.N
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miPrevFromList.Image, "PrevComic", group, "Previous Book", (Action)delegate
			{
				OpenPrevComic();
			}, new CommandKey[1]
			{
				CommandKey.P
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miRandomFromList.Image, "RandomComic", group, "Random Book", (Action)delegate
			{
				OpenRandomComic();
			}, new CommandKey[1]
			{
				CommandKey.L
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miToggleBrowser.Image, "ShowBrowser", group, "Show Browser", ToggleBrowserFromReader, CommandKey.MouseLeft, CommandKey.Escape));
			group = "Browse";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miFirstPage.Image, "MoveToFirstPage", group, "First Page", ComicDisplay.DisplayFirstPage, CommandKey.Home | CommandKey.Ctrl, CommandKey.GestureDouble1));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miPrevPage.Image, "MoveToPreviousPage", group, "Previous Page", (Action)delegate
			{
				ComicDisplay.DisplayPreviousPage(ComicDisplay.PagingMode.Double);
			}, new CommandKey[4]
			{
				CommandKey.PageUp,
				CommandKey.Left | CommandKey.Alt,
				CommandKey.Gesture1,
				CommandKey.FlickRight
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miNextPage.Image, "MoveToNextPage", group, "Next Page", (Action)delegate
			{
				ComicDisplay.DisplayNextPage(ComicDisplay.PagingMode.Double);
			}, new CommandKey[4]
			{
				CommandKey.PageDown,
				CommandKey.Right | CommandKey.Alt,
				CommandKey.Gesture3,
				CommandKey.FlickLeft
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miLastPage.Image, "MoveToLastPage", group, "Last Page", ComicDisplay.DisplayLastPage, CommandKey.End | CommandKey.Ctrl, CommandKey.GestureDouble3));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miPrevBookmark.Image, "MoveToPrevBookmark", group, "Previous Bookmark", ComicDisplay.DisplayPreviousBookmarkedPage, CommandKey.PageUp | CommandKey.Ctrl));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miNextBookmark.Image, "MoveToNextBookmark", group, "Next Bookmark", ComicDisplay.DisplayNextBookmarkedPage, CommandKey.PageDown | CommandKey.Ctrl));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miPrevTab.Image, "PrevTab", group, "Previous Tab", OpenBooks.PreviousSlot, CommandKey.Tab | CommandKey.Shift));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miNextTab.Image, "NextTab", group, "Next Tab", OpenBooks.NextSlot, CommandKey.Tab));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveToPrevPageSingle", group, "Single Page Back", delegate
			{
				ComicDisplay.DisplayPreviousPage(ComicDisplay.PagingMode.None);
			}, CommandKey.PageUp | CommandKey.Shift));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveToNextPageSingle", group, "Single Page Forward", delegate
			{
				ComicDisplay.DisplayNextPage(ComicDisplay.PagingMode.None);
			}, CommandKey.PageDown | CommandKey.Shift));
			group = "Auto Scroll";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MovePrevPart", group, "Previous Part", delegate
			{
				ComicDisplay.DisplayPreviousPageOrPart();
			}, CommandKey.Space | CommandKey.Shift, CommandKey.Gesture7));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveNextPart", group, "Next Part", delegate
			{
				ComicDisplay.DisplayNextPageOrPart();
			}, CommandKey.Space, CommandKey.Gesture9));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveFirstPart", group, "Page Start", delegate
			{
				ComicDisplay.DisplayPart(PartPageToDisplay.First);
			}, CommandKey.Home));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveLastPart", group, "Page End", delegate
			{
				ComicDisplay.DisplayPart(PartPageToDisplay.Last);
			}, CommandKey.End));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MovePartDown10", group, "Move Part 10% down", delegate
			{
				ComicDisplay.MovePartDown(0.1f);
			}, CommandKey.V, CommandKey.Down | CommandKey.Ctrl));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MovePartUp10", group, "Move Part 10% up", delegate
			{
				ComicDisplay.MovePartDown(-0.1f);
			}, CommandKey.B, CommandKey.Up | CommandKey.Ctrl));
			group = "Scroll";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miAutoScroll.Image, "ToggleAutoScrolling", group, "Toggle Auto Scrolling", (Action)delegate
			{
				Program.Settings.AutoScrolling = !Program.Settings.AutoScrolling;
			}, new CommandKey[1]
			{
				CommandKey.S
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("DoublePageAutoScroll", group, "Double Page Auto Scroll", delegate
			{
				ComicDisplay.TwoPageNavigation = !ComicDisplay.TwoPageNavigation;
			}, CommandKey.S | CommandKey.Shift));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveUp", group, "Up", ComicDisplay.ScrollUp, CommandKey.Up, CommandKey.MouseWheelUp));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveDown", group, "Down", ComicDisplay.ScrollDown, CommandKey.Down, CommandKey.MouseWheelDown));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveLeft", group, "Left", ComicDisplay.ScrollLeft, CommandKey.Left, CommandKey.MouseTiltLeft));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveRight", group, "Right", ComicDisplay.ScrollRight, CommandKey.Right, CommandKey.MouseTiltRight));
			group = "Display Options";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miReaderUndocked.Image, "ToggleUndockReader", group, "Toggle Undock Reader", ToggleUndockReader, CommandKey.D));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miFullScreen.Image, "ToggleFullScreen", group, "Toggle Full Screen", ComicDisplay.ToggleFullScreen, CommandKey.F, CommandKey.MouseDoubleLeft, CommandKey.Gesture2));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miTwoPages.Image, "ToggleTwoPages", group, "Toggle Two Pages", ComicDisplay.TogglePageLayout, CommandKey.T));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("ToggleRealisticPages", group, "Toggle Realistic Display", ComicDisplay.ToogleRealisticPages, CommandKey.D | CommandKey.Shift));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miMagnify.Image, "ToggleMagnify", group, "Toggle Magnifier", (Action)delegate
			{
				ComicDisplay.MagnifierVisible = !ComicDisplay.MagnifierVisible;
			}, new CommandKey[2]
			{
				CommandKey.M,
				CommandKey.TouchPressAndTap
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("ToggleMenu", group, "Toggle Menu", delegate
			{
				MinimalGui = !MinimalGui;
			}, CommandKey.K));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(null, "ToggleNavigationOverlay", group, "Toggle Navigation Overlay", ComicDisplay.ToggleNavigationOverlay, CommandKey.Gesture8, CommandKey.TouchTwoFingerTap));
			group = "Page Display";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miOriginal.Image, "Original", group, "Original Size", ComicDisplay.SetPageOriginal, CommandKey.D1));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miFitAll.Image, "FitAll", group, "Fit All", ComicDisplay.SetPageFitAll, CommandKey.D2));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miFitWidth.Image, "FitWidth", group, "Fit Width", ComicDisplay.SetPageFitWidth, CommandKey.D3));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miFitWidthAdaptive.Image, "FitWidthAdaptive", group, "Fit Width (adaptive)", ComicDisplay.SetPageFitWidthAdaptive, CommandKey.D4));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miFitHeight.Image, "FitHeight", group, "Fit Height", ComicDisplay.SetPageFitHeight, CommandKey.D5));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miBestFit.Image, "FitBest", group, "Best Fit", ComicDisplay.SetPageBestFit, CommandKey.D6));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miSinglePage.Image, "SinglePage", group, "Single Page", (Action)delegate
			{
				ComicDisplay.PageLayout = PageLayoutMode.Single;
			}, new CommandKey[1]
			{
				CommandKey.D7
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miTwoPages.Image, "TwoPages", group, "Two Pages", (Action)delegate
			{
				ComicDisplay.PageLayout = PageLayoutMode.Double;
			}, new CommandKey[1]
			{
				CommandKey.D8
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miTwoPagesAdaptive.Image, "TwoPagesAdaptive", group, "Two Pages (adaptive)", (Action)delegate
			{
				ComicDisplay.PageLayout = PageLayoutMode.DoubleAdaptive;
			}, new CommandKey[1]
			{
				CommandKey.D9
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miRightToLeft.Image, "RightToLeft", group, "Right to Left", (Action)delegate
			{
				ComicDisplay.RightToLeftReading = !ComicDisplay.RightToLeftReading;
			}, new CommandKey[1]
			{
				CommandKey.D0
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miOnlyFitOversized.Image, "OnlyFitIfOversized", group, "Only Fit if oversized", ComicDisplay.ToggleFitOnlyIfOversized, CommandKey.O));
			group = "ZoomAndRotate";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miRotateRight.Image, "RotateC", group, "Rotate Right", (Action)delegate
			{
				ComicDisplay.ImageRotation = ComicDisplay.ImageRotation.RotateRight();
			}, new CommandKey[1]
			{
				CommandKey.R
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miRotateLeft.Image, "RotateCC", group, "Rotate Left", (Action)delegate
			{
				ComicDisplay.ImageRotation = ComicDisplay.ImageRotation.RotateLeft();
			}, new CommandKey[1]
			{
				CommandKey.R | CommandKey.Shift
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miAutoRotate.Image, "AutoRotate", group, "Autorotate Double Pages", (Action)delegate
			{
				ComicDisplay.ImageAutoRotate = !ComicDisplay.ImageAutoRotate;
			}, new CommandKey[1]
			{
				CommandKey.A
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miZoomIn.Image, "ZoomIn", group, "Zoom In", (Action)delegate
			{
				ComicDisplay.ImageZoom = (ComicDisplay.ImageZoom + 0.1f).Clamp(1f, 8f);
			}, new CommandKey[1]
			{
				CommandKey.MouseWheelUp | CommandKey.Ctrl
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miZoomOut.Image, "ZoomOut", group, "Zoom Out", (Action)delegate
			{
				ComicDisplay.ImageZoom = (ComicDisplay.ImageZoom - 0.1f).Clamp(1f, 8f);
			}, new CommandKey[1]
			{
				CommandKey.MouseWheelDown | CommandKey.Ctrl
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miZoomIn.Image, "StepZoomIn", group, "Step Zoom In", (Action)delegate
			{
				ComicDisplay.ImageZoom = (ComicDisplay.ImageZoom + Program.ExtendedSettings.KeyboardZoomStepping).Clamp(1f, 4f);
			}, new CommandKey[1]
			{
				CommandKey.Z
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miZoomOut.Image, "StepZoomOut", group, "Step Zoom Out", (Action)delegate
			{
				ComicDisplay.ImageZoom = (ComicDisplay.ImageZoom - Program.ExtendedSettings.KeyboardZoomStepping).Clamp(1f, 4f);
			}, new CommandKey[1]
			{
				CommandKey.Z | CommandKey.Shift
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand(miToggleZoom.Image, "ToggleZoom", group, "Toggle Zoom", ToggleZoom, CommandKey.TouchDoubleTap));
			group = "Edit";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand((Image)Resources.Rotate90Permanent, "PageRotateC", group, "Rotate Page Right", (Action)delegate
			{
				GetPageEditor().Rotation = GetPageEditor().Rotation.RotateRight();
			}, new CommandKey[1]
			{
				CommandKey.Y
			}));
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand((Image)Resources.Rotate270Permanent, "PageRotateCC", group, "Rotate Page Left", (Action)delegate
			{
				GetPageEditor().Rotation = GetPageEditor().Rotation.RotateLeft();
			}, new CommandKey[1]
			{
				CommandKey.Y | CommandKey.Shift
			}));
			group = "Other";
			ComicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("Exit", group, "Exit", ControlExit, CommandKey.Q));
			Program.DefaultKeyboardMapping = ComicDisplay.KeyboardMap.GetKeyMapping().ToArray();
			ComicDisplay.KeyboardMap.SetKeyMapping(Program.Settings.ReaderKeyboardMapping);
			mainKeys.Commands.Add(new KeyboardCommand("FocusQuickSearch", "General", "FQS", FocusQuickSearch, CommandKey.F | CommandKey.Ctrl));
		}

		public bool AddRemoteLibrary(ShareInformation info, MainView.AddRemoteLibraryOptions options)
		{
			if (info == null)
			{
				return false;
			}
			if (mainView.IsRemoteConnected(info.Uri))
			{
				return true;
			}
			mainView.AddRemoteLibrary(ComicLibraryClient.Connect(info), options);
			return true;
		}

		public void OnRemoteServerStarted(ShareInformation info)
		{
			MainView.AddRemoteLibraryOptions addRemoteLibraryOptions = MainView.AddRemoteLibraryOptions.Auto;
			if (Program.Settings.AutoConnectShares && info.IsLocal)
			{
				addRemoteLibraryOptions |= MainView.AddRemoteLibraryOptions.Open;
			}
			AddRemoteLibrary(info, addRemoteLibraryOptions);
		}

		public void OnRemoteServerStopped(string address)
		{
			mainView.RemoveRemoteLibrary(address);
		}

		public void OpenRemoteLibrary()
		{
			RemoteShareItem share = OpenRemoteDialog.GetShare(this, Program.Settings.RemoteShares.First, Program.Settings.RemoteShares, showPublic: false);
			if (share != null && !string.IsNullOrEmpty(share.Uri))
			{
				string serverName = share.Uri;
				ShareInformation serverInfo = null;
				AutomaticProgressDialog.Process(this, TR.Messages["ConnectToServer", "Connecting to Server"], TR.Messages["GetShareInfoText", "Getting information about the shared Library"], 1000, delegate
				{
					serverInfo = ComicLibraryClient.GetServerInfo(serverName);
				}, AutomaticProgressDialogOptions.EnableCancel);
				if (serverInfo == null || !AddRemoteLibrary(serverInfo, MainView.AddRemoteLibraryOptions.Open | MainView.AddRemoteLibraryOptions.Select))
				{
					MessageBox.Show(this, StringUtility.Format(TR.Messages["ConnectRemoteError", "Failed to connect to remote Server"], share), TR.Messages["Error", "Error"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					Program.Settings.RemoteShares.UpdateMostRecent(new RemoteShareItem(serverInfo));
				}
			}
		}

		private void BookDragEnter(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			e.Effect = ((array != null && array.Length == 1) ? DragDropEffects.Copy : DragDropEffects.None);
		}

		private void BookDragDrop(object sender, DragEventArgs e)
		{
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			OpenSupportedFile(array[0]);
		}

		public ComicBook AddNewBook(bool showDialog = true)
		{
			ComicBook comicBook = new ComicBook
			{
				AddedTime = DateTime.Now
			};
			if (showDialog && !ComicBookDialog.Show(Form.ActiveForm ?? this, comicBook, null, null))
			{
				return null;
			}
			Program.Database.Add(comicBook);
			return comicBook;
		}

		public ComicListItem ImportComicList(string file)
		{
			return this.FindFirstService<IImportComicList>()?.ImportList(file);
		}

		public void MenuClose()
		{
			menuClose = true;
			Close();
		}

		public void MenuRestart()
		{
			Program.Restart = true;
			MenuClose();
		}

		public void UpdateFeeds()
		{
			using (ItemMonitor.Lock(Program.News))
			{
				Program.News.UpdateFeeds(Program.NewsIntervalMinutes);
			}
		}

		public void ShowNews(bool always)
		{
			if (always)
			{
				AutomaticProgressDialog.Process(this, TR.Messages["RetrieveNews", "Retrieving News"], TR.Messages["RetrieveNewsText", "Refreshing subscribed News Channels"], 1000, UpdateFeeds, AutomaticProgressDialogOptions.EnableCancel);
				NewsDialog.ShowNews(this, Program.News);
				return;
			}
			ThreadUtility.RunInBackground("Read News", delegate
			{
				UpdateFeeds();
				if (Program.News.HasUnread)
				{
					this.BeginInvoke(delegate
					{
						NewsDialog.ShowNews(this, Program.News);
					});
				}
			});
		}

		public void AddFolderToLibrary()
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.Description = TR.Messages["AddFolderLibrary", "Books in this Folder and all sub Folders will be added to the library."];
				folderBrowserDialog.ShowNewFolderButton = true;
				if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
				{
					Program.Scanner.ScanFileOrFolder(folderBrowserDialog.SelectedPath, all: true, removeMissing: false);
				}
			}
		}

		public void StartFullScan()
		{
			Program.QueueManager.StartScan(all: true, Program.Settings.RemoveMissingFilesOnFullScan);
		}

		public void UpdateComics()
		{
			Program.Database.Books.Concat(Program.BookFactory.TemporaryBooks).ForEach((ComicBook cb) =>
            {
                Program.QueueManager.AddBookToFileUpdate(cb, alwaysWrite: true);
            });
		}

		public void UpdateWebComics(bool refresh = false)
		{
			Program.Database.Books.Concat(Program.BookFactory.TemporaryBooks).ForEach((ComicBook cb) =>
            {
                UpdateWebComic(cb, refresh);
            });
		}

		public void UpdateWebComics()
		{
			UpdateWebComics(refresh: false);
		}

		public void UpdateWebComic(ComicBook cb, bool refresh)
		{
			Program.QueueManager.AddBookToDynamicUpdate(cb, refresh);
		}

		public bool OpenNextComic(int relative, OpenComicOptions openOptions)
		{
			if (ComicDisplay == null || ComicDisplay.Book == null)
			{
				return false;
			}
			ComicBook comic = ComicDisplay.Book.Comic;
			if (comic == null)
			{
				return false;
			}
			IComicBrowser comicBrowser = this.FindServices<IComicBrowser>().FirstOrDefault((IComicBrowser cb) => cb.Library == comic.Container);
			if (comicBrowser == null)
			{
				return false;
			}
			if (comicBrowser.Library != null && comic.LastOpenedFromListId != Guid.Empty)
			{
				ComicListItem comicListItem = comicBrowser.Library.ComicLists.GetItems<ComicListItem>().FirstOrDefault((ComicListItem li) => li.Id == comic.LastOpenedFromListId);
				if (comicListItem != null)
				{
					ShowBookInList(comicBrowser.Library, comicListItem, comic, switchToList: false);
				}
			}
			ComicBook[] array = comicBrowser.GetBookList(ComicBookFilterType.IsNotFileless).ToArray();
			if (array.Length == 0)
			{
				return false;
			}
			int num = array.FindIndex((ComicBook cb) => cb.Id == comic.Id);
			ComicBook comicBook;
			if (relative != 0)
			{
				if (num == -1)
				{
					return false;
				}
				num += relative;
				if (num < 0 || num >= array.Length)
				{
					return false;
				}
				comicBook = array[num];
			}
			else
			{
				if (!lastRandomList.SequenceEqual(array))
				{
					lastRandomList = array;
					randomSelectedComics = new List<ComicBook>();
				}
				if (lastRandomList.Length == randomSelectedComics.Count)
				{
					randomSelectedComics.Clear();
				}
				ComicBook[] array2 = lastRandomList.Except(randomSelectedComics).ToArray();
				int num2 = new Random().Next(0, array2.Length);
				comicBook = array2[num2];
				randomSelectedComics.Add(comicBook);
			}
			if (comicBook == null)
			{
				return false;
			}
			comicBook.LastOpenedFromListId = comicBrowser.GetBookListId();
			return books.Open(comicBook, openOptions);
		}

		public bool OpenNextComic(int relative)
		{
			return OpenNextComic(relative, OpenComicOptions.None);
		}

		public void ShowInfo()
		{
			IGetBookList getBookList = FormUtility.FindActiveService<IGetBookList>();
			if (getBookList == null)
			{
				return;
			}
			IEnumerable<ComicBook> bookList = getBookList.GetBookList(ComicBookFilterType.Selected);
			if (bookList.Count() > 1 && bookList.All((ComicBook cb) => cb.EditMode.CanEditProperties()))
			{
				Program.Database.Undo.SetMarker(TR.Messages["UndoEditMultipleComics", "Edit multiple Books"]);
				using (MultipleComicBooksDialog multipleComicBooksDialog = new MultipleComicBooksDialog(bookList))
				{
					multipleComicBooksDialog.ShowDialog(this);
				}
			}
			else if (!bookList.IsEmpty())
			{
				IComicBrowser comicBrowser = FormUtility.FindActiveService<IComicBrowser>();
				Program.Database.Undo.SetMarker(TR.Messages["UndoShowInfo", "Show Info"]);
				ComicBookDialog.Show(Form.ActiveForm ?? this, bookList.FirstOrDefault(), getBookList.GetBookList(ComicBookFilterType.All).ToArray(), (comicBrowser != null) ? new Func<ComicBook, bool>(comicBrowser.SelectComic) : null);
			}
		}

		public void ToggleBrowser(bool alwaysShow, IComicBrowser cb = null)
		{
			BrowserVisible = !BrowserVisible || alwaysShow;
			if (ReaderUndocked)
			{
				if (!BrowserVisible)
				{
					readerForm.Focus();
					return;
				}
				mainView.Focus();
				if (cb != null)
				{
					mainView.ShowLibrary(cb.Library);
				}
			}
			else if (!BrowserVisible)
			{
				UpdateBrowserVisibility();
			}
			else if (cb == null)
			{
				mainView.ShowLast();
			}
			else
			{
				mainView.ShowLibrary(cb.Library);
			}
		}

		public void ToggleBrowser()
		{
			ToggleBrowser(alwaysShow: false);
		}

		public void ToggleBrowserFromReader()
		{
			if (!Program.ExtendedSettings.MouseSwitchesToFullLibrary && (ReaderUndocked || mainViewContainer.Dock == DockStyle.Fill))
			{
				MinimalGui = !MinimalGui;
			}
			else
			{
				ToggleBrowser(alwaysShow: false);
			}
		}

		public IEditRating GetRatingEditor()
		{
			IGetBookList getBookList = FormUtility.FindActiveService<IGetBookList>();
			return new RatingEditor(Form.ActiveForm ?? this, getBookList?.GetBookList(ComicBookFilterType.IsEditable | ComicBookFilterType.Selected));
		}

		public IEditPage GetPageEditor()
		{
			return new PageEditorWrapper(FormUtility.FindActiveService<IEditPage>());
		}

		private BookmarkEditorWrapper GetBookmarkEditor()
		{
			return new BookmarkEditorWrapper(FormUtility.FindActiveService<IEditBookmark>());
		}

		private void SetBookmark()
		{
			BookmarkEditorWrapper bookmarkEditor = GetBookmarkEditor();
			if (bookmarkEditor.CanBookmark)
			{
				bookmarkEditor.Bookmark = SelectItemDialog.GetName<string>(Form.ActiveForm ?? this, TR.Default["Bookmark", "Bookmark"], bookmarkEditor.BookmarkProposal, null);
			}
		}

		private bool SetBookmarkAvailable()
		{
			return GetBookmarkEditor().CanBookmark;
		}

		private void RemoveBookmark()
		{
			GetBookmarkEditor().Bookmark = string.Empty;
		}

		private bool RemoveBookmarkAvailable()
		{
			return !string.IsNullOrEmpty(GetBookmarkEditor().Bookmark);
		}

		public void ExportImage(string name, Image image)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Title = LocalizeUtility.GetText(this, "SavePageTitle", "Save Page as");
				saveFileDialog.Filter = TR.Load("FileFilter")["PageImageSave", "JPEG Image|*.jpg|Windows Bitmap Image|*.bmp|PNG Image|*.png|GIF Image|*.gif|TIFF Image|*.tif"];
				saveFileDialog.FileName = FileUtility.MakeValidFilename(name);
				saveFileDialog.FilterIndex = Program.Settings.LastExportPageFilterIndex;
				IWin32Window owner = Form.ActiveForm ?? this;
				if (saveFileDialog.ShowDialog(owner) != DialogResult.OK)
				{
					return;
				}
				Program.Settings.LastExportPageFilterIndex = saveFileDialog.FilterIndex;
				name = saveFileDialog.FileName;
				try
				{
					switch (saveFileDialog.FilterIndex)
					{
					case 1:
						image.SaveImage(AddExtension(name, ".jpg"), ImageFormat.Jpeg, 24);
						break;
					case 2:
						image.SaveImage(AddExtension(name, ".bmp"), ImageFormat.Bmp, 24);
						break;
					case 3:
						image.SaveImage(AddExtension(name, ".png"), ImageFormat.Png, 24);
						break;
					case 4:
						image.SaveImage(AddExtension(name, ".gif"), ImageFormat.Gif, 8);
						break;
					case 5:
						image.SaveImage(AddExtension(name, ".tif"), ImageFormat.Tiff, 24);
						break;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, StringUtility.Format(TR.Messages["CouldNotSaveImage", "Could not save the page image!\nReason: {0}"], ex.Message), TR.Messages["Error", "Error"], MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
		}

		private static string AddExtension(string file, string ext)
		{
			if (!Path.HasExtension(file))
			{
				return file + ext;
			}
			return file;
		}

		private void ControlExit()
		{
			if (Program.Settings.CloseMinimizesToTray)
			{
				MinimizeToTray();
			}
			else
			{
				Close();
			}
		}

		public void ShowNews()
		{
			ShowNews(always: true);
		}

		public void ShowPortableDevices(DeviceSyncSettings dss = null, Guid? guid = null)
		{
			DevicesEditDialog.Show(Form.ActiveForm ?? this, Program.Settings.Devices, dss, guid);
		}

		public void MenuSynchronizeDevices()
		{
			if (!Program.QueueManager.SynchronizeDevices())
			{
				ShowPortableDevices();
			}
		}

		public void ShowPreferences(string autoInstallplugin = null)
		{
			KeyboardShortcuts keyboardMap = new KeyboardShortcuts(ComicDisplay.KeyboardMap);
			if (PreferencesDialog.Show(Form.ActiveForm ?? this, keyboardMap, ScriptUtility.Scripts, autoInstallplugin))
			{
				ComicDisplay.KeyboardMap = keyboardMap;
			}
		}

		public void ShowOpenDialog()
		{
			string text = Program.ShowComicOpenDialog(Form.ActiveForm ?? this, miOpenComic.Text.Replace("&", ""), includeReadingLists: true);
			if (text != null)
			{
				OpenSupportedFile(text, Program.Settings.OpenInNewTab);
			}
		}

		public bool OpenSupportedFile(string file, bool newSlot = false, int page = 0, bool fromShell = false)
		{
			if (Path.GetExtension(file).Equals(".crplugin", StringComparison.OrdinalIgnoreCase))
			{
				ShowPreferences(file);
				return true;
			}
			if (!Path.GetExtension(file).Equals(".cbl", StringComparison.OrdinalIgnoreCase))
			{
				bool result = books.Open(file, newSlot, Math.Max(0, page - 1));
				if (fromShell && Program.ExtendedSettings.HideBrowserIfShellOpen)
				{
					BrowserVisible = false;
				}
				return result;
			}
			ComicListItem comicListItem = ImportComicList(file);
			if (comicListItem == null)
			{
				return false;
			}
			ComicBook[] array = comicListItem.GetBooks().ToArray();
			if (array.Length == 0)
			{
				return false;
			}
			ComicBook cb = array.Aggregate((ComicBook a, ComicBook b) => (!(a.OpenedTime > b.OpenedTime)) ? b : a);
			return books.Open(cb, newSlot);
		}

		public void ShowAboutDialog()
		{
			using (Splash splash = new Splash())
			{
				splash.Fade = true;
				splash.Location = splash.Bounds.Align(Screen.FromPoint(base.Location).Bounds, ContentAlignment.MiddleCenter).Location;
				splash.ShowDialog(this);
			}
		}

		public void ExportCurrentImage()
		{
			if (ComicDisplay.Book == null || ComicDisplay.Book.Comic == null)
			{
				return;
			}
			using (Image image = ComicDisplay.CreatePageImage())
			{
				if (image != null)
				{
					ExportImage(StringUtility.Format("{0} - {1} {2}", ComicDisplay.Book.Comic.Caption, TR.Default["Page", "Page"], ComicDisplay.Book.CurrentPage + 1), image);
				}
			}
		}

		public bool SyncBrowser()
		{
			if (ComicDisplay == null || ComicDisplay.Book == null || ComicDisplay.Book.Comic == null)
			{
				return false;
			}
			ComicBook comic = ComicDisplay.Book.Comic;
			IComicBrowser comicBrowser = this.FindServices<IComicBrowser>().FirstOrDefault((IComicBrowser b) => b.Library == comic.Container);
			if (comicBrowser == null)
			{
				return false;
			}
			ToggleBrowser(alwaysShow: true, comicBrowser);
			if (comicBrowser.SelectComic(ComicDisplay.Book.Comic))
			{
				return true;
			}
			if (comic.LastOpenedFromListId != Guid.Empty)
			{
				ComicListItem comicListItem = comicBrowser.Library.ComicLists.GetItems<ComicListItem>().FirstOrDefault((ComicListItem li) => li.Id == comic.LastOpenedFromListId);
				if (comicListItem != null && ShowBookInList(comicBrowser.Library, comicListItem, comic))
				{
					return true;
				}
			}
			return false;
		}

		public void ToggleSidebar()
		{
			ISidebar sidebar = this.FindActiveService<ISidebar>();
			if (sidebar != null)
			{
				sidebar.Visible = !sidebar.Visible;
			}
		}

		public void ToggleSmallPreview()
		{
			ISidebar sidebar = this.FindActiveService<ISidebar>();
			if (sidebar != null)
			{
				sidebar.Preview = !sidebar.Preview;
			}
		}

		public void ToggleInfoPanel()
		{
			ISidebar sidebar = this.FindActiveService<ISidebar>();
			if (sidebar != null)
			{
				sidebar.Info = !sidebar.Info;
			}
		}

		public void ToggleSearchBrowser()
		{
			ISearchOptions searchOptions = this.FindActiveService<ISearchOptions>();
			if (searchOptions != null)
			{
				searchOptions.SearchBrowserVisible = !searchOptions.SearchBrowserVisible;
			}
		}

		public void ConvertComic(IEnumerable<ComicBook> books, ExportSetting setting)
		{
			ExportSetting exportSetting = setting ?? ExportComicsDialog.Show(this, Program.ExportComicRackPresets, Program.Settings.ExportUserPresets, Program.Settings.CurrentExportSetting ?? new ExportSetting());
			if (exportSetting == null)
			{
				return;
			}
			bool flag = books.All((ComicBook b) => b.EditMode.IsLocalComic());
			Program.Settings.CurrentExportSetting = exportSetting;
			if (flag && (exportSetting.Target == ExportTarget.ReplaceSource || exportSetting.DeleteOriginal) && !Program.AskQuestion(this, TR.Messages["AskExport", "You have chosen to delete or replace existing files during export. Are you sure you want to continue?\nThe deleted files will be moved to the Recycle Bin during export. Please make sure there is enough disk space available and the eComics are not located on a network drive!"], TR.Messages["Export", "Export"], HiddenMessageBoxes.ConvertComics))
			{
				return;
			}
			exportSetting = CloneUtility.Clone(exportSetting);
			if (!flag)
			{
				exportSetting.DeleteOriginal = false;
				if (exportSetting.Target == ExportTarget.ReplaceSource || exportSetting.Target == ExportTarget.SameAsSource)
				{
					if (exportSetting.Target == ExportTarget.ReplaceSource)
					{
						exportSetting.AddToLibrary = true;
					}
					exportSetting.Target = ExportTarget.NewFolder;
					using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
					{
						folderBrowserDialog.Description = TR.Messages["SelectLocalFolder", "Select a local folder to store the remote Books"];
						folderBrowserDialog.ShowNewFolderButton = true;
						if (folderBrowserDialog.ShowDialog(this) == DialogResult.Cancel || string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
						{
							return;
						}
						exportSetting.TargetFolder = folderBrowserDialog.SelectedPath;
					}
				}
			}
			if (exportSetting.Combine)
			{
				if (exportSetting.Target != ExportTarget.Ask)
				{
					Program.QueueManager.ExportComic(books, exportSetting, 0);
					return;
				}
				ComicBook comicBook = books.FirstOrDefault();
				if (comicBook != null)
				{
					ExportSetting exportSetting2 = FileSaveDialog(comicBook, exportSetting);
					if (exportSetting2 != null)
					{
						Program.QueueManager.ExportComic(books, exportSetting2, 0);
					}
				}
				return;
			}
			int num = 0;
			foreach (ComicBook book in books)
			{
				ExportSetting exportSetting3 = ((exportSetting.Target == ExportTarget.Ask) ? FileSaveDialog(book, exportSetting) : exportSetting);
				if (exportSetting3 == null)
				{
					break;
				}
				Program.QueueManager.ExportComic(book, exportSetting3, num++);
			}
		}

		private ExportSetting FileSaveDialog(ComicBook cb, ExportSetting cs)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				FileFormat fileFormat = cs.GetFileFormat(cb);
				saveFileDialog.Title = TR.Messages["ExportComicTitle", "Export Book to"];
				saveFileDialog.Filter = new FileFormat[1]
				{
					fileFormat
				}.GetDialogFilter(withAllFilter: false);
				saveFileDialog.FileName = cs.GetTargetFileName(cb, 0);
				saveFileDialog.DefaultExt = fileFormat.MainExtension;
				if (saveFileDialog.ShowDialog(this) == DialogResult.Cancel)
				{
					return null;
				}
				ExportSetting exportSetting = CloneUtility.Clone(cs);
				exportSetting.Target = ExportTarget.NewFolder;
				exportSetting.Naming = ExportNaming.Custom;
				exportSetting.CustomNamingStart = 0;
				exportSetting.TargetFolder = Path.GetDirectoryName(saveFileDialog.FileName);
				exportSetting.CustomName = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
				return exportSetting;
			}
		}

		private void ToggleZoom(CommandKey key)
		{
			float num;
			if (ComicDisplay.ImageZoom < 1.05f)
			{
				num = lastZoom;
			}
			else
			{
				lastZoom = ComicDisplay.ImageZoom;
				num = 1f;
			}
			if (key.IsMouseButton())
			{
				ComicDisplay.ZoomTo(Point.Empty, num);
			}
			else
			{
				ComicDisplay.ImageZoom = num;
			}
		}

		private void EditWorkspaceDisplaySettings()
		{
			DisplayWorkspace ws = new DisplayWorkspace();
			StoreWorkspace(ws);
			ComicDisplaySettingsDialog.Show(this, ComicDisplay.IsHardwareRenderer, ws, delegate
			{
				SetWorkspaceDisplayOptions(ws);
			});
		}

		private DisplayWorkspace CreateNewWorkspace()
		{
			DisplayWorkspace displayWorkspace = new DisplayWorkspace();
			StoreWorkspace(displayWorkspace);
			displayWorkspace.Name = lastWorkspaceName ?? TR.Default["Workspace", "Workspace"];
			displayWorkspace.Type = lastWorkspaceType;
			if (SaveWorkspaceDialog.Show(this, displayWorkspace))
			{
				return displayWorkspace;
			}
			return null;
		}

		private void SaveWorkspace()
		{
			DisplayWorkspace newWs = CreateNewWorkspace();
			if (newWs != null)
			{
				lastWorkspaceName = newWs.Name;
				lastWorkspaceType = newWs.Type;
				int num = Program.Settings.Workspaces.FindIndex((DisplayWorkspace ws) => ws.Name == newWs.Name);
				if (num != -1)
				{
					Program.Settings.Workspaces[num] = newWs;
					return;
				}
				Program.Settings.Workspaces.Add(newWs);
				UpdateWorkspaceMenus();
			}
		}

		private void EditWorkspace()
		{
			if (Program.Settings.Workspaces.Count != 0)
			{
				IList<DisplayWorkspace> list = ListEditorDialog.Show(Form.ActiveForm ?? this, TR.Default["Workspaces"], Program.Settings.Workspaces, CreateNewWorkspace, null, (DisplayWorkspace w) =>
                {
                    SetWorkspace(w, remember: true);
                });
				if (list != null)
				{
					Program.Settings.Workspaces.Clear();
					Program.Settings.Workspaces.AddRange(list);
					UpdateWorkspaceMenus();
				}
			}
		}

		private void UpdateWorkspaceMenus()
		{
			UpdateWorkspaceMenus(tsWorkspaces.DropDownItems);
			UpdateWorkspaceMenus(miWorkspaces.DropDownItems);
		}

		private void UpdateWorkspaceMenus(ToolStripItemCollection items)
		{
			ToolStripSeparator toolStripSeparator = null;
			for (int num = items.Count - 1; num > 0; num--)
			{
				if (items[num] is ToolStripSeparator)
				{
					toolStripSeparator = items[num] as ToolStripSeparator;
					break;
				}
				items.RemoveAt(num);
			}
			if (toolStripSeparator != null)
			{
				toolStripSeparator.Visible = Program.Settings.Workspaces.Count > 0;
			}
			int num2 = 0;
			foreach (DisplayWorkspace workspace in Program.Settings.Workspaces)
			{
				DisplayWorkspace itemWs = workspace;
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(FormUtility.FixAmpersand(workspace.Name), null, delegate
				{
					SetWorkspace(CloneUtility.Clone(itemWs), remember: true);
				});
				if (num2 < 6)
				{
					toolStripMenuItem.ShortcutKeys = (Keys)(0x50000 | (112 + num2++));
				}
				items.Add(toolStripMenuItem);
			}
		}

		private void SetWorkspace(DisplayWorkspace workspace, bool remember)
		{
			if (ComicDisplay == null)
			{
				return;
			}
			SuspendLayout();
			bool enableAnimation = SizableContainer.EnableAnimation;
			VisibilityAnimator.EnableAnimation = (SizableContainer.EnableAnimation = false);
			try
			{
				if (remember)
				{
					lastWorkspaceName = workspace.Name;
					lastWorkspaceType = workspace.Type;
				}
				ComicDisplay.FullScreen = false;
				if (workspace.IsWindowLayout)
				{
					if (!workspace.FormBounds.IsEmpty)
					{
						Rectangle b = workspace.FormBounds;
						Screen screen = Screen.AllScreens.Where((Screen scr) => scr.Bounds.IntersectsWith(b)).FirstOrDefault();
						if (screen == null)
						{
							Rectangle bounds = Screen.PrimaryScreen.Bounds;
							b.Width = Math.Min(b.Width, bounds.Width);
							b.Height = Math.Min(b.Height, bounds.Height);
							b = b.Center(bounds);
						}
						base.Bounds = b;
					}
					BrowserVisible = workspace.PanelVisible || (!ComicDisplay.IsValid && Program.Settings.ShowQuickOpen);
					mainViewContainer.DockSize = workspace.PanelSize;
					BrowserDock = workspace.PanelDock;
					ReaderUndocked = workspace.ReaderUndocked;
					UndockedReaderBounds = workspace.UndockedReaderBounds;
					UndockedReaderState = workspace.UndockedReaderState;
					ScriptOutputBounds = workspace.ScriptOutputBounds;
				}
				if (workspace.IsViewsSetup)
				{
					foreach (IDisplayWorkspace item in this.FindServices<IDisplayWorkspace>())
					{
						item.SetWorkspace(workspace);
					}
				}
				if (workspace.IsWindowLayout)
				{
					base.WindowState = workspace.FormState;
					ComicDisplay.FullScreen = workspace.FullScreen;
					MinimalGui = workspace.MinimalGui;
					ComicBookDialog.PagesConfig = workspace.ComicBookDialogPagesConfig;
				}
				SetWorkspaceDisplayOptions(workspace);
			}
			finally
			{
				ResumeLayout();
				VisibilityAnimator.EnableAnimation = (SizableContainer.EnableAnimation = enableAnimation);
			}
		}

		private void SetWorkspaceDisplayOptions(DisplayWorkspace workspace)
		{
			if (workspace.IsComicPageLayout)
			{
				bool displayChangeAnimation = ComicDisplay.DisplayChangeAnimation;
				ComicDisplay.DisplayChangeAnimation = false;
				try
				{
					ComicDisplay.PageLayout = workspace.Layout.PageLayout;
					ComicDisplay.TwoPageNavigation = workspace.Layout.TwoPageAutoScroll;
					ComicDisplay.RightToLeftReading = workspace.RightToLeftReading;
					ComicDisplay.ImageFitMode = workspace.Layout.PageDisplayMode;
					ComicDisplay.ImageFitOnlyIfOversized = workspace.Layout.FitOnlyIfOversized;
					ComicDisplay.ImageRotation = workspace.Layout.PageImageRotation;
					ComicDisplay.ImageAutoRotate = workspace.Layout.AutoRotate;
					try
					{
						ComicDisplay.ImageZoom = workspace.Layout.PageZoom;
					}
					catch
					{
						ComicDisplay.DisplayChangeAnimation = displayChangeAnimation;
					}
				}
				finally
				{
					ComicDisplay.DisplayChangeAnimation = displayChangeAnimation;
				}
			}
			if (workspace.IsComicPageDisplay)
			{
				ComicDisplay.RealisticPages = workspace.DrawRealisticPages;
				ComicDisplay.BackColor = workspace.BackColor;
				ComicDisplay.BackgroundTexture = workspace.BackgroundTexture;
				ComicDisplay.PaperTexture = workspace.PaperTexture;
				ComicDisplay.PaperTextureStrength = workspace.PaperTextureStrength;
				ComicDisplay.ImageBackgroundMode = workspace.PageImageBackgroundMode;
				ComicDisplay.PaperTextureLayout = workspace.PaperTextureLayout;
				ComicDisplay.BackgroundImageLayout = workspace.BackgroundImageLayout;
				ComicDisplay.PageTransitionEffect = workspace.PageTransitionEffect;
				ComicDisplay.PageMargin = workspace.PageMargin;
				ComicDisplay.PageMarginPercentWidth = workspace.PageMarginPercentWidth;
			}
		}

		private void StoreWorkspace(DisplayWorkspace workspace)
		{
			workspace.FormState = base.WindowState;
			workspace.FormBounds = SafeBounds;
			workspace.MinimalGui = MinimalGui;
			workspace.PanelDock = BrowserDock;
			workspace.PanelVisible = BrowserVisible;
			workspace.PanelSize = mainViewContainer.DockSize;
			workspace.RightToLeftReading = ComicDisplay.RightToLeftReading;
			workspace.FullScreen = ComicDisplay.FullScreen;
			workspace.Layout.PageLayout = ComicDisplay.PageLayout;
			workspace.Layout.TwoPageAutoScroll = ComicDisplay.TwoPageNavigation;
			workspace.Layout.FitOnlyIfOversized = ComicDisplay.ImageFitOnlyIfOversized;
			workspace.Layout.PageZoom = ComicDisplay.ImageZoom;
			workspace.Layout.PageDisplayMode = ComicDisplay.ImageFitMode;
			workspace.Layout.PageImageRotation = ComicDisplay.ImageRotation;
			workspace.Layout.AutoRotate = ComicDisplay.ImageAutoRotate;
			workspace.DrawRealisticPages = ComicDisplay.RealisticPages;
			workspace.BackColor = ComicDisplay.BackColor;
			workspace.BackgroundTexture = ComicDisplay.BackgroundTexture;
			workspace.PaperTexture = ComicDisplay.PaperTexture;
			workspace.PaperTextureStrength = ComicDisplay.PaperTextureStrength;
			workspace.PageImageBackgroundMode = ComicDisplay.ImageBackgroundMode;
			workspace.PaperTextureLayout = ComicDisplay.PaperTextureLayout;
			workspace.BackgroundImageLayout = ComicDisplay.BackgroundImageLayout;
			workspace.PageMargin = ComicDisplay.PageMargin;
			workspace.PageMarginPercentWidth = ComicDisplay.PageMarginPercentWidth;
			workspace.PageTransitionEffect = ComicDisplay.PageTransitionEffect;
			workspace.ReaderUndocked = ReaderUndocked;
			if (ScriptConsole != null)
			{
				workspace.ScriptOutputBounds = ScriptOutputBounds;
			}
			if (workspace.ReaderUndocked)
			{
				workspace.UndockedReaderBounds = UndockedReaderBounds;
				workspace.UndockedReaderState = UndockedReaderState;
			}
			foreach (IDisplayWorkspace item in this.FindServices<IDisplayWorkspace>())
			{
				item.StoreWorkspace(workspace);
			}
			workspace.ComicBookDialogPagesConfig = ComicBookDialog.PagesConfig;
			if (workspace.ComicBookDialogPagesConfig != null)
			{
				workspace.ComicBookDialogPagesConfig.GroupsStatus = null;
			}
		}

		public void StoreWorkspace()
		{
			StoreWorkspace(Program.Settings.CurrentWorkspace);
		}

		public void SetListLayout(DisplayListConfig cfg)
		{
			IComicBrowser comicBrowser = this.FindActiveService<IComicBrowser>();
			if (comicBrowser != null)
			{
				comicBrowser.ListConfig = cfg;
			}
		}

		public void SetListLayoutToAll(DisplayListConfig dlc = null)
		{
			if (!Program.AskQuestion(this, TR.Messages["AskSetAllLists", "Are you sure you want to set all Lists to the current layout?"], TR.Messages["Set", "Set"], HiddenMessageBoxes.SetAllListLayouts))
			{
				return;
			}
			if (dlc == null)
			{
				IComicBrowser comicBrowser = this.FindActiveService<IComicBrowser>();
				if (comicBrowser != null)
				{
					dlc = comicBrowser.ListConfig;
				}
			}
			if (dlc != null)
			{
				Program.Database.ResetDisplayConfigs(dlc);
			}
		}

		public void EditListLayout()
		{
			IComicBrowser cb = this.FindActiveService<IComicBrowser>();
			if (cb != null)
			{
				DisplayListConfig cfg = cb.ListConfig;
				if (ListLayoutDialog.Show(this, cfg, cfg.View.ItemViewMode, delegate
				{
					cb.ListConfig = cfg;
				}))
				{
					cb.ListConfig = cfg;
				}
			}
		}

		private ListConfiguration CreateListLayout()
		{
			IComicBrowser comicBrowser = this.FindActiveService<IComicBrowser>();
			if (comicBrowser == null)
			{
				return null;
			}
			string name = SelectItemDialog.GetName(this, TR.Messages["SaveListLayout", "Save List Layout"], TR.Default["Layout", "Layout"], Program.Settings.ListConfigurations);
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			return new ListConfiguration(name)
			{
				Config = comicBrowser.ListConfig
			};
		}

		public void SaveListLayout()
		{
			ListConfiguration cfg = CreateListLayout();
			if (cfg != null)
			{
				int num = Program.Settings.ListConfigurations.FindIndex((ListConfiguration c) => c.Name == cfg.Name);
				if (num != -1)
				{
					Program.Settings.ListConfigurations[num] = cfg;
					return;
				}
				Program.Settings.ListConfigurations.Add(cfg);
				UpdateListConfigMenus();
			}
		}

		public void EditListLayouts()
		{
			if (Program.Settings.ListConfigurations.Count != 0)
			{
				IList<ListConfiguration> list = ListEditorDialog.Show(Form.ActiveForm ?? this, TR.Messages["ListLayouts", "List Layouts"], Program.Settings.ListConfigurations, CreateListLayout, null, (ListConfiguration elc) =>
                {
                    SetListLayout(elc.Config);
                }, (ListConfiguration elc) =>
                {
                    SetListLayoutToAll(elc.Config);
                });
				if (list != null)
				{
					Program.Settings.ListConfigurations.Clear();
					Program.Settings.ListConfigurations.AddRange(list);
					UpdateListConfigMenus();
				}
			}
		}

		public void UpdateListConfigMenus(ToolStripItemCollection items)
		{
			items.RemoveAll((ToolStripItem c) => c.Tag is ListConfiguration);
			ToolStripSeparator toolStripSeparator = items.OfType<ToolStripSeparator>().LastOrDefault();
			if (toolStripSeparator != null)
			{
				toolStripSeparator.Visible = Program.Settings.ListConfigurations.Count > 0;
			}
			int num = 0;
			TR tR = TR.Load(base.Name);
			foreach (ListConfiguration listConfiguration in Program.Settings.ListConfigurations)
			{
				ListConfiguration itemCfg = listConfiguration;
				ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(StringUtility.Format(tR["SetLayoutMenu", "Set '{0}' Layout"], FormUtility.FixAmpersand(listConfiguration.Name)), null, delegate
				{
					SetListLayout(itemCfg.Config);
				});
				toolStripMenuItem.Tag = itemCfg;
				if (num < 6)
				{
					toolStripMenuItem.ShortcutKeys = (Keys)(0x50000 | (117 + num++));
				}
				items.Add(toolStripMenuItem);
			}
		}

		private void UpdateListConfigMenus()
		{
			UpdateListConfigMenus(miListLayouts.DropDownItems);
		}

		private void OnOpenRecent(object sender, EventArgs e)
		{
			string text = ((ToolStripMenuItem)sender).Text;
			int num = Convert.ToInt32(text.Substring(0, 2)) - 1;
			OpenSupportedFile(recentFiles[num], Program.Settings.OpenInNewTab);
		}

		private void RecentFilesMenuOpening(object sender, EventArgs e)
		{
			int num = 0;
			foreach (ToolStripMenuItem dropDownItem in miOpenRecent.DropDownItems)
			{
				if (dropDownItem.Image != null)
				{
					dropDownItem.Image.Dispose();
				}
			}
			FormUtility.SafeToolStripClear(miOpenRecent.DropDownItems);
			string[] array = recentFiles;
			foreach (string text in array)
			{
				if (!File.Exists(text))
				{
					continue;
				}
				string text2 = ++num + " - " + FormUtility.FixAmpersand(FileUtility.GetSafeFileName(text));
				using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.Thumbs.GetImage(Program.BookFactory.Create(text, CreateBookOption.DoNotAdd).GetFrontCoverThumbnailKey()))
				{
					try
					{
						ToolStripMenuItem value = new ToolStripMenuItem(text2, (itemLock != null && itemLock.Item != null) ? itemLock.Item.Bitmap.Resize(16, 16) : null, OnOpenRecent);
						miOpenRecent.DropDownItems.Add(value);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private void ToggleUndockReader()
		{
			if (!ReaderUndocked)
			{
				ComicDisplay.FullScreen = false;
			}
			ReaderUndocked = !ReaderUndocked;
		}

		private void ReaderFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (!shieldReaderFormClosing)
			{
				try
				{
					shieldReaderFormClosing = true;
					ReaderUndocked = false;
				}
				finally
				{
					shieldReaderFormClosing = false;
				}
			}
		}

		private void ReaderFormKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = commands.InvokeKey(e.KeyData);
		}

		private void RebuildBookTabs()
		{
			using (ItemMonitor.Lock(OpenBooks.Slots.SyncRoot))
			{
				mainMenuStrip.SuspendLayout();
				fileTabs.SuspendLayout();
				for (int num = fileTabs.Items.Count - 1; num >= 0; num--)
				{
					if (fileTabs.Items[num].Tag != null)
					{
						fileTabs.Items.RemoveAt(num);
					}
				}
				FormUtility.SafeToolStripClear(miOpenNow.DropDownItems);
				FormUtility.SafeToolStripClear(cmComics.DropDownItems, cmComics.DropDownItems.IndexOf(cmComicsSep) + 1);
				mainView.ClearFileTabs();
				Bitmap thumb = default(Bitmap);
				for (int i = 0; i < OpenBooks.Slots.Count; i++)
				{
					string text = FormUtility.FixAmpersand(OpenBooks.GetSlotCaption(i));
					ComicBookNavigator nav = OpenBooks.Slots[i];
					string text2 = text;
					string text3 = null;
					KeysConverter keysConverter = new KeysConverter();
					ToolStripMenuItem tmi = new ToolStripMenuItem(text);
					tmi.Click += OpenBooks_Clicked;
					tmi.Tag = i;
					if (i < 12)
					{
						tmi.ShortcutKeys = (Keys)(0x60000 | (112 + i));
						text3 = keysConverter.ConvertToString(tmi.ShortcutKeys);
						text2 = text2 + "\r\n(" + text3 + ")";
					}
					miOpenNow.DropDownItems.Add(tmi);
					ToolStripMenuItem tmi2 = new ToolStripMenuItem(text);
					tmi2.Click += OpenBooks_Clicked;
					tmi2.Tag = i;
					tmi2.ShortcutKeys = tmi.ShortcutKeys;
					cmComics.DropDownItems.Add(tmi2);
					TabBar.TabBarItem tbi = new ComicReaderTab(text, nav, Font, text3)
					{
						Tag = i,
						MinimumWidth = 100,
						CanClose = true,
						ToolTipText = text2,
						ContextMenu = tabContextMenu
					};
					if (nav == null)
					{
						tbi.Image = emptyTabImage;
					}
					tbi.Selected += OpenBooks_Selected;
					tbi.CloseClick += btn_CloseClick;
					tbi.CaptionClick += tbi_CaptionClick;
					fileTabs.Items.Add(tbi);
					TabBar.TabBarItem tbi2 = new ComicReaderTab(text, nav, Font, text3)
					{
						Image = emptyTabImage,
						Tag = i,
						MinimumWidth = 100,
						CanClose = true,
						ToolTipText = text2,
						ContextMenu = tabContextMenu,
						Visible = (ViewDock == DockStyle.Fill)
					};
					if (nav == null)
					{
						tbi2.Image = emptyTabImage;
					}
					tbi2.Selected += OpenBooks_Selected;
					tbi2.CloseClick += btn_CloseClick;
					tbi2.CaptionClick += tbi_CaptionClick;
					mainView.AddFileTab(tbi2);
					ThreadUtility.RunInBackground("Create tab thumbnails", delegate
					{
						try
						{
							using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(nav.Comic))
							{
								thumb = itemLock.Item.Bitmap.Resize(16, 16);
							}
							this.Invoke(delegate
							{
								if (!tmi.IsDisposed)
								{
									tmi.Image = thumb;
								}
								if (!tmi2.IsDisposed)
								{
									tmi2.Image = thumb;
								}
								tbi.Image = thumb;
								tbi2.Image = thumb;
							});
						}
						catch
						{
						}
					});
				}
				string text4 = miAddTab.Text.Replace("&", string.Empty);
				TabBar.TabBarItem tabBarItem = new TabBar.TabBarItem(text4)
				{
					Tag = -1,
					Image = addTabImage,
					MinimumWidth = 32,
					ShowText = false,
					ToolTipText = text4,
					AdjustWidth = false
				};
				tabBarItem.Click += delegate
				{
					OpenBooks.AddSlot();
					OpenBooks.CurrentSlot = OpenBooks.Slots.Count - 1;
				};
				fileTabs.Items.Add(tabBarItem);
				tabBarItem = new TabBar.TabBarItem(text4)
				{
					Tag = -1,
					Image = addTabImage,
					MinimumWidth = 32,
					AdjustWidth = false,
					ShowText = false,
					ToolTipText = text4,
					Visible = (ViewDock == DockStyle.Fill)
				};
				tabBarItem.Click += delegate
				{
					OpenBooks.AddSlot();
					OpenBooks.CurrentSlot = OpenBooks.Slots.Count - 1;
				};
				mainView.AddFileTab(tabBarItem);
				fileTabs.ResumeLayout(performLayout: false);
				fileTabs.PerformLayout();
				mainMenuStrip.ResumeLayout(performLayout: false);
				mainMenuStrip.PerformLayout();
				OnGuiVisibilities();
				if (books.OpenCount == 0 && !Program.Settings.ShowQuickOpen)
				{
					BrowserVisible = true;
					mainView.ShowLast();
				}
			}
		}

		private void btn_CloseClick(object sender, EventArgs e)
		{
			OpenBooks.Close((int)((TabBar.TabBarItem)sender).Tag);
		}

		private void tbi_CaptionClick(object sender, CancelEventArgs e)
		{
			TabBar.TabBarItem tabBarItem = sender as TabBar.TabBarItem;
			if (tabBarItem.IsSelected)
			{
				ToggleBrowser();
				e.Cancel = true;
			}
		}

		private void OpenBooks_Clicked(object sender, EventArgs e)
		{
			object obj = ((sender is ToolStripItem) ? ((ToolStripItem)sender).Tag : ((TabBar.TabBarItem)sender).Tag);
			OpenBooks.CurrentSlot = (int)obj;
		}

		private void OpenBooks_Selected(object sender, CancelEventArgs e)
		{
			object obj = ((sender is ToolStripItem) ? ((ToolStripItem)sender).Tag : ((TabBar.TabBarItem)sender).Tag);
			OpenBooks.CurrentSlot = (int)obj;
		}

		private void OpenBooks_SlotsChanged(object sender, SmartListChangedEventArgs<ComicBookNavigator> e)
		{
			RebuildBookTabs();
		}

		private void OpenBooks_CurrentSlotChanged(object sender, EventArgs e)
		{
			foreach (TabBar.TabBarItem item in fileTabs.Items)
			{
				if (item.Tag is int && OpenBooks.CurrentSlot == (int)item.Tag)
				{
					fileTabs.SelectedTab = item;
				}
			}
			foreach (ToolStripMenuItem item2 in from tmi in miOpenNow.DropDownItems.OfType<ToolStripMenuItem>()
				where tmi.Tag is int
				select tmi)
			{
				item2.Checked = OpenBooks.CurrentSlot == (int)item2.Tag;
			}
			foreach (ToolStripMenuItem item3 in from tmi in cmComics.DropDownItems.OfType<ToolStripMenuItem>()
				where tmi.Tag is int
				select tmi)
			{
				item3.Checked = OpenBooks.CurrentSlot == (int)item3.Tag;
			}
			mainView.ShowView(OpenBooks.CurrentSlot);
			UpdateTabCaptions();
			if (OpenBooks.CurrentBook != null)
			{
				Win7.SetActiveThumbnail(OpenBooks.CurrentBook.Comic.FilePath);
			}
		}

		private void OpenBooks_CaptionsChanged(object sender, EventArgs e)
		{
			UpdateTabCaptions();
		}

		private void UpdateTabCaptions()
		{
			using (ItemMonitor.Lock(OpenBooks.Slots.SyncRoot))
			{
				foreach (TabBar.TabBarItem item in fileTabs.Items.Where((TabBar.TabBarItem t) => t.Tag is int && (int)t.Tag >= 0))
				{
					item.Text = OpenBooks.GetSlotCaption((int)item.Tag);
				}
				foreach (TabBar.TabBarItem fileTab in mainView.GetFileTabs())
				{
					fileTab.Text = OpenBooks.GetSlotCaption((int)fileTab.Tag);
				}
				foreach (ToolStripMenuItem dropDownItem in miOpenNow.DropDownItems)
				{
					if (dropDownItem.Tag is int)
					{
						dropDownItem.Text = OpenBooks.GetSlotCaption((int)dropDownItem.Tag);
					}
				}
			}
		}

		private void InitializePluginHelp()
		{
			IEnumerable<PackageManager.Package> enumerable = from p in Program.ScriptPackages.GetPackages()
				where !string.IsNullOrEmpty(p.HelpLink)
				select p;
			miHelpPlugins.Visible = enumerable.Count() > 0;
			foreach (PackageManager.Package p2 in enumerable)
			{
				miHelpPlugins.DropDownItems.Add(p2.Name, p2.Image, delegate
				{
					Program.StartDocument(p2.HelpLink, p2.PackagePath);
				});
			}
		}

		private void InitializeHelp(string helpSystem)
		{
			Program.HelpSystem = helpSystem;
			miWebHelp.DropDownItems.Clear();
			miHelp.Visible = false;
			if (miHelp.DropDownItems.Contains(miWebHelp))
			{
				miHelp.DropDownItems.Remove(miWebHelp);
				helpMenu.DropDownItems.Insert(helpMenu.DropDownItems.IndexOf(miHelp) + 1, miWebHelp);
			}
			miHelp.DropDownItems.Clear();
			ToolStripItem[] array = Program.Help.GetCustomHelpMenu().ToArray();
			if (array.Length != 0)
			{
				helpMenu.DropDownItems.Remove(miWebHelp);
				miHelp.Visible = true;
				miHelp.DropDownItems.Add(miWebHelp);
				miHelp.DropDownItems.Add(new ToolStripSeparator());
				miHelp.DropDownItems.AddRange(array);
			}
			IEnumerable<string> helpSystems = Program.HelpSystems;
			miChooseHelpSystem.Visible = helpSystems.Count() > 1;
			miChooseHelpSystem.DropDownItems.Clear();
			foreach (string item in helpSystems)
			{
				string name = item;
				((ToolStripMenuItem)miChooseHelpSystem.DropDownItems.Add(name, null, delegate
				{
					Program.Settings.HelpSystem = name;
				})).Checked = Program.HelpSystem == name;
			}
		}

		private void ShowPendingTasks(int tab = 0)
		{
			if (taskDialog != null && !taskDialog.IsDisposed)
			{
				taskDialog.SelectedTab = tab;
				taskDialog.Activate();
			}
			else
			{
				taskDialog = TasksDialog.Show(this, Program.QueueManager.GetQueues(), tab);
			}
		}

		private void FocusQuickSearch()
		{
			this.FindActiveService<ISearchOptions>()?.FocusQuickSearch();
		}

		private bool CheckSidebarAvailable()
		{
			return this.FindActiveService<ISidebar>() != null;
		}

		private bool CheckInfoPanelAvailable()
		{
			return this.FindActiveService<ISidebar>()?.HasInfoPanels ?? false;
		}

		private bool CheckSidebarEnabled()
		{
			return this.FindActiveService<ISidebar>()?.Visible ?? false;
		}

		private bool CheckInfoPanelEnabled()
		{
			return this.FindActiveService<ISidebar>()?.Info ?? false;
		}

		private bool CheckSmallPreviewEnabled()
		{
			return this.FindActiveService<ISidebar>()?.Preview ?? false;
		}

		private bool CheckSearchBrowserEnabled()
		{
			return this.FindActiveService<ISearchOptions>()?.SearchBrowserVisible ?? false;
		}

		private bool CheckSearchAvailable()
		{
			return this.FindActiveService<ISearchOptions>() != null;
		}

		private bool CheckViewOptionsAvailable()
		{
			return this.FindActiveService<IComicBrowser>() != null;
		}

		private Image GetFitModeImage()
		{
			try
			{
				int imageFitMode = (int)ComicDisplay.ImageFitMode;
				foreach (ToolStripItem dropDownItem in tbFit.DropDownItems)
				{
					if (dropDownItem.Image != null && imageFitMode-- == 0)
					{
						return dropDownItem.Image;
					}
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		private Image GetLayoutImage()
		{
			switch (ComicDisplay.PageLayout)
			{
			default:
				if (!ComicDisplay.RightToLeftReading)
				{
					return miSinglePage.Image;
				}
				return SinglePageRtl;
			case PageLayoutMode.Double:
				if (!ComicDisplay.RightToLeftReading)
				{
					return miTwoPages.Image;
				}
				return TwoPagesRtl;
			case PageLayoutMode.DoubleAdaptive:
				if (!ComicDisplay.RightToLeftReading)
				{
					return miTwoPagesAdaptive.Image;
				}
				return TwoPagesAdaptiveRtl;
			}
		}

		private void readerContainer_Paint(object sender, PaintEventArgs e)
		{
			try
			{
				if (EngineConfiguration.Default.AeroFullScreenWorkaround)
				{
					e.Graphics.Clear(Color.Black);
				}
			}
			catch (Exception)
			{
			}
		}

		private void tsCurrentPage_Click(object sender, EventArgs e)
		{
			Program.Settings.TrackCurrentPage = !Program.Settings.TrackCurrentPage;
		}

		private void tabContextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool flag = ComicDisplay == null || ComicDisplay.Book == null || ComicDisplay.Book.Comic.EditMode.IsLocalComic();
			ToolStripSeparator toolStripSeparator = sepBeforeRevealInBrowser;
			ToolStripMenuItem toolStripMenuItem = cmRevealInExplorer;
			bool flag3 = (cmCopyPath.Visible = flag);
			bool visible = (toolStripMenuItem.Visible = flag3);
			toolStripSeparator.Visible = visible;
		}

		private void Application_Idle(object sender, EventArgs e)
		{
			OnUpdateGui();
		}

		private void viewer_BookChanged(object sender, EventArgs e)
		{
			if (!Program.ExtendedSettings.DoNotResetZoomOnBookOpen)
			{
				ComicDisplay.ImageZoom = 1f;
			}
			if (ComicDisplay.Book != null)
			{
				ComicDisplay.Focus();
			}
		}

		private void WatchedBookHasChanged(object sender, ContainerBookChangedEventArgs e)
		{
			if (e.IsComicInfo && e.Book.EditMode.IsLocalComic() && e.Book.FileInfoRetrieved)
			{
				e.Book.ComicInfoIsDirty = true;
				if (!books.IsOpen(e.Book))
				{
					Program.QueueManager.AddBookToFileUpdate(e.Book);
				}
			}
		}

		private void MagnifySetupChanged(object sender, EventArgs e)
		{
			MagnifySetupControl magnifySetupControl = (MagnifySetupControl)sender;
			ComicDisplay.MagnifierOpacity = magnifySetupControl.MagnifyOpaque;
			ComicDisplay.MagnifierSize = magnifySetupControl.MagnifySize;
			ComicDisplay.MagnifierZoom = magnifySetupControl.MagnifyZoom;
			ComicDisplay.MagnifierStyle = magnifySetupControl.MagnifyStyle;
			ComicDisplay.AutoHideMagnifier = magnifySetupControl.AutoHideMagnifier;
			ComicDisplay.AutoMagnifier = magnifySetupControl.AutoMagnifier;
		}

		private void viewer_PageDisplayModeChanged(object sender, EventArgs e)
		{
			tbZoom.Text = $"{(int)(ComicDisplay.ImageZoom * 100f)}%";
			tbRotate.Text = TR.Translate(ComicDisplay.ImageRotation);
			tbRotate.Image = (ComicDisplay.ImageAutoRotate ? miAutoRotate.Image : miRotateRight.Image);
		}

		private void viewer_FirstPageReached(object sender, EventArgs e)
		{
			if (Program.Settings.AutoNavigateComics)
			{
				OpenPrevComic();
			}
		}

		private void viewer_LastPageReached(object sender, EventArgs e)
		{
			if (Program.Settings.AutoNavigateComics)
			{
				OpenNextComic(1, OpenComicOptions.NoMoveToLastPage);
			}
		}

		private void backgroundSaveTimer_Tick(object sender, EventArgs e)
		{
			Program.DatabaseManager.SaveInBackground();
		}

		private void tsExportActivity_Click(object sender, EventArgs e)
		{
			if (Program.QueueManager.ExportErrors.Count != 0)
			{
				ShowErrorsDialog.ShowErrors(this, Program.QueueManager.ExportErrors, ShowErrorsDialog.ComicExporterConverter);
			}
			else
			{
				ShowPendingTasks();
			}
		}

		private void tsDeviceSyncActivity_Click(object sender, EventArgs e)
		{
			if (Program.QueueManager.DeviceSyncErrors.Count != 0)
			{
				ShowErrorsDialog.ShowErrors(this, Program.QueueManager.DeviceSyncErrors, ShowErrorsDialog.DeviceSyncErrorConverter);
			}
			else
			{
				ShowPendingTasks();
			}
		}

		private void tsPageActivity_Click(object sender, EventArgs e)
		{
			ShowPendingTasks();
		}

		private void tsReadInfoActivity_Click(object sender, EventArgs e)
		{
			ShowPendingTasks();
		}

		private void tsUpdateInfoActivity_Click(object sender, EventArgs e)
		{
			ShowPendingTasks();
		}

		private void tsScanActivity_Click(object sender, EventArgs e)
		{
			ShowPendingTasks();
		}

		private void tsServerActivity_Click(object sender, EventArgs e)
		{
			ShowPendingTasks(1);
		}

		private void pageContextMenu_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				if (ComicDisplay == null)
				{
					e.Cancel = true;
					return;
				}
				if (ComicDisplay.SupressContextMenu)
				{
					ComicDisplay.SupressContextMenu = false;
					e.Cancel = true;
					return;
				}
				IEditPage pageEditor = GetPageEditor();
				EnumMenuUtility enumMenuUtility = pageTypeContextMenu;
				bool enabled = (pageRotationContextMenu.Enabled = pageEditor.IsValid);
				enumMenuUtility.Enabled = enabled;
				pageTypeContextMenu.Value = (int)pageEditor.PageType;
				pageRotationContextMenu.Value = (int)pageEditor.Rotation;
			}
			catch
			{
				e.Cancel = true;
			}
		}

		private void fileMenu_DropDownOpening(object sender, EventArgs e)
		{
			miUpdateAllComicFiles.Visible = !Program.Settings.AutoUpdateComicsFiles;
		}

		private void editMenu_DropDownOpening(object sender, EventArgs e)
		{
			try
			{
				bool flag = ComicDisplay != null && ComicDisplay.Book != null;
				IEditPage pageEditor = GetPageEditor();
				EnumMenuUtility enumMenuUtility = pageTypeEditMenu;
				bool enabled = (pageRotationEditMenu.Enabled = pageEditor.IsValid);
				enumMenuUtility.Enabled = enabled;
				pageTypeEditMenu.Value = (int)pageEditor.PageType;
				pageRotationEditMenu.Value = (int)pageEditor.Rotation;
				if (miUndo.Tag == null)
				{
					miUndo.Tag = miUndo.Text;
				}
				string undoLabel = Program.Database.Undo.UndoLabel;
				miUndo.Text = (string)miUndo.Tag + (string.IsNullOrEmpty(undoLabel) ? string.Empty : (": " + undoLabel));
				if (miRedo.Tag == null)
				{
					miRedo.Tag = miRedo.Text;
				}
				string text = Program.Database.Undo.RedoEntries.FirstOrDefault();
				miRedo.Text = (string)miRedo.Tag + (string.IsNullOrEmpty(text) ? string.Empty : (": " + text));
			}
			catch (Exception)
			{
			}
		}

		private void mainViewContainer_ExpandedChanged(object sender, EventArgs e)
		{
			OnGuiVisibilities();
			if (base.Visible)
			{
				if (!mainViewContainer.Expanded)
				{
					ComicDisplay.Focus();
				}
				if (mainViewContainer.Expanded && mainViewContainer.Dock == DockStyle.Fill)
				{
					mainView.Focus();
				}
			}
		}

		private void ViewerFullScreenChanged(object sender, EventArgs e)
		{
			if (Program.Settings.AutoMinimalGui)
			{
				MinimalGui = ComicDisplay.FullScreen;
			}
			OnGuiVisibilities();
		}

		private void mainViewContainer_DockChanged(object sender, EventArgs e)
		{
			OnGuiVisibilities();
			if (ReaderUndocked)
			{
				return;
			}
			if (mainViewContainer.Dock == DockStyle.Fill)
			{
				mainViewContainer.BringToFront();
				if (base.Controls.Contains(viewContainer))
				{
					base.Controls.Remove(viewContainer);
					mainView.SetComicViewer(viewContainer);
					mainView.ShowView(books.CurrentSlot);
				}
			}
			else
			{
				if (!base.Controls.Contains(viewContainer))
				{
					mainView.SetComicViewer(null);
					base.Controls.Add(viewContainer);
				}
				viewContainer.Visible = true;
				viewContainer.BringToFront();
			}
		}

		private void OnGuiVisibilities()
		{
			bool flag = !MinimalGui;
			bool flag2 = books.OpenCount > 0;
			miOpenNow.Enabled = miOpenNow.DropDownItems.Count > 0;
			cmComicsSep.Visible = miOpenNow.DropDownItems.Count > 0;
			if (ReaderUndocked)
			{
				fileTabsVisibility.Visible = flag;
				fileTabs.TopPadding = 2;
				mainView.TabBar.TopPadding = 6;
				mainView.TabBar.BottomPadding = 0;
				VisibilityAnimator visibilityAnimator = statusStripVisibility;
				bool visible = (MainToolStripVisible = true);
				visibilityAnimator.Visible = visible;
				enableAutoHideMenu = false;
				mainView.TabBarVisible = true;
				mainMenuStripVisibility.Visible = true;
			}
			else
			{
				bool expanded = mainViewContainer.Expanded;
				if (mainViewContainer.Dock == DockStyle.Fill)
				{
					fileTabsVisibility.Visible = false;
					MainToolStripVisible = false;
					bool flag4 = flag || !mainView.IsComicViewer || (ShowMainMenuNoComicOpen && !flag2);
					VisibilityAnimator visibilityAnimator2 = statusStripVisibility;
					bool visible = (mainView.TabBarVisible = flag4);
					visibilityAnimator2.Visible = visible;
					mainMenuStripVisibility.Visible = flag4 && (!AutoHideMainMenu || (ShowMainMenuNoComicOpen && !flag2));
					enableAutoHideMenu = !mainMenuStripVisibility.Visible && flag;
					mainView.TabBar.TopPadding = (mainMenuStripVisibility.Visible ? 2 : 6);
					mainView.TabBar.BottomPadding = (mainView.IsComicViewer ? 4 : 0);
				}
				else
				{
					flag = flag || expanded;
					MainToolStripVisible = true;
					bool flag6 = flag || (ShowMainMenuNoComicOpen && !flag2);
					VisibilityAnimator visibilityAnimator3 = statusStripVisibility;
					bool visible = (fileTabsVisibility.Visible = flag6);
					visibilityAnimator3.Visible = visible;
					mainMenuStripVisibility.Visible = flag6 && (!AutoHideMainMenu || (ShowMainMenuNoComicOpen && !flag2));
					enableAutoHideMenu = !mainMenuStripVisibility.Visible && flag;
					fileTabs.TopPadding = (mainMenuStripVisibility.Visible ? 2 : 6);
					fileTabs.BottomPadding = 2;
					mainView.TabBarVisible = true;
					mainView.TabBar.TopPadding = ((mainViewContainer.Dock != DockStyle.Bottom) ? fileTabs.TopPadding : 0);
					mainView.TabBar.BottomPadding = ((mainViewContainer.Dock != DockStyle.Bottom) ? fileTabs.BottomPadding : 0);
				}
			}
			fileTabs.PerformLayout();
			mainView.TabBar.PerformLayout();
			if (base.Visible)
			{
				mainViewContainer.Visible = mainViewContainer.Expanded || Program.Settings.AlwaysDisplayBrowserDockingGrip;
			}
		}

		private void TrackBar_Scroll(object sender, EventArgs e)
		{
			this.FindActiveService<IItemSize>()?.SetItemSize(thumbSize.TrackBar.Value);
		}

		private void mainView_TabChanged(object sender, EventArgs e)
		{
			if (!ReaderUndocked && BrowserDock == DockStyle.Fill)
			{
				BrowserVisible = !mainView.IsComicVisible;
			}
			OnGuiVisibilities();
		}

		private void tbBookmarks_DropDownOpening(object sender, EventArgs e)
		{
			UpdateBookmarkMenu(tbBookmarks.DropDownItems, 0);
		}

		private void cmBookmarks_DropDownOpening(object sender, EventArgs e)
		{
			UpdateBookmarkMenu(cmBookmarks.DropDownItems, 0);
		}

		private void tbPrevPage_DropDownOpening(object sender, EventArgs e)
		{
			UpdateBookmarkMenu(tbPrevPage.DropDownItems, -1);
		}

		private void tbNextPage_DropDownOpening(object sender, EventArgs e)
		{
			UpdateBookmarkMenu(tbNextPage.DropDownItems, 1);
		}

		private void miBookmarks_DropDownOpening(object sender, EventArgs e)
		{
			UpdateBookmarkMenu(miBookmarks.DropDownItems, 0);
		}

		private void UpdateBookmarkMenu(ToolStripItemCollection items, int direction)
		{
			for (int num = items.Count - 1; num >= 0; num--)
			{
				if ("bm".Equals(items[num].Tag))
				{
					items.RemoveAt(num);
				}
			}
			ToolStripItem toolStripItem = items.OfType<ToolStripItem>().FirstOrDefault((ToolStripItem ti) => "bms".Equals(ti.Tag));
			int num2 = items.IndexOf(toolStripItem) + 1;
			if (toolStripItem != null)
			{
				toolStripItem.Visible = false;
			}
			if (books.CurrentBook == null)
			{
				return;
			}
			int i = 0;
			int currentPage = books.CurrentBook.CurrentPage;
			var enumerable = from p in books.CurrentBook.Comic.Pages
				select new
				{
					Page = i++,
					Info = p
				} into pi
				where pi.Info.IsBookmark
				select pi;
			if (direction < 0)
			{
				enumerable = enumerable.Reverse();
			}
			try
			{
				foreach (var item in enumerable)
				{
					var cpi = item;
					if ((direction >= 0 || cpi.Page >= currentPage) && (direction <= 0 || cpi.Page <= currentPage) && direction != 0)
					{
						continue;
					}
					ToolStripMenuItem value = new ToolStripMenuItem(string.Format("{0} ({1} {2})", FormUtility.FixAmpersand(cpi.Info.Bookmark), TR.Default["Page", "Page"], cpi.Page + 1), null, delegate
					{
						try
						{
							ComicDisplay.Book.Navigate(cpi.Page, PageSeekOrigin.Beginning, noFilter: true);
						}
						catch
						{
						}
					})
					{
						Tag = "bm",
						Enabled = (cpi.Page != currentPage)
					};
					items.Insert(num2++, value);
					if (toolStripItem != null)
					{
						toolStripItem.Visible = true;
					}
				}
			}
			catch
			{
			}
		}

		private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
		{
			if (notifyIcon.Tag is HiddenMessageBoxes)
			{
				Program.Settings.HiddenMessageBoxes |= (HiddenMessageBoxes)notifyIcon.Tag;
				notifyIcon.Tag = null;
			}
		}

		private void trimTimer_Tick(object sender, EventArgs e)
		{
			Program.ImagePool.Thumbs.MemoryCache.Trim();
			Program.ImagePool.Pages.MemoryCache.Trim();
			int val = ((Program.ExtendedSettings.LimitMemory == 0) ? Settings.UnlimitedSystemMemory : Program.ExtendedSettings.LimitMemory);
			val = Math.Min(val, Program.Settings.MaximumMemoryMB);
			if (val == Settings.UnlimitedSystemMemory)
			{
				return;
			}
			try
			{
				using (Process process = Process.GetCurrentProcess())
				{
                    process.MaxWorkingSet = new IntPtr(Convert.ToInt64(val.Clamp(50, Settings.UnlimitedSystemMemory)) * 1024 * 1024);
				}
			}
			catch
			{
			}
		}

		private void tbTools_DropDownOpening(object sender, EventArgs e)
		{
			tbUpdateWebComics.Visible = Program.Database.Books.FirstOrDefault((ComicBook cb) => cb.IsDynamicSource) != null;
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (AutoHideMainMenu && enableAutoHideMenu && menuDown && e.KeyCode == Keys.Menu && Machine.Ticks - menuAutoClosed > 500)
			{
				mainMenuStripVisibility.Visible = !mainMenuStripVisibility.Visible;
				if (mainMenuStripVisibility.Visible)
				{
					mainMenuStrip.Items[0].Select();
				}
				else
				{
					menuAutoClosed = Machine.Ticks;
				}
			}
			menuDown = false;
			base.OnKeyDown(e);
		}

		private void mainMenuStrip_MenuDeactivate(object sender, EventArgs e)
		{
			if (AutoHideMainMenu && enableAutoHideMenu)
			{
				mainMenuStripVisibility.Visible = false;
				menuAutoClosed = Machine.Ticks;
			}
		}

		private static string TotalPageInformation(ComicBookNavigator nav)
		{
			if (nav == null)
			{
				return NotAvailable;
			}
			if (nav.IsIndexRetrievalCompleted || nav.IndexPagesRetrieved == nav.Comic.PageCount)
			{
				return nav.Comic.PagesAsText;
			}
			return $"{nav.Comic.PagesAsText} ({nav.IndexPagesRetrieved})";
		}

		private void OnUpdateGui()
		{
			UpdateQuickList();
			miOpenRecent.Enabled = recentFiles.Length != 0;
			string text = ((ComicDisplay.Book == null) ? null : ComicDisplay.Book.Caption.Ellipsis(60, "..."));
			tsBook.Text = (string.IsNullOrEmpty(text) ? None : text);
			if (readerForm != null && !MinimizedToTray)
			{
				readerForm.Visible = books.OpenCount > 0;
				readerForm.Text = tsBook.Text;
			}
			if (ComicDisplay.Book == null || string.IsNullOrEmpty(text))
			{
				Text = Application.ProductName;
			}
			else
			{
				Text = Application.ProductName + " - " + (ComicDisplay.Book.Comic.IsInContainer ? text : ComicDisplay.Book.Comic.FileName);
			}
			tsCurrentPage.Text = ((ComicDisplay.Book == null) ? NotAvailable : (ComicDisplay.Book.CurrentPage + 1).ToString());
			tsPageCount.Text = TotalPageInformation(ComicDisplay.Book);
			IComicBrowser comicBrowser = mainView.FindActiveService<IComicBrowser>();
			tsText.Text = FormUtility.FixAmpersand((comicBrowser != null) ? comicBrowser.SelectionInfo : string.Empty);
			tbFit.Image = GetFitModeImage();
			tbPageLayout.Image = GetLayoutImage();
			ToolStripMenuItem toolStripMenuItem = miMagnify;
			ToolStripMenuItem toolStripMenuItem2 = cmMagnify;
			Image image2 = (tbMagnify.Image = (ComicDisplay.MagnifierVisible ? zoomImage : zoomClearImage));
			Image image5 = (toolStripMenuItem.Image = (toolStripMenuItem2.Image = image2));
			ItemSizeInfo itemSizeInfo = this.FindActiveService<IItemSize>()?.GetItemSize();
			thumbSize.Visible = mainViewContainer.Expanded && itemSizeInfo != null;
			if (itemSizeInfo != null)
			{
				thumbSize.SetSlider(itemSizeInfo.Minimum, itemSizeInfo.Maximum, itemSizeInfo.Value);
			}
			ToolStripMenuItem toolStripMenuItem3 = miSynchronizeDevices;
			bool visible = (tsSynchronizeDevices.Visible = Program.Settings.Devices.Count > 0);
			toolStripMenuItem3.Visible = visible;
			ToolStripMenuItem toolStripMenuItem4 = readMenu;
			ToolStripSplitButton toolStripSplitButton = tbPrevPage;
			ToolStripSplitButton toolStripSplitButton2 = tbNextPage;
			ToolStripSeparator toolStripSeparator = toolStripSeparator5;
			ToolStripSplitButton toolStripSplitButton3 = tbPageLayout;
			ToolStripSplitButton toolStripSplitButton4 = tbFit;
			ToolStripSplitButton toolStripSplitButton5 = tbZoom;
			ToolStripSplitButton toolStripSplitButton6 = tbRotate;
			ToolStripSeparator toolStripSeparator2 = toolStripSeparator7;
			bool flag3 = (tbMagnify.Visible = IsComicVisible || ComicDisplay.Book != null);
			bool flag5 = (toolStripSeparator2.Visible = flag3);
			bool flag7 = (toolStripSplitButton6.Visible = flag5);
			bool flag9 = (toolStripSplitButton5.Visible = flag7);
			bool flag11 = (toolStripSplitButton4.Visible = flag9);
			bool flag13 = (toolStripSplitButton3.Visible = flag11);
			bool flag15 = (toolStripSeparator.Visible = flag13);
			bool flag17 = (toolStripSplitButton2.Visible = flag15);
			visible = (toolStripSplitButton.Visible = flag17);
			toolStripMenuItem4.Visible = visible;
		}

		private void UpdateActivityTimerTick(object sender, EventArgs e)
		{
			ToolStripStatusLabel[] array = new ToolStripStatusLabel[5]
			{
				tsReadInfoActivity,
				tsWriteInfoActivity,
				tsScanActivity,
				tsExportActivity,
				tsDeviceSyncActivity
			};
			int num = Numeric.BinaryHash(array.Select((ToolStripStatusLabel l) => l.Visible).ToArray());
			tsScanActivity.Visible = Program.Scanner.IsScanning;
			tsWriteInfoActivity.Visible = Program.QueueManager.IsInComicFileUpdate;
			tsReadInfoActivity.Visible = Program.QueueManager.IsInComicFileRefresh;
			tsPageActivity.Visible = Program.ImagePool.IsWorking;
			bool isInComicConversion = Program.QueueManager.IsInComicConversion;
			int pendingComicConversions = Program.QueueManager.PendingComicConversions;
			int count = Program.QueueManager.ExportErrors.Count;
			tsExportActivity.Visible = isInComicConversion || count > 0;
			if (tsExportActivity.Visible)
			{
				Image image = ((count <= 0) ? exportAnimation : ((pendingComicConversions == 0) ? exportError : exportErrorAnimation));
				tsExportActivity.Image = image;
				string text = StringUtility.Format(ExportingComics, pendingComicConversions);
				if (count > 0)
				{
					text += "\n";
					text += StringUtility.Format(ExportingErrors, count);
				}
				tsExportActivity.ToolTipText = text;
			}
			bool isInDeviceSync = Program.QueueManager.IsInDeviceSync;
			int pendingDeviceSyncs = Program.QueueManager.PendingDeviceSyncs;
			int count2 = Program.QueueManager.DeviceSyncErrors.Count;
			tsDeviceSyncActivity.Visible = isInDeviceSync || count2 > 0;
			if (tsDeviceSyncActivity.Visible)
			{
				Image image2 = ((count2 <= 0) ? deviceSyncAnimation : ((pendingDeviceSyncs == 0) ? deviceSyncError : deviceSyncErrorAnimation));
				tsDeviceSyncActivity.Image = image2;
				string text2 = StringUtility.Format(DeviceSyncing, pendingDeviceSyncs);
				if (count > 0)
				{
					text2 += "\n";
					text2 += StringUtility.Format(DeviceSyncingErrors, count2);
				}
				tsDeviceSyncActivity.ToolTipText = text2;
			}
			Image image3 = null;
			if (comicDisplay != null && comicDisplay.Book != null && !comicDisplay.Book.IsIndexRetrievalCompleted)
			{
				image3 = updatePages;
			}
			tsCurrentPage.Image = (Program.Settings.TrackCurrentPage ? null : trackPagesLockedImage);
			tsPageCount.Image = image3;
			int num2 = Numeric.BinaryHash(array.Select((ToolStripStatusLabel l) => l.Visible).ToArray());
			if (num2 != num)
			{
				int num3 = Numeric.HighestBit(num2);
				if (num3 == -1)
				{
					Win7.SetOverlayIcon(null, null);
				}
				else
				{
					Win7.SetOverlayIcon(array[num3].Image as Bitmap, null);
				}
			}
			tsServerActivity.Visible = Program.NetworkManager.HasActiveServers();
			if (tsServerActivity.Visible)
			{
				tsServerActivity.ToolTipText = string.Format(TR.Messages["ServerActivity", "{0} Server(s) running"], Program.NetworkManager.RunningServers.Count);
				tsServerActivity.Image = (Program.NetworkManager.RecentServerActivity() ? greenLight : grayLight);
			}
			bool flag = Program.Database != null && Program.Database.ComicStorage != null;
			tsDataSourceState.Visible = flag;
			if (flag)
			{
				tsDataSourceState.Image = (Program.Database.ComicStorage.IsConnected ? datasourceConnected : datasourceDisconnected);
				tsDataSourceState.ToolTipText = (Program.Database.ComicStorage.IsConnected ? TR.Messages["DataSourceConnected", "Connected to data source"] : TR.Messages["DataSourceDisconnected", "Disconnected from data source!"]);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (!base.IsHandleCreated || !base.Visible)
			{
				return;
			}
			switch (base.WindowState)
			{
			case FormWindowState.Maximized:
				maximized = true;
				break;
			case FormWindowState.Minimized:
				if (Program.Settings.MinimizeToTray)
				{
					MinimizeToTray();
				}
				else
				{
					maximized = false;
				}
				Program.Collect();
				break;
			case FormWindowState.Normal:
				UpdateSafeBounds();
				maximized = false;
				break;
			}
		}

		protected override void OnMove(EventArgs e)
		{
			base.OnMove(e);
			UpdateSafeBounds();
		}

		private void UpdateSafeBounds()
		{
			if (base.IsHandleCreated && base.WindowState == FormWindowState.Normal && base.FormBorderStyle != 0)
			{
				SafeBounds = base.Bounds;
			}
		}

		private void MinimizeToTray()
		{
			if (shieldTray)
			{
				return;
			}
			shieldTray = true;
			try
			{
				if (readerForm != null)
				{
					readerForm.Visible = false;
				}
				base.Visible = false;
				notifyIcon.Visible = true;
			}
			finally
			{
				shieldTray = false;
			}
		}

		private void RestoreFromTray()
		{
			if (shieldTray)
			{
				return;
			}
			shieldTray = true;
			try
			{
				notifyIcon.Visible = false;
				if (readerForm != null)
				{
					readerForm.Visible = books.OpenCount > 0;
				}
				base.Visible = true;
				base.Bounds = SafeBounds;
				base.WindowState = (Maximized ? FormWindowState.Maximized : FormWindowState.Normal);
			}
			finally
			{
				shieldTray = false;
			}
		}

		public void RestoreToFront()
		{
			if (MinimizedToTray)
			{
				RestoreFromTray();
			}
			else if (base.WindowState == FormWindowState.Minimized)
			{
				base.WindowState = FormWindowState.Normal;
			}
			BringToFront();
			Activate();
		}

		private void NotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
		{
			RestoreFromTray();
		}

		private void StartMouseDisabledTimer()
		{
			ComicDisplay.MouseClickEnabled = false;
			mouseDisableTimer.Stop();
			mouseDisableTimer.Start();
		}

		private void pageContextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			StartMouseDisabledTimer();
		}

		private void showDisableTimer_Tick(object sender, EventArgs e)
		{
			ComicDisplay.MouseClickEnabled = true;
		}

		public void ShowComic()
		{
			if (!ReaderUndocked && mainViewContainer.Dock == DockStyle.Fill)
			{
				mainView.ShowView(books.CurrentSlot);
			}
		}

		public bool ShowBookInList(ComicLibrary library, ComicListItem list, ComicBook cb, bool switchToList = true)
		{
			if (list == null || cb == null)
			{
				return false;
			}
			ILibraryBrowser libraryBrowser = this.FindFirstService<ILibraryBrowser>();
			if (libraryBrowser == null)
			{
				return false;
			}
			if (switchToList)
			{
				mainView.ShowLibrary(library);
			}
			if (!libraryBrowser.SelectList(list.Id))
			{
				return false;
			}
			return this.FindActiveService<IComicBrowser>()?.SelectComic(cb) ?? false;
		}

		void IApplication.Restart()
		{
			MenuRestart();
		}

		void IApplication.ScanFolders()
		{
			StartFullScan();
		}

		void IApplication.SynchronizeDevices()
		{
			Program.QueueManager.SynchronizeDevices();
		}

		public IEnumerable<ComicBook> ReadDatabaseBooks(string file)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public int AskQuestion(string question, string buttonText, string optionText)
		{
			switch (QuestionDialog.AskQuestion(this, question, buttonText, optionText))
			{
			default:
				return 0;
			case QuestionResult.Ok:
				return 1;
			case QuestionResult.OkWithOption:
				return 2;
			}
		}

		public Bitmap GetComicPage(ComicBook cb, int page)
		{
			try
			{
				using (IItemLock<PageImage> itemLock = Program.ImagePool.GetPage(cb.GetPageKey(page, BitmapAdjustment.Empty), cb))
				{
					if (itemLock == null || itemLock.Item == null || itemLock.Item.Bitmap == null)
					{
						return null;
					}
					return itemLock.Item.Bitmap.Clone() as Bitmap;
				}
			}
			catch
			{
				return null;
			}
		}

		public Bitmap GetComicThumbnail(ComicBook cb, int page)
		{
			try
			{
				using (IItemLock<ThumbnailImage> itemLock = Program.ImagePool.GetThumbnail(cb.GetThumbnailKey(page), cb))
				{
					if (itemLock == null || itemLock.Item == null || itemLock.Item.Bitmap == null)
					{
						return null;
					}
					return itemLock.Item.Bitmap.Clone() as Bitmap;
				}
			}
			catch
			{
				return null;
			}
		}

		public Bitmap GetComicPublisherIcon(ComicBook cb)
		{
			Image image = ComicBook.PublisherIcons.GetImage(cb.GetPublisherIconKey()) ?? ComicBook.PublisherIcons.GetImage(cb.Publisher);
			return image.CreateCopy(alwaysTrueCopy: true);
		}

		public Bitmap GetComicImprintIcon(ComicBook cb)
		{
			Image image = ComicBook.PublisherIcons.GetImage(cb.GetImprintIconKey()) ?? ComicBook.PublisherIcons.GetImage(cb.Imprint);
			return image.CreateCopy(alwaysTrueCopy: true);
		}

		public Bitmap GetComicAgeRatingIcon(ComicBook cb)
		{
			return ComicBook.AgeRatingIcons.GetImage(cb.AgeRating).CreateCopy(alwaysTrueCopy: true);
		}

		public Bitmap GetComicFormatIcon(ComicBook cb)
		{
			return ComicBook.FormatIcons.GetImage(cb.Format).CreateCopy(alwaysTrueCopy: true);
		}

		public IDictionary<string, string> GetComicFields()
		{
			return ComicBook.GetTranslatedWritableStringProperties();
		}

		public string ReadInternet(string text)
		{
			return HttpAccess.ReadText(text);
		}

		public IEnumerable<ComicBook> GetLibraryBooks()
		{
			return Program.Database.Books.ToArray();
		}

		public bool RemoveBook(ComicBook cb)
		{
			return Program.Database.Remove(cb);
		}

		public bool SetCustomBookThumbnail(ComicBook cb, Bitmap bmp)
		{
			if (cb.IsLinked)
			{
				return false;
			}
			cb.CustomThumbnailKey = Program.ImagePool.AddCustomThumbnail(bmp);
			return true;
		}

		public ComicBook GetBook(string file)
		{
			return Program.BookFactory.Create(file, CreateBookOption.AddToTemporary);
		}

		public bool OpenNextComic()
		{
			return OpenNextComic(1);
		}

		public bool OpenPrevComic()
		{
			return OpenNextComic(-1);
		}

		public bool OpenRandomComic()
		{
			return OpenNextComic(0);
		}

		public void SelectComics(IEnumerable<ComicBook> books)
		{
			this.FindActiveService<IComicBrowser>()?.SelectComics(books);
		}

		public void ShowComicInfo(IEnumerable<ComicBook> books)
		{
			books = (books ?? Enumerable.Empty<ComicBook>()).Where((ComicBook cb) => cb.EditMode.CanEditProperties());
			if (books.IsEmpty())
			{
				return;
			}
			if (books.Count() > 1)
			{
				Program.Database.Undo.SetMarker(TR.Messages["UndoEditMultipleComics", "Edit multiple Books"]);
				using (MultipleComicBooksDialog multipleComicBooksDialog = new MultipleComicBooksDialog(books))
				{
					multipleComicBooksDialog.ShowDialog(this);
				}
			}
			else
			{
				Program.Database.Undo.SetMarker(TR.Messages["UndoShowInfo", "Show Info"]);
				ComicBookDialog.Show(Form.ActiveForm ?? this, books.FirstOrDefault(), null, null);
			}
		}

		private ComicDisplay CreateComicDisplay()
		{
			ComicDisplayControl pageDisplay = new ComicDisplayControl
			{
				AllowDrop = true,
				Dock = DockStyle.Fill,
				Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Point, 0),
				MagnifierSize = new Size(400, 300),
				Name = "pageDisplay",
				Padding = new Padding(16),
				ShowStatusMessage = true,
				ContextMenuStrip = pageContextMenu,
				AnamorphicTolerance = Program.ExtendedSettings.AnamorphicScalingTolerance,
				AutoHideCursorDelay = Program.ExtendedSettings.AutoHideCursorDuration
			};
			pageDisplay.DragDrop += BookDragDrop;
			pageDisplay.DragEnter += BookDragEnter;
			pageDisplay.BookChanged += viewer_BookChanged;
			pageDisplay.PageDisplayModeChanged += viewer_PageDisplayModeChanged;
			pageDisplay.Resize += delegate
			{
				ScriptUtility.Invoke(PluginEngine.ScriptTypeReaderResized, pageDisplay.Width, pageDisplay.Height);
			};
			pageDisplay.VisibleInfoOverlaysChanged += delegate
			{
				Program.Settings.ShowVisiblePagePartOverlay = pageDisplay.VisibleInfoOverlays.HasFlag(InfoOverlays.PartInfo);
			};
			if (EngineConfiguration.Default.AeroFullScreenWorkaround)
			{
				pageDisplay.SizeChanged += pageDisplay_SizeChanged;
			}
			readerContainer.Controls.Add(pageDisplay);
			readerContainer.Controls.SetChildIndex(pageDisplay, 0);
			readerContainer.Controls.SetChildIndex(quickOpenView, 0);
			ComicDisplay comicDisplay = new ComicDisplay(pageDisplay);
			FormUtility.ServiceTranslation[pageDisplay] = comicDisplay;
			return comicDisplay;
		}

		private void pageDisplay_SizeChanged(object sender, EventArgs e)
		{
			Control control = sender as Control;
			Screen screen = Screen.FromControl(control);
			if (control.Size == screen.Bounds.Size)
			{
				control.Height--;
			}
		}

		private void QuickOpenVisibleChanged(object sender, EventArgs e)
		{
			if (quickOpenView.Visible)
			{
				quickListDirty = true;
			}
		}

		private void QuickOpenBookActivated(object sender, EventArgs e)
		{
			ComicBook selectedBook = quickOpenView.SelectedBook;
			if (selectedBook != null)
			{
				OpenBooks.Open(selectedBook, inNewSlot: false);
			}
		}

		private void QuickOpenBooksChanged(object sender, SmartListChangedEventArgs<ComicBook> e)
		{
			if (e.Action == SmartListAction.Remove)
			{
				quickListDirty = true;
			}
		}

		private void UpdateQuickList()
		{
			quickOpenView.Visible = quickOpenView.Parent.Visible && Program.Settings.ShowQuickOpen && OpenBooks.CurrentBook == null && Program.Database.Books.Count > 0;
			if (!quickOpenView.Visible)
			{
				return;
			}
			if (!quickUpdateRegistered)
			{
				Program.Database.ComicListsChanged += (object s, ComicListItemChangedEventArgs e) =>
                {
                    if (e.Change != ComicListItemChange.Statistic)
                    {
                        quickListDirty = true;
                    }
                };
				Program.Database.Books.Changed += QuickOpenBooksChanged;
				mainView.ViewAdded += delegate
				{
					quickListDirty = true;
				};
				mainView.ViewRemoved += delegate
				{
					quickListDirty = true;
				};
				quickUpdateRegistered = true;
			}
			while (quickListDirty)
			{
				quickListDirty = false;
				FillWithQuickOpenBooks();
			}
		}

		private void FillWithQuickOpenBooks()
		{
			if (this.InvokeIfRequired(FillWithQuickOpenBooks))
			{
				return;
			}
			List<ShareableComicListItem> list = (from cli in Program.Database.ComicLists.GetItems<ShareableComicListItem>()
				where cli.QuickOpen
				select cli.Clone() as ShareableComicListItem).ToList();
			if (list.Count == 0 || !Program.ExtendedSettings.ReplaceDefaultListsInQuickOpen)
			{
				defaultQuickOpenLists = defaultQuickOpenLists ?? new ShareableComicListItem[3]
				{
					ComicLibrary.DefaultReadingList(Program.Database),
					ComicLibrary.DefaultRecentlyReadList(Program.Database),
					ComicLibrary.DefaultRecentlyAddedList(Program.Database)
				};
				list.AddRange(defaultQuickOpenLists);
			}
			quickOpenView.BeginUpdate();
			try
			{
				int num = 0;
				foreach (ShareableComicListItem item in list)
				{
					HashSet<ComicBook> list2 = new HashSet<ComicBook>(ComicBook.GuidEquality);
					using (IEnumerator<ComicLibrary> enumerator2 = mainView.GetLibraries(Program.ExtendedSettings.RemoteLibrariesInQuickOpen, Program.ExtendedSettings.OnlyLocalRemoteLibrariesInQuickOpen).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							ComicLibrary comicLibrary = (item.Library = enumerator2.Current);
							list2.AddRange(item.GetBooks());
						}
					}
					quickOpenView.AddGroup(new GroupInfo(item.Name, num++), list2, Program.ExtendedSettings.QuickOpenListSize);
				}
			}
			finally
			{
				quickOpenView.EndUpdate();
			}
		}

		private void quickOpenView_ShowBrowser(object sender, EventArgs e)
		{
			ToggleBrowser();
		}

		private void quickOpenView_OpenFile(object sender, EventArgs e)
		{
			ShowOpenDialog();
		}

		//Decompile Error
		//string IApplication.get_ProductVersion()
		//{
		//	return base.ProductVersion;
		//}
	}
}
