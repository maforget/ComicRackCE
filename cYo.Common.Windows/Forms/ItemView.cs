using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Threading;
using cYo.Common.Win32;

namespace cYo.Common.Windows.Forms
{
	public class ItemView : ScrollControl
	{
		#region Classes
		private class DisplayItem
		{
			private Rectangle bounds;

			public Rectangle Bounds => bounds;

			public DisplayItem(Rectangle bounds)
			{
				this.bounds = bounds;
			}

			public void Offset(int x, int y)
			{
				bounds.Offset(x, y);
			}
		}

		private class ItemInformation : DisplayItem
		{
			public IViewableItem Item
			{
				get;
				private set;
			}

			public int Column
			{
				get;
				private set;
			}

			public int Row
			{
				get;
				private set;
			}

			public GroupHeaderInformation Group
			{
				get;
				private set;
			}

			public ItemInformation(IViewableItem item, Rectangle bounds, int column, int row, GroupHeaderInformation group)
				: base(bounds)
			{
				Item = item;
				Column = column;
				Row = row;
				Group = group;
			}
		}

		private class StateChangedEventArgs : EventArgs
		{
			public IViewableItem Item
			{
				get;
				private set;
			}

			public ItemViewStates OldState
			{
				get;
				private set;
			}

			public ItemViewStates NewState
			{
				get;
				private set;
			}

			public StateChangedEventArgs(IViewableItem item, ItemViewStates oldState, ItemViewStates newState)
			{
				Item = item;
				OldState = oldState;
				NewState = newState;
			}
		}

		private class StateInfo
		{
			private readonly Dictionary<IViewableItem, ItemViewStates> stateDict = new Dictionary<IViewableItem, ItemViewStates>();

			public ItemViewStates this[IViewableItem item]
			{
				get
				{
					using (ItemMonitor.Lock(stateDict))
					{
						ItemViewStates value = ItemViewStates.None;
						if (item != null)
						{
							stateDict.TryGetValue(item, out value);
						}
						return value;
					}
				}
				set
				{
					if (item == null)
					{
						return;
					}
					ItemViewStates itemViewStates = this[item];
					if (itemViewStates == value)
					{
						return;
					}
					using (ItemMonitor.Lock(stateDict))
					{
						if (value == ItemViewStates.None)
						{
							stateDict.Remove(item);
						}
						else
						{
							stateDict[item] = value;
						}
					}
					if (this.StateChanged != null)
					{
						this.StateChanged(this, new StateChangedEventArgs(item, itemViewStates, value));
					}
				}
			}

			public event EventHandler<StateChangedEventArgs> StateChanged;

			public StateInfo()
			{
			}

			public StateInfo(StateInfo org)
			{
				using (ItemMonitor.Lock(org))
				{
					stateDict = new Dictionary<IViewableItem, ItemViewStates>(org.stateDict);
				}
			}

			public void Set(IViewableItem item, ItemViewStates mask, bool on)
			{
				ItemViewStates itemViewStates = this[item];
				itemViewStates = (this[item] = ((!on) ? (itemViewStates & ~mask) : (itemViewStates | mask)));
			}

			public void Flip(IViewableItem item, ItemViewStates mask)
			{
				this[item] ^= mask;
			}

			public void Clear(ItemViewStates mask)
			{
				GetItems().ForEach((IViewableItem vi) =>
				{
					Set(vi, mask, on: false);
				});
			}

			public void Focus(IViewableItem item)
			{
				Clear(ItemViewStates.Focused);
				if (item != null)
				{
					Set(item, ItemViewStates.Focused, on: true);
				}
			}

			public IViewableItem FindFirst(ItemViewStates mask)
			{
				using (ItemMonitor.Lock(stateDict))
				{
					return stateDict.Keys.FirstOrDefault((IViewableItem item) => (this[item] & mask) != 0);
				}
			}

			private IViewableItem[] GetItems()
			{
				using (ItemMonitor.Lock(stateDict))
				{
					return stateDict.Keys.ToArray();
				}
			}

			public void Update(StateInfo si)
			{
				using (ItemMonitor.Lock(si.stateDict))
				{
					using (ItemMonitor.Lock(stateDict))
					{
						IViewableItem[] array = stateDict.Keys.ToArray();
						foreach (IViewableItem viewableItem in array)
						{
							if (!si.stateDict.ContainsKey(viewableItem))
							{
								this[viewableItem] = ItemViewStates.None;
							}
						}
					}
					foreach (IViewableItem key in si.stateDict.Keys)
					{
						this[key] = si[key];
					}
				}
			}
		}

		public class StackInfo
		{
			private readonly GroupContainer<IViewableItem> container;

			public string Text => GroupInfo.Caption;

			public object Key => GroupInfo.Key;

			public List<IViewableItem> Items => container.Items;

			public IGroupInfo GroupInfo => container.Info;

			public StackInfo(GroupContainer<IViewableItem> groupContainer)
			{
				container = groupContainer;
			}
		}

		public class StackEventArgs : EventArgs
		{
			private readonly StackInfo stack;

			public StackInfo Stack => stack;

			public StackEventArgs(StackInfo stack)
			{
				this.stack = stack;
			}
		}

		private class AlphabetGrouper : IGrouper<IViewableItem>
		{
			private readonly IGrouper<IViewableItem> grouper;

			private readonly Dictionary<object, IGroupInfo> groupDict = new Dictionary<object, IGroupInfo>();

			public bool IsMultiGroup => false;

			public AlphabetGrouper(IGrouper<IViewableItem> grouper)
			{
				this.grouper = grouper;
			}

			public IEnumerable<IGroupInfo> GetGroups(IViewableItem item)
			{
				throw new NotImplementedException();
			}

			public IGroupInfo GetGroup(IViewableItem item)
			{
				IGroupInfo group = grouper.GetGroup(item);
				using (ItemMonitor.Lock(groupDict))
				{
					if (!groupDict.ContainsKey(group.Key))
					{
						groupDict[group.Key] = GroupInfo.GetAlphabetGroup(group.Caption, articleAware: true);
					}
					return groupDict[group.Key];
				}
			}
		} 
		#endregion

		#region Fields
		private const int maxPopupSize = 20;

		private const int longClickDelta = 2;

		private static readonly TR tr = TR.Load(typeof(ItemView).Name);

		private volatile bool pendingSelectedIndexChanged;

		private volatile bool positionsInvalidated;

		private volatile bool itemsResort;

		private readonly StateInfo itemStates = new StateInfo();

		private Dictionary<IViewableItem, ItemInformation> itemInfos = new Dictionary<IViewableItem, ItemInformation>();

		private Dictionary<IViewableItem, StackInfo> stackInfo = new Dictionary<IViewableItem, StackInfo>();

		private List<GroupHeaderInformation> displayedGroups = new List<GroupHeaderInformation>();

		private volatile bool multiselect = true;

		private volatile bool hideSelection = true;

		private volatile SortOrder itemSortOrder = SortOrder.Ascending;

		private volatile bool groupDisplayEnabled;

		private volatile SortOrder groupSortingOrder = SortOrder.Ascending;

		private bool stackDisplayEnabled;

		private HorizontalAlignment horizontalItemAlignment;

		private Bitmap groupExpandedImage;

		private Bitmap groupCollapsedImage;

		private bool showHeader = true;

		private int columnHeaderHeight = 20;

		private ContentAlignment backgroundImageAlignment = ContentAlignment.TopLeft;

		private volatile ItemViewLayout itemViewLayout;

		private volatile ItemViewMode itemViewMode;

		private volatile string expandedDetailColumnName;

		private volatile int expandedDetailColumnMinimumHeight = -160;

		private Size itemPadding = new Size(1, 1);

		private Size itemThumbSize = new Size(128, 128);

		private Size itemTileSize = new Size(192, 96);

		private volatile int itemRowHeight = 16;

		private volatile int groupHeaderHeight = 40;

		private volatile bool showGroupCount = true;

		private volatile IViewableItem markerItem;

		private bool markerVisible;

		private volatile bool groupHeaderTrueCount;

		private readonly ViewableItemCollection<IViewableItem> items = new ViewableItemCollection<IViewableItem>();

		private readonly ItemViewColumnCollection<IColumn> columns = new ItemViewColumnCollection<IColumn>();

		private readonly List<IComparer<IViewableItem>> itemSorters = new List<IComparer<IViewableItem>>(new IComparer<IViewableItem>[1]);

		private volatile IGrouper<IViewableItem> itemGrouper;

		private volatile IGrouper<IViewableItem> itemStacker;

		private volatile IComparer<IViewableItem> itemStackSorter;

		private ItemViewGroupsStatus groupsStatus = new ItemViewGroupsStatus(null);

		private List<IViewableItem> displayedItems = new List<IViewableItem>();

		private readonly List<IViewableItem> selectedItems = new List<IViewableItem>();

		private readonly List<IViewableItem> visibleItems = new List<IViewableItem>();

		private int currentFontHeight;

		private MouseButtons activateButton;

		private IViewableItem anchorItem;

		private int updateBlockCount;

		private volatile bool pendingUpdate;

		private const int ColumnOffsetX = 8;

		private const int markerWidth = 2;

		private IColumn resizeColumn;

		private int resizeColumnPos;

		private int resizeColumnWidth;

		private IColumn pressedHeader;

		private Point pressedHeaderPoint;

		private IColumn hotHeader;

		private bool customClick;

		private IViewableItem clickItem;

		private IViewableItem dragItem;

		private Point pressetViewPoint;

		private StateInfo selectItemState;

		private IColumn dragHeader;

		private int longClickSubItem = -1;

		private Point mouseDownPoint;

		private MouseButtons lastMouseButton;

		private IViewableItem longClickItem;

		private Rectangle selectionRect = Rectangle.Empty;

		private bool doubleGroupClick;

		private IViewableItem currentInplaceEditItem;

		private int currentInplaceEditSubItem;

		private Control currentInplaceEditControl;

		private IContainer components;

		private ToolStripMenuItem dummyItem;

		private ContextMenuStrip autoHeaderContextMenuStrip;

		private ContextMenuStrip autoViewContextMenuStrip;

		private ToolStripMenuItem miViewMode;

		private ToolStripMenuItem miViewThumbs;

		private ToolStripMenuItem miViewTiles;

		private ToolStripMenuItem miViewDetails;

		private ToolStripMenuItem miArrange;

		private ToolStripMenuItem miGroup;

		private Timer longClickTimer;

		private ToolStripMenuItem miStack;

		private ToolTip toolTip; 
		#endregion

