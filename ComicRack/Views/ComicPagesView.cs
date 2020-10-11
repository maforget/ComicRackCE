using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Controls;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public class ComicPagesView : SubView, IDisplayWorkspace, IRefreshDisplay, IItemSize
	{
		private readonly string None;

		private readonly string ArrangedBy;

		private readonly string NotArranged;

		private readonly string GroupedBy;

		private readonly string NotGrouped;

		private readonly Image groupUp = Resources.GroupUp;

		private readonly Image groupDown = Resources.GroupDown;

		private readonly Image sortUp = Resources.SortUp;

		private readonly Image sortDown = Resources.SortDown;

		private readonly CommandMapper command = new CommandMapper();

		private EnumMenuUtility filterMenu;

		private IContainer components = new Container();

		private ToolStrip toolStrip;

		private ToolStripSplitButton tbbView;

		private ToolStripMenuItem miViewThumbnails;

		private ToolStripMenuItem miViewTiles;

		private ToolStripMenuItem miViewDetails;

		private ToolStripSplitButton tbbSort;

		private ToolStripSplitButton tbbGroup;

		private ToolStripDropDownButton tbFilter;

		private PagesView pagesView;

		private ToolStripSeparator toolStripMenuItem1;

		private ToolStripMenuItem miExpandAllGroups;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ComicPageType PageFilter
		{
			get
			{
				try
				{
					return base.Main.ComicDisplay.PageFilter;
				}
				catch
				{
					return pagesView.PageFilter;
				}
			}
			set
			{
				try
				{
					if (base.Main.ComicDisplay.PageFilter != value)
					{
						ComicDisplay comicDisplay = base.Main.ComicDisplay;
						ComicPageType pageFilter = (pagesView.PageFilter = value);
						comicDisplay.PageFilter = pageFilter;
					}
				}
				catch
				{
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemViewConfig ViewConfig
		{
			get
			{
				return pagesView.ViewConfig;
			}
			set
			{
				pagesView.ViewConfig = value;
			}
		}

		public ComicPagesView()
		{
			InitializeComponent();
			LocalizeUtility.Localize(this, components);
			components.Add(command);
			None = TR.Load(base.Name)["None", "None"];
			ArrangedBy = TR.Load(base.Name)["ArrangedBy", "Arranged by {0}"];
			NotArranged = TR.Load(base.Name)["NotArranged", "Not sorted"];
			GroupedBy = TR.Load(base.Name)["GroupedBy", "Grouped by {0}"];
			NotGrouped = TR.Load(base.Name)["NotGrouped", "Not grouped"];
			SubView.TranslateColumns(pagesView.ItemView.Columns);
			foreach (ItemViewColumn column in pagesView.ItemView.Columns)
			{
				column.TooltipText = ((ComicListField)column.Tag).Description;
			}
			pagesView.ItemView.ItemActivate += ItemView_ItemActivate;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnMainFormChanged()
		{
			base.OnMainFormChanged();
			base.Main.ComicDisplay.BookChanged += Viewer_BookChanged;
			filterMenu.Value = (int)PageFilter;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!base.DesignMode)
			{
				command.Add(delegate
				{
					pagesView.ItemView.ItemViewMode = ItemViewMode.Thumbnail;
				}, true, () => pagesView.ItemView.ItemViewMode == ItemViewMode.Thumbnail, miViewThumbnails);
				command.Add(delegate
				{
					pagesView.ItemView.ItemViewMode = ItemViewMode.Tile;
				}, true, () => pagesView.ItemView.ItemViewMode == ItemViewMode.Tile, miViewTiles);
				command.Add(delegate
				{
					pagesView.ItemView.ItemViewMode = ItemViewMode.Detail;
				}, true, () => pagesView.ItemView.ItemViewMode == ItemViewMode.Detail, miViewDetails);
				command.Add(pagesView.ItemView.ToggleGroups, () => pagesView.ItemView.AreGroupsVisible, miExpandAllGroups);
				filterMenu = new EnumMenuUtility(tbFilter, typeof(ComicPageType), flagsMode: true, null, Keys.None);
				filterMenu.ValueChanged += filterMenu_ValueChanged;
			}
		}

		private void filterMenu_ValueChanged(object sender, EventArgs e)
		{
			PageFilter = (ComicPageType)filterMenu.Value;
		}

		private void Viewer_BookChanged(object sender, EventArgs e)
		{
			pagesView.PageFilter = base.Main.ComicDisplay.PageFilter;
			pagesView.Book = base.Main.ComicDisplay.Book;
		}

		private void ItemView_ItemActivate(object sender, EventArgs e)
		{
			PageViewItem pageViewItem = pagesView.ItemView.FocusedItem as PageViewItem;
			if (pageViewItem != null)
			{
				pagesView.Book.Navigate(pageViewItem.Page, PageSeekOrigin.Absolute);
				base.Main.ShowComic();
			}
		}

		protected override void OnIdle()
		{
			if (base.Visible)
			{
				ItemView itemView = pagesView.ItemView;
				tbbSort.Enabled = itemView.Columns.Count != 0;
				tbbSort.Text = ((itemView.SortColumn != null) ? itemView.SortColumn.Text : None);
				tbbSort.ToolTipText = StringUtility.Format(ArrangedBy, tbbSort.Text);
				tbbGroup.Enabled = itemView.Columns.Count != 0;
				tbbGroup.Text = ((itemView.GroupColumn != null) ? itemView.GroupColumn.Text : None);
				tbbGroup.ToolTipText = StringUtility.Format(GroupedBy, tbbGroup.Text);
				tbbSort.Image = ((itemView.ItemSortOrder == SortOrder.Ascending) ? sortUp : sortDown);
				tbbGroup.Image = ((itemView.GroupSortingOrder == SortOrder.Ascending) ? groupDown : groupUp);
			}
		}

		private void tbbSort_DropDownOpening(object sender, EventArgs e)
		{
			FormUtility.SafeToolStripClear(tbbSort.DropDownItems);
			pagesView.ItemView.CreateArrangeMenu(tbbSort.DropDownItems);
		}

		private void tbbGroup_DropDownOpening(object sender, EventArgs e)
		{
			FormUtility.SafeToolStripClear(tbbGroup.DropDownItems);
			pagesView.ItemView.CreateGroupMenu(tbbGroup.DropDownItems);
		}

		private void tbbGroup_ButtonClick(object sender, EventArgs e)
		{
			pagesView.ItemView.GroupSortingOrder = ItemView.FlipSortOrder(pagesView.ItemView.GroupSortingOrder);
		}

		private void tbbSort_ButtonClick(object sender, EventArgs e)
		{
			pagesView.ItemView.ItemSortOrder = ItemView.FlipSortOrder(pagesView.ItemView.ItemSortOrder);
		}

		private void tbbView_ButtonClick(object sender, EventArgs e)
		{
			ItemViewMode itemViewMode = pagesView.ItemView.ItemViewMode;
			switch (itemViewMode)
			{
			case ItemViewMode.Thumbnail:
				itemViewMode = ItemViewMode.Tile;
				break;
			case ItemViewMode.Tile:
				itemViewMode = ItemViewMode.Detail;
				break;
			case ItemViewMode.Detail:
				itemViewMode = ItemViewMode.Thumbnail;
				break;
			}
			pagesView.ItemView.ItemViewMode = itemViewMode;
		}

		public virtual ItemSizeInfo GetItemSize()
		{
			switch (pagesView.ItemView.ItemViewMode)
			{
			case ItemViewMode.Thumbnail:
				return new ItemSizeInfo(FormUtility.ScaleDpiY(96), FormUtility.ScaleDpiY(512), pagesView.ItemView.ItemThumbSize.Height);
			case ItemViewMode.Tile:
				return new ItemSizeInfo(FormUtility.ScaleDpiY(64), FormUtility.ScaleDpiY(256), pagesView.ItemView.ItemTileSize.Height);
			case ItemViewMode.Detail:
				return new ItemSizeInfo(FormUtility.ScaleDpiY(12), FormUtility.ScaleDpiY(48), pagesView.ItemView.ItemRowHeight);
			default:
				return null;
			}
		}

		public virtual void SetItemSize(int value)
		{
			pagesView.SetItemSize(value);
		}

		public void SetWorkspace(DisplayWorkspace ws)
		{
			ViewConfig = ws.PagesViewConfig;
		}

		public void StoreWorkspace(DisplayWorkspace ws)
		{
			ws.PagesViewConfig = ViewConfig;
			if (ws.PagesViewConfig != null)
			{
				ws.PagesViewConfig.GroupsStatus = null;
			}
		}

		public void RefreshDisplay()
		{
			pagesView.RefreshDisplay();
		}

		private void InitializeComponent()
		{
			toolStrip = new System.Windows.Forms.ToolStrip();
			tbbView = new System.Windows.Forms.ToolStripSplitButton();
			miViewThumbnails = new System.Windows.Forms.ToolStripMenuItem();
			miViewTiles = new System.Windows.Forms.ToolStripMenuItem();
			miViewDetails = new System.Windows.Forms.ToolStripMenuItem();
			tbbGroup = new System.Windows.Forms.ToolStripSplitButton();
			tbbSort = new System.Windows.Forms.ToolStripSplitButton();
			tbFilter = new System.Windows.Forms.ToolStripDropDownButton();
			pagesView = new cYo.Projects.ComicRack.Viewer.Controls.PagesView();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			miExpandAllGroups = new System.Windows.Forms.ToolStripMenuItem();
			toolStrip.SuspendLayout();
			SuspendLayout();
			toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[4]
			{
				tbbView,
				tbbGroup,
				tbbSort,
				tbFilter
			});
			toolStrip.Location = new System.Drawing.Point(0, 0);
			toolStrip.Name = "toolStrip";
			toolStrip.Size = new System.Drawing.Size(656, 25);
			toolStrip.TabIndex = 1;
			toolStrip.Text = "toolStrip1";
			tbbView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5]
			{
				miViewThumbnails,
				miViewTiles,
				miViewDetails,
				toolStripMenuItem1,
				miExpandAllGroups
			});
			tbbView.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.View;
			tbbView.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbbView.Name = "tbbView";
			tbbView.Size = new System.Drawing.Size(69, 22);
			tbbView.Text = "Views";
			tbbView.ToolTipText = "Change how Books are displayed";
			tbbView.ButtonClick += new System.EventHandler(tbbView_ButtonClick);
			miViewThumbnails.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.ThumbView;
			miViewThumbnails.Name = "miViewThumbnails";
			miViewThumbnails.Size = new System.Drawing.Size(218, 22);
			miViewThumbnails.Text = "T&humbnails";
			miViewTiles.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TileView;
			miViewTiles.Name = "miViewTiles";
			miViewTiles.Size = new System.Drawing.Size(218, 22);
			miViewTiles.Text = "&Tiles";
			miViewDetails.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.DetailView;
			miViewDetails.Name = "miViewDetails";
			miViewDetails.Size = new System.Drawing.Size(218, 22);
			miViewDetails.Text = "&Details";
			tbbGroup.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Group;
			tbbGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbbGroup.Name = "tbbGroup";
			tbbGroup.Size = new System.Drawing.Size(72, 22);
			tbbGroup.Text = "Group";
			tbbGroup.ToolTipText = "Group Books by different criteria";
			tbbGroup.ButtonClick += new System.EventHandler(tbbGroup_ButtonClick);
			tbbGroup.DropDownOpening += new System.EventHandler(tbbGroup_DropDownOpening);
			tbbSort.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SortUp;
			tbbSort.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbbSort.Name = "tbbSort";
			tbbSort.Size = new System.Drawing.Size(81, 22);
			tbbSort.Text = "Arrange";
			tbbSort.ToolTipText = "Change the sort order of the Books";
			tbbSort.ButtonClick += new System.EventHandler(tbbSort_ButtonClick);
			tbbSort.DropDownOpening += new System.EventHandler(tbbSort_DropDownOpening);
			tbFilter.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			tbFilter.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Search;
			tbFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbFilter.Name = "tbFilter";
			tbFilter.Size = new System.Drawing.Size(91, 22);
			tbFilter.Text = "Page Filter";
			pagesView.Bookmark = null;
			pagesView.Dock = System.Windows.Forms.DockStyle.Fill;
			pagesView.Location = new System.Drawing.Point(0, 25);
			pagesView.Name = "pagesView";
			pagesView.Size = new System.Drawing.Size(656, 399);
			pagesView.TabIndex = 2;
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(215, 6);
			miExpandAllGroups.Name = "miExpandAllGroups";
			miExpandAllGroups.Size = new System.Drawing.Size(218, 22);
			miExpandAllGroups.Text = "Collapse/Expand all Groups";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.Controls.Add(pagesView);
			base.Controls.Add(toolStrip);
			base.Name = "ComicPagesView";
			base.Size = new System.Drawing.Size(656, 424);
			toolStrip.ResumeLayout(false);
			toolStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
