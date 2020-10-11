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
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class ComicListFolderFilesBrowser : ComicListFilesBrowser, IDisplayWorkspace
	{
		private readonly CommandMapper commands = new CommandMapper();

		private string cachedCurrentFolder = string.Empty;

		private SmartList<string> paths;

		private bool initialEnter = true;

		private IContainer components;

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

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CurrentFolder
		{
			get
			{
				if (tvFolders == null || tvFolders.Nodes.Count <= 0)
				{
					return cachedCurrentFolder;
				}
				return tvFolders.GetSelectedNodePath();
			}
			set
			{
				if (tvFolders == null || tvFolders.Nodes.Count == 0)
				{
					cachedCurrentFolder = value;
					return;
				}
				tvFolders.DrillToFolder(value);
				tvFolders.SelectedNode.EnsureVisible();
				cachedCurrentFolder = string.Empty;
			}
		}

		public SmartList<string> Paths
		{
			get
			{
				return paths;
			}
			set
			{
				if (paths != value)
				{
					if (paths != null)
					{
						paths.Changed -= paths_Changed;
					}
					paths = value;
					if (paths != null)
					{
						paths.Changed += paths_Changed;
					}
				}
			}
		}

		public override bool TopBrowserVisible
		{
			get
			{
				return favContainer.Expanded;
			}
			set
			{
				favContainer.Expanded = value;
			}
		}

		public override int TopBrowserSplit
		{
			get
			{
				return favContainer.ExpandedWidth;
			}
			set
			{
				favContainer.ExpandedWidth = value;
			}
		}

		public ComicListFolderFilesBrowser()
		{
			InitializeComponent();
			tvFolders.SortNetworkFolders = Program.ExtendedSettings.SortNetworkFolders;
			tvFolders.Font = SystemFonts.IconTitleFont;
			components.Add(commands);
			LocalizeUtility.Localize(this, components);
		}

		public ComicListFolderFilesBrowser(SmartList<string> paths)
			: this()
		{
			Paths = paths;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				components?.Dispose();
			}
			base.Dispose(disposing);
		}

		protected virtual void OnInitalDisplay()
		{
			this.BeginInvoke(delegate
			{
				tvFolders.Init();
				Update();
				if (!string.IsNullOrEmpty(cachedCurrentFolder))
				{
					CurrentFolder = cachedCurrentFolder;
				}
			});
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!base.DesignMode)
			{
				commands.Add(OpenWindow, miOpenWindow, tbOpenWindow);
				commands.Add(OpenTab, miOpenTab, tbOpenTab);
				commands.Add(AddToFavorites, miAddToFavorites);
				commands.Add(RefreshDisplay, tbRefresh, miRefresh);
				commands.Add(delegate
				{
					Program.Scanner.ScanFileOrFolder(CurrentFolder, all: true, removeMissing: false);
				}, miAddFolderLibrary);
				commands.Add(base.SwitchIncludeSubFolders, true, () => base.IncludeSubFolders, tbIncludeSubFolders);
				commands.Add(delegate
				{
					TopBrowserVisible = !TopBrowserVisible;
				}, true, () => TopBrowserVisible, tbFavorites);
				commands.Add(RefreshFavorites, miFavRefresh);
				commands.Add(RemoveFavorite, miRemove);
				CurrentFolder = Program.Settings.LastExplorerFolder;
				base.IncludeSubFolders = Program.Settings.ExplorerIncludeSubFolders;
				FillFavorites();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (initialEnter)
			{
				initialEnter = false;
				OnInitalDisplay();
			}
		}

		protected override void OnRefreshDisplay()
		{
			base.OnRefreshDisplay();
			using (new WaitCursor())
			{
				string currentFolder = CurrentFolder;
				tvFolders.Init();
				tvFolders.DrillToFolder(currentFolder);
			}
		}

		private void tvFolders_AfterSelect(object sender, TreeViewEventArgs e)
		{
			FillBooks(CurrentFolder);
		}

		private void paths_Changed(object sender, SmartListChangedEventArgs<string> e)
		{
			FillFavorites();
		}

		private void favView_SelectedIndexChanged(object sender, EventArgs e)
		{
			ItemViewItem itemViewItem = favView.FocusedItem as ItemViewItem;
			if (itemViewItem != null)
			{
				CurrentFolder = itemViewItem.Tag as string;
			}
		}

		private void favView_Resize(object sender, EventArgs e)
		{
			int width = favView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth - 8;
			favView.ItemTileSize = new Size(width, FormUtility.ScaleDpiY(50));
		}

		private void FillFavorites()
		{
			FillFavorites(refreshThumbnails: false);
		}

		private void FillFavorites(bool refreshThumbnails)
		{
			using (new WaitCursor())
			{
				if (Program.Settings == null)
				{
					return;
				}
				List<string> list = new List<string>();
				favView.Items.Clear();
				foreach (string favoriteFolder in Program.Settings.FavoriteFolders)
				{
					if (!Directory.Exists(favoriteFolder))
					{
						list.Add(favoriteFolder);
						continue;
					}
					FolderViewItem folderViewItem = FolderViewItem.Create(favoriteFolder);
					folderViewItem.Tag = favoriteFolder;
					folderViewItem.TooltipText = favoriteFolder;
					favView.Items.Add(folderViewItem);
					if (refreshThumbnails)
					{
						Program.ImagePool.Thumbs.RefreshImage(folderViewItem.ThumbnailKey);
					}
				}
				Program.Settings.FavoriteFolders.RemoveRange(list);
			}
		}

		private void RemoveFavorite()
		{
			ItemViewItem itemViewItem = ((favView.FocusedItem == null) ? null : (favView.FocusedItem as ItemViewItem));
			if (itemViewItem != null && Program.AskQuestion(this, TR.Messages["AskRemoveFavorite", "Do you really want to remove this Favorite Folder link?"], TR.Messages["Remove", "Remove"], HiddenMessageBoxes.RemoveFavorite))
			{
				Program.Settings.FavoriteFolders.Remove(itemViewItem.Tag as string);
				favView.Items.Remove(itemViewItem);
			}
		}

		private void RefreshFavorites()
		{
			FillFavorites(refreshThumbnails: true);
		}

		private void AddToFavorites()
		{
			if (!Program.Settings.FavoriteFolders.Contains(CurrentFolder))
			{
				Program.Settings.FavoriteFolders.Add(CurrentFolder);
			}
		}

		private void OpenWindow()
		{
			try
			{
				OpenListInNewWindow();
			}
			catch
			{
			}
		}

		private void OpenTab()
		{
			try
			{
				OpenListInNewTab(Resources.SearchFolder);
			}
			catch
			{
			}
		}

		public void SetWorkspace(DisplayWorkspace workspace)
		{
		}

		public void StoreWorkspace(DisplayWorkspace workspace)
		{
			Program.Settings.ExplorerIncludeSubFolders = base.IncludeSubFolders;
			Program.Settings.LastExplorerFolder = CurrentFolder;
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cYo.Projects.ComicRack.Viewer.Views.ComicListFolderFilesBrowser));
			contextMenuFolders = new System.Windows.Forms.ContextMenuStrip(components);
			miOpenWindow = new System.Windows.Forms.ToolStripMenuItem();
			miOpenTab = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			miAddToFavorites = new System.Windows.Forms.ToolStripMenuItem();
			miAddFolderLibrary = new System.Windows.Forms.ToolStripMenuItem();
			menuItem2 = new System.Windows.Forms.ToolStripSeparator();
			miRefresh = new System.Windows.Forms.ToolStripMenuItem();
			tvFolders = new cYo.Common.Windows.Forms.FolderTreeView();
			toolStrip = new System.Windows.Forms.ToolStrip();
			tbFavorites = new System.Windows.Forms.ToolStripButton();
			tbIncludeSubFolders = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			tbOpenWindow = new System.Windows.Forms.ToolStripButton();
			tbOpenTab = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			tbRefresh = new System.Windows.Forms.ToolStripButton();
			favContainer = new cYo.Common.Windows.Forms.SizableContainer();
			favView = new cYo.Common.Windows.Forms.ItemView();
			contextMenuFavorites = new System.Windows.Forms.ContextMenuStrip(components);
			miFavRefresh = new System.Windows.Forms.ToolStripMenuItem();
			menuItem1 = new System.Windows.Forms.ToolStripSeparator();
			miRemove = new System.Windows.Forms.ToolStripMenuItem();
			contextMenuFolders.SuspendLayout();
			toolStrip.SuspendLayout();
			favContainer.SuspendLayout();
			contextMenuFavorites.SuspendLayout();
			SuspendLayout();
			contextMenuFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[7]
			{
				miOpenWindow,
				miOpenTab,
				toolStripMenuItem1,
				miAddToFavorites,
				miAddFolderLibrary,
				menuItem2,
				miRefresh
			});
			contextMenuFolders.Name = "contextMenuFolders";
			contextMenuFolders.Size = new System.Drawing.Size(363, 206);
			miOpenWindow.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewWindow;
			miOpenWindow.Name = "miOpenWindow";
			miOpenWindow.Size = new System.Drawing.Size(362, 38);
			miOpenWindow.Text = "&Open in New Window";
			miOpenTab.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewTab;
			miOpenTab.Name = "miOpenTab";
			miOpenTab.Size = new System.Drawing.Size(362, 38);
			miOpenTab.Text = "Open in New Tab";
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(359, 6);
			miAddToFavorites.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.AddFavorites;
			miAddToFavorites.Name = "miAddToFavorites";
			miAddToFavorites.Size = new System.Drawing.Size(362, 38);
			miAddToFavorites.Text = "&Add Folder to Favorites";
			miAddFolderLibrary.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.AddFolder;
			miAddFolderLibrary.Name = "miAddFolderLibrary";
			miAddFolderLibrary.Size = new System.Drawing.Size(362, 38);
			miAddFolderLibrary.Text = "&Add Folder to Library";
			menuItem2.Name = "menuItem2";
			menuItem2.Size = new System.Drawing.Size(359, 6);
			miRefresh.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			miRefresh.Name = "miRefresh";
			miRefresh.Size = new System.Drawing.Size(362, 38);
			miRefresh.Text = "&Refresh";
			tvFolders.ContextMenuStrip = contextMenuFolders;
			tvFolders.Dock = System.Windows.Forms.DockStyle.Fill;
			tvFolders.FullRowSelect = true;
			tvFolders.HideSelection = false;
			tvFolders.ImageIndex = 0;
			tvFolders.Indent = 15;
			tvFolders.Location = new System.Drawing.Point(0, 199);
			tvFolders.Name = "tvFolders";
			tvFolders.SelectedImageIndex = 0;
			tvFolders.Size = new System.Drawing.Size(379, 255);
			tvFolders.TabIndex = 7;
			tvFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(tvFolders_AfterSelect);
			toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[7]
			{
				tbFavorites,
				tbIncludeSubFolders,
				toolStripSeparator1,
				tbOpenWindow,
				tbOpenTab,
				toolStripSeparator2,
				tbRefresh
			});
			toolStrip.Location = new System.Drawing.Point(0, 0);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new System.Drawing.Size(379, 39);
			toolStrip.TabIndex = 8;
			toolStrip.Text = "toolStrip1";
			tbFavorites.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			tbFavorites.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbFavorites.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Favorites;
			tbFavorites.Name = "tbFavorites";
			tbFavorites.Size = new System.Drawing.Size(36, 36);
			tbFavorites.Text = "Show Favorites";
			tbFavorites.ToolTipText = "Favorites";
			tbIncludeSubFolders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbIncludeSubFolders.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.IncludeSubFolders;
			tbIncludeSubFolders.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbIncludeSubFolders.Name = "tbIncludeSubFolders";
			tbIncludeSubFolders.Size = new System.Drawing.Size(36, 36);
			tbIncludeSubFolders.Text = "Include all Subfolders";
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
			tbOpenWindow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbOpenWindow.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewWindow;
			tbOpenWindow.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbOpenWindow.Name = "tbOpenWindow";
			tbOpenWindow.Size = new System.Drawing.Size(36, 36);
			tbOpenWindow.Text = "New Window";
			tbOpenWindow.ToolTipText = "Open current list in new Window";
			tbOpenTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbOpenTab.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.NewTab;
			tbOpenTab.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbOpenTab.Name = "tbOpenTab";
			tbOpenTab.Size = new System.Drawing.Size(36, 36);
			tbOpenTab.Text = "New Tab";
			tbOpenTab.ToolTipText = "Open current list in new Tab";
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(6, 39);
			tbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbRefresh.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			tbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbRefresh.Name = "tbRefresh";
			tbRefresh.Size = new System.Drawing.Size(36, 36);
			tbRefresh.Text = "Refresh";
			favContainer.AutoGripPosition = true;
			favContainer.Controls.Add(favView);
			favContainer.Dock = System.Windows.Forms.DockStyle.Top;
			favContainer.Grip = cYo.Common.Windows.Forms.SizableContainer.GripPosition.Bottom;
			favContainer.Location = new System.Drawing.Point(0, 39);
			favContainer.Name = "favContainer";
			favContainer.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
			favContainer.Size = new System.Drawing.Size(379, 160);
			favContainer.TabIndex = 9;
			favContainer.Text = "favContainer";
			favView.BackColor = System.Drawing.SystemColors.Window;
			favView.ContextMenuStrip = contextMenuFavorites;
			favView.Dock = System.Windows.Forms.DockStyle.Fill;
			favView.GroupColumns = new cYo.Common.Windows.Forms.IColumn[0];
			favView.GroupColumnsKey = null;
			favView.GroupsStatus = (cYo.Common.Windows.Forms.ItemViewGroupsStatus)resources.GetObject("favView.GroupsStatus");
			favView.ItemViewMode = cYo.Common.Windows.Forms.ItemViewMode.Tile;
			favView.Location = new System.Drawing.Point(0, 6);
			favView.Multiselect = false;
			favView.Name = "favView";
			favView.Size = new System.Drawing.Size(379, 148);
			favView.SortColumn = null;
			favView.SortColumns = new cYo.Common.Windows.Forms.IColumn[0];
			favView.SortColumnsKey = null;
			favView.StackColumns = new cYo.Common.Windows.Forms.IColumn[0];
			favView.StackColumnsKey = null;
			favView.TabIndex = 1;
			favView.SelectedIndexChanged += new System.EventHandler(favView_SelectedIndexChanged);
			favView.Resize += new System.EventHandler(favView_Resize);
			contextMenuFavorites.Items.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
				miFavRefresh,
				menuItem1,
				miRemove
			});
			contextMenuFavorites.Name = "contextMenuFavorites";
			contextMenuFavorites.Size = new System.Drawing.Size(268, 86);
			miFavRefresh.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			miFavRefresh.Name = "miFavRefresh";
			miFavRefresh.Size = new System.Drawing.Size(267, 38);
			miFavRefresh.Text = "&Refresh";
			menuItem1.Name = "menuItem1";
			menuItem1.Size = new System.Drawing.Size(264, 6);
			miRemove.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.EditDelete;
			miRemove.Name = "miRemove";
			miRemove.ShortcutKeys = System.Windows.Forms.Keys.Delete;
			miRemove.Size = new System.Drawing.Size(267, 38);
			miRemove.Text = "&Remove...";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.Controls.Add(tvFolders);
			base.Controls.Add(favContainer);
			base.Controls.Add(toolStrip);
			base.Name = "ComicListFolderFilesBrowser";
			base.Size = new System.Drawing.Size(379, 454);
			contextMenuFolders.ResumeLayout(false);
			toolStrip.ResumeLayout(false);
			toolStrip.PerformLayout();
			favContainer.ResumeLayout(false);
			contextMenuFavorites.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
