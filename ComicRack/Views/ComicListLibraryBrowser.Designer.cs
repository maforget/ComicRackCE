#define TRACE
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicListLibraryBrowser : ComicListBrowser, IDisplayWorkspace, IImportComicList, ILibraryBrowser, IBrowseHistory
	{

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}


		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComicListLibraryBrowser));
			this.tvQueries = new cYo.Common.Windows.Forms.TreeViewEx();
			this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miEditSmartList = new System.Windows.Forms.ToolStripMenuItem();
			this.miQueryRename = new System.Windows.Forms.ToolStripMenuItem();
			this.miNodeSort = new System.Windows.Forms.ToolStripMenuItem();
			this.miNewSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.miNewFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.miNewList = new System.Windows.Forms.ToolStripMenuItem();
			this.miNewSmartList = new System.Windows.Forms.ToolStripMenuItem();
			this.miCopySeparator = new System.Windows.Forms.ToolStripSeparator();
			this.miCopyList = new System.Windows.Forms.ToolStripMenuItem();
			this.miPasteList = new System.Windows.Forms.ToolStripMenuItem();
			this.miExportReadingList = new System.Windows.Forms.ToolStripMenuItem();
			this.miImportReadingList = new System.Windows.Forms.ToolStripMenuItem();
			this.miOpenSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.miOpenWindow = new System.Windows.Forms.ToolStripMenuItem();
			this.miOpenTab = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.cmEditDevices = new System.Windows.Forms.ToolStripMenuItem();
			this.miAddToFavorites = new System.Windows.Forms.ToolStripMenuItem();
			this.miRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.miRemoveSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.miRemoveListOrFolder = new System.Windows.Forms.ToolStripMenuItem();
			this.treeImages = new System.Windows.Forms.ImageList(this.components);
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.tbNewFolder = new System.Windows.Forms.ToolStripButton();
			this.tbNewList = new System.Windows.Forms.ToolStripButton();
			this.tbNewSmartList = new System.Windows.Forms.ToolStripButton();
			this.tssOpenWindow = new System.Windows.Forms.ToolStripSeparator();
			this.tbOpenWindow = new System.Windows.Forms.ToolStripButton();
			this.tbOpenTab = new System.Windows.Forms.ToolStripButton();
			this.tbRefreshSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.tbExpandCollapseAll = new System.Windows.Forms.ToolStripButton();
			this.tbRefresh = new System.Windows.Forms.ToolStripButton();
			this.tbFavorites = new System.Windows.Forms.ToolStripButton();
			this.tsQuickSearch = new System.Windows.Forms.ToolStripButton();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.favContainer = new cYo.Common.Windows.Forms.SizableContainer();
			this.favView = new cYo.Common.Windows.Forms.ItemView();
			this.contextMenuFavorites = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.miFavRefresh = new System.Windows.Forms.ToolStripMenuItem();
			this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.miFavRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.queryCacheTimer = new System.Windows.Forms.Timer(this.components);
			this.quickSearchPanel = new System.Windows.Forms.Panel();
			this.quickSearch = new cYo.Common.Windows.Forms.SearchTextBox();
			this.treeContextMenu.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.favContainer.SuspendLayout();
			this.contextMenuFavorites.SuspendLayout();
			this.quickSearchPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvQueries
			// 
			this.tvQueries.AllowDrop = true;
			this.tvQueries.ContextMenuStrip = this.treeContextMenu;
			this.tvQueries.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvQueries.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tvQueries.FullRowSelect = true;
			this.tvQueries.HideSelection = false;
			this.tvQueries.ImageIndex = 0;
			this.tvQueries.ImageList = this.treeImages;
			this.tvQueries.ItemHeight = 18;
			this.tvQueries.LabelEdit = true;
			this.tvQueries.Location = new System.Drawing.Point(0, 213);
			this.tvQueries.Name = "tvQueries";
			this.tvQueries.SelectedImageIndex = 0;
			this.tvQueries.ShowLines = false;
			this.tvQueries.ShowNodeToolTips = true;
			this.tvQueries.Size = new System.Drawing.Size(397, 207);
			this.tvQueries.TabIndex = 0;
			this.tvQueries.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvQueries_BeforeLabelEdit);
			this.tvQueries.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.tvQueries_AfterLabelEdit);
			this.tvQueries.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvQueries_AfterCollapse);
			this.tvQueries.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvQueries_AfterExpand);
			this.tvQueries.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.tvQueries_DrawNode);
			this.tvQueries.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvQueries_ItemDrag);
			this.tvQueries.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvQueries_AfterSelect);
			this.tvQueries.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvQueries_NodeMouseDoubleClick);
			this.tvQueries.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvQueries_DragDrop);
			this.tvQueries.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvQueries_DragEnter);
			this.tvQueries.DragOver += new System.Windows.Forms.DragEventHandler(this.tvQueries_DragOver);
			this.tvQueries.DragLeave += new System.EventHandler(this.tvQueries_DragLeave);
			this.tvQueries.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.GiveDragCursorFeedback);
			this.tvQueries.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvQueries_MouseDown);
			// 
			// treeContextMenu
			// 
			this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditSmartList,
            this.miQueryRename,
            this.miNodeSort,
            this.miNewSeparator,
            this.miNewFolder,
            this.miNewList,
            this.miNewSmartList,
            this.miCopySeparator,
            this.miCopyList,
            this.miPasteList,
            this.miExportReadingList,
            this.miImportReadingList,
            this.miOpenSeparator,
            this.miOpenWindow,
            this.miOpenTab,
            this.toolStripMenuItem2,
            this.cmEditDevices,
            this.miAddToFavorites,
            this.miRefresh,
            this.miRemoveSeparator,
            this.miRemoveListOrFolder});
			this.treeContextMenu.Name = "treeContextMenu";
			this.treeContextMenu.Size = new System.Drawing.Size(260, 386);
			this.treeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.treeContextMenu_Opening);
			// 
			// miEditSmartList
			// 
			this.miEditSmartList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditSearchDocument;
			this.miEditSmartList.Name = "miEditSmartList";
			this.miEditSmartList.Size = new System.Drawing.Size(259, 22);
			this.miEditSmartList.Text = "&Edit...";
			// 
			// miQueryRename
			// 
			this.miQueryRename.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Rename;
			this.miQueryRename.Name = "miQueryRename";
			this.miQueryRename.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.miQueryRename.Size = new System.Drawing.Size(259, 22);
			this.miQueryRename.Text = "&Rename";
			// 
			// miNodeSort
			// 
			this.miNodeSort.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Sort;
			this.miNodeSort.Name = "miNodeSort";
			this.miNodeSort.Size = new System.Drawing.Size(259, 22);
			this.miNodeSort.Text = "&Sort";
			// 
			// miNewSeparator
			// 
			this.miNewSeparator.Name = "miNewSeparator";
			this.miNewSeparator.Size = new System.Drawing.Size(256, 6);
			// 
			// miNewFolder
			// 
			this.miNewFolder.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewSearchFolder;
			this.miNewFolder.Name = "miNewFolder";
			this.miNewFolder.Size = new System.Drawing.Size(259, 22);
			this.miNewFolder.Text = "New &Folder...";
			// 
			// miNewList
			// 
			this.miNewList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewList;
			this.miNewList.Name = "miNewList";
			this.miNewList.Size = new System.Drawing.Size(259, 22);
			this.miNewList.Text = "New &List...";
			// 
			// miNewSmartList
			// 
			this.miNewSmartList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewSearchDocument;
			this.miNewSmartList.Name = "miNewSmartList";
			this.miNewSmartList.Size = new System.Drawing.Size(259, 22);
			this.miNewSmartList.Text = "&New Smart List...";
			// 
			// miCopySeparator
			// 
			this.miCopySeparator.Name = "miCopySeparator";
			this.miCopySeparator.Size = new System.Drawing.Size(256, 6);
			// 
			// miCopyList
			// 
			this.miCopyList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditCopy;
			this.miCopyList.Name = "miCopyList";
			this.miCopyList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.miCopyList.Size = new System.Drawing.Size(259, 22);
			this.miCopyList.Text = "&Copy List";
			// 
			// miPasteList
			// 
			this.miPasteList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditPaste;
			this.miPasteList.Name = "miPasteList";
			this.miPasteList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.miPasteList.Size = new System.Drawing.Size(259, 22);
			this.miPasteList.Text = "&Paste List";
			// 
			// miExportReadingList
			// 
			this.miExportReadingList.Name = "miExportReadingList";
			this.miExportReadingList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
			this.miExportReadingList.Size = new System.Drawing.Size(259, 22);
			this.miExportReadingList.Text = "Export Reading List...";
			// 
			// miImportReadingList
			// 
			this.miImportReadingList.Name = "miImportReadingList";
			this.miImportReadingList.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
			this.miImportReadingList.Size = new System.Drawing.Size(259, 22);
			this.miImportReadingList.Text = "Import Reading List...";
			// 
			// miOpenSeparator
			// 
			this.miOpenSeparator.Name = "miOpenSeparator";
			this.miOpenSeparator.Size = new System.Drawing.Size(256, 6);
			// 
			// miOpenWindow
			// 
			this.miOpenWindow.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewWindow;
			this.miOpenWindow.Name = "miOpenWindow";
			this.miOpenWindow.Size = new System.Drawing.Size(259, 22);
			this.miOpenWindow.Text = "&Open in New Window";
			// 
			// miOpenTab
			// 
			this.miOpenTab.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewTab;
			this.miOpenTab.Name = "miOpenTab";
			this.miOpenTab.Size = new System.Drawing.Size(259, 22);
			this.miOpenTab.Text = "Open in New Tab";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(256, 6);
			// 
			// cmEditDevices
			// 
			this.cmEditDevices.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDevices;
			this.cmEditDevices.Name = "cmEditDevices";
			this.cmEditDevices.Size = new System.Drawing.Size(259, 22);
			this.cmEditDevices.Text = "Sync with Devices";
			this.cmEditDevices.Click += new System.EventHandler(this.cmEditDevices_Click);
			// 
			// miAddToFavorites
			// 
			this.miAddToFavorites.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AddListFavorites;
			this.miAddToFavorites.Name = "miAddToFavorites";
			this.miAddToFavorites.Size = new System.Drawing.Size(259, 22);
			this.miAddToFavorites.Text = "Add to Favorites";
			// 
			// miRefresh
			// 
			this.miRefresh.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			this.miRefresh.Name = "miRefresh";
			this.miRefresh.Size = new System.Drawing.Size(259, 22);
			this.miRefresh.Text = "Re&fresh";
			// 
			// miRemoveSeparator
			// 
			this.miRemoveSeparator.Name = "miRemoveSeparator";
			this.miRemoveSeparator.Size = new System.Drawing.Size(256, 6);
			// 
			// miRemoveListOrFolder
			// 
			this.miRemoveListOrFolder.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
			this.miRemoveListOrFolder.Name = "miRemoveListOrFolder";
			this.miRemoveListOrFolder.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.miRemoveListOrFolder.Size = new System.Drawing.Size(259, 22);
			this.miRemoveListOrFolder.Text = "&Re&move...";
			// 
			// treeImages
			// 
			this.treeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.treeImages.ImageSize = new System.Drawing.Size(16, 16);
			this.treeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbNewFolder,
            this.tbNewList,
            this.tbNewSmartList,
            this.tssOpenWindow,
            this.tbOpenWindow,
            this.tbOpenTab,
            this.tbRefreshSeparator,
            this.tbExpandCollapseAll,
            this.tbRefresh,
            this.tbFavorites,
            this.tsQuickSearch});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(397, 25);
			this.toolStrip.TabIndex = 0;
			this.toolStrip.Text = "toolStrip";
			// 
			// tbNewFolder
			// 
			this.tbNewFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbNewFolder.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewSearchFolder;
			this.tbNewFolder.Name = "tbNewFolder";
			this.tbNewFolder.Size = new System.Drawing.Size(23, 22);
			this.tbNewFolder.Text = "New &Folder";
			this.tbNewFolder.ToolTipText = "Create a new folder to organize your lists";
			// 
			// tbNewList
			// 
			this.tbNewList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbNewList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewList;
			this.tbNewList.Name = "tbNewList";
			this.tbNewList.Size = new System.Drawing.Size(23, 22);
			this.tbNewList.Text = "New &List";
			this.tbNewList.ToolTipText = "Create a new custom List";
			// 
			// tbNewSmartList
			// 
			this.tbNewSmartList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbNewSmartList.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewSearchDocument;
			this.tbNewSmartList.Name = "tbNewSmartList";
			this.tbNewSmartList.Size = new System.Drawing.Size(23, 22);
			this.tbNewSmartList.Text = "&New Smart List";
			this.tbNewSmartList.ToolTipText = "Create a new Smart List";
			// 
			// tssOpenWindow
			// 
			this.tssOpenWindow.Name = "tssOpenWindow";
			this.tssOpenWindow.Size = new System.Drawing.Size(6, 25);
			// 
			// tbOpenWindow
			// 
			this.tbOpenWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbOpenWindow.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewWindow;
			this.tbOpenWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbOpenWindow.Name = "tbOpenWindow";
			this.tbOpenWindow.Size = new System.Drawing.Size(23, 22);
			this.tbOpenWindow.Text = "New Window";
			this.tbOpenWindow.ToolTipText = "Open current list in new Window";
			// 
			// tbOpenTab
			// 
			this.tbOpenTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbOpenTab.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewTab;
			this.tbOpenTab.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbOpenTab.Name = "tbOpenTab";
			this.tbOpenTab.Size = new System.Drawing.Size(23, 22);
			this.tbOpenTab.Text = "New Tab";
			this.tbOpenTab.ToolTipText = "Open current list in new Tab";
			// 
			// tbRefreshSeparator
			// 
			this.tbRefreshSeparator.Name = "tbRefreshSeparator";
			this.tbRefreshSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// tbExpandCollapseAll
			// 
			this.tbExpandCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbExpandCollapseAll.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.ExpandCollapseAll;
			this.tbExpandCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbExpandCollapseAll.Name = "tbExpandCollapseAll";
			this.tbExpandCollapseAll.Size = new System.Drawing.Size(23, 22);
			this.tbExpandCollapseAll.Text = "Expand/Collapse all";
			// 
			// tbRefresh
			// 
			this.tbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbRefresh.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			this.tbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tbRefresh.Name = "tbRefresh";
			this.tbRefresh.Size = new System.Drawing.Size(23, 22);
			this.tbRefresh.Text = "Refresh";
			// 
			// tbFavorites
			// 
			this.tbFavorites.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tbFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tbFavorites.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Favorites;
			this.tbFavorites.Name = "tbFavorites";
			this.tbFavorites.Size = new System.Drawing.Size(23, 22);
			this.tbFavorites.Text = "Show Favorites";
			this.tbFavorites.ToolTipText = "Favorites";
			// 
			// tsQuickSearch
			// 
			this.tsQuickSearch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tsQuickSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsQuickSearch.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Search;
			this.tsQuickSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsQuickSearch.Name = "tsQuickSearch";
			this.tsQuickSearch.Size = new System.Drawing.Size(23, 22);
			this.tsQuickSearch.Text = "Quick Search (Ctrl+Alt+F)";
			// 
			// updateTimer
			// 
			this.updateTimer.Interval = 500;
			this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
			// 
			// favContainer
			// 
			this.favContainer.AutoGripPosition = true;
			this.favContainer.Controls.Add(this.favView);
			this.favContainer.Dock = System.Windows.Forms.DockStyle.Top;
			this.favContainer.Grip = cYo.Common.Windows.Forms.SizableContainer.GripPosition.Bottom;
			this.favContainer.Location = new System.Drawing.Point(0, 25);
			this.favContainer.Name = "favContainer";
			this.favContainer.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
			this.favContainer.Size = new System.Drawing.Size(397, 160);
			this.favContainer.TabIndex = 2;
			this.favContainer.Text = "favContainer";
			this.favContainer.ExpandedChanged += new System.EventHandler(this.favContainer_ExpandedChanged);
			// 
			// favView
			// 
			this.favView.BackColor = System.Drawing.SystemColors.Window;
			this.favView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.favView.GroupColumns = new cYo.Common.Windows.Forms.IColumn[0];
			this.favView.GroupColumnsKey = null;
			this.favView.GroupsStatus = ((cYo.Common.Windows.Forms.ItemViewGroupsStatus)(resources.GetObject("favView.GroupsStatus")));
			this.favView.ItemContextMenuStrip = this.contextMenuFavorites;
			this.favView.ItemViewMode = cYo.Common.Windows.Forms.ItemViewMode.Tile;
			this.favView.Location = new System.Drawing.Point(0, 6);
			this.favView.Multiselect = false;
			this.favView.Name = "favView";
			this.favView.Size = new System.Drawing.Size(397, 148);
			this.favView.SortColumn = null;
			this.favView.SortColumns = new cYo.Common.Windows.Forms.IColumn[0];
			this.favView.SortColumnsKey = null;
			this.favView.StackColumns = new cYo.Common.Windows.Forms.IColumn[0];
			this.favView.StackColumnsKey = null;
			this.favView.TabIndex = 0;
			this.favView.SelectedIndexChanged += new System.EventHandler(this.favView_SelectedIndexChanged);
			this.favView.Resize += new System.EventHandler(this.favView_Resize);
			// 
			// contextMenuFavorites
			// 
			this.contextMenuFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFavRefresh,
            this.menuItem1,
            this.miFavRemove});
			this.contextMenuFavorites.Name = "contextMenuFavorites";
			this.contextMenuFavorites.Size = new System.Drawing.Size(151, 54);
			// 
			// miFavRefresh
			// 
			this.miFavRefresh.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			this.miFavRefresh.Name = "miFavRefresh";
			this.miFavRefresh.Size = new System.Drawing.Size(150, 22);
			this.miFavRefresh.Text = "&Refresh";
			// 
			// menuItem1
			// 
			this.menuItem1.Name = "menuItem1";
			this.menuItem1.Size = new System.Drawing.Size(147, 6);
			// 
			// miFavRemove
			// 
			this.miFavRemove.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
			this.miFavRemove.Name = "miFavRemove";
			this.miFavRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			this.miFavRemove.Size = new System.Drawing.Size(150, 22);
			this.miFavRemove.Text = "&Remove...";
			// 
			// queryCacheTimer
			// 
			this.queryCacheTimer.Tick += new System.EventHandler(this.queryCacheTimer_Tick);
			// 
			// quickSearchPanel
			// 
			this.quickSearchPanel.AutoSize = true;
			this.quickSearchPanel.Controls.Add(this.quickSearch);
			this.quickSearchPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.quickSearchPanel.Location = new System.Drawing.Point(0, 185);
			this.quickSearchPanel.Name = "quickSearchPanel";
			this.quickSearchPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
			this.quickSearchPanel.Size = new System.Drawing.Size(397, 28);
			this.quickSearchPanel.TabIndex = 3;
			this.quickSearchPanel.Visible = false;
			// 
			// quickSearch
			// 
			this.quickSearch.ClearButtonImage = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallCloseGray;
			this.quickSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.quickSearch.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.quickSearch.Location = new System.Drawing.Point(0, 0);
			this.quickSearch.MinimumSize = new System.Drawing.Size(0, 24);
			this.quickSearch.Name = "quickSearch";
			this.quickSearch.Padding = new System.Windows.Forms.Padding(0);
			this.quickSearch.SearchButtonImage = null;
			this.quickSearch.SearchButtonVisible = false;
			this.quickSearch.Size = new System.Drawing.Size(397, 24);
			this.quickSearch.TabIndex = 0;
			this.quickSearch.TextChanged += new System.EventHandler(this.quickSearch_TextChanged);
			// 
			// ComicListLibraryBrowser
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tvQueries);
			this.Controls.Add(this.quickSearchPanel);
			this.Controls.Add(this.favContainer);
			this.Controls.Add(this.toolStrip);
			this.Name = "ComicListLibraryBrowser";
			this.Size = new System.Drawing.Size(397, 420);
			this.treeContextMenu.ResumeLayout(false);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.favContainer.ResumeLayout(false);
			this.contextMenuFavorites.ResumeLayout(false);
			this.quickSearchPanel.ResumeLayout(false);
			this.quickSearchPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

        private TreeViewEx tvQueries;
        private ContextMenuStrip treeContextMenu;
        private ToolStripMenuItem miEditSmartList;
        private ToolStripMenuItem miQueryRename;
        private ToolStripSeparator miRemoveSeparator;
        private ToolStripMenuItem miRemoveListOrFolder;
        private ToolStripSeparator miOpenSeparator;
        private ToolStripMenuItem miNewSmartList;
        private ToolStripMenuItem miNewFolder;
        private ImageList treeImages;
        private ToolStripMenuItem miNewList;
        private ToolStrip toolStrip;
        private ToolStripButton tbNewFolder;
        private ToolStripButton tbNewList;
        private ToolStripButton tbNewSmartList;
        private ToolStripMenuItem miOpenWindow;
        private ToolStripSeparator miNewSeparator;
        private ToolStripSeparator tssOpenWindow;
        private ToolStripButton tbOpenWindow;
        private ToolStripButton tbRefresh;
        private ToolStripMenuItem miRefresh;
        private ToolStripMenuItem miNodeSort;
        private ToolStripSeparator miCopySeparator;
        private ToolStripMenuItem miCopyList;
        private ToolStripMenuItem miPasteList;
        private ToolStripMenuItem miExportReadingList;
        private ToolStripMenuItem miImportReadingList;
        private ToolStripMenuItem miOpenTab;
        private ToolStripButton tbOpenTab;
        private ToolStripSeparator tbRefreshSeparator;
        private Timer updateTimer;
        private SizableContainer favContainer;
        private ItemView favView;
        private ToolStripButton tbFavorites;
        private ContextMenuStrip contextMenuFavorites;
        private ToolStripMenuItem miFavRefresh;
        private ToolStripSeparator menuItem1;
        private ToolStripMenuItem miFavRemove;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem miAddToFavorites;
        private ToolStripButton tbExpandCollapseAll;
        private Timer queryCacheTimer;
        private ToolStripMenuItem cmEditDevices;
        private ToolStripButton tsQuickSearch;
        private Panel quickSearchPanel;
        private SearchTextBox quickSearch;
    }
}
