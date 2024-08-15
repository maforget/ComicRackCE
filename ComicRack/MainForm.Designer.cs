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
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
			this.components = new System.ComponentModel.Container();
			this.mouseDisableTimer = new System.Windows.Forms.Timer(this.components);
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.miOpenComic = new System.Windows.Forms.ToolStripMenuItem();
			this.miCloseComic = new System.Windows.Forms.ToolStripMenuItem();
			this.miCloseAllComics = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.miAddTab = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.miAddFolderToLibrary = new System.Windows.Forms.ToolStripMenuItem();
			this.miScan = new System.Windows.Forms.ToolStripMenuItem();
			this.miUpdateAllComicFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.miUpdateWebComics = new System.Windows.Forms.ToolStripMenuItem();
			this.miSynchronizeDevices = new System.Windows.Forms.ToolStripMenuItem();
			this.miTasks = new System.Windows.Forms.ToolStripMenuItem();
			this.miFileAutomation = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem57 = new System.Windows.Forms.ToolStripSeparator();
			this.miNewComic = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem42 = new System.Windows.Forms.ToolStripSeparator();
			this.miOpenRemoteLibrary = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripInsertSeperator = new System.Windows.Forms.ToolStripSeparator();
			this.miOpenNow = new System.Windows.Forms.ToolStripMenuItem();
			this.miOpenRecent = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.miRestart = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
			this.miExit = new System.Windows.Forms.ToolStripMenuItem();
			this.editMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem43 = new System.Windows.Forms.ToolStripSeparator();
			this.miUndo = new System.Windows.Forms.ToolStripMenuItem();
			this.miRedo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
			this.miRating = new System.Windows.Forms.ToolStripMenuItem();
			this.contextRating = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miRate0 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.miRate1 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRate2 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRate3 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRate4 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRate5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem58 = new System.Windows.Forms.ToolStripSeparator();
			this.miQuickRating = new System.Windows.Forms.ToolStripMenuItem();
			this.miPageType = new System.Windows.Forms.ToolStripMenuItem();
			this.miPageRotate = new System.Windows.Forms.ToolStripMenuItem();
			this.miBookmarks = new System.Windows.Forms.ToolStripMenuItem();
			this.miSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.miRemoveBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem26 = new System.Windows.Forms.ToolStripSeparator();
			this.miPrevBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.miNextBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.miLastPageRead = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem37 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem40 = new System.Windows.Forms.ToolStripSeparator();
			this.miCopyPage = new System.Windows.Forms.ToolStripMenuItem();
			this.miExportPage = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem39 = new System.Windows.Forms.ToolStripSeparator();
			this.miViewRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.miDevices = new System.Windows.Forms.ToolStripMenuItem();
			this.miPreferences = new System.Windows.Forms.ToolStripMenuItem();
			this.browseMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.miToggleBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.miViewLibrary = new System.Windows.Forms.ToolStripMenuItem();
			this.miViewFolders = new System.Windows.Forms.ToolStripMenuItem();
			this.miViewPages = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.miSidebar = new System.Windows.Forms.ToolStripMenuItem();
			this.miSmallPreview = new System.Windows.Forms.ToolStripMenuItem();
			this.miSearchBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.miInfoPanel = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem56 = new System.Windows.Forms.ToolStripSeparator();
			this.miPreviousList = new System.Windows.Forms.ToolStripMenuItem();
			this.miNextList = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.miWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			this.miSaveWorkspace = new System.Windows.Forms.ToolStripMenuItem();
			this.miEditWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			this.miWorkspaceSep = new System.Windows.Forms.ToolStripSeparator();
			this.miListLayouts = new System.Windows.Forms.ToolStripMenuItem();
			this.miEditListLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.miSaveListLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.miEditLayouts = new System.Windows.Forms.ToolStripMenuItem();
			this.miSetAllListsSame = new System.Windows.Forms.ToolStripMenuItem();
			this.miLayoutSep = new System.Windows.Forms.ToolStripSeparator();
			this.readMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.miFirstPage = new System.Windows.Forms.ToolStripMenuItem();
			this.miPrevPage = new System.Windows.Forms.ToolStripMenuItem();
			this.miNextPage = new System.Windows.Forms.ToolStripMenuItem();
			this.miLastPage = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem18 = new System.Windows.Forms.ToolStripSeparator();
			this.miPrevFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.miNextFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.miRandomFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.miSyncBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripSeparator();
			this.miPrevTab = new System.Windows.Forms.ToolStripMenuItem();
			this.miNextTab = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.miAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.miDoublePageAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.miTrackCurrentPage = new System.Windows.Forms.ToolStripMenuItem();
			this.displayMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.miComicDisplaySettings = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.miPageLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.miOriginal = new System.Windows.Forms.ToolStripMenuItem();
			this.miFitAll = new System.Windows.Forms.ToolStripMenuItem();
			this.miFitWidth = new System.Windows.Forms.ToolStripMenuItem();
			this.miFitWidthAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			this.miFitHeight = new System.Windows.Forms.ToolStripMenuItem();
			this.miBestFit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem27 = new System.Windows.Forms.ToolStripSeparator();
			this.miSinglePage = new System.Windows.Forms.ToolStripMenuItem();
			this.miTwoPages = new System.Windows.Forms.ToolStripMenuItem();
			this.miTwoPagesAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			this.miRightToLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem44 = new System.Windows.Forms.ToolStripSeparator();
			this.miOnlyFitOversized = new System.Windows.Forms.ToolStripMenuItem();
			this.miZoom = new System.Windows.Forms.ToolStripMenuItem();
			this.miZoomIn = new System.Windows.Forms.ToolStripMenuItem();
			this.miZoomOut = new System.Windows.Forms.ToolStripMenuItem();
			this.miToggleZoom = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.miZoom100 = new System.Windows.Forms.ToolStripMenuItem();
			this.miZoom125 = new System.Windows.Forms.ToolStripMenuItem();
			this.miZoom150 = new System.Windows.Forms.ToolStripMenuItem();
			this.miZoom200 = new System.Windows.Forms.ToolStripMenuItem();
			this.miZoom400 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.miZoomCustom = new System.Windows.Forms.ToolStripMenuItem();
			this.miRotation = new System.Windows.Forms.ToolStripMenuItem();
			this.miRotateLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.miRotateRight = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem33 = new System.Windows.Forms.ToolStripSeparator();
			this.miRotate0 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRotate90 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRotate180 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRotate270 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem36 = new System.Windows.Forms.ToolStripSeparator();
			this.miAutoRotate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.miMinimalGui = new System.Windows.Forms.ToolStripMenuItem();
			this.miFullScreen = new System.Windows.Forms.ToolStripMenuItem();
			this.miReaderUndocked = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem41 = new System.Windows.Forms.ToolStripSeparator();
			this.miMagnify = new System.Windows.Forms.ToolStripMenuItem();
			this.helpMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.miWebHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.miHelpPlugins = new System.Windows.Forms.ToolStripMenuItem();
			this.miChooseHelpSystem = new System.Windows.Forms.ToolStripMenuItem();
			this.miHelpQuickIntro = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.miWebHome = new System.Windows.Forms.ToolStripMenuItem();
			this.miWebUserForum = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.miNews = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripSeparator();
			this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.tsText = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsDeviceSyncActivity = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsExportActivity = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsReadInfoActivity = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsWriteInfoActivity = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsPageActivity = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsScanActivity = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsDataSourceState = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsBook = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsCurrentPage = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsPageCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.tsServerActivity = new System.Windows.Forms.ToolStripStatusLabel();
			this.pageContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmShowInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRating = new System.Windows.Forms.ToolStripMenuItem();
			this.contextRating2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmRate0 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.cmRate1 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRate2 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRate3 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRate4 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRate5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.cmQuickRating = new System.Windows.Forms.ToolStripMenuItem();
			this.cmPageType = new System.Windows.Forms.ToolStripMenuItem();
			this.cmPageRotate = new System.Windows.Forms.ToolStripMenuItem();
			this.cmBookmarks = new System.Windows.Forms.ToolStripMenuItem();
			this.cmSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRemoveBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem32 = new System.Windows.Forms.ToolStripSeparator();
			this.cmPrevBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.cmNextBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.cmLastPageRead = new System.Windows.Forms.ToolStripMenuItem();
			this.cmBookmarkSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.cmComics = new System.Windows.Forms.ToolStripMenuItem();
			this.cmOpenComic = new System.Windows.Forms.ToolStripMenuItem();
			this.cmCloseComic = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.cmPrevFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.cmNextFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRandomFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.cmComicsSep = new System.Windows.Forms.ToolStripSeparator();
			this.cmPageLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.cmOriginal = new System.Windows.Forms.ToolStripMenuItem();
			this.cmFitAll = new System.Windows.Forms.ToolStripMenuItem();
			this.cmFitWidth = new System.Windows.Forms.ToolStripMenuItem();
			this.cmFitWidthAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			this.cmFitHeight = new System.Windows.Forms.ToolStripMenuItem();
			this.cmFitBest = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem29 = new System.Windows.Forms.ToolStripSeparator();
			this.cmSinglePage = new System.Windows.Forms.ToolStripMenuItem();
			this.cmTwoPages = new System.Windows.Forms.ToolStripMenuItem();
			this.cmTwoPagesAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRightToLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem38 = new System.Windows.Forms.ToolStripSeparator();
			this.cmRotate0 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRotate90 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRotate180 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmRotate270 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem55 = new System.Windows.Forms.ToolStripSeparator();
			this.cmOnlyFitOversized = new System.Windows.Forms.ToolStripMenuItem();
			this.cmMagnify = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.cmCopyPage = new System.Windows.Forms.ToolStripMenuItem();
			this.cmExportPage = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.cmRefreshPage = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem46 = new System.Windows.Forms.ToolStripSeparator();
			this.cmMinimalGui = new System.Windows.Forms.ToolStripMenuItem();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.notfifyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmNotifyRestore = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.cmNotifyExit = new System.Windows.Forms.ToolStripMenuItem();
			this.viewContainer = new System.Windows.Forms.Panel();
			this.panelReader = new System.Windows.Forms.Panel();
			this.readerContainer = new System.Windows.Forms.Panel();
			this.quickOpenView = new cYo.Projects.ComicRack.Viewer.Views.QuickOpenView();
			this.fileTabs = new cYo.Common.Windows.Forms.TabBar();
			this.mainToolStrip = new System.Windows.Forms.ToolStrip();
			this.tbPrevPage = new System.Windows.Forms.ToolStripSplitButton();
			this.tbFirstPage = new System.Windows.Forms.ToolStripMenuItem();
			this.tbPrevBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem53 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem19 = new System.Windows.Forms.ToolStripSeparator();
			this.tbPrevFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.tbNextPage = new System.Windows.Forms.ToolStripSplitButton();
			this.tbLastPage = new System.Windows.Forms.ToolStripMenuItem();
			this.tbNextBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.tbLastPageRead = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem28 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem49 = new System.Windows.Forms.ToolStripSeparator();
			this.tbNextFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.tbRandomFromList = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.tbPageLayout = new System.Windows.Forms.ToolStripSplitButton();
			this.tbSinglePage = new System.Windows.Forms.ToolStripMenuItem();
			this.tbTwoPages = new System.Windows.Forms.ToolStripMenuItem();
			this.tbTwoPagesAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem54 = new System.Windows.Forms.ToolStripSeparator();
			this.tbRightToLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.tbFit = new System.Windows.Forms.ToolStripSplitButton();
			this.tbOriginal = new System.Windows.Forms.ToolStripMenuItem();
			this.tbFitAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tbFitWidth = new System.Windows.Forms.ToolStripMenuItem();
			this.tbFitWidthAdaptive = new System.Windows.Forms.ToolStripMenuItem();
			this.tbFitHeight = new System.Windows.Forms.ToolStripMenuItem();
			this.tbBestFit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem20 = new System.Windows.Forms.ToolStripSeparator();
			this.tbOnlyFitOversized = new System.Windows.Forms.ToolStripMenuItem();
			this.tbZoom = new System.Windows.Forms.ToolStripSplitButton();
			this.tbZoomIn = new System.Windows.Forms.ToolStripMenuItem();
			this.tbZoomOut = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem30 = new System.Windows.Forms.ToolStripSeparator();
			this.tbZoom100 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbZoom125 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbZoom150 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbZoom200 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbZoom400 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem31 = new System.Windows.Forms.ToolStripSeparator();
			this.tbZoomCustom = new System.Windows.Forms.ToolStripMenuItem();
			this.tbRotate = new System.Windows.Forms.ToolStripSplitButton();
			this.tbRotateLeft = new System.Windows.Forms.ToolStripMenuItem();
			this.tbRotateRight = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.tbRotate0 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbRotate90 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbRotate180 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbRotate270 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem34 = new System.Windows.Forms.ToolStripSeparator();
			this.tbAutoRotate = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.tbMagnify = new System.Windows.Forms.ToolStripSplitButton();
			this.tbFullScreen = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tbTools = new System.Windows.Forms.ToolStripDropDownButton();
			this.toolsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tbOpenComic = new System.Windows.Forms.ToolStripMenuItem();
			this.tbOpenRemoteLibrary = new System.Windows.Forms.ToolStripMenuItem();
			this.tbShowInfo = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem47 = new System.Windows.Forms.ToolStripSeparator();
			this.tsWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			this.tsSaveWorkspace = new System.Windows.Forms.ToolStripMenuItem();
			this.tsEditWorkspaces = new System.Windows.Forms.ToolStripMenuItem();
			this.tsWorkspaceSep = new System.Windows.Forms.ToolStripSeparator();
			this.tbBookmarks = new System.Windows.Forms.ToolStripMenuItem();
			this.tbSetBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.tbRemoveBookmark = new System.Windows.Forms.ToolStripMenuItem();
			this.tbBookmarkSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.tbAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem45 = new System.Windows.Forms.ToolStripSeparator();
			this.tbMinimalGui = new System.Windows.Forms.ToolStripMenuItem();
			this.tbReaderUndocked = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem52 = new System.Windows.Forms.ToolStripSeparator();
			this.tbScan = new System.Windows.Forms.ToolStripMenuItem();
			this.tbUpdateAllComicFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.tbUpdateWebComics = new System.Windows.Forms.ToolStripMenuItem();
			this.tsSynchronizeDevices = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem48 = new System.Windows.Forms.ToolStripSeparator();
			this.tbComicDisplaySettings = new System.Windows.Forms.ToolStripMenuItem();
			this.tbPreferences = new System.Windows.Forms.ToolStripMenuItem();
			this.tbAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem50 = new System.Windows.Forms.ToolStripSeparator();
			this.tbShowMainMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem51 = new System.Windows.Forms.ToolStripSeparator();
			this.tbExit = new System.Windows.Forms.ToolStripMenuItem();
			this.tabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.cmClose = new System.Windows.Forms.ToolStripMenuItem();
			this.cmCloseAllButThis = new System.Windows.Forms.ToolStripMenuItem();
			this.cmCloseAllToTheRight = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem35 = new System.Windows.Forms.ToolStripSeparator();
			this.cmSyncBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.sepBeforeRevealInBrowser = new System.Windows.Forms.ToolStripSeparator();
			this.cmRevealInExplorer = new System.Windows.Forms.ToolStripMenuItem();
			this.cmCopyPath = new System.Windows.Forms.ToolStripMenuItem();
			this.trimTimer = new System.Windows.Forms.Timer(this.components);
			this.mainViewContainer = new cYo.Common.Windows.Forms.SizableContainer();
			this.mainView = new cYo.Projects.ComicRack.Viewer.Views.MainView();
			this.updateActivityTimer = new System.Windows.Forms.Timer(this.components);
			this.miCheckUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenuStrip.SuspendLayout();
			this.contextRating.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.pageContextMenu.SuspendLayout();
			this.contextRating2.SuspendLayout();
			this.notfifyContextMenu.SuspendLayout();
			this.viewContainer.SuspendLayout();
			this.panelReader.SuspendLayout();
			this.readerContainer.SuspendLayout();
			this.fileTabs.SuspendLayout();
			this.mainToolStrip.SuspendLayout();
			this.toolsContextMenu.SuspendLayout();
			this.tabContextMenu.SuspendLayout();
			this.mainViewContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// mouseDisableTimer
			// 
			this.mouseDisableTimer.Interval = 500;
			this.mouseDisableTimer.Tick += new System.EventHandler(this.showDisableTimer_Tick);
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.browseMenu,
            this.readMenu,
            this.displayMenu,
            this.helpMenu});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Size = new System.Drawing.Size(744, 24);
			this.mainMenuStrip.TabIndex = 0;
			this.mainMenuStrip.MenuDeactivate += new System.EventHandler(this.mainMenuStrip_MenuDeactivate);
			// 
			// fileMenu
			// 
			this.fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenComic,
            this.miCloseComic,
            this.miCloseAllComics,
            this.toolStripMenuItem7,
            this.miAddTab,
            this.toolStripMenuItem14,
            this.miAddFolderToLibrary,
            this.miScan,
            this.miUpdateAllComicFiles,
            this.miUpdateWebComics,
            this.miSynchronizeDevices,
            this.miTasks,
            this.miFileAutomation,
            this.toolStripMenuItem57,
            this.miNewComic,
            this.toolStripMenuItem42,
            this.miOpenRemoteLibrary,
            this.toolStripInsertSeperator,
            this.miOpenNow,
            this.miOpenRecent,
            this.toolStripMenuItem4,
            this.miRestart,
            this.toolStripMenuItem24,
            this.miExit});
			this.fileMenu.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.fileMenu.Name = "fileMenu";
			this.fileMenu.Size = new System.Drawing.Size(37, 20);
			this.fileMenu.Text = "&File";
			this.fileMenu.DropDownOpening += new System.EventHandler(this.fileMenu_DropDownOpening);
			// 
			// miOpenComic
			// 
			this.miOpenComic.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			this.miOpenComic.Name = "miOpenComic";
			this.miOpenComic.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.miOpenComic.Size = new System.Drawing.Size(280, 22);
			this.miOpenComic.Text = "&Open File...";
			// 
			// miCloseComic
			// 
			this.miCloseComic.Name = "miCloseComic";
			this.miCloseComic.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.miCloseComic.Size = new System.Drawing.Size(280, 22);
			this.miCloseComic.Text = "&Close";
			// 
			// miCloseAllComics
			// 
			this.miCloseAllComics.Name = "miCloseAllComics";
			this.miCloseAllComics.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.X)));
			this.miCloseAllComics.Size = new System.Drawing.Size(280, 22);
			this.miCloseAllComics.Text = "Close A&ll";
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(277, 6);
			// 
			// miAddTab
			// 
			this.miAddTab.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewTab;
			this.miAddTab.Name = "miAddTab";
			this.miAddTab.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
			this.miAddTab.Size = new System.Drawing.Size(280, 22);
			this.miAddTab.Text = "New &Tab";
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(277, 6);
			// 
			// miAddFolderToLibrary
			// 
			this.miAddFolderToLibrary.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AddFolder;
			this.miAddFolderToLibrary.Name = "miAddFolderToLibrary";
			this.miAddFolderToLibrary.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.A)));
			this.miAddFolderToLibrary.Size = new System.Drawing.Size(280, 22);
			this.miAddFolderToLibrary.Text = "&Add Folder to Library...";
			// 
			// miScan
			// 
			this.miScan.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Scan;
			this.miScan.Name = "miScan";
			this.miScan.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.miScan.Size = new System.Drawing.Size(280, 22);
			this.miScan.Text = "Scan Book &Folders";
			// 
			// miUpdateAllComicFiles
			// 
			this.miUpdateAllComicFiles.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateSmall;
			this.miUpdateAllComicFiles.Name = "miUpdateAllComicFiles";
			this.miUpdateAllComicFiles.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
			this.miUpdateAllComicFiles.Size = new System.Drawing.Size(280, 22);
			this.miUpdateAllComicFiles.Text = "Update all Book Files";
			// 
			// miUpdateWebComics
			// 
			this.miUpdateWebComics.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateWeb;
			this.miUpdateWebComics.Name = "miUpdateWebComics";
			this.miUpdateWebComics.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
			this.miUpdateWebComics.Size = new System.Drawing.Size(280, 22);
			this.miUpdateWebComics.Text = "Update Web Comics";
			// 
			// miSynchronizeDevices
			// 
			this.miSynchronizeDevices.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DeviceSync;
			this.miSynchronizeDevices.Name = "miSynchronizeDevices";
			this.miSynchronizeDevices.Size = new System.Drawing.Size(280, 22);
			this.miSynchronizeDevices.Text = "Synchronize Devices";
			// 
			// miTasks
			// 
			this.miTasks.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.BackgroundJob;
			this.miTasks.Name = "miTasks";
			this.miTasks.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
			this.miTasks.Size = new System.Drawing.Size(280, 22);
			this.miTasks.Text = "&Tasks...";
			// 
			// miFileAutomation
			// 
			this.miFileAutomation.Name = "miFileAutomation";
			this.miFileAutomation.Size = new System.Drawing.Size(280, 22);
			this.miFileAutomation.Text = "A&utomation";
			// 
			// toolStripMenuItem57
			// 
			this.toolStripMenuItem57.Name = "toolStripMenuItem57";
			this.toolStripMenuItem57.Size = new System.Drawing.Size(277, 6);
			// 
			// miNewComic
			// 
			this.miNewComic.Name = "miNewComic";
			this.miNewComic.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.miNewComic.Size = new System.Drawing.Size(280, 22);
			this.miNewComic.Text = "&New fileless Book Entry...";
			// 
			// toolStripMenuItem42
			// 
			this.toolStripMenuItem42.Name = "toolStripMenuItem42";
			this.toolStripMenuItem42.Size = new System.Drawing.Size(277, 6);
			// 
			// miOpenRemoteLibrary
			// 
			this.miOpenRemoteLibrary.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoteDatabase;
			this.miOpenRemoteLibrary.Name = "miOpenRemoteLibrary";
			this.miOpenRemoteLibrary.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
			this.miOpenRemoteLibrary.Size = new System.Drawing.Size(280, 22);
			this.miOpenRemoteLibrary.Text = "Open Remote Library...";
			// 
			// toolStripInsertSeperator
			// 
			this.toolStripInsertSeperator.Name = "toolStripInsertSeperator";
			this.toolStripInsertSeperator.Size = new System.Drawing.Size(277, 6);
			// 
			// miOpenNow
			// 
			this.miOpenNow.Name = "miOpenNow";
			this.miOpenNow.Size = new System.Drawing.Size(280, 22);
			this.miOpenNow.Text = "Open Books";
			// 
			// miOpenRecent
			// 
			this.miOpenRecent.Name = "miOpenRecent";
			this.miOpenRecent.Size = new System.Drawing.Size(280, 22);
			this.miOpenRecent.Text = "&Recent Books";
			this.miOpenRecent.DropDownOpening += new System.EventHandler(this.RecentFilesMenuOpening);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(277, 6);
			// 
			// miRestart
			// 
			this.miRestart.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Restart;
			this.miRestart.Name = "miRestart";
			this.miRestart.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
			this.miRestart.Size = new System.Drawing.Size(280, 22);
			this.miRestart.Text = "Rest&art";
			// 
			// toolStripMenuItem24
			// 
			this.toolStripMenuItem24.Name = "toolStripMenuItem24";
			this.toolStripMenuItem24.Size = new System.Drawing.Size(277, 6);
			// 
			// miExit
			// 
			this.miExit.Name = "miExit";
			this.miExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.miExit.Size = new System.Drawing.Size(280, 22);
			this.miExit.Text = "&Exit";
			// 
			// editMenu
			// 
			this.editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShowInfo,
            this.toolStripMenuItem43,
            this.miUndo,
            this.miRedo,
            this.toolStripMenuItem22,
            this.miRating,
            this.miPageType,
            this.miPageRotate,
            this.miBookmarks,
            this.toolStripMenuItem40,
            this.miCopyPage,
            this.miExportPage,
            this.toolStripMenuItem39,
            this.miViewRefresh,
            this.toolStripSeparator4,
            this.miDevices,
            this.miPreferences});
			this.editMenu.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.editMenu.Name = "editMenu";
			this.editMenu.Size = new System.Drawing.Size(39, 20);
			this.editMenu.Text = "&Edit";
			this.editMenu.DropDownOpening += new System.EventHandler(this.editMenu_DropDownOpening);
			// 
			// miShowInfo
			// 
			this.miShowInfo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GetInfo;
			this.miShowInfo.Name = "miShowInfo";
			this.miShowInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.miShowInfo.Size = new System.Drawing.Size(220, 22);
			this.miShowInfo.Text = "Info...";
			// 
			// toolStripMenuItem43
			// 
			this.toolStripMenuItem43.Name = "toolStripMenuItem43";
			this.toolStripMenuItem43.Size = new System.Drawing.Size(217, 6);
			// 
			// miUndo
			// 
			this.miUndo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Undo;
			this.miUndo.Name = "miUndo";
			this.miUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.miUndo.Size = new System.Drawing.Size(220, 22);
			this.miUndo.Text = "&Undo";
			// 
			// miRedo
			// 
			this.miRedo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Redo;
			this.miRedo.Name = "miRedo";
			this.miRedo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.miRedo.Size = new System.Drawing.Size(220, 22);
			this.miRedo.Text = "&Redo";
			// 
			// toolStripMenuItem22
			// 
			this.toolStripMenuItem22.Name = "toolStripMenuItem22";
			this.toolStripMenuItem22.Size = new System.Drawing.Size(217, 6);
			// 
			// miRating
			// 
			this.miRating.DropDown = this.contextRating;
			this.miRating.Name = "miRating";
			this.miRating.Size = new System.Drawing.Size(220, 22);
			this.miRating.Text = "My R&ating";
			// 
			// contextRating
			// 
			this.contextRating.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRate0,
            this.toolStripMenuItem12,
            this.miRate1,
            this.miRate2,
            this.miRate3,
            this.miRate4,
            this.miRate5,
            this.toolStripMenuItem58,
            this.miQuickRating});
			this.contextRating.Name = "contextRating";
			this.contextRating.OwnerItem = this.miRating;
			this.contextRating.Size = new System.Drawing.Size(286, 170);
			// 
			// miRate0
			// 
			this.miRate0.Name = "miRate0";
			this.miRate0.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.miRate0.Size = new System.Drawing.Size(285, 22);
			this.miRate0.Text = "None";
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(282, 6);
			// 
			// miRate1
			// 
			this.miRate1.Name = "miRate1";
			this.miRate1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D1)));
			this.miRate1.Size = new System.Drawing.Size(285, 22);
			this.miRate1.Text = "* (1 Star)";
			// 
			// miRate2
			// 
			this.miRate2.Name = "miRate2";
			this.miRate2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D2)));
			this.miRate2.Size = new System.Drawing.Size(285, 22);
			this.miRate2.Text = "** (2 Stars)";
			// 
			// miRate3
			// 
			this.miRate3.Name = "miRate3";
			this.miRate3.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D3)));
			this.miRate3.Size = new System.Drawing.Size(285, 22);
			this.miRate3.Text = "*** (3 Stars)";
			// 
			// miRate4
			// 
			this.miRate4.Name = "miRate4";
			this.miRate4.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D4)));
			this.miRate4.Size = new System.Drawing.Size(285, 22);
			this.miRate4.Text = "**** (4 Stars)";
			// 
			// miRate5
			// 
			this.miRate5.Name = "miRate5";
			this.miRate5.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D5)));
			this.miRate5.Size = new System.Drawing.Size(285, 22);
			this.miRate5.Text = "***** (5 Stars)";
			// 
			// toolStripMenuItem58
			// 
			this.toolStripMenuItem58.Name = "toolStripMenuItem58";
			this.toolStripMenuItem58.Size = new System.Drawing.Size(282, 6);
			// 
			// miQuickRating
			// 
			this.miQuickRating.Name = "miQuickRating";
			this.miQuickRating.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
			this.miQuickRating.Size = new System.Drawing.Size(285, 22);
			this.miQuickRating.Text = "Quick Rating and Review...";
			// 
			// miPageType
			// 
			this.miPageType.Name = "miPageType";
			this.miPageType.Size = new System.Drawing.Size(220, 22);
			this.miPageType.Text = "&Page Type";
			// 
			// miPageRotate
			// 
			this.miPageRotate.Name = "miPageRotate";
			this.miPageRotate.Size = new System.Drawing.Size(220, 22);
			this.miPageRotate.Text = "Page Rotation";
			// 
			// miBookmarks
			// 
			this.miBookmarks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSetBookmark,
            this.miRemoveBookmark,
            this.toolStripMenuItem26,
            this.miPrevBookmark,
            this.miNextBookmark,
            this.toolStripMenuItem8,
            this.miLastPageRead,
            this.toolStripMenuItem37});
			this.miBookmarks.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Bookmark;
			this.miBookmarks.Name = "miBookmarks";
			this.miBookmarks.Size = new System.Drawing.Size(220, 22);
			this.miBookmarks.Text = "&Bookmarks";
			this.miBookmarks.DropDownOpening += new System.EventHandler(this.miBookmarks_DropDownOpening);
			// 
			// miSetBookmark
			// 
			this.miSetBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewBookmark;
			this.miSetBookmark.Name = "miSetBookmark";
			this.miSetBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
			this.miSetBookmark.Size = new System.Drawing.Size(249, 22);
			this.miSetBookmark.Text = "Set Bookmark...";
			// 
			// miRemoveBookmark
			// 
			this.miRemoveBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoveBookmark;
			this.miRemoveBookmark.Name = "miRemoveBookmark";
			this.miRemoveBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
			this.miRemoveBookmark.Size = new System.Drawing.Size(249, 22);
			this.miRemoveBookmark.Text = "Remove Bookmark";
			// 
			// toolStripMenuItem26
			// 
			this.toolStripMenuItem26.Name = "toolStripMenuItem26";
			this.toolStripMenuItem26.Size = new System.Drawing.Size(246, 6);
			// 
			// miPrevBookmark
			// 
			this.miPrevBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.PreviousBookmark;
			this.miPrevBookmark.Name = "miPrevBookmark";
			this.miPrevBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
			this.miPrevBookmark.Size = new System.Drawing.Size(249, 22);
			this.miPrevBookmark.Text = "Previous Bookmark";
			// 
			// miNextBookmark
			// 
			this.miNextBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NextBookmark;
			this.miNextBookmark.Name = "miNextBookmark";
			this.miNextBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.miNextBookmark.Size = new System.Drawing.Size(249, 22);
			this.miNextBookmark.Text = "Next Bookmark";
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(246, 6);
			// 
			// miLastPageRead
			// 
			this.miLastPageRead.Name = "miLastPageRead";
			this.miLastPageRead.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.L)));
			this.miLastPageRead.Size = new System.Drawing.Size(249, 22);
			this.miLastPageRead.Text = "L&ast Page Read";
			// 
			// toolStripMenuItem37
			// 
			this.toolStripMenuItem37.Name = "toolStripMenuItem37";
			this.toolStripMenuItem37.Size = new System.Drawing.Size(246, 6);
			this.toolStripMenuItem37.Tag = "bms";
			// 
			// toolStripMenuItem40
			// 
			this.toolStripMenuItem40.Name = "toolStripMenuItem40";
			this.toolStripMenuItem40.Size = new System.Drawing.Size(217, 6);
			// 
			// miCopyPage
			// 
			this.miCopyPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Copy;
			this.miCopyPage.Name = "miCopyPage";
			this.miCopyPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.miCopyPage.Size = new System.Drawing.Size(220, 22);
			this.miCopyPage.Text = "&Copy Page";
			// 
			// miExportPage
			// 
			this.miExportPage.Name = "miExportPage";
			this.miExportPage.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
			this.miExportPage.Size = new System.Drawing.Size(220, 22);
			this.miExportPage.Text = "&Export Page...";
			// 
			// toolStripMenuItem39
			// 
			this.toolStripMenuItem39.Name = "toolStripMenuItem39";
			this.toolStripMenuItem39.Size = new System.Drawing.Size(217, 6);
			// 
			// miViewRefresh
			// 
			this.miViewRefresh.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			this.miViewRefresh.Name = "miViewRefresh";
			this.miViewRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.miViewRefresh.Size = new System.Drawing.Size(220, 22);
			this.miViewRefresh.Text = "&Refresh";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(217, 6);
			// 
			// miDevices
			// 
			this.miDevices.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDevices;
			this.miDevices.Name = "miDevices";
			this.miDevices.Size = new System.Drawing.Size(220, 22);
			this.miDevices.Text = "Devices...";
			// 
			// miPreferences
			// 
			this.miPreferences.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Preferences;
			this.miPreferences.Name = "miPreferences";
			this.miPreferences.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F9)));
			this.miPreferences.Size = new System.Drawing.Size(220, 22);
			this.miPreferences.Text = "&Preferences...";
			// 
			// browseMenu
			// 
			this.browseMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miToggleBrowser,
            this.toolStripMenuItem2,
            this.miViewLibrary,
            this.miViewFolders,
            this.miViewPages,
            this.toolStripMenuItem9,
            this.miSidebar,
            this.miSmallPreview,
            this.miSearchBrowser,
            this.miInfoPanel,
            this.toolStripMenuItem56,
            this.miPreviousList,
            this.miNextList,
            this.toolStripMenuItem6,
            this.miWorkspaces,
            this.miListLayouts});
			this.browseMenu.Name = "browseMenu";
			this.browseMenu.Size = new System.Drawing.Size(57, 20);
			this.browseMenu.Text = "&Browse";
			// 
			// miToggleBrowser
			// 
			this.miToggleBrowser.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Browser;
			this.miToggleBrowser.Name = "miToggleBrowser";
			this.miToggleBrowser.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.miToggleBrowser.Size = new System.Drawing.Size(205, 22);
			this.miToggleBrowser.Text = "&Browser";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(202, 6);
			// 
			// miViewLibrary
			// 
			this.miViewLibrary.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Database;
			this.miViewLibrary.Name = "miViewLibrary";
			this.miViewLibrary.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.miViewLibrary.Size = new System.Drawing.Size(205, 22);
			this.miViewLibrary.Text = "Li&brary";
			// 
			// miViewFolders
			// 
			this.miViewFolders.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FileBrowser;
			this.miViewFolders.Name = "miViewFolders";
			this.miViewFolders.ShortcutKeys = System.Windows.Forms.Keys.F7;
			this.miViewFolders.Size = new System.Drawing.Size(205, 22);
			this.miViewFolders.Text = "&Folders";
			// 
			// miViewPages
			// 
			this.miViewPages.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ComicPage;
			this.miViewPages.Name = "miViewPages";
			this.miViewPages.ShortcutKeys = System.Windows.Forms.Keys.F8;
			this.miViewPages.Size = new System.Drawing.Size(205, 22);
			this.miViewPages.Text = "&Pages";
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(202, 6);
			// 
			// miSidebar
			// 
			this.miSidebar.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Sidebar;
			this.miSidebar.Name = "miSidebar";
			this.miSidebar.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F6)));
			this.miSidebar.Size = new System.Drawing.Size(205, 22);
			this.miSidebar.Text = "&Sidebar";
			// 
			// miSmallPreview
			// 
			this.miSmallPreview.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallPreview;
			this.miSmallPreview.Name = "miSmallPreview";
			this.miSmallPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F7)));
			this.miSmallPreview.Size = new System.Drawing.Size(205, 22);
			this.miSmallPreview.Text = "S&mall Preview";
			// 
			// miSearchBrowser
			// 
			this.miSearchBrowser.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Search;
			this.miSearchBrowser.Name = "miSearchBrowser";
			this.miSearchBrowser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F8)));
			this.miSearchBrowser.Size = new System.Drawing.Size(205, 22);
			this.miSearchBrowser.Text = "S&earch Browser";
			// 
			// miInfoPanel
			// 
			this.miInfoPanel.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.InfoPanel;
			this.miInfoPanel.Name = "miInfoPanel";
			this.miInfoPanel.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F9)));
			this.miInfoPanel.Size = new System.Drawing.Size(205, 22);
			this.miInfoPanel.Text = "Info Panel";
			// 
			// toolStripMenuItem56
			// 
			this.toolStripMenuItem56.Name = "toolStripMenuItem56";
			this.toolStripMenuItem56.Size = new System.Drawing.Size(202, 6);
			// 
			// miPreviousList
			// 
			this.miPreviousList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.BrowsePrevious;
			this.miPreviousList.Name = "miPreviousList";
			this.miPreviousList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
			this.miPreviousList.Size = new System.Drawing.Size(205, 22);
			this.miPreviousList.Text = "Previous List";
			// 
			// miNextList
			// 
			this.miNextList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.BrowseNext;
			this.miNextList.Name = "miNextList";
			this.miNextList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
			this.miNextList.Size = new System.Drawing.Size(205, 22);
			this.miNextList.Text = "Next List";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(202, 6);
			// 
			// miWorkspaces
			// 
			this.miWorkspaces.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSaveWorkspace,
            this.miEditWorkspaces,
            this.miWorkspaceSep});
			this.miWorkspaces.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Workspace;
			this.miWorkspaces.Name = "miWorkspaces";
			this.miWorkspaces.Size = new System.Drawing.Size(205, 22);
			this.miWorkspaces.Text = "&Workspaces";
			// 
			// miSaveWorkspace
			// 
			this.miSaveWorkspace.Name = "miSaveWorkspace";
			this.miSaveWorkspace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.miSaveWorkspace.Size = new System.Drawing.Size(237, 22);
			this.miSaveWorkspace.Text = "&Save Workspace...";
			// 
			// miEditWorkspaces
			// 
			this.miEditWorkspaces.Name = "miEditWorkspaces";
			this.miEditWorkspaces.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.W)));
			this.miEditWorkspaces.Size = new System.Drawing.Size(237, 22);
			this.miEditWorkspaces.Text = "&Edit Workspaces...";
			// 
			// miWorkspaceSep
			// 
			this.miWorkspaceSep.Name = "miWorkspaceSep";
			this.miWorkspaceSep.Size = new System.Drawing.Size(234, 6);
			// 
			// miListLayouts
			// 
			this.miListLayouts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditListLayout,
            this.miSaveListLayout,
            this.toolStripMenuItem10,
            this.miEditLayouts,
            this.miSetAllListsSame,
            this.miLayoutSep});
			this.miListLayouts.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ListLayout;
			this.miListLayouts.Name = "miListLayouts";
			this.miListLayouts.Size = new System.Drawing.Size(205, 22);
			this.miListLayouts.Text = "List Layout";
			// 
			// miEditListLayout
			// 
			this.miEditListLayout.Name = "miEditListLayout";
			this.miEditListLayout.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.miEditListLayout.Size = new System.Drawing.Size(225, 22);
			this.miEditListLayout.Text = "&Edit List Layout...";
			// 
			// miSaveListLayout
			// 
			this.miSaveListLayout.Name = "miSaveListLayout";
			this.miSaveListLayout.Size = new System.Drawing.Size(225, 22);
			this.miSaveListLayout.Text = "&Save List Layout...";
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			this.toolStripMenuItem10.Size = new System.Drawing.Size(222, 6);
			// 
			// miEditLayouts
			// 
			this.miEditLayouts.Name = "miEditLayouts";
			this.miEditLayouts.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.L)));
			this.miEditLayouts.Size = new System.Drawing.Size(225, 22);
			this.miEditLayouts.Text = "&Edit Layouts...";
			// 
			// miSetAllListsSame
			// 
			this.miSetAllListsSame.Name = "miSetAllListsSame";
			this.miSetAllListsSame.Size = new System.Drawing.Size(225, 22);
			this.miSetAllListsSame.Text = "Set all Lists to current Layout";
			// 
			// miLayoutSep
			// 
			this.miLayoutSep.Name = "miLayoutSep";
			this.miLayoutSep.Size = new System.Drawing.Size(222, 6);
			// 
			// readMenu
			// 
			this.readMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFirstPage,
            this.miPrevPage,
            this.miNextPage,
            this.miLastPage,
            this.toolStripMenuItem18,
            this.miPrevFromList,
            this.miNextFromList,
            this.miRandomFromList,
            this.miSyncBrowser,
            this.toolStripMenuItem17,
            this.miPrevTab,
            this.miNextTab,
            this.toolStripMenuItem1,
            this.miAutoScroll,
            this.miDoublePageAutoScroll,
            this.toolStripMenuItem21,
            this.miTrackCurrentPage});
			this.readMenu.Name = "readMenu";
			this.readMenu.Size = new System.Drawing.Size(45, 20);
			this.readMenu.Text = "&Read";
			// 
			// miFirstPage
			// 
			this.miFirstPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
			this.miFirstPage.Name = "miFirstPage";
			this.miFirstPage.ShortcutKeyDisplayString = "";
			this.miFirstPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.miFirstPage.Size = new System.Drawing.Size(287, 22);
			this.miFirstPage.Text = "&First Page";
			// 
			// miPrevPage
			// 
			this.miPrevPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
			this.miPrevPage.Name = "miPrevPage";
			this.miPrevPage.ShortcutKeyDisplayString = "";
			this.miPrevPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.miPrevPage.Size = new System.Drawing.Size(287, 22);
			this.miPrevPage.Text = "&Previous Page";
			// 
			// miNextPage
			// 
			this.miNextPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
			this.miNextPage.Name = "miNextPage";
			this.miNextPage.ShortcutKeyDisplayString = "";
			this.miNextPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.miNextPage.Size = new System.Drawing.Size(287, 22);
			this.miNextPage.Text = "&Next Page";
			// 
			// miLastPage
			// 
			this.miLastPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
			this.miLastPage.Name = "miLastPage";
			this.miLastPage.ShortcutKeyDisplayString = "";
			this.miLastPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.miLastPage.Size = new System.Drawing.Size(287, 22);
			this.miLastPage.Text = "&Last Page";
			// 
			// toolStripMenuItem18
			// 
			this.toolStripMenuItem18.Name = "toolStripMenuItem18";
			this.toolStripMenuItem18.Size = new System.Drawing.Size(284, 6);
			// 
			// miPrevFromList
			// 
			this.miPrevFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.PrevFromList;
			this.miPrevFromList.Name = "miPrevFromList";
			this.miPrevFromList.ShortcutKeyDisplayString = "";
			this.miPrevFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.P)));
			this.miPrevFromList.Size = new System.Drawing.Size(287, 22);
			this.miPrevFromList.Text = "Pre&vious Book";
			// 
			// miNextFromList
			// 
			this.miNextFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NextFromList;
			this.miNextFromList.Name = "miNextFromList";
			this.miNextFromList.ShortcutKeyDisplayString = "";
			this.miNextFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.N)));
			this.miNextFromList.Size = new System.Drawing.Size(287, 22);
			this.miNextFromList.Text = "Ne&xt Book";
			// 
			// miRandomFromList
			// 
			this.miRandomFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RandomComic;
			this.miRandomFromList.Name = "miRandomFromList";
			this.miRandomFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.O)));
			this.miRandomFromList.Size = new System.Drawing.Size(287, 22);
			this.miRandomFromList.Text = "Random Book";
			// 
			// miSyncBrowser
			// 
			this.miSyncBrowser.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SyncBrowser;
			this.miSyncBrowser.Name = "miSyncBrowser";
			this.miSyncBrowser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
			this.miSyncBrowser.Size = new System.Drawing.Size(287, 22);
			this.miSyncBrowser.Text = "Show in &Browser";
			// 
			// toolStripMenuItem17
			// 
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			this.toolStripMenuItem17.Size = new System.Drawing.Size(284, 6);
			// 
			// miPrevTab
			// 
			this.miPrevTab.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Previous;
			this.miPrevTab.Name = "miPrevTab";
			this.miPrevTab.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.J)));
			this.miPrevTab.Size = new System.Drawing.Size(287, 22);
			this.miPrevTab.Text = "&Previous Tab";
			// 
			// miNextTab
			// 
			this.miNextTab.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Next;
			this.miNextTab.Name = "miNextTab";
			this.miNextTab.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.K)));
			this.miNextTab.Size = new System.Drawing.Size(287, 22);
			this.miNextTab.Text = "Next &Tab";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(284, 6);
			// 
			// miAutoScroll
			// 
			this.miAutoScroll.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.CursorScroll;
			this.miAutoScroll.Name = "miAutoScroll";
			this.miAutoScroll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.miAutoScroll.Size = new System.Drawing.Size(287, 22);
			this.miAutoScroll.Text = "&Auto Scrolling";
			// 
			// miDoublePageAutoScroll
			// 
			this.miDoublePageAutoScroll.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageAutoscroll;
			this.miDoublePageAutoScroll.Name = "miDoublePageAutoScroll";
			this.miDoublePageAutoScroll.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.miDoublePageAutoScroll.Size = new System.Drawing.Size(287, 22);
			this.miDoublePageAutoScroll.Text = "Double Page Auto Scrolling";
			// 
			// toolStripMenuItem21
			// 
			this.toolStripMenuItem21.Name = "toolStripMenuItem21";
			this.toolStripMenuItem21.Size = new System.Drawing.Size(284, 6);
			// 
			// miTrackCurrentPage
			// 
			this.miTrackCurrentPage.Name = "miTrackCurrentPage";
			this.miTrackCurrentPage.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.T)));
			this.miTrackCurrentPage.Size = new System.Drawing.Size(287, 22);
			this.miTrackCurrentPage.Text = "Track current Page";
			// 
			// displayMenu
			// 
			this.displayMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miComicDisplaySettings,
            this.toolStripSeparator3,
            this.miPageLayout,
            this.miZoom,
            this.miRotation,
            this.toolStripMenuItem23,
            this.miMinimalGui,
            this.miFullScreen,
            this.miReaderUndocked,
            this.toolStripMenuItem41,
            this.miMagnify});
			this.displayMenu.ImageTransparentColor = System.Drawing.Color.Fuchsia;
			this.displayMenu.Name = "displayMenu";
			this.displayMenu.Size = new System.Drawing.Size(57, 20);
			this.displayMenu.Text = "&Display";
			// 
			// miComicDisplaySettings
			// 
			this.miComicDisplaySettings.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DisplaySettings;
			this.miComicDisplaySettings.Name = "miComicDisplaySettings";
			this.miComicDisplaySettings.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.miComicDisplaySettings.Size = new System.Drawing.Size(221, 22);
			this.miComicDisplaySettings.Text = "Book Display Settings...";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(218, 6);
			// 
			// miPageLayout
			// 
			this.miPageLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOriginal,
            this.miFitAll,
            this.miFitWidth,
            this.miFitWidthAdaptive,
            this.miFitHeight,
            this.miBestFit,
            this.toolStripMenuItem27,
            this.miSinglePage,
            this.miTwoPages,
            this.miTwoPagesAdaptive,
            this.miRightToLeft,
            this.toolStripMenuItem44,
            this.miOnlyFitOversized});
			this.miPageLayout.Name = "miPageLayout";
			this.miPageLayout.Size = new System.Drawing.Size(221, 22);
			this.miPageLayout.Text = "&Page Layout";
			// 
			// miOriginal
			// 
			this.miOriginal.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Original;
			this.miOriginal.Name = "miOriginal";
			this.miOriginal.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
			this.miOriginal.Size = new System.Drawing.Size(247, 22);
			this.miOriginal.Text = "Original Size";
			// 
			// miFitAll
			// 
			this.miFitAll.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			this.miFitAll.Name = "miFitAll";
			this.miFitAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
			this.miFitAll.Size = new System.Drawing.Size(247, 22);
			this.miFitAll.Text = "Fit &All";
			// 
			// miFitWidth
			// 
			this.miFitWidth.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidth;
			this.miFitWidth.Name = "miFitWidth";
			this.miFitWidth.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
			this.miFitWidth.Size = new System.Drawing.Size(247, 22);
			this.miFitWidth.Text = "Fit &Width";
			// 
			// miFitWidthAdaptive
			// 
			this.miFitWidthAdaptive.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidthAdaptive;
			this.miFitWidthAdaptive.Name = "miFitWidthAdaptive";
			this.miFitWidthAdaptive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
			this.miFitWidthAdaptive.Size = new System.Drawing.Size(247, 22);
			this.miFitWidthAdaptive.Text = "Fit Width (adaptive)";
			// 
			// miFitHeight
			// 
			this.miFitHeight.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitHeight;
			this.miFitHeight.Name = "miFitHeight";
			this.miFitHeight.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D5)));
			this.miFitHeight.Size = new System.Drawing.Size(247, 22);
			this.miFitHeight.Text = "Fit &Height";
			// 
			// miBestFit
			// 
			this.miBestFit.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitBest;
			this.miBestFit.Name = "miBestFit";
			this.miBestFit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D6)));
			this.miBestFit.Size = new System.Drawing.Size(247, 22);
			this.miBestFit.Text = "Fit &Best";
			// 
			// toolStripMenuItem27
			// 
			this.toolStripMenuItem27.Name = "toolStripMenuItem27";
			this.toolStripMenuItem27.Size = new System.Drawing.Size(244, 6);
			// 
			// miSinglePage
			// 
			this.miSinglePage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			this.miSinglePage.Name = "miSinglePage";
			this.miSinglePage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D7)));
			this.miSinglePage.Size = new System.Drawing.Size(247, 22);
			this.miSinglePage.Text = "Single Page";
			// 
			// miTwoPages
			// 
			this.miTwoPages.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageForced;
			this.miTwoPages.Name = "miTwoPages";
			this.miTwoPages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D8)));
			this.miTwoPages.Size = new System.Drawing.Size(247, 22);
			this.miTwoPages.Text = "Two Pages";
			// 
			// miTwoPagesAdaptive
			// 
			this.miTwoPagesAdaptive.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
			this.miTwoPagesAdaptive.Name = "miTwoPagesAdaptive";
			this.miTwoPagesAdaptive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D9)));
			this.miTwoPagesAdaptive.Size = new System.Drawing.Size(247, 22);
			this.miTwoPagesAdaptive.Text = "Two Pages (adaptive)";
			this.miTwoPagesAdaptive.ToolTipText = "Show one or two pages";
			// 
			// miRightToLeft
			// 
			this.miRightToLeft.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RightToLeft;
			this.miRightToLeft.Name = "miRightToLeft";
			this.miRightToLeft.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
			this.miRightToLeft.Size = new System.Drawing.Size(247, 22);
			this.miRightToLeft.Text = "Right to Left";
			// 
			// toolStripMenuItem44
			// 
			this.toolStripMenuItem44.Name = "toolStripMenuItem44";
			this.toolStripMenuItem44.Size = new System.Drawing.Size(244, 6);
			// 
			// miOnlyFitOversized
			// 
			this.miOnlyFitOversized.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Oversized;
			this.miOnlyFitOversized.Name = "miOnlyFitOversized";
			this.miOnlyFitOversized.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.miOnlyFitOversized.Size = new System.Drawing.Size(247, 22);
			this.miOnlyFitOversized.Text = "&Only fit if oversized";
			// 
			// miZoom
			// 
			this.miZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miZoomIn,
            this.miZoomOut,
            this.miToggleZoom,
            this.toolStripSeparator14,
            this.miZoom100,
            this.miZoom125,
            this.miZoom150,
            this.miZoom200,
            this.miZoom400,
            this.toolStripSeparator15,
            this.miZoomCustom});
			this.miZoom.Name = "miZoom";
			this.miZoom.Size = new System.Drawing.Size(221, 22);
			this.miZoom.Text = "Zoom";
			// 
			// miZoomIn
			// 
			this.miZoomIn.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomIn;
			this.miZoomIn.Name = "miZoomIn";
			this.miZoomIn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.miZoomIn.Size = new System.Drawing.Size(222, 22);
			this.miZoomIn.Text = "Zoom &In";
			// 
			// miZoomOut
			// 
			this.miZoomOut.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomOut;
			this.miZoomOut.Name = "miZoomOut";
			this.miZoomOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.miZoomOut.Size = new System.Drawing.Size(222, 22);
			this.miZoomOut.Text = "Zoom &Out";
			// 
			// miToggleZoom
			// 
			this.miToggleZoom.Name = "miToggleZoom";
			this.miToggleZoom.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Z)));
			this.miToggleZoom.Size = new System.Drawing.Size(222, 22);
			this.miToggleZoom.Text = "Toggle Zoom";
			// 
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(219, 6);
			// 
			// miZoom100
			// 
			this.miZoom100.Name = "miZoom100";
			this.miZoom100.Size = new System.Drawing.Size(222, 22);
			this.miZoom100.Text = "100%";
			// 
			// miZoom125
			// 
			this.miZoom125.Name = "miZoom125";
			this.miZoom125.Size = new System.Drawing.Size(222, 22);
			this.miZoom125.Text = "125%";
			// 
			// miZoom150
			// 
			this.miZoom150.Name = "miZoom150";
			this.miZoom150.Size = new System.Drawing.Size(222, 22);
			this.miZoom150.Text = "150%";
			// 
			// miZoom200
			// 
			this.miZoom200.Name = "miZoom200";
			this.miZoom200.Size = new System.Drawing.Size(222, 22);
			this.miZoom200.Text = "200%";
			// 
			// miZoom400
			// 
			this.miZoom400.Name = "miZoom400";
			this.miZoom400.Size = new System.Drawing.Size(222, 22);
			this.miZoom400.Text = "400%";
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(219, 6);
			// 
			// miZoomCustom
			// 
			this.miZoomCustom.Name = "miZoomCustom";
			this.miZoomCustom.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
			this.miZoomCustom.Size = new System.Drawing.Size(222, 22);
			this.miZoomCustom.Text = "&Custom...";
			// 
			// miRotation
			// 
			this.miRotation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRotateLeft,
            this.miRotateRight,
            this.toolStripMenuItem33,
            this.miRotate0,
            this.miRotate90,
            this.miRotate180,
            this.miRotate270,
            this.toolStripMenuItem36,
            this.miAutoRotate});
			this.miRotation.Name = "miRotation";
			this.miRotation.Size = new System.Drawing.Size(221, 22);
			this.miRotation.Text = "&Rotation";
			// 
			// miRotateLeft
			// 
			this.miRotateLeft.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateLeft;
			this.miRotateLeft.Name = "miRotateLeft";
			this.miRotateLeft.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.OemMinus)));
			this.miRotateLeft.Size = new System.Drawing.Size(256, 22);
			this.miRotateLeft.Text = "Rotate Left";
			// 
			// miRotateRight
			// 
			this.miRotateRight.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateRight;
			this.miRotateRight.Name = "miRotateRight";
			this.miRotateRight.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Oemplus)));
			this.miRotateRight.Size = new System.Drawing.Size(256, 22);
			this.miRotateRight.Text = "Rotate Right";
			// 
			// toolStripMenuItem33
			// 
			this.toolStripMenuItem33.Name = "toolStripMenuItem33";
			this.toolStripMenuItem33.Size = new System.Drawing.Size(253, 6);
			// 
			// miRotate0
			// 
			this.miRotate0.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate0;
			this.miRotate0.Name = "miRotate0";
			this.miRotate0.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D7)));
			this.miRotate0.Size = new System.Drawing.Size(256, 22);
			this.miRotate0.Text = "&No Rotation";
			// 
			// miRotate90
			// 
			this.miRotate90.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate90;
			this.miRotate90.Name = "miRotate90";
			this.miRotate90.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D8)));
			this.miRotate90.Size = new System.Drawing.Size(256, 22);
			this.miRotate90.Text = "90°";
			// 
			// miRotate180
			// 
			this.miRotate180.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate180;
			this.miRotate180.Name = "miRotate180";
			this.miRotate180.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D9)));
			this.miRotate180.Size = new System.Drawing.Size(256, 22);
			this.miRotate180.Text = "180°";
			// 
			// miRotate270
			// 
			this.miRotate270.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate270;
			this.miRotate270.Name = "miRotate270";
			this.miRotate270.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.miRotate270.Size = new System.Drawing.Size(256, 22);
			this.miRotate270.Text = "270°";
			// 
			// toolStripMenuItem36
			// 
			this.toolStripMenuItem36.Name = "toolStripMenuItem36";
			this.toolStripMenuItem36.Size = new System.Drawing.Size(253, 6);
			// 
			// miAutoRotate
			// 
			this.miAutoRotate.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AutoRotate;
			this.miAutoRotate.Name = "miAutoRotate";
			this.miAutoRotate.Size = new System.Drawing.Size(256, 22);
			this.miAutoRotate.Text = "Autorotate Double Pages";
			// 
			// toolStripMenuItem23
			// 
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			this.toolStripMenuItem23.Size = new System.Drawing.Size(218, 6);
			// 
			// miMinimalGui
			// 
			this.miMinimalGui.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.MenuToggle;
			this.miMinimalGui.Name = "miMinimalGui";
			this.miMinimalGui.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.miMinimalGui.Size = new System.Drawing.Size(221, 22);
			this.miMinimalGui.Text = "Minimal User Interface";
			// 
			// miFullScreen
			// 
			this.miFullScreen.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FullScreen;
			this.miFullScreen.Name = "miFullScreen";
			this.miFullScreen.ShortcutKeyDisplayString = "";
			this.miFullScreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
			this.miFullScreen.Size = new System.Drawing.Size(221, 22);
			this.miFullScreen.Text = "&Full Screen";
			// 
			// miReaderUndocked
			// 
			this.miReaderUndocked.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UndockReader;
			this.miReaderUndocked.Name = "miReaderUndocked";
			this.miReaderUndocked.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.miReaderUndocked.Size = new System.Drawing.Size(221, 22);
			this.miReaderUndocked.Text = "Reader in &own Window";
			// 
			// toolStripMenuItem41
			// 
			this.toolStripMenuItem41.Name = "toolStripMenuItem41";
			this.toolStripMenuItem41.Size = new System.Drawing.Size(218, 6);
			// 
			// miMagnify
			// 
			this.miMagnify.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Zoom;
			this.miMagnify.Name = "miMagnify";
			this.miMagnify.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
			this.miMagnify.Size = new System.Drawing.Size(221, 22);
			this.miMagnify.Text = "&Magnifier";
			// 
			// helpMenu
			// 
			this.helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelp,
            this.miWebHelp,
            this.miHelpPlugins,
            this.miChooseHelpSystem,
            this.miHelpQuickIntro,
            this.toolStripMenuItem3,
            this.miWebHome,
            this.miWebUserForum,
            this.toolStripMenuItem5,
            this.miNews,
            this.miCheckUpdate,
            this.toolStripMenuItem25,
            this.miAbout});
			this.helpMenu.Name = "helpMenu";
			this.helpMenu.Size = new System.Drawing.Size(44, 20);
			this.helpMenu.Text = "&Help";
			// 
			// miHelp
			// 
			this.miHelp.Name = "miHelp";
			this.miHelp.Size = new System.Drawing.Size(256, 22);
			this.miHelp.Text = "Help";
			this.miHelp.Visible = false;
			// 
			// miWebHelp
			// 
			this.miWebHelp.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Help;
			this.miWebHelp.Name = "miWebHelp";
			this.miWebHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.miWebHelp.Size = new System.Drawing.Size(256, 22);
			this.miWebHelp.Text = "ComicRack Documentation...";
			// 
			// miHelpPlugins
			// 
			this.miHelpPlugins.Name = "miHelpPlugins";
			this.miHelpPlugins.Size = new System.Drawing.Size(256, 22);
			this.miHelpPlugins.Text = "Plugins";
			// 
			// miChooseHelpSystem
			// 
			this.miChooseHelpSystem.Name = "miChooseHelpSystem";
			this.miChooseHelpSystem.Size = new System.Drawing.Size(256, 22);
			this.miChooseHelpSystem.Text = "Choose Help System";
			// 
			// miHelpQuickIntro
			// 
			this.miHelpQuickIntro.Name = "miHelpQuickIntro";
			this.miHelpQuickIntro.Size = new System.Drawing.Size(256, 22);
			this.miHelpQuickIntro.Text = "Quick Introduction";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(253, 6);
			// 
			// miWebHome
			// 
			this.miWebHome.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.WebBlog;
			this.miWebHome.Name = "miWebHome";
			this.miWebHome.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F1)));
			this.miWebHome.Size = new System.Drawing.Size(256, 22);
			this.miWebHome.Text = "ComicRack Homepage...";
			// 
			// miWebUserForum
			// 
			this.miWebUserForum.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.WebForum;
			this.miWebUserForum.Name = "miWebUserForum";
			this.miWebUserForum.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F1)));
			this.miWebUserForum.Size = new System.Drawing.Size(256, 22);
			this.miWebUserForum.Text = "ComicRack User Forum...";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(253, 6);
			// 
			// miNews
			// 
			this.miNews.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.News;
			this.miNews.Name = "miNews";
			this.miNews.Size = new System.Drawing.Size(256, 22);
			this.miNews.Text = "&News...";
			// 
			// toolStripMenuItem25
			// 
			this.toolStripMenuItem25.Name = "toolStripMenuItem25";
			this.toolStripMenuItem25.Size = new System.Drawing.Size(253, 6);
			// 
			// miAbout
			// 
			this.miAbout.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.About;
			this.miAbout.Name = "miAbout";
			this.miAbout.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
			this.miAbout.Size = new System.Drawing.Size(256, 22);
			this.miAbout.Text = "&About...";
			// 
			// statusStrip
			// 
			this.statusStrip.AutoSize = false;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsText,
            this.tsDeviceSyncActivity,
            this.tsExportActivity,
            this.tsReadInfoActivity,
            this.tsWriteInfoActivity,
            this.tsPageActivity,
            this.tsScanActivity,
            this.tsDataSourceState,
            this.tsBook,
            this.tsCurrentPage,
            this.tsPageCount,
            this.tsServerActivity});
			this.statusStrip.Location = new System.Drawing.Point(0, 638);
			this.statusStrip.MinimumSize = new System.Drawing.Size(0, 24);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.ShowItemToolTips = true;
			this.statusStrip.Size = new System.Drawing.Size(744, 24);
			this.statusStrip.TabIndex = 3;
			// 
			// tsText
			// 
			this.tsText.Name = "tsText";
			this.tsText.Size = new System.Drawing.Size(603, 19);
			this.tsText.Spring = true;
			this.tsText.Text = "Ready";
			this.tsText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tsDeviceSyncActivity
			// 
			this.tsDeviceSyncActivity.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsDeviceSyncActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsDeviceSyncActivity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsDeviceSyncActivity.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DeviceSyncAnimation;
			this.tsDeviceSyncActivity.Name = "tsDeviceSyncActivity";
			this.tsDeviceSyncActivity.Size = new System.Drawing.Size(20, 19);
			this.tsDeviceSyncActivity.Text = "Exporting";
			this.tsDeviceSyncActivity.Visible = false;
			this.tsDeviceSyncActivity.Click += new System.EventHandler(this.tsDeviceSyncActivity_Click);
			// 
			// tsExportActivity
			// 
			this.tsExportActivity.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsExportActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsExportActivity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsExportActivity.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ExportAnimation;
			this.tsExportActivity.Name = "tsExportActivity";
			this.tsExportActivity.Size = new System.Drawing.Size(20, 19);
			this.tsExportActivity.Text = "Exporting";
			this.tsExportActivity.Visible = false;
			this.tsExportActivity.Click += new System.EventHandler(this.tsExportActivity_Click);
			// 
			// tsReadInfoActivity
			// 
			this.tsReadInfoActivity.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsReadInfoActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsReadInfoActivity.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ReadInfoAnimation;
			this.tsReadInfoActivity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			this.tsReadInfoActivity.Name = "tsReadInfoActivity";
			this.tsReadInfoActivity.Size = new System.Drawing.Size(20, 19);
			this.tsReadInfoActivity.ToolTipText = "Reading info data from files...";
			this.tsReadInfoActivity.Visible = false;
			this.tsReadInfoActivity.Click += new System.EventHandler(this.tsReadInfoActivity_Click);
			// 
			// tsWriteInfoActivity
			// 
			this.tsWriteInfoActivity.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsWriteInfoActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsWriteInfoActivity.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateInfoAnimation;
			this.tsWriteInfoActivity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			this.tsWriteInfoActivity.Name = "tsWriteInfoActivity";
			this.tsWriteInfoActivity.Size = new System.Drawing.Size(20, 19);
			this.tsWriteInfoActivity.ToolTipText = "Writing info data to files...";
			this.tsWriteInfoActivity.Visible = false;
			this.tsWriteInfoActivity.Click += new System.EventHandler(this.tsUpdateInfoActivity_Click);
			// 
			// tsPageActivity
			// 
			this.tsPageActivity.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsPageActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsPageActivity.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ReadPagesAnimation;
			this.tsPageActivity.Name = "tsPageActivity";
			this.tsPageActivity.Size = new System.Drawing.Size(20, 19);
			this.tsPageActivity.ToolTipText = "Getting Pages and Thumbnails...";
			this.tsPageActivity.Visible = false;
			this.tsPageActivity.Click += new System.EventHandler(this.tsPageActivity_Click);
			// 
			// tsScanActivity
			// 
			this.tsScanActivity.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsScanActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsScanActivity.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ScanAnimation;
			this.tsScanActivity.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			this.tsScanActivity.Name = "tsScanActivity";
			this.tsScanActivity.Size = new System.Drawing.Size(20, 19);
			this.tsScanActivity.ToolTipText = "A scan is running...";
			this.tsScanActivity.Visible = false;
			this.tsScanActivity.Click += new System.EventHandler(this.tsScanActivity_Click);
			// 
			// tsDataSourceState
			// 
			this.tsDataSourceState.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsDataSourceState.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsDataSourceState.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
			this.tsDataSourceState.Name = "tsDataSourceState";
			this.tsDataSourceState.Size = new System.Drawing.Size(4, 19);
			this.tsDataSourceState.Visible = false;
			// 
			// tsBook
			// 
			this.tsBook.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsBook.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsBook.Name = "tsBook";
			this.tsBook.Size = new System.Drawing.Size(38, 19);
			this.tsBook.Text = "Book";
			this.tsBook.ToolTipText = "Name of the opened Book";
			// 
			// tsCurrentPage
			// 
			this.tsCurrentPage.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsCurrentPage.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsCurrentPage.Name = "tsCurrentPage";
			this.tsCurrentPage.Size = new System.Drawing.Size(37, 19);
			this.tsCurrentPage.Text = "Page";
			this.tsCurrentPage.ToolTipText = "Current Page of the open Book";
			this.tsCurrentPage.Click += new System.EventHandler(this.tsCurrentPage_Click);
			// 
			// tsPageCount
			// 
			this.tsPageCount.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsPageCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsPageCount.Name = "tsPageCount";
			this.tsPageCount.Size = new System.Drawing.Size(51, 19);
			this.tsPageCount.Text = "0 Pages";
			this.tsPageCount.ToolTipText = "Page count of the open Book";
			// 
			// tsServerActivity
			// 
			this.tsServerActivity.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsServerActivity.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GrayLight;
			this.tsServerActivity.Name = "tsServerActivity";
			this.tsServerActivity.Size = new System.Drawing.Size(16, 19);
			this.tsServerActivity.Visible = false;
			this.tsServerActivity.Click += new System.EventHandler(this.tsServerActivity_Click);
			// 
			// pageContextMenu
			// 
			this.pageContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmShowInfo,
            this.cmRating,
            this.cmPageType,
            this.cmPageRotate,
            this.cmBookmarks,
            this.toolStripSeparator10,
            this.cmComics,
            this.cmPageLayout,
            this.cmMagnify,
            this.toolStripSeparator2,
            this.cmCopyPage,
            this.cmExportPage,
            this.toolStripMenuItem11,
            this.cmRefreshPage,
            this.toolStripMenuItem46,
            this.cmMinimalGui});
			this.pageContextMenu.Name = "pageContextMenu";
			this.pageContextMenu.Size = new System.Drawing.Size(221, 292);
			this.pageContextMenu.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.pageContextMenu_Closed);
			this.pageContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.pageContextMenu_Opening);
			// 
			// cmShowInfo
			// 
			this.cmShowInfo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GetInfo;
			this.cmShowInfo.Name = "cmShowInfo";
			this.cmShowInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.cmShowInfo.Size = new System.Drawing.Size(220, 22);
			this.cmShowInfo.Text = "Info...";
			// 
			// cmRating
			// 
			this.cmRating.DropDown = this.contextRating2;
			this.cmRating.Name = "cmRating";
			this.cmRating.Size = new System.Drawing.Size(220, 22);
			this.cmRating.Text = "My R&ating";
			// 
			// contextRating2
			// 
			this.contextRating2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmRate0,
            this.toolStripMenuItem16,
            this.cmRate1,
            this.cmRate2,
            this.cmRate3,
            this.cmRate4,
            this.cmRate5,
            this.toolStripSeparator6,
            this.cmQuickRating});
			this.contextRating2.Name = "contextRating2";
			this.contextRating2.OwnerItem = this.cmRating;
			this.contextRating2.Size = new System.Drawing.Size(286, 170);
			// 
			// cmRate0
			// 
			this.cmRate0.Name = "cmRate0";
			this.cmRate0.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.cmRate0.Size = new System.Drawing.Size(285, 22);
			this.cmRate0.Text = "None";
			// 
			// toolStripMenuItem16
			// 
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			this.toolStripMenuItem16.Size = new System.Drawing.Size(282, 6);
			// 
			// cmRate1
			// 
			this.cmRate1.Name = "cmRate1";
			this.cmRate1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D1)));
			this.cmRate1.Size = new System.Drawing.Size(285, 22);
			this.cmRate1.Text = "* (1 Star)";
			// 
			// cmRate2
			// 
			this.cmRate2.Name = "cmRate2";
			this.cmRate2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D2)));
			this.cmRate2.Size = new System.Drawing.Size(285, 22);
			this.cmRate2.Text = "** (2 Stars)";
			// 
			// cmRate3
			// 
			this.cmRate3.Name = "cmRate3";
			this.cmRate3.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D3)));
			this.cmRate3.Size = new System.Drawing.Size(285, 22);
			this.cmRate3.Text = "*** (3 Stars)";
			// 
			// cmRate4
			// 
			this.cmRate4.Name = "cmRate4";
			this.cmRate4.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D4)));
			this.cmRate4.Size = new System.Drawing.Size(285, 22);
			this.cmRate4.Text = "**** (4 Stars)";
			// 
			// cmRate5
			// 
			this.cmRate5.Name = "cmRate5";
			this.cmRate5.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D5)));
			this.cmRate5.Size = new System.Drawing.Size(285, 22);
			this.cmRate5.Text = "***** (5 Stars)";
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(282, 6);
			// 
			// cmQuickRating
			// 
			this.cmQuickRating.Name = "cmQuickRating";
			this.cmQuickRating.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
			this.cmQuickRating.Size = new System.Drawing.Size(285, 22);
			this.cmQuickRating.Text = "Quick Rating and Review...";
			// 
			// cmPageType
			// 
			this.cmPageType.Name = "cmPageType";
			this.cmPageType.Size = new System.Drawing.Size(220, 22);
			this.cmPageType.Text = "&Page Type";
			// 
			// cmPageRotate
			// 
			this.cmPageRotate.Name = "cmPageRotate";
			this.cmPageRotate.Size = new System.Drawing.Size(220, 22);
			this.cmPageRotate.Text = "Page Rotation";
			// 
			// cmBookmarks
			// 
			this.cmBookmarks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmSetBookmark,
            this.cmRemoveBookmark,
            this.toolStripMenuItem32,
            this.cmPrevBookmark,
            this.cmNextBookmark,
            this.toolStripSeparator13,
            this.cmLastPageRead,
            this.cmBookmarkSeparator});
			this.cmBookmarks.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Bookmark;
			this.cmBookmarks.Name = "cmBookmarks";
			this.cmBookmarks.Size = new System.Drawing.Size(220, 22);
			this.cmBookmarks.Text = "&Bookmarks";
			this.cmBookmarks.DropDownOpening += new System.EventHandler(this.cmBookmarks_DropDownOpening);
			// 
			// cmSetBookmark
			// 
			this.cmSetBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewBookmark;
			this.cmSetBookmark.Name = "cmSetBookmark";
			this.cmSetBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
			this.cmSetBookmark.Size = new System.Drawing.Size(249, 22);
			this.cmSetBookmark.Text = "Set Bookmark...";
			// 
			// cmRemoveBookmark
			// 
			this.cmRemoveBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoveBookmark;
			this.cmRemoveBookmark.Name = "cmRemoveBookmark";
			this.cmRemoveBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
			this.cmRemoveBookmark.Size = new System.Drawing.Size(249, 22);
			this.cmRemoveBookmark.Text = "Remove Bookmark";
			// 
			// toolStripMenuItem32
			// 
			this.toolStripMenuItem32.Name = "toolStripMenuItem32";
			this.toolStripMenuItem32.Size = new System.Drawing.Size(246, 6);
			// 
			// cmPrevBookmark
			// 
			this.cmPrevBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.PreviousBookmark;
			this.cmPrevBookmark.Name = "cmPrevBookmark";
			this.cmPrevBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
			this.cmPrevBookmark.Size = new System.Drawing.Size(249, 22);
			this.cmPrevBookmark.Text = "Previous Bookmark";
			// 
			// cmNextBookmark
			// 
			this.cmNextBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NextBookmark;
			this.cmNextBookmark.Name = "cmNextBookmark";
			this.cmNextBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.cmNextBookmark.Size = new System.Drawing.Size(249, 22);
			this.cmNextBookmark.Text = "Next Bookmark";
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(246, 6);
			// 
			// cmLastPageRead
			// 
			this.cmLastPageRead.Name = "cmLastPageRead";
			this.cmLastPageRead.Size = new System.Drawing.Size(249, 22);
			this.cmLastPageRead.Text = "L&ast Page Read";
			// 
			// cmBookmarkSeparator
			// 
			this.cmBookmarkSeparator.Name = "cmBookmarkSeparator";
			this.cmBookmarkSeparator.Size = new System.Drawing.Size(246, 6);
			this.cmBookmarkSeparator.Tag = "bms";
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(217, 6);
			// 
			// cmComics
			// 
			this.cmComics.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmOpenComic,
            this.cmCloseComic,
            this.toolStripMenuItem13,
            this.cmPrevFromList,
            this.cmNextFromList,
            this.cmRandomFromList,
            this.cmComicsSep});
			this.cmComics.Name = "cmComics";
			this.cmComics.Size = new System.Drawing.Size(220, 22);
			this.cmComics.Text = "Books";
			// 
			// cmOpenComic
			// 
			this.cmOpenComic.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			this.cmOpenComic.Name = "cmOpenComic";
			this.cmOpenComic.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.cmOpenComic.Size = new System.Drawing.Size(218, 22);
			this.cmOpenComic.Text = "&Open File...";
			// 
			// cmCloseComic
			// 
			this.cmCloseComic.Name = "cmCloseComic";
			this.cmCloseComic.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cmCloseComic.Size = new System.Drawing.Size(218, 22);
			this.cmCloseComic.Text = "&Close";
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(215, 6);
			// 
			// cmPrevFromList
			// 
			this.cmPrevFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.PrevFromList;
			this.cmPrevFromList.Name = "cmPrevFromList";
			this.cmPrevFromList.ShortcutKeyDisplayString = "";
			this.cmPrevFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
			this.cmPrevFromList.Size = new System.Drawing.Size(218, 22);
			this.cmPrevFromList.Text = "Pre&vious Book";
			// 
			// cmNextFromList
			// 
			this.cmNextFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NextFromList;
			this.cmNextFromList.Name = "cmNextFromList";
			this.cmNextFromList.ShortcutKeyDisplayString = "";
			this.cmNextFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.cmNextFromList.Size = new System.Drawing.Size(218, 22);
			this.cmNextFromList.Text = "Ne&xt Book";
			// 
			// cmRandomFromList
			// 
			this.cmRandomFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RandomComic;
			this.cmRandomFromList.Name = "cmRandomFromList";
			this.cmRandomFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.N)));
			this.cmRandomFromList.Size = new System.Drawing.Size(218, 22);
			this.cmRandomFromList.Text = "Random Book";
			// 
			// cmComicsSep
			// 
			this.cmComicsSep.Name = "cmComicsSep";
			this.cmComicsSep.Size = new System.Drawing.Size(215, 6);
			// 
			// cmPageLayout
			// 
			this.cmPageLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmOriginal,
            this.cmFitAll,
            this.cmFitWidth,
            this.cmFitWidthAdaptive,
            this.cmFitHeight,
            this.cmFitBest,
            this.toolStripMenuItem29,
            this.cmSinglePage,
            this.cmTwoPages,
            this.cmTwoPagesAdaptive,
            this.cmRightToLeft,
            this.toolStripMenuItem38,
            this.cmRotate0,
            this.cmRotate90,
            this.cmRotate180,
            this.cmRotate270,
            this.toolStripMenuItem55,
            this.cmOnlyFitOversized});
			this.cmPageLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmPageLayout.Name = "cmPageLayout";
			this.cmPageLayout.Size = new System.Drawing.Size(220, 22);
			this.cmPageLayout.Text = "Page Layout";
			// 
			// cmOriginal
			// 
			this.cmOriginal.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Original;
			this.cmOriginal.Name = "cmOriginal";
			this.cmOriginal.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
			this.cmOriginal.Size = new System.Drawing.Size(241, 22);
			this.cmOriginal.Text = "Original";
			// 
			// cmFitAll
			// 
			this.cmFitAll.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			this.cmFitAll.Name = "cmFitAll";
			this.cmFitAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
			this.cmFitAll.Size = new System.Drawing.Size(241, 22);
			this.cmFitAll.Text = "Fit All";
			// 
			// cmFitWidth
			// 
			this.cmFitWidth.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidth;
			this.cmFitWidth.Name = "cmFitWidth";
			this.cmFitWidth.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
			this.cmFitWidth.Size = new System.Drawing.Size(241, 22);
			this.cmFitWidth.Text = "Fit Width";
			// 
			// cmFitWidthAdaptive
			// 
			this.cmFitWidthAdaptive.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidthAdaptive;
			this.cmFitWidthAdaptive.Name = "cmFitWidthAdaptive";
			this.cmFitWidthAdaptive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
			this.cmFitWidthAdaptive.Size = new System.Drawing.Size(241, 22);
			this.cmFitWidthAdaptive.Text = "Fit Width (adaptive)";
			// 
			// cmFitHeight
			// 
			this.cmFitHeight.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitHeight;
			this.cmFitHeight.Name = "cmFitHeight";
			this.cmFitHeight.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D5)));
			this.cmFitHeight.Size = new System.Drawing.Size(241, 22);
			this.cmFitHeight.Text = "Fit Height";
			// 
			// cmFitBest
			// 
			this.cmFitBest.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitBest;
			this.cmFitBest.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmFitBest.Name = "cmFitBest";
			this.cmFitBest.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D6)));
			this.cmFitBest.Size = new System.Drawing.Size(241, 22);
			this.cmFitBest.Text = "Fit Best";
			// 
			// toolStripMenuItem29
			// 
			this.toolStripMenuItem29.Name = "toolStripMenuItem29";
			this.toolStripMenuItem29.Size = new System.Drawing.Size(238, 6);
			// 
			// cmSinglePage
			// 
			this.cmSinglePage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			this.cmSinglePage.Name = "cmSinglePage";
			this.cmSinglePage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D7)));
			this.cmSinglePage.Size = new System.Drawing.Size(241, 22);
			this.cmSinglePage.Text = "Single Page";
			// 
			// cmTwoPages
			// 
			this.cmTwoPages.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageForced;
			this.cmTwoPages.Name = "cmTwoPages";
			this.cmTwoPages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D8)));
			this.cmTwoPages.Size = new System.Drawing.Size(241, 22);
			this.cmTwoPages.Text = "Two Pages";
			// 
			// cmTwoPagesAdaptive
			// 
			this.cmTwoPagesAdaptive.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
			this.cmTwoPagesAdaptive.Name = "cmTwoPagesAdaptive";
			this.cmTwoPagesAdaptive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D9)));
			this.cmTwoPagesAdaptive.Size = new System.Drawing.Size(241, 22);
			this.cmTwoPagesAdaptive.Text = "Two Pages (adaptive)";
			this.cmTwoPagesAdaptive.ToolTipText = "Show one or two pages";
			// 
			// cmRightToLeft
			// 
			this.cmRightToLeft.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RightToLeft;
			this.cmRightToLeft.Name = "cmRightToLeft";
			this.cmRightToLeft.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
			this.cmRightToLeft.Size = new System.Drawing.Size(241, 22);
			this.cmRightToLeft.Text = "Right to Left";
			// 
			// toolStripMenuItem38
			// 
			this.toolStripMenuItem38.Name = "toolStripMenuItem38";
			this.toolStripMenuItem38.Size = new System.Drawing.Size(238, 6);
			// 
			// cmRotate0
			// 
			this.cmRotate0.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate0;
			this.cmRotate0.Name = "cmRotate0";
			this.cmRotate0.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D7)));
			this.cmRotate0.Size = new System.Drawing.Size(241, 22);
			this.cmRotate0.Text = "&No Rotation";
			// 
			// cmRotate90
			// 
			this.cmRotate90.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate90;
			this.cmRotate90.Name = "cmRotate90";
			this.cmRotate90.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D8)));
			this.cmRotate90.Size = new System.Drawing.Size(241, 22);
			this.cmRotate90.Text = "90°";
			// 
			// cmRotate180
			// 
			this.cmRotate180.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate180;
			this.cmRotate180.Name = "cmRotate180";
			this.cmRotate180.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D9)));
			this.cmRotate180.Size = new System.Drawing.Size(241, 22);
			this.cmRotate180.Text = "180°";
			// 
			// cmRotate270
			// 
			this.cmRotate270.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate270;
			this.cmRotate270.Name = "cmRotate270";
			this.cmRotate270.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.cmRotate270.Size = new System.Drawing.Size(241, 22);
			this.cmRotate270.Text = "270°";
			// 
			// toolStripMenuItem55
			// 
			this.toolStripMenuItem55.Name = "toolStripMenuItem55";
			this.toolStripMenuItem55.Size = new System.Drawing.Size(238, 6);
			// 
			// cmOnlyFitOversized
			// 
			this.cmOnlyFitOversized.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Oversized;
			this.cmOnlyFitOversized.Name = "cmOnlyFitOversized";
			this.cmOnlyFitOversized.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.O)));
			this.cmOnlyFitOversized.Size = new System.Drawing.Size(241, 22);
			this.cmOnlyFitOversized.Text = "&Only fit if oversized";
			// 
			// cmMagnify
			// 
			this.cmMagnify.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Zoom;
			this.cmMagnify.Name = "cmMagnify";
			this.cmMagnify.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
			this.cmMagnify.Size = new System.Drawing.Size(220, 22);
			this.cmMagnify.Text = "&Magnifier";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(217, 6);
			// 
			// cmCopyPage
			// 
			this.cmCopyPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Copy;
			this.cmCopyPage.Name = "cmCopyPage";
			this.cmCopyPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.cmCopyPage.Size = new System.Drawing.Size(220, 22);
			this.cmCopyPage.Text = "&Copy Page";
			// 
			// cmExportPage
			// 
			this.cmExportPage.Name = "cmExportPage";
			this.cmExportPage.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
			this.cmExportPage.Size = new System.Drawing.Size(220, 22);
			this.cmExportPage.Text = "&Export Page...";
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(217, 6);
			// 
			// cmRefreshPage
			// 
			this.cmRefreshPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			this.cmRefreshPage.Name = "cmRefreshPage";
			this.cmRefreshPage.ShortcutKeys = System.Windows.Forms.Keys.F5;
			this.cmRefreshPage.Size = new System.Drawing.Size(220, 22);
			this.cmRefreshPage.Text = "&Refresh";
			// 
			// toolStripMenuItem46
			// 
			this.toolStripMenuItem46.Name = "toolStripMenuItem46";
			this.toolStripMenuItem46.Size = new System.Drawing.Size(217, 6);
			// 
			// cmMinimalGui
			// 
			this.cmMinimalGui.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.MenuToggle;
			this.cmMinimalGui.Name = "cmMinimalGui";
			this.cmMinimalGui.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.cmMinimalGui.Size = new System.Drawing.Size(220, 22);
			this.cmMinimalGui.Text = "&Minimal User Interface";
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.notfifyContextMenu;
			this.notifyIcon.Text = "Double Click to restore";
			this.notifyIcon.BalloonTipClicked += new System.EventHandler(this.notifyIcon_BalloonTipClicked);
			// 
			// notfifyContextMenu
			// 
			this.notfifyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmNotifyRestore,
            this.toolStripMenuItem15,
            this.cmNotifyExit});
			this.notfifyContextMenu.Name = "notfifyContextMenu";
			this.notfifyContextMenu.Size = new System.Drawing.Size(114, 54);
			// 
			// cmNotifyRestore
			// 
			this.cmNotifyRestore.Name = "cmNotifyRestore";
			this.cmNotifyRestore.Size = new System.Drawing.Size(113, 22);
			this.cmNotifyRestore.Text = "&Restore";
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			this.toolStripMenuItem15.Size = new System.Drawing.Size(110, 6);
			// 
			// cmNotifyExit
			// 
			this.cmNotifyExit.Name = "cmNotifyExit";
			this.cmNotifyExit.Size = new System.Drawing.Size(113, 22);
			this.cmNotifyExit.Text = "&Exit";
			// 
			// viewContainer
			// 
			this.viewContainer.Controls.Add(this.panelReader);
			this.viewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.viewContainer.Location = new System.Drawing.Point(0, 24);
			this.viewContainer.Name = "viewContainer";
			this.viewContainer.Size = new System.Drawing.Size(744, 364);
			this.viewContainer.TabIndex = 14;
			// 
			// panelReader
			// 
			this.panelReader.Controls.Add(this.readerContainer);
			this.panelReader.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelReader.Location = new System.Drawing.Point(0, 0);
			this.panelReader.Name = "panelReader";
			this.panelReader.Size = new System.Drawing.Size(744, 364);
			this.panelReader.TabIndex = 2;
			// 
			// readerContainer
			// 
			this.readerContainer.Controls.Add(this.quickOpenView);
			this.readerContainer.Controls.Add(this.fileTabs);
			this.readerContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.readerContainer.Location = new System.Drawing.Point(0, 0);
			this.readerContainer.Name = "readerContainer";
			this.readerContainer.Size = new System.Drawing.Size(744, 364);
			this.readerContainer.TabIndex = 0;
			this.readerContainer.Paint += new System.Windows.Forms.PaintEventHandler(this.readerContainer_Paint);
			// 
			// quickOpenView
			// 
			this.quickOpenView.AllowDrop = true;
			this.quickOpenView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.quickOpenView.BackColor = System.Drawing.SystemColors.Window;
			this.quickOpenView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.quickOpenView.Caption = "Quick Open";
			this.quickOpenView.CaptionMargin = new System.Windows.Forms.Padding(2);
			this.quickOpenView.Location = new System.Drawing.Point(63, 50);
			this.quickOpenView.Margin = new System.Windows.Forms.Padding(12);
			this.quickOpenView.MinimumSize = new System.Drawing.Size(300, 250);
			this.quickOpenView.Name = "quickOpenView";
			this.quickOpenView.ShowBrowserCommand = true;
			this.quickOpenView.Size = new System.Drawing.Size(616, 289);
			this.quickOpenView.TabIndex = 2;
			this.quickOpenView.ThumbnailSize = 128;
			this.quickOpenView.Visible = false;
			this.quickOpenView.BookActivated += new System.EventHandler(this.QuickOpenBookActivated);
			this.quickOpenView.ShowBrowser += new System.EventHandler(this.quickOpenView_ShowBrowser);
			this.quickOpenView.OpenFile += new System.EventHandler(this.quickOpenView_OpenFile);
			this.quickOpenView.VisibleChanged += new System.EventHandler(this.QuickOpenVisibleChanged);
			this.quickOpenView.DragDrop += new System.Windows.Forms.DragEventHandler(this.BookDragDrop);
			this.quickOpenView.DragEnter += new System.Windows.Forms.DragEventHandler(this.BookDragEnter);
			// 
			// fileTabs
			// 
			this.fileTabs.AllowDrop = true;
			this.fileTabs.CloseImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Close;
			this.fileTabs.Controls.Add(this.mainToolStrip);
			this.fileTabs.Dock = System.Windows.Forms.DockStyle.Top;
			this.fileTabs.DragDropReorder = true;
			this.fileTabs.LeftIndent = 8;
			this.fileTabs.Location = new System.Drawing.Point(0, 0);
			this.fileTabs.Name = "fileTabs";
			this.fileTabs.OwnerDrawnTooltips = true;
			this.fileTabs.Size = new System.Drawing.Size(744, 31);
			this.fileTabs.TabIndex = 1;
			// 
			// mainToolStrip
			// 
			this.mainToolStrip.BackColor = System.Drawing.Color.Transparent;
			this.mainToolStrip.Dock = System.Windows.Forms.DockStyle.Right;
			this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPrevPage,
            this.tbNextPage,
            this.toolStripSeparator5,
            this.tbPageLayout,
            this.tbFit,
            this.tbZoom,
            this.tbRotate,
            this.toolStripSeparator7,
            this.tbMagnify,
            this.tbFullScreen,
            this.toolStripSeparator1,
            this.tbTools});
			this.mainToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.mainToolStrip.Location = new System.Drawing.Point(400, 1);
			this.mainToolStrip.MinimumSize = new System.Drawing.Size(0, 24);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new System.Drawing.Size(344, 25);
			this.mainToolStrip.TabIndex = 2;
			this.mainToolStrip.Text = "mainToolStrip";
			// 
			// tbPrevPage
			// 
			this.tbPrevPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbPrevPage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbFirstPage,
            this.tbPrevBookmark,
            this.toolStripMenuItem53,
            this.toolStripMenuItem19,
            this.tbPrevFromList});
			this.tbPrevPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
			this.tbPrevPage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbPrevPage.Name = "tbPrevPage";
			this.tbPrevPage.Size = new System.Drawing.Size(32, 22);
			this.tbPrevPage.Text = "Previous Page";
			this.tbPrevPage.DropDownOpening += new System.EventHandler(this.tbPrevPage_DropDownOpening);
			// 
			// tbFirstPage
			// 
			this.tbFirstPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
			this.tbFirstPage.Name = "tbFirstPage";
			this.tbFirstPage.ShortcutKeyDisplayString = "";
			this.tbFirstPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
			this.tbFirstPage.Size = new System.Drawing.Size(268, 22);
			this.tbFirstPage.Text = "&First Page";
			// 
			// tbPrevBookmark
			// 
			this.tbPrevBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.PreviousBookmark;
			this.tbPrevBookmark.Name = "tbPrevBookmark";
			this.tbPrevBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
			this.tbPrevBookmark.Size = new System.Drawing.Size(268, 22);
			this.tbPrevBookmark.Text = "Previous Bookmark";
			// 
			// toolStripMenuItem53
			// 
			this.toolStripMenuItem53.Name = "toolStripMenuItem53";
			this.toolStripMenuItem53.Size = new System.Drawing.Size(265, 6);
			this.toolStripMenuItem53.Tag = "bms";
			// 
			// toolStripMenuItem19
			// 
			this.toolStripMenuItem19.Name = "toolStripMenuItem19";
			this.toolStripMenuItem19.Size = new System.Drawing.Size(265, 6);
			// 
			// tbPrevFromList
			// 
			this.tbPrevFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.PrevFromList;
			this.tbPrevFromList.Name = "tbPrevFromList";
			this.tbPrevFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
			this.tbPrevFromList.Size = new System.Drawing.Size(268, 22);
			this.tbPrevFromList.Text = "Previous Book from List";
			// 
			// tbNextPage
			// 
			this.tbNextPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbNextPage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbLastPage,
            this.tbNextBookmark,
            this.tbLastPageRead,
            this.toolStripMenuItem28,
            this.toolStripMenuItem49,
            this.tbNextFromList,
            this.tbRandomFromList});
			this.tbNextPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
			this.tbNextPage.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbNextPage.Name = "tbNextPage";
			this.tbNextPage.Size = new System.Drawing.Size(32, 22);
			this.tbNextPage.Text = "Next Page";
			this.tbNextPage.DropDownOpening += new System.EventHandler(this.tbNextPage_DropDownOpening);
			// 
			// tbLastPage
			// 
			this.tbLastPage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
			this.tbLastPage.Name = "tbLastPage";
			this.tbLastPage.ShortcutKeyDisplayString = "";
			this.tbLastPage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
			this.tbLastPage.Size = new System.Drawing.Size(250, 22);
			this.tbLastPage.Text = "&Last Page";
			// 
			// tbNextBookmark
			// 
			this.tbNextBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NextBookmark;
			this.tbNextBookmark.Name = "tbNextBookmark";
			this.tbNextBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.tbNextBookmark.Size = new System.Drawing.Size(250, 22);
			this.tbNextBookmark.Text = "Next Bookmark";
			// 
			// tbLastPageRead
			// 
			this.tbLastPageRead.Name = "tbLastPageRead";
			this.tbLastPageRead.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.L)));
			this.tbLastPageRead.Size = new System.Drawing.Size(250, 22);
			this.tbLastPageRead.Text = "L&ast Page Read";
			// 
			// toolStripMenuItem28
			// 
			this.toolStripMenuItem28.Name = "toolStripMenuItem28";
			this.toolStripMenuItem28.Size = new System.Drawing.Size(247, 6);
			this.toolStripMenuItem28.Tag = "bms";
			// 
			// toolStripMenuItem49
			// 
			this.toolStripMenuItem49.Name = "toolStripMenuItem49";
			this.toolStripMenuItem49.Size = new System.Drawing.Size(247, 6);
			// 
			// tbNextFromList
			// 
			this.tbNextFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NextFromList;
			this.tbNextFromList.Name = "tbNextFromList";
			this.tbNextFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.N)));
			this.tbNextFromList.Size = new System.Drawing.Size(250, 22);
			this.tbNextFromList.Text = "Next Book from List";
			// 
			// tbRandomFromList
			// 
			this.tbRandomFromList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RandomComic;
			this.tbRandomFromList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbRandomFromList.Name = "tbRandomFromList";
			this.tbRandomFromList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.N)));
			this.tbRandomFromList.Size = new System.Drawing.Size(250, 22);
			this.tbRandomFromList.Text = "Random Book";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// tbPageLayout
			// 
			this.tbPageLayout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbPageLayout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSinglePage,
            this.tbTwoPages,
            this.tbTwoPagesAdaptive,
            this.toolStripMenuItem54,
            this.tbRightToLeft});
			this.tbPageLayout.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			this.tbPageLayout.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbPageLayout.Name = "tbPageLayout";
			this.tbPageLayout.Size = new System.Drawing.Size(32, 22);
			this.tbPageLayout.Text = "Page Layout";
			// 
			// tbSinglePage
			// 
			this.tbSinglePage.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SinglePage;
			this.tbSinglePage.Name = "tbSinglePage";
			this.tbSinglePage.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D7)));
			this.tbSinglePage.Size = new System.Drawing.Size(225, 22);
			this.tbSinglePage.Text = "Single Page";
			// 
			// tbTwoPages
			// 
			this.tbTwoPages.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPageForced;
			this.tbTwoPages.Name = "tbTwoPages";
			this.tbTwoPages.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D8)));
			this.tbTwoPages.Size = new System.Drawing.Size(225, 22);
			this.tbTwoPages.Text = "Two Pages";
			// 
			// tbTwoPagesAdaptive
			// 
			this.tbTwoPagesAdaptive.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
			this.tbTwoPagesAdaptive.Name = "tbTwoPagesAdaptive";
			this.tbTwoPagesAdaptive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D9)));
			this.tbTwoPagesAdaptive.Size = new System.Drawing.Size(225, 22);
			this.tbTwoPagesAdaptive.Text = "Two Pages (adaptive)";
			this.tbTwoPagesAdaptive.ToolTipText = "Show one or two pages";
			// 
			// toolStripMenuItem54
			// 
			this.toolStripMenuItem54.Name = "toolStripMenuItem54";
			this.toolStripMenuItem54.Size = new System.Drawing.Size(222, 6);
			// 
			// tbRightToLeft
			// 
			this.tbRightToLeft.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RightToLeft;
			this.tbRightToLeft.Name = "tbRightToLeft";
			this.tbRightToLeft.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0)));
			this.tbRightToLeft.Size = new System.Drawing.Size(225, 22);
			this.tbRightToLeft.Text = "Right to Left";
			// 
			// tbFit
			// 
			this.tbFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbFit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbOriginal,
            this.tbFitAll,
            this.tbFitWidth,
            this.tbFitWidthAdaptive,
            this.tbFitHeight,
            this.tbBestFit,
            this.toolStripMenuItem20,
            this.tbOnlyFitOversized});
			this.tbFit.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			this.tbFit.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbFit.Name = "tbFit";
			this.tbFit.Size = new System.Drawing.Size(32, 22);
			this.tbFit.Text = "Fit";
			this.tbFit.ToolTipText = "Toggle Fit Mode";
			// 
			// tbOriginal
			// 
			this.tbOriginal.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Original;
			this.tbOriginal.Name = "tbOriginal";
			this.tbOriginal.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
			this.tbOriginal.Size = new System.Drawing.Size(247, 22);
			this.tbOriginal.Text = "Original Size";
			// 
			// tbFitAll
			// 
			this.tbFitAll.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitAll;
			this.tbFitAll.Name = "tbFitAll";
			this.tbFitAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
			this.tbFitAll.Size = new System.Drawing.Size(247, 22);
			this.tbFitAll.Text = "Fit All";
			// 
			// tbFitWidth
			// 
			this.tbFitWidth.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidth;
			this.tbFitWidth.Name = "tbFitWidth";
			this.tbFitWidth.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
			this.tbFitWidth.Size = new System.Drawing.Size(247, 22);
			this.tbFitWidth.Text = "Fit Width";
			// 
			// tbFitWidthAdaptive
			// 
			this.tbFitWidthAdaptive.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitWidthAdaptive;
			this.tbFitWidthAdaptive.Name = "tbFitWidthAdaptive";
			this.tbFitWidthAdaptive.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
			this.tbFitWidthAdaptive.Size = new System.Drawing.Size(247, 22);
			this.tbFitWidthAdaptive.Text = "Fit Width (adaptive)";
			// 
			// tbFitHeight
			// 
			this.tbFitHeight.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitHeight;
			this.tbFitHeight.Name = "tbFitHeight";
			this.tbFitHeight.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D5)));
			this.tbFitHeight.Size = new System.Drawing.Size(247, 22);
			this.tbFitHeight.Text = "Fit Height";
			// 
			// tbBestFit
			// 
			this.tbBestFit.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FitBest;
			this.tbBestFit.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbBestFit.Name = "tbBestFit";
			this.tbBestFit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D6)));
			this.tbBestFit.Size = new System.Drawing.Size(247, 22);
			this.tbBestFit.Text = "Fit Best";
			// 
			// toolStripMenuItem20
			// 
			this.toolStripMenuItem20.Name = "toolStripMenuItem20";
			this.toolStripMenuItem20.Size = new System.Drawing.Size(244, 6);
			// 
			// tbOnlyFitOversized
			// 
			this.tbOnlyFitOversized.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Oversized;
			this.tbOnlyFitOversized.Name = "tbOnlyFitOversized";
			this.tbOnlyFitOversized.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.tbOnlyFitOversized.Size = new System.Drawing.Size(247, 22);
			this.tbOnlyFitOversized.Text = "&Only fit if oversized";
			// 
			// tbZoom
			// 
			this.tbZoom.AutoToolTip = false;
			this.tbZoom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbZoomIn,
            this.tbZoomOut,
            this.toolStripMenuItem30,
            this.tbZoom100,
            this.tbZoom125,
            this.tbZoom150,
            this.tbZoom200,
            this.tbZoom400,
            this.toolStripMenuItem31,
            this.tbZoomCustom});
			this.tbZoom.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomIn;
			this.tbZoom.Name = "tbZoom";
			this.tbZoom.Size = new System.Drawing.Size(70, 22);
			this.tbZoom.Text = "100 %";
			this.tbZoom.ToolTipText = "Change the page zoom";
			// 
			// tbZoomIn
			// 
			this.tbZoomIn.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomIn;
			this.tbZoomIn.Name = "tbZoomIn";
			this.tbZoomIn.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
			this.tbZoomIn.Size = new System.Drawing.Size(222, 22);
			this.tbZoomIn.Text = "Zoom &In";
			// 
			// tbZoomOut
			// 
			this.tbZoomOut.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ZoomOut;
			this.tbZoomOut.Name = "tbZoomOut";
			this.tbZoomOut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
			this.tbZoomOut.Size = new System.Drawing.Size(222, 22);
			this.tbZoomOut.Text = "Zoom &Out";
			// 
			// toolStripMenuItem30
			// 
			this.toolStripMenuItem30.Name = "toolStripMenuItem30";
			this.toolStripMenuItem30.Size = new System.Drawing.Size(219, 6);
			// 
			// tbZoom100
			// 
			this.tbZoom100.Name = "tbZoom100";
			this.tbZoom100.Size = new System.Drawing.Size(222, 22);
			this.tbZoom100.Text = "100%";
			// 
			// tbZoom125
			// 
			this.tbZoom125.Name = "tbZoom125";
			this.tbZoom125.Size = new System.Drawing.Size(222, 22);
			this.tbZoom125.Text = "125%";
			// 
			// tbZoom150
			// 
			this.tbZoom150.Name = "tbZoom150";
			this.tbZoom150.Size = new System.Drawing.Size(222, 22);
			this.tbZoom150.Text = "150%";
			// 
			// tbZoom200
			// 
			this.tbZoom200.Name = "tbZoom200";
			this.tbZoom200.Size = new System.Drawing.Size(222, 22);
			this.tbZoom200.Text = "200%";
			// 
			// tbZoom400
			// 
			this.tbZoom400.Name = "tbZoom400";
			this.tbZoom400.Size = new System.Drawing.Size(222, 22);
			this.tbZoom400.Text = "400%";
			// 
			// toolStripMenuItem31
			// 
			this.toolStripMenuItem31.Name = "toolStripMenuItem31";
			this.toolStripMenuItem31.Size = new System.Drawing.Size(219, 6);
			// 
			// tbZoomCustom
			// 
			this.tbZoomCustom.Name = "tbZoomCustom";
			this.tbZoomCustom.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
			this.tbZoomCustom.Size = new System.Drawing.Size(222, 22);
			this.tbZoomCustom.Text = "&Custom...";
			// 
			// tbRotate
			// 
			this.tbRotate.AutoToolTip = false;
			this.tbRotate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbRotateLeft,
            this.tbRotateRight,
            this.toolStripSeparator11,
            this.tbRotate0,
            this.tbRotate90,
            this.tbRotate180,
            this.tbRotate270,
            this.toolStripMenuItem34,
            this.tbAutoRotate});
			this.tbRotate.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateRight;
			this.tbRotate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbRotate.Name = "tbRotate";
			this.tbRotate.Size = new System.Drawing.Size(50, 22);
			this.tbRotate.Text = "0°";
			this.tbRotate.ToolTipText = "Change the page rotation";
			// 
			// tbRotateLeft
			// 
			this.tbRotateLeft.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateLeft;
			this.tbRotateLeft.Name = "tbRotateLeft";
			this.tbRotateLeft.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.OemMinus)));
			this.tbRotateLeft.Size = new System.Drawing.Size(256, 22);
			this.tbRotateLeft.Text = "Rotate Left";
			// 
			// tbRotateRight
			// 
			this.tbRotateRight.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RotateRight;
			this.tbRotateRight.Name = "tbRotateRight";
			this.tbRotateRight.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Oemplus)));
			this.tbRotateRight.Size = new System.Drawing.Size(256, 22);
			this.tbRotateRight.Text = "Rotate Right";
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(253, 6);
			// 
			// tbRotate0
			// 
			this.tbRotate0.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate0;
			this.tbRotate0.Name = "tbRotate0";
			this.tbRotate0.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D7)));
			this.tbRotate0.Size = new System.Drawing.Size(256, 22);
			this.tbRotate0.Text = "&No Rotation";
			// 
			// tbRotate90
			// 
			this.tbRotate90.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate90;
			this.tbRotate90.Name = "tbRotate90";
			this.tbRotate90.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D8)));
			this.tbRotate90.Size = new System.Drawing.Size(256, 22);
			this.tbRotate90.Text = "90°";
			// 
			// tbRotate180
			// 
			this.tbRotate180.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate180;
			this.tbRotate180.Name = "tbRotate180";
			this.tbRotate180.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D9)));
			this.tbRotate180.Size = new System.Drawing.Size(256, 22);
			this.tbRotate180.Text = "180°";
			// 
			// tbRotate270
			// 
			this.tbRotate270.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rotate270;
			this.tbRotate270.Name = "tbRotate270";
			this.tbRotate270.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.tbRotate270.Size = new System.Drawing.Size(256, 22);
			this.tbRotate270.Text = "270°";
			// 
			// toolStripMenuItem34
			// 
			this.toolStripMenuItem34.Name = "toolStripMenuItem34";
			this.toolStripMenuItem34.Size = new System.Drawing.Size(253, 6);
			// 
			// tbAutoRotate
			// 
			this.tbAutoRotate.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AutoRotate;
			this.tbAutoRotate.Name = "tbAutoRotate";
			this.tbAutoRotate.Size = new System.Drawing.Size(256, 22);
			this.tbAutoRotate.Text = "Autorotate Double Pages";
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// tbMagnify
			// 
			this.tbMagnify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbMagnify.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Zoom;
			this.tbMagnify.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbMagnify.Name = "tbMagnify";
			this.tbMagnify.Size = new System.Drawing.Size(32, 22);
			this.tbMagnify.Text = "Magnifier";
			// 
			// tbFullScreen
			// 
			this.tbFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbFullScreen.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.FullScreen;
			this.tbFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbFullScreen.Name = "tbFullScreen";
			this.tbFullScreen.Size = new System.Drawing.Size(23, 22);
			this.tbFullScreen.Text = "Full Screen";
			this.tbFullScreen.ToolTipText = "Full Screen";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tbTools
			// 
			this.tbTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbTools.DropDown = this.toolsContextMenu;
			this.tbTools.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Tools;
			this.tbTools.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbTools.Name = "tbTools";
			this.tbTools.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this.tbTools.ShowDropDownArrow = false;
			this.tbTools.Size = new System.Drawing.Size(20, 22);
			this.tbTools.Text = "Tools";
			this.tbTools.DropDownOpening += new System.EventHandler(this.tbTools_DropDownOpening);
			// 
			// toolsContextMenu
			// 
			this.toolsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbOpenComic,
            this.tbOpenRemoteLibrary,
            this.tbShowInfo,
            this.toolStripMenuItem47,
            this.tsWorkspaces,
            this.tbBookmarks,
            this.tbAutoScroll,
            this.toolStripMenuItem45,
            this.tbMinimalGui,
            this.tbReaderUndocked,
            this.toolStripMenuItem52,
            this.tbScan,
            this.tbUpdateAllComicFiles,
            this.tbUpdateWebComics,
            this.tsSynchronizeDevices,
            this.toolStripMenuItem48,
            this.tbComicDisplaySettings,
            this.tbPreferences,
            this.tbAbout,
            this.toolStripMenuItem50,
            this.tbShowMainMenu,
            this.toolStripMenuItem51,
            this.tbExit});
			this.toolsContextMenu.Name = "toolsContextMenu";
			this.toolsContextMenu.OwnerItem = this.tbTools;
			this.toolsContextMenu.Size = new System.Drawing.Size(269, 414);
			// 
			// tbOpenComic
			// 
			this.tbOpenComic.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			this.tbOpenComic.Name = "tbOpenComic";
			this.tbOpenComic.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.tbOpenComic.Size = new System.Drawing.Size(268, 22);
			this.tbOpenComic.Text = "&Open Book...";
			// 
			// tbOpenRemoteLibrary
			// 
			this.tbOpenRemoteLibrary.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoteDatabase;
			this.tbOpenRemoteLibrary.Name = "tbOpenRemoteLibrary";
			this.tbOpenRemoteLibrary.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
			this.tbOpenRemoteLibrary.Size = new System.Drawing.Size(268, 22);
			this.tbOpenRemoteLibrary.Text = "Open Remote Library...";
			// 
			// tbShowInfo
			// 
			this.tbShowInfo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GetInfo;
			this.tbShowInfo.Name = "tbShowInfo";
			this.tbShowInfo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.tbShowInfo.Size = new System.Drawing.Size(268, 22);
			this.tbShowInfo.Text = "Info...";
			this.tbShowInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
			// 
			// toolStripMenuItem47
			// 
			this.toolStripMenuItem47.Name = "toolStripMenuItem47";
			this.toolStripMenuItem47.Size = new System.Drawing.Size(265, 6);
			// 
			// tsWorkspaces
			// 
			this.tsWorkspaces.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSaveWorkspace,
            this.tsEditWorkspaces,
            this.tsWorkspaceSep});
			this.tsWorkspaces.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Workspace;
			this.tsWorkspaces.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsWorkspaces.Name = "tsWorkspaces";
			this.tsWorkspaces.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.tsWorkspaces.Size = new System.Drawing.Size(268, 22);
			this.tsWorkspaces.Text = "Workspaces";
			// 
			// tsSaveWorkspace
			// 
			this.tsSaveWorkspace.Name = "tsSaveWorkspace";
			this.tsSaveWorkspace.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.tsSaveWorkspace.Size = new System.Drawing.Size(237, 22);
			this.tsSaveWorkspace.Text = "&Save Workspace...";
			// 
			// tsEditWorkspaces
			// 
			this.tsEditWorkspaces.Name = "tsEditWorkspaces";
			this.tsEditWorkspaces.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.W)));
			this.tsEditWorkspaces.Size = new System.Drawing.Size(237, 22);
			this.tsEditWorkspaces.Text = "&Edit Workspaces...";
			// 
			// tsWorkspaceSep
			// 
			this.tsWorkspaceSep.Name = "tsWorkspaceSep";
			this.tsWorkspaceSep.Size = new System.Drawing.Size(234, 6);
			// 
			// tbBookmarks
			// 
			this.tbBookmarks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSetBookmark,
            this.tbRemoveBookmark,
            this.tbBookmarkSeparator});
			this.tbBookmarks.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Bookmark;
			this.tbBookmarks.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbBookmarks.Name = "tbBookmarks";
			this.tbBookmarks.Size = new System.Drawing.Size(268, 22);
			this.tbBookmarks.Text = "Bookmarks";
			this.tbBookmarks.DropDownOpening += new System.EventHandler(this.tbBookmarks_DropDownOpening);
			// 
			// tbSetBookmark
			// 
			this.tbSetBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewBookmark;
			this.tbSetBookmark.Name = "tbSetBookmark";
			this.tbSetBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.B)));
			this.tbSetBookmark.Size = new System.Drawing.Size(248, 22);
			this.tbSetBookmark.Text = "Set Bookmark...";
			// 
			// tbRemoveBookmark
			// 
			this.tbRemoveBookmark.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RemoveBookmark;
			this.tbRemoveBookmark.Name = "tbRemoveBookmark";
			this.tbRemoveBookmark.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D)));
			this.tbRemoveBookmark.Size = new System.Drawing.Size(248, 22);
			this.tbRemoveBookmark.Text = "Remove Bookmark";
			// 
			// tbBookmarkSeparator
			// 
			this.tbBookmarkSeparator.Name = "tbBookmarkSeparator";
			this.tbBookmarkSeparator.Size = new System.Drawing.Size(245, 6);
			this.tbBookmarkSeparator.Tag = "bms";
			// 
			// tbAutoScroll
			// 
			this.tbAutoScroll.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.CursorScroll;
			this.tbAutoScroll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbAutoScroll.Name = "tbAutoScroll";
			this.tbAutoScroll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.tbAutoScroll.Size = new System.Drawing.Size(268, 22);
			this.tbAutoScroll.Text = "Auto Scrolling";
			// 
			// toolStripMenuItem45
			// 
			this.toolStripMenuItem45.Name = "toolStripMenuItem45";
			this.toolStripMenuItem45.Size = new System.Drawing.Size(265, 6);
			// 
			// tbMinimalGui
			// 
			this.tbMinimalGui.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.MenuToggle;
			this.tbMinimalGui.Name = "tbMinimalGui";
			this.tbMinimalGui.ShortcutKeys = System.Windows.Forms.Keys.F10;
			this.tbMinimalGui.Size = new System.Drawing.Size(268, 22);
			this.tbMinimalGui.Text = "Minimal User Interface";
			// 
			// tbReaderUndocked
			// 
			this.tbReaderUndocked.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UndockReader;
			this.tbReaderUndocked.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbReaderUndocked.Name = "tbReaderUndocked";
			this.tbReaderUndocked.ShortcutKeys = System.Windows.Forms.Keys.F12;
			this.tbReaderUndocked.Size = new System.Drawing.Size(268, 22);
			this.tbReaderUndocked.Text = "Reader in own Window";
			// 
			// toolStripMenuItem52
			// 
			this.toolStripMenuItem52.Name = "toolStripMenuItem52";
			this.toolStripMenuItem52.Size = new System.Drawing.Size(265, 6);
			// 
			// tbScan
			// 
			this.tbScan.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Scan;
			this.tbScan.Name = "tbScan";
			this.tbScan.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.tbScan.Size = new System.Drawing.Size(268, 22);
			this.tbScan.Text = "Scan Book &Folders";
			// 
			// tbUpdateAllComicFiles
			// 
			this.tbUpdateAllComicFiles.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateSmall;
			this.tbUpdateAllComicFiles.Name = "tbUpdateAllComicFiles";
			this.tbUpdateAllComicFiles.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
			this.tbUpdateAllComicFiles.Size = new System.Drawing.Size(268, 22);
			this.tbUpdateAllComicFiles.Text = "Update all Book Files";
			// 
			// tbUpdateWebComics
			// 
			this.tbUpdateWebComics.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateWeb;
			this.tbUpdateWebComics.Name = "tbUpdateWebComics";
			this.tbUpdateWebComics.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.W)));
			this.tbUpdateWebComics.Size = new System.Drawing.Size(268, 22);
			this.tbUpdateWebComics.Text = "Update Web Comics";
			// 
			// tsSynchronizeDevices
			// 
			this.tsSynchronizeDevices.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DeviceSync;
			this.tsSynchronizeDevices.Name = "tsSynchronizeDevices";
			this.tsSynchronizeDevices.Size = new System.Drawing.Size(268, 22);
			this.tsSynchronizeDevices.Text = "Synchronize Devices";
			// 
			// toolStripMenuItem48
			// 
			this.toolStripMenuItem48.Name = "toolStripMenuItem48";
			this.toolStripMenuItem48.Size = new System.Drawing.Size(265, 6);
			// 
			// tbComicDisplaySettings
			// 
			this.tbComicDisplaySettings.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DisplaySettings;
			this.tbComicDisplaySettings.Name = "tbComicDisplaySettings";
			this.tbComicDisplaySettings.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.tbComicDisplaySettings.Size = new System.Drawing.Size(268, 22);
			this.tbComicDisplaySettings.Text = "Book Display Settings...";
			// 
			// tbPreferences
			// 
			this.tbPreferences.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Preferences;
			this.tbPreferences.Name = "tbPreferences";
			this.tbPreferences.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F9)));
			this.tbPreferences.Size = new System.Drawing.Size(268, 22);
			this.tbPreferences.Text = "&Preferences...";
			// 
			// tbAbout
			// 
			this.tbAbout.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.About;
			this.tbAbout.Name = "tbAbout";
			this.tbAbout.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F1)));
			this.tbAbout.Size = new System.Drawing.Size(268, 22);
			this.tbAbout.Text = "&About...";
			// 
			// toolStripMenuItem50
			// 
			this.toolStripMenuItem50.Name = "toolStripMenuItem50";
			this.toolStripMenuItem50.Size = new System.Drawing.Size(265, 6);
			// 
			// tbShowMainMenu
			// 
			this.tbShowMainMenu.Name = "tbShowMainMenu";
			this.tbShowMainMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F10)));
			this.tbShowMainMenu.Size = new System.Drawing.Size(268, 22);
			this.tbShowMainMenu.Text = "Show Main Menu";
			// 
			// toolStripMenuItem51
			// 
			this.toolStripMenuItem51.Name = "toolStripMenuItem51";
			this.toolStripMenuItem51.Size = new System.Drawing.Size(265, 6);
			// 
			// tbExit
			// 
			this.tbExit.Name = "tbExit";
			this.tbExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.tbExit.Size = new System.Drawing.Size(268, 22);
			this.tbExit.Text = "&Exit";
			// 
			// tabContextMenu
			// 
			this.tabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmClose,
            this.cmCloseAllButThis,
            this.cmCloseAllToTheRight,
            this.toolStripMenuItem35,
            this.cmSyncBrowser,
            this.sepBeforeRevealInBrowser,
            this.cmRevealInExplorer,
            this.cmCopyPath});
			this.tabContextMenu.Name = "tabContextMenu";
			this.tabContextMenu.Size = new System.Drawing.Size(221, 148);
			this.tabContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.tabContextMenu_Opening);
			// 
			// cmClose
			// 
			this.cmClose.Name = "cmClose";
			this.cmClose.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cmClose.Size = new System.Drawing.Size(220, 22);
			this.cmClose.Text = "Close";
			// 
			// cmCloseAllButThis
			// 
			this.cmCloseAllButThis.Name = "cmCloseAllButThis";
			this.cmCloseAllButThis.Size = new System.Drawing.Size(220, 22);
			this.cmCloseAllButThis.Text = "Close All But This";
			// 
			// cmCloseAllToTheRight
			// 
			this.cmCloseAllToTheRight.Name = "cmCloseAllToTheRight";
			this.cmCloseAllToTheRight.Size = new System.Drawing.Size(220, 22);
			this.cmCloseAllToTheRight.Text = "Close All to the Right";
			// 
			// toolStripMenuItem35
			// 
			this.toolStripMenuItem35.Name = "toolStripMenuItem35";
			this.toolStripMenuItem35.Size = new System.Drawing.Size(217, 6);
			// 
			// cmSyncBrowser
			// 
			this.cmSyncBrowser.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SyncBrowser;
			this.cmSyncBrowser.Name = "cmSyncBrowser";
			this.cmSyncBrowser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F3)));
			this.cmSyncBrowser.Size = new System.Drawing.Size(220, 22);
			this.cmSyncBrowser.Text = "Show in &Browser";
			// 
			// sepBeforeRevealInBrowser
			// 
			this.sepBeforeRevealInBrowser.Name = "sepBeforeRevealInBrowser";
			this.sepBeforeRevealInBrowser.Size = new System.Drawing.Size(217, 6);
			// 
			// cmRevealInExplorer
			// 
			this.cmRevealInExplorer.Name = "cmRevealInExplorer";
			this.cmRevealInExplorer.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.cmRevealInExplorer.Size = new System.Drawing.Size(220, 22);
			this.cmRevealInExplorer.Text = "Reveal in Explorer";
			// 
			// cmCopyPath
			// 
			this.cmCopyPath.Name = "cmCopyPath";
			this.cmCopyPath.Size = new System.Drawing.Size(220, 22);
			this.cmCopyPath.Text = "Copy Full Path to Clipboard";
			// 
			// trimTimer
			// 
			this.trimTimer.Enabled = true;
			this.trimTimer.Interval = 5000;
			this.trimTimer.Tick += new System.EventHandler(this.trimTimer_Tick);
			// 
			// mainViewContainer
			// 
			this.mainViewContainer.AutoGripPosition = true;
			this.mainViewContainer.BackColor = System.Drawing.SystemColors.Control;
			this.mainViewContainer.Controls.Add(this.mainView);
			this.mainViewContainer.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.mainViewContainer.Location = new System.Drawing.Point(0, 388);
			this.mainViewContainer.Name = "mainViewContainer";
			this.mainViewContainer.Size = new System.Drawing.Size(744, 250);
			this.mainViewContainer.TabIndex = 2;
			this.mainViewContainer.ExpandedChanged += new System.EventHandler(this.mainViewContainer_ExpandedChanged);
			this.mainViewContainer.DockChanged += new System.EventHandler(this.mainViewContainer_DockChanged);
			// 
			// mainView
			// 
			this.mainView.BackColor = System.Drawing.Color.Transparent;
			this.mainView.Caption = "";
			this.mainView.CaptionMargin = new System.Windows.Forms.Padding(2);
			this.mainView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainView.InfoPanelRight = false;
			this.mainView.Location = new System.Drawing.Point(0, 6);
			this.mainView.Margin = new System.Windows.Forms.Padding(6);
			this.mainView.Name = "mainView";
			this.mainView.Size = new System.Drawing.Size(744, 244);
			this.mainView.TabBarVisible = true;
			this.mainView.TabIndex = 0;
			this.mainView.TabChanged += new System.EventHandler(this.mainView_TabChanged);
			// 
			// updateActivityTimer
			// 
			this.updateActivityTimer.Enabled = true;
			this.updateActivityTimer.Interval = 1000;
			this.updateActivityTimer.Tick += new System.EventHandler(this.UpdateActivityTimerTick);
			// 
			// miCheckUpdate
			// 
			this.miCheckUpdate.Name = "miCheckUpdate";
			this.miCheckUpdate.Size = new System.Drawing.Size(256, 22);
			this.miCheckUpdate.Text = "Check For Update";
			// 
			// MainForm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(744, 662);
			this.Controls.Add(this.viewContainer);
			this.Controls.Add(this.mainViewContainer);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.mainMenuStrip);
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.Text = "ComicRack";
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.contextRating.ResumeLayout(false);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.pageContextMenu.ResumeLayout(false);
			this.contextRating2.ResumeLayout(false);
			this.notfifyContextMenu.ResumeLayout(false);
			this.viewContainer.ResumeLayout(false);
			this.panelReader.ResumeLayout(false);
			this.readerContainer.ResumeLayout(false);
			this.readerContainer.PerformLayout();
			this.fileTabs.ResumeLayout(false);
			this.fileTabs.PerformLayout();
			this.mainToolStrip.ResumeLayout(false);
			this.mainToolStrip.PerformLayout();
			this.toolsContextMenu.ResumeLayout(false);
			this.tabContextMenu.ResumeLayout(false);
			this.mainViewContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private ComicDisplay comicDisplay;
		private readonly NavigatorManager books;
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
		private ToolStripMenuItem miCheckUpdate;
	}
}
