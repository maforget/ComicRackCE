using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicListFolderFilesBrowser : ComicListFilesBrowser, IDisplayWorkspace
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				components?.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ComicListFolderFilesBrowser));
            this.contextMenuFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miOpenWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.miOpenTab = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miAddToFavorites = new System.Windows.Forms.ToolStripMenuItem();
            this.miAddFolderLibrary = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.miRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.tvFolders = new cYo.Common.Windows.Forms.FolderTreeView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tbFavorites = new System.Windows.Forms.ToolStripButton();
            this.tbIncludeSubFolders = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbOpenWindow = new System.Windows.Forms.ToolStripButton();
            this.tbOpenTab = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbRefresh = new System.Windows.Forms.ToolStripButton();
            this.favContainer = new cYo.Common.Windows.Forms.SizableContainer();
            this.favView = new cYo.Common.Windows.Forms.ItemView();
            this.contextMenuFavorites = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miFavRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuFolders.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.favContainer.SuspendLayout();
            this.contextMenuFavorites.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuFolders
            // 
            this.contextMenuFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOpenWindow,
            this.miOpenTab,
            this.toolStripMenuItem1,
            this.miAddToFavorites,
            this.miAddFolderLibrary,
            this.menuItem2,
            this.miRefresh});
            this.contextMenuFolders.Name = "contextMenuFolders";
            this.contextMenuFolders.Size = new System.Drawing.Size(197, 126);
            // 
            // miOpenWindow
            // 
            this.miOpenWindow.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewWindow;
            this.miOpenWindow.Name = "miOpenWindow";
            this.miOpenWindow.Size = new System.Drawing.Size(196, 22);
            this.miOpenWindow.Text = "&Open in New Window";
            // 
            // miOpenTab
            // 
            this.miOpenTab.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.NewTab;
            this.miOpenTab.Name = "miOpenTab";
            this.miOpenTab.Size = new System.Drawing.Size(196, 22);
            this.miOpenTab.Text = "Open in New Tab";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(193, 6);
            // 
            // miAddToFavorites
            // 
            this.miAddToFavorites.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AddFavorites;
            this.miAddToFavorites.Name = "miAddToFavorites";
            this.miAddToFavorites.Size = new System.Drawing.Size(196, 22);
            this.miAddToFavorites.Text = "&Add Folder to Favorites";
            // 
            // miAddFolderLibrary
            // 
            this.miAddFolderLibrary.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.AddFolder;
            this.miAddFolderLibrary.Name = "miAddFolderLibrary";
            this.miAddFolderLibrary.Size = new System.Drawing.Size(196, 22);
            this.miAddFolderLibrary.Text = "&Add Folder to Library";
            // 
            // menuItem2
            // 
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(193, 6);
            // 
            // miRefresh
            // 
            this.miRefresh.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
            this.miRefresh.Name = "miRefresh";
            this.miRefresh.Size = new System.Drawing.Size(196, 22);
            this.miRefresh.Text = "&Refresh";
            // 
            // tvFolders
            // 
            this.tvFolders.ContextMenuStrip = this.contextMenuFolders;
            this.tvFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvFolders.FullRowSelect = true;
            this.tvFolders.HideSelection = false;
            this.tvFolders.ImageIndex = 0;
            this.tvFolders.Indent = 15;
            this.tvFolders.Location = new System.Drawing.Point(0, 185);
            this.tvFolders.Name = "tvFolders";
            this.tvFolders.SelectedImageIndex = 0;
            this.tvFolders.Size = new System.Drawing.Size(379, 269);
            this.tvFolders.TabIndex = 7;
            this.tvFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFolders_AfterSelect);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbFavorites,
            this.tbIncludeSubFolders,
            this.toolStripSeparator1,
            this.tbOpenWindow,
            this.tbOpenTab,
            this.toolStripSeparator2,
            this.tbRefresh});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(379, 25);
            this.toolStrip.TabIndex = 8;
            this.toolStrip.Text = "toolStrip1";
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
            // tbIncludeSubFolders
            // 
            this.tbIncludeSubFolders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbIncludeSubFolders.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.IncludeSubFolders;
            this.tbIncludeSubFolders.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbIncludeSubFolders.Name = "tbIncludeSubFolders";
            this.tbIncludeSubFolders.Size = new System.Drawing.Size(23, 22);
            this.tbIncludeSubFolders.Text = "Include all Subfolders";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // favContainer
            // 
            this.favContainer.AutoGripPosition = true;
            this.favContainer.Controls.Add(this.favView);
            this.favContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.favContainer.Grip = cYo.Common.Windows.Forms.SizableContainer.GripPosition.Bottom;
            this.favContainer.Location = new System.Drawing.Point(0, 25);
            this.favContainer.Name = "favContainer";
            this.favContainer.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.favContainer.Size = new System.Drawing.Size(379, 160);
            this.favContainer.TabIndex = 9;
            this.favContainer.Text = "favContainer";
            // 
            // favView
            // 
            this.favView.BackColor = ThemeColors.ComicListFolderFilesBrowser.FavViewBack;
            this.favView.ContextMenuStrip = this.contextMenuFavorites;
            this.favView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favView.GroupColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.favView.GroupColumnsKey = null;
            this.favView.GroupsStatus = ((cYo.Common.Windows.Forms.ItemViewGroupsStatus)(resources.GetObject("favView.GroupsStatus")));
            this.favView.ItemViewMode = cYo.Common.Windows.Forms.ItemViewMode.Tile;
            this.favView.Location = new System.Drawing.Point(0, 6);
            this.favView.Multiselect = false;
            this.favView.Name = "favView";
            this.favView.Size = new System.Drawing.Size(379, 148);
            this.favView.SortColumn = null;
            this.favView.SortColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.favView.SortColumnsKey = null;
            this.favView.StackColumns = new cYo.Common.Windows.Forms.IColumn[0];
            this.favView.StackColumnsKey = null;
            this.favView.TabIndex = 1;
            this.favView.SelectedIndexChanged += new System.EventHandler(this.favView_SelectedIndexChanged);
            this.favView.Resize += new System.EventHandler(this.favView_Resize);
            // 
            // contextMenuFavorites
            // 
            this.contextMenuFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFavRefresh,
            this.menuItem1,
            this.miRemove});
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
            // miRemove
            // 
            this.miRemove.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
            this.miRemove.Name = "miRemove";
            this.miRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.miRemove.Size = new System.Drawing.Size(150, 22);
            this.miRemove.Text = "&Remove...";
            // 
            // ComicListFolderFilesBrowser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tvFolders);
            this.Controls.Add(this.favContainer);
            this.Controls.Add(this.toolStrip);
            this.Name = "ComicListFolderFilesBrowser";
            this.Size = new System.Drawing.Size(379, 454);
            this.contextMenuFolders.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.favContainer.ResumeLayout(false);
            this.contextMenuFavorites.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private ContextMenuStrip contextMenuFolders;
        private ToolStripMenuItem miAddToFavorites;
        private ToolStripMenuItem miRefresh;
        private FolderTreeView tvFolders;
        private ToolStripSeparator menuItem2;
        private ToolStrip toolStrip;
        private ToolStripMenuItem miOpenWindow;
        private ToolStripButton tbOpenWindow;
        private ToolStripButton tbFavorites;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem miAddFolderLibrary;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tbRefresh;
        private ToolStripButton tbIncludeSubFolders;
        private SizableContainer favContainer;
        private ContextMenuStrip contextMenuFavorites;
        private ToolStripMenuItem miFavRefresh;
        private ToolStripSeparator menuItem1;
        private ToolStripMenuItem miRemove;
        private ItemView favView;
        private ToolStripButton tbOpenTab;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem miOpenTab;
    }
}
