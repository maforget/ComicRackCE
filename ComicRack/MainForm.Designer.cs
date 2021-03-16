using cYo.Common.ComponentModel;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Viewer.Views;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer
{
    public partial class MainForm
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IdleProcess.Idle -= Application_Idle;
				Program.Database.BookChanged -= WatchedBookHasChanged;
				Program.BookFactory.TemporaryBookChanged -= WatchedBookHasChanged;
				books.BookOpened -= OnBookOpened;
				books.Slots.Changed -= OpenBooks_SlotsChanged;
				books.Slots.ForEach(delegate(ComicBookNavigator n)
				{
					n.SafeDispose();
				});
				books.Slots.Clear();
				Program.Settings.SettingsChanged -= SettingsChanged;
				ToolStripStatusLabel toolStripStatusLabel = tsScanActivity;
				ToolStripStatusLabel toolStripStatusLabel2 = tsReadInfoActivity;
				ToolStripStatusLabel toolStripStatusLabel3 = tsWriteInfoActivity;
				ToolStripStatusLabel toolStripStatusLabel4 = tsPageActivity;
				bool flag2 = (tsExportActivity.Visible = false);
				bool flag4 = (toolStripStatusLabel4.Visible = flag2);
				bool flag6 = (toolStripStatusLabel3.Visible = flag4);
				bool visible = (toolStripStatusLabel2.Visible = flag6);
				toolStripStatusLabel.Visible = visible;
				if (comicDisplay != null)
				{
					comicDisplay.Dispose();
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			mouseDisableTimer = new System.Windows.Forms.Timer(components);
			mainMenuStrip = new System.Windows.Forms.MenuStrip();
			fileMenu = new System.Windows.Forms.ToolStripMenuItem();
			miOpenComic = new System.Windows.Forms.ToolStripMenuItem();
			miCloseComic = new System.Windows.Forms.ToolStripMenuItem();
			miCloseAllComics = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			miAddTab = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			miAddFolderToLibrary = new System.Windows.Forms.ToolStripMenuItem();
			miScan = new System.Windows.Forms.ToolStripMenuItem();
			miUpdateAllComicFiles = new System.Windows.Forms.ToolStripMenuItem();
			miUpdateWebComics = new System.Windows.Forms.ToolStripMenuItem();
			miSynchronizeDevices = new System.Windows.Forms.ToolStripMenuItem();
			miTasks = new System.Windows.Forms.ToolStripMenuItem();
			miFileAutomation = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem57 = new System.Windows.Forms.ToolStripSeparator();
			miNewComic = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem42 = new System.Windows.Forms.ToolStripSeparator();
			miOpenRemoteLibrary = new System.Windows.Forms.ToolStripMenuItem();
			toolStripInsertSeperator = new System.Windows.Forms.ToolStripSeparator();
			miOpenNow = new System.Windows.Forms.ToolStripMenuItem();
			miOpenRecent = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			miRestart = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
			miExit = new System.Windows.Forms.ToolStripMenuItem();
			editMenu = new System.Windows.Forms.ToolStripMenuItem();
			miShowInfo = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem43 = new System.Windows.Forms.ToolStripSeparator();
			miUndo = new System.Windows.Forms.ToolStripMenuItem();
			miRedo = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
			miRating = new System.Windows.Forms.ToolStripMenuItem();
			contextRating = new System.Windows.Forms.ContextMenuStrip(components);
			miRate0 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			miRate1 = new System.Windows.Forms.ToolStripMenuItem();
			miRate2 = new System.Windows.Forms.ToolStripMenuItem();
			miRate3 = new System.Windows.Forms.ToolStripMenuItem();
			miRate4 = new System.Windows.Forms.ToolStripMenuItem();
			miRate5 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem58 = new System.Windows.Forms.ToolStripSeparator();
			miQuickRating = new System.Windows.Forms.ToolStripMenuItem();
			miPageType = new System.Windows.Forms.ToolStripMenuItem();
			miPageRotate = new System.Windows.Forms.ToolStripMenuItem();
			miBookmarks = new System.Windows.Forms.ToolStripMenuItem();
			miSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
			miRemoveBookmark = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
			miPrevBookmark = new System.Windows.Forms.ToolStripMenuItem();
			miNextBookmark = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			miLastPageRead = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem37 = new System.Windows.Forms.ToolStripSeparator();
			toolStripMenuItem40 = new System.Windows.Forms.ToolStripSeparator();
			miCopyPage = new System.Windows.Forms.ToolStripMenuItem();
			miExportPage = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem39 = new System.Windows.Forms.ToolStripSeparator();
			miViewRefresh = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			miDevices = new System.Windows.Forms.ToolStripMenuItem();
			miPreferences = new System.Windows.Forms.ToolStripMenuItem();
			browseMenu = new System.Windows.Forms.ToolStripMenuItem();
			miToggleBrowser = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			miViewLibrary = new System.Windows.Forms.ToolStripMenuItem();
			miViewFolders = new System.Windows.Forms.ToolStripMenuItem();
			miViewPages = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			miSidebar = new System.Windows.Forms.ToolStripMenuItem();
			miSmallPreview = new System.Windows.Forms.ToolStripMenuItem();
			miSearchBrowser = new System.Windows.Forms.ToolStripMenuItem();
			miInfoPanel = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem56 = new System.Windows.Forms.ToolStripSeparator();
			miPreviousList = new System.Windows.Forms.ToolStripMenuItem();
			miNextList = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			miWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			miSaveWorkspace = new System.Windows.Forms.ToolStripMenuItem();
			miEditWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			miWorkspaceSep = new System.Windows.Forms.ToolStripSeparator();
			miListLayouts = new System.Windows.Forms.ToolStripMenuItem();
			miEditListLayout = new System.Windows.Forms.ToolStripMenuItem();
			miSaveListLayout = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			miEditLayouts = new System.Windows.Forms.ToolStripMenuItem();
			miSetAllListsSame = new System.Windows.Forms.ToolStripMenuItem();
			miLayoutSep = new System.Windows.Forms.ToolStripSeparator();
			readMenu = new System.Windows.Forms.ToolStripMenuItem();
			miFirstPage = new System.Windows.Forms.ToolStripMenuItem();
			miPrevPage = new System.Windows.Forms.ToolStripMenuItem();
			miNextPage = new System.Windows.Forms.ToolStripMenuItem();
			miLastPage = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
			miPrevFromList = new System.Windows.Forms.ToolStripMenuItem();
			miNextFromList = new System.Windows.Forms.ToolStripMenuItem();
			miRandomFromList = new System.Windows.Forms.ToolStripMenuItem();
			miSyncBrowser = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
			miPrevTab = new System.Windows.Forms.ToolStripMenuItem();
			miNextTab = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			miAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			miDoublePageAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			miTrackCurrentPage = new System.Windows.Forms.ToolStripMenuItem();
			displayMenu = new System.Windows.Forms.ToolStripMenuItem();
			miComicDisplaySettings = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			miPageLayout = new System.Windows.Forms.ToolStripMenuItem();
			miOriginal = new System.Windows.Forms.ToolStripMenuItem();
			miFitAll = new System.Windows.Forms.ToolStripMenuItem();
			miFitWidth = new System.Windows.Forms.ToolStripMenuItem();
			miFitWidthAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			miFitHeight = new System.Windows.Forms.ToolStripMenuItem();
			miBestFit = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
			miSinglePage = new System.Windows.Forms.ToolStripMenuItem();
			miTwoPages = new System.Windows.Forms.ToolStripMenuItem();
			miTwoPagesAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			miRightToLeft = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem44 = new System.Windows.Forms.ToolStripSeparator();
			miOnlyFitOversized = new System.Windows.Forms.ToolStripMenuItem();
			miZoom = new System.Windows.Forms.ToolStripMenuItem();
			miZoomIn = new System.Windows.Forms.ToolStripMenuItem();
			miZoomOut = new System.Windows.Forms.ToolStripMenuItem();
			miToggleZoom = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			miZoom100 = new System.Windows.Forms.ToolStripMenuItem();
			miZoom125 = new System.Windows.Forms.ToolStripMenuItem();
			miZoom150 = new System.Windows.Forms.ToolStripMenuItem();
			miZoom200 = new System.Windows.Forms.ToolStripMenuItem();
			miZoom400 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			miZoomCustom = new System.Windows.Forms.ToolStripMenuItem();
			miRotation = new System.Windows.Forms.ToolStripMenuItem();
			miRotateLeft = new System.Windows.Forms.ToolStripMenuItem();
			miRotateRight = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem33 = new System.Windows.Forms.ToolStripSeparator();
			miRotate0 = new System.Windows.Forms.ToolStripMenuItem();
			miRotate90 = new System.Windows.Forms.ToolStripMenuItem();
			miRotate180 = new System.Windows.Forms.ToolStripMenuItem();
			miRotate270 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem36 = new System.Windows.Forms.ToolStripSeparator();
			miAutoRotate = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			miMinimalGui = new System.Windows.Forms.ToolStripMenuItem();
			miFullScreen = new System.Windows.Forms.ToolStripMenuItem();
			miReaderUndocked = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem41 = new System.Windows.Forms.ToolStripSeparator();
			miMagnify = new System.Windows.Forms.ToolStripMenuItem();
			helpMenu = new System.Windows.Forms.ToolStripMenuItem();
			miHelp = new System.Windows.Forms.ToolStripMenuItem();
			miWebHelp = new System.Windows.Forms.ToolStripMenuItem();
			miHelpPlugins = new System.Windows.Forms.ToolStripMenuItem();
			miChooseHelpSystem = new System.Windows.Forms.ToolStripMenuItem();
			miHelpQuickIntro = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			miWebHome = new System.Windows.Forms.ToolStripMenuItem();
			miWebUserForum = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			miNews = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
			miAbout = new System.Windows.Forms.ToolStripMenuItem();
			statusStrip = new System.Windows.Forms.StatusStrip();
			tsText = new System.Windows.Forms.ToolStripStatusLabel();
			tsDeviceSyncActivity = new System.Windows.Forms.ToolStripStatusLabel();
			tsExportActivity = new System.Windows.Forms.ToolStripStatusLabel();
			tsReadInfoActivity = new System.Windows.Forms.ToolStripStatusLabel();
			tsWriteInfoActivity = new System.Windows.Forms.ToolStripStatusLabel();
			tsPageActivity = new System.Windows.Forms.ToolStripStatusLabel();
			tsScanActivity = new System.Windows.Forms.ToolStripStatusLabel();
			tsDataSourceState = new System.Windows.Forms.ToolStripStatusLabel();
			tsBook = new System.Windows.Forms.ToolStripStatusLabel();
			tsCurrentPage = new System.Windows.Forms.ToolStripStatusLabel();
			tsPageCount = new System.Windows.Forms.ToolStripStatusLabel();
			tsServerActivity = new System.Windows.Forms.ToolStripStatusLabel();
			pageContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			cmShowInfo = new System.Windows.Forms.ToolStripMenuItem();
			cmRating = new System.Windows.Forms.ToolStripMenuItem();
			contextRating2 = new System.Windows.Forms.ContextMenuStrip(components);
			cmRate0 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			cmRate1 = new System.Windows.Forms.ToolStripMenuItem();
			cmRate2 = new System.Windows.Forms.ToolStripMenuItem();
			cmRate3 = new System.Windows.Forms.ToolStripMenuItem();
			cmRate4 = new System.Windows.Forms.ToolStripMenuItem();
			cmRate5 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			cmQuickRating = new System.Windows.Forms.ToolStripMenuItem();
			cmPageType = new System.Windows.Forms.ToolStripMenuItem();
			cmPageRotate = new System.Windows.Forms.ToolStripMenuItem();
			cmBookmarks = new System.Windows.Forms.ToolStripMenuItem();
			cmSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
			cmRemoveBookmark = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem32 = new System.Windows.Forms.ToolStripSeparator();
			cmPrevBookmark = new System.Windows.Forms.ToolStripMenuItem();
			cmNextBookmark = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			cmLastPageRead = new System.Windows.Forms.ToolStripMenuItem();
			cmBookmarkSeparator = new System.Windows.Forms.ToolStripSeparator();
			toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			cmComics = new System.Windows.Forms.ToolStripMenuItem();
			cmOpenComic = new System.Windows.Forms.ToolStripMenuItem();
			cmCloseComic = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			cmPrevFromList = new System.Windows.Forms.ToolStripMenuItem();
			cmNextFromList = new System.Windows.Forms.ToolStripMenuItem();
			cmRandomFromList = new System.Windows.Forms.ToolStripMenuItem();
			cmComicsSep = new System.Windows.Forms.ToolStripSeparator();
			cmPageLayout = new System.Windows.Forms.ToolStripMenuItem();
			cmOriginal = new System.Windows.Forms.ToolStripMenuItem();
			cmFitAll = new System.Windows.Forms.ToolStripMenuItem();
			cmFitWidth = new System.Windows.Forms.ToolStripMenuItem();
			cmFitWidthAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			cmFitHeight = new System.Windows.Forms.ToolStripMenuItem();
			cmFitBest = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem29 = new System.Windows.Forms.ToolStripSeparator();
			cmSinglePage = new System.Windows.Forms.ToolStripMenuItem();
			cmTwoPages = new System.Windows.Forms.ToolStripMenuItem();
			cmTwoPagesAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			cmRightToLeft = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem38 = new System.Windows.Forms.ToolStripSeparator();
			cmRotate0 = new System.Windows.Forms.ToolStripMenuItem();
			cmRotate90 = new System.Windows.Forms.ToolStripMenuItem();
			cmRotate180 = new System.Windows.Forms.ToolStripMenuItem();
			cmRotate270 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem55 = new System.Windows.Forms.ToolStripSeparator();
			cmOnlyFitOversized = new System.Windows.Forms.ToolStripMenuItem();
			cmMagnify = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			cmCopyPage = new System.Windows.Forms.ToolStripMenuItem();
			cmExportPage = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			cmRefreshPage = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem46 = new System.Windows.Forms.ToolStripSeparator();
			cmMinimalGui = new System.Windows.Forms.ToolStripMenuItem();
			notifyIcon = new System.Windows.Forms.NotifyIcon(components);
			notfifyContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			cmNotifyRestore = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			cmNotifyExit = new System.Windows.Forms.ToolStripMenuItem();
			viewContainer = new System.Windows.Forms.Panel();
			panelReader = new System.Windows.Forms.Panel();
			readerContainer = new System.Windows.Forms.Panel();
			quickOpenView = new cYo.Projects.ComicRack.Viewer.Views.QuickOpenView();
			fileTabs = new cYo.Common.Windows.Forms.TabBar();
			mainToolStrip = new System.Windows.Forms.ToolStrip();
			tbPrevPage = new System.Windows.Forms.ToolStripSplitButton();
			tbFirstPage = new System.Windows.Forms.ToolStripMenuItem();
			tbPrevBookmark = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem53 = new System.Windows.Forms.ToolStripSeparator();
			toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
			tbPrevFromList = new System.Windows.Forms.ToolStripMenuItem();
			tbNextPage = new System.Windows.Forms.ToolStripSplitButton();
			tbLastPage = new System.Windows.Forms.ToolStripMenuItem();
			tbNextBookmark = new System.Windows.Forms.ToolStripMenuItem();
			tbLastPageRead = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem28 = new System.Windows.Forms.ToolStripSeparator();
			toolStripMenuItem49 = new System.Windows.Forms.ToolStripSeparator();
			tbNextFromList = new System.Windows.Forms.ToolStripMenuItem();
			tbRandomFromList = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			tbPageLayout = new System.Windows.Forms.ToolStripSplitButton();
			tbSinglePage = new System.Windows.Forms.ToolStripMenuItem();
			tbTwoPages = new System.Windows.Forms.ToolStripMenuItem();
			tbTwoPagesAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem54 = new System.Windows.Forms.ToolStripSeparator();
			tbRightToLeft = new System.Windows.Forms.ToolStripMenuItem();
			tbFit = new System.Windows.Forms.ToolStripSplitButton();
			tbOriginal = new System.Windows.Forms.ToolStripMenuItem();
			tbFitAll = new System.Windows.Forms.ToolStripMenuItem();
			tbFitWidth = new System.Windows.Forms.ToolStripMenuItem();
			tbFitWidthAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			tbFitHeight = new System.Windows.Forms.ToolStripMenuItem();
			tbBestFit = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
			tbOnlyFitOversized = new System.Windows.Forms.ToolStripMenuItem();
			tbZoom = new System.Windows.Forms.ToolStripSplitButton();
			tbZoomIn = new System.Windows.Forms.ToolStripMenuItem();
			tbZoomOut = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem30 = new System.Windows.Forms.ToolStripSeparator();
			tbZoom100 = new System.Windows.Forms.ToolStripMenuItem();
			tbZoom125 = new System.Windows.Forms.ToolStripMenuItem();
			tbZoom150 = new System.Windows.Forms.ToolStripMenuItem();
			tbZoom200 = new System.Windows.Forms.ToolStripMenuItem();
			tbZoom400 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem31 = new System.Windows.Forms.ToolStripSeparator();
			tbZoomCustom = new System.Windows.Forms.ToolStripMenuItem();
			tbRotate = new System.Windows.Forms.ToolStripSplitButton();
			tbRotateLeft = new System.Windows.Forms.ToolStripMenuItem();
			tbRotateRight = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			tbRotate0 = new System.Windows.Forms.ToolStripMenuItem();
			tbRotate90 = new System.Windows.Forms.ToolStripMenuItem();
			tbRotate180 = new System.Windows.Forms.ToolStripMenuItem();
			tbRotate270 = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem34 = new System.Windows.Forms.ToolStripSeparator();
			tbAutoRotate = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			tbMagnify = new System.Windows.Forms.ToolStripSplitButton();
			tbFullScreen = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			tbTools = new System.Windows.Forms.ToolStripDropDownButton();
			toolsContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			tbOpenComic = new System.Windows.Forms.ToolStripMenuItem();
			tbOpenRemoteLibrary = new System.Windows.Forms.ToolStripMenuItem();
			tbShowInfo = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem47 = new System.Windows.Forms.ToolStripSeparator();
			tsWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			tsSaveWorkspace = new System.Windows.Forms.ToolStripMenuItem();
			tsEditWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			tsWorkspaceSep = new System.Windows.Forms.ToolStripSeparator();
			tbBookmarks = new System.Windows.Forms.ToolStripMenuItem();
			tbSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
			tbRemoveBookmark = new System.Windows.Forms.ToolStripMenuItem();
			tbBookmarkSeparator = new System.Windows.Forms.ToolStripSeparator();
			tbAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem45 = new System.Windows.Forms.ToolStripSeparator();
			tbMinimalGui = new System.Windows.Forms.ToolStripMenuItem();
			tbReaderUndocked = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem52 = new System.Windows.Forms.ToolStripSeparator();
			tbScan = new System.Windows.Forms.ToolStripMenuItem();
			tbUpdateAllComicFiles = new System.Windows.Forms.ToolStripMenuItem();
			tbUpdateWebComics = new System.Windows.Forms.ToolStripMenuItem();
			tsSynchronizeDevices = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem48 = new System.Windows.Forms.ToolStripSeparator();
			tbComicDisplaySettings = new System.Windows.Forms.ToolStripMenuItem();
			tbPreferences = new System.Windows.Forms.ToolStripMenuItem();
			tbAbout = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem50 = new System.Windows.Forms.ToolStripSeparator();
			tbShowMainMenu = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem51 = new System.Windows.Forms.ToolStripSeparator();
			tbExit = new System.Windows.Forms.ToolStripMenuItem();
			tabContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			cmClose = new System.Windows.Forms.ToolStripMenuItem();
			cmCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
			cmCloseAllToTheRight = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem35 = new System.Windows.Forms.ToolStripSeparator();
			cmSyncBrowser = new System.Windows.Forms.ToolStripMenuItem();
			sepBeforeRevealInBrowser = new System.Windows.Forms.ToolStripSeparator();
			cmRevealInExplorer = new System.Windows.Forms.ToolStripMenuItem();
			cmCopyPath = new System.Windows.Forms.ToolStripMenuItem();
			trimTimer = new System.Windows.Forms.Timer(components);
			mainViewContainer = new cYo.Common.Windows.Forms.SizableContainer();
			mainView = new cYo.Projects.ComicRack.Viewer.Views.MainView();
			updateActivityTimer = new System.Windows.Forms.Timer(components);
			mainMenuStrip.SuspendLayout();
			contextRating.SuspendLayout();
			statusStrip.SuspendLayout();
			pageContextMenu.SuspendLayout();
			contextRating2.SuspendLayout();
			notfifyContextMenu.SuspendLayout();
			viewContainer.SuspendLayout();
			panelReader.SuspendLayout();
			readerContainer.SuspendLayout();
			fileTabs.SuspendLayout();
			mainToolStrip.SuspendLayout();
			toolsContextMenu.SuspendLayout();
			tabContextMenu.SuspendLayout();
			mainViewContainer.SuspendLayout();
			SuspendLayout();
			mouseDisableTimer.Interval = 500;
			mouseDisableTimer.Tick += new System.EventHandler(showDisableTimer_Tick);
			mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[6]
			{
		fileMenu,
		editMenu,
		browseMenu,
		readMenu,
		displayMenu,
		helpMenu
			});
			mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			mainMenuStrip.Name = "mainMenuStrip";
			mainMenuStrip.Size = new System.Drawing.Size(744, 24);
			mainMenuStrip.TabIndex = 0;
			mainMenuStrip.MenuDeactivate += new System.EventHandler(mainMenuStrip_MenuDeactivate);
			fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[24]
			{
		miOpenComic,
		miCloseComic,
		miCloseAllComics,
		toolStripMenuItem7,
		miAddTab,
		toolStripMenuItem14,
		miAddFolderToLibrary,
		miScan,
		miUpdateAllComicFiles,
		miUpdateWebComics,
		miSynchronizeDevices,
		miTasks,
		miFileAutomation,
		toolStripMenuItem57,
		miNewComic,
		toolStripMenuItem42,
		miOpenRemoteLibrary,
		toolStripInsertSeperator,
		miOpenNow,
		miOpenRecent,
		toolStripMenuItem4,
		miRestart,
		toolStripMenuItem24,
		miExit
			});
			fileMenu.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			fileMenu.Name = "fileMenu";
			fileMenu.Size = new System.Drawing.Size(37, 20);
			fileMenu.Text = "&File";
			fileMenu.DropDownOpening += new System.EventHandler(fileMenu_DropDownOpening);
			miOpenComic.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			miOpenComic.Name = "miOpenComic";
			miOpenComic.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control;
			miOpenComic.Size = new System.Drawing.Size(296, 38);
			miOpenComic.Text = "&Open File...";
			miCloseComic.Name = "miCloseComic";
			miCloseComic.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
			miCloseComic.Size = new System.Drawing.Size(296, 38);
			miCloseComic.Text = "&Close";
			miCloseAllComics.Name = "miCloseAllComics";
			miCloseAllComics.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miCloseAllComics.Size = new System.Drawing.Size(296, 38);
			miCloseAllComics.Text = "Close A&ll";
			toolStripMenuItem7.Name = "toolStripMenuItem7";
			toolStripMenuItem7.Size = new System.Drawing.Size(293, 6);
			miAddTab.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewTab;
			miAddTab.Name = "miAddTab";
			miAddTab.ShortcutKeys = System.Windows.Forms.Keys.T | System.Windows.Forms.Keys.Control;
			miAddTab.Size = new System.Drawing.Size(296, 38);
			miAddTab.Text = "New &Tab";
			toolStripMenuItem14.Name = "toolStripMenuItem14";
			toolStripMenuItem14.Size = new System.Drawing.Size(293, 6);
			miAddFolderToLibrary.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.AddFolder;
			miAddFolderToLibrary.Name = "miAddFolderToLibrary";
			miAddFolderToLibrary.ShortcutKeys = System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miAddFolderToLibrary.Size = new System.Drawing.Size(296, 38);
			miAddFolderToLibrary.Text = "&Add Folder to Library...";
			miScan.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Scan;
			miScan.Name = "miScan";
			miScan.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miScan.Size = new System.Drawing.Size(296, 38);
			miScan.Text = "Scan Book &Folders";
			miUpdateAllComicFiles.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateSmall;
			miUpdateAllComicFiles.Name = "miUpdateAllComicFiles";
			miUpdateAllComicFiles.ShortcutKeys = System.Windows.Forms.Keys.U | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miUpdateAllComicFiles.Size = new System.Drawing.Size(296, 38);
			miUpdateAllComicFiles.Text = "Update all Book Files";
			miUpdateWebComics.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateWeb;
			miUpdateWebComics.Name = "miUpdateWebComics";
			miUpdateWebComics.ShortcutKeys = System.Windows.Forms.Keys.W | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miUpdateWebComics.Size = new System.Drawing.Size(296, 38);
			miUpdateWebComics.Text = "Update Web Comics";
			miSynchronizeDevices.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DeviceSync;
			miSynchronizeDevices.Name = "miSynchronizeDevices";
			miSynchronizeDevices.Size = new System.Drawing.Size(296, 38);
			miSynchronizeDevices.Text = "Synchronize Devices";
			miTasks.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.BackgroundJob;
			miTasks.Name = "miTasks";
			miTasks.ShortcutKeys = System.Windows.Forms.Keys.T | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miTasks.Size = new System.Drawing.Size(296, 38);
			miTasks.Text = "&Tasks...";
			miFileAutomation.Name = "miFileAutomation";
			miFileAutomation.Size = new System.Drawing.Size(296, 38);
			miFileAutomation.Text = "A&utomation";
			toolStripMenuItem57.Name = "toolStripMenuItem57";
			toolStripMenuItem57.Size = new System.Drawing.Size(293, 6);
			miNewComic.Name = "miNewComic";
			miNewComic.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miNewComic.Size = new System.Drawing.Size(296, 38);
			miNewComic.Text = "&New fileless Book Entry...";
			toolStripMenuItem42.Name = "toolStripMenuItem42";
			toolStripMenuItem42.Size = new System.Drawing.Size(293, 6);
			miOpenRemoteLibrary.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoteDatabase;
			miOpenRemoteLibrary.Name = "miOpenRemoteLibrary";
			miOpenRemoteLibrary.ShortcutKeys = System.Windows.Forms.Keys.R | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miOpenRemoteLibrary.Size = new System.Drawing.Size(296, 38);
			miOpenRemoteLibrary.Text = "Open Remote Library...";
			toolStripInsertSeperator.Name = "toolStripInsertSeperator";
			toolStripInsertSeperator.Size = new System.Drawing.Size(293, 6);
			miOpenNow.Name = "miOpenNow";
			miOpenNow.Size = new System.Drawing.Size(296, 38);
			miOpenNow.Text = "Open Books";
			miOpenRecent.Name = "miOpenRecent";
			miOpenRecent.Size = new System.Drawing.Size(296, 38);
			miOpenRecent.Text = "&Recent Books";
			miOpenRecent.DropDownOpening += new System.EventHandler(RecentFilesMenuOpening);
			toolStripMenuItem4.Name = "toolStripMenuItem4";
			toolStripMenuItem4.Size = new System.Drawing.Size(293, 6);
			miRestart.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Restart;
			miRestart.Name = "miRestart";
			miRestart.ShortcutKeys = System.Windows.Forms.Keys.Q | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRestart.Size = new System.Drawing.Size(296, 38);
			miRestart.Text = "Rest&art";
			toolStripMenuItem24.Name = "toolStripMenuItem24";
			toolStripMenuItem24.Size = new System.Drawing.Size(293, 6);
			miExit.Name = "miExit";
			miExit.ShortcutKeys = System.Windows.Forms.Keys.Q | System.Windows.Forms.Keys.Control;
			miExit.Size = new System.Drawing.Size(296, 38);
			miExit.Text = "&Exit";
			editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[17]
			{
		miShowInfo,
		toolStripMenuItem43,
		miUndo,
		miRedo,
		toolStripMenuItem22,
		miRating,
		miPageType,
		miPageRotate,
		miBookmarks,
		toolStripMenuItem40,
		miCopyPage,
		miExportPage,
		toolStripMenuItem39,
		miViewRefresh,
		toolStripSeparator4,
		miDevices,
		miPreferences
			});
			editMenu.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			editMenu.Name = "editMenu";
			editMenu.Size = new System.Drawing.Size(39, 20);
			editMenu.Text = "&Edit";
			editMenu.DropDownOpening += new System.EventHandler(editMenu_DropDownOpening);
			miShowInfo.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GetInfo;
			miShowInfo.Name = "miShowInfo";
			miShowInfo.ShortcutKeys = System.Windows.Forms.Keys.I | System.Windows.Forms.Keys.Control;
			miShowInfo.Size = new System.Drawing.Size(219, 22);
			miShowInfo.Text = "Info...";
			toolStripMenuItem43.Name = "toolStripMenuItem43";
			toolStripMenuItem43.Size = new System.Drawing.Size(216, 6);
			miUndo.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Undo;
			miUndo.Name = "miUndo";
			miUndo.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Control;
			miUndo.Size = new System.Drawing.Size(219, 22);
			miUndo.Text = "&Undo";
			miRedo.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Redo;
			miRedo.Name = "miRedo";
			miRedo.ShortcutKeys = System.Windows.Forms.Keys.Y | System.Windows.Forms.Keys.Control;
			miRedo.Size = new System.Drawing.Size(219, 22);
			miRedo.Text = "&Redo";
			toolStripMenuItem22.Name = "toolStripMenuItem22";
			toolStripMenuItem22.Size = new System.Drawing.Size(216, 6);
			miRating.DropDown = contextRating;
			miRating.Name = "miRating";
			miRating.Size = new System.Drawing.Size(219, 22);
			miRating.Text = "My R&ating";
			contextRating.Items.AddRange(new System.Windows.Forms.ToolStripItem[9]
			{
		miRate0,
		toolStripMenuItem12,
		miRate1,
		miRate2,
		miRate3,
		miRate4,
		miRate5,
		toolStripMenuItem58,
		miQuickRating
			});
			contextRating.Name = "contextRating";
			contextRating.Size = new System.Drawing.Size(286, 170);
			miRate0.Name = "miRate0";
			miRate0.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miRate0.Size = new System.Drawing.Size(285, 22);
			miRate0.Text = "None";
			toolStripMenuItem12.Name = "toolStripMenuItem12";
			toolStripMenuItem12.Size = new System.Drawing.Size(282, 6);
			miRate1.Name = "miRate1";
			miRate1.ShortcutKeys = System.Windows.Forms.Keys.D1 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miRate1.Size = new System.Drawing.Size(285, 22);
			miRate1.Text = "* (1 Star)";
			miRate2.Name = "miRate2";
			miRate2.ShortcutKeys = System.Windows.Forms.Keys.D2 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miRate2.Size = new System.Drawing.Size(285, 22);
			miRate2.Text = "** (2 Stars)";
			miRate3.Name = "miRate3";
			miRate3.ShortcutKeys = System.Windows.Forms.Keys.D3 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miRate3.Size = new System.Drawing.Size(285, 22);
			miRate3.Text = "*** (3 Stars)";
			miRate4.Name = "miRate4";
			miRate4.ShortcutKeys = System.Windows.Forms.Keys.D4 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miRate4.Size = new System.Drawing.Size(285, 22);
			miRate4.Text = "**** (4 Stars)";
			miRate5.Name = "miRate5";
			miRate5.ShortcutKeys = System.Windows.Forms.Keys.D5 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miRate5.Size = new System.Drawing.Size(285, 22);
			miRate5.Text = "***** (5 Stars)";
			toolStripMenuItem58.Name = "toolStripMenuItem58";
			toolStripMenuItem58.Size = new System.Drawing.Size(282, 6);
			miQuickRating.Name = "miQuickRating";
			miQuickRating.ShortcutKeys = System.Windows.Forms.Keys.Q | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miQuickRating.Size = new System.Drawing.Size(285, 22);
			miQuickRating.Text = "Quick Rating and Review...";
			miPageType.Name = "miPageType";
			miPageType.Size = new System.Drawing.Size(219, 22);
			miPageType.Text = "&Page Type";
			miPageRotate.Name = "miPageRotate";
			miPageRotate.Size = new System.Drawing.Size(219, 22);
			miPageRotate.Text = "Page Rotation";
			miBookmarks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8]
			{
		miSetBookmark,
		miRemoveBookmark,
		toolStripMenuItem26,
		miPrevBookmark,
		miNextBookmark,
		toolStripMenuItem8,
		miLastPageRead,
		toolStripMenuItem37
			});
			miBookmarks.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Bookmark;
			miBookmarks.Name = "miBookmarks";
			miBookmarks.Size = new System.Drawing.Size(219, 22);
			miBookmarks.Text = "&Bookmarks";
			miBookmarks.DropDownOpening += new System.EventHandler(miBookmarks_DropDownOpening);
			miSetBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewBookmark;
			miSetBookmark.Name = "miSetBookmark";
			miSetBookmark.ShortcutKeys = System.Windows.Forms.Keys.B | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miSetBookmark.Size = new System.Drawing.Size(249, 22);
			miSetBookmark.Text = "Set Bookmark...";
			miRemoveBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoveBookmark;
			miRemoveBookmark.Name = "miRemoveBookmark";
			miRemoveBookmark.ShortcutKeys = System.Windows.Forms.Keys.D | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRemoveBookmark.Size = new System.Drawing.Size(249, 22);
			miRemoveBookmark.Text = "Remove Bookmark";
			toolStripMenuItem26.Name = "toolStripMenuItem26";
			toolStripMenuItem26.Size = new System.Drawing.Size(246, 6);
			miPrevBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.PreviousBookmark;
			miPrevBookmark.Name = "miPrevBookmark";
			miPrevBookmark.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miPrevBookmark.Size = new System.Drawing.Size(249, 22);
			miPrevBookmark.Text = "Previous Bookmark";
			miNextBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NextBookmark;
			miNextBookmark.Name = "miNextBookmark";
			miNextBookmark.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miNextBookmark.Size = new System.Drawing.Size(249, 22);
			miNextBookmark.Text = "Next Bookmark";
			toolStripMenuItem8.Name = "toolStripMenuItem8";
			toolStripMenuItem8.Size = new System.Drawing.Size(246, 6);
			miLastPageRead.Name = "miLastPageRead";
			miLastPageRead.ShortcutKeys = System.Windows.Forms.Keys.L | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miLastPageRead.Size = new System.Drawing.Size(249, 22);
			miLastPageRead.Text = "L&ast Page Read";
			toolStripMenuItem37.Name = "toolStripMenuItem37";
			toolStripMenuItem37.Size = new System.Drawing.Size(246, 6);
			toolStripMenuItem37.Tag = "bms";
			toolStripMenuItem40.Name = "toolStripMenuItem40";
			toolStripMenuItem40.Size = new System.Drawing.Size(216, 6);
			miCopyPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Copy;
			miCopyPage.Name = "miCopyPage";
			miCopyPage.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
			miCopyPage.Size = new System.Drawing.Size(219, 22);
			miCopyPage.Text = "&Copy Page";
			miExportPage.Name = "miExportPage";
			miExportPage.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miExportPage.Size = new System.Drawing.Size(219, 22);
			miExportPage.Text = "&Export Page...";
			toolStripMenuItem39.Name = "toolStripMenuItem39";
			toolStripMenuItem39.Size = new System.Drawing.Size(216, 6);
			miViewRefresh.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			miViewRefresh.Name = "miViewRefresh";
			miViewRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
			miViewRefresh.Size = new System.Drawing.Size(219, 22);
			miViewRefresh.Text = "&Refresh";
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new System.Drawing.Size(216, 6);
			miDevices.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDevices;
			miDevices.Name = "miDevices";
			miDevices.Size = new System.Drawing.Size(219, 22);
			miDevices.Text = "Devices...";
			miPreferences.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Preferences;
			miPreferences.Name = "miPreferences";
			miPreferences.ShortcutKeys = System.Windows.Forms.Keys.F9 | System.Windows.Forms.Keys.Control;
			miPreferences.Size = new System.Drawing.Size(219, 22);
			miPreferences.Text = "&Preferences...";
			browseMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[16]
			{
		miToggleBrowser,
		toolStripMenuItem2,
		miViewLibrary,
		miViewFolders,
		miViewPages,
		toolStripMenuItem9,
		miSidebar,
		miSmallPreview,
		miSearchBrowser,
		miInfoPanel,
		toolStripMenuItem56,
		miPreviousList,
		miNextList,
		toolStripMenuItem6,
		miWorkspaces,
		miListLayouts
			});
			browseMenu.Name = "browseMenu";
			browseMenu.Size = new System.Drawing.Size(57, 20);
			browseMenu.Text = "&Browse";
			miToggleBrowser.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Browser;
			miToggleBrowser.Name = "miToggleBrowser";
			miToggleBrowser.ShortcutKeys = System.Windows.Forms.Keys.F3;
			miToggleBrowser.Size = new System.Drawing.Size(205, 22);
			miToggleBrowser.Text = "&Browser";
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new System.Drawing.Size(202, 6);
			miViewLibrary.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Database;
			miViewLibrary.Name = "miViewLibrary";
			miViewLibrary.ShortcutKeys = System.Windows.Forms.Keys.F6;
			miViewLibrary.Size = new System.Drawing.Size(205, 22);
			miViewLibrary.Text = "Li&brary";
			miViewFolders.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FileBrowser;
			miViewFolders.Name = "miViewFolders";
			miViewFolders.ShortcutKeys = System.Windows.Forms.Keys.F7;
			miViewFolders.Size = new System.Drawing.Size(205, 22);
			miViewFolders.Text = "&Folders";
			miViewPages.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ComicPage;
			miViewPages.Name = "miViewPages";
			miViewPages.ShortcutKeys = System.Windows.Forms.Keys.F8;
			miViewPages.Size = new System.Drawing.Size(205, 22);
			miViewPages.Text = "&Pages";
			toolStripMenuItem9.Name = "toolStripMenuItem9";
			toolStripMenuItem9.Size = new System.Drawing.Size(202, 6);
			miSidebar.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Sidebar;
			miSidebar.Name = "miSidebar";
			miSidebar.ShortcutKeys = System.Windows.Forms.Keys.F6 | System.Windows.Forms.Keys.Shift;
			miSidebar.Size = new System.Drawing.Size(205, 22);
			miSidebar.Text = "&Sidebar";
			miSmallPreview.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallPreview;
			miSmallPreview.Name = "miSmallPreview";
			miSmallPreview.ShortcutKeys = System.Windows.Forms.Keys.F7 | System.Windows.Forms.Keys.Shift;
			miSmallPreview.Size = new System.Drawing.Size(205, 22);
			miSmallPreview.Text = "S&mall Preview";
			miSearchBrowser.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Search;
			miSearchBrowser.Name = "miSearchBrowser";
			miSearchBrowser.ShortcutKeys = System.Windows.Forms.Keys.F8 | System.Windows.Forms.Keys.Shift;
			miSearchBrowser.Size = new System.Drawing.Size(205, 22);
			miSearchBrowser.Text = "S&earch Browser";
			miInfoPanel.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.InfoPanel;
			miInfoPanel.Name = "miInfoPanel";
			miInfoPanel.ShortcutKeys = System.Windows.Forms.Keys.F9 | System.Windows.Forms.Keys.Shift;
			miInfoPanel.Size = new System.Drawing.Size(205, 22);
			miInfoPanel.Text = "Info Panel";
			toolStripMenuItem56.Name = "toolStripMenuItem56";
			toolStripMenuItem56.Size = new System.Drawing.Size(202, 6);
			miPreviousList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.BrowsePrevious;
			miPreviousList.Name = "miPreviousList";
			miPreviousList.ShortcutKeys = System.Windows.Forms.Keys.J | System.Windows.Forms.Keys.Control;
			miPreviousList.Size = new System.Drawing.Size(205, 22);
			miPreviousList.Text = "Previous List";
			miNextList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.BrowseNext;
			miNextList.Name = "miNextList";
			miNextList.ShortcutKeys = System.Windows.Forms.Keys.K | System.Windows.Forms.Keys.Control;
			miNextList.Size = new System.Drawing.Size(205, 22);
			miNextList.Text = "Next List";
			toolStripMenuItem6.Name = "toolStripMenuItem6";
			toolStripMenuItem6.Size = new System.Drawing.Size(202, 6);
			miWorkspaces.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
		miSaveWorkspace,
		miEditWorkspaces,
		miWorkspaceSep
			});
			miWorkspaces.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Workspace;
			miWorkspaces.Name = "miWorkspaces";
			miWorkspaces.Size = new System.Drawing.Size(205, 22);
			miWorkspaces.Text = "&Workspaces";
			miSaveWorkspace.Name = "miSaveWorkspace";
			miSaveWorkspace.ShortcutKeys = System.Windows.Forms.Keys.W | System.Windows.Forms.Keys.Control;
			miSaveWorkspace.Size = new System.Drawing.Size(237, 22);
			miSaveWorkspace.Text = "&Save Workspace...";
			miEditWorkspaces.Name = "miEditWorkspaces";
			miEditWorkspaces.ShortcutKeys = System.Windows.Forms.Keys.W | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			miEditWorkspaces.Size = new System.Drawing.Size(237, 22);
			miEditWorkspaces.Text = "&Edit Workspaces...";
			miWorkspaceSep.Name = "miWorkspaceSep";
			miWorkspaceSep.Size = new System.Drawing.Size(234, 6);
			miListLayouts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[6]
			{
		miEditListLayout,
		miSaveListLayout,
		toolStripMenuItem10,
		miEditLayouts,
		miSetAllListsSame,
		miLayoutSep
			});
			miListLayouts.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ListLayout;
			miListLayouts.Name = "miListLayouts";
			miListLayouts.Size = new System.Drawing.Size(205, 22);
			miListLayouts.Text = "List Layout";
			miEditListLayout.Name = "miEditListLayout";
			miEditListLayout.ShortcutKeys = System.Windows.Forms.Keys.L | System.Windows.Forms.Keys.Control;
			miEditListLayout.Size = new System.Drawing.Size(225, 22);
			miEditListLayout.Text = "&Edit List Layout...";
			miSaveListLayout.Name = "miSaveListLayout";
			miSaveListLayout.Size = new System.Drawing.Size(225, 22);
			miSaveListLayout.Text = "&Save List Layout...";
			toolStripMenuItem10.Name = "toolStripMenuItem10";
			toolStripMenuItem10.Size = new System.Drawing.Size(222, 6);
			miEditLayouts.Name = "miEditLayouts";
			miEditLayouts.ShortcutKeys = System.Windows.Forms.Keys.L | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			miEditLayouts.Size = new System.Drawing.Size(225, 22);
			miEditLayouts.Text = "&Edit Layouts...";
			miSetAllListsSame.Name = "miSetAllListsSame";
			miSetAllListsSame.Size = new System.Drawing.Size(225, 22);
			miSetAllListsSame.Text = "Set all Lists to current Layout";
			miLayoutSep.Name = "miLayoutSep";
			miLayoutSep.Size = new System.Drawing.Size(222, 6);
			readMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[17]
			{
		miFirstPage,
		miPrevPage,
		miNextPage,
		miLastPage,
		toolStripMenuItem18,
		miPrevFromList,
		miNextFromList,
		miRandomFromList,
		miSyncBrowser,
		toolStripMenuItem17,
		miPrevTab,
		miNextTab,
		toolStripMenuItem1,
		miAutoScroll,
		miDoublePageAutoScroll,
		toolStripMenuItem21,
		miTrackCurrentPage
			});
			readMenu.Name = "readMenu";
			readMenu.Size = new System.Drawing.Size(45, 20);
			readMenu.Text = "&Read";
			miFirstPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
			miFirstPage.Name = "miFirstPage";
			miFirstPage.ShortcutKeyDisplayString = "";
			miFirstPage.ShortcutKeys = System.Windows.Forms.Keys.B | System.Windows.Forms.Keys.Control;
			miFirstPage.Size = new System.Drawing.Size(287, 22);
			miFirstPage.Text = "&First Page";
			miPrevPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
			miPrevPage.Name = "miPrevPage";
			miPrevPage.ShortcutKeyDisplayString = "";
			miPrevPage.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Control;
			miPrevPage.Size = new System.Drawing.Size(287, 22);
			miPrevPage.Text = "&Previous Page";
			miNextPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
			miNextPage.Name = "miNextPage";
			miNextPage.ShortcutKeyDisplayString = "";
			miNextPage.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Control;
			miNextPage.Size = new System.Drawing.Size(287, 22);
			miNextPage.Text = "&Next Page";
			miLastPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
			miLastPage.Name = "miLastPage";
			miLastPage.ShortcutKeyDisplayString = "";
			miLastPage.ShortcutKeys = System.Windows.Forms.Keys.E | System.Windows.Forms.Keys.Control;
			miLastPage.Size = new System.Drawing.Size(287, 22);
			miLastPage.Text = "&Last Page";
			toolStripMenuItem18.Name = "toolStripMenuItem18";
			toolStripMenuItem18.Size = new System.Drawing.Size(284, 6);
			miPrevFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.PrevFromList;
			miPrevFromList.Name = "miPrevFromList";
			miPrevFromList.ShortcutKeyDisplayString = "";
			miPrevFromList.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			miPrevFromList.Size = new System.Drawing.Size(287, 22);
			miPrevFromList.Text = "Pre&vious Book";
			miNextFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NextFromList;
			miNextFromList.Name = "miNextFromList";
			miNextFromList.ShortcutKeyDisplayString = "";
			miNextFromList.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			miNextFromList.Size = new System.Drawing.Size(287, 22);
			miNextFromList.Text = "Ne&xt Book";
			miRandomFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RandomComic;
			miRandomFromList.Name = "miRandomFromList";
			miRandomFromList.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			miRandomFromList.Size = new System.Drawing.Size(287, 22);
			miRandomFromList.Text = "Random Book";
			miSyncBrowser.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SyncBrowser;
			miSyncBrowser.Name = "miSyncBrowser";
			miSyncBrowser.ShortcutKeys = System.Windows.Forms.Keys.F3 | System.Windows.Forms.Keys.Control;
			miSyncBrowser.Size = new System.Drawing.Size(287, 22);
			miSyncBrowser.Text = "Show in &Browser";
			toolStripMenuItem17.Name = "toolStripMenuItem17";
			toolStripMenuItem17.Size = new System.Drawing.Size(284, 6);
			miPrevTab.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Previous;
			miPrevTab.Name = "miPrevTab";
			miPrevTab.ShortcutKeys = System.Windows.Forms.Keys.J | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miPrevTab.Size = new System.Drawing.Size(287, 22);
			miPrevTab.Text = "&Previous Tab";
			miNextTab.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Next;
			miNextTab.Name = "miNextTab";
			miNextTab.ShortcutKeys = System.Windows.Forms.Keys.K | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miNextTab.Size = new System.Drawing.Size(287, 22);
			miNextTab.Text = "Next &Tab";
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(284, 6);
			miAutoScroll.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.CursorScroll;
			miAutoScroll.Name = "miAutoScroll";
			miAutoScroll.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control;
			miAutoScroll.Size = new System.Drawing.Size(287, 22);
			miAutoScroll.Text = "&Auto Scrolling";
			miDoublePageAutoScroll.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageAutoscroll;
			miDoublePageAutoScroll.Name = "miDoublePageAutoScroll";
			miDoublePageAutoScroll.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miDoublePageAutoScroll.Size = new System.Drawing.Size(287, 22);
			miDoublePageAutoScroll.Text = "Double Page Auto Scrolling";
			toolStripMenuItem21.Name = "toolStripMenuItem21";
			toolStripMenuItem21.Size = new System.Drawing.Size(284, 6);
			miTrackCurrentPage.Name = "miTrackCurrentPage";
			miTrackCurrentPage.ShortcutKeys = System.Windows.Forms.Keys.T | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			miTrackCurrentPage.Size = new System.Drawing.Size(287, 22);
			miTrackCurrentPage.Text = "Track current Page";
			displayMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[11]
			{
		miComicDisplaySettings,
		toolStripSeparator3,
		miPageLayout,
		miZoom,
		miRotation,
		toolStripMenuItem23,
		miMinimalGui,
		miFullScreen,
		miReaderUndocked,
		toolStripMenuItem41,
		miMagnify
			});
			displayMenu.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			displayMenu.Name = "displayMenu";
			displayMenu.Size = new System.Drawing.Size(57, 20);
			displayMenu.Text = "&Display";
			miComicDisplaySettings.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DisplaySettings;
			miComicDisplaySettings.Name = "miComicDisplaySettings";
			miComicDisplaySettings.ShortcutKeys = System.Windows.Forms.Keys.F9;
			miComicDisplaySettings.Size = new System.Drawing.Size(237, 38);
			miComicDisplaySettings.Text = "Book Display Settings...";
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new System.Drawing.Size(234, 6);
			miPageLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[13]
			{
		miOriginal,
		miFitAll,
		miFitWidth,
		miFitWidthAdaptive,
		miFitHeight,
		miBestFit,
		toolStripMenuItem27,
		miSinglePage,
		miTwoPages,
		miTwoPagesAdaptive,
		miRightToLeft,
		toolStripMenuItem44,
		miOnlyFitOversized
			});
			miPageLayout.Name = "miPageLayout";
			miPageLayout.Size = new System.Drawing.Size(237, 38);
			miPageLayout.Text = "&Page Layout";
			miOriginal.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Original;
			miOriginal.Name = "miOriginal";
			miOriginal.ShortcutKeys = System.Windows.Forms.Keys.D1 | System.Windows.Forms.Keys.Control;
			miOriginal.Size = new System.Drawing.Size(247, 22);
			miOriginal.Text = "Original Size";
			miFitAll.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			miFitAll.Name = "miFitAll";
			miFitAll.ShortcutKeys = System.Windows.Forms.Keys.D2 | System.Windows.Forms.Keys.Control;
			miFitAll.Size = new System.Drawing.Size(247, 22);
			miFitAll.Text = "Fit &All";
			miFitWidth.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidth;
			miFitWidth.Name = "miFitWidth";
			miFitWidth.ShortcutKeys = System.Windows.Forms.Keys.D3 | System.Windows.Forms.Keys.Control;
			miFitWidth.Size = new System.Drawing.Size(247, 22);
			miFitWidth.Text = "Fit &Width";
			miFitWidthAdaptive.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidthAdaptive;
			miFitWidthAdaptive.Name = "miFitWidthAdaptive";
			miFitWidthAdaptive.ShortcutKeys = System.Windows.Forms.Keys.D4 | System.Windows.Forms.Keys.Control;
			miFitWidthAdaptive.Size = new System.Drawing.Size(247, 22);
			miFitWidthAdaptive.Text = "Fit Width (adaptive)";
			miFitHeight.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitHeight;
			miFitHeight.Name = "miFitHeight";
			miFitHeight.ShortcutKeys = System.Windows.Forms.Keys.D5 | System.Windows.Forms.Keys.Control;
			miFitHeight.Size = new System.Drawing.Size(247, 22);
			miFitHeight.Text = "Fit &Height";
			miBestFit.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitBest;
			miBestFit.Name = "miBestFit";
			miBestFit.ShortcutKeys = System.Windows.Forms.Keys.D6 | System.Windows.Forms.Keys.Control;
			miBestFit.Size = new System.Drawing.Size(247, 22);
			miBestFit.Text = "Fit &Best";
			toolStripMenuItem27.Name = "toolStripMenuItem27";
			toolStripMenuItem27.Size = new System.Drawing.Size(244, 6);
			miSinglePage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			miSinglePage.Name = "miSinglePage";
			miSinglePage.ShortcutKeys = System.Windows.Forms.Keys.D7 | System.Windows.Forms.Keys.Control;
			miSinglePage.Size = new System.Drawing.Size(247, 22);
			miSinglePage.Text = "Single Page";
			miTwoPages.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageForced;
			miTwoPages.Name = "miTwoPages";
			miTwoPages.ShortcutKeys = System.Windows.Forms.Keys.D8 | System.Windows.Forms.Keys.Control;
			miTwoPages.Size = new System.Drawing.Size(247, 22);
			miTwoPages.Text = "Two Pages";
			miTwoPagesAdaptive.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
			miTwoPagesAdaptive.Name = "miTwoPagesAdaptive";
			miTwoPagesAdaptive.ShortcutKeys = System.Windows.Forms.Keys.D9 | System.Windows.Forms.Keys.Control;
			miTwoPagesAdaptive.Size = new System.Drawing.Size(247, 22);
			miTwoPagesAdaptive.Text = "Two Pages (adaptive)";
			miTwoPagesAdaptive.ToolTipText = "Show one or two pages";
			miRightToLeft.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RightToLeft;
			miRightToLeft.Name = "miRightToLeft";
			miRightToLeft.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Control;
			miRightToLeft.Size = new System.Drawing.Size(247, 22);
			miRightToLeft.Text = "Right to Left";
			toolStripMenuItem44.Name = "toolStripMenuItem44";
			toolStripMenuItem44.Size = new System.Drawing.Size(244, 6);
			miOnlyFitOversized.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Oversized;
			miOnlyFitOversized.Name = "miOnlyFitOversized";
			miOnlyFitOversized.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miOnlyFitOversized.Size = new System.Drawing.Size(247, 22);
			miOnlyFitOversized.Text = "&Only fit if oversized";
			miZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[11]
			{
		miZoomIn,
		miZoomOut,
		miToggleZoom,
		toolStripSeparator14,
		miZoom100,
		miZoom125,
		miZoom150,
		miZoom200,
		miZoom400,
		toolStripSeparator15,
		miZoomCustom
			});
			miZoom.Name = "miZoom";
			miZoom.Size = new System.Drawing.Size(237, 38);
			miZoom.Text = "Zoom";
			miZoomIn.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomIn;
			miZoomIn.Name = "miZoomIn";
			miZoomIn.ShortcutKeys = System.Windows.Forms.Keys.Oemplus | System.Windows.Forms.Keys.Control;
			miZoomIn.Size = new System.Drawing.Size(222, 22);
			miZoomIn.Text = "Zoom &In";
			miZoomOut.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomOut;
			miZoomOut.Name = "miZoomOut";
			miZoomOut.ShortcutKeys = System.Windows.Forms.Keys.OemMinus | System.Windows.Forms.Keys.Control;
			miZoomOut.Size = new System.Drawing.Size(222, 22);
			miZoomOut.Text = "Zoom &Out";
			miToggleZoom.Name = "miToggleZoom";
			miToggleZoom.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			miToggleZoom.Size = new System.Drawing.Size(222, 22);
			miToggleZoom.Text = "Toggle Zoom";
			toolStripSeparator14.Name = "toolStripSeparator14";
			toolStripSeparator14.Size = new System.Drawing.Size(219, 6);
			miZoom100.Name = "miZoom100";
			miZoom100.Size = new System.Drawing.Size(222, 22);
			miZoom100.Text = "100%";
			miZoom125.Name = "miZoom125";
			miZoom125.Size = new System.Drawing.Size(222, 22);
			miZoom125.Text = "125%";
			miZoom150.Name = "miZoom150";
			miZoom150.Size = new System.Drawing.Size(222, 22);
			miZoom150.Text = "150%";
			miZoom200.Name = "miZoom200";
			miZoom200.Size = new System.Drawing.Size(222, 22);
			miZoom200.Text = "200%";
			miZoom400.Name = "miZoom400";
			miZoom400.Size = new System.Drawing.Size(222, 22);
			miZoom400.Text = "400%";
			toolStripSeparator15.Name = "toolStripSeparator15";
			toolStripSeparator15.Size = new System.Drawing.Size(219, 6);
			miZoomCustom.Name = "miZoomCustom";
			miZoomCustom.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miZoomCustom.Size = new System.Drawing.Size(222, 22);
			miZoomCustom.Text = "&Custom...";
			miRotation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9]
			{
		miRotateLeft,
		miRotateRight,
		toolStripMenuItem33,
		miRotate0,
		miRotate90,
		miRotate180,
		miRotate270,
		toolStripMenuItem36,
		miAutoRotate
			});
			miRotation.Name = "miRotation";
			miRotation.Size = new System.Drawing.Size(237, 38);
			miRotation.Text = "&Rotation";
			miRotateLeft.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateLeft;
			miRotateLeft.Name = "miRotateLeft";
			miRotateLeft.ShortcutKeys = System.Windows.Forms.Keys.OemMinus | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRotateLeft.Size = new System.Drawing.Size(256, 22);
			miRotateLeft.Text = "Rotate Left";
			miRotateRight.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateRight;
			miRotateRight.Name = "miRotateRight";
			miRotateRight.ShortcutKeys = System.Windows.Forms.Keys.Oemplus | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRotateRight.Size = new System.Drawing.Size(256, 22);
			miRotateRight.Text = "Rotate Right";
			toolStripMenuItem33.Name = "toolStripMenuItem33";
			toolStripMenuItem33.Size = new System.Drawing.Size(253, 6);
			miRotate0.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate0;
			miRotate0.Name = "miRotate0";
			miRotate0.ShortcutKeys = System.Windows.Forms.Keys.D7 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRotate0.Size = new System.Drawing.Size(256, 22);
			miRotate0.Text = "&No Rotation";
			miRotate90.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate90;
			miRotate90.Name = "miRotate90";
			miRotate90.ShortcutKeys = System.Windows.Forms.Keys.D8 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRotate90.Size = new System.Drawing.Size(256, 22);
			miRotate90.Text = "90°";
			miRotate180.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate180;
			miRotate180.Name = "miRotate180";
			miRotate180.ShortcutKeys = System.Windows.Forms.Keys.D9 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRotate180.Size = new System.Drawing.Size(256, 22);
			miRotate180.Text = "180°";
			miRotate270.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate270;
			miRotate270.Name = "miRotate270";
			miRotate270.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			miRotate270.Size = new System.Drawing.Size(256, 22);
			miRotate270.Text = "270°";
			toolStripMenuItem36.Name = "toolStripMenuItem36";
			toolStripMenuItem36.Size = new System.Drawing.Size(253, 6);
			miAutoRotate.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.AutoRotate;
			miAutoRotate.Name = "miAutoRotate";
			miAutoRotate.Size = new System.Drawing.Size(256, 22);
			miAutoRotate.Text = "Autorotate Double Pages";
			toolStripMenuItem23.Name = "toolStripMenuItem23";
			toolStripMenuItem23.Size = new System.Drawing.Size(234, 6);
			miMinimalGui.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.MenuToggle;
			miMinimalGui.Name = "miMinimalGui";
			miMinimalGui.ShortcutKeys = System.Windows.Forms.Keys.F10;
			miMinimalGui.Size = new System.Drawing.Size(237, 38);
			miMinimalGui.Text = "Minimal User Interface";
			miFullScreen.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FullScreen;
			miFullScreen.Name = "miFullScreen";
			miFullScreen.ShortcutKeyDisplayString = "";
			miFullScreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
			miFullScreen.Size = new System.Drawing.Size(237, 38);
			miFullScreen.Text = "&Full Screen";
			miReaderUndocked.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.UndockReader;
			miReaderUndocked.Name = "miReaderUndocked";
			miReaderUndocked.ShortcutKeys = System.Windows.Forms.Keys.F12;
			miReaderUndocked.Size = new System.Drawing.Size(237, 38);
			miReaderUndocked.Text = "Reader in &own Window";
			toolStripMenuItem41.Name = "toolStripMenuItem41";
			toolStripMenuItem41.Size = new System.Drawing.Size(234, 6);
			miMagnify.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Zoom;
			miMagnify.Name = "miMagnify";
			miMagnify.ShortcutKeys = System.Windows.Forms.Keys.M | System.Windows.Forms.Keys.Control;
			miMagnify.Size = new System.Drawing.Size(237, 38);
			miMagnify.Text = "&Magnifier";
			helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[12]
			{
		miHelp,
		miWebHelp,
		miHelpPlugins,
		miChooseHelpSystem,
		miHelpQuickIntro,
		toolStripMenuItem3,
		miWebHome,
		miWebUserForum,
		toolStripMenuItem5,
		miNews,
		toolStripMenuItem25,
		miAbout
			});
			helpMenu.Name = "helpMenu";
			helpMenu.Size = new System.Drawing.Size(44, 20);
			helpMenu.Text = "&Help";
			miHelp.Name = "miHelp";
			miHelp.Size = new System.Drawing.Size(256, 22);
			miHelp.Text = "Help";
			miHelp.Visible = false;
			miWebHelp.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Help;
			miWebHelp.Name = "miWebHelp";
			miWebHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
			miWebHelp.Size = new System.Drawing.Size(256, 22);
			miWebHelp.Text = "ComicRack Documentation...";
			miHelpPlugins.Name = "miHelpPlugins";
			miHelpPlugins.Size = new System.Drawing.Size(256, 22);
			miHelpPlugins.Text = "Plugins";
			miChooseHelpSystem.Name = "miChooseHelpSystem";
			miChooseHelpSystem.Size = new System.Drawing.Size(256, 22);
			miChooseHelpSystem.Text = "Choose Help System";
			miHelpQuickIntro.Name = "miHelpQuickIntro";
			miHelpQuickIntro.Size = new System.Drawing.Size(256, 22);
			miHelpQuickIntro.Text = "Quick Introduction";
			toolStripMenuItem3.Name = "toolStripMenuItem3";
			toolStripMenuItem3.Size = new System.Drawing.Size(253, 6);
			miWebHome.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.WebBlog;
			miWebHome.Name = "miWebHome";
			miWebHome.ShortcutKeys = System.Windows.Forms.Keys.F1 | System.Windows.Forms.Keys.Shift;
			miWebHome.Size = new System.Drawing.Size(256, 22);
			miWebHome.Text = "ComicRack Homepage...";
			miWebUserForum.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.WebForum;
			miWebUserForum.Name = "miWebUserForum";
			miWebUserForum.ShortcutKeys = System.Windows.Forms.Keys.F1 | System.Windows.Forms.Keys.Control;
			miWebUserForum.Size = new System.Drawing.Size(256, 22);
			miWebUserForum.Text = "ComicRack User Forum...";
			toolStripMenuItem5.Name = "toolStripMenuItem5";
			toolStripMenuItem5.Size = new System.Drawing.Size(253, 6);
			miNews.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.News;
			miNews.Name = "miNews";
			miNews.Size = new System.Drawing.Size(256, 22);
			miNews.Text = "&News...";
			toolStripMenuItem25.Name = "toolStripMenuItem25";
			toolStripMenuItem25.Size = new System.Drawing.Size(253, 6);
			miAbout.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.About;
			miAbout.Name = "miAbout";
			miAbout.ShortcutKeys = System.Windows.Forms.Keys.F1 | System.Windows.Forms.Keys.Alt;
			miAbout.Size = new System.Drawing.Size(256, 22);
			miAbout.Text = "&About...";
			statusStrip.AutoSize = false;
			statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
			{
		tsText,
		tsDeviceSyncActivity,
		tsExportActivity,
		tsReadInfoActivity,
		tsWriteInfoActivity,
		tsPageActivity,
		tsScanActivity,
		tsDataSourceState,
		tsBook,
		tsCurrentPage,
		tsPageCount,
		tsServerActivity
			});
			statusStrip.Location = new System.Drawing.Point(0, 638);
			statusStrip.MinimumSize = new System.Drawing.Size(0, 24);
			statusStrip.Name = "statusStrip";
			statusStrip.ShowItemToolTips = true;
			statusStrip.Size = new System.Drawing.Size(744, 24);
			statusStrip.TabIndex = 3;
			tsText.Name = "tsText";
			tsText.Size = new System.Drawing.Size(603, 19);
			tsText.Spring = true;
			tsText.Text = "Ready";
			tsText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			tsDeviceSyncActivity.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsDeviceSyncActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsDeviceSyncActivity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsDeviceSyncActivity.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DeviceSyncAnimation;
			tsDeviceSyncActivity.Name = "tsDeviceSyncActivity";
			tsDeviceSyncActivity.Size = new System.Drawing.Size(36, 19);
			tsDeviceSyncActivity.Text = "Exporting";
			tsDeviceSyncActivity.Visible = false;
			tsDeviceSyncActivity.Click += new System.EventHandler(tsDeviceSyncActivity_Click);
			tsExportActivity.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsExportActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsExportActivity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsExportActivity.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ExportAnimation;
			tsExportActivity.Name = "tsExportActivity";
			tsExportActivity.Size = new System.Drawing.Size(36, 19);
			tsExportActivity.Text = "Exporting";
			tsExportActivity.Visible = false;
			tsExportActivity.Click += new System.EventHandler(tsExportActivity_Click);
			tsReadInfoActivity.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsReadInfoActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsReadInfoActivity.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ReadInfoAnimation;
			tsReadInfoActivity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			tsReadInfoActivity.Name = "tsReadInfoActivity";
			tsReadInfoActivity.Size = new System.Drawing.Size(36, 19);
			tsReadInfoActivity.ToolTipText = "Reading info data from files...";
			tsReadInfoActivity.Visible = false;
			tsReadInfoActivity.Click += new System.EventHandler(tsReadInfoActivity_Click);
			tsWriteInfoActivity.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsWriteInfoActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsWriteInfoActivity.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateInfoAnimation;
			tsWriteInfoActivity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			tsWriteInfoActivity.Name = "tsWriteInfoActivity";
			tsWriteInfoActivity.Size = new System.Drawing.Size(36, 19);
			tsWriteInfoActivity.ToolTipText = "Writing info data to files...";
			tsWriteInfoActivity.Visible = false;
			tsWriteInfoActivity.Click += new System.EventHandler(tsUpdateInfoActivity_Click);
			tsPageActivity.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsPageActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsPageActivity.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ReadPagesAnimation;
			tsPageActivity.Name = "tsPageActivity";
			tsPageActivity.Size = new System.Drawing.Size(36, 19);
			tsPageActivity.ToolTipText = "Getting Pages and Thumbnails...";
			tsPageActivity.Visible = false;
			tsPageActivity.Click += new System.EventHandler(tsPageActivity_Click);
			tsScanActivity.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsScanActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsScanActivity.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ScanAnimation;
			tsScanActivity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			tsScanActivity.Name = "tsScanActivity";
			tsScanActivity.Size = new System.Drawing.Size(36, 19);
			tsScanActivity.ToolTipText = "A scan is running...";
			tsScanActivity.Visible = false;
			tsScanActivity.Click += new System.EventHandler(tsScanActivity_Click);
			tsDataSourceState.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsDataSourceState.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsDataSourceState.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			tsDataSourceState.Name = "tsDataSourceState";
			tsDataSourceState.Size = new System.Drawing.Size(4, 19);
			tsDataSourceState.Visible = false;
			tsBook.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsBook.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsBook.Name = "tsBook";
			tsBook.Size = new System.Drawing.Size(38, 19);
			tsBook.Text = "Book";
			tsBook.ToolTipText = "Name of the opened Book";
			tsCurrentPage.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsCurrentPage.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsCurrentPage.Name = "tsCurrentPage";
			tsCurrentPage.Size = new System.Drawing.Size(37, 19);
			tsCurrentPage.Text = "Page";
			tsCurrentPage.ToolTipText = "Current Page of the open Book";
			tsCurrentPage.Click += new System.EventHandler(tsCurrentPage_Click);
			tsPageCount.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.All;
			tsPageCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsPageCount.Name = "tsPageCount";
			tsPageCount.Size = new System.Drawing.Size(51, 19);
			tsPageCount.Text = "0 Pages";
			tsPageCount.ToolTipText = "Page count of the open Book";
			tsServerActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			tsServerActivity.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GrayLight;
			tsServerActivity.Name = "tsServerActivity";
			tsServerActivity.Size = new System.Drawing.Size(32, 19);
			tsServerActivity.Visible = false;
			tsServerActivity.Click += new System.EventHandler(tsServerActivity_Click);
			pageContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
			{
		cmShowInfo,
		cmRating,
		cmPageType,
		cmPageRotate,
		cmBookmarks,
		toolStripSeparator10,
		cmComics,
		cmPageLayout,
		cmMagnify,
		toolStripSeparator2,
		cmCopyPage,
		cmExportPage,
		toolStripMenuItem11,
		cmRefreshPage,
		toolStripMenuItem46,
		cmMinimalGui
			});
			pageContextMenu.Name = "pageContextMenu";
			pageContextMenu.Size = new System.Drawing.Size(220, 292);
			pageContextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(pageContextMenu_Closed);
			pageContextMenu.Opening += new System.ComponentModel.CancelEventHandler(pageContextMenu_Opening);
			cmShowInfo.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GetInfo;
			cmShowInfo.Name = "cmShowInfo";
			cmShowInfo.ShortcutKeys = System.Windows.Forms.Keys.I | System.Windows.Forms.Keys.Control;
			cmShowInfo.Size = new System.Drawing.Size(219, 22);
			cmShowInfo.Text = "Info...";
			cmRating.DropDown = contextRating2;
			cmRating.Name = "cmRating";
			cmRating.Size = new System.Drawing.Size(219, 22);
			cmRating.Text = "My R&ating";
			contextRating2.Items.AddRange(new System.Windows.Forms.ToolStripItem[9]
			{
		cmRate0,
		toolStripMenuItem16,
		cmRate1,
		cmRate2,
		cmRate3,
		cmRate4,
		cmRate5,
		toolStripSeparator6,
		cmQuickRating
			});
			contextRating2.Name = "contextRating2";
			contextRating2.OwnerItem = cmRating;
			contextRating2.Size = new System.Drawing.Size(286, 170);
			cmRate0.Name = "cmRate0";
			cmRate0.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmRate0.Size = new System.Drawing.Size(285, 22);
			cmRate0.Text = "None";
			toolStripMenuItem16.Name = "toolStripMenuItem16";
			toolStripMenuItem16.Size = new System.Drawing.Size(282, 6);
			cmRate1.Name = "cmRate1";
			cmRate1.ShortcutKeys = System.Windows.Forms.Keys.D1 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmRate1.Size = new System.Drawing.Size(285, 22);
			cmRate1.Text = "* (1 Star)";
			cmRate2.Name = "cmRate2";
			cmRate2.ShortcutKeys = System.Windows.Forms.Keys.D2 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmRate2.Size = new System.Drawing.Size(285, 22);
			cmRate2.Text = "** (2 Stars)";
			cmRate3.Name = "cmRate3";
			cmRate3.ShortcutKeys = System.Windows.Forms.Keys.D3 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmRate3.Size = new System.Drawing.Size(285, 22);
			cmRate3.Text = "*** (3 Stars)";
			cmRate4.Name = "cmRate4";
			cmRate4.ShortcutKeys = System.Windows.Forms.Keys.D4 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmRate4.Size = new System.Drawing.Size(285, 22);
			cmRate4.Text = "**** (4 Stars)";
			cmRate5.Name = "cmRate5";
			cmRate5.ShortcutKeys = System.Windows.Forms.Keys.D5 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmRate5.Size = new System.Drawing.Size(285, 22);
			cmRate5.Text = "***** (5 Stars)";
			toolStripSeparator6.Name = "toolStripSeparator6";
			toolStripSeparator6.Size = new System.Drawing.Size(282, 6);
			cmQuickRating.Name = "cmQuickRating";
			cmQuickRating.ShortcutKeys = System.Windows.Forms.Keys.Q | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmQuickRating.Size = new System.Drawing.Size(285, 22);
			cmQuickRating.Text = "Quick Rating and Review...";
			cmPageType.Name = "cmPageType";
			cmPageType.Size = new System.Drawing.Size(219, 22);
			cmPageType.Text = "&Page Type";
			cmPageRotate.Name = "cmPageRotate";
			cmPageRotate.Size = new System.Drawing.Size(219, 22);
			cmPageRotate.Text = "Page Rotation";
			cmBookmarks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8]
			{
		cmSetBookmark,
		cmRemoveBookmark,
		toolStripMenuItem32,
		cmPrevBookmark,
		cmNextBookmark,
		toolStripSeparator13,
		cmLastPageRead,
		cmBookmarkSeparator
			});
			cmBookmarks.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Bookmark;
			cmBookmarks.Name = "cmBookmarks";
			cmBookmarks.Size = new System.Drawing.Size(219, 22);
			cmBookmarks.Text = "&Bookmarks";
			cmBookmarks.DropDownOpening += new System.EventHandler(cmBookmarks_DropDownOpening);
			cmSetBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewBookmark;
			cmSetBookmark.Name = "cmSetBookmark";
			cmSetBookmark.ShortcutKeys = System.Windows.Forms.Keys.B | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmSetBookmark.Size = new System.Drawing.Size(249, 22);
			cmSetBookmark.Text = "Set Bookmark...";
			cmRemoveBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoveBookmark;
			cmRemoveBookmark.Name = "cmRemoveBookmark";
			cmRemoveBookmark.ShortcutKeys = System.Windows.Forms.Keys.D | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmRemoveBookmark.Size = new System.Drawing.Size(249, 22);
			cmRemoveBookmark.Text = "Remove Bookmark";
			toolStripMenuItem32.Name = "toolStripMenuItem32";
			toolStripMenuItem32.Size = new System.Drawing.Size(246, 6);
			cmPrevBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.PreviousBookmark;
			cmPrevBookmark.Name = "cmPrevBookmark";
			cmPrevBookmark.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmPrevBookmark.Size = new System.Drawing.Size(249, 22);
			cmPrevBookmark.Text = "Previous Bookmark";
			cmNextBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NextBookmark;
			cmNextBookmark.Name = "cmNextBookmark";
			cmNextBookmark.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmNextBookmark.Size = new System.Drawing.Size(249, 22);
			cmNextBookmark.Text = "Next Bookmark";
			toolStripSeparator13.Name = "toolStripSeparator13";
			toolStripSeparator13.Size = new System.Drawing.Size(246, 6);
			cmLastPageRead.Name = "cmLastPageRead";
			cmLastPageRead.Size = new System.Drawing.Size(249, 22);
			cmLastPageRead.Text = "L&ast Page Read";
			cmBookmarkSeparator.Name = "cmBookmarkSeparator";
			cmBookmarkSeparator.Size = new System.Drawing.Size(246, 6);
			cmBookmarkSeparator.Tag = "bms";
			toolStripSeparator10.Name = "toolStripSeparator10";
			toolStripSeparator10.Size = new System.Drawing.Size(216, 6);
			cmComics.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[7]
			{
		cmOpenComic,
		cmCloseComic,
		toolStripMenuItem13,
		cmPrevFromList,
		cmNextFromList,
		cmRandomFromList,
		cmComicsSep
			});
			cmComics.Name = "cmComics";
			cmComics.Size = new System.Drawing.Size(219, 22);
			cmComics.Text = "Books";
			cmOpenComic.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			cmOpenComic.Name = "cmOpenComic";
			cmOpenComic.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control;
			cmOpenComic.Size = new System.Drawing.Size(218, 22);
			cmOpenComic.Text = "&Open File...";
			cmCloseComic.Name = "cmCloseComic";
			cmCloseComic.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
			cmCloseComic.Size = new System.Drawing.Size(218, 22);
			cmCloseComic.Text = "&Close";
			toolStripMenuItem13.Name = "toolStripMenuItem13";
			toolStripMenuItem13.Size = new System.Drawing.Size(215, 6);
			cmPrevFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.PrevFromList;
			cmPrevFromList.Name = "cmPrevFromList";
			cmPrevFromList.ShortcutKeyDisplayString = "";
			cmPrevFromList.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmPrevFromList.Size = new System.Drawing.Size(218, 22);
			cmPrevFromList.Text = "Pre&vious Book";
			cmNextFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NextFromList;
			cmNextFromList.Name = "cmNextFromList";
			cmNextFromList.ShortcutKeyDisplayString = "";
			cmNextFromList.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			cmNextFromList.Size = new System.Drawing.Size(218, 22);
			cmNextFromList.Text = "Ne&xt Book";
			cmRandomFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RandomComic;
			cmRandomFromList.Name = "cmRandomFromList";
			cmRandomFromList.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			cmRandomFromList.Size = new System.Drawing.Size(218, 22);
			cmRandomFromList.Text = "Random Book";
			cmComicsSep.Name = "cmComicsSep";
			cmComicsSep.Size = new System.Drawing.Size(215, 6);
			cmPageLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[18]
			{
		cmOriginal,
		cmFitAll,
		cmFitWidth,
		cmFitWidthAdaptive,
		cmFitHeight,
		cmFitBest,
		toolStripMenuItem29,
		cmSinglePage,
		cmTwoPages,
		cmTwoPagesAdaptive,
		cmRightToLeft,
		toolStripMenuItem38,
		cmRotate0,
		cmRotate90,
		cmRotate180,
		cmRotate270,
		toolStripMenuItem55,
		cmOnlyFitOversized
			});
			cmPageLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
			cmPageLayout.Name = "cmPageLayout";
			cmPageLayout.Size = new System.Drawing.Size(219, 22);
			cmPageLayout.Text = "Page Layout";
			cmOriginal.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Original;
			cmOriginal.Name = "cmOriginal";
			cmOriginal.ShortcutKeys = System.Windows.Forms.Keys.D1 | System.Windows.Forms.Keys.Control;
			cmOriginal.Size = new System.Drawing.Size(241, 22);
			cmOriginal.Text = "Original";
			cmFitAll.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			cmFitAll.Name = "cmFitAll";
			cmFitAll.ShortcutKeys = System.Windows.Forms.Keys.D2 | System.Windows.Forms.Keys.Control;
			cmFitAll.Size = new System.Drawing.Size(241, 22);
			cmFitAll.Text = "Fit All";
			cmFitWidth.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidth;
			cmFitWidth.Name = "cmFitWidth";
			cmFitWidth.ShortcutKeys = System.Windows.Forms.Keys.D3 | System.Windows.Forms.Keys.Control;
			cmFitWidth.Size = new System.Drawing.Size(241, 22);
			cmFitWidth.Text = "Fit Width";
			cmFitWidthAdaptive.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidthAdaptive;
			cmFitWidthAdaptive.Name = "cmFitWidthAdaptive";
			cmFitWidthAdaptive.ShortcutKeys = System.Windows.Forms.Keys.D4 | System.Windows.Forms.Keys.Control;
			cmFitWidthAdaptive.Size = new System.Drawing.Size(241, 22);
			cmFitWidthAdaptive.Text = "Fit Width (adaptive)";
			cmFitHeight.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitHeight;
			cmFitHeight.Name = "cmFitHeight";
			cmFitHeight.ShortcutKeys = System.Windows.Forms.Keys.D5 | System.Windows.Forms.Keys.Control;
			cmFitHeight.Size = new System.Drawing.Size(241, 22);
			cmFitHeight.Text = "Fit Height";
			cmFitBest.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitBest;
			cmFitBest.ImageTransparentColor = System.Drawing.Color.Magenta;
			cmFitBest.Name = "cmFitBest";
			cmFitBest.ShortcutKeys = System.Windows.Forms.Keys.D6 | System.Windows.Forms.Keys.Control;
			cmFitBest.Size = new System.Drawing.Size(241, 22);
			cmFitBest.Text = "Fit Best";
			toolStripMenuItem29.Name = "toolStripMenuItem29";
			toolStripMenuItem29.Size = new System.Drawing.Size(238, 6);
			cmSinglePage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			cmSinglePage.Name = "cmSinglePage";
			cmSinglePage.ShortcutKeys = System.Windows.Forms.Keys.D7 | System.Windows.Forms.Keys.Control;
			cmSinglePage.Size = new System.Drawing.Size(241, 22);
			cmSinglePage.Text = "Single Page";
			cmTwoPages.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageForced;
			cmTwoPages.Name = "cmTwoPages";
			cmTwoPages.ShortcutKeys = System.Windows.Forms.Keys.D8 | System.Windows.Forms.Keys.Control;
			cmTwoPages.Size = new System.Drawing.Size(241, 22);
			cmTwoPages.Text = "Two Pages";
			cmTwoPagesAdaptive.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
			cmTwoPagesAdaptive.Name = "cmTwoPagesAdaptive";
			cmTwoPagesAdaptive.ShortcutKeys = System.Windows.Forms.Keys.D9 | System.Windows.Forms.Keys.Control;
			cmTwoPagesAdaptive.Size = new System.Drawing.Size(241, 22);
			cmTwoPagesAdaptive.Text = "Two Pages (adaptive)";
			cmTwoPagesAdaptive.ToolTipText = "Show one or two pages";
			cmRightToLeft.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RightToLeft;
			cmRightToLeft.Name = "cmRightToLeft";
			cmRightToLeft.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Control;
			cmRightToLeft.Size = new System.Drawing.Size(241, 22);
			cmRightToLeft.Text = "Right to Left";
			toolStripMenuItem38.Name = "toolStripMenuItem38";
			toolStripMenuItem38.Size = new System.Drawing.Size(238, 6);
			cmRotate0.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate0;
			cmRotate0.Name = "cmRotate0";
			cmRotate0.ShortcutKeys = System.Windows.Forms.Keys.D7 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmRotate0.Size = new System.Drawing.Size(241, 22);
			cmRotate0.Text = "&No Rotation";
			cmRotate90.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate90;
			cmRotate90.Name = "cmRotate90";
			cmRotate90.ShortcutKeys = System.Windows.Forms.Keys.D8 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmRotate90.Size = new System.Drawing.Size(241, 22);
			cmRotate90.Text = "90°";
			cmRotate180.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate180;
			cmRotate180.Name = "cmRotate180";
			cmRotate180.ShortcutKeys = System.Windows.Forms.Keys.D9 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmRotate180.Size = new System.Drawing.Size(241, 22);
			cmRotate180.Text = "180°";
			cmRotate270.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate270;
			cmRotate270.Name = "cmRotate270";
			cmRotate270.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmRotate270.Size = new System.Drawing.Size(241, 22);
			cmRotate270.Text = "270°";
			toolStripMenuItem55.Name = "toolStripMenuItem55";
			toolStripMenuItem55.Size = new System.Drawing.Size(238, 6);
			cmOnlyFitOversized.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Oversized;
			cmOnlyFitOversized.Name = "cmOnlyFitOversized";
			cmOnlyFitOversized.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			cmOnlyFitOversized.Size = new System.Drawing.Size(241, 22);
			cmOnlyFitOversized.Text = "&Only fit if oversized";
			cmMagnify.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Zoom;
			cmMagnify.Name = "cmMagnify";
			cmMagnify.ShortcutKeys = System.Windows.Forms.Keys.M | System.Windows.Forms.Keys.Control;
			cmMagnify.Size = new System.Drawing.Size(219, 22);
			cmMagnify.Text = "&Magnifier";
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(216, 6);
			cmCopyPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Copy;
			cmCopyPage.Name = "cmCopyPage";
			cmCopyPage.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Control;
			cmCopyPage.Size = new System.Drawing.Size(219, 22);
			cmCopyPage.Text = "&Copy Page";
			cmExportPage.Name = "cmExportPage";
			cmExportPage.ShortcutKeys = System.Windows.Forms.Keys.C | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			cmExportPage.Size = new System.Drawing.Size(219, 22);
			cmExportPage.Text = "&Export Page...";
			toolStripMenuItem11.Name = "toolStripMenuItem11";
			toolStripMenuItem11.Size = new System.Drawing.Size(216, 6);
			cmRefreshPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			cmRefreshPage.Name = "cmRefreshPage";
			cmRefreshPage.ShortcutKeys = System.Windows.Forms.Keys.F5;
			cmRefreshPage.Size = new System.Drawing.Size(219, 22);
			cmRefreshPage.Text = "&Refresh";
			toolStripMenuItem46.Name = "toolStripMenuItem46";
			toolStripMenuItem46.Size = new System.Drawing.Size(216, 6);
			cmMinimalGui.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.MenuToggle;
			cmMinimalGui.Name = "cmMinimalGui";
			cmMinimalGui.ShortcutKeys = System.Windows.Forms.Keys.F10;
			cmMinimalGui.Size = new System.Drawing.Size(219, 22);
			cmMinimalGui.Text = "&Minimal User Interface";
			notifyIcon.ContextMenuStrip = notfifyContextMenu;
			notifyIcon.Text = "Double Click to restore";
			notifyIcon.BalloonTipClicked += new System.EventHandler(notifyIcon_BalloonTipClicked);
			notfifyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
		cmNotifyRestore,
		toolStripMenuItem15,
		cmNotifyExit
			});
			notfifyContextMenu.Name = "notfifyContextMenu";
			notfifyContextMenu.Size = new System.Drawing.Size(114, 54);
			cmNotifyRestore.Name = "cmNotifyRestore";
			cmNotifyRestore.Size = new System.Drawing.Size(113, 22);
			cmNotifyRestore.Text = "&Restore";
			toolStripMenuItem15.Name = "toolStripMenuItem15";
			toolStripMenuItem15.Size = new System.Drawing.Size(110, 6);
			cmNotifyExit.Name = "cmNotifyExit";
			cmNotifyExit.Size = new System.Drawing.Size(113, 22);
			cmNotifyExit.Text = "&Exit";
			viewContainer.Controls.Add(panelReader);
			viewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			viewContainer.Location = new System.Drawing.Point(0, 24);
			viewContainer.Name = "viewContainer";
			viewContainer.Size = new System.Drawing.Size(744, 364);
			viewContainer.TabIndex = 14;
			panelReader.Controls.Add(readerContainer);
			panelReader.Dock = System.Windows.Forms.DockStyle.Fill;
			panelReader.Location = new System.Drawing.Point(0, 0);
			panelReader.Name = "panelReader";
			panelReader.Size = new System.Drawing.Size(744, 364);
			panelReader.TabIndex = 2;
			readerContainer.Controls.Add(quickOpenView);
			readerContainer.Controls.Add(fileTabs);
			readerContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			readerContainer.Location = new System.Drawing.Point(0, 0);
			readerContainer.Name = "readerContainer";
			readerContainer.Size = new System.Drawing.Size(744, 364);
			readerContainer.TabIndex = 0;
			readerContainer.Paint += new System.Windows.Forms.PaintEventHandler(readerContainer_Paint);
			quickOpenView.AllowDrop = true;
			quickOpenView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			quickOpenView.BackColor = System.Drawing.SystemColors.Window;
			quickOpenView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			quickOpenView.Caption = "Quick Open";
			quickOpenView.CaptionMargin = new System.Windows.Forms.Padding(2);
			quickOpenView.Location = new System.Drawing.Point(63, 50);
			quickOpenView.Margin = new System.Windows.Forms.Padding(12);
			quickOpenView.MinimumSize = new System.Drawing.Size(300, 250);
			quickOpenView.Name = "quickOpenView";
			quickOpenView.ShowBrowserCommand = true;
			quickOpenView.Size = new System.Drawing.Size(616, 289);
			quickOpenView.TabIndex = 2;
			quickOpenView.ThumbnailSize = 128;
			quickOpenView.Visible = false;
			quickOpenView.BookActivated += new System.EventHandler(QuickOpenBookActivated);
			quickOpenView.ShowBrowser += new System.EventHandler(quickOpenView_ShowBrowser);
			quickOpenView.OpenFile += new System.EventHandler(quickOpenView_OpenFile);
			quickOpenView.VisibleChanged += new System.EventHandler(QuickOpenVisibleChanged);
			quickOpenView.DragDrop += new System.Windows.Forms.DragEventHandler(BookDragDrop);
			quickOpenView.DragEnter += new System.Windows.Forms.DragEventHandler(BookDragEnter);
			fileTabs.AllowDrop = true;
			fileTabs.CloseImage = cYo.Projects.ComicRack.Viewer.Properties.Resources.Close;
			fileTabs.Controls.Add(mainToolStrip);
			fileTabs.Dock = System.Windows.Forms.DockStyle.Top;
			fileTabs.DragDropReorder = true;
			fileTabs.LeftIndent = 8;
			fileTabs.Location = new System.Drawing.Point(0, 0);
			fileTabs.Name = "fileTabs";
			fileTabs.OwnerDrawnTooltips = true;
			fileTabs.Size = new System.Drawing.Size(744, 31);
			fileTabs.TabIndex = 1;
			mainToolStrip.BackColor = System.Drawing.Color.Transparent;
			mainToolStrip.Dock = System.Windows.Forms.DockStyle.Right;
			mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[12]
			{
		tbPrevPage,
		tbNextPage,
		toolStripSeparator5,
		tbPageLayout,
		tbFit,
		tbZoom,
		tbRotate,
		toolStripSeparator7,
		tbMagnify,
		tbFullScreen,
		toolStripSeparator1,
		tbTools
			});
			mainToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			mainToolStrip.Location = new System.Drawing.Point(400, 1);
			mainToolStrip.MinimumSize = new System.Drawing.Size(0, 24);
			mainToolStrip.Name = "mainToolStrip";
			mainToolStrip.Size = new System.Drawing.Size(344, 25);
			mainToolStrip.TabIndex = 2;
			mainToolStrip.Text = "mainToolStrip";
			tbPrevPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbPrevPage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5]
			{
		tbFirstPage,
		tbPrevBookmark,
		toolStripMenuItem53,
		toolStripMenuItem19,
		tbPrevFromList
			});
			tbPrevPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
			tbPrevPage.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbPrevPage.Name = "tbPrevPage";
			tbPrevPage.Size = new System.Drawing.Size(32, 22);
			tbPrevPage.Text = "Previous Page";
			tbPrevPage.DropDownOpening += new System.EventHandler(tbPrevPage_DropDownOpening);
			tbFirstPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
			tbFirstPage.Name = "tbFirstPage";
			tbFirstPage.ShortcutKeyDisplayString = "";
			tbFirstPage.ShortcutKeys = System.Windows.Forms.Keys.B | System.Windows.Forms.Keys.Control;
			tbFirstPage.Size = new System.Drawing.Size(268, 22);
			tbFirstPage.Text = "&First Page";
			tbPrevBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.PreviousBookmark;
			tbPrevBookmark.Name = "tbPrevBookmark";
			tbPrevBookmark.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbPrevBookmark.Size = new System.Drawing.Size(268, 22);
			tbPrevBookmark.Text = "Previous Bookmark";
			toolStripMenuItem53.Name = "toolStripMenuItem53";
			toolStripMenuItem53.Size = new System.Drawing.Size(265, 6);
			toolStripMenuItem53.Tag = "bms";
			toolStripMenuItem19.Name = "toolStripMenuItem19";
			toolStripMenuItem19.Size = new System.Drawing.Size(265, 6);
			tbPrevFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.PrevFromList;
			tbPrevFromList.Name = "tbPrevFromList";
			tbPrevFromList.ShortcutKeys = System.Windows.Forms.Keys.P | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			tbPrevFromList.Size = new System.Drawing.Size(268, 22);
			tbPrevFromList.Text = "Previous Book from List";
			tbNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbNextPage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[7]
			{
		tbLastPage,
		tbNextBookmark,
		tbLastPageRead,
		toolStripMenuItem28,
		toolStripMenuItem49,
		tbNextFromList,
		tbRandomFromList
			});
			tbNextPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
			tbNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbNextPage.Name = "tbNextPage";
			tbNextPage.Size = new System.Drawing.Size(32, 22);
			tbNextPage.Text = "Next Page";
			tbNextPage.DropDownOpening += new System.EventHandler(tbNextPage_DropDownOpening);
			tbLastPage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
			tbLastPage.Name = "tbLastPage";
			tbLastPage.ShortcutKeyDisplayString = "";
			tbLastPage.ShortcutKeys = System.Windows.Forms.Keys.E | System.Windows.Forms.Keys.Control;
			tbLastPage.Size = new System.Drawing.Size(249, 22);
			tbLastPage.Text = "&Last Page";
			tbNextBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NextBookmark;
			tbNextBookmark.Name = "tbNextBookmark";
			tbNextBookmark.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbNextBookmark.Size = new System.Drawing.Size(249, 22);
			tbNextBookmark.Text = "Next Bookmark";
			tbLastPageRead.Name = "tbLastPageRead";
			tbLastPageRead.ShortcutKeys = System.Windows.Forms.Keys.L | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbLastPageRead.Size = new System.Drawing.Size(249, 22);
			tbLastPageRead.Text = "L&ast Page Read";
			toolStripMenuItem28.Name = "toolStripMenuItem28";
			toolStripMenuItem28.Size = new System.Drawing.Size(246, 6);
			toolStripMenuItem28.Tag = "bms";
			toolStripMenuItem49.Name = "toolStripMenuItem49";
			toolStripMenuItem49.Size = new System.Drawing.Size(246, 6);
			tbNextFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NextFromList;
			tbNextFromList.Name = "tbNextFromList";
			tbNextFromList.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Alt;
			tbNextFromList.Size = new System.Drawing.Size(249, 22);
			tbNextFromList.Text = "Next Book from List";
			tbRandomFromList.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RandomComic;
			tbRandomFromList.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbRandomFromList.Name = "tbRandomFromList";
			tbRandomFromList.ShortcutKeys = System.Windows.Forms.Keys.N | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			tbRandomFromList.Size = new System.Drawing.Size(249, 22);
			tbRandomFromList.Text = "Random Book";
			toolStripSeparator5.Name = "toolStripSeparator5";
			toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			tbPageLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbPageLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5]
			{
		tbSinglePage,
		tbTwoPages,
		tbTwoPagesAdaptive,
		toolStripMenuItem54,
		tbRightToLeft
			});
			tbPageLayout.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			tbPageLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbPageLayout.Name = "tbPageLayout";
			tbPageLayout.Size = new System.Drawing.Size(32, 22);
			tbPageLayout.Text = "Page Layout";
			tbSinglePage.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			tbSinglePage.Name = "tbSinglePage";
			tbSinglePage.ShortcutKeys = System.Windows.Forms.Keys.D7 | System.Windows.Forms.Keys.Control;
			tbSinglePage.Size = new System.Drawing.Size(226, 22);
			tbSinglePage.Text = "Single Page";
			tbTwoPages.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageForced;
			tbTwoPages.Name = "tbTwoPages";
			tbTwoPages.ShortcutKeys = System.Windows.Forms.Keys.D8 | System.Windows.Forms.Keys.Control;
			tbTwoPages.Size = new System.Drawing.Size(226, 22);
			tbTwoPages.Text = "Two Pages";
			tbTwoPagesAdaptive.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
			tbTwoPagesAdaptive.Name = "tbTwoPagesAdaptive";
			tbTwoPagesAdaptive.ShortcutKeys = System.Windows.Forms.Keys.D9 | System.Windows.Forms.Keys.Control;
			tbTwoPagesAdaptive.Size = new System.Drawing.Size(226, 22);
			tbTwoPagesAdaptive.Text = "Two Pages (adaptive)";
			tbTwoPagesAdaptive.ToolTipText = "Show one or two pages";
			toolStripMenuItem54.Name = "toolStripMenuItem54";
			toolStripMenuItem54.Size = new System.Drawing.Size(223, 6);
			tbRightToLeft.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RightToLeft;
			tbRightToLeft.Name = "tbRightToLeft";
			tbRightToLeft.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Control;
			tbRightToLeft.Size = new System.Drawing.Size(226, 22);
			tbRightToLeft.Text = "Right to Left";
			tbFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbFit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[8]
			{
		tbOriginal,
		tbFitAll,
		tbFitWidth,
		tbFitWidthAdaptive,
		tbFitHeight,
		tbBestFit,
		toolStripMenuItem20,
		tbOnlyFitOversized
			});
			tbFit.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			tbFit.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbFit.Name = "tbFit";
			tbFit.Size = new System.Drawing.Size(32, 22);
			tbFit.Text = "Fit";
			tbFit.ToolTipText = "Toggle Fit Mode";
			tbOriginal.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Original;
			tbOriginal.Name = "tbOriginal";
			tbOriginal.ShortcutKeys = System.Windows.Forms.Keys.D1 | System.Windows.Forms.Keys.Control;
			tbOriginal.Size = new System.Drawing.Size(247, 22);
			tbOriginal.Text = "Original Size";
			tbFitAll.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			tbFitAll.Name = "tbFitAll";
			tbFitAll.ShortcutKeys = System.Windows.Forms.Keys.D2 | System.Windows.Forms.Keys.Control;
			tbFitAll.Size = new System.Drawing.Size(247, 22);
			tbFitAll.Text = "Fit All";
			tbFitWidth.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidth;
			tbFitWidth.Name = "tbFitWidth";
			tbFitWidth.ShortcutKeys = System.Windows.Forms.Keys.D3 | System.Windows.Forms.Keys.Control;
			tbFitWidth.Size = new System.Drawing.Size(247, 22);
			tbFitWidth.Text = "Fit Width";
			tbFitWidthAdaptive.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidthAdaptive;
			tbFitWidthAdaptive.Name = "tbFitWidthAdaptive";
			tbFitWidthAdaptive.ShortcutKeys = System.Windows.Forms.Keys.D4 | System.Windows.Forms.Keys.Control;
			tbFitWidthAdaptive.Size = new System.Drawing.Size(247, 22);
			tbFitWidthAdaptive.Text = "Fit Width (adaptive)";
			tbFitHeight.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitHeight;
			tbFitHeight.Name = "tbFitHeight";
			tbFitHeight.ShortcutKeys = System.Windows.Forms.Keys.D5 | System.Windows.Forms.Keys.Control;
			tbFitHeight.Size = new System.Drawing.Size(247, 22);
			tbFitHeight.Text = "Fit Height";
			tbBestFit.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FitBest;
			tbBestFit.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbBestFit.Name = "tbBestFit";
			tbBestFit.ShortcutKeys = System.Windows.Forms.Keys.D6 | System.Windows.Forms.Keys.Control;
			tbBestFit.Size = new System.Drawing.Size(247, 22);
			tbBestFit.Text = "Fit Best";
			toolStripMenuItem20.Name = "toolStripMenuItem20";
			toolStripMenuItem20.Size = new System.Drawing.Size(244, 6);
			tbOnlyFitOversized.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Oversized;
			tbOnlyFitOversized.Name = "tbOnlyFitOversized";
			tbOnlyFitOversized.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbOnlyFitOversized.Size = new System.Drawing.Size(247, 22);
			tbOnlyFitOversized.Text = "&Only fit if oversized";
			tbZoom.AutoToolTip = false;
			tbZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[10]
			{
		tbZoomIn,
		tbZoomOut,
		toolStripMenuItem30,
		tbZoom100,
		tbZoom125,
		tbZoom150,
		tbZoom200,
		tbZoom400,
		toolStripMenuItem31,
		tbZoomCustom
			});
			tbZoom.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomIn;
			tbZoom.Name = "tbZoom";
			tbZoom.Size = new System.Drawing.Size(70, 22);
			tbZoom.Text = "100 %";
			tbZoom.ToolTipText = "Change the page zoom";
			tbZoomIn.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomIn;
			tbZoomIn.Name = "tbZoomIn";
			tbZoomIn.ShortcutKeys = System.Windows.Forms.Keys.Oemplus | System.Windows.Forms.Keys.Control;
			tbZoomIn.Size = new System.Drawing.Size(222, 22);
			tbZoomIn.Text = "Zoom &In";
			tbZoomOut.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomOut;
			tbZoomOut.Name = "tbZoomOut";
			tbZoomOut.ShortcutKeys = System.Windows.Forms.Keys.OemMinus | System.Windows.Forms.Keys.Control;
			tbZoomOut.Size = new System.Drawing.Size(222, 22);
			tbZoomOut.Text = "Zoom &Out";
			toolStripMenuItem30.Name = "toolStripMenuItem30";
			toolStripMenuItem30.Size = new System.Drawing.Size(219, 6);
			tbZoom100.Name = "tbZoom100";
			tbZoom100.Size = new System.Drawing.Size(222, 22);
			tbZoom100.Text = "100%";
			tbZoom125.Name = "tbZoom125";
			tbZoom125.Size = new System.Drawing.Size(222, 22);
			tbZoom125.Text = "125%";
			tbZoom150.Name = "tbZoom150";
			tbZoom150.Size = new System.Drawing.Size(222, 22);
			tbZoom150.Text = "150%";
			tbZoom200.Name = "tbZoom200";
			tbZoom200.Size = new System.Drawing.Size(222, 22);
			tbZoom200.Text = "200%";
			tbZoom400.Name = "tbZoom400";
			tbZoom400.Size = new System.Drawing.Size(222, 22);
			tbZoom400.Text = "400%";
			toolStripMenuItem31.Name = "toolStripMenuItem31";
			toolStripMenuItem31.Size = new System.Drawing.Size(219, 6);
			tbZoomCustom.Name = "tbZoomCustom";
			tbZoomCustom.ShortcutKeys = System.Windows.Forms.Keys.Z | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbZoomCustom.Size = new System.Drawing.Size(222, 22);
			tbZoomCustom.Text = "&Custom...";
			tbRotate.AutoToolTip = false;
			tbRotate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9]
			{
		tbRotateLeft,
		tbRotateRight,
		toolStripSeparator11,
		tbRotate0,
		tbRotate90,
		tbRotate180,
		tbRotate270,
		toolStripMenuItem34,
		tbAutoRotate
			});
			tbRotate.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateRight;
			tbRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbRotate.Name = "tbRotate";
			tbRotate.Size = new System.Drawing.Size(50, 22);
			tbRotate.Text = "0°";
			tbRotate.ToolTipText = "Change the page rotation";
			tbRotateLeft.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateLeft;
			tbRotateLeft.Name = "tbRotateLeft";
			tbRotateLeft.ShortcutKeys = System.Windows.Forms.Keys.OemMinus | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbRotateLeft.Size = new System.Drawing.Size(256, 22);
			tbRotateLeft.Text = "Rotate Left";
			tbRotateRight.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateRight;
			tbRotateRight.Name = "tbRotateRight";
			tbRotateRight.ShortcutKeys = System.Windows.Forms.Keys.Oemplus | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbRotateRight.Size = new System.Drawing.Size(256, 22);
			tbRotateRight.Text = "Rotate Right";
			toolStripSeparator11.Name = "toolStripSeparator11";
			toolStripSeparator11.Size = new System.Drawing.Size(253, 6);
			tbRotate0.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate0;
			tbRotate0.Name = "tbRotate0";
			tbRotate0.ShortcutKeys = System.Windows.Forms.Keys.D7 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbRotate0.Size = new System.Drawing.Size(256, 22);
			tbRotate0.Text = "&No Rotation";
			tbRotate90.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate90;
			tbRotate90.Name = "tbRotate90";
			tbRotate90.ShortcutKeys = System.Windows.Forms.Keys.D8 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbRotate90.Size = new System.Drawing.Size(256, 22);
			tbRotate90.Text = "90°";
			tbRotate180.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate180;
			tbRotate180.Name = "tbRotate180";
			tbRotate180.ShortcutKeys = System.Windows.Forms.Keys.D9 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbRotate180.Size = new System.Drawing.Size(256, 22);
			tbRotate180.Text = "180°";
			tbRotate270.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate270;
			tbRotate270.Name = "tbRotate270";
			tbRotate270.ShortcutKeys = System.Windows.Forms.Keys.D0 | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbRotate270.Size = new System.Drawing.Size(256, 22);
			tbRotate270.Text = "270°";
			toolStripMenuItem34.Name = "toolStripMenuItem34";
			toolStripMenuItem34.Size = new System.Drawing.Size(253, 6);
			tbAutoRotate.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.AutoRotate;
			tbAutoRotate.Name = "tbAutoRotate";
			tbAutoRotate.Size = new System.Drawing.Size(256, 22);
			tbAutoRotate.Text = "Autorotate Double Pages";
			toolStripSeparator7.Name = "toolStripSeparator7";
			toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			tbMagnify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbMagnify.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Zoom;
			tbMagnify.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbMagnify.Name = "tbMagnify";
			tbMagnify.Size = new System.Drawing.Size(32, 22);
			tbMagnify.Text = "Magnifier";
			tbFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbFullScreen.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.FullScreen;
			tbFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbFullScreen.Name = "tbFullScreen";
			tbFullScreen.Size = new System.Drawing.Size(23, 22);
			tbFullScreen.Text = "Full Screen";
			tbFullScreen.ToolTipText = "Full Screen";
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			tbTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbTools.DropDown = toolsContextMenu;
			tbTools.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Tools;
			tbTools.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbTools.Name = "tbTools";
			tbTools.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			tbTools.ShowDropDownArrow = false;
			tbTools.Size = new System.Drawing.Size(20, 22);
			tbTools.Text = "Tools";
			tbTools.DropDownOpening += new System.EventHandler(tbTools_DropDownOpening);
			toolsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[23]
			{
		tbOpenComic,
		tbOpenRemoteLibrary,
		tbShowInfo,
		toolStripMenuItem47,
		tsWorkspaces,
		tbBookmarks,
		tbAutoScroll,
		toolStripMenuItem45,
		tbMinimalGui,
		tbReaderUndocked,
		toolStripMenuItem52,
		tbScan,
		tbUpdateAllComicFiles,
		tbUpdateWebComics,
		tsSynchronizeDevices,
		toolStripMenuItem48,
		tbComicDisplaySettings,
		tbPreferences,
        tbAbout,
		toolStripMenuItem50,
		tbShowMainMenu,
		toolStripMenuItem51,
		tbExit
			});
			toolsContextMenu.Name = "toolsContextMenu";
			toolsContextMenu.OwnerItem = tbTools;
			toolsContextMenu.Size = new System.Drawing.Size(285, 724);
			tbOpenComic.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			tbOpenComic.Name = "tbOpenComic";
			tbOpenComic.ShortcutKeys = System.Windows.Forms.Keys.O | System.Windows.Forms.Keys.Control;
			tbOpenComic.Size = new System.Drawing.Size(284, 38);
			tbOpenComic.Text = "&Open Book...";
			tbOpenRemoteLibrary.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoteDatabase;
			tbOpenRemoteLibrary.Name = "tbOpenRemoteLibrary";
			tbOpenRemoteLibrary.ShortcutKeys = System.Windows.Forms.Keys.R | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbOpenRemoteLibrary.Size = new System.Drawing.Size(284, 38);
			tbOpenRemoteLibrary.Text = "Open Remote Library...";
			tbShowInfo.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GetInfo;
			tbShowInfo.Name = "tbShowInfo";
			tbShowInfo.ShortcutKeys = System.Windows.Forms.Keys.I | System.Windows.Forms.Keys.Control;
			tbShowInfo.Size = new System.Drawing.Size(284, 38);
			tbShowInfo.Text = "Info...";
			tbShowInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			toolStripMenuItem47.Name = "toolStripMenuItem47";
			toolStripMenuItem47.Size = new System.Drawing.Size(281, 6);
			tsWorkspaces.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
		tsSaveWorkspace,
		tsEditWorkspaces,
		tsWorkspaceSep
			});
			tsWorkspaces.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Workspace;
			tsWorkspaces.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsWorkspaces.Name = "tsWorkspaces";
			tsWorkspaces.ShortcutKeys = System.Windows.Forms.Keys.W | System.Windows.Forms.Keys.Control;
			tsWorkspaces.Size = new System.Drawing.Size(284, 38);
			tsWorkspaces.Text = "Workspaces";
			tsSaveWorkspace.Name = "tsSaveWorkspace";
			tsSaveWorkspace.ShortcutKeys = System.Windows.Forms.Keys.W | System.Windows.Forms.Keys.Control;
			tsSaveWorkspace.Size = new System.Drawing.Size(237, 22);
			tsSaveWorkspace.Text = "&Save Workspace...";
			tsEditWorkspaces.Name = "tsEditWorkspaces";
			tsEditWorkspaces.ShortcutKeys = System.Windows.Forms.Keys.W | System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt;
			tsEditWorkspaces.Size = new System.Drawing.Size(237, 22);
			tsEditWorkspaces.Text = "&Edit Workspaces...";
			tsWorkspaceSep.Name = "tsWorkspaceSep";
			tsWorkspaceSep.Size = new System.Drawing.Size(234, 6);
			tbBookmarks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
		tbSetBookmark,
		tbRemoveBookmark,
		tbBookmarkSeparator
			});
			tbBookmarks.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Bookmark;
			tbBookmarks.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbBookmarks.Name = "tbBookmarks";
			tbBookmarks.Size = new System.Drawing.Size(284, 38);
			tbBookmarks.Text = "Bookmarks";
			tbBookmarks.DropDownOpening += new System.EventHandler(tbBookmarks_DropDownOpening);
			tbSetBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewBookmark;
			tbSetBookmark.Name = "tbSetBookmark";
			tbSetBookmark.ShortcutKeys = System.Windows.Forms.Keys.B | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbSetBookmark.Size = new System.Drawing.Size(248, 22);
			tbSetBookmark.Text = "Set Bookmark...";
			tbRemoveBookmark.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoveBookmark;
			tbRemoveBookmark.Name = "tbRemoveBookmark";
			tbRemoveBookmark.ShortcutKeys = System.Windows.Forms.Keys.D | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbRemoveBookmark.Size = new System.Drawing.Size(248, 22);
			tbRemoveBookmark.Text = "Remove Bookmark";
			tbBookmarkSeparator.Name = "tbBookmarkSeparator";
			tbBookmarkSeparator.Size = new System.Drawing.Size(245, 6);
			tbBookmarkSeparator.Tag = "bms";
			tbAutoScroll.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.CursorScroll;
			tbAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbAutoScroll.Name = "tbAutoScroll";
			tbAutoScroll.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Control;
			tbAutoScroll.Size = new System.Drawing.Size(284, 38);
			tbAutoScroll.Text = "Auto Scrolling";
			toolStripMenuItem45.Name = "toolStripMenuItem45";
			toolStripMenuItem45.Size = new System.Drawing.Size(281, 6);
			tbMinimalGui.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.MenuToggle;
			tbMinimalGui.Name = "tbMinimalGui";
			tbMinimalGui.ShortcutKeys = System.Windows.Forms.Keys.F10;
			tbMinimalGui.Size = new System.Drawing.Size(284, 38);
			tbMinimalGui.Text = "Minimal User Interface";
			tbReaderUndocked.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.UndockReader;
			tbReaderUndocked.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbReaderUndocked.Name = "tbReaderUndocked";
			tbReaderUndocked.ShortcutKeys = System.Windows.Forms.Keys.F12;
			tbReaderUndocked.Size = new System.Drawing.Size(284, 38);
			tbReaderUndocked.Text = "Reader in own Window";
			toolStripMenuItem52.Name = "toolStripMenuItem52";
			toolStripMenuItem52.Size = new System.Drawing.Size(281, 6);
			tbScan.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Scan;
			tbScan.Name = "tbScan";
			tbScan.ShortcutKeys = System.Windows.Forms.Keys.S | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbScan.Size = new System.Drawing.Size(284, 38);
			tbScan.Text = "Scan Book &Folders";
			tbUpdateAllComicFiles.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateSmall;
			tbUpdateAllComicFiles.Name = "tbUpdateAllComicFiles";
			tbUpdateAllComicFiles.ShortcutKeys = System.Windows.Forms.Keys.U | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbUpdateAllComicFiles.Size = new System.Drawing.Size(284, 38);
			tbUpdateAllComicFiles.Text = "Update all Book Files";
			tbUpdateWebComics.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateWeb;
			tbUpdateWebComics.Name = "tbUpdateWebComics";
			tbUpdateWebComics.ShortcutKeys = System.Windows.Forms.Keys.W | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.Control;
			tbUpdateWebComics.Size = new System.Drawing.Size(284, 38);
			tbUpdateWebComics.Text = "Update Web Comics";
			tsSynchronizeDevices.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DeviceSync;
			tsSynchronizeDevices.Name = "tsSynchronizeDevices";
			tsSynchronizeDevices.Size = new System.Drawing.Size(284, 38);
			tsSynchronizeDevices.Text = "Synchronize Devices";
			toolStripMenuItem48.Name = "toolStripMenuItem48";
			toolStripMenuItem48.Size = new System.Drawing.Size(281, 6);
			tbComicDisplaySettings.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DisplaySettings;
			tbComicDisplaySettings.Name = "tbComicDisplaySettings";
			tbComicDisplaySettings.ShortcutKeys = System.Windows.Forms.Keys.F9;
			tbComicDisplaySettings.Size = new System.Drawing.Size(284, 38);
			tbComicDisplaySettings.Text = "Book Display Settings...";
			tbPreferences.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Preferences;
			tbPreferences.Name = "tbPreferences";
			tbPreferences.ShortcutKeys = System.Windows.Forms.Keys.F9 | System.Windows.Forms.Keys.Control;
			tbPreferences.Size = new System.Drawing.Size(284, 38);
			tbPreferences.Text = "&Preferences...";
			tbAbout.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.About;
			tbAbout.Name = "tbAbout";
			tbAbout.ShortcutKeys = System.Windows.Forms.Keys.F1 | System.Windows.Forms.Keys.Alt;
			tbAbout.Size = new System.Drawing.Size(284, 38);
			tbAbout.Text = "&About...";
			toolStripMenuItem50.Name = "toolStripMenuItem50";
			toolStripMenuItem50.Size = new System.Drawing.Size(281, 6);
			tbShowMainMenu.Name = "tbShowMainMenu";
			tbShowMainMenu.ShortcutKeys = System.Windows.Forms.Keys.F10 | System.Windows.Forms.Keys.Shift;
			tbShowMainMenu.Size = new System.Drawing.Size(284, 38);
			tbShowMainMenu.Text = "Show Main Menu";
			toolStripMenuItem51.Name = "toolStripMenuItem51";
			toolStripMenuItem51.Size = new System.Drawing.Size(281, 6);
			tbExit.Name = "tbExit";
			tbExit.ShortcutKeys = System.Windows.Forms.Keys.Q | System.Windows.Forms.Keys.Control;
			tbExit.Size = new System.Drawing.Size(284, 38);
			tbExit.Text = "&Exit";
			tabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[8]
			{
		cmClose,
		cmCloseAllButThis,
		cmCloseAllToTheRight,
		toolStripMenuItem35,
		cmSyncBrowser,
		sepBeforeRevealInBrowser,
		cmRevealInExplorer,
		cmCopyPath
			});
			tabContextMenu.Name = "tabContextMenu";
			tabContextMenu.Size = new System.Drawing.Size(237, 244);
			tabContextMenu.Opening += new System.ComponentModel.CancelEventHandler(tabContextMenu_Opening);
			cmClose.Name = "cmClose";
			cmClose.ShortcutKeys = System.Windows.Forms.Keys.X | System.Windows.Forms.Keys.Control;
			cmClose.Size = new System.Drawing.Size(236, 38);
			cmClose.Text = "Close";
			cmCloseAllButThis.Name = "cmCloseAllButThis";
			cmCloseAllButThis.Size = new System.Drawing.Size(236, 38);
			cmCloseAllButThis.Text = "Close All But This";
			cmCloseAllToTheRight.Name = "cmCloseAllToTheRight";
			cmCloseAllToTheRight.Size = new System.Drawing.Size(236, 38);
			cmCloseAllToTheRight.Text = "Close All to the Right";
			toolStripMenuItem35.Name = "toolStripMenuItem35";
			toolStripMenuItem35.Size = new System.Drawing.Size(233, 6);
			cmSyncBrowser.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SyncBrowser;
			cmSyncBrowser.Name = "cmSyncBrowser";
			cmSyncBrowser.ShortcutKeys = System.Windows.Forms.Keys.F3 | System.Windows.Forms.Keys.Control;
			cmSyncBrowser.Size = new System.Drawing.Size(236, 38);
			cmSyncBrowser.Text = "Show in &Browser";
			sepBeforeRevealInBrowser.Name = "sepBeforeRevealInBrowser";
			sepBeforeRevealInBrowser.Size = new System.Drawing.Size(233, 6);
			cmRevealInExplorer.Name = "cmRevealInExplorer";
			cmRevealInExplorer.ShortcutKeys = System.Windows.Forms.Keys.G | System.Windows.Forms.Keys.Control;
			cmRevealInExplorer.Size = new System.Drawing.Size(236, 38);
			cmRevealInExplorer.Text = "Reveal in Explorer";
			cmCopyPath.Name = "cmCopyPath";
			cmCopyPath.Size = new System.Drawing.Size(236, 38);
			cmCopyPath.Text = "Copy Full Path to Clipboard";
			trimTimer.Enabled = true;
			trimTimer.Interval = 5000;
			trimTimer.Tick += new System.EventHandler(trimTimer_Tick);
			mainViewContainer.AutoGripPosition = true;
			mainViewContainer.BackColor = System.Drawing.SystemColors.Control;
			mainViewContainer.Controls.Add(mainView);
			mainViewContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			mainViewContainer.Location = new System.Drawing.Point(0, 388);
			mainViewContainer.Name = "mainViewContainer";
			mainViewContainer.Size = new System.Drawing.Size(744, 250);
			mainViewContainer.TabIndex = 2;
			mainViewContainer.ExpandedChanged += new System.EventHandler(mainViewContainer_ExpandedChanged);
			mainViewContainer.DockChanged += new System.EventHandler(mainViewContainer_DockChanged);
			mainView.BackColor = System.Drawing.Color.Transparent;
			mainView.Caption = "";
			mainView.CaptionMargin = new System.Windows.Forms.Padding(2);
			mainView.Dock = System.Windows.Forms.DockStyle.Fill;
			mainView.InfoPanelRight = false;
			mainView.Location = new System.Drawing.Point(0, 6);
			mainView.Margin = new System.Windows.Forms.Padding(6);
			mainView.Name = "mainView";
			mainView.Size = new System.Drawing.Size(744, 244);
			mainView.TabBarVisible = true;
			mainView.TabIndex = 0;
			mainView.TabChanged += new System.EventHandler(mainView_TabChanged);
			updateActivityTimer.Enabled = true;
			updateActivityTimer.Interval = 1000;
			updateActivityTimer.Tick += new System.EventHandler(UpdateActivityTimerTick);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(744, 662);
			base.Controls.Add(viewContainer);
			base.Controls.Add(mainViewContainer);
			base.Controls.Add(statusStrip);
			base.Controls.Add(mainMenuStrip);
			base.KeyPreview = true;
			base.Name = "MainForm";
			Text = "ComicRack";
			mainMenuStrip.ResumeLayout(false);
			mainMenuStrip.PerformLayout();
			contextRating.ResumeLayout(false);
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			pageContextMenu.ResumeLayout(false);
			contextRating2.ResumeLayout(false);
			notfifyContextMenu.ResumeLayout(false);
			viewContainer.ResumeLayout(false);
			panelReader.ResumeLayout(false);
			readerContainer.ResumeLayout(false);
			readerContainer.PerformLayout();
			fileTabs.ResumeLayout(false);
			fileTabs.PerformLayout();
			mainToolStrip.ResumeLayout(false);
			mainToolStrip.PerformLayout();
			toolsContextMenu.ResumeLayout(false);
			tabContextMenu.ResumeLayout(false);
			mainViewContainer.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		private ComicDisplay comicDisplay;

		private readonly NavigatorManager books;

		private IContainer components;

		private Timer mouseDisableTimer;

		private MenuStrip mainMenuStrip;

		private ToolStripMenuItem fileMenu;

		private ToolStripMenuItem miOpenComic;

		private ToolStripSeparator toolStripMenuItem14;

		private ToolStripMenuItem miAddFolderToLibrary;

		private ToolStripMenuItem miScan;

		private ToolStripSeparator toolStripInsertSeperator;

		private ToolStripMenuItem miOpenRecent;

		private ToolStripSeparator toolStripMenuItem4;

		private ToolStripMenuItem miExit;

		private ToolStripMenuItem editMenu;

		private ToolStripMenuItem miShowInfo;

		private ToolStripMenuItem miRating;

		private ToolStripSeparator toolStripSeparator4;

		private ToolStripMenuItem miPreferences;

		private ToolStripMenuItem readMenu;

		private ToolStripMenuItem miFirstPage;

		private ToolStripMenuItem miPrevPage;

		private ToolStripMenuItem miNextPage;

		private ToolStripMenuItem miLastPage;

		private ToolStripSeparator toolStripMenuItem1;

		private ToolStripMenuItem miPrevFromList;

		private ToolStripMenuItem miNextFromList;

		private ToolStripMenuItem displayMenu;

		private ToolStripSeparator toolStripSeparator3;

		private ToolStripSeparator toolStripMenuItem41;

		private ToolStripMenuItem helpMenu;

		private ToolStripMenuItem miWebHome;

		private ToolStripMenuItem miWebUserForum;

		private ToolStripSeparator toolStripMenuItem5;

		private ToolStripMenuItem miAbout;

		private StatusStrip statusStrip;

		private ToolStripStatusLabel tsText;

		private ToolStripStatusLabel tsScanActivity;

		private ToolStripStatusLabel tsBook;

		private ToolStripStatusLabel tsCurrentPage;

		private ToolStripStatusLabel tsPageCount;

		private MainView mainView;

		private ToolStripMenuItem miZoom;

		private ToolStripMenuItem miRotation;

		private ToolStripMenuItem miRotate0;

		private ToolStripMenuItem miRotate90;

		private ToolStripMenuItem miRotate180;

		private ToolStripMenuItem miRotate270;

		private ContextMenuStrip pageContextMenu;

		private ToolStripMenuItem cmShowInfo;

		private ToolStripMenuItem cmRating;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem cmCopyPage;

		private ToolStripSeparator toolStripSeparator10;

		private ToolStripMenuItem cmRefreshPage;

		private NotifyIcon notifyIcon;

		private ToolStripMenuItem miNews;

		private ToolStripMenuItem miMagnify;

		private ToolStripMenuItem cmMagnify;

		private ToolStripMenuItem miFileAutomation;

		private ToolStripSeparator toolStripMenuItem40;

		private ToolStripMenuItem miViewRefresh;

		private ToolStripMenuItem browseMenu;

		private ToolStripMenuItem miViewLibrary;

		private ToolStripMenuItem miViewFolders;

		private ToolStripMenuItem miViewPages;

		private ToolStripSeparator toolStripMenuItem9;

		private ToolStripMenuItem miSidebar;

		private ToolStripMenuItem miSmallPreview;

		private ToolStripMenuItem miSearchBrowser;

		private ToolStripMenuItem miFullScreen;

		private ToolStripMenuItem miToggleBrowser;

		private ToolStripSeparator toolStripMenuItem2;

		private ToolStripStatusLabel tsWriteInfoActivity;

		private ToolStripStatusLabel tsExportActivity;

		private ToolStripMenuItem cmPageType;

		private ToolStripMenuItem miSyncBrowser;

		private SizableContainer mainViewContainer;

		private ToolStripSeparator toolStripMenuItem11;

		private ContextMenuStrip notfifyContextMenu;

		private ToolStripMenuItem cmNotifyRestore;

		private ToolStripSeparator toolStripMenuItem15;

		private ToolStripMenuItem cmNotifyExit;

		private TabBar fileTabs;

		private ToolStripMenuItem miCloseComic;

		private ToolStripMenuItem miCloseAllComics;

		private ToolStripSeparator toolStripMenuItem7;

		private ToolStripMenuItem miAddTab;

		private Panel viewContainer;

		private ToolStripMenuItem miOpenNow;

		private ToolStripSeparator toolStripMenuItem17;

		private ToolStripMenuItem miPrevTab;

		private ToolStripMenuItem miNextTab;

		private ToolStripStatusLabel tsPageActivity;

		private ToolStripMenuItem cmExportPage;

		private ToolStripSeparator toolStripMenuItem6;

		private ToolStripMenuItem miListLayouts;

		private ToolStripMenuItem miEditListLayout;

		private ToolStripMenuItem miSaveListLayout;

		private ToolStripMenuItem miEditLayouts;

		private ToolStripMenuItem miSetAllListsSame;

		private ToolStripSeparator miLayoutSep;

		private ToolStripMenuItem cmPageLayout;

		private ToolStripMenuItem cmFitAll;

		private ToolStripMenuItem cmFitWidth;

		private ToolStripMenuItem cmFitHeight;

		private ToolStripMenuItem cmFitBest;

		private ToolStripMenuItem miRestart;

		private ToolStripSeparator toolStripMenuItem24;

		private ToolStripSeparator toolStripMenuItem25;

		private ToolStripMenuItem miReaderUndocked;

		private Panel panelReader;

		private Panel readerContainer;

		private ToolStripMenuItem miPageLayout;

		private ToolStripMenuItem miFitAll;

		private ToolStripMenuItem miFitWidth;

		private ToolStripMenuItem miFitHeight;

		private ToolStripMenuItem miBestFit;

		private ToolStripSeparator toolStripMenuItem29;

		private ToolStripMenuItem miWorkspaces;

		private ToolStripMenuItem miSaveWorkspace;

		private ToolStripMenuItem miEditWorkspaces;

		private ToolStripSeparator miWorkspaceSep;

		private ToolStripMenuItem miOriginal;

		private ToolStripMenuItem cmOriginal;

		private ToolStripMenuItem cmFitWidthAdaptive;

		private ToolStripMenuItem miFitWidthAdaptive;

		private ToolStripMenuItem miOnlyFitOversized;

		private ToolStripMenuItem cmOnlyFitOversized;

		private ToolStripMenuItem miAutoScroll;

		private ToolStripMenuItem miRightToLeft;

		private ToolStripMenuItem miDoublePageAutoScroll;

		private ToolStripMenuItem cmRightToLeft;

		private ToolStripSeparator toolStripMenuItem27;

		private ToolStripMenuItem miMinimalGui;

		private ToolStripMenuItem cmMinimalGui;

		private ToolStripMenuItem miWebHelp;

		private ToolStripSeparator toolStripMenuItem3;

		private ToolStripSeparator toolStripMenuItem18;

		private ToolStripMenuItem cmBookmarks;

		private ToolStripMenuItem cmSetBookmark;

		private ToolStripMenuItem cmRemoveBookmark;

		private ToolStripSeparator toolStripSeparator13;

		private ToolStripMenuItem cmLastPageRead;

		private ToolStripSeparator toolStripMenuItem23;

		private ToolStripSeparator toolStripMenuItem32;

		private ToolStripMenuItem cmPrevBookmark;

		private ToolStripMenuItem cmNextBookmark;

		private ToolStripMenuItem miZoomIn;

		private ToolStripMenuItem miZoomOut;

		private ToolStripSeparator toolStripSeparator14;

		private ToolStripMenuItem miZoom100;

		private ToolStripMenuItem miZoom125;

		private ToolStripMenuItem miZoom150;

		private ToolStripMenuItem miZoom200;

		private ToolStripMenuItem miZoom400;

		private ToolStripSeparator toolStripSeparator15;

		private ToolStripMenuItem miZoomCustom;

		private ToolStripMenuItem miRotateLeft;

		private ToolStripMenuItem miRotateRight;

		private ToolStripSeparator toolStripMenuItem33;

		private ToolStripSeparator toolStripMenuItem36;

		private ToolStripMenuItem miAutoRotate;

		private ContextMenuStrip tabContextMenu;

		private ToolStripMenuItem cmClose;

		private ToolStripMenuItem cmCloseAllButThis;

		private ToolStripSeparator toolStripMenuItem35;

		private ToolStripMenuItem cmCopyPath;

		private ToolStripMenuItem cmRevealInExplorer;

		private ToolStripMenuItem cmSyncBrowser;

		private ToolStripMenuItem miTasks;

		private ToolStripStatusLabel tsReadInfoActivity;

		private ToolStripSeparator toolStripMenuItem22;

		private ToolStripMenuItem miPageType;

		private ToolStripMenuItem miBookmarks;

		private ToolStripMenuItem miSetBookmark;

		private ToolStripMenuItem miRemoveBookmark;

		private ToolStripSeparator toolStripMenuItem26;

		private ToolStripMenuItem miPrevBookmark;

		private ToolStripMenuItem miNextBookmark;

		private ToolStripSeparator toolStripMenuItem8;

		private ToolStripMenuItem miLastPageRead;

		private ToolStripMenuItem miCopyPage;

		private ToolStripMenuItem miExportPage;

		private ToolStripSeparator toolStripMenuItem39;

		private ToolStripSeparator sepBeforeRevealInBrowser;

		private ToolStripMenuItem cmComics;

		private ToolStripMenuItem cmOpenComic;

		private ToolStripMenuItem cmCloseComic;

		private ToolStripSeparator toolStripMenuItem13;

		private ToolStripMenuItem cmPrevFromList;

		private ToolStripMenuItem cmNextFromList;

		private ToolStripMenuItem cmPageRotate;

		private ToolStripSeparator toolStripMenuItem38;

		private ToolStripMenuItem cmRotate0;

		private ToolStripMenuItem cmRotate90;

		private ToolStripMenuItem cmRotate180;

		private ToolStripMenuItem cmRotate270;

		private ToolStripMenuItem miPageRotate;

		private ToolStripSeparator toolStripMenuItem42;

		private ToolStripMenuItem miOpenRemoteLibrary;

		private ToolStripMenuItem miRandomFromList;

		private ToolStripMenuItem cmRandomFromList;

		private ToolStripMenuItem miUpdateAllComicFiles;

		private ToolStripSeparator toolStripMenuItem43;

		private ToolStripMenuItem miUndo;

		private ToolStripMenuItem miRedo;

		private Timer trimTimer;

		private ToolStripSeparator toolStripMenuItem44;

		private ToolStripMenuItem miComicDisplaySettings;

		private ToolStrip mainToolStrip;

		private ToolStripSplitButton tbFit;

		private ToolStripMenuItem tbOriginal;

		private ToolStripMenuItem tbFitAll;

		private ToolStripMenuItem tbFitWidth;

		private ToolStripMenuItem tbFitWidthAdaptive;

		private ToolStripMenuItem tbFitHeight;

		private ToolStripMenuItem tbBestFit;

		private ToolStripSeparator toolStripMenuItem20;

		private ToolStripMenuItem tbOnlyFitOversized;

		private ToolStripSplitButton tbZoom;

		private ToolStripMenuItem tbZoomIn;

		private ToolStripMenuItem tbZoomOut;

		private ToolStripSeparator toolStripMenuItem30;

		private ToolStripMenuItem tbZoom100;

		private ToolStripMenuItem tbZoom125;

		private ToolStripMenuItem tbZoom150;

		private ToolStripMenuItem tbZoom200;

		private ToolStripMenuItem tbZoom400;

		private ToolStripSeparator toolStripMenuItem31;

		private ToolStripMenuItem tbZoomCustom;

		private ToolStripSplitButton tbRotate;

		private ToolStripMenuItem tbRotateLeft;

		private ToolStripMenuItem tbRotateRight;

		private ToolStripSeparator toolStripSeparator11;

		private ToolStripMenuItem tbRotate0;

		private ToolStripMenuItem tbRotate90;

		private ToolStripMenuItem tbRotate180;

		private ToolStripMenuItem tbRotate270;

		private ToolStripSeparator toolStripMenuItem34;

		private ToolStripMenuItem tbAutoRotate;

		private ToolStripSeparator toolStripSeparator7;

		private ToolStripSplitButton tbMagnify;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripDropDownButton tbTools;

		private ToolStripSeparator toolStripSeparator5;

		private ToolStripSplitButton tbPrevPage;

		private ToolStripMenuItem tbFirstPage;

		private ToolStripMenuItem tbPrevFromList;

		private ToolStripSplitButton tbNextPage;

		private ToolStripMenuItem tbLastPage;

		private ToolStripMenuItem tbNextFromList;

		private ToolStripSeparator toolStripMenuItem49;

		private ToolStripMenuItem tbRandomFromList;

		private ContextMenuStrip toolsContextMenu;

		private ToolStripMenuItem tbOpenComic;

		private ToolStripMenuItem tbShowInfo;

		private ToolStripSeparator toolStripMenuItem47;

		private ToolStripMenuItem tsWorkspaces;

		private ToolStripMenuItem tsSaveWorkspace;

		private ToolStripMenuItem tsEditWorkspaces;

		private ToolStripSeparator tsWorkspaceSep;

		private ToolStripMenuItem tbBookmarks;

		private ToolStripMenuItem tbSetBookmark;

		private ToolStripMenuItem tbRemoveBookmark;

		private ToolStripSeparator tbBookmarkSeparator;

		private ToolStripMenuItem tbAutoScroll;

		private ToolStripSeparator toolStripMenuItem45;

		private ToolStripMenuItem tbMinimalGui;

		private ToolStripMenuItem tbReaderUndocked;

		private ToolStripSeparator toolStripMenuItem52;

		private ToolStripMenuItem tbScan;

		private ToolStripMenuItem tbUpdateAllComicFiles;

		private ToolStripSeparator toolStripMenuItem48;

		private ToolStripMenuItem tbComicDisplaySettings;

		private ToolStripMenuItem tbPreferences;

		private ToolStripMenuItem tbAbout;

		private ToolStripSeparator toolStripMenuItem50;

		private ToolStripMenuItem tbShowMainMenu;

		private ToolStripSeparator toolStripMenuItem51;

		private ToolStripMenuItem tbExit;

		private ToolStripSeparator toolStripMenuItem46;

		private ToolStripMenuItem tbPrevBookmark;

		private ToolStripSeparator toolStripMenuItem19;

		private ToolStripMenuItem tbNextBookmark;

		private ToolStripMenuItem tbLastPageRead;

		private ToolStripSeparator cmBookmarkSeparator;

		private ToolStripSeparator toolStripMenuItem28;

		private ToolStripSeparator toolStripMenuItem53;

		private Timer updateActivityTimer;

		private ToolStripSplitButton tbPageLayout;

		private ToolStripMenuItem tbTwoPagesAdaptive;

		private ToolStripMenuItem tbSinglePage;

		private ToolStripMenuItem tbTwoPages;

		private ToolStripSeparator toolStripMenuItem54;

		private ToolStripMenuItem tbRightToLeft;

		private ToolStripMenuItem miTwoPagesAdaptive;

		private ToolStripMenuItem miTwoPages;

		private ToolStripMenuItem miSinglePage;

		private ToolStripMenuItem cmSinglePage;

		private ToolStripMenuItem cmTwoPages;

		private ToolStripMenuItem cmTwoPagesAdaptive;

		private ToolStripSeparator toolStripMenuItem55;

		private ToolStripSeparator toolStripMenuItem56;

		private ToolStripMenuItem miPreviousList;

		private ToolStripMenuItem miNextList;

		private ToolStripMenuItem miUpdateWebComics;

		private ToolStripMenuItem tbUpdateWebComics;

		private ToolStripMenuItem tbOpenRemoteLibrary;

		private ToolStripMenuItem miInfoPanel;

		private ToolStripStatusLabel tsServerActivity;

		private ToolStripSeparator toolStripMenuItem37;

		private ToolStripMenuItem miNewComic;

		private ToolStripMenuItem miToggleZoom;

		private ToolStripSeparator toolStripMenuItem57;

		private ToolStripSeparator toolStripMenuItem10;

		private QuickOpenView quickOpenView;

		private ToolStripButton tbFullScreen;

		private ContextMenuStrip contextRating;

		private ToolStripMenuItem miRate0;

		private ToolStripSeparator toolStripMenuItem12;

		private ToolStripMenuItem miRate1;

		private ToolStripMenuItem miRate2;

		private ToolStripMenuItem miRate3;

		private ToolStripMenuItem miRate4;

		private ToolStripMenuItem miRate5;

		private ContextMenuStrip contextRating2;

		private ToolStripMenuItem cmRate0;

		private ToolStripSeparator toolStripMenuItem16;

		private ToolStripMenuItem cmRate1;

		private ToolStripMenuItem cmRate2;

		private ToolStripMenuItem cmRate3;

		private ToolStripMenuItem cmRate4;

		private ToolStripMenuItem cmRate5;

		private ToolStripMenuItem cmCloseAllToTheRight;

		private ToolStripSeparator cmComicsSep;

		private ToolStripMenuItem miHelp;

		private ToolStripMenuItem miChooseHelpSystem;

		private ToolStripMenuItem miHelpPlugins;

		private ToolStripMenuItem miHelpQuickIntro;

		private ToolStripMenuItem miDevices;

		private ToolStripMenuItem miSynchronizeDevices;

		private ToolStripStatusLabel tsDeviceSyncActivity;

		private ToolStripMenuItem tsSynchronizeDevices;

		private ToolStripSeparator toolStripMenuItem21;

		private ToolStripMenuItem miTrackCurrentPage;

		private ToolStripStatusLabel tsDataSourceState;

		private ToolStripSeparator toolStripMenuItem58;

		private ToolStripMenuItem miQuickRating;

		private ToolStripSeparator toolStripSeparator6;

		private ToolStripMenuItem cmQuickRating;
	}
}