		#region Properties
		[Category("Behavior")]
		[DefaultValue(true)]
		public bool LabelEdit
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool HeaderToolTips
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool AutomaticHeaderMenu
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(null)]
		public ContextMenuStrip HeaderContextMenuStrip
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool AutomaticViewMenu
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(null)]
		public ContextMenuStrip ViewContextMenuStrip
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(null)]
		public ContextMenuStrip ItemContextMenuStrip
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool ItemsOwned
		{
			get;
			set;
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool Multiselect
		{
			get
			{
				return multiselect;
			}
			set
			{
				if (multiselect != value)
				{
					multiselect = value;
					SafeInvalidate();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool HideSelection
		{
			get
			{
				return hideSelection;
			}
			set
			{
				if (hideSelection != value)
				{
					hideSelection = value;
					SafeInvalidate();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(SortOrder.Ascending)]
		public SortOrder ItemSortOrder
		{
			get
			{
				return itemSortOrder;
			}
			set
			{
				if (value != itemSortOrder)
				{
					itemSortOrder = value;
					if (ItemSorter != null)
					{
						SafeInvalidate(ItemViewInvalidateOptions.Full);
					}
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(false)]
		public bool GroupDisplayEnabled
		{
			get
			{
				return groupDisplayEnabled;
			}
			set
			{
				if (value != groupDisplayEnabled)
				{
					groupDisplayEnabled = value;
					SafeInvalidate(ItemViewInvalidateOptions.Full);
					OnGroupDisplayChanged();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(SortOrder.Ascending)]
		public SortOrder GroupSortingOrder
		{
			get
			{
				return groupSortingOrder;
			}
			set
			{
				if (value != groupSortingOrder)
				{
					groupSortingOrder = value;
					if (ItemGrouper != null)
					{
						SafeInvalidate(ItemViewInvalidateOptions.Full);
					}
					OnGroupDisplayChanged();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(false)]
		public bool StackDisplayEnabled
		{
			get
			{
				return stackDisplayEnabled;
			}
			set
			{
				if (value != stackDisplayEnabled)
				{
					stackDisplayEnabled = value;
					SafeInvalidate(ItemViewInvalidateOptions.Full);
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(SelectionMode.MultiSimple)]
		public SelectionMode SelectionMode
		{
			get;
			set;
		}

		[Category("Appearance")]
		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment HorizontalItemAlignment
		{
			get
			{
				return horizontalItemAlignment;
			}
			set
			{
				if (horizontalItemAlignment != value)
				{
					horizontalItemAlignment = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Bitmap GroupExpandedImage
		{
			get
			{
				return groupExpandedImage;
			}
			set
			{
				if (groupExpandedImage != value)
				{
					groupExpandedImage = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Bitmap GroupCollapsedImage
		{
			get
			{
				return groupCollapsedImage;
			}
			set
			{
				if (groupCollapsedImage != value)
				{
					groupCollapsedImage = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool ShowHeader
		{
			get
			{
				return showHeader;
			}
			set
			{
				if (showHeader != value)
				{
					showHeader = value;
					if (ItemViewMode == ItemViewMode.Detail)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(20)]
		public int ColumnHeaderHeight
		{
			get
			{
				return columnHeaderHeight;
			}
			set
			{
				if (columnHeaderHeight != value)
				{
					columnHeaderHeight = value;
					if (IsHeaderVisible)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(ContentAlignment.TopLeft)]
		public ContentAlignment BackgroundImageAlignment
		{
			get
			{
				return backgroundImageAlignment;
			}
			set
			{
				if (backgroundImageAlignment != value)
				{
					backgroundImageAlignment = value;
					if (BackgroundImage != null)
					{
						Invalidate();
					}
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(ItemViewLayout.Top)]
		public ItemViewLayout ItemViewLayout
		{
			get
			{
				return itemViewLayout;
			}
			set
			{
				if (value != itemViewLayout)
				{
					itemViewLayout = value;
					SafeInvalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(ItemViewMode.Thumbnail)]
		public ItemViewMode ItemViewMode
		{
			get
			{
				return itemViewMode;
			}
			set
			{
				if (value != itemViewMode)
				{
					itemViewMode = value;
					IViewableItem focusedItem = FocusedItem;
					SafeInvalidate(ItemViewInvalidateOptions.Full);
					if (focusedItem != null)
					{
						EnsureItemVisible(focusedItem);
					}
					OnItemDisplayChanged();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public string ExpandedDetailColumnName
		{
			get
			{
				return expandedDetailColumnName;
			}
			set
			{
				if (!(value == expandedDetailColumnName))
				{
					expandedDetailColumnName = value;
					if (GetExpandedColumn() != null)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(-160)]
		public int ExpandedDetailColumnMinimumHeight
		{
			get
			{
				return expandedDetailColumnMinimumHeight;
			}
			set
			{
				if (value != expandedDetailColumnMinimumHeight)
				{
					expandedDetailColumnMinimumHeight = value;
					if (GetExpandedColumn() != null)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(typeof(Size), "1, 1")]
		public Size ItemPadding
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return itemPadding;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (value == itemPadding)
					{
						return;
					}
					itemPadding = value;
				}
				SafeInvalidate();
			}
		}

		[Category("Appearance")]
		[DefaultValue(typeof(Size), "128, 128")]
		public Size ItemThumbSize
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return itemThumbSize;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (itemThumbSize == value)
					{
						return;
					}
					itemThumbSize = value;
				}
				if (ItemViewMode == ItemViewMode.Thumbnail)
				{
					SafeInvalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(typeof(Size), "192, 96")]
		public Size ItemTileSize
		{
			get
			{
				using (ItemMonitor.Lock(this))
				{
					return itemTileSize;
				}
			}
			set
			{
				using (ItemMonitor.Lock(this))
				{
					if (itemTileSize == value)
					{
						return;
					}
					itemTileSize = value;
				}
				if (ItemViewMode == ItemViewMode.Tile)
				{
					SafeInvalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(16)]
		public int ItemRowHeight
		{
			get
			{
				return itemRowHeight;
			}
			set
			{
				if (itemRowHeight != value)
				{
					itemRowHeight = value;
					if (ItemViewMode == ItemViewMode.Detail)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(40)]
		public int GroupHeaderHeight
		{
			get
			{
				return groupHeaderHeight;
			}
			set
			{
				if (groupHeaderHeight != value)
				{
					groupHeaderHeight = value;
					if (AreGroupsVisible)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool ShowGroupCount
		{
			get
			{
				return showGroupCount;
			}
			set
			{
				if (showGroupCount != value)
				{
					showGroupCount = value;
					if (AreGroupsVisible)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IViewableItem MarkerItem
		{
			get
			{
				return markerItem;
			}
			set
			{
				if (markerItem != value)
				{
					InvalidateMarker();
					markerItem = value;
					InvalidateMarker();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool MarkerVisible
		{
			get
			{
				return markerVisible;
			}
			set
			{
				if (markerVisible != value)
				{
					InvalidateMarker();
					markerVisible = value;
					InvalidateMarker();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(false)]
		public bool GroupHeaderTrueCount
		{
			get
			{
				return groupHeaderTrueCount;
			}
			set
			{
				if (groupHeaderTrueCount != value)
				{
					groupHeaderTrueCount = value;
					if (AreGroupsVisible)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ViewableItemCollection<IViewableItem> Items => items;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ItemViewColumnCollection<IColumn> Columns => columns;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IComparer<IViewableItem> ItemSorter
		{
			get
			{
				using (ItemMonitor.Lock(itemSorters))
				{
					return itemSorters[0];
				}
			}
			set
			{
				using (ItemMonitor.Lock(itemSorters))
				{
					if (itemSorters[0] == value)
					{
						return;
					}
					itemSorters.Remove(value);
					itemSorters.Insert(0, value);
					itemSorters.Trim(3);
				}
				SafeInvalidate(ItemViewInvalidateOptions.Full);
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IGrouper<IViewableItem> ItemGrouper
		{
			get
			{
				return itemGrouper;
			}
			set
			{
				if (itemGrouper != value)
				{
					itemGrouper = value;
					SafeInvalidate(ItemViewInvalidateOptions.Full);
					OnGroupDisplayChanged();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IGrouper<IViewableItem> ItemStacker
		{
			get
			{
				return itemStacker;
			}
			set
			{
				if (itemStacker != value)
				{
					itemStacker = value;
					IViewableItem focusedItem = FocusedItem;
					SafeInvalidate(ItemViewInvalidateOptions.Full);
					if (focusedItem != null)
					{
						EnsureItemVisible(focusedItem);
					}
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IComparer<IViewableItem> ItemStackSorter
		{
			get
			{
				return itemStackSorter;
			}
			set
			{
				if (itemStackSorter != value)
				{
					itemStackSorter = value;
					if (IsStacked)
					{
						SafeInvalidate(ItemViewInvalidateOptions.Full);
					}
				}
			}
		}

		[Browsable(false)]
		public ItemViewGroupsStatus GroupsStatus
		{
			get
			{
				return groupsStatus;
			}
			set
			{
				if (groupsStatus != value)
				{
					groupsStatus = value;
					if (AreGroupsVisible)
					{
						SafeInvalidate();
					}
				}
			}
		}

		[Browsable(false)]
		public bool IsHeaderVisible
		{
			get
			{
				if (showHeader && ItemViewMode == ItemViewMode.Detail)
				{
					return IsTopLayout;
				}
				return false;
			}
		}

		[Browsable(false)]
		public bool AreGroupsVisible
		{
			get
			{
				if (IsTopLayout && GroupDisplayEnabled)
				{
					return ItemGrouper != null;
				}
				return false;
			}
		}

		[Browsable(false)]
		public bool IsStacked
		{
			get
			{
				if (ItemViewMode != ItemViewMode.Detail && ItemStacker != null)
				{
					return StackDisplayEnabled;
				}
				return false;
			}
		}

		[Browsable(false)]
		public bool IsTopLayout
		{
			get
			{
				if (ItemViewLayout != 0)
				{
					return ItemViewMode == ItemViewMode.Detail;
				}
				return true;
			}
		}

		[Browsable(false)]
		public IEnumerable<IViewableItem> DisplayedItems
		{
			get
			{
				UpdatePositions(null);
				return displayedItems.Lock();
			}
		}

		public IEnumerable<string> DisplayedGroups
		{
			get
			{
				if (!AreGroupsVisible)
				{
					yield break;
				}
				foreach (GroupHeaderInformation item in displayedGroups.Lock())
				{
					yield return item.Caption;
				}
			}
		}

		[Browsable(false)]
		public int SelectedCount
		{
			get
			{
				using (ItemMonitor.Lock(selectedItems))
				{
					return IsStacked ? selectedItems.Sum((IViewableItem si) => GetStackCount(si)) : selectedItems.Count;
				}
			}
		}

		[Browsable(false)]
		public IEnumerable<IViewableItem> SelectedItems
		{
			get
			{
				using (ItemMonitor.Lock(selectedItems))
				{
					foreach (IViewableItem item in selectedItems)
					{
						if (IsStack(item))
						{
							IViewableItem[] stackItems = GetStackItems(item);
							for (int i = 0; i < stackItems.Length; i++)
							{
								yield return stackItems[i];
							}
						}
						else
						{
							yield return item;
						}
					}
				}
			}
		}

		[Browsable(false)]
		public ReadOnlyCollection<IViewableItem> VisibleItems
		{
			get
			{
				using (ItemMonitor.Lock(visibleItems))
				{
					return visibleItems.AsReadOnly();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IViewableItem FocusedItem
		{
			get
			{
				return itemStates.FindFirst(ItemViewStates.Focused);
			}
			set
			{
				if (value != FocusedItem)
				{
					SetItemState(FocusedItem, ItemViewStates.None);
					if (value != null)
					{
						SetItemState(value, GetItemState(value) | ItemViewStates.Focused);
					}
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int FocusedItemIndex
		{
			get
			{
				return items.IndexOf(FocusedItem);
			}
			set
			{
				try
				{
					IViewableItem focusedItem;
					using (ItemMonitor.Lock(items))
					{
						focusedItem = items[value];
					}
					FocusedItem = focusedItem;
				}
				catch
				{
					FocusedItem = null;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int FocusedItemDisplayIndex
		{
			get
			{
				using (ItemMonitor.Lock(displayedItems))
				{
					return displayedItems.IndexOf(FocusedItem);
				}
			}
		}

		public IColumn SortColumn
		{
			get
			{
				if (ItemSorter == null)
				{
					return null;
				}
				return Columns.FindBySorter(ItemSorter);
			}
			set
			{
				ItemSorter = value?.ColumnSorter;
			}
		}

		public IColumn[] SortColumns
		{
			get
			{
				return (from comp in itemSorters.Lock()
						select Columns.FindBySorter(comp)).TakeWhile((IColumn ic) => ic != null).ToArray();
			}
			set
			{
				using (ItemMonitor.Lock(itemSorters))
				{
					ItemSorter = null;
					for (int num = value.Length - 1; num >= 0; num--)
					{
						ItemSorter = value[num].ColumnSorter;
					}
				}
			}
		}

		public string SortColumnsKey
		{
			get
			{
				return CovnertColumnsToKey(SortColumns);
			}
			set
			{
				SortColumns = ConvertKeyToColumns(value).ToArray();
			}
		}

		public IColumn[] GroupColumns
		{
			get
			{
				return (from comp in ItemGrouper.GetGroupers()
						select Columns.FindByGrouper(comp)).TakeWhile((IColumn ic) => ic != null).ToArray();
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					ItemGrouper = null;
					return;
				}
				if (value.Length == 1)
				{
					ItemGrouper = value[0].ColumnGrouper;
					return;
				}
				ItemGrouper = new CompoundSingleGrouper<IViewableItem>(value.Select((IColumn c) => c.ColumnGrouper).ToArray());
			}
		}

		public string GroupColumnsKey
		{
			get
			{
				return CovnertColumnsToKey(GroupColumns);
			}
			set
			{
				GroupColumns = ConvertKeyToColumns(value).ToArray();
			}
		}

		public IColumn GroupColumn
		{
			get
			{
				IGrouper<IViewableItem> grouper = ((ItemGrouper is CompoundSingleGrouper<IViewableItem>) ? ((CompoundSingleGrouper<IViewableItem>)ItemGrouper).Groupers.FirstOrDefault() : ItemGrouper);
				if (grouper != null)
				{
					return Columns.FirstOrDefault((IColumn h) => h.ColumnGrouper == grouper);
				}
				return null;
			}
		}

		public IColumn StackColumn
		{
			get
			{
				IGrouper<IViewableItem> stacker = ((ItemStacker is CompoundSingleGrouper<IViewableItem>) ? ((CompoundSingleGrouper<IViewableItem>)ItemStacker).Groupers.FirstOrDefault() : ItemStacker);
				if (ItemStacker != null)
				{
					return Columns.FirstOrDefault((IColumn h) => h.ColumnGrouper == stacker);
				}
				return null;
			}
		}

		public IColumn[] StackColumns
		{
			get
			{
				return (from comp in ItemStacker.GetGroupers()
						select Columns.FindByGrouper(comp)).TakeWhile((IColumn ic) => ic != null).ToArray();
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					ItemStacker = null;
					return;
				}
				if (value.Length == 1)
				{
					ItemStacker = value[0].ColumnGrouper;
					return;
				}
				ItemStacker = new CompoundSingleGrouper<IViewableItem>(value.Select((IColumn c) => c.ColumnGrouper).ToArray());
			}
		}

		public string StackColumnsKey
		{
			get
			{
				return CovnertColumnsToKey(StackColumns);
			}
			set
			{
				StackColumns = ConvertKeyToColumns(value).ToArray();
			}
		}

		public IColumn StackSorterColum
		{
			get
			{
				if (ItemStackSorter != null)
				{
					return Columns.Find((IColumn h) => h.ColumnSorter == ItemStackSorter);
				}
				return null;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		private IEnumerable<ItemViewColumnInfo> ColumnHeaderConfiguration
		{
			get
			{
				return Columns.Select((IColumn ivch) => new ItemViewColumnInfo(ivch));
			}
			set
			{
				int num = 0;
				foreach (ItemViewColumnInfo item in value)
				{
					IColumn column = Columns.FindById(item.Id);
					if (column != null)
					{
						column.FormatId = item.FormatId;
						column.Visible = item.Visible;
						column.Width = item.Width;
						column.LastTimeVisible = (column.Visible ? DateTime.UtcNow : item.LastTimeVisible);
						columns.Remove(column);
						columns.Insert(num, column);
						num++;
					}
				}
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ItemViewConfig ViewConfig
		{
			get
			{
				return new ItemViewConfig
				{
					Columns = ColumnHeaderConfiguration.ToList(),
					Grouping = GroupDisplayEnabled,
					ItemSortOrder = ItemSortOrder,
					GroupSortOrder = GroupSortingOrder,
					GroupsStatus = GroupsStatus,
					SortKey = SortColumnsKey,
					GrouperId = GroupColumnsKey,
					StackerId = StackColumnsKey,
					ItemViewMode = ItemViewMode,
					ThumbnailSize = ItemThumbSize,
					TileSize = ItemTileSize,
					ItemRowHeight = ItemRowHeight
				};
			}
			set
			{
				if (value != null)
				{
					ColumnHeaderConfiguration = value.Columns;
					GroupDisplayEnabled = value.Grouping;
					ItemSortOrder = value.ItemSortOrder;
					GroupSortingOrder = value.GroupSortOrder;
					SortColumnsKey = value.SortKey;
					GroupColumnsKey = value.GrouperId;
					StackColumnsKey = (StackDisplayEnabled ? value.StackerId : null);
					ItemStackSorter = ((StackColumn == null) ? null : StackColumn.ColumnSorter);
					ItemViewMode = value.ItemViewMode;
					GroupsStatus = value.GroupsStatus ?? new ItemViewGroupsStatus();
					if (value.ThumbnailSize.Height >= 16 && value.ThumbnailSize.Width >= 16)
					{
						ItemThumbSize = value.ThumbnailSize;
					}
					if (value.TileSize.Width >= 16 && value.TileSize.Height >= 16)
					{
						ItemTileSize = value.TileSize;
					}
					if (value.ItemRowHeight >= 8)
					{
						ItemRowHeight = value.ItemRowHeight;
					}
				}
			}
		}

		[Browsable(false)]
		public int CurrentFontHeight => currentFontHeight;

		[Browsable(false)]
		public MouseButtons ActivateButton => activateButton;

		protected bool InUpdateBlock => updateBlockCount > 0;

		public override Rectangle ViewRectangle
		{
			get
			{
				Rectangle viewRectangle = base.ViewRectangle;
				if (IsHeaderVisible)
				{
					viewRectangle.Y += columnHeaderHeight;
					viewRectangle.Height -= columnHeaderHeight;
					if (viewRectangle.Height < 0)
					{
						viewRectangle.Height = 0;
					}
				}
				return viewRectangle;
			}
		}

		protected Rectangle ColumnHeadersRectangle
		{
			get
			{
				if (!IsHeaderVisible)
				{
					return Rectangle.Empty;
				}
				Rectangle displayRectangle = DisplayRectangle;
				displayRectangle.X += 8;
				displayRectangle.Height = columnHeaderHeight;
				displayRectangle.Width = GetColumnHeadersWidth();
				return displayRectangle;
			}
		}

		public ContextMenuStrip AutoViewContextMenuStrip => autoViewContextMenuStrip;

		public IViewableItem InplaceEditItem => currentInplaceEditItem;

		public int InplaceEditSubItem => currentInplaceEditSubItem; 
		#endregion

		public event EventHandler<StackEventArgs> ProcessStack;

		public event EventHandler ItemActivate;

		public event EventHandler SelectedIndexChanged;

		public event EventHandler ItemDisplayChanged;

		public event EventHandler GroupDisplayChanged;

		public event EventHandler<ItemViewColumnHeaderClickEventArgs> HeaderClick;

		public event ItemDragEventHandler ItemDrag;

		public event PaintEventHandler PostPaint;

		public ItemView()
		{
			SelectionMode = SelectionMode.MultiSimple;
			ItemsOwned = true;
			AutomaticViewMenu = true;
			AutomaticHeaderMenu = true;
			HeaderToolTips = true;
			LabelEdit = true;
			InitializeComponent();
			LocalizeUtility.Localize(tr, autoViewContextMenuStrip);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			items.Changed += ItemsChanged;
			columns.Changed += HeadersChanged;
			itemStates.StateChanged += ItemStatesStateChanged;
			miViewMode.DropDownItemClicked += ViewDropDownItemClicked;
			currentFontHeight = Font.Height;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				try
				{
					items.Clear();
					columns.Clear();
				}
				catch (Exception)
				{
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		public int GetGroupSizeFromCaption(string caption)
		{
			return displayedGroups.Lock().FirstOrDefault((GroupHeaderInformation g) => g.Caption == caption)?.ItemCount ?? 0;
		}

		public string CovnertColumnsToKey(IEnumerable<IColumn> cols)
		{
			if (cols == null || !cols.Any())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IColumn col in cols)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(col.Id);
			}
			return stringBuilder.ToString();
		}

		public IEnumerable<IColumn> ConvertKeyToColumns(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				yield break;
			}
			string[] array = key.Split(',');
			foreach (string s in array)
			{
				if (int.TryParse(s, out var result))
				{
					IColumn column = columns.FindById(result);
					if (column != null)
					{
						yield return column;
					}
				}
			}
		}

		public void ResetMouse()
		{
			ResetMouseEventArgs();
		}

		public ItemViewStates GetItemState(IViewableItem item)
		{
			if (item == null)
			{
				return ItemViewStates.None;
			}
			return itemStates[item];
		}

		public bool IsItemSelected(IViewableItem item)
		{
			return (GetItemState(item) & ItemViewStates.Selected) != 0;
		}

		public bool IsItemFocused(IViewableItem item)
		{
			return (GetItemState(item) & ItemViewStates.Focused) != 0;
		}

		public void SetItemState(IViewableItem item, ItemViewStates state, bool multiSelect)
		{
			if (item != null)
			{
				StateInfo stateInfo = new StateInfo(itemStates);
				if ((state & ItemViewStates.Selected) != 0 && !multiSelect)
				{
					stateInfo.Clear(ItemViewStates.Selected);
				}
				stateInfo[item] = state;
				if ((state & ItemViewStates.Focused) != 0 || !multiSelect)
				{
					stateInfo.Focus(item);
				}
				itemStates.Update(stateInfo);
			}
		}

		public void SetItemState(IViewableItem item, ItemViewStates state)
		{
			SetItemState(item, state, Multiselect);
		}

		public void SelectAll(bool selectionState)
		{
			if (!Multiselect)
			{
				return;
			}
			StateInfo stateInfo = new StateInfo(itemStates);
			foreach (IViewableItem item in displayedItems.Lock())
			{
				stateInfo.Set(item, ItemViewStates.Selected, selectionState);
			}
			itemStates.Update(stateInfo);
		}

		public void SelectAll()
		{
			SelectAll(selectionState: true);
		}

		public void SelectNone()
		{
			SelectAll(selectionState: false);
		}

		public void Select(IViewableItem item, bool selectionState)
		{
			StateInfo stateInfo = new StateInfo(itemStates);
			stateInfo.Set(item, ItemViewStates.Selected, selectionState);
			itemStates.Update(stateInfo);
		}

		public void Select(IViewableItem item)
		{
			Select(item, selectionState: true);
		}

		public void InvertSelection()
		{
			if (!Multiselect)
			{
				return;
			}
			StateInfo stateInfo = new StateInfo(itemStates);
			foreach (IViewableItem item in displayedItems.Lock())
			{
				stateInfo.Flip(item, ItemViewStates.Selected);
			}
			itemStates.Update(stateInfo);
		}

		private void SelectFromAnchorItem(StateInfo si, IViewableItem item, bool overideAnchor = false)
		{
			using (ItemMonitor.Lock(displayedItems))
			{
				if (displayedItems.Count == 0)
				{
					return;
				}
				if (overideAnchor)
				{
					anchorItem = null;
				}
				int a = displayedItems.IndexOf(anchorItem);
				int b = displayedItems.IndexOf(item);
				if (b == -1)
				{
					return;
				}
				if (a == -1)
				{
					a = displayedItems.IndexOf(FocusedItem);
					if (a == -1)
					{
						a = 0;
					}
					anchorItem = displayedItems[a];
				}
				if (a > b)
				{
					CloneUtility.Swap(ref a, ref b);
				}
				for (int i = a; i <= b; i++)
				{
					si.Set(displayedItems[i], ItemViewStates.Selected, on: true);
				}
			}
		}

		private void UpdateHotItemState(MouseButtons button, int x, int y)
		{
			StateInfo stateInfo = new StateInfo(itemStates);
			stateInfo.Clear(ItemViewStates.Hot);
			IViewableItem viewableItem = ItemHitTest(x, y);
			if (viewableItem != null && button == MouseButtons.None)
			{
				stateInfo.Set(viewableItem, ItemViewStates.Hot, on: true);
			}
			itemStates.Update(stateInfo);
		}

		public void InvokeActivate()
		{
			if (FocusedItem != null)
			{
				OnItemActivate();
			}
		}

		public void BeginUpdate()
		{
			using (ItemMonitor.Lock(this))
			{
				updateBlockCount++;
			}
		}

		public void EndUpdate()
		{
			using (ItemMonitor.Lock(this))
			{
				if (updateBlockCount > 0)
				{
					updateBlockCount--;
				}
			}
			if (pendingUpdate)
			{
				pendingUpdate = false;
				if (!base.InvokeRequired)
				{
					UpdateItems();
				}
				SafeInvalidate();
			}
		}

		private void SafeInvalidate(ItemViewInvalidateOptions options = ItemViewInvalidateOptions.Position, Rectangle bounds = default(Rectangle))
		{
			itemsResort |= options == ItemViewInvalidateOptions.Full;
			positionsInvalidated |= options != ItemViewInvalidateOptions.None;
			if (InUpdateBlock)
			{
				pendingUpdate = true;
			}
			else if (bounds.IsEmpty)
			{
				Invalidate();
			}
			else
			{
				Invalidate(bounds);
			}
		}

		public void UpdateItem(IViewableItem item, bool sizeChanged)
		{
			InvalidateItem(item, sizeChanged);
		}

		public void UpdateItems()
		{
			UpdatePositions(null);
		}

		public void EnsureGroupVisible(string caption)
		{
			if (AreGroupsVisible)
			{
				GroupHeaderInformation groupHeaderInformation = displayedGroups.Lock().FirstOrDefault((GroupHeaderInformation h) => h.Caption == caption);
				if (groupHeaderInformation != null)
				{
					UpdateItems();
					base.ScrollPosition = new Point(0, groupHeaderInformation.Bounds.Top);
				}
			}
		}

		public void EnsureItemVisible(IViewableItem item, int subItem = -1)
		{
			if (item == null)
			{
				return;
			}
			UpdateItems();
			Rectangle rectangle = Translate(ViewRectangle, fromClient: true);
			Rectangle itemBounds = GetItemBounds(item, subItem);
			if (itemBounds.IsEmpty)
			{
				return;
			}
			itemBounds.Inflate(GetItemBorderSize());
			if (rectangle.Contains(itemBounds))
			{
				return;
			}
			Point scrollPosition = base.ScrollPosition;
			if (itemBounds.Bottom > rectangle.Bottom)
			{
				scrollPosition.Y += itemBounds.Bottom - rectangle.Bottom;
			}
			else if (itemBounds.Top < rectangle.Top)
			{
				scrollPosition.Y -= rectangle.Top - itemBounds.Top;
			}
			if (ItemViewMode != ItemViewMode.Detail || subItem != -1)
			{
				if (itemBounds.Right > rectangle.Right)
				{
					scrollPosition.X += itemBounds.Right - rectangle.Right;
				}
				else if (itemBounds.Left < rectangle.Left)
				{
					scrollPosition.X -= rectangle.Left - itemBounds.Left;
				}
			}
			base.ScrollPosition = scrollPosition;
		}

		public void ScrollView(float linesOrColumns)
		{
			Point scrollPosition = base.ScrollPosition;
			if (IsTopLayout)
			{
				scrollPosition.Offset(0, (int)((float)LineHeight * linesOrColumns));
			}
			else
			{
				scrollPosition.Offset((int)((float)ColumnWidth * linesOrColumns), 0);
			}
			base.ScrollPosition = scrollPosition;
		}

		public int GetAutoHeaderSize(IColumn header)
		{
			int num = 0;
			int num2 = 0;
			ItemSizeInformation itemSizeInformation = new ItemSizeInformation();
			Graphics graphics2 = (itemSizeInformation.Graphics = CreateGraphics());
			using (graphics2)
			{
				itemSizeInformation.Header = header;
				itemSizeInformation.SubItem = Columns.IndexOf(header);
				foreach (IViewableItem item in displayedItems.Lock())
				{
					itemSizeInformation.Item = num2;
					itemSizeInformation.Width = header.Width;
					item.OnMeasure(itemSizeInformation);
					if (itemSizeInformation.Bounds.Width > num)
					{
						num = itemSizeInformation.Bounds.Width;
					}
					num2++;
				}
				return num;
			}
		}

		public void AutoSizeHeader(IColumn header)
		{
			header.Width = GetAutoHeaderSize(header);
		}

		public void AutoSizeHeaders(bool all)
		{
			foreach (IColumn column in Columns)
			{
				if (all || column.Visible)
				{
					AutoSizeHeader(column);
				}
			}
		}

		public void AutoFitHeaders(bool withAutosize)
		{
			if (withAutosize)
			{
				AutoSizeHeaders(all: false);
			}
			int columnHeadersWidth = GetColumnHeadersWidth();
			if (columnHeadersWidth == 0)
			{
				return;
			}
			float num = (float)DisplayRectangle.Width / (float)(columnHeadersWidth + 8);
			foreach (IColumn column in Columns)
			{
				column.Width = (int)((float)column.Width * num);
			}
		}

		public Bitmap GetDisplayBitmap(DrawItemViewOptions flags)
		{
			try
			{
				Size size = base.ClientRectangle.Size;
				Bitmap bitmap = new Bitmap(size.Width, size.Height);
				using (Graphics gr = Graphics.FromImage(bitmap))
				{
					DrawItems(gr, flags);
				}
				return bitmap;
			}
			catch
			{
				return null;
			}
		}

		public IBitmapCursor GetDragCursor(float alpha)
		{
			Bitmap displayBitmap = GetDisplayBitmap(DrawItemViewOptions.SelectedOnly);
			IBitmapCursor bitmapCursor = BitmapCursor.Create(displayBitmap, PointToClient(Cursor.Position));
			if (bitmapCursor == null)
			{
				displayBitmap.Dispose();
			}
			else
			{
				bitmapCursor.BitmapOwned = true;
				if (bitmapCursor.Bitmap != null)
				{
					bitmapCursor.Bitmap.ChangeAlpha(alpha);
				}
			}
			return bitmapCursor;
		}

		public void ExpandGroups(bool expand, string caption = null)
		{
			if (!AreGroupsVisible)
			{
				return;
			}
			bool flag = false;
			foreach (GroupHeaderInformation item in displayedGroups.Lock())
			{
				if (item.Collapsed != !expand && (string.IsNullOrEmpty(caption) || item.Caption == caption))
				{
					item.Collapsed = !expand;
					flag = true;
				}
			}
			if (flag)
			{
				SafeInvalidate();
			}
		}

		public void ToggleGroups()
		{
			try
			{
				ExpandGroups(displayedGroups.Lock().FirstOrDefault().Collapsed);
			}
			catch
			{
			}
		}

		protected virtual void OnItemDrag(ItemDragEventArgs itemDragEventArgs)
		{
			if (this.ItemDrag != null)
			{
				this.ItemDrag(this, itemDragEventArgs);
			}
		}

		protected virtual void OnItemActivate()
		{
			if (this.ItemActivate != null)
			{
				this.ItemActivate(this, EventArgs.Empty);
			}
		}

		protected virtual void OnSelectedIndexChanged()
		{
			if (this.SelectedIndexChanged != null)
			{
				this.SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnItemDisplayChanged()
		{
			if (this.ItemDisplayChanged != null)
			{
				this.ItemDisplayChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnGroupDisplayChanged()
		{
			if (this.GroupDisplayChanged != null)
			{
				this.GroupDisplayChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnHeaderClick(IColumn header)
		{
			if (this.HeaderClick != null)
			{
				this.HeaderClick(this, new ItemViewColumnHeaderClickEventArgs(header));
			}
			if (header.FormatTexts.Length != 0)
			{
				Rectangle dropDownBounds = HeaderControl.GetDropDownBounds(GetColumnHeaderRectangle(header));
				dropDownBounds.Offset(-base.ScrollPositionX, 0);
				if (dropDownBounds.Contains(pressedHeaderPoint))
				{
					ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
					for (int j = 0; j < header.FormatTexts.Length; j++)
					{
						int i = j;
						ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(header.FormatTexts[j], null, delegate
						{
							if (header.FormatId != i)
							{
								header.FormatId = i;
								Invalidate();
							}
						});
						toolStripMenuItem.Checked = header.FormatId == j;
						contextMenuStrip.Items.Add(toolStripMenuItem);
					}
					contextMenuStrip.Show(this, dropDownBounds.BottomRight(), ToolStripDropDownDirection.BelowLeft);
					return;
				}
			}
			if (header.ColumnSorter != null)
			{
				if (ItemSorter == header.ColumnSorter)
				{
					ItemSortOrder = FlipSortOrder(ItemSortOrder);
					return;
				}
				ItemSortOrder = SortOrder.Ascending;
				ItemSorter = header.ColumnSorter;
			}
		}

		protected virtual void OnProcessStack(StackInfo lsi)
		{
			if (this.ProcessStack != null)
			{
				this.ProcessStack(this, new StackEventArgs(lsi));
			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			currentFontHeight = Font.Height;
		}

		protected override void OnScroll()
		{
			base.OnScroll();
			Point position = Cursor.Position;
			position = PointToClient(position);
			UpdateHotItemState(Control.MouseButtons, position.X, position.Y);
			UpdateSelection(position.X, position.Y);
			ExitEdit(currentInplaceEditControl);
		}

		private void ItemsChanged(object sender, SmartListChangedEventArgs<IViewableItem> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				e.Item.View = this;
				break;
			case SmartListAction.Remove:
				e.Item.View = null;
				using (ItemMonitor.Lock(itemStates))
				{
					itemStates.Set(e.Item, ItemViewStates.All, on: false);
				}
				if (ItemsOwned)
				{
					(e.Item as IDisposable)?.Dispose();
				}
				break;
			}
			SafeInvalidate(ItemViewInvalidateOptions.Full);
		}

		private void HeadersChanged(object sender, SmartListChangedEventArgs<IColumn> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				e.Item.View = this;
				e.Item.PropertyChanged += HeaderPropertyChanged;
				break;
			case SmartListAction.Remove:
				e.Item.View = null;
				e.Item.PropertyChanged -= HeaderPropertyChanged;
				break;
			}
			if (ItemViewMode == ItemViewMode.Detail)
			{
				SafeInvalidate();
			}
		}

		private void HeaderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (ItemViewMode == ItemViewMode.Detail)
			{
				SafeInvalidate();
			}
		}

		private void ItemStatesStateChanged(object sender, StateChangedEventArgs e)
		{
			InvalidateItem(e.Item, withSize: false);
			if (!pendingSelectedIndexChanged && (e.NewState & ItemViewStates.Selected) != (e.OldState & ItemViewStates.Selected))
			{
				pendingSelectedIndexChanged = true;
			}
		}

		private int GetColumnHeadersWidth()
		{
			return columns.Where((IColumn ivch) => ivch.Visible).Sum((IColumn ivch) => ivch.Width);
		}

		public IColumn GetExpandedColumn()
		{
			if (ItemViewMode != ItemViewMode.Detail || string.IsNullOrEmpty(ExpandedDetailColumnName) || !AreGroupsVisible)
			{
				return null;
			}
			IColumn column = columns.FirstOrDefault((IColumn ch) => ch.Visible);
			if (column != null && column.Name == ExpandedDetailColumnName)
			{
				return column;
			}
			IColumn column2 = columns.LastOrDefault((IColumn ch) => ch.Visible);
			if (column2 != null && column2.Name == ExpandedDetailColumnName)
			{
				return column2;
			}
			return null;
		}

		public int GetExpandedColumnMinimumHeight(int width)
		{
			if (expandedDetailColumnMinimumHeight < 0)
			{
				return width * -ExpandedDetailColumnMinimumHeight / 100;
			}
			return ExpandedDetailColumnMinimumHeight;
		}

		private Rectangle GetColumnHeaderRectangle(IColumn header)
		{
			if (header.Visible)
			{
				int num = 8;
				foreach (IColumn column in columns)
				{
					if (column == header)
					{
						Rectangle displayRectangle = DisplayRectangle;
						return new Rectangle(displayRectangle.X + num, displayRectangle.Y, column.Width, ColumnHeaderHeight);
					}
					if (column.Visible)
					{
						num += column.Width;
					}
				}
			}
			return Rectangle.Empty;
		}

		private void MoveColumnHeader(IColumn columnHeader, int position)
		{
			if (columnHeader == null || !columnHeader.Visible)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (num < columns.Count - 1)
			{
				if (position >= num2 && position < num2 + columnHeader.Width)
				{
					if (columns[num] != columnHeader)
					{
						columns.Remove(columnHeader);
						columns.Insert(num, columnHeader);
					}
					break;
				}
				if (columns[num3] == columnHeader)
				{
					num3++;
				}
				if (columns[num3].Visible)
				{
					num2 += columns[num3].Width;
				}
				num++;
				num3++;
			}
		}

		private void InvalidateMarker()
		{
			if (MarkerVisible && Items.Count != 0 && MarkerItem != null)
			{
				Rectangle markerBounds = GetMarkerBounds(MarkerItem, 2);
				if (!markerBounds.IsEmpty)
				{
					markerBounds.Inflate(1, 1);
					SafeInvalidate(ItemViewInvalidateOptions.None, Translate(markerBounds, fromClient: false));
				}
			}
		}

		private Rectangle GetMarkerBounds(IViewableItem item, int width)
		{
			Rectangle itemBounds = GetItemBounds(item);
			switch (ItemViewMode)
			{
			case ItemViewMode.Thumbnail:
			case ItemViewMode.Tile:
				if (ItemViewLayout != 0)
				{
					return new Rectangle(itemBounds.Left, itemBounds.Top - width / 2, itemBounds.Width, width);
				}
				return new Rectangle(itemBounds.Left - width / 2, itemBounds.Top, width, itemBounds.Height);
			case ItemViewMode.Detail:
				return new Rectangle(itemBounds.Left, itemBounds.Top - width / 2, itemBounds.Width, width);
			default:
				return Rectangle.Empty;
			}
		}

		private Size GetItemBorderSize()
		{
			if (ItemViewMode == ItemViewMode.Detail)
			{
				return Size.Empty;
			}
			return ItemPadding;
		}

		private Size GetDefaultItemSize(int clientWidth)
		{
			switch (ItemViewMode)
			{
			case ItemViewMode.Thumbnail:
				return itemThumbSize;
			case ItemViewMode.Tile:
				return itemTileSize;
			default:
				return new Size(clientWidth, itemRowHeight);
			}
		}

		public IViewableItem GetFirstGroupItem(string group)
		{
			if (displayedGroups == null)
			{
				return null;
			}
			foreach (GroupHeaderInformation displayedGroup in displayedGroups)
			{
				if (displayedGroup.Caption == group)
				{
					return displayedGroup.Items.FirstOrDefault();
				}
			}
			return null;
		}

		private ItemInformation GetItemInformation(IViewableItem item)
		{
			if (item == null)
			{
				return null;
			}
			if (!IsStacked || IsStack(item))
			{
				using (ItemMonitor.Lock(itemInfos))
				{
					itemInfos.TryGetValue(item, out var value);
					return value;
				}
			}
			using (ItemMonitor.Lock(this.stackInfo))
			{
				foreach (IViewableItem key in this.stackInfo.Keys)
				{
					StackInfo stackInfo = this.stackInfo[key];
					if (stackInfo.Items.Contains(item))
					{
						return GetItemInformation(key);
					}
				}
				return null;
			}
		}

		protected Rectangle GetItemBounds(IViewableItem item, int subItem)
		{
			Rectangle itemBounds = GetItemBounds(item);
			if (itemBounds.IsEmpty || ItemViewMode != ItemViewMode.Detail || subItem < 0 || subItem > columns.Count)
			{
				return itemBounds;
			}
			IColumn header = columns[subItem];
			Rectangle columnHeaderRectangle = GetColumnHeaderRectangle(header);
			itemBounds.X = columnHeaderRectangle.X;
			itemBounds.Width = columnHeaderRectangle.Width;
			return itemBounds;
		}

		public Rectangle GetItemBounds(IViewableItem item)
		{
			return GetItemInformation(item)?.Bounds ?? Rectangle.Empty;
		}

		public bool IsStack(IViewableItem item)
		{
			if (item == null || !IsStacked)
			{
				return false;
			}
			using (ItemMonitor.Lock(stackInfo))
			{
				return stackInfo.ContainsKey(item);
			}
		}

		public IGroupInfo GetStackGroupInfo(IViewableItem item)
		{
			if (item == null)
			{
				return null;
			}
			using (ItemMonitor.Lock(stackInfo))
			{
				if (!stackInfo.TryGetValue(item, out var value))
				{
					return null;
				}
				return value.GroupInfo;
			}
		}

		public string GetStackCaption(IViewableItem item)
		{
			if (item == null)
			{
				return null;
			}
			using (ItemMonitor.Lock(stackInfo))
			{
				StackInfo value;
				return (!stackInfo.TryGetValue(item, out value)) ? item.Text : value.Text;
			}
		}

		public object GetStackKey(IViewableItem item)
		{
			if (item == null)
			{
				return null;
			}
			using (ItemMonitor.Lock(stackInfo))
			{
				StackInfo value;
				return (!stackInfo.TryGetValue(item, out value)) ? null : value.Key;
			}
		}

		public IViewableItem[] GetStackItems(IViewableItem item)
		{
			if (item == null)
			{
				return null;
			}
			using (ItemMonitor.Lock(stackInfo))
			{
				StackInfo value;
				return stackInfo.TryGetValue(item, out value) ? value.Items.ToArray() : new IViewableItem[1]
				{
					item
				};
			}
		}

		public int GetStackCount(IViewableItem item)
		{
			if (item == null)
			{
				return 0;
			}
			using (ItemMonitor.Lock(stackInfo))
			{
				StackInfo value;
				return (!stackInfo.TryGetValue(item, out value)) ? 1 : value.Items.Count;
			}
		}

		private Rectangle CalcItemPositions(Graphics gr, bool withSort)
		{
			bool areGroupsVisible = AreGroupsVisible;
			bool flag;
			using (ItemMonitor.Lock(displayedItems))
			{
				flag = displayedItems == null || displayedItems.Count == 0;
			}
			List<IViewableItem> viewableItems;
			List<GroupHeaderInformation> grpHeaders;
			if (withSort || flag)
			{
				viewableItems = new List<IViewableItem>(this.items.Lock());
				grpHeaders = new List<GroupHeaderInformation>();
				if (areGroupsVisible)
				{
					IGrouper<IViewableItem> grouper = ItemGrouper;
					if (IsStacked && ItemStacker.First() == ItemGrouper.First())
					{
						grouper = new AlphabetGrouper(grouper);
					}
					GroupManager<IViewableItem> groupManager = new GroupManager<IViewableItem>(grouper, viewableItems);
					IEnumerable<GroupContainer<IViewableItem>> enumerable = groupManager.GetGroups();
					if (GroupSortingOrder != 0)
					{
						enumerable = enumerable.OrderBy((GroupContainer<IViewableItem> a) => a);
					}
					if (GroupSortingOrder == SortOrder.Descending)
					{
						enumerable = enumerable.Reverse();
					}
					foreach (GroupContainer<IViewableItem> item in enumerable)
					{
						grpHeaders.Add(new GroupHeaderInformation(item.Caption, item.Items, GroupsStatus.IsCollapsed(item.Caption)));
						viewableItems.AddRange(item.Items);
					}
				}
				else
				{
					grpHeaders.Add(new GroupHeaderInformation(null, new List<IViewableItem>(viewableItems)));
				}
				if (IsStacked)
				{
					Dictionary<IViewableItem, StackInfo> dictionary = new Dictionary<IViewableItem, StackInfo>();
					foreach (GroupHeaderInformation item2 in grpHeaders)
					{
						GroupManager<IViewableItem> groupManager2 = new GroupManager<IViewableItem>(ItemStacker, item2.Items);
						item2.Items.Clear();
						item2.ItemCount = 0;
						foreach (GroupContainer<IViewableItem> group in groupManager2.GetGroups())
						{
							if (ItemStackSorter != null)
							{
								group.Items.Sort(ItemStackSorter);
							}
							StackInfo stackInfo = new StackInfo(group);
							OnProcessStack(stackInfo);
							IViewableItem key = group.Items[0];
							dictionary[key] = stackInfo;
							item2.Items.Add(group.Items[0]);
							if (GroupHeaderTrueCount)
							{
								item2.ItemCount += group.Items.Count;
							}
							else
							{
								item2.ItemCount++;
							}
						}
					}
					using (ItemMonitor.Lock(this.stackInfo))
					{
						this.stackInfo = dictionary;
					}
				}
				else
				{
					using (ItemMonitor.Lock(this.stackInfo))
					{
						this.stackInfo.Clear();
					}
				}
				IComparer<IViewableItem> comparer = null;
				using (ItemMonitor.Lock(itemSorters))
				{
					if (itemSorters[0] != null)
					{
						comparer = new ChainedComparer<IViewableItem>(itemSorters);
					}
				}
				if (comparer != null && ItemSortOrder == SortOrder.Descending)
				{
					comparer = comparer.Reverse();
				}
				if (comparer != null)
				{
					try
					{
						grpHeaders.ParallelForEach((GroupHeaderInformation ghi) =>
                        {
                            ghi.Items.Sort(comparer);
                        });
					}
					catch
					{
					}
				}
				viewableItems.Clear();
				foreach (GroupHeaderInformation item3 in grpHeaders)
				{
					viewableItems.AddRange(item3.Items);
				}
			}
			else
			{
				using (ItemMonitor.Lock(displayedItems))
				{
					viewableItems = displayedItems;
				}
				using (ItemMonitor.Lock(displayedGroups))
				{
					grpHeaders = displayedGroups;
				}
			}
			if (grpHeaders.Count > 0)
			{
				groupsStatus = new ItemViewGroupsStatus(grpHeaders);
			}
			Dictionary<IViewableItem, ItemInformation> dictionary2 = new Dictionary<IViewableItem, ItemInformation>();
			int width = 0;
			int height = 0;
			int col = 0;
			int row = 0;
			int itemIndex = 0;
			IColumn expandedColumn = GetExpandedColumn();
			IColumn column = columns.FirstOrDefault((IColumn c) => c.Visible);
			Rectangle rectangle = ((expandedColumn != null) ? GetColumnHeaderRectangle(expandedColumn) : Rectangle.Empty);
			Rectangle viewRectangle = ViewRectangle;
			Size itemBorderSize = GetItemBorderSize();
			if (ItemViewMode == ItemViewMode.Detail)
			{
				viewRectangle.Width = GetColumnHeadersWidth();
				viewRectangle.Y -= ViewRectangle.Y - DisplayRectangle.Y;
			}
			Point empty = Point.Empty;
			Rectangle rectangle2 = new Rectangle(empty, Size.Empty);
			for (int i = 0; i < grpHeaders.Count; i++)
			{
				GroupHeaderInformation groupHeaderInformation = grpHeaders[i];
				if (areGroupsVisible && !string.IsNullOrEmpty(groupHeaderInformation.Caption))
				{
					if (empty.X != 0)
					{
						empty.Y += itemBorderSize.Height * 2 + height;
						empty.X = 0;
						row++;
						col = 0;
					}
					groupHeaderInformation.Bounds = new Rectangle(empty.X, empty.Y, viewRectangle.Width, GroupHeaderHeight);
					groupHeaderInformation.ExpandedColumnBounds = Rectangle.Empty;
					empty.Y += GroupHeaderHeight;
					rectangle2 = Rectangle.Union(rectangle2, groupHeaderInformation.Bounds);
				}
				if (groupHeaderInformation.Collapsed)
				{
					continue;
				}
				int startIndex = 0;
				int grpIndex = 0;
				foreach (IViewableItem item4 in groupHeaderInformation.Items)
				{
					if (item4.View == null)
					{
						continue;
					}
					Size size = GetDefaultItemSize(viewRectangle.Width);
					int offset = 0;
					switch (ItemViewMode)
					{
					case ItemViewMode.Detail:
						offset = 8;
						break;
					case ItemViewMode.Thumbnail:
					case ItemViewMode.Tile:
					{
						ItemSizeInformation itemSizeInformation = new ItemSizeInformation
						{
							Graphics = gr,
							Size = size,
							DisplayType = ItemViewMode,
							Item = itemIndex,
							GroupItem = grpIndex,
							SubItem = -1,
							Header = null
						};
						item4.OnMeasure(itemSizeInformation);
						size = itemSizeInformation.Bounds.Size;
						break;
					}
					}
					if (size.Width > width)
					{
						width = size.Width;
					}
					if (size.Height > height)
					{
						height = size.Height;
					}
					if (IsTopLayout)
					{
						if (empty.X + 2 * itemBorderSize.Width + size.Width >= viewRectangle.Width && col > 0)
						{
							RealignItems(groupHeaderInformation.Items, dictionary2, startIndex, col, viewRectangle.Left, viewRectangle.Right, HorizontalItemAlignment);
							row++;
							startIndex = grpIndex;
							col = 0;
							empty.X = 0;
							empty.Y += itemBorderSize.Height * 2 + height;
							height = size.Height;
						}
					}
					else if (empty.Y + 2 * itemBorderSize.Height + size.Height >= viewRectangle.Height && row > 0)
					{
						col++;
						row = 0;
						empty.Y = 0;
						empty.X += itemBorderSize.Width * 2 + width;
						width = size.Width;
					}
					Rectangle rectangle3 = new Rectangle(empty.X + itemBorderSize.Width + offset, empty.Y + itemBorderSize.Height, size.Width, size.Height);
					rectangle3.Offset(viewRectangle.Location);
					if (expandedColumn != null)
					{
						groupHeaderInformation.ExpandedColumnBounds = RectangleExtensions.Union(b: new Rectangle(rectangle.X, rectangle3.Y, rectangle.Width, rectangle3.Height), a: groupHeaderInformation.ExpandedColumnBounds);
						rectangle3.Width -= rectangle.Width;
						if (expandedColumn == column)
						{
							rectangle3.X += rectangle.Width;
						}
					}
					dictionary2[item4] = new ItemInformation(item4, rectangle3, col, row, groupHeaderInformation);
					rectangle2 = Rectangle.Union(rectangle2, rectangle3);
					if (IsTopLayout)
					{
						empty.X += itemBorderSize.Width * 2 + size.Width;
						col++;
					}
					else
					{
						empty.Y += itemBorderSize.Height * 2 + size.Height;
						row++;
					}
					grpIndex++;
					itemIndex++;
				}
				if (IsTopLayout)
				{
					RealignItems(groupHeaderInformation.Items, dictionary2, startIndex, col, viewRectangle.Left, viewRectangle.Right, HorizontalItemAlignment);
				}
				if (expandedColumn != null)
				{
					Rectangle expandedColumnBounds = groupHeaderInformation.ExpandedColumnBounds;
					int expandedColumnMinimumHeight = GetExpandedColumnMinimumHeight(groupHeaderInformation.ExpandedColumnBounds.Width);
					if (groupHeaderInformation.ExpandedColumnBounds.Height < expandedColumnMinimumHeight)
					{
						empty.Y += expandedColumnMinimumHeight - groupHeaderInformation.ExpandedColumnBounds.Height;
						expandedColumnBounds.Height = expandedColumnMinimumHeight;
						groupHeaderInformation.ExpandedColumnBounds = expandedColumnBounds;
						rectangle2 = Rectangle.Union(rectangle2, expandedColumnBounds);
					}
				}
			}
			using (ItemMonitor.Lock(itemInfos))
			{
				itemInfos = dictionary2;
			}
			using (ItemMonitor.Lock(displayedItems))
			{
				displayedItems = viewableItems;
			}
			using (ItemMonitor.Lock(displayedGroups))
			{
				displayedGroups = grpHeaders;
				return rectangle2;
			}
		}

		private static void RealignItems(IList<IViewableItem> list, IDictionary<IViewableItem, ItemInformation> infos, int startIndex, int count, int left, int right, HorizontalAlignment horizontalAlignment)
		{
			if (list.Count == 0 || horizontalAlignment == HorizontalAlignment.Left)
			{
				return;
			}
			int num = 0;
			switch (horizontalAlignment)
			{
			case HorizontalAlignment.Right:
				num = right - infos[list[startIndex + count - 1]].Bounds.Right;
				break;
			case HorizontalAlignment.Center:
			{
				Rectangle a = infos[list[startIndex]].Bounds;
				for (int i = 1; i < count; i++)
				{
					a = Rectangle.Union(a, infos[list[startIndex + i]].Bounds);
				}
				num = (right - left - a.Width) / 2;
				break;
			}
			}
			if (num != 0)
			{
				for (int j = 0; j < count; j++)
				{
					infos[list[startIndex + j]].Offset(num, 0);
				}
			}
		}

		public IViewableItem[] GetColumnRowItems(int column, int row)
		{
			using (ItemMonitor.Lock(itemInfos))
			{
				return (from info in itemInfos.Values
					where !info.Bounds.IsEmpty && (info.Row == row || row == -1) && (info.Column == column || column == -1)
					select info.Item).ToArray();
			}
		}

		protected bool UpdatePositions(Graphics gr)
		{
			if (positionsInvalidated && !base.IsDisposed)
			{
				bool withSort = itemsResort;
				positionsInvalidated = false;
				itemsResort = false;
				bool flag = gr == null;
				try
				{
					if (flag)
					{
						gr = CreateGraphics();
					}
					Size size = CalcItemPositions(gr, withSort).Size;
					if (size == base.VirtualSize)
					{
						return false;
					}
					base.VirtualSize = size;
					return true;
				}
				finally
				{
					if (flag)
					{
						gr?.Dispose();
					}
				}
			}
			return false;
		}

		public IColumn ColumnHeaderHitTest(int x, int y)
		{
			if (!IsHeaderVisible)
			{
				return null;
			}
			if (y > ColumnHeaderHeight)
			{
				return null;
			}
			x += base.ScrollPosition.X;
			return columns.FirstOrDefault((IColumn ivch) => GetColumnHeaderRectangle(ivch).Contains(x, y));
		}

		public int ColumnHeaderSeparatorHitTest(int x, int y)
		{
			if (!IsHeaderVisible)
			{
				return -1;
			}
			if (y > ColumnHeaderHeight)
			{
				return -1;
			}
			x += base.ScrollPosition.X;
			for (int num = columns.Count - 1; num >= 0; num--)
			{
				IColumn column = columns[num];
				if (column.Visible)
				{
					Rectangle columnHeaderRectangle = GetColumnHeaderRectangle(columns[num]);
					if (x >= columnHeaderRectangle.Right - 2 && x <= columnHeaderRectangle.Right + 2 && y >= columnHeaderRectangle.Top && y < columnHeaderRectangle.Bottom)
					{
						return num;
					}
				}
			}
			return -1;
		}

		public IViewableItem ItemHitTest(Point pt)
		{
			return ItemHitTest(pt.X, pt.Y);
		}

		public IViewableItem ItemHitTest(int x, int y)
		{
			if (!ViewRectangle.Contains(x, y))
			{
				return null;
			}
			Point test = Translate(new Point(x, y), fromClient: true);
			try
			{
				return visibleItems.Lock().FirstOrDefault((IViewableItem item) => ItemIntersects(item, test));
			}
			catch
			{
			}
			return null;
		}

		public IViewableItem ItemHitTest(int x, int y, out int subItem)
		{
			subItem = -1;
			IViewableItem viewableItem = ItemHitTest(x, y);
			if (viewableItem != null && ItemViewMode == ItemViewMode.Detail)
			{
				Point pt = Translate(new Point(x, y), fromClient: true);
				using (ItemMonitor.Lock(columns.SyncRoot))
				{
					for (int i = 0; i < columns.Count; i++)
					{
						if (GetItemBounds(viewableItem, i).Contains(pt))
						{
							subItem = i;
							return viewableItem;
						}
					}
					return viewableItem;
				}
			}
			return viewableItem;
		}

		public bool ItemIntersects(IViewableItem item, Point pt)
		{
			Rectangle itemBounds = GetItemBounds(item);
			if (!itemBounds.Contains(pt))
			{
				return false;
			}
			IViewableItemHitTest viewableItemHitTest = item as IViewableItemHitTest;
			if (viewableItemHitTest == null)
			{
				return true;
			}
			pt.Offset(-itemBounds.X, -itemBounds.Y);
			return viewableItemHitTest.Contains(pt);
		}

		public bool ItemIntersects(IViewableItem item, Rectangle rc)
		{
			Rectangle itemBounds = GetItemBounds(item);
			if (!itemBounds.IntersectsWith(rc))
			{
				return false;
			}
			IViewableItemHitTest viewableItemHitTest = item as IViewableItemHitTest;
			if (viewableItemHitTest == null)
			{
				return true;
			}
			rc.Offset(-itemBounds.X, -itemBounds.Y);
			return viewableItemHitTest.IntersectsWith(rc);
		}

		public bool InvokeItemClick(IViewableItem item, Point pt)
		{
			Rectangle itemBounds = GetItemBounds(item);
			pt = Translate(pt, fromClient: true);
			if (!itemBounds.Contains(pt))
			{
				return false;
			}
			pt.Offset(-itemBounds.X, -itemBounds.Y);
			return item.OnClick(pt);
		}

		protected void InvalidateItem(IViewableItem item, bool withSize)
		{
			if (withSize)
			{
				SafeInvalidate();
				return;
			}
			Rectangle itemBounds = GetItemBounds(item);
			itemBounds.Inflate(GetItemBorderSize());
			itemBounds.Inflate(1, 1);
			SafeInvalidate(ItemViewInvalidateOptions.None, Translate(itemBounds, fromClient: false));
			ItemInformation itemInformation = GetItemInformation(item);
			if (itemInformation != null && itemInformation.Group != null)
			{
				itemBounds = itemInformation.Group.ExpandedColumnBounds;
				if (!itemBounds.IsEmpty)
				{
					SafeInvalidate(ItemViewInvalidateOptions.None, Translate(itemBounds, fromClient: false));
				}
			}
		}

		protected virtual void OnDrawColumnHeaders(Graphics gr)
		{
			foreach (ItemViewColumn column in columns)
			{
				if (!column.Visible)
				{
					continue;
				}
				try
				{
					Rectangle columnHeaderRectangle = GetColumnHeaderRectangle(column);
					using (gr.SaveState())
					{
						gr.IntersectClip(columnHeaderRectangle);
						gr.TranslateTransform(columnHeaderRectangle.X, columnHeaderRectangle.Y);
						columnHeaderRectangle.Offset(-columnHeaderRectangle.X, -columnHeaderRectangle.Y);
						if (pressedHeader == column)
						{
							column.DrawHeader(gr, columnHeaderRectangle, HeaderState.Pressed);
						}
						else if (hotHeader == column)
						{
							column.DrawHeader(gr, columnHeaderRectangle, HeaderState.Hot);
						}
						else
						{
							column.DrawHeader(gr, columnHeaderRectangle, (SortColumn == column) ? HeaderState.Active : HeaderState.Normal);
						}
					}
				}
				catch
				{
				}
			}
		}

		protected virtual void OnDrawGroupHeader(Graphics graphics, GroupHeaderInformation groupHeaderInformation)
		{
			Rectangle bounds = groupHeaderInformation.Bounds;
			string text = (ShowGroupCount ? $"{groupHeaderInformation.Caption} ({groupHeaderInformation.ItemCount})" : groupHeaderInformation.Caption);
			Font font = FC.Get(Font, Font.Size * 1.15f);
			Color darkBlue = Color.DarkBlue;
			Color controlDark = SystemColors.ControlDark;
			Size size = graphics.MeasureString(text, font).ToSize();
			Bitmap bitmap = (groupHeaderInformation.Collapsed ? groupCollapsedImage : groupExpandedImage);
			int num = size.Width;
			int height = size.Height;
			int num2 = (bounds.Height - height) / 2 + 2;
			int y = num2 + (height - 1) / 2;
			int num3 = 2;
			if (bitmap != null)
			{
				num += bitmap.Width + 2;
			}
			if (HorizontalItemAlignment == HorizontalAlignment.Center)
			{
				num3 += (bounds.Width - num) / 2;
			}
			int num4 = num3;
			if (bitmap != null)
			{
				int y2 = num2 + (height - bitmap.Height) / 2;
				Rectangle rectangle = new Rectangle(num3, y2, bitmap.Width, bitmap.Height);
				graphics.DrawImage(bitmap, rectangle);
				rectangle.Offset(bounds.Location);
				groupHeaderInformation.ArrowBounds = rectangle;
				num3 += bitmap.Width;
			}
			else
			{
				groupHeaderInformation.ArrowBounds = Rectangle.Empty;
			}
			using (Brush brush = new SolidBrush(darkBlue))
			{
				graphics.DrawString(text, font, brush, num3, num2);
				groupHeaderInformation.TextBounds = new Rectangle(bounds.X + num3, bounds.Y + num2, size.Width, size.Height);
			}
			int num5 = num4 + num + 5;
			Rectangle rect = new Rectangle(num5, y, bounds.Width - num5 - 5, 1);
			if (rect.Width > 5)
			{
				using (Brush brush2 = new SolidBrush(controlDark))
				{
					graphics.FillRectangle(brush2, rect);
				}
			}
			rect = new Rectangle(5, y, num4 - 10, 1);
			if (rect.Width > 5)
			{
				using (Brush brush3 = new SolidBrush(controlDark))
				{
					graphics.FillRectangle(brush3, rect);
				}
			}
		}

		protected virtual void OnDrawItemSelection(Graphics gr, Rectangle rc, ItemViewStates drawState)
		{
			Rectangle rc2 = rc;
			rc2.Inflate(-1, -1);
			gr.DrawStyledRectangle(rc2, StyledRenderer.GetAlphaStyle(drawState.HasFlag(ItemViewStates.Selected), drawState.HasFlag(ItemViewStates.Hot), drawState.HasFlag(ItemViewStates.Focused)), Focused ? SystemColors.Highlight : Color.Gray);
		}

		protected virtual void OnDrawItemStates(Graphics gr, IViewableItem item, Rectangle rc, ItemViewStates drawState)
		{
			OnDrawItemSelection(gr, rc, drawState & ~item.GetOwnerDrawnStates(ItemViewMode));
		}

		protected void DrawBackground(Graphics gr, DrawItemViewOptions drawItemsFlags = DrawItemViewOptions.Default)
		{
			if ((drawItemsFlags & DrawItemViewOptions.Background) != 0)
			{
				gr.Clear(BackColor);
			}
			if ((drawItemsFlags & DrawItemViewOptions.BackgroundImage) != 0 && BackgroundImage != null)
			{
				Rectangle rectangle = new Rectangle(0, 0, BackgroundImage.Width, BackgroundImage.Height);
				gr.DrawImage(BackgroundImage, rectangle.Align(DisplayRectangle, BackgroundImageAlignment), rectangle, GraphicsUnit.Pixel);
			}
		}

		protected virtual void DrawMarker(Graphics gr, Rectangle bounds)
		{
			Color color = Color.FromArgb(128, SystemColors.ControlDarkDark);
			using (Brush brush = new SolidBrush(color))
			{
				gr.FillRectangle(brush, bounds);
				gr.DrawRectangle(Pens.Black, bounds);
			}
		}

		protected void DrawItems(Graphics gr, DrawItemViewOptions drawItemsFlags = DrawItemViewOptions.Default)
		{
			Rectangle viewRectangle = ViewRectangle;
			using (gr.SaveState())
			{
				gr.TranslateTransform(-base.ScrollPosition.X, 0f);
				viewRectangle.Offset(base.ScrollPosition.X, 0);
				if (IsHeaderVisible && (drawItemsFlags & DrawItemViewOptions.ColumnHeaders) != 0)
				{
					OnDrawColumnHeaders(gr);
					gr.IntersectClip(viewRectangle);
				}
				gr.TranslateTransform(0f, -base.ScrollPosition.Y + viewRectangle.Top);
				viewRectangle.Offset(0, base.ScrollPosition.Y - viewRectangle.Top);
				using (ItemMonitor.Lock(visibleItems))
				{
					using (ItemMonitor.Lock(selectedItems))
					{
						using (ItemMonitor.Lock(displayedGroups))
						{
							using (ItemMonitor.Lock(items.SyncRoot))
							{
								visibleItems.Clear();
								selectedItems.Clear();
								ItemDrawInformation itemDrawInformation = new ItemDrawInformation
								{
									Item = -1,
									Graphics = gr,
									DisplayType = ItemViewMode
								};
								IColumn expandedColumn = GetExpandedColumn();
								IViewableItem focusedItem = FocusedItem;
								ItemInformation itemInformation = GetItemInformation(focusedItem);
								foreach (GroupHeaderInformation displayedGroup in displayedGroups)
								{
									if (AreGroupsVisible && !displayedGroup.Bounds.IsEmpty && gr.IsVisible(displayedGroup.Bounds) && drawItemsFlags.HasFlag(DrawItemViewOptions.GroupHeaders))
									{
										using (gr.SaveState())
										{
											Rectangle bounds = displayedGroup.Bounds;
											gr.IntersectClip(bounds);
											gr.TranslateTransform(bounds.X, bounds.Y);
											bounds.Offset(-bounds.X, -bounds.Y);
											OnDrawGroupHeader(gr, displayedGroup);
										}
									}
									if (expandedColumn != null && gr.IsVisible(displayedGroup.ExpandedColumnBounds) && drawItemsFlags.HasFlag(DrawItemViewOptions.GroupHeaders))
									{
										using (gr.SaveState())
										{
											Rectangle expandedColumnBounds = displayedGroup.ExpandedColumnBounds;
											gr.IntersectClip(expandedColumnBounds);
											gr.TranslateTransform(expandedColumnBounds.X, expandedColumnBounds.Y);
											expandedColumnBounds.Offset(-expandedColumnBounds.X, -expandedColumnBounds.Y);
											ItemDrawInformation drawInfo = new ItemDrawInformation
											{
												Item = itemDrawInformation.Item + 1,
												Graphics = gr,
												DisplayType = ItemViewMode.Detail,
												Bounds = expandedColumnBounds,
												State = ItemViewStates.None,
												DrawBorder = false,
												SubItem = 0,
												Header = expandedColumn,
												TextColor = GetTextColor(ItemViewStates.None),
												ControlFocused = Focused,
												ExpandedColumn = true
											};
											((itemInformation == null || itemInformation.Group != displayedGroup) ? displayedGroup.Items.FirstOrDefault() : focusedItem)?.OnDraw(drawInfo);
										}
									}
									itemDrawInformation.GroupItem = -1;
									foreach (IViewableItem item in displayedGroup.Items)
									{
										itemDrawInformation.Item++;
										itemDrawInformation.GroupItem++;
										if (item.View == null)
										{
											continue;
										}
										Rectangle itemBounds = GetItemBounds(item);
										bool flag = false;
										if (itemBounds.IsEmpty)
										{
											continue;
										}
										if ((itemStates[item] & ItemViewStates.Selected) != 0)
										{
											selectedItems.Add(item);
											flag = true;
										}
										if (!viewRectangle.IntersectsWith(itemBounds))
										{
											continue;
										}
										visibleItems.Add(item);
										if (!gr.IsVisible(itemBounds) || (!flag && (drawItemsFlags & DrawItemViewOptions.SelectedOnly) != 0))
										{
											continue;
										}
										ItemViewStates itemViewStates = GetItemState(item);
										if ((drawItemsFlags & DrawItemViewOptions.FocusRectangle) == 0)
										{
											itemViewStates &= ~ItemViewStates.Focused;
										}
										if (!Focused)
										{
											itemViewStates &= ~ItemViewStates.Focused;
										}
										if (!Focused && hideSelection)
										{
											itemViewStates &= ~ItemViewStates.Selected;
										}
										OnDrawItemStates(gr, item, itemBounds, itemViewStates);
										using (gr.SaveState())
										{
											try
											{
												Rectangle rect = itemBounds;
												gr.IntersectClip(rect);
												gr.TranslateTransform(itemBounds.X, itemBounds.Y);
												itemBounds.Offset(-itemBounds.X, -itemBounds.Y);
												itemDrawInformation.Bounds = itemBounds;
												itemDrawInformation.State = itemViewStates;
												itemDrawInformation.SubItem = -1;
												itemDrawInformation.Header = null;
												itemDrawInformation.TextColor = GetTextColor(itemViewStates);
												itemDrawInformation.ControlFocused = Focused;
												item.OnDraw(itemDrawInformation);
												itemDrawInformation.DrawBorder = false;
												if (itemViewMode != ItemViewMode.Detail)
												{
													continue;
												}
												foreach (ItemViewColumn column in columns)
												{
													itemDrawInformation.SubItem++;
													if (!column.Visible || column == expandedColumn)
													{
														continue;
													}
													itemBounds.Width = column.Width;
													if (gr.IsVisible(itemBounds))
													{
														itemDrawInformation.Bounds = itemBounds;
														itemDrawInformation.Header = column;
														using (gr.SaveState())
														{
															item.OnDraw(itemDrawInformation);
														}
													}
													itemDrawInformation.DrawBorder = true;
													itemBounds.X += itemBounds.Width;
												}
											}
											catch (Exception)
											{
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private Color GetTextColor(ItemViewStates drawState)
		{
			return ForeColor;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			try
			{
				UpdatePositions(e.Graphics);
				DrawBackground(e.Graphics);
			}
			catch (Exception)
			{
				base.OnPaintBackground(e);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			using (graphics.SaveState())
			{
				try
				{
					if (UpdatePositions(graphics))
					{
						return;
					}
					DrawItems(graphics);
					if (MarkerVisible)
					{
						Rectangle markerBounds = GetMarkerBounds(MarkerItem, 2);
						Rectangle bounds = Translate(markerBounds, fromClient: false);
						DrawMarker(graphics, bounds);
					}
					if (!selectionRect.IsEmpty)
					{
						Rectangle rect = Translate(selectionRect, fromClient: false);
						rect.Inflate(-2, -2);
						using (Brush brush = new SolidBrush(Color.FromArgb(128, SystemColors.Highlight)))
						{
							graphics.FillRectangle(brush, rect);
						}
						graphics.DrawRectangle(SystemPens.Highlight, rect);
					}
					if (resizeColumn != null)
					{
						Rectangle viewRectangle = ViewRectangle;
						viewRectangle.X = GetColumnHeaderRectangle(resizeColumn).Right - base.ScrollPositionX;
						using (Pen pen = new Pen(Color.Black))
						{
							pen.DashStyle = DashStyle.DashDot;
							graphics.DrawLine(pen, viewRectangle.Location, new Point(viewRectangle.X, viewRectangle.Bottom));
						}
					}
					if (dragHeader != null)
					{
						Rectangle viewRectangle2 = ViewRectangle;
						Rectangle columnHeaderRectangle = GetColumnHeaderRectangle(dragHeader);
						viewRectangle2.X = columnHeaderRectangle.X - base.ScrollPositionX;
						viewRectangle2.Width = columnHeaderRectangle.Width;
						using (Brush brush2 = new SolidBrush(Color.FromArgb(128, SystemColors.ControlDark)))
						{
							graphics.FillRectangle(brush2, viewRectangle2);
						}
					}
					while (pendingSelectedIndexChanged)
					{
						pendingSelectedIndexChanged = false;
						OnSelectedIndexChanged();
					}
				}
				catch
				{
				}
			}
			OnPostPaint(e);
		}

		protected virtual void OnPostPaint(PaintEventArgs e)
		{
			if (this.PostPaint != null)
			{
				this.PostPaint(this, e);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (ItemViewMode != ItemViewMode.Detail)
			{
				SafeInvalidate();
			}
			Point position = Cursor.Position;
			position = PointToClient(position);
			UpdateHotItemState(Control.MouseButtons, position.X, position.Y);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			if (displayedItems.Count > 0)
			{
				IViewableItem viewableItem = itemStates.FindFirst(ItemViewStates.Focused);
				if (viewableItem == null)
				{
					viewableItem = displayedItems[0];
					itemStates.Focus(viewableItem);
				}
				Invalidate();
			}
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			Invalidate();
		}

		private ContextMenuStrip GetHeaderMenu()
		{
			if (HeaderContextMenuStrip != null)
			{
				return HeaderContextMenuStrip;
			}
			if (AutomaticHeaderMenu)
			{
				return autoHeaderContextMenuStrip;
			}
			return null;
		}

		public ContextMenuStrip GetViewMenu()
		{
			if (ViewContextMenuStrip != null)
			{
				return ViewContextMenuStrip;
			}
			if (AutomaticViewMenu && Columns.Count > 0)
			{
				return autoViewContextMenuStrip;
			}
			return null;
		}

		public void CreateGroupMenu(ToolStripItemCollection toolStripItemCollection)
		{
			ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(tr["NotGrouped", "Not Grouped"])
			{
				Checked = (ItemGrouper == null),
				Tag = null
			};
			toolStripMenuItem.Click += GroupMenuItemClicked;
			toolStripItemCollection.Add(toolStripMenuItem);
			toolStripItemCollection.Add(new ToolStripSeparator());
			foreach (ItemViewColumn column in Columns)
			{
				if (column.ColumnGrouper != null)
				{
					bool flag = ItemGrouper.Contains(column.ColumnGrouper);
					bool topLevel = column.Visible || flag;
					contextMenuBuilder.Add(FormUtility.FixAmpersand(column.Text), topLevel, flag, GroupMenuItemClicked, column, column.LastTimeVisible);
				}
			}
			toolStripItemCollection.AddRange(contextMenuBuilder.Create(20));
		}

		public void CreateArrangeMenu(ToolStripItemCollection toolStripItemCollection)
		{
			ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(tr["NotSorted", "Not Sorted"])
			{
				Checked = (ItemSorter == null)
			};
			toolStripMenuItem.Click += ArrangeMenuItemClicked;
			toolStripItemCollection.Add(toolStripMenuItem);
			toolStripItemCollection.Add(new ToolStripSeparator());
			foreach (ItemViewColumn column in Columns)
			{
				if (column.ColumnSorter != null)
				{
					bool flag = column.ColumnSorter == ItemSorter;
					bool topLevel = column.Visible || flag;
					contextMenuBuilder.Add(FormUtility.FixAmpersand(column.Text), topLevel, flag, ArrangeMenuItemClicked, column, column.LastTimeVisible);
				}
			}
			toolStripItemCollection.AddRange(contextMenuBuilder.Create(20));
		}

		public void CreateHeaderMenu(ToolStripItemCollection toolStripItemCollection, IColumn sizeColumn = null)
		{
			if (sizeColumn != null)
			{
				toolStripItemCollection.Add(tr["AutoSizeColumn", "Auto Size Column"], null, delegate
				{
					AutoSizeHeader(sizeColumn);
				});
			}
			toolStripItemCollection.Add(tr["AutoSizeAllColumns", "Auto Size All Columns"], null, delegate
			{
				using (new WaitCursor())
				{
					AutoSizeHeaders(all: false);
				}
			});
			toolStripItemCollection.Add(tr["AutoFitAllColumns", "Auto Fit All Columns"], null, delegate
			{
				using (new WaitCursor())
				{
					AutoFitHeaders(withAutosize: true);
				}
			});
			toolStripItemCollection.Add(new ToolStripSeparator());
			ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(tr["Layout", "Layout"], null)
			{
				DropDown = GetViewMenu()
			};
			if (toolStripMenuItem.DropDown != null)
			{
				toolStripItemCollection.Add(toolStripMenuItem);
				toolStripItemCollection.Add(new ToolStripSeparator());
			}
			ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
			foreach (ItemViewColumn column in columns)
			{
				contextMenuBuilder.Add(FormUtility.FixAmpersand(column.Text), column.Visible, column.Visible, HeaderMenuItemClicked, column.Id, column.LastTimeVisible);
			}
			toolStripItemCollection.AddRange(contextMenuBuilder.Create(20));
		}

		public void CreateStackMenu(ToolStripItemCollection toolStripItemCollection)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)toolStripItemCollection.Add(tr["NotStacked", "Not Stacked"], null, StackMenuItemClicked);
			toolStripMenuItem.Checked = ItemStacker == null;
			toolStripItemCollection.Add(new ToolStripSeparator());
			ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
			foreach (ItemViewColumn column in columns)
			{
				if (column.ColumnGrouper != null && column.ColumnSorter != null)
				{
					bool flag = ItemStacker.Contains(column.ColumnGrouper);
					bool topLevel = column.Visible || flag;
					contextMenuBuilder.Add(FormUtility.FixAmpersand(column.Text), topLevel, flag, StackMenuItemClicked, column, column.LastTimeVisible);
				}
			}
			toolStripItemCollection.AddRange(contextMenuBuilder.Create(20));
		}

		private void GroupMenuItemClicked(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			ItemViewColumn itemViewColumn = toolStripMenuItem.Tag as ItemViewColumn;
			if (itemViewColumn == null)
			{
				GroupDisplayEnabled = false;
				ItemGrouper = null;
				return;
			}
			if (Control.ModifierKeys.HasFlag(Keys.Control))
			{
				ItemGrouper = ItemGrouper.Append(itemViewColumn.ColumnGrouper, 3, removeIfContained: true);
				GroupSortingOrder = SortOrder.Ascending;
			}
			else if (ItemGrouper == itemViewColumn.ColumnGrouper)
			{
				GroupSortingOrder = FlipSortOrder(GroupSortingOrder);
			}
			else
			{
				ItemGrouper = itemViewColumn.ColumnGrouper;
				GroupSortingOrder = SortOrder.Ascending;
			}
			GroupDisplayEnabled = ItemGrouper != null;
		}

		private void ArrangeMenuItemClicked(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			ItemViewColumn itemViewColumn = toolStripMenuItem.Tag as ItemViewColumn;
			if (itemViewColumn == null)
			{
				ItemSorter = null;
				return;
			}
			if (ItemSorter == itemViewColumn.ColumnSorter)
			{
				ItemSortOrder = FlipSortOrder(ItemSortOrder);
				return;
			}
			ItemSorter = itemViewColumn.ColumnSorter;
			itemSortOrder = SortOrder.Ascending;
		}

		private void HeaderMenuItemClicked(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			IColumn column = Columns.FindById((int)toolStripMenuItem.Tag);
			if (column != null)
			{
				column.Visible = !column.Visible;
			}
		}

		private void StackMenuItemClicked(object sender, EventArgs e)
		{
			ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
			ItemViewColumn itemViewColumn = toolStripMenuItem.Tag as ItemViewColumn;
			if (itemViewColumn == null)
			{
				ItemStacker = null;
				return;
			}
			ItemStackSorter = itemViewColumn.ColumnSorter;
			if (Control.ModifierKeys.HasFlag(Keys.Control))
			{
				ItemStacker = ItemStacker.Append(itemViewColumn.ColumnGrouper, 3, removeIfContained: true);
			}
			else
			{
				ItemStacker = itemViewColumn.ColumnGrouper;
			}
		}

		private void autoHeaderContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			FormUtility.SafeToolStripClear(autoHeaderContextMenuStrip.Items);
			Point point = PointToClient(Cursor.Position);
			IColumn sizeColumn = ColumnHeaderHitTest(point.X, point.Y);
			CreateHeaderMenu(autoHeaderContextMenuStrip.Items, sizeColumn);
		}

		private void autoViewContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			miViewDetails.Checked = ItemViewMode == ItemViewMode.Detail;
			miViewThumbs.Checked = ItemViewMode == ItemViewMode.Thumbnail;
			miViewTiles.Checked = ItemViewMode == ItemViewMode.Tile;
			FormUtility.SafeToolStripClear(miArrange.DropDownItems);
			CreateArrangeMenu(miArrange.DropDownItems);
			FormUtility.SafeToolStripClear(miGroup.DropDownItems);
			CreateGroupMenu(miGroup.DropDownItems);
			miStack.Visible = StackDisplayEnabled && ItemViewMode == ItemViewMode.Thumbnail;
			FormUtility.SafeToolStripClear(miStack.DropDownItems);
			CreateStackMenu(miStack.DropDownItems);
		}

		private void ViewDropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			switch (e.ClickedItem.Name)
			{
			case "miViewDetails":
				ItemViewMode = ItemViewMode.Detail;
				break;
			case "miViewThumbs":
				ItemViewMode = ItemViewMode.Thumbnail;
				break;
			case "miViewTiles":
				ItemViewMode = ItemViewMode.Tile;
				break;
			}
		}

		protected virtual void OnDoubleClickColumnHeaderSeperator(IColumn column, Point point)
		{
			AutoSizeHeader(column);
		}

		protected virtual void OnMouseDownColumnHeaderSeparator(IColumn column, Point point)
		{
			resizeColumn = column;
			resizeColumnPos = point.X;
			resizeColumnWidth = resizeColumn.Width;
			Invalidate();
		}

		protected virtual void OnMouseMoveResizeColumnHeader(MouseEventArgs e)
		{
			resizeColumn.Width = (resizeColumnWidth + (e.X - resizeColumnPos)).Clamp(0, 10000);
		}

		protected virtual void OnMouseUpResizeColumnHeader(IColumn column, MouseEventArgs e)
		{
			resizeColumn = null;
			Invalidate();
		}

		protected virtual void OnMouseDownColumnHeader(IColumn column, Point pt)
		{
			pressedHeader = column;
			pressedHeaderPoint = pt;
			InvalidateHeader(column);
		}

		private void SetHotHeader(IColumn header)
		{
			if (hotHeader != header)
			{
				if (hotHeader != null)
				{
					InvalidateHeader(hotHeader);
				}
				hotHeader = header;
				if (hotHeader != null)
				{
					InvalidateHeader(hotHeader);
				}
			}
		}

		private void InvalidateHeader(IColumn column)
		{
			Rectangle columnHeaderRectangle = GetColumnHeaderRectangle(column);
			if (!columnHeaderRectangle.IsEmpty)
			{
				columnHeaderRectangle.Offset(-base.ScrollPosition.X, 0);
				Invalidate(columnHeaderRectangle);
			}
		}

		private void UpdateSelectionFromMouse(IViewableItem item, MouseEventArgs e)
		{
			bool flag = (Control.ModifierKeys & Keys.Control) != 0;
			bool flag2 = (Control.ModifierKeys & Keys.Shift) != 0;
			bool flag3 = (Control.ModifierKeys & Keys.Alt) != 0;
			bool flag4 = flag || flag2;
			if (item == null)
			{
				if (multiselect)
				{
					if (!flag4)
					{
						itemStates.Clear(ItemViewStates.Selected | ItemViewStates.Focused);
					}
					pressetViewPoint = Translate(new Point(e.X, e.Y), fromClient: true);
					selectItemState = new StateInfo(itemStates);
				}
				return;
			}
			StateInfo stateInfo = new StateInfo(itemStates);
			if ((!flag3 && SelectionMode == SelectionMode.MultiSimple && (e.Button & MouseButtons.Left) != 0) || !IsItemSelected(item) || flag4)
			{
				if (!flag4)
				{
					anchorItem = item;
				}
				if (!flag || !multiselect)
				{
					stateInfo.Clear(ItemViewStates.Selected);
				}
				if (flag2 && multiselect)
				{
					SelectFromAnchorItem(stateInfo, item, overideAnchor: true);
				}
				else if (flag && multiselect)
				{
					stateInfo.Flip(item, ItemViewStates.Selected);
				}
				else
				{
					stateInfo.Set(item, ItemViewStates.Selected, on: true);
				}
			}
			stateInfo.Focus(item);
			itemStates.Update(stateInfo);
		}

		protected virtual void OnMouseDownView(MouseEventArgs e)
		{
			IViewableItem viewableItem = ItemHitTest(e.X, e.Y);
			clickItem = null;
			customClick = false;
			if (viewableItem == null || !IsItemSelected(viewableItem))
			{
				UpdateSelectionFromMouse(viewableItem, e);
			}
			else
			{
				customClick = e.Button.IsSet(MouseButtons.Left) && InvokeItemClick(viewableItem, e.Location);
				if (!customClick)
				{
					clickItem = viewableItem;
				}
			}
			if (viewableItem != null && !customClick)
			{
				dragItem = viewableItem;
			}
		}

		protected virtual void OnMouseUpView(MouseEventArgs e)
		{
			if (clickItem != null)
			{
				UpdateSelectionFromMouse(clickItem, e);
			}
			if (selectItemState != null)
			{
				Invalidate(Translate(selectionRect, fromClient: false));
				selectionRect = Rectangle.Empty;
				pressetViewPoint = Point.Empty;
				selectItemState = null;
			}
			if (FocusedItem != null)
			{
				EnsureItemVisible(FocusedItem);
			}
			dragItem = null;
		}

		protected virtual void OnStartDragColumnHeader(IColumn column, MouseEventArgs e)
		{
			dragHeader = column;
			Invalidate();
		}

		protected virtual void OnDragColumnHeader(IColumn column, MouseEventArgs e)
		{
			MoveColumnHeader(column, e.X + base.ScrollPosition.X);
		}

		protected virtual void OnMouseUpColumnHeader(IColumn column, MouseEventArgs e)
		{
			pressedHeader = null;
			dragHeader = null;
		}

		protected virtual void OnMouseClickGroupHeader(GroupHeaderInformation groupHeaderInformation, bool arrow)
		{
			if (arrow)
			{
				groupHeaderInformation.Collapsed = !groupHeaderInformation.Collapsed;
				SafeInvalidate();
			}
			else
			{
				if (SelectionMode == SelectionMode.One || SelectionMode == SelectionMode.None)
				{
					return;
				}
				StateInfo stateInfo = new StateInfo(itemStates);
				ItemViewStates itemViewStates = ItemViewStates.Focused;
				stateInfo.Clear(ItemViewStates.Selected | ItemViewStates.Focused);
				foreach (IViewableItem item in groupHeaderInformation.Items.Lock())
				{
					stateInfo.Set(item, ItemViewStates.Selected | itemViewStates, on: true);
					itemViewStates = ItemViewStates.None;
				}
				itemStates.Update(stateInfo);
			}
		}

		protected virtual void OnMouseDoubleClickGroupHeader(GroupHeaderInformation groupHeaderInformation, bool arrow)
		{
			if (arrow)
			{
				ExpandGroups(groupHeaderInformation.Collapsed);
				return;
			}
			groupHeaderInformation.Collapsed = !groupHeaderInformation.Collapsed;
			SafeInvalidate();
		}

		protected override void OnAutoScrolling(AutoScrollEventArgs e)
		{
			base.OnAutoScrolling(e);
			if (resizeColumn != null || dragHeader != null)
			{
				e.Y = 0;
			}
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);
			StopLongClick();
			Point point = PointToClient(Cursor.Position);
			int num = ColumnHeaderSeparatorHitTest(point.X, point.Y);
			if (num != -1)
			{
				OnDoubleClickColumnHeaderSeperator(columns[num], point);
			}
			else
			{
				if (!ViewRectangle.Contains(point))
				{
					return;
				}
				if (AreGroupsVisible)
				{
					Point pt = Translate(point, fromClient: true);
					foreach (GroupHeaderInformation item in displayedGroups.Lock())
					{
						if (item.Bounds.Contains(pt))
						{
							OnMouseDoubleClickGroupHeader(item, item.ArrowBounds.Contains(pt));
							doubleGroupClick = true;
							return;
						}
					}
				}
				if (!customClick)
				{
					activateButton = lastMouseButton;
					try
					{
						InvokeActivate();
					}
					finally
					{
						activateButton = MouseButtons.None;
					}
				}
			}
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			mouseDownPoint = new Point(e.X, e.Y);
			lastMouseButton = e.Button;
			IViewableItem focusedItem = FocusedItem;
			longClickTimer.Stop();
			longClickSubItem = -1;
			if (e.Button == MouseButtons.XButton1 || e.Button == MouseButtons.XButton2)
			{
				return ;
			}
			if (e.Button == MouseButtons.Left)
			{
				int num = ColumnHeaderSeparatorHitTest(e.X, e.Y);
				if (num != -1)
				{
					OnMouseDownColumnHeaderSeparator(columns[num], e.Location);
					return;
				}
			}
			if (e.Button == MouseButtons.Left)
			{
				IColumn column = ColumnHeaderHitTest(e.X, e.Y);
				if (column != null)
				{
					OnMouseDownColumnHeader(column, e.Location);
					return;
				}
			}
			if (!ViewRectangle.Contains(e.X, e.Y))
			{
				return;
			}
			OnMouseDownView(e);
			if (!customClick && (e.Button & MouseButtons.Left) != 0)
			{
				int subItem;
				IViewableItem viewableItem = ItemHitTest(e.X, e.Y, out subItem);
				if (subItem != -1 && viewableItem == focusedItem)
				{
					longClickItem = viewableItem;
					longClickSubItem = subItem;
				}
			}
		}

		private void UpdateSelection(int x, int y)
		{
			if (pressetViewPoint.IsEmpty)
			{
				return;
			}
			Point point = Translate(new Point(x, y), fromClient: true);
			Rectangle right = RectangleExtensions.Create(pressetViewPoint, point);
			if (selectionRect == right)
			{
				return;
			}
			Invalidate(Translate(selectionRect, fromClient: false));
			selectionRect = right;
			Invalidate(Translate(selectionRect, fromClient: false));
			StateInfo stateInfo = new StateInfo(itemStates);
			stateInfo.Clear(ItemViewStates.Focused);
			bool flag = false;
			bool flag2 = (Control.ModifierKeys & Keys.Control) != 0;
			IEnumerable<IViewableItem> enumerable = visibleItems.Lock();
			Rectangle rc = Translate(ViewRectangle, fromClient: true);
			foreach (IViewableItem item in enumerable)
			{
				if (!ItemIntersects(item, rc))
				{
					continue;
				}
				if ((Control.ModifierKeys & (Keys.Shift | Keys.Control)) != 0)
				{
					stateInfo.Set(item, ItemViewStates.Selected, selectItemState[item].HasFlag(ItemViewStates.Selected));
				}
				else
				{
					stateInfo.Set(item, ItemViewStates.Selected, on: false);
				}
				if (ItemIntersects(item, selectionRect))
				{
					if (flag2)
					{
						stateInfo.Flip(item, ItemViewStates.Selected);
					}
					else
					{
						stateInfo.Set(item, ItemViewStates.Selected, on: true);
					}
					if (!flag && ItemIntersects(item, point))
					{
						flag = true;
						stateInfo.Focus(item);
					}
				}
			}
			itemStates.Update(stateInfo);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			string text = ((hotHeader == null || !HeaderToolTips) ? string.Empty : hotHeader.TooltipText);
			if (text != toolTip.GetToolTip(this))
			{
				toolTip.SetToolTip(this, text);
			}
			if (longClickSubItem != -1 && (Math.Abs(e.X - mouseDownPoint.X) > 2 || Math.Abs(e.Y - mouseDownPoint.Y) > 2))
			{
				StopLongClick();
			}
			if (resizeColumn != null)
			{
				OnMouseMoveResizeColumnHeader(e);
				return;
			}
			if (dragHeader != null)
			{
				OnDragColumnHeader(dragHeader, e);
				return;
			}
			if (pressedHeader != null)
			{
				if (SystemInformation.DragSize.Width < Math.Abs(e.X - pressedHeaderPoint.X) || SystemInformation.DragSize.Height < Math.Abs(e.Y - pressedHeaderPoint.Y))
				{
					OnStartDragColumnHeader(pressedHeader, e);
				}
				return;
			}
			if (dragItem != null && (Math.Abs(mouseDownPoint.X - e.X) > SystemInformation.DragSize.Width || Math.Abs(mouseDownPoint.Y - e.Y) > SystemInformation.DragSize.Height))
			{
				OnItemDrag(new ItemDragEventArgs(e.Button, dragItem));
				clickItem = null;
				dragItem = null;
			}
			SetHotHeader(ColumnHeaderHitTest(e.X, e.Y));
			UpdateContextMenu(e.Location);
			Cursor = ((ColumnHeaderSeparatorHitTest(e.X, e.Y) != -1) ? Cursors.VSplit : Cursors.Default);
			UpdateHotItemState(e.Button, e.X, e.Y);
			if (e.Button != MouseButtons.Middle)
			{
				UpdateSelection(e.X, e.Y);
			}
		}

		private void UpdateContextMenu(Point location)
		{
			if (location == Point.Empty)
			{
				location = PointToClient(Cursor.Position);
			}
			if (InplaceEditItem != null)
			{
				ContextMenuStrip = null;
			}
			else if (GetHeaderMenu() != null && hotHeader != null)
			{
				ContextMenuStrip = GetHeaderMenu();
			}
			else if (ItemContextMenuStrip != null && ItemHitTest(location.X, location.Y) != null)
			{
				ContextMenuStrip = ItemContextMenuStrip;
			}
			else if (GetViewMenu() != null)
			{
				ContextMenuStrip = GetViewMenu();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (resizeColumn != null)
			{
				OnMouseUpResizeColumnHeader(resizeColumn, e);
			}
			else if (pressedHeader != null)
			{
				if (dragHeader == null)
				{
					OnHeaderClick(pressedHeader);
					Invalidate(ColumnHeadersRectangle);
				}
				else
				{
					Invalidate();
				}
				OnMouseUpColumnHeader(pressedHeader, e);
			}
			else
			{
				OnMouseUpView(e);
			}
			if (e.Button == MouseButtons.Left && AreGroupsVisible && !doubleGroupClick)
			{
				Point pt = Translate(mouseDownPoint, fromClient: true);
				foreach (GroupHeaderInformation item in displayedGroups.Lock())
				{
					if (item.ArrowBounds.Contains(pt))
					{
						OnMouseClickGroupHeader(item, arrow: true);
						break;
					}
					if (item.TextBounds.Contains(pt))
					{
						OnMouseClickGroupHeader(item, arrow: false);
						break;
					}
				}
			}
			doubleGroupClick = false;
			if (longClickSubItem != -1)
			{
				longClickTimer.Start();
			}
			base.OnMouseUp(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			StopLongClick();
			SetHotHeader(null);
			StateInfo stateInfo = new StateInfo(itemStates);
			IViewableItem item;
			while ((item = stateInfo.FindFirst(ItemViewStates.Hot)) != null)
			{
				stateInfo.Set(item, ItemViewStates.Hot, on: false);
			}
			itemStates.Update(stateInfo);
			base.OnMouseLeave(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			StopLongClick();
			if (Control.ModifierKeys == Keys.None)
			{
				float num = (float)(-e.Delta) / (float)SystemInformation.MouseWheelScrollDelta;
				ScrollView(num * (float)SystemInformation.MouseWheelScrollLines);
			}
		}

		public bool EditItem(Point screenCursorLocation)
		{
			screenCursorLocation = PointToClient(screenCursorLocation);
			int subItem;
			IViewableItem viewableItem = ItemHitTest(screenCursorLocation.X, screenCursorLocation.Y, out subItem);
			if (viewableItem == null || subItem == -1)
			{
				return false;
			}
			return EditItem(viewableItem, subItem);
		}

		public bool EditItem(IViewableItem editItem, int editSubItem = -1)
		{
			if (!LabelEdit || editItem == null || ItemViewMode != ItemViewMode.Detail)
			{
				return false;
			}
			if (editSubItem == -1)
			{
				editSubItem = GetNextEditSubItem(editItem, 0, 1);
			}
			if (editSubItem == -1)
			{
				return false;
			}
			Rectangle itemBounds = GetItemBounds(editItem, editSubItem);
			if (itemBounds.IsEmpty)
			{
				return false;
			}
			itemBounds = Translate(itemBounds, fromClient: false);
			Control editControl = editItem.GetEditControl(editSubItem);
			if (editControl == null)
			{
				return false;
			}
			currentInplaceEditItem = editItem;
			currentInplaceEditSubItem = editSubItem;
			currentInplaceEditControl = editControl;
			base.Controls.Add(editControl);
			editControl.Bounds = itemBounds;
			MoveEditControl(editControl);
			editControl.Show();
			editControl.Focus();
			editControl.LostFocus += EditorControlLostFocus;
			editControl.SizeChanged += EditorControlSizeChanged;
			editControl.KeyDown += EditorControlKeyDown;
			UpdateContextMenu(Point.Empty);
			return true;
		}

		protected virtual void OnLongClick(IViewableItem editItem, int editSubItem)
		{
			EditItem(editItem, editSubItem);
		}

		private void MoveEditControl(Control c)
		{
			Rectangle bounds = c.Bounds;
			Rectangle viewRectangle = ViewRectangle;
			if (bounds.Y < viewRectangle.Y)
			{
				bounds.Y = viewRectangle.Y;
			}
			if (bounds.X < viewRectangle.X)
			{
				bounds.X = viewRectangle.X;
			}
			if (bounds.Right > viewRectangle.Right)
			{
				bounds.X = viewRectangle.Right - bounds.Width;
			}
			if (bounds.Bottom > viewRectangle.Bottom)
			{
				bounds.Y = viewRectangle.Bottom - bounds.Height;
			}
			c.Bounds = bounds;
		}

		private void ExitEdit(Control c)
		{
			if (c != null)
			{
				c.LostFocus -= EditorControlLostFocus;
				c.SizeChanged -= EditorControlSizeChanged;
				base.Controls.Remove(c);
				c.Dispose();
				Focus();
				currentInplaceEditItem = null;
				currentInplaceEditSubItem = -1;
				currentInplaceEditControl = null;
				UpdateContextMenu(Point.Empty);
			}
		}

		private void longClickTimer_Tick(object sender, EventArgs e)
		{
			longClickTimer.Stop();
			if (longClickItem != null && longClickSubItem != -1)
			{
				OnLongClick(longClickItem, longClickSubItem);
			}
		}

		private void EditorControlSizeChanged(object sender, EventArgs e)
		{
			MoveEditControl(sender as Control);
		}

		private void EditorControlLostFocus(object sender, EventArgs e)
		{
			ExitEdit(sender as Control);
		}

		private void EditorControlKeyDown(object sender, KeyEventArgs e)
		{
			Control c = (Control)sender;
			switch (e.KeyCode)
			{
			case Keys.Down:
			{
				IViewableItem relativeItem = GetRelativeItem(currentInplaceEditItem, 0, 1);
				if (relativeItem != currentInplaceEditItem)
				{
					ExitEdit(c);
					SetItemState(relativeItem, ItemViewStates.Selected | ItemViewStates.Focused, multiSelect: false);
					EnsureItemVisible(relativeItem, currentInplaceEditSubItem);
					EditItem(relativeItem, currentInplaceEditSubItem);
				}
				break;
			}
			case Keys.Up:
			{
				IViewableItem relativeItem = GetRelativeItem(currentInplaceEditItem, 0, -1);
				if (relativeItem != currentInplaceEditItem)
				{
					ExitEdit(c);
					SetItemState(relativeItem, ItemViewStates.Selected | ItemViewStates.Focused, multiSelect: false);
					EnsureItemVisible(relativeItem, currentInplaceEditSubItem);
					EditItem(relativeItem, currentInplaceEditSubItem);
				}
				break;
			}
			case Keys.Tab:
			case Keys.Left:
			case Keys.Right:
			{
				int num;
				if (e.KeyCode == Keys.Tab)
				{
					num = ((!e.Modifiers.HasFlag(Keys.Shift)) ? 1 : (-1));
				}
				else
				{
					if (!e.Alt)
					{
						break;
					}
					num = ((e.KeyCode != Keys.Left) ? 1 : (-1));
				}
				IViewableItem viewableItem = currentInplaceEditItem;
				int nextEditSubItem = GetNextEditSubItem(viewableItem, currentInplaceEditSubItem + num, num);
				if (nextEditSubItem != -1)
				{
					ExitEdit(c);
					EnsureItemVisible(viewableItem, nextEditSubItem);
					EditItem(viewableItem, nextEditSubItem);
				}
				break;
			}
			}
		}

		private int GetNextEditSubItem(IViewableItem item, int current, int relative)
		{
			if (item == null)
			{
				return -1;
			}
			while (current >= 0 && current < Columns.Count)
			{
				if (Columns[current].Visible)
				{
					Control editControl = item.GetEditControl(current);
					if (editControl != null)
					{
						editControl.Dispose();
						return current;
					}
				}
				current += relative;
			}
			return -1;
		}

		private void StopLongClick()
		{
			if (longClickSubItem != -1)
			{
				longClickSubItem = -1;
				longClickTimer.Stop();
			}
		}

		public IViewableItem GetRelativeItem(IViewableItem item, int deltaColumns, int deltaRows)
		{
			ItemInformation itemInformation = GetItemInformation(item);
			if (itemInformation == null)
			{
				return null;
			}
			int num = GetColumnRowItems(0, -1).Length;
			int num2 = itemInformation.Row;
			int num3 = itemInformation.Column;
			if (num2 + deltaRows < 0)
			{
				num2 = 0;
				num3 = (num3 + deltaColumns).Clamp(0, GetColumnRowItems(-1, num2).Length - 1);
			}
			else if (num2 + deltaRows >= num)
			{
				num2 = num - 1;
				num3 = (num3 + deltaColumns).Clamp(0, GetColumnRowItems(-1, num2).Length - 1);
			}
			else
			{
				int num4 = GetColumnRowItems(-1, num2).Length;
				int num5 = Math.Sign(deltaColumns);
				deltaColumns = Math.Abs(deltaColumns);
				while (--deltaColumns >= 0)
				{
					num3 += num5;
					if (num3 < 0)
					{
						if (num2 == 0)
						{
							num3 = 0;
							break;
						}
						num3 = GetColumnRowItems(-1, --num2).Length - 1;
					}
					else if (num3 >= num4)
					{
						if (num2 == num - 1)
						{
							num3 = num4 - 1;
							break;
						}
						num3 = 0;
						num4 = GetColumnRowItems(-1, ++num2).Length;
					}
				}
				num2 = (num2 + deltaRows).Clamp(0, num - 1);
				num3 = num3.Clamp(0, GetColumnRowItems(-1, num2).Length - 1);
			}
			return GetColumnRowItems(num3, num2).FirstOrDefault();
		}

		private IViewableItem GetRelativeItem(IViewableItem focus, Keys key)
		{
			int num = 0;
			int num2 = 0;
			if (IsTopLayout)
			{
				num = ViewRectangle.Height / GetDefaultItemSize(0).Height;
			}
			else
			{
				num2 = ViewRectangle.Width / GetDefaultItemSize(ViewRectangle.Width).Width;
			}
			switch (key)
			{
			case Keys.Left:
				return GetRelativeItem(focus, -1, 0);
			case Keys.Right:
				return GetRelativeItem(focus, 1, 0);
			case Keys.Up:
				return GetRelativeItem(focus, 0, -1);
			case Keys.Down:
				return GetRelativeItem(focus, 0, 1);
			case Keys.Next:
				return GetRelativeItem(focus, num2, num);
			case Keys.Prior:
				return GetRelativeItem(focus, -num2, -num);
			case Keys.Home:
				return GetColumnRowItems(0, 0).FirstOrDefault();
			case Keys.End:
				return GetRelativeItem(focus, 10000000, 10000000);
			default:
				return focus;
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			int count;
			using (ItemMonitor.Lock(displayedItems))
			{
				count = displayedItems.Count;
			}
			if (count == 0)
			{
				base.OnKeyDown(e);
				return;
			}
			StateInfo stateInfo = new StateInfo(itemStates);
			IViewableItem viewableItem = stateInfo.FindFirst(ItemViewStates.Focused);
			using (ItemMonitor.Lock(displayedItems))
			{
				if (viewableItem == null)
				{
					viewableItem = displayedItems[0];
				}
			}
			switch (e.KeyCode)
			{
			case Keys.Prior:
			case Keys.Next:
			case Keys.End:
			case Keys.Home:
			case Keys.Left:
			case Keys.Up:
			case Keys.Right:
			case Keys.Down:
				viewableItem = GetRelativeItem(viewableItem, e.KeyCode);
				stateInfo.Focus(viewableItem);
				if (!e.Control && !e.Shift)
				{
					anchorItem = viewableItem;
				}
				if (!e.Control)
				{
					stateInfo.Clear(ItemViewStates.Selected);
					stateInfo.Set(viewableItem, ItemViewStates.Selected, on: true);
				}
				if (e.Shift && multiselect)
				{
					SelectFromAnchorItem(stateInfo, viewableItem);
				}
				e.Handled = true;
				break;
			case Keys.Space:
				if (!multiselect)
				{
					stateInfo.Clear(ItemViewStates.Selected);
				}
				if (e.Control)
				{
					stateInfo.Flip(viewableItem, ItemViewStates.Selected);
				}
				else
				{
					stateInfo.Set(viewableItem, ItemViewStates.Selected, on: true);
				}
				e.Handled = true;
				break;
			case Keys.Return:
				InvokeActivate();
				e.Handled = true;
				break;
			default:
				base.OnKeyDown(e);
				return;
			}
			if (e.Handled)
			{
				StopLongClick();
			}
			EnsureItemVisible(viewableItem);
			itemStates.Update(stateInfo);
		}

		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData & ~Keys.Shift)
			{
			case Keys.Left:
			case Keys.Up:
			case Keys.Right:
			case Keys.Down:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		public static SortOrder FlipSortOrder(SortOrder sortOrder)
		{
			switch (sortOrder)
			{
			case SortOrder.Ascending:
				return SortOrder.Descending;
			case SortOrder.Descending:
				return SortOrder.Ascending;
			default:
				return SortOrder.None;
			}
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			autoHeaderContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
			dummyItem = new System.Windows.Forms.ToolStripMenuItem();
			autoViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
			miViewMode = new System.Windows.Forms.ToolStripMenuItem();
			miViewThumbs = new System.Windows.Forms.ToolStripMenuItem();
			miViewTiles = new System.Windows.Forms.ToolStripMenuItem();
			miViewDetails = new System.Windows.Forms.ToolStripMenuItem();
			miArrange = new System.Windows.Forms.ToolStripMenuItem();
			miGroup = new System.Windows.Forms.ToolStripMenuItem();
			miStack = new System.Windows.Forms.ToolStripMenuItem();
			longClickTimer = new System.Windows.Forms.Timer(components);
			toolTip = new System.Windows.Forms.ToolTip(components);
			autoHeaderContextMenuStrip.SuspendLayout();
			autoViewContextMenuStrip.SuspendLayout();
			SuspendLayout();
			autoHeaderContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				dummyItem
			});
			autoHeaderContextMenuStrip.Name = "autoHeaderContextMenuStrip";
			autoHeaderContextMenuStrip.Size = new System.Drawing.Size(181, 26);
			autoHeaderContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(autoHeaderContextMenuStrip_Opening);
			dummyItem.Name = "dummyItem";
			dummyItem.Size = new System.Drawing.Size(180, 22);
			dummyItem.Text = "toolStripMenuItem1";
			autoViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[4]
			{
				miViewMode,
				miArrange,
				miGroup,
				miStack
			});
			autoViewContextMenuStrip.Name = "autoViewContextMenuStrip";
			autoViewContextMenuStrip.Size = new System.Drawing.Size(133, 92);
			autoViewContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(autoViewContextMenuStrip_Opening);
			miViewMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
				miViewThumbs,
				miViewTiles,
				miViewDetails
			});
			miViewMode.Name = "miViewMode";
			miViewMode.Size = new System.Drawing.Size(132, 22);
			miViewMode.Text = "View";
			miViewThumbs.Name = "miViewThumbs";
			miViewThumbs.Size = new System.Drawing.Size(137, 22);
			miViewThumbs.Text = "Thumbnails";
			miViewTiles.Name = "miViewTiles";
			miViewTiles.Size = new System.Drawing.Size(137, 22);
			miViewTiles.Text = "Tiles";
			miViewDetails.Name = "miViewDetails";
			miViewDetails.Size = new System.Drawing.Size(137, 22);
			miViewDetails.Text = "Details";
			miArrange.Name = "miArrange";
			miArrange.Size = new System.Drawing.Size(132, 22);
			miArrange.Text = "Arrange by";
			miGroup.Name = "miGroup";
			miGroup.Size = new System.Drawing.Size(132, 22);
			miGroup.Text = "Group by";
			miStack.Name = "miStack";
			miStack.Size = new System.Drawing.Size(132, 22);
			miStack.Text = "Stack by";
			longClickTimer.Interval = 1000;
			longClickTimer.Tick += new System.EventHandler(longClickTimer_Tick);
			BackColor = System.Drawing.SystemColors.Window;
			base.Size = new System.Drawing.Size(624, 600);
			autoHeaderContextMenuStrip.ResumeLayout(false);
			autoViewContextMenuStrip.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
