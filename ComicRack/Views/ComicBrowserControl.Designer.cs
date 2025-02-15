using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Plugins;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicBrowserControl : SubView, IComicBrowser, IGetBookList, IRefreshDisplay, ISearchOptions, IDisplayWorkspace, IItemSize, ISettingsChanged
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				UnregisterBookList();
				bookList = null;
				if (base.Main != null)
				{
					base.Main.OpenBooks.BookClosed -= OpenBooksChanged;
					base.Main.OpenBooks.BookOpened -= OpenBooksChanged;
				}
				groupUp.Dispose();
				groupDown.Dispose();
				sortUp.Dispose();
				sortDown.Dispose();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComicBrowserControl));
			this.contextRating = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miRating0 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.miRating1 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRating2 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRating3 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRating4 = new System.Windows.Forms.ToolStripMenuItem();
			this.miRating5 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.miQuickRating = new System.Windows.Forms.ToolStripMenuItem();
			this.miRateMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMarkAs = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miMarkUnread = new System.Windows.Forms.ToolStripMenuItem();
			this.miMarkRead = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.miMarkChecked = new System.Windows.Forms.ToolStripMenuItem();
			this.miMarkUnchecked = new System.Windows.Forms.ToolStripMenuItem();
			this.miMarkAs = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuItems = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miRead = new System.Windows.Forms.ToolStripMenuItem();
			this.miReadTab = new System.Windows.Forms.ToolStripMenuItem();
			this.miOpenWith = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.contextOpenWith = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miOpenWithManager = new System.Windows.Forms.ToolStripMenuItem();
			this.miProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowWeb = new System.Windows.Forms.ToolStripMenuItem();
			this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.miEditList = new System.Windows.Forms.ToolStripMenuItem();
			this.miEditListMoveToTop = new System.Windows.Forms.ToolStripMenuItem();
			this.miEditListMoveToBottom = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.miEditListApplyOrder = new System.Windows.Forms.ToolStripMenuItem();
			this.miAddList = new System.Windows.Forms.ToolStripMenuItem();
			this.dummyEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tsMarkAsSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.miAddLibrary = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowInList = new System.Windows.Forms.ToolStripMenuItem();
			this.dummyEntryToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.miExportComics = new System.Windows.Forms.ToolStripMenuItem();
			this.contextExport = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miExportComicsAs = new System.Windows.Forms.ToolStripMenuItem();
			this.miExportComicsWithPrevious = new System.Windows.Forms.ToolStripMenuItem();
			this.miAutomation = new System.Windows.Forms.ToolStripMenuItem();
			this.miUpdateComicFiles = new System.Windows.Forms.ToolStripMenuItem();
			this.miRevealBrowser = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.miCopyData = new System.Windows.Forms.ToolStripMenuItem();
			this.miPasteData = new System.Windows.Forms.ToolStripMenuItem();
			this.miClearData = new System.Windows.Forms.ToolStripMenuItem();
			this.tsCopySeparator = new System.Windows.Forms.ToolStripSeparator();
			this.miSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.miInvertSelection = new System.Windows.Forms.ToolStripMenuItem();
			this.miRefreshInformation = new System.Windows.Forms.ToolStripMenuItem();
			this.sepListBackground = new System.Windows.Forms.ToolStripSeparator();
			this.miSetTopOfStack = new System.Windows.Forms.ToolStripMenuItem();
			this.miSetStackThumbnail = new System.Windows.Forms.ToolStripMenuItem();
			this.miRemoveStackThumbnail = new System.Windows.Forms.ToolStripMenuItem();
			this.miSetListBackground = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripRemoveSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.miRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.quickSearchTimer = new System.Windows.Forms.Timer(this.components);
			this.contextQuickSearch = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miSearchAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.miSearchSeries = new System.Windows.Forms.ToolStripMenuItem();
			this.miSearchWriter = new System.Windows.Forms.ToolStripMenuItem();
			this.miSearchArtists = new System.Windows.Forms.ToolStripMenuItem();
			this.miSearchDescriptive = new System.Windows.Forms.ToolStripMenuItem();
			this.miSearchCatalog = new System.Windows.Forms.ToolStripMenuItem();
			this.miSearchFile = new System.Windows.Forms.ToolStripMenuItem();
			this.itemView = new cYo.Common.Windows.Forms.ItemView();
			this.displayOptionPanel = new System.Windows.Forms.Panel();
			this.lblDisplayOptionText = new System.Windows.Forms.Label();
			this.btDisplayAll = new System.Windows.Forms.Button();
			this.searchBrowserContainer = new cYo.Common.Windows.Forms.SizableContainer();
			this.bookSelectorPanel = new cYo.Projects.ComicRack.Engine.Controls.SearchBrowserControl();
			this.openStackPanel = new System.Windows.Forms.Panel();
			this.btPrevStack = new System.Windows.Forms.Button();
			this.btNextStack = new System.Windows.Forms.Button();
			this.lblOpenStackText = new System.Windows.Forms.Label();
			this.btCloseStack = new System.Windows.Forms.Button();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.tbSidebar = new System.Windows.Forms.ToolStripButton();
			this.btBrowsePrev = new System.Windows.Forms.ToolStripButton();
			this.btBrowseNext = new System.Windows.Forms.ToolStripButton();
			this.tbBrowseSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.tbbView = new System.Windows.Forms.ToolStripSplitButton();
			this.miViewThumbnails = new System.Windows.Forms.ToolStripMenuItem();
			this.miViewTiles = new System.Windows.Forms.ToolStripMenuItem();
			this.miViewDetails = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.miExpandAllGroups = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowGroupHeaders = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.miShowOnlyAllComics = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowOnlyUnreadComics = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowOnlyReadingComics = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowOnlyReadComics = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.miShowOnlyComics = new System.Windows.Forms.ToolStripMenuItem();
			this.miShowOnlyFileless = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.miShowOnlyDuplicates = new System.Windows.Forms.ToolStripMenuItem();
			this.tbbGroup = new System.Windows.Forms.ToolStripSplitButton();
			this.dummyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tbbStack = new System.Windows.Forms.ToolStripSplitButton();
			this.dummyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.tbbSort = new System.Windows.Forms.ToolStripSplitButton();
			this.dummyToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.tsQuickSearch = new cYo.Common.Windows.Forms.ToolStripSearchTextBox();
			this.tsListLayouts = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsEditListLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.tsSaveListLayout = new System.Windows.Forms.ToolStripMenuItem();
			this.miResetListBackground = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.tsEditLayouts = new System.Windows.Forms.ToolStripMenuItem();
			this.separatorListLayout = new System.Windows.Forms.ToolStripSeparator();
			this.sepDuplicateList = new System.Windows.Forms.ToolStripSeparator();
			this.tbbDuplicateList = new System.Windows.Forms.ToolStripDropDownButton();
			this.dummyEntryToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.sepUndo = new System.Windows.Forms.ToolStripSeparator();
			this.tbUndo = new System.Windows.Forms.ToolStripButton();
			this.tbRedo = new System.Windows.Forms.ToolStripButton();
			this.lvGroupHeaders = new cYo.Common.Windows.Forms.ListViewEx();
			this.lvGroupsName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lvGroupsCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.browserContainer = new System.Windows.Forms.SplitContainer();
			this.contextRating.SuspendLayout();
			this.contextMarkAs.SuspendLayout();
			this.contextMenuItems.SuspendLayout();
			this.contextOpenWith.SuspendLayout();
			this.contextExport.SuspendLayout();
			this.contextQuickSearch.SuspendLayout();
			this.displayOptionPanel.SuspendLayout();
			this.searchBrowserContainer.SuspendLayout();
			this.openStackPanel.SuspendLayout();
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.browserContainer)).BeginInit();
			this.browserContainer.Panel1.SuspendLayout();
			this.browserContainer.Panel2.SuspendLayout();
			this.browserContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextRating
			// 
			this.contextRating.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRating0,
            this.toolStripMenuItem3,
            this.miRating1,
            this.miRating2,
            this.miRating3,
            this.miRating4,
            this.miRating5,
            this.toolStripSeparator1,
            this.miQuickRating});
			this.contextRating.Name = "contextRating";
			this.contextRating.OwnerItem = this.miRateMenu;
			this.contextRating.Size = new System.Drawing.Size(286, 170);
			// 
			// miRating0
			// 
			this.miRating0.Name = "miRating0";
			this.miRating0.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D0)));
			this.miRating0.Size = new System.Drawing.Size(285, 22);
			this.miRating0.Tag = "0";
			this.miRating0.Text = "None";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(282, 6);
			// 
			// miRating1
			// 
			this.miRating1.Name = "miRating1";
			this.miRating1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D1)));
			this.miRating1.Size = new System.Drawing.Size(285, 22);
			this.miRating1.Tag = "1";
			this.miRating1.Text = "* (1 Star)";
			// 
			// miRating2
			// 
			this.miRating2.Name = "miRating2";
			this.miRating2.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D2)));
			this.miRating2.Size = new System.Drawing.Size(285, 22);
			this.miRating2.Tag = "2";
			this.miRating2.Text = "** (2 Stars)";
			// 
			// miRating3
			// 
			this.miRating3.Name = "miRating3";
			this.miRating3.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D3)));
			this.miRating3.Size = new System.Drawing.Size(285, 22);
			this.miRating3.Tag = "3";
			this.miRating3.Text = "*** (3 Stars)";
			// 
			// miRating4
			// 
			this.miRating4.Name = "miRating4";
			this.miRating4.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D4)));
			this.miRating4.Size = new System.Drawing.Size(285, 22);
			this.miRating4.Tag = "4";
			this.miRating4.Text = "**** (4 Stars)";
			// 
			// miRating5
			// 
			this.miRating5.Name = "miRating5";
			this.miRating5.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.D5)));
			this.miRating5.Size = new System.Drawing.Size(285, 22);
			this.miRating5.Tag = "5";
			this.miRating5.Text = "***** (5 Stars)";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(282, 6);
			// 
			// miQuickRating
			// 
			this.miQuickRating.Name = "miQuickRating";
			this.miQuickRating.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Q)));
			this.miQuickRating.Size = new System.Drawing.Size(285, 22);
			this.miQuickRating.Text = "Quick Rating and Review...";
			// 
			// miRateMenu
			// 
			this.miRateMenu.DropDown = this.contextRating;
			this.miRateMenu.Name = "miRateMenu";
			this.miRateMenu.Size = new System.Drawing.Size(251, 22);
			this.miRateMenu.Text = "My &Rating";
			// 
			// contextMarkAs
			// 
			this.contextMarkAs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMarkUnread,
            this.miMarkRead,
            this.toolStripMenuItem9,
            this.miMarkChecked,
            this.miMarkUnchecked});
			this.contextMarkAs.Name = "contextMarkAs";
			this.contextMarkAs.OwnerItem = this.miMarkAs;
			this.contextMarkAs.Size = new System.Drawing.Size(203, 98);
			// 
			// miMarkUnread
			// 
			this.miMarkUnread.Name = "miMarkUnread";
			this.miMarkUnread.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
			this.miMarkUnread.Size = new System.Drawing.Size(202, 22);
			this.miMarkUnread.Text = "&Unread";
			// 
			// miMarkRead
			// 
			this.miMarkRead.Name = "miMarkRead";
			this.miMarkRead.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.R)));
			this.miMarkRead.Size = new System.Drawing.Size(202, 22);
			this.miMarkRead.Text = "&Read";
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(199, 6);
			// 
			// miMarkChecked
			// 
			this.miMarkChecked.Name = "miMarkChecked";
			this.miMarkChecked.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.M)));
			this.miMarkChecked.Size = new System.Drawing.Size(202, 22);
			this.miMarkChecked.Text = "Checked";
			// 
			// miMarkUnchecked
			// 
			this.miMarkUnchecked.Name = "miMarkUnchecked";
			this.miMarkUnchecked.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
			this.miMarkUnchecked.Size = new System.Drawing.Size(202, 22);
			this.miMarkUnchecked.Text = "Unchecked";
			// 
			// miMarkAs
			// 
			this.miMarkAs.DropDown = this.contextMarkAs;
			this.miMarkAs.Name = "miMarkAs";
			this.miMarkAs.Size = new System.Drawing.Size(251, 22);
			this.miMarkAs.Text = "&Mark as";
			// 
			// toolTip
			// 
			this.toolTip.AutomaticDelay = 1000;
			this.toolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip_Popup);
			// 
			// contextMenuItems
			// 
			this.contextMenuItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRead,
            this.miReadTab,
            this.miOpenWith,
            this.miProperties,
            this.miShowWeb,
            this.miEdit,
            this.toolStripMenuItem5,
            this.miRateMenu,
            this.miMarkAs,
            this.miEditList,
            this.miAddList,
            this.tsMarkAsSeparator,
            this.miAddLibrary,
            this.miShowOnly,
            this.miShowInList,
            this.miExportComics,
            this.miAutomation,
            this.miUpdateComicFiles,
            this.miRevealBrowser,
            this.toolStripMenuItem7,
            this.miCopyData,
            this.miPasteData,
            this.miClearData,
            this.tsCopySeparator,
            this.miSelectAll,
            this.miInvertSelection,
            this.miRefreshInformation,
            this.sepListBackground,
            this.miSetTopOfStack,
            this.miSetStackThumbnail,
            this.miRemoveStackThumbnail,
            this.miSetListBackground,
            this.toolStripRemoveSeparator,
            this.miRemove});
			this.contextMenuItems.Name = "contextMenuFiles";
			this.contextMenuItems.Size = new System.Drawing.Size(252, 656);
			this.contextMenuItems.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.contextMenuItems_Closed);
			this.contextMenuItems.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuItems_Opening);
			this.contextMenuItems.Opened += new System.EventHandler(this.contextMenuItems_Opened);
			// 
			// miRead
			// 
			this.miRead.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			this.miRead.Name = "miRead";
			this.miRead.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.miRead.Size = new System.Drawing.Size(251, 22);
			this.miRead.Text = "&Open";
			// 
			// miReadTab
			// 
			this.miReadTab.Name = "miReadTab";
			this.miReadTab.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
			this.miReadTab.Size = new System.Drawing.Size(251, 22);
			this.miReadTab.Text = "Open in new Tab";
			// 
			// miOpenWith
			// 
			this.miOpenWith.DropDown = this.contextOpenWith;
			this.miOpenWith.Name = "miOpenWith";
			this.miOpenWith.Size = new System.Drawing.Size(251, 22);
			this.miOpenWith.Text = "Open With";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
			this.toolStripSeparator2.Visible = false;
			// 
			// contextOpenWith
			// 
			this.contextOpenWith.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenWithManager,
            this.toolStripSeparator2});
			this.contextOpenWith.Name = "contextOpenWith";
			this.contextOpenWith.OwnerItem = this.miOpenWith;
			this.contextOpenWith.Size = new System.Drawing.Size(181, 54);
			this.contextOpenWith.Opening += new System.ComponentModel.CancelEventHandler(this.contextOpenWith_Opening);
			// 
			// miOpenWithManager
			// 
			this.miOpenWithManager.Name = "miOpenWithManager";
			this.miOpenWithManager.Size = new System.Drawing.Size(180, 22);
			this.miOpenWithManager.Text = "Manage Programs...";
			this.miOpenWithManager.Click += new System.EventHandler(this.miOpenWithManager_Click);
			// 
			// miProperties
			// 
			this.miProperties.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GetInfo;
			this.miProperties.Name = "miProperties";
			this.miProperties.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
			this.miProperties.Size = new System.Drawing.Size(251, 22);
			this.miProperties.Text = "Info...";
			// 
			// miShowWeb
			// 
			this.miShowWeb.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.WebBlog;
			this.miShowWeb.Name = "miShowWeb";
			this.miShowWeb.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
			this.miShowWeb.Size = new System.Drawing.Size(251, 22);
			this.miShowWeb.Text = "Web...";
			// 
			// miEdit
			// 
			this.miEdit.Name = "miEdit";
			this.miEdit.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.miEdit.Size = new System.Drawing.Size(251, 22);
			this.miEdit.Text = "Edit";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(248, 6);
			// 
			// miEditList
			// 
			this.miEditList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditListMoveToTop,
            this.miEditListMoveToBottom,
            this.toolStripMenuItem8,
            this.miEditListApplyOrder});
			this.miEditList.Name = "miEditList";
			this.miEditList.Size = new System.Drawing.Size(251, 22);
			this.miEditList.Text = "Edit List";
			// 
			// miEditListMoveToTop
			// 
			this.miEditListMoveToTop.Name = "miEditListMoveToTop";
			this.miEditListMoveToTop.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.T)));
			this.miEditListMoveToTop.Size = new System.Drawing.Size(225, 22);
			this.miEditListMoveToTop.Text = "Move to Top";
			// 
			// miEditListMoveToBottom
			// 
			this.miEditListMoveToBottom.Name = "miEditListMoveToBottom";
			this.miEditListMoveToBottom.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.B)));
			this.miEditListMoveToBottom.Size = new System.Drawing.Size(225, 22);
			this.miEditListMoveToBottom.Text = "Move to Bottom";
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(222, 6);
			// 
			// miEditListApplyOrder
			// 
			this.miEditListApplyOrder.Name = "miEditListApplyOrder";
			this.miEditListApplyOrder.Size = new System.Drawing.Size(225, 22);
			this.miEditListApplyOrder.Text = "Apply current Order";
			// 
			// miAddList
			// 
			this.miAddList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyEntryToolStripMenuItem});
			this.miAddList.Name = "miAddList";
			this.miAddList.Size = new System.Drawing.Size(251, 22);
			this.miAddList.Text = "Add to List";
			this.miAddList.DropDownOpening += new System.EventHandler(this.miAddList_DropDownOpening);
			// 
			// dummyEntryToolStripMenuItem
			// 
			this.dummyEntryToolStripMenuItem.Name = "dummyEntryToolStripMenuItem";
			this.dummyEntryToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
			this.dummyEntryToolStripMenuItem.Text = "dummyEntry";
			// 
			// tsMarkAsSeparator
			// 
			this.tsMarkAsSeparator.Name = "tsMarkAsSeparator";
			this.tsMarkAsSeparator.Size = new System.Drawing.Size(248, 6);
			// 
			// miAddLibrary
			// 
			this.miAddLibrary.Name = "miAddLibrary";
			this.miAddLibrary.Size = new System.Drawing.Size(251, 22);
			this.miAddLibrary.Text = "&Add to Library";
			// 
			// miShowOnly
			// 
			this.miShowOnly.Name = "miShowOnly";
			this.miShowOnly.Size = new System.Drawing.Size(251, 22);
			this.miShowOnly.Text = "&Browse Books";
			// 
			// miShowInList
			// 
			this.miShowInList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyEntryToolStripMenuItem1});
			this.miShowInList.Name = "miShowInList";
			this.miShowInList.Size = new System.Drawing.Size(251, 22);
			this.miShowInList.Text = "Show in List";
			this.miShowInList.DropDownClosed += new System.EventHandler(this.miShowInList_DropDownClosed);
			this.miShowInList.DropDownOpening += new System.EventHandler(this.miShowInList_DropDownOpening);
			// 
			// dummyEntryToolStripMenuItem1
			// 
			this.dummyEntryToolStripMenuItem1.Name = "dummyEntryToolStripMenuItem1";
			this.dummyEntryToolStripMenuItem1.Size = new System.Drawing.Size(143, 22);
			this.dummyEntryToolStripMenuItem1.Text = "dummyEntry";
			// 
			// miExportComics
			// 
			this.miExportComics.DropDown = this.contextExport;
			this.miExportComics.Name = "miExportComics";
			this.miExportComics.Size = new System.Drawing.Size(251, 22);
			this.miExportComics.Text = "Export Books";
			// 
			// contextExport
			// 
			this.contextExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miExportComicsAs,
            this.miExportComicsWithPrevious});
			this.contextExport.Name = "contextExport";
            this.contextExport.OwnerItem = this.miExportComics;
			this.contextExport.Size = new System.Drawing.Size(246, 48);
			this.contextExport.Opening += new System.ComponentModel.CancelEventHandler(this.contextExport_Opening);
			// 
			// miExportComicsAs
			// 
			this.miExportComicsAs.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Save;
			this.miExportComicsAs.Name = "miExportComicsAs";
			this.miExportComicsAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.E)));
			this.miExportComicsAs.Size = new System.Drawing.Size(245, 22);
			this.miExportComicsAs.Text = "Export Books...";
			// 
			// miExportComicsWithPrevious
			// 
			this.miExportComicsWithPrevious.Name = "miExportComicsWithPrevious";
			this.miExportComicsWithPrevious.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.E)));
			this.miExportComicsWithPrevious.Size = new System.Drawing.Size(245, 22);
			this.miExportComicsWithPrevious.Text = "Export with Previous";
			// 
			// miAutomation
			// 
			this.miAutomation.Name = "miAutomation";
			this.miAutomation.Size = new System.Drawing.Size(251, 22);
			this.miAutomation.Text = "Automation";
			// 
			// miUpdateComicFiles
			// 
			this.miUpdateComicFiles.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.UpdateSmall;
			this.miUpdateComicFiles.Name = "miUpdateComicFiles";
			this.miUpdateComicFiles.Size = new System.Drawing.Size(251, 22);
			this.miUpdateComicFiles.Text = "Update Book Files";
			// 
			// miRevealBrowser
			// 
			this.miRevealBrowser.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RevealExplorer;
			this.miRevealBrowser.Name = "miRevealBrowser";
			this.miRevealBrowser.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
			this.miRevealBrowser.Size = new System.Drawing.Size(251, 22);
			this.miRevealBrowser.Text = "Reveal Book in &Explorer";
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(248, 6);
			// 
			// miCopyData
			// 
			this.miCopyData.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditCopy;
			this.miCopyData.Name = "miCopyData";
			this.miCopyData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.miCopyData.Size = new System.Drawing.Size(251, 22);
			this.miCopyData.Text = "&Copy Data";
			// 
			// miPasteData
			// 
			this.miPasteData.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditPaste;
			this.miPasteData.Name = "miPasteData";
			this.miPasteData.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.miPasteData.Size = new System.Drawing.Size(251, 22);
			this.miPasteData.Text = "&Paste Data...";
			// 
			// miClearData
			// 
			this.miClearData.Name = "miClearData";
			this.miClearData.Size = new System.Drawing.Size(251, 22);
			this.miClearData.Text = "Clear Data";
			// 
			// tsCopySeparator
			// 
			this.tsCopySeparator.Name = "tsCopySeparator";
			this.tsCopySeparator.Size = new System.Drawing.Size(248, 6);
			// 
			// miSelectAll
			// 
			this.miSelectAll.Name = "miSelectAll";
			this.miSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.miSelectAll.Size = new System.Drawing.Size(251, 22);
			this.miSelectAll.Text = "Select &All";
			// 
			// miInvertSelection
			// 
			this.miInvertSelection.Name = "miInvertSelection";
			this.miInvertSelection.Size = new System.Drawing.Size(251, 22);
			this.miInvertSelection.Text = "&Invert Selection";
			// 
			// miRefreshInformation
			// 
			this.miRefreshInformation.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.RefreshThumbnail;
			this.miRefreshInformation.Name = "miRefreshInformation";
			this.miRefreshInformation.Size = new System.Drawing.Size(251, 22);
			this.miRefreshInformation.Text = "Refresh";
			// 
			// sepListBackground
			// 
			this.sepListBackground.Name = "sepListBackground";
			this.sepListBackground.Size = new System.Drawing.Size(248, 6);
			// 
			// miSetTopOfStack
			// 
			this.miSetTopOfStack.Name = "miSetTopOfStack";
			this.miSetTopOfStack.Size = new System.Drawing.Size(251, 22);
			this.miSetTopOfStack.Text = "Set as Top of Stack";
			// 
			// miSetStackThumbnail
			// 
			this.miSetStackThumbnail.Name = "miSetStackThumbnail";
			this.miSetStackThumbnail.Size = new System.Drawing.Size(251, 22);
			this.miSetStackThumbnail.Text = "Set custom Stack Thumbnail...";
			// 
			// miRemoveStackThumbnail
			// 
			this.miRemoveStackThumbnail.Name = "miRemoveStackThumbnail";
			this.miRemoveStackThumbnail.Size = new System.Drawing.Size(251, 22);
			this.miRemoveStackThumbnail.Text = "Remove custom Stack Thumbnail";
			// 
			// miSetListBackground
			// 
			this.miSetListBackground.Name = "miSetListBackground";
			this.miSetListBackground.Size = new System.Drawing.Size(251, 22);
			this.miSetListBackground.Text = "Set as List Background";
			// 
			// toolStripRemoveSeparator
			// 
			this.toolStripRemoveSeparator.Name = "toolStripRemoveSeparator";
			this.toolStripRemoveSeparator.Size = new System.Drawing.Size(248, 6);
			// 
			// miRemove
			// 
			this.miRemove.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
			this.miRemove.Name = "miRemove";
			this.miRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.miRemove.Size = new System.Drawing.Size(251, 22);
			this.miRemove.Text = "Re&move...";
			// 
			// quickSearchTimer
			// 
			this.quickSearchTimer.Interval = 500;
			this.quickSearchTimer.Tick += new System.EventHandler(this.quickSearchTimer_Tick);
			// 
			// contextQuickSearch
			// 
			this.contextQuickSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSearchAll,
            this.toolStripSeparator5,
            this.miSearchSeries,
            this.miSearchWriter,
            this.miSearchArtists,
            this.miSearchDescriptive,
            this.miSearchCatalog,
            this.miSearchFile});
			this.contextQuickSearch.Name = "contextQuickSearch";
			this.contextQuickSearch.Size = new System.Drawing.Size(133, 164);
			// 
			// miSearchAll
			// 
			this.miSearchAll.Checked = true;
			this.miSearchAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.miSearchAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.miSearchAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.miSearchAll.Name = "miSearchAll";
			this.miSearchAll.Size = new System.Drawing.Size(132, 22);
			this.miSearchAll.Text = "All";
			this.miSearchAll.ToolTipText = "Quick Search is checking all available data";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(129, 6);
			// 
			// miSearchSeries
			// 
			this.miSearchSeries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.miSearchSeries.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.miSearchSeries.Name = "miSearchSeries";
			this.miSearchSeries.Size = new System.Drawing.Size(132, 22);
			this.miSearchSeries.Text = "Series";
			this.miSearchSeries.ToolTipText = "Quick Search is only checking the Series name";
			// 
			// miSearchWriter
			// 
			this.miSearchWriter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.miSearchWriter.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.miSearchWriter.Name = "miSearchWriter";
			this.miSearchWriter.Size = new System.Drawing.Size(132, 22);
			this.miSearchWriter.Text = "Writer";
			this.miSearchWriter.ToolTipText = "Quick Search is only checking the Writer";
			// 
			// miSearchArtists
			// 
			this.miSearchArtists.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.miSearchArtists.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.miSearchArtists.Name = "miSearchArtists";
			this.miSearchArtists.Size = new System.Drawing.Size(132, 22);
			this.miSearchArtists.Text = "Artists";
			this.miSearchArtists.ToolTipText = "Quick Search is checking all Artists";
			// 
			// miSearchDescriptive
			// 
			this.miSearchDescriptive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.miSearchDescriptive.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.miSearchDescriptive.Name = "miSearchDescriptive";
			this.miSearchDescriptive.Size = new System.Drawing.Size(132, 22);
			this.miSearchDescriptive.Text = "Descriptive";
			this.miSearchDescriptive.ToolTipText = "Quick Search is checking all notes and summaries";
			// 
			// miSearchCatalog
			// 
			this.miSearchCatalog.Name = "miSearchCatalog";
			this.miSearchCatalog.Size = new System.Drawing.Size(132, 22);
			this.miSearchCatalog.Text = "Catalog";
			this.miSearchCatalog.ToolTipText = "Quick Search is only checking the Catalog entries";
			// 
			// miSearchFile
			// 
			this.miSearchFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.miSearchFile.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.miSearchFile.Name = "miSearchFile";
			this.miSearchFile.Size = new System.Drawing.Size(132, 22);
			this.miSearchFile.Text = "Filename";
			this.miSearchFile.ToolTipText = "Quick Search is only checking the Filename";
			// 
			// itemView
			// 
			this.itemView.AllowDrop = true;
			this.itemView.BackColor = System.Drawing.SystemColors.Window;
			this.itemView.ColumnHeaderHeight = 19;
			this.itemView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.itemView.ExpandedDetailColumnName = "Cover";
			this.itemView.GroupCollapsedImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ArrowRight;
			this.itemView.GroupColumns = new cYo.Common.Windows.Forms.IColumn[0];
			this.itemView.GroupColumnsKey = null;
			this.itemView.GroupExpandedImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ArrowDown;
			this.itemView.GroupsStatus = ((cYo.Common.Windows.Forms.ItemViewGroupsStatus)(resources.GetObject("itemView.GroupsStatus")));
			this.itemView.HideSelection = false;
			this.itemView.ItemRowHeight = 19;
			this.itemView.Location = new System.Drawing.Point(0, 0);
			this.itemView.Name = "itemView";
			this.itemView.Size = new System.Drawing.Size(710, 172);
			this.itemView.SortColumn = null;
			this.itemView.SortColumns = new cYo.Common.Windows.Forms.IColumn[0];
			this.itemView.SortColumnsKey = null;
			this.itemView.StackColumns = new cYo.Common.Windows.Forms.IColumn[0];
			this.itemView.StackColumnsKey = null;
			this.itemView.StackDisplayEnabled = true;
			this.itemView.TabIndex = 0;
			this.itemView.ProcessStack += new System.EventHandler<cYo.Common.Windows.Forms.ItemView.StackEventArgs>(this.itemView_ProcessStack);
			this.itemView.ItemActivate += new System.EventHandler(this.itemView_ItemActivate);
			this.itemView.SelectedIndexChanged += new System.EventHandler(this.itemView_SelectedIndexChanged);
			this.itemView.ItemDisplayChanged += new System.EventHandler(this.itemView_ItemDisplayChanged);
			this.itemView.GroupDisplayChanged += new System.EventHandler(this.itemView_GroupDisplayChanged);
			this.itemView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.itemView_ItemDrag);
			this.itemView.PostPaint += new System.Windows.Forms.PaintEventHandler(this.itemView_PostPaint);
			this.itemView.DragDrop += new System.Windows.Forms.DragEventHandler(this.itemView_DragDrop);
			this.itemView.DragEnter += new System.Windows.Forms.DragEventHandler(this.itemView_DragEnter);
			this.itemView.DragOver += new System.Windows.Forms.DragEventHandler(this.itemView_DragOver);
			this.itemView.DragLeave += new System.EventHandler(this.itemView_DragLeave);
			this.itemView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.DragGiveDragCursorFeedback);
			this.itemView.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.DragQueryContinueDrag);
			this.itemView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.itemView_KeyDown);
			this.itemView.MouseLeave += new System.EventHandler(this.foundView_MouseLeave);
			this.itemView.MouseHover += new System.EventHandler(this.foundView_MouseHover);
			this.itemView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.foundView_MouseMove);
			// 
			// displayOptionPanel
			// 
			this.displayOptionPanel.Controls.Add(this.lblDisplayOptionText);
			this.displayOptionPanel.Controls.Add(this.btDisplayAll);
			this.displayOptionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.displayOptionPanel.Location = new System.Drawing.Point(0, 396);
			this.displayOptionPanel.Name = "displayOptionPanel";
			this.displayOptionPanel.Size = new System.Drawing.Size(710, 39);
			this.displayOptionPanel.TabIndex = 7;
			this.displayOptionPanel.Visible = false;
			// 
			// lblDisplayOptionText
			// 
			this.lblDisplayOptionText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblDisplayOptionText.Location = new System.Drawing.Point(5, 11);
			this.lblDisplayOptionText.Name = "lblDisplayOptionText";
			this.lblDisplayOptionText.Size = new System.Drawing.Size(598, 18);
			this.lblDisplayOptionText.TabIndex = 1;
			this.lblDisplayOptionText.Text = "Because of active Views options, not all Books are displayed.";
			// 
			// btDisplayAll
			// 
			this.btDisplayAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btDisplayAll.Location = new System.Drawing.Point(609, 6);
			this.btDisplayAll.Name = "btDisplayAll";
			this.btDisplayAll.Size = new System.Drawing.Size(90, 23);
			this.btDisplayAll.TabIndex = 0;
			this.btDisplayAll.Text = "Display &All";
			this.btDisplayAll.UseVisualStyleBackColor = true;
			this.btDisplayAll.Click += new System.EventHandler(this.btDisplayAll_Click);
			// 
			// searchBrowserContainer
			// 
			this.searchBrowserContainer.Controls.Add(this.bookSelectorPanel);
			this.searchBrowserContainer.Dock = System.Windows.Forms.DockStyle.Top;
			this.searchBrowserContainer.Grip = cYo.Common.Windows.Forms.SizableContainer.GripPosition.Bottom;
			this.searchBrowserContainer.Location = new System.Drawing.Point(0, 64);
			this.searchBrowserContainer.Name = "searchBrowserContainer";
			this.searchBrowserContainer.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
			this.searchBrowserContainer.Size = new System.Drawing.Size(710, 160);
			this.searchBrowserContainer.TabIndex = 6;
			this.searchBrowserContainer.Text = "sizableContainer1";
			this.searchBrowserContainer.ExpandedChanged += new System.EventHandler(this.searchBrowserContainer_ExpandedChanged);
			// 
			// bookSelectorPanel
			// 
			this.bookSelectorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bookSelectorPanel.Location = new System.Drawing.Point(0, 6);
			this.bookSelectorPanel.Name = "bookSelectorPanel";
			this.bookSelectorPanel.Size = new System.Drawing.Size(710, 148);
			this.bookSelectorPanel.TabIndex = 0;
			this.bookSelectorPanel.CurrentMatcherChanged += new System.EventHandler(this.bookSelectorPanel_FilterChanged);
			this.bookSelectorPanel.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.bookSelectorPanel_ItemDrag);
			this.bookSelectorPanel.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.DragGiveDragCursorFeedback);
			this.bookSelectorPanel.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.DragQueryContinueDrag);
			// 
			// openStackPanel
			// 
			this.openStackPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.openStackPanel.Controls.Add(this.btPrevStack);
			this.openStackPanel.Controls.Add(this.btNextStack);
			this.openStackPanel.Controls.Add(this.lblOpenStackText);
			this.openStackPanel.Controls.Add(this.btCloseStack);
			this.openStackPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.openStackPanel.Location = new System.Drawing.Point(0, 25);
			this.openStackPanel.Name = "openStackPanel";
			this.openStackPanel.Size = new System.Drawing.Size(710, 39);
			this.openStackPanel.TabIndex = 8;
			this.openStackPanel.Visible = false;
			// 
			// btPrevStack
			// 
			this.btPrevStack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btPrevStack.Location = new System.Drawing.Point(541, 6);
			this.btPrevStack.Name = "btPrevStack";
			this.btPrevStack.Size = new System.Drawing.Size(75, 23);
			this.btPrevStack.TabIndex = 2;
			this.btPrevStack.Text = "&Previous";
			this.btPrevStack.UseVisualStyleBackColor = true;
			this.btPrevStack.Click += new System.EventHandler(this.btPrevStack_Click);
			// 
			// btNextStack
			// 
			this.btNextStack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btNextStack.Location = new System.Drawing.Point(622, 6);
			this.btNextStack.Name = "btNextStack";
			this.btNextStack.Size = new System.Drawing.Size(75, 23);
			this.btNextStack.TabIndex = 3;
			this.btNextStack.Text = "&Next";
			this.btNextStack.UseVisualStyleBackColor = true;
			this.btNextStack.Click += new System.EventHandler(this.btNextStack_Click);
			// 
			// lblOpenStackText
			// 
			this.lblOpenStackText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblOpenStackText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblOpenStackText.Location = new System.Drawing.Point(119, 8);
			this.lblOpenStackText.Name = "lblOpenStackText";
			this.lblOpenStackText.Size = new System.Drawing.Size(416, 18);
			this.lblOpenStackText.TabIndex = 1;
			this.lblOpenStackText.Text = "Lorem Ipsum";
			this.lblOpenStackText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btCloseStack
			// 
			this.btCloseStack.Location = new System.Drawing.Point(6, 6);
			this.btCloseStack.Name = "btCloseStack";
			this.btCloseStack.Size = new System.Drawing.Size(107, 23);
			this.btCloseStack.TabIndex = 0;
			this.btCloseStack.Text = "&Close Stack";
			this.btCloseStack.UseVisualStyleBackColor = true;
			this.btCloseStack.Click += new System.EventHandler(this.btCloseStack_Click);
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSidebar,
            this.btBrowsePrev,
            this.btBrowseNext,
            this.tbBrowseSeparator,
            this.tbbView,
            this.tbbGroup,
            this.tbbStack,
            this.tbbSort,
            this.tsQuickSearch,
            this.tsListLayouts,
            this.sepDuplicateList,
            this.tbbDuplicateList,
            this.sepUndo,
            this.tbUndo,
            this.tbRedo});
			this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(710, 25);
			this.toolStrip.TabIndex = 1;
			// 
			// tbSidebar
			// 
			this.tbSidebar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbSidebar.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Sidebar;
			this.tbSidebar.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbSidebar.Name = "tbSidebar";
			this.tbSidebar.Size = new System.Drawing.Size(23, 22);
			this.tbSidebar.Text = "Sidebar";
			// 
			// btBrowsePrev
			// 
			this.btBrowsePrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btBrowsePrev.Enabled = false;
			this.btBrowsePrev.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.BrowsePrevious;
			this.btBrowsePrev.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btBrowsePrev.Name = "btBrowsePrev";
			this.btBrowsePrev.Size = new System.Drawing.Size(23, 22);
			this.btBrowsePrev.Text = "Previous";
			// 
			// btBrowseNext
			// 
			this.btBrowseNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btBrowseNext.Enabled = false;
			this.btBrowseNext.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.BrowseNext;
			this.btBrowseNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btBrowseNext.Name = "btBrowseNext";
			this.btBrowseNext.Size = new System.Drawing.Size(23, 22);
			this.btBrowseNext.Text = "Next";
			// 
			// tbBrowseSeparator
			// 
			this.tbBrowseSeparator.Name = "tbBrowseSeparator";
			this.tbBrowseSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// tbbView
			// 
			this.tbbView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miViewThumbnails,
            this.miViewTiles,
            this.miViewDetails,
            this.toolStripMenuItem6,
            this.miExpandAllGroups,
            this.miShowGroupHeaders,
            this.toolStripMenuItem2,
            this.miShowOnlyAllComics,
            this.miShowOnlyUnreadComics,
            this.miShowOnlyReadingComics,
            this.miShowOnlyReadComics,
            this.toolStripMenuItem4,
            this.miShowOnlyComics,
            this.miShowOnlyFileless,
            this.toolStripMenuItem1,
            this.miShowOnlyDuplicates});
			this.tbbView.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.View;
			this.tbbView.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbView.Name = "tbbView";
			this.tbbView.Size = new System.Drawing.Size(69, 22);
			this.tbbView.Text = "Views";
			this.tbbView.ToolTipText = "Change how and what Books are displayed";
			this.tbbView.ButtonClick += new System.EventHandler(this.tbbView_ButtonClick);
			// 
			// miViewThumbnails
			// 
			this.miViewThumbnails.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ThumbView;
			this.miViewThumbnails.Name = "miViewThumbnails";
			this.miViewThumbnails.Size = new System.Drawing.Size(259, 22);
			this.miViewThumbnails.Text = "T&humbnails";
			// 
			// miViewTiles
			// 
			this.miViewTiles.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TileView;
			this.miViewTiles.Name = "miViewTiles";
			this.miViewTiles.Size = new System.Drawing.Size(259, 22);
			this.miViewTiles.Text = "&Tiles";
			// 
			// miViewDetails
			// 
			this.miViewDetails.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.DetailView;
			this.miViewDetails.Name = "miViewDetails";
			this.miViewDetails.Size = new System.Drawing.Size(259, 22);
			this.miViewDetails.Text = "&Details";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(256, 6);
			// 
			// miExpandAllGroups
			// 
			this.miExpandAllGroups.Name = "miExpandAllGroups";
			this.miExpandAllGroups.Size = new System.Drawing.Size(259, 22);
			this.miExpandAllGroups.Text = "Collapse/Expand all Groups";
			// 
			// miShowGroupHeaders
			// 
			this.miShowGroupHeaders.Name = "miShowGroupHeaders";
			this.miShowGroupHeaders.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.G)));
			this.miShowGroupHeaders.Size = new System.Drawing.Size(259, 22);
			this.miShowGroupHeaders.Text = "Show Group Headers";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(256, 6);
			// 
			// miShowOnlyAllComics
			// 
			this.miShowOnlyAllComics.Checked = true;
			this.miShowOnlyAllComics.CheckState = System.Windows.Forms.CheckState.Checked;
			this.miShowOnlyAllComics.Name = "miShowOnlyAllComics";
			this.miShowOnlyAllComics.Size = new System.Drawing.Size(259, 22);
			this.miShowOnlyAllComics.Text = "Show All";
			// 
			// miShowOnlyUnreadComics
			// 
			this.miShowOnlyUnreadComics.Name = "miShowOnlyUnreadComics";
			this.miShowOnlyUnreadComics.Size = new System.Drawing.Size(259, 22);
			this.miShowOnlyUnreadComics.Text = "Show not Read";
			// 
			// miShowOnlyReadingComics
			// 
			this.miShowOnlyReadingComics.Name = "miShowOnlyReadingComics";
			this.miShowOnlyReadingComics.Size = new System.Drawing.Size(259, 22);
			this.miShowOnlyReadingComics.Text = "Show Reading";
			// 
			// miShowOnlyReadComics
			// 
			this.miShowOnlyReadComics.Name = "miShowOnlyReadComics";
			this.miShowOnlyReadComics.Size = new System.Drawing.Size(259, 22);
			this.miShowOnlyReadComics.Text = "Show Read";
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(256, 6);
			// 
			// miShowOnlyComics
			// 
			this.miShowOnlyComics.Name = "miShowOnlyComics";
			this.miShowOnlyComics.Size = new System.Drawing.Size(259, 22);
			this.miShowOnlyComics.Text = "Show only Books";
			// 
			// miShowOnlyFileless
			// 
			this.miShowOnlyFileless.Name = "miShowOnlyFileless";
			this.miShowOnlyFileless.Size = new System.Drawing.Size(259, 22);
			this.miShowOnlyFileless.Text = "Show only fileless Entries";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(256, 6);
			// 
			// miShowOnlyDuplicates
			// 
			this.miShowOnlyDuplicates.Name = "miShowOnlyDuplicates";
			this.miShowOnlyDuplicates.Size = new System.Drawing.Size(259, 22);
			this.miShowOnlyDuplicates.Text = "Show Duplicates";
			// 
			// tbbGroup
			// 
			this.tbbGroup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyToolStripMenuItem});
			this.tbbGroup.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Group;
			this.tbbGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbGroup.Name = "tbbGroup";
			this.tbbGroup.Size = new System.Drawing.Size(72, 22);
			this.tbbGroup.Text = "Group";
			this.tbbGroup.ToolTipText = "Group Books by different criteria";
			this.tbbGroup.ButtonClick += new System.EventHandler(this.tbbGroup_ButtonClick);
			this.tbbGroup.DropDownOpening += new System.EventHandler(this.tbbGroup_DropDownOpening);
			// 
			// dummyToolStripMenuItem
			// 
			this.dummyToolStripMenuItem.Name = "dummyToolStripMenuItem";
			this.dummyToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
			this.dummyToolStripMenuItem.Text = "Dummy";
			// 
			// tbbStack
			// 
			this.tbbStack.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyToolStripMenuItem1});
			this.tbbStack.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Stacking;
			this.tbbStack.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbStack.Name = "tbbStack";
			this.tbbStack.Size = new System.Drawing.Size(67, 22);
			this.tbbStack.Text = "Stack";
			this.tbbStack.ButtonClick += new System.EventHandler(this.tbbStack_ButtonClick);
			this.tbbStack.DropDownOpening += new System.EventHandler(this.tbbStack_DropDownOpening);
			// 
			// dummyToolStripMenuItem1
			// 
			this.dummyToolStripMenuItem1.Name = "dummyToolStripMenuItem1";
			this.dummyToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
			this.dummyToolStripMenuItem1.Text = "Dummy";
			// 
			// tbbSort
			// 
			this.tbbSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyToolStripMenuItem2});
			this.tbbSort.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SortUp;
			this.tbbSort.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbSort.Name = "tbbSort";
			this.tbbSort.Size = new System.Drawing.Size(81, 22);
			this.tbbSort.Text = "Arrange";
			this.tbbSort.ToolTipText = "Change the sort order of the Books";
			this.tbbSort.ButtonClick += new System.EventHandler(this.tbbSort_ButtonClick);
			this.tbbSort.DropDownOpening += new System.EventHandler(this.tbbSort_DropDownOpening);
			// 
			// dummyToolStripMenuItem2
			// 
			this.dummyToolStripMenuItem2.Name = "dummyToolStripMenuItem2";
			this.dummyToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
			this.dummyToolStripMenuItem2.Text = "Dummy";
			// 
			// tsQuickSearch
			// 
			this.tsQuickSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsQuickSearch.CausesValidation = false;
			this.tsQuickSearch.Margin = new System.Windows.Forms.Padding(0, 1, 1, 0);
			this.tsQuickSearch.Name = "tsQuickSearch";
			this.tsQuickSearch.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			this.tsQuickSearch.Size = new System.Drawing.Size(130, 24);
			this.tsQuickSearch.Enter += new System.EventHandler(this.tsQuickSearch_Enter);
			this.tsQuickSearch.Leave += new System.EventHandler(this.tsQuickSearch_Leave);
			this.tsQuickSearch.TextChanged += new System.EventHandler(this.tsQuickSearch_TextChanged);
			// 
			// tsListLayouts
			// 
			this.tsListLayouts.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsListLayouts.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsEditListLayout,
            this.tsSaveListLayout,
            this.miResetListBackground,
            this.toolStripMenuItem23,
            this.tsEditLayouts,
            this.separatorListLayout});
			this.tsListLayouts.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ListLayout;
			this.tsListLayouts.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsListLayouts.Name = "tsListLayouts";
			this.tsListLayouts.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.tsListLayouts.Size = new System.Drawing.Size(29, 22);
			this.tsListLayouts.Text = "List Layouts";
			this.tsListLayouts.ToolTipText = "Manage List Layouts";
			this.tsListLayouts.DropDownOpening += new System.EventHandler(this.tsListLayouts_DropDownOpening);
			// 
			// tsEditListLayout
			// 
			this.tsEditListLayout.Name = "tsEditListLayout";
			this.tsEditListLayout.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
			this.tsEditListLayout.Size = new System.Drawing.Size(210, 22);
			this.tsEditListLayout.Text = "&Edit List Layout...";
			// 
			// tsSaveListLayout
			// 
			this.tsSaveListLayout.Name = "tsSaveListLayout";
			this.tsSaveListLayout.Size = new System.Drawing.Size(210, 22);
			this.tsSaveListLayout.Text = "&Save List Layout...";
			// 
			// miResetListBackground
			// 
			this.miResetListBackground.Name = "miResetListBackground";
			this.miResetListBackground.Size = new System.Drawing.Size(210, 22);
			this.miResetListBackground.Text = "Reset List Background";
			// 
			// toolStripMenuItem23
			// 
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			this.toolStripMenuItem23.Size = new System.Drawing.Size(207, 6);
			// 
			// tsEditLayouts
			// 
			this.tsEditLayouts.Name = "tsEditLayouts";
			this.tsEditLayouts.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.L)));
			this.tsEditLayouts.Size = new System.Drawing.Size(210, 22);
			this.tsEditLayouts.Text = "&Edit Layouts...";
			// 
			// separatorListLayout
			// 
			this.separatorListLayout.Name = "separatorListLayout";
			this.separatorListLayout.Size = new System.Drawing.Size(207, 6);
			// 
			// sepDuplicateList
			// 
			this.sepDuplicateList.Name = "sepDuplicateList";
			this.sepDuplicateList.Size = new System.Drawing.Size(6, 25);
			// 
			// tbbDuplicateList
			// 
			this.tbbDuplicateList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbbDuplicateList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dummyEntryToolStripMenuItem2});
			this.tbbDuplicateList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AddList;
			this.tbbDuplicateList.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbbDuplicateList.Name = "tbbDuplicateList";
			this.tbbDuplicateList.Size = new System.Drawing.Size(29, 22);
			this.tbbDuplicateList.Text = "Duplicate";
			this.tbbDuplicateList.ToolTipText = "Duplicate current List";
			this.tbbDuplicateList.DropDownOpening += new System.EventHandler(this.tbbDuplicateList_DropDownOpening);
			// 
			// dummyEntryToolStripMenuItem2
			// 
			this.dummyEntryToolStripMenuItem2.Name = "dummyEntryToolStripMenuItem2";
			this.dummyEntryToolStripMenuItem2.Size = new System.Drawing.Size(143, 22);
			this.dummyEntryToolStripMenuItem2.Text = "dummyEntry";
			// 
			// sepUndo
			// 
			this.sepUndo.Name = "sepUndo";
			this.sepUndo.Size = new System.Drawing.Size(6, 25);
			// 
			// tbUndo
			// 
			this.tbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbUndo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Undo;
			this.tbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbUndo.Name = "tbUndo";
			this.tbUndo.Size = new System.Drawing.Size(23, 22);
			this.tbUndo.Text = "Undo";
			// 
			// tbRedo
			// 
			this.tbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbRedo.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Redo;
			this.tbRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbRedo.Name = "tbRedo";
			this.tbRedo.Size = new System.Drawing.Size(23, 22);
			this.tbRedo.Text = "Redo";
			// 
			// lvGroupHeaders
			// 
			this.lvGroupHeaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvGroupsName,
            this.lvGroupsCount});
			this.lvGroupHeaders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvGroupHeaders.FullRowSelect = true;
			this.lvGroupHeaders.HideSelection = false;
			this.lvGroupHeaders.Location = new System.Drawing.Point(0, 0);
			this.lvGroupHeaders.MultiSelect = false;
			this.lvGroupHeaders.Name = "lvGroupHeaders";
			this.lvGroupHeaders.Size = new System.Drawing.Size(96, 100);
			this.lvGroupHeaders.TabIndex = 1;
			this.lvGroupHeaders.UseCompatibleStateImageBehavior = false;
			this.lvGroupHeaders.View = System.Windows.Forms.View.Details;
			this.lvGroupHeaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvGroupHeaders_ColumnClick);
			this.lvGroupHeaders.SelectedIndexChanged += new System.EventHandler(this.lvGroupHeaders_SelectedIndexChanged);
			this.lvGroupHeaders.ClientSizeChanged += new System.EventHandler(this.lvGroupHeaders_ClientSizeChanged);
			// 
			// lvGroupsName
			// 
			this.lvGroupsName.Text = "Group";
			this.lvGroupsName.Width = 162;
			// 
			// lvGroupsCount
			// 
			this.lvGroupsCount.Text = "#";
			this.lvGroupsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// browserContainer
			// 
			this.browserContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.browserContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.browserContainer.Location = new System.Drawing.Point(0, 224);
			this.browserContainer.Name = "browserContainer";
			// 
			// browserContainer.Panel1
			// 
			this.browserContainer.Panel1.Controls.Add(this.itemView);
			// 
			// browserContainer.Panel2
			// 
			this.browserContainer.Panel2.Controls.Add(this.lvGroupHeaders);
			this.browserContainer.Panel2Collapsed = true;
			this.browserContainer.Panel2MinSize = 200;
			this.browserContainer.Size = new System.Drawing.Size(710, 172);
			this.browserContainer.SplitterDistance = 25;
			this.browserContainer.TabIndex = 2;
			this.browserContainer.DoubleClick += new System.EventHandler(this.browserContainer_DoubleClick);
			// 
			// ComicBrowserControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.browserContainer);
			this.Controls.Add(this.displayOptionPanel);
			this.Controls.Add(this.searchBrowserContainer);
			this.Controls.Add(this.openStackPanel);
			this.Controls.Add(this.toolStrip);
			this.Name = "ComicBrowserControl";
			this.Size = new System.Drawing.Size(710, 435);
			this.contextRating.ResumeLayout(false);
			this.contextMarkAs.ResumeLayout(false);
			this.contextMenuItems.ResumeLayout(false);
			this.contextOpenWith.ResumeLayout(false);
			this.contextExport.ResumeLayout(false);
			this.contextQuickSearch.ResumeLayout(false);
			this.displayOptionPanel.ResumeLayout(false);
			this.searchBrowserContainer.ResumeLayout(false);
			this.openStackPanel.ResumeLayout(false);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.browserContainer.Panel1.ResumeLayout(false);
			this.browserContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.browserContainer)).EndInit();
			this.browserContainer.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private ItemView itemView;
		private SearchBrowserControl bookSelectorPanel;
		private ToolStrip toolStrip;
		private ToolStripSplitButton tbbView;
		private ToolStripMenuItem miViewThumbnails;
		private ToolStripMenuItem miViewTiles;
		private ToolStripMenuItem miViewDetails;
		private ToolStripSplitButton tbbSort;
		private ToolStripSplitButton tbbGroup;
		private ContextMenuStrip contextRating;
		private ToolStripMenuItem miRating0;
		private ToolStripMenuItem miRating1;
		private ToolStripMenuItem miRating2;
		private ToolStripMenuItem miRating3;
		private ToolStripMenuItem miRating4;
		private ToolStripMenuItem miRating5;
		private ToolStripMenuItem miRateMenu;
		private ContextMenuStrip contextMarkAs;
		private ToolStripMenuItem miMarkRead;
		private ToolStripMenuItem miMarkUnread;
		private ToolStripMenuItem miMarkAs;
		private ToolTip toolTip;
		private ContextMenuStrip contextMenuItems;
		private ToolStripMenuItem miRead;
		private ToolStripMenuItem miProperties;
		private ToolStripMenuItem miRevealBrowser;
		private ToolStripMenuItem miRefreshInformation;
		private ToolStripSeparator tsMarkAsSeparator;
		private ToolStripSeparator tsCopySeparator;
		private ToolStripMenuItem miSelectAll;
		private ToolStripMenuItem miInvertSelection;
		private ToolStripSeparator toolStripRemoveSeparator;
		private ToolStripMenuItem miRemove;
		private System.Windows.Forms.Timer quickSearchTimer;
		private ToolStripMenuItem miShowOnly;
		private ToolStripMenuItem miAutomation;
		private ToolStripSearchTextBox tsQuickSearch;
		private ContextMenuStrip contextQuickSearch;
		private ToolStripMenuItem miSearchAll;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripMenuItem miSearchSeries;
		private ToolStripMenuItem miSearchWriter;
		private ToolStripMenuItem miSearchArtists;
		private ToolStripMenuItem miSearchDescriptive;
		private ToolStripMenuItem miSearchFile;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem miShowOnlyAllComics;
		private ToolStripMenuItem miShowOnlyUnreadComics;
		private ToolStripMenuItem miShowOnlyReadingComics;
		private ToolStripMenuItem miShowOnlyReadComics;
		private ToolStripSeparator toolStripMenuItem3;
		private ToolStripMenuItem miAddLibrary;
		private ToolStripSplitButton tbbStack;
		private SizableContainer searchBrowserContainer;
		private ToolStripMenuItem miReadTab;
		private ToolStripSeparator toolStripMenuItem5;
		private ToolStripMenuItem miCopyData;
		private ToolStripMenuItem miPasteData;
		private ToolStripMenuItem miSetTopOfStack;
		private ToolStripDropDownButton tsListLayouts;
		private ToolStripMenuItem tsSaveListLayout;
		private ToolStripMenuItem tsEditLayouts;
		private ToolStripSeparator toolStripMenuItem23;
		private ToolStripMenuItem tsEditListLayout;
		private ToolStripMenuItem miExportComics;
		private ToolStripSeparator toolStripMenuItem7;
		private ToolStripMenuItem dummyToolStripMenuItem;
		private ToolStripMenuItem dummyToolStripMenuItem1;
		private ToolStripMenuItem dummyToolStripMenuItem2;
		private ToolStripMenuItem miSetListBackground;
		private ToolStripSeparator sepListBackground;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem miShowOnlyDuplicates;
		private Panel displayOptionPanel;
		private Label lblDisplayOptionText;
		private Button btDisplayAll;
		private ToolStripMenuItem miUpdateComicFiles;
		private Panel openStackPanel;
		private Label lblOpenStackText;
		private Button btCloseStack;
		private ToolStripMenuItem miAddList;
		private ToolStripMenuItem dummyEntryToolStripMenuItem;
		private ToolStripMenuItem miShowInList;
		private ToolStripMenuItem dummyEntryToolStripMenuItem1;
		private ToolStripMenuItem miEdit;
		private ToolStripSeparator sepDuplicateList;
		private ToolStripSeparator sepUndo;
		private ToolStripButton tbUndo;
		private ToolStripButton tbRedo;
		private Button btPrevStack;
		private Button btNextStack;
		private ToolStripMenuItem miResetListBackground;
		private ToolStripSeparator separatorListLayout;
		private ContextMenuStrip contextExport;
		private ToolStripMenuItem miExportComicsAs;
		private ToolStripMenuItem miExportComicsWithPrevious;
		private ToolStripSeparator toolStripMenuItem4;
		private ToolStripMenuItem miShowOnlyComics;
		private ToolStripMenuItem miShowOnlyFileless;
		private ToolStripButton btBrowsePrev;
		private ToolStripButton btBrowseNext;
		private ToolStripSeparator tbBrowseSeparator;
		private ToolStripMenuItem miSearchCatalog;
		private ToolStripSeparator toolStripMenuItem6;
		private ToolStripMenuItem miExpandAllGroups;
		private ToolStripMenuItem miSetStackThumbnail;
		private ToolStripMenuItem miRemoveStackThumbnail;
		private ToolStripButton tbSidebar;
		private ToolStripMenuItem miEditList;
		private ToolStripMenuItem miEditListMoveToTop;
		private ToolStripMenuItem miEditListMoveToBottom;
		private ToolStripSeparator toolStripMenuItem8;
		private ToolStripMenuItem miEditListApplyOrder;
		private ToolStripMenuItem miClearData;
		private ToolStripMenuItem miShowWeb;
		private ToolStripSeparator toolStripMenuItem9;
		private ToolStripMenuItem miMarkChecked;
		private ToolStripMenuItem miMarkUnchecked;
		private ToolStripDropDownButton tbbDuplicateList;
		private ToolStripMenuItem dummyEntryToolStripMenuItem2;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem miQuickRating;
		private ListViewEx lvGroupHeaders;
		private ToolStripMenuItem miShowGroupHeaders;
		private SplitContainer browserContainer;
		private ColumnHeader lvGroupsName;
		private ColumnHeader lvGroupsCount;
        private ToolStripMenuItem miOpenWith;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem miOpenWithManager;
		private ContextMenuStrip contextOpenWith;
	}
}
