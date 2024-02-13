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
	public partial class ComicPagesView : SubView, IDisplayWorkspace, IRefreshDisplay, IItemSize
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
				return new ItemSizeInfo(FormUtility.ScaleDpiY(Program.MinThumbHeight), FormUtility.ScaleDpiY(Program.MaxThumbHeight), pagesView.ItemView.ItemThumbSize.Height);
			case ItemViewMode.Tile:
				return new ItemSizeInfo(FormUtility.ScaleDpiY(Program.MinTileHeight), FormUtility.ScaleDpiY(Program.MaxTileHeight), pagesView.ItemView.ItemTileSize.Height);
			case ItemViewMode.Detail:
				return new ItemSizeInfo(FormUtility.ScaleDpiY(Program.MinRowHeight), FormUtility.ScaleDpiY(Program.MaxRowHeight), pagesView.ItemView.ItemRowHeight);
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
	}
}
