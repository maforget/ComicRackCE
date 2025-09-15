using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows.Properties;
using Windows7.Multitouch;
using Windows7.Multitouch.WinForms;

namespace cYo.Common.Windows.Forms
{
	public class TabBar : ContainerControl
	{
		private enum ItemState
		{
			None,
			Hot,
			Selected
		}

		public class TabBarItem
		{
			private string text;

			private Image image;

			private bool enabled = true;

			private TabItemState state = TabItemState.Normal;

			private Padding padding = Padding.Empty;

			private bool showInDropDown = true;

			private bool canClose;

			private bool visible = true;

			private bool fontBold;

			private TextFormatFlags textFormat = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.PreserveGraphicsClipping;

			private int minimumWidth = 60;

			private bool adjustWidth = true;

			private bool showText = true;

			private bool closeButtonHot;

			[DefaultValue(null)]
			public string Name
			{
				get;
				set;
			}

			[DefaultValue(null)]
			public string Text
			{
				get
				{
					return text;
				}
				set
				{
					SetValue(ref text, value);
				}
			}

			[DefaultValue(null)]
			public virtual string ToolTipText
			{
				get;
				set;
			}

			[DefaultValue(typeof(Size), "Empty")]
			public virtual Size ToolTipSize
			{
				get;
				set;
			}

			[DefaultValue(null)]
			public object Tag
			{
				get;
				set;
			}

			[DefaultValue(null)]
			public Image Image
			{
				get
				{
					return image;
				}
				set
				{
					SetValue(ref image, value);
					ImageSize = ((image == null) ? Size.Empty : image.Size);
				}
			}

			[Browsable(false)]
			public Size ImageSize
			{
				get;
				private set;
			}

			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public Rectangle Bounds
			{
				get;
				internal set;
			}

			[DefaultValue(true)]
			public bool Enabled
			{
				get
				{
					return enabled;
				}
				set
				{
					SetValue(ref enabled, value);
				}
			}

			[DefaultValue(TabItemState.Normal)]
			public TabItemState State
			{
				get
				{
					return state;
				}
				set
				{
					if (state == value)
					{
						return;
					}
					if (value == TabItemState.Selected)
					{
						TabItemState tabItemState = state;
						state = value;
						CancelEventArgs cancelEventArgs = new CancelEventArgs();
						OnSelected(cancelEventArgs);
						if (cancelEventArgs.Cancel)
						{
							state = tabItemState;
							return;
						}
					}
					state = value;
					OnChanged();
				}
			}

			[DefaultValue(typeof(Padding), "Empty")]
			public Padding Padding
			{
				get
				{
					return padding;
				}
				set
				{
					SetValue(ref padding, value);
				}
			}

			[DefaultValue(true)]
			public bool ShowInDropDown
			{
				get
				{
					return showInDropDown;
				}
				set
				{
					showInDropDown = value;
				}
			}

			[DefaultValue(false)]
			public bool CanClose
			{
				get
				{
					return canClose;
				}
				set
				{
					SetValue(ref canClose, value);
				}
			}

			[DefaultValue(null)]
			public ContextMenuStrip ContextMenu
			{
				get;
				set;
			}

			[DefaultValue(true)]
			public bool Visible
			{
				get
				{
					return visible;
				}
				set
				{
					SetValue(ref visible, value);
				}
			}

			[DefaultValue(false)]
			public bool FontBold
			{
				get
				{
					return fontBold;
				}
				set
				{
					SetValue(ref fontBold, value);
				}
			}

			[DefaultValue(TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine)]
			public TextFormatFlags TextFormat
			{
				get
				{
					return textFormat;
				}
				set
				{
					SetValue(ref textFormat, value);
				}
			}

			[DefaultValue(60)]
			public int MinimumWidth
			{
				get
				{
					return minimumWidth;
				}
				set
				{
					SetValue(ref minimumWidth, value);
				}
			}

			[DefaultValue(true)]
			public bool AdjustWidth
			{
				get
				{
					return adjustWidth;
				}
				set
				{
					SetValue(ref adjustWidth, value);
				}
			}

			[DefaultValue(true)]
			public bool ShowText
			{
				get
				{
					return showText;
				}
				set
				{
					SetValue(ref showText, value);
				}
			}

			public bool IsSelected => State == TabItemState.Selected;

			internal Rectangle CloseBounds
			{
				get;
				set;
			}

			internal bool CloseButtonHot
			{
				get
				{
					return closeButtonHot;
				}
				set
				{
					SetValue(ref closeButtonHot, value);
				}
			}

			public event EventHandler Changed;

			public event CancelEventHandler Selected;

			public event CancelEventHandler CaptionClick;

			public event EventHandler Click;

			public event EventHandler CloseClick;

			public event EventHandler Removed;

			public TabBarItem()
			{
			}

			public TabBarItem(string text)
			{
				this.text = text;
			}

			internal Font GetFont(Font font)
			{
				if (!FontBold)
				{
					return font;
				}
				return FC.Get(font, FontStyle.Bold);
			}

			protected virtual void OnChanged()
			{
				if (this.Changed != null)
				{
					this.Changed(this, EventArgs.Empty);
				}
			}

			protected virtual void OnClick()
			{
				if (this.Click != null)
				{
					this.Click(this, EventArgs.Empty);
				}
			}

			protected virtual void OnCaptionClick(CancelEventArgs e)
			{
				if (this.CaptionClick != null)
				{
					this.CaptionClick(this, e);
				}
			}

			protected virtual void OnCloseClick()
			{
				if (this.CloseClick != null)
				{
					this.CloseClick(this, EventArgs.Empty);
				}
			}

			protected virtual void OnSelected(CancelEventArgs e)
			{
				if (this.Selected != null)
				{
					this.Selected(this, e);
				}
			}

			protected virtual void OnRemoved()
			{
				if (this.Removed != null)
				{
					this.Removed(this, EventArgs.Empty);
				}
			}

			private bool SetValue<T>(ref T value, T newValue)
			{
				if (object.Equals(value, newValue))
				{
					return false;
				}
				value = newValue;
				OnChanged();
				return true;
			}

			public bool InvokeCaptionClick()
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs();
				OnCaptionClick(cancelEventArgs);
				return cancelEventArgs.Cancel;
			}

			public void InvokeClick()
			{
				OnClick();
			}

			public void InvokeCloseClick()
			{
				OnCloseClick();
			}

			public void InvokeRemoved()
			{
				OnRemoved();
			}

			public virtual bool ShowToolTip()
			{
				return true;
			}

			public virtual void DrawTooltip(Graphics gr, Rectangle rc)
			{
			}
		}

		public class TabBarItemCollection : SmartList<TabBarItem>
		{
			public TabBarItem this[string name] => Find((TabBarItem x) => x.Name == name);

			public TabBarItemCollection()
				: base(SmartListOptions.DisableOnSet | SmartListOptions.ClearWithRemove)
			{
			}
		}

		public class SelectedTabChangedEventArgs : CancelEventArgs
		{
			private readonly TabBarItem newItem;

			private readonly TabBarItem oldItem;

			public TabBarItem NewItem => newItem;

			public TabBarItem OldItem => oldItem;

			public SelectedTabChangedEventArgs(TabBarItem oldItem, TabBarItem newItem)
			{
				this.newItem = newItem;
				this.oldItem = oldItem;
			}
		}

		public class ToolStripHost : ToolStripControlHost
		{
			private bool spring = true;

			public TabBar TabBar => base.Control as TabBar;

			public bool Spring
			{
				get
				{
					return spring;
				}
				set
				{
					spring = value;
					if (base.Owner != null)
					{
						base.Owner.PerformLayout();
					}
				}
			}

			public ToolStripHost(TabBar tabBar)
				: base(tabBar)
			{
				base.AutoSize = true;
				base.Overflow = ToolStripItemOverflow.Never;
				tabBar.BackColor = Color.Transparent;
				tabBar.TabHeight = 24;
				tabBar.DrawBaseLine = false;
			}

			public override Size GetPreferredSize(Size constrainingSize)
			{
				if (!Spring || base.IsOnOverflow || base.Owner.Orientation == Orientation.Vertical)
				{
					return DefaultSize;
				}
				int num = base.Owner.DisplayRectangle.Width;
				if (base.Owner.OverflowButton.Visible)
				{
					num = num - base.Owner.OverflowButton.Width - base.Owner.OverflowButton.Margin.Horizontal;
				}
				int num2 = 0;
				foreach (ToolStripItem item in base.Owner.Items)
				{
					if (!item.IsOnOverflow)
					{
						if (item is ToolStripHost)
						{
							num2++;
							num -= item.Margin.Horizontal;
						}
						else
						{
							num -= item.Width + item.Margin.Horizontal;
						}
					}
				}
				if (num2 > 1)
				{
					num /= num2;
				}
				Size preferredSize = base.GetPreferredSize(constrainingSize);
				if (TabBar.TabHeight != 0)
				{
					preferredSize.Height = TabBar.TabHeight;
				}
				preferredSize.Width = num;
				return preferredSize;
			}
		}

		public class TabBarToolStripRenderer : ExtendedToolStripRenderer
		{
			public TabBarToolStripRenderer(ToolStripRenderer renderer)
				: base(renderer)
			{
			}

			protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
			{
				base.OnRenderToolStripBorder(e);
				Rectangle affectedBounds = e.AffectedBounds;
				using (Pen pen = new Pen(BorderColor))
				{
					e.Graphics.DrawLine(pen, affectedBounds.Left, affectedBounds.Bottom - 2, affectedBounds.Right, affectedBounds.Bottom - 2);
				}
			}

			protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
			{
				if (!Application.RenderWithVisualStyles || !VisualStyleRenderer.IsElementDefined(VisualStyleElement.Tab.Body.Normal))
				{
					base.OnRenderToolStripBackground(e);
					return;
				}
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
				visualStyleRenderer.DrawBackground(e.Graphics, e.AffectedBounds);
			}
		}

		private static readonly int arrowWidth = SystemInformation.VerticalScrollBarWidth;

		private static readonly int dropDownWidth = SystemInformation.VerticalScrollBarWidth;

		public static bool StyleEnabled = true;

		private static Bitmap arrowLeft = Resources.SimpleArrowLeft;

		private static Bitmap arrowRight = Resources.SimpleArrowRight;

		private static Bitmap arrowDown = Resources.SimpleArrowDown;

		private static Bitmap insertArrow = Resources.InsertArrow;

		private readonly Timer scrollTimer = new Timer();

		private readonly Timer toolTipTimer = new Timer();

		private readonly ToolTip toolTip = new ToolTip();

		private Point clickPoint;

		private int inDrag = -1;

		private readonly TabBarItemCollection items = new TabBarItemCollection();

		private TabBarItem selectedTab;

		private bool showDropDown = true;

		private Bitmap closeImage;

		private int tabHeight;

		private int topPadding = 2;

		private int bottomPadding = 4;

		private bool drawBaseLine = true;

		private int leftIndent;

		private int minimumTabWidth = 250;

		private int markerPosition = -1;

		private bool showArrows;

		private ItemState leftArrowState;

		private ItemState rightArrowState;

		private ItemState dropDownState;

		private int tabsOffset;

		private Rectangle tabBounds;

		private int tabsWidth;

		private TabBarItem toolTipItem;

		private readonly Dictionary<TabBarItem, Image> animatedImages = new Dictionary<TabBarItem, Image>();

		private GestureHandler gestureHandler;

		public TabBarItemCollection Items => items;

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public TabBarItem SelectedTab
		{
			get
			{
				return selectedTab;
			}
			set
			{
				if (value != null)
				{
					value.State = TabItemState.Selected;
					return;
				}
				TabBarItem oldItem = selectedTab;
				selectedTab = null;
				items.ForEach(delegate(TabBarItem x)
				{
					if (x.State == TabItemState.Selected)
					{
						x.State = TabItemState.Normal;
					}
				});
				OnSelectedTabChanged(new SelectedTabChangedEventArgs(oldItem, null));
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public int SelectedTabIndex
		{
			get
			{
				return items.IndexOf(selectedTab);
			}
			set
			{
				if (value >= 0 && value < items.Count)
				{
					SelectedTab = items[value];
				}
			}
		}

		[DefaultValue(true)]
		public bool ShowDropDown
		{
			get
			{
				return showDropDown;
			}
			set
			{
				SetValue(ref showDropDown, value);
			}
		}

		[DefaultValue(null)]
		public Bitmap CloseImage
		{
			get
			{
				return closeImage;
			}
			set
			{
				SetValue(ref closeImage, value);
			}
		}

		[DefaultValue(0)]
		public int TabHeight
		{
			get
			{
				return tabHeight;
			}
			set
			{
				SetValue(ref tabHeight, value);
			}
		}

		[DefaultValue(2)]
		public int TopPadding
		{
			get
			{
				return topPadding;
			}
			set
			{
				SetValue(ref topPadding, value);
			}
		}

		[DefaultValue(4)]
		public int BottomPadding
		{
			get
			{
				return bottomPadding;
			}
			set
			{
				SetValue(ref bottomPadding, value);
			}
		}

		[DefaultValue(true)]
		public bool DrawBaseLine
		{
			get
			{
				return drawBaseLine;
			}
			set
			{
				SetValue(ref drawBaseLine, value);
			}
		}

		[DefaultValue(0)]
		public int LeftIndent
		{
			get
			{
				return leftIndent;
			}
			set
			{
				SetValue(ref leftIndent, value);
			}
		}

		[DefaultValue(false)]
		public bool OwnerDrawnTooltips
		{
			get;
			set;
		}

		[DefaultValue(250)]
		public int MinimumTabWidth
		{
			get
			{
				return minimumTabWidth;
			}
			set
			{
				SetValue(ref minimumTabWidth, value);
			}
		}

		[DefaultValue(-1)]
		public int MarkerPosition
		{
			get
			{
				return markerPosition;
			}
			set
			{
				SetValue(ref markerPosition, value);
			}
		}

		[DefaultValue(false)]
		public bool DragDropReorder
		{
			get;
			set;
		}

		private static Color BorderColor
		{
			get
			{
				Color result = SystemColors.AppWorkspace;
				if (Application.RenderWithVisualStyles)
				{
					VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.Tab.Pane.Normal);
					result = visualStyleRenderer.GetColor(ColorProperty.BorderColorHint);
				}
				return result;
			}
		}

		private bool ShowArrows
		{
			get
			{
				return showArrows;
			}
			set
			{
				if (showArrows != value)
				{
					showArrows = value;
					Invalidate();
				}
			}
		}

		private ItemState LeftArrowState
		{
			get
			{
				return leftArrowState;
			}
			set
			{
				if (leftArrowState != value)
				{
					leftArrowState = value;
					Invalidate(GetLeftArrowBounds(TabsRectangle));
				}
			}
		}

		private ItemState RightArrowState
		{
			get
			{
				return rightArrowState;
			}
			set
			{
				if (rightArrowState != value)
				{
					rightArrowState = value;
					Invalidate(GetRightArrowBounds(TabsRectangle));
				}
			}
		}

		private ItemState DropDownState
		{
			get
			{
				return dropDownState;
			}
			set
			{
				if (dropDownState != value)
				{
					dropDownState = value;
					Invalidate(GetDropDownBounds(TabsRectangle));
				}
			}
		}

		private int TabsOffset
		{
			get
			{
				return tabsOffset;
			}
			set
			{
				int num = value;
				int width = tabBounds.Width;
				if (num + tabsWidth < width)
				{
					num = width - tabsWidth;
				}
				if (num > 0)
				{
					num = 0;
				}
				if (tabsOffset != num)
				{
					tabsOffset = num;
					Invalidate();
				}
			}
		}

		[Browsable(true)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		private Rectangle TabsRectangle
		{
			get
			{
				if (base.Controls.Count == 0)
				{
					return base.ClientRectangle;
				}
				int num = base.Controls.Cast<Control>().Max((Control c) => (c.Dock == DockStyle.Left) ? c.Right : 0);
				int num2 = base.Controls.Cast<Control>().Min((Control c) => (c.Dock == DockStyle.Right) ? c.Left : 0);
				if (num != 0)
				{
					num += 20;
				}
				if (num2 != 0)
				{
					num2 -= 20;
				}
				return Rectangle.FromLTRB(num, 0, num2, base.ClientRectangle.Bottom);
			}
		}

		public event EventHandler<SelectedTabChangedEventArgs> SelectedTabChanged;

		public TabBar()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.Selectable, value: true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			items.Changed += items_Changed;
			AutoSize = true;
			AllowDrop = true;
			scrollTimer.Interval = 25;
			scrollTimer.Enabled = false;
			scrollTimer.Tick += scrollTimer_Tick;
			toolTipTimer.Interval = SystemInformation.MouseHoverTime;
			toolTipTimer.Enabled = false;
			toolTipTimer.Tick += toolTipTimer_Tick;
			toolTip.OwnerDraw = true;
			toolTip.Draw += toolTip_Draw;
			toolTip.Popup += toolTip_Popup;
		}

		protected override void OnCreateControl()
		{
			InitWindowsTouch();
			base.OnCreateControl();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				animatedImages.Values.ForEach(delegate(Image img)
				{
					ImageAnimator.StopAnimate(img, AnimationHandler);
				});
				scrollTimer.Dispose();
				toolTipTimer.Dispose();
				toolTip.Dispose();
			}
			base.Dispose(disposing);
		}

		public void EnsureVisible(TabBarItem item)
		{
			if (items.Contains(item))
			{
				Rectangle rectangle = GetTabBounds(TabsRectangle);
				Rectangle bounds = item.Bounds;
				if (bounds.Left < rectangle.Left)
				{
					TabsOffset += rectangle.Left - bounds.Left;
				}
				if (bounds.Right > rectangle.Right)
				{
					TabsOffset -= bounds.Right - rectangle.Right;
				}
			}
		}

		public bool SelectTab(int tab, int offset, bool rollover)
		{
			tab = tab.Clamp(0, items.Count - 1);
			TabBarItem[] array = items.Where((TabBarItem i) => i.Visible).ToArray();
			if (array.Length == 0)
			{
				return false;
			}
			int num = array.FindIndex((TabBarItem t) => t == items[tab]);
			if (num == -1)
			{
				num = ((tab >= array.Length) ? (array.Length - 1) : 0);
			}
			SelectedTab = array[rollover ? Numeric.Rollover(num, array.Length, offset) : num.Clamp(0, array.Length)];
			return true;
		}

		public bool SelectNextTab()
		{
			return SelectNextTab(rollOver: true);
		}

		public bool SelectNextTab(bool rollOver)
		{
			return SelectTab(SelectedTabIndex, 1, rollOver);
		}

		public bool SelectPreviousTab()
		{
			return SelectPreviousTab(rollOver: true);
		}

		public bool SelectPreviousTab(bool rollOver)
		{
			return SelectTab(SelectedTabIndex, -1, rollOver);
		}

		public void SelectFirstTab()
		{
			SelectTab(0, 0, rollover: false);
		}

		public void SelectLastTab()
		{
			SelectTab(items.Count - 1, 0, rollover: false);
		}

		private void scrollTimer_Tick(object sender, EventArgs e)
		{
			Rectangle tabsRectangle = TabsRectangle;
			Point pt = PointToClient(Cursor.Position);
			if (GetRightArrowBounds(tabsRectangle).Contains(pt))
			{
				TabsOffset -= 20;
			}
			else if (GetLeftArrowBounds(tabsRectangle).Contains(pt))
			{
				TabsOffset += 20;
			}
		}

		private void toolTipTimer_Tick(object sender, EventArgs e)
		{
			toolTipTimer.Stop();
			ShowToolTip(always: true);
		}

		private void items_Changed(object sender, SmartListChangedEventArgs<TabBarItem> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				e.Item.State = TabItemState.Normal;
				e.Item.Changed += Item_Changed;
				e.Item.Selected += Item_Selected;
				ImageAnimate(e.Item, enable: true);
				break;
			case SmartListAction.Remove:
				e.Item.Changed -= Item_Changed;
				e.Item.Selected -= Item_Selected;
				if (selectedTab == e.Item)
				{
					SelectedTab = null;
				}
				e.Item.InvokeRemoved();
				ImageAnimate(e.Item, enable: false);
				break;
			}
			Invalidate();
		}

		private void Item_Selected(object sender, CancelEventArgs e)
		{
			TabBarItem tabBarItem = selectedTab;
			TabBarItem newItem = sender as TabBarItem;
			SelectedTabChangedEventArgs selectedTabChangedEventArgs = new SelectedTabChangedEventArgs(selectedTab, newItem);
			selectedTab = newItem;
			try
			{
				OnSelectedTabChanged(selectedTabChangedEventArgs);
			}
			finally
			{
				selectedTab = tabBarItem;
			}
			e.Cancel = selectedTabChangedEventArgs.Cancel;
		}

		private void Item_Changed(object sender, EventArgs e)
		{
			TabBarItem tbi = (TabBarItem)sender;
			ImageAnimate(tbi, enable: true);
			if (tbi.State == TabItemState.Selected)
			{
				selectedTab = tbi;
				items.ForEach(delegate(TabBarItem x)
				{
					if (x != tbi)
					{
						x.State = TabItemState.Normal;
					}
				});
				EnsureVisible(tbi);
			}
			Invalidate();
		}

		private void toolTip_Popup(object sender, PopupEventArgs e)
		{
			if (!OwnerDrawnTooltips)
			{
				return;
			}
			if (toolTipItem == null || !toolTipItem.ShowToolTip())
			{
				e.Cancel = true;
				return;
			}
			Size toolTipSize = toolTipItem.ToolTipSize;
			if (!toolTipSize.IsEmpty)
			{
				e.ToolTipSize = toolTipSize;
			}
		}

		private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
		{
			VisualStyleRenderer visualStyleRenderer = null;
			VisualStyleElement normal = VisualStyleElement.ToolTip.Standard.Normal;
			if (VisualStyleRenderer.IsSupported && VisualStyleRenderer.IsElementDefined(normal))
			{
				visualStyleRenderer = new VisualStyleRenderer(normal);
				visualStyleRenderer.DrawBackground(e.Graphics, e.Bounds);
			}
			else
			{
				e.DrawBackground();
				e.DrawBorder();
			}
			if (OwnerDrawnTooltips && toolTipItem != null && !toolTipItem.ToolTipSize.IsEmpty)
			{
				toolTipItem.DrawTooltip(e.Graphics, e.Bounds);
			}
			else
			{
				DrawTooltipText(visualStyleRenderer, e);
			}
		}

		private static void DrawTooltipText(VisualStyleRenderer vr, DrawToolTipEventArgs e)
		{
			if (vr == null)
			{
				e.DrawText();
				return;
			}
			using (FontDC dc = new FontDC(e.Graphics, e.Font))
			{
				Rectangle backgroundContentRectangle = vr.GetBackgroundContentRectangle(dc, e.Bounds);
				vr.DrawText(dc, backgroundContentRectangle, e.ToolTipText);
			}
		}

		private void SetValue<T>(ref T old, T value)
		{
			if (!object.Equals(old, value))
			{
				old = value;
				Invalidate();
			}
		}

		private void ShowTabDropDown(Point location)
		{
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			bool flag = false;
			int num = items.Count((TabBarItem x) => x.Visible);
			for (int i = 0; i < items.Count; i++)
			{
				TabBarItem tbi = items[i];
				if (tbi.ShowInDropDown && tbi.Visible)
				{
					if (!flag && i > 0 && tbi.Padding.Left != 0)
					{
						contextMenuStrip.Items.Add(new ToolStripSeparator());
					}
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(tbi.Text, tbi.Image, delegate
					{
						SelectedTab = tbi;
					});
					if (tbi.State == TabItemState.Selected)
					{
						toolStripMenuItem.Checked = true;
					}
					if (tbi.State == TabItemState.Disabled)
					{
						toolStripMenuItem.Enabled = false;
					}
					contextMenuStrip.Items.Add(toolStripMenuItem);
					flag = false;
					if (i < num - 1 && tbi.Padding.Right != 0)
					{
						contextMenuStrip.Items.Add(new ToolStripSeparator());
						flag = true;
					}
				}
			}
			if (contextMenuStrip.Items.Count == 0)
			{
				contextMenuStrip.Dispose();
			}
			else
			{
				contextMenuStrip.Show(PointToScreen(location), ToolStripDropDownDirection.BelowLeft);
			}
		}

		private void DrawButton(Graphics gr, Rectangle rc, Image image, ItemState state)
		{
			TabItemState tabItemState;
			switch (state)
			{
			case ItemState.None:
				tabItemState = TabItemState.Normal;
				break;
			default:
				tabItemState = TabItemState.Hot;
				break;
			case ItemState.Selected:
				tabItemState = TabItemState.Selected;
				break;
			}
			DrawTabItem(gr, rc, tabItemState, buttonMode: true);
			Rectangle rect = image.Size.Align(rc, System.Drawing.ContentAlignment.MiddleCenter);
			gr.DrawImage(image, rect);
		}

		private void DrawTabItem(Graphics gr, Rectangle rc, TabItemState tabItemState, bool buttonMode)
		{
			if (TabRenderer.IsSupported)
			{
				TabRenderer.DrawTabItem(gr, rc, tabItemState);
				if (buttonMode)
				{
					using (Pen pen = new Pen(BorderColor))
					{
						gr.DrawLine(pen, rc.Left, rc.Bottom - 1, rc.Right, rc.Bottom - 1);
					}
				}
				return;
			}
			Border3DSide border3DSide = Border3DSide.Left | Border3DSide.Top | Border3DSide.Right;
			Border3DStyle style = Border3DStyle.Raised;
			if (buttonMode)
			{
				if (tabItemState == TabItemState.Selected)
				{
					style = Border3DStyle.Sunken;
				}
				border3DSide |= Border3DSide.Bottom;
			}
			using (Brush brush = new SolidBrush((tabItemState == TabItemState.Selected) ? SystemColors.ControlLightLight : BackColor))
			{
				gr.FillRectangle(brush, rc);
			}
			ControlPaint.DrawBorder3D(gr, rc, style, border3DSide);
		}

		private void UpdateTabAndButtonStates(MouseEventArgs e)
		{
			Rectangle tabsRectangle = TabsRectangle;
			Point pt = e.Location;
			RightArrowState = GetItemState(GetRightArrowBounds(tabsRectangle), pt, e.Button);
			LeftArrowState = GetItemState(GetLeftArrowBounds(tabsRectangle), pt, e.Button);
			DropDownState = GetItemState(GetDropDownBounds(tabsRectangle), pt, e.Button);
			TabBarItem tbi = items.Find((TabBarItem x) => x.Bounds.Contains(pt));
			if (tbi != null && GetTabBounds(tabsRectangle).Contains(pt))
			{
				items.ForEach(delegate(TabBarItem x)
				{
					if (x.State != TabItemState.Selected)
					{
						x.State = ((x != tbi) ? TabItemState.Normal : TabItemState.Hot);
					}
					x.CloseButtonHot = false;
				});
				if (tbi.State != TabItemState.Selected)
				{
					tbi.State = TabItemState.Hot;
				}
				if (tbi.CloseBounds.Contains(pt))
				{
					tbi.CloseButtonHot = true;
				}
				return;
			}
			items.ForEach(delegate(TabBarItem x)
			{
				if (x.State != TabItemState.Selected)
				{
					x.State = TabItemState.Normal;
				}
				x.CloseButtonHot = false;
			});
		}

		private void ShowToolTip(bool always, TabBarItem item)
		{
			Point pt = PointToClient(Cursor.Position);
			item = item ?? items.Find((TabBarItem x) => x.Bounds.Contains(pt));
			if (item == null)
			{
				HideToolTip();
			}
			else if (item != toolTipItem && (always || toolTip.Active))
			{
				toolTipItem = item;
				toolTip.Show(item.ToolTipText, this, new Point(item.Bounds.Left, item.Bounds.Bottom));
			}
		}

		private void ShowToolTip(bool always)
		{
			ShowToolTip(always, null);
		}

		private void HideToolTip()
		{
			toolTip.Hide(this);
			toolTipItem = null;
		}

		private void ImageAnimate(TabBarItem tbi, bool enable)
		{
			using (ItemMonitor.Lock(animatedImages))
			{
				if (animatedImages.TryGetValue(tbi, out var value))
				{
					if (enable && tbi.Image == value)
					{
						return;
					}
					ImageAnimator.StopAnimate(value, AnimationHandler);
					animatedImages.Remove(tbi);
				}
				if (enable && tbi.Image != null && ImageAnimator.CanAnimate(tbi.Image))
				{
					ImageAnimator.Animate(tbi.Image, AnimationHandler);
					animatedImages[tbi] = tbi.Image;
				}
			}
		}

		private void AnimationHandler(object sender, EventArgs e)
		{
			Image img = sender as Image;
			ImageAnimator.UpdateFrames(img);
			using (ItemMonitor.Lock(animatedImages))
			{
				TabBarItem tabBarItem = animatedImages.Keys.FirstOrDefault((TabBarItem tbi) => tbi.Image == img);
				if (tabBarItem != null)
				{
					Invalidate(tabBarItem.Bounds);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			try
			{
				Rectangle clientRectangle = base.ClientRectangle;
				Rectangle rectangle = clientRectangle;
				DrawBackground(e.Graphics, rectangle);
				rectangle = rectangle.Pad(0, topPadding, 0, bottomPadding);
				if (drawBaseLine)
				{
					using (Pen pen = new Pen(BorderColor))
					{
						if (BottomPadding == 0)
						{
							e.Graphics.DrawLine(pen, rectangle.Left, rectangle.Bottom - 1, rectangle.Width, rectangle.Bottom - 1);
						}
						else
						{
							e.Graphics.DrawRectangle(pen, rectangle.Left, rectangle.Bottom - 1, rectangle.Width - 1, clientRectangle.Bottom - rectangle.Bottom);
						}
					}
				}
				rectangle = TabsRectangle.Pad(0, topPadding, 0, bottomPadding);
				tabBounds = GetTabBounds(rectangle);
				tabsWidth = LayoutTabs(TabsOffset, tabBounds, 0);
				if (tabsWidth > tabBounds.Width)
				{
					tabsWidth = LayoutTabs(TabsOffset, tabBounds, tabsWidth - tabBounds.Width);
				}
				ShowArrows = tabsWidth > tabBounds.Width;
				using (e.Graphics.SaveState())
				{
					e.Graphics.IntersectClip(rectangle);
					using (e.Graphics.SaveState())
					{
						e.Graphics.IntersectClip(tabBounds.Pad(0, 0, -1));
						items.ForEach(delegate(TabBarItem x)
						{
							DrawTab(e.Graphics, x);
						});
						if (MarkerPosition >= 0)
						{
							TabBarItem[] array = items.Where((TabBarItem x) => x.Visible).ToArray();
							if (array.Length != 0)
							{
								DrawMarker(rc: new Rectangle(((MarkerPosition < array.Length) ? array[MarkerPosition].Bounds.Left : array[array.Length - 1].Bounds.Right) - 2, rectangle.Top, 4, rectangle.Height), gr: e.Graphics);
							}
						}
					}
					DrawArrows(e.Graphics, rectangle);
					DrawDropDown(e.Graphics, rectangle);
				}
			}
			catch (Exception)
			{
				Invalidate();
			}
		}

		private static ItemState GetItemState(Rectangle rc, Point pt, MouseButtons mb)
		{
			if (!rc.Contains(pt))
			{
				return ItemState.None;
			}
			if ((mb & MouseButtons.Left) == 0)
			{
				return ItemState.Hot;
			}
			return ItemState.Selected;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			int num = items.Where((TabBarItem x) => x.Visible).FindIndex((TabBarItem x) => x.Bounds.Contains(e.Location));
			if (DragDropReorder && inDrag < 0 && !clickPoint.IsEmpty && items.Count((TabBarItem x) => x.Visible) > 1)
			{
				Point point = e.Location.Subtract(clickPoint);
				if (Math.Abs(point.X) > SystemInformation.DragSize.Width / 2 || Math.Abs(point.Y) > SystemInformation.DragSize.Height / 2)
				{
					inDrag = num;
				}
			}
			if (inDrag >= 0)
			{
				if (num > inDrag)
				{
					num++;
				}
				MarkerPosition = num;
				return;
			}
			UpdateTabAndButtonStates(e);
			if (toolTipItem != null)
			{
				ShowToolTip(always: false);
			}
			else if (!e.Location.Equals(toolTipTimer.Tag))
			{
				toolTipTimer.Stop();
				toolTipTimer.Start();
				toolTipTimer.Tag = e.Location;
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			items.ForEach(delegate(TabBarItem x)
			{
				if (x.State != TabItemState.Selected)
				{
					x.State = TabItemState.Normal;
				}
				x.CloseButtonHot = false;
			});
			ItemState itemState2 = (DropDownState = ItemState.None);
			ItemState itemState5 = (RightArrowState = (LeftArrowState = itemState2));
			HideToolTip();
			toolTipTimer.Stop();
			toolTipTimer.Tag = null;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			scrollTimer.Enabled = true;
			UpdateTabAndButtonStates(e);
			clickPoint = e.Location;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			clickPoint = Point.Empty;
			scrollTimer.Enabled = false;
			if (inDrag >= 0)
			{
				TabBarItem[] array = items.Where((TabBarItem x) => x.Visible).ToArray();
				if (inDrag != MarkerPosition && MarkerPosition >= 0 && MarkerPosition <= array.Length)
				{
					TabBarItem item = array[inDrag];
					int newIndex = ((MarkerPosition < array.Length) ? items.IndexOf(array[MarkerPosition]) : items.Count);
					items.Move(items.IndexOf(item), newIndex);
				}
				MarkerPosition = -1;
				inDrag = -1;
				return;
			}
			UpdateTabAndButtonStates(e);
			Focus();
			Rectangle tabsRectangle = TabsRectangle;
			Point pt = PointToClient(Cursor.Position);
			Rectangle dropDownBounds = GetDropDownBounds(tabsRectangle);
			if (dropDownBounds.Contains(pt))
			{
				ShowTabDropDown(new Point(dropDownBounds.Right, dropDownBounds.Bottom));
			}
			else
			{
				if (!GetTabBounds(tabsRectangle).Contains(pt))
				{
					return;
				}
				TabBarItem tabBarItem = items.Find((TabBarItem x) => x.Visible && x.Bounds.Contains(pt));
				if (tabBarItem == null)
				{
					return;
				}
				if ((e.Button & MouseButtons.Left) != 0)
				{
					if (tabBarItem.CloseBounds.Contains(pt))
					{
						tabBarItem.InvokeCloseClick();
						return;
					}
					if (tabBarItem.InvokeCaptionClick())
					{
						return;
					}
					tabBarItem.State = TabItemState.Selected;
					tabBarItem.InvokeClick();
				}
				if ((e.Button & MouseButtons.Right) != 0 && tabBarItem.ContextMenu != null)
				{
					tabBarItem.ContextMenu.Show(PointToScreen(e.Location));
				}
				if ((e.Button & MouseButtons.Middle) != 0)
				{
					tabBarItem.InvokeCloseClick();
				}
			}
		}

		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
			case Keys.End:
			case Keys.Home:
			case Keys.Left:
			case Keys.Right:
				return true;
			default:
				return base.IsInputKey(keyData);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
			case Keys.Left:
				SelectPreviousTab();
				e.Handled = true;
				break;
			case Keys.Right:
				SelectNextTab();
				e.Handled = true;
				break;
			case Keys.End:
				SelectLastTab();
				e.Handled = true;
				break;
			case Keys.Home:
				SelectFirstTab();
				e.Handled = true;
				break;
			}
			base.OnKeyDown(e);
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (e.Delta > 0)
			{
				SelectPreviousTab();
			}
			else
			{
				SelectNextTab();
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			Invalidate();
		}

		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			Invalidate();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			TabsOffset = 0;
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			if (!AutoSize)
			{
				return base.GetPreferredSize(proposedSize);
			}
			proposedSize.Height = Font.Height + FormUtility.ScaleDpiY(12) + topPadding + bottomPadding;
			return proposedSize;
		}

		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			Rectangle rectangle = base.ClientRectangle.Pad(MinimumTabWidth, 0, 0, bottomPadding + 1);
			IEnumerable<ToolStrip> enumerable = base.Controls.OfType<ToolStrip>();
			if (enumerable.Count() == 0)
			{
				return;
			}
			int num = rectangle.Right;
			Size size = rectangle.Size;
			foreach (ToolStrip item in enumerable)
			{
				if (item.AutoSize)
				{
					Size preferredSize = item.GetPreferredSize(size);
					preferredSize.Height = rectangle.Height - 1;
					item.Size = preferredSize;
				}
				if (item.Dock == DockStyle.Right)
				{
					num -= item.Width;
					item.Location = new Point(num, 1);
				}
				else
				{
					item.Location = new Point(0, 1);
				}
				size.Width -= item.Width;
			}
			Invalidate();
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			Point pt = PointToClient(new Point(drgevent.X, drgevent.Y));
			TabBarItem tabBarItem = items.Find((TabBarItem x) => x.Bounds.Contains(pt));
			if (tabBarItem != null)
			{
				SelectedTab = tabBarItem;
			}
		}

		private Rectangle GetTabBounds(Rectangle rc, bool includeArrows = true)
		{
			if (tabHeight > FormUtility.ScaleDpiY(8))
			{
				rc = rc.Pad(0, rc.Height - tabHeight);
			}
			if (ShowArrows && includeArrows)
			{
				rc = rc.Pad(arrowWidth, 0, arrowWidth);
			}
			if (showDropDown)
			{
				rc = rc.Pad(0, 0, dropDownWidth);
			}
			return rc;
		}

		private Rectangle GetLeftArrowBounds(Rectangle rc)
		{
			if (!ShowArrows)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(rc.Left, rc.Top, arrowWidth, rc.Height);
		}

		private Rectangle GetRightArrowBounds(Rectangle rc)
		{
			if (!ShowArrows)
			{
				return Rectangle.Empty;
			}
			return new Rectangle(rc.Right - GetDropDownBounds(rc).Width + 1 - arrowWidth, rc.Top, arrowWidth, rc.Height);
		}

		private Rectangle GetDropDownBounds(Rectangle rc)
		{
			if (!ShowArrows || !ShowDropDown || !items.Exists((TabBarItem x) => x.ShowInDropDown))
			{
				return Rectangle.Empty;
			}
			return new Rectangle(rc.Right - dropDownWidth, rc.Top, dropDownWidth, rc.Height);
		}

		private int LayoutTabs(int offset, Rectangle rc, int decreaseSize)
		{
			int result = 0;
			int num = rc.Left + offset;
			int num2 = num + leftIndent;
			float num3 = 1f;
			if (decreaseSize > 0)
			{
				int num4 = items.Where((TabBarItem tbi) => tbi.AdjustWidth && tbi.Bounds.Width > FormUtility.ScaleDpiX(tbi.MinimumWidth)).Sum((TabBarItem tbi) => tbi.Bounds.Width);
				if (num4 > 0)
				{
					float val = num4 - (decreaseSize + 10);
					num3 = Math.Max(0f, val) / (float)num4;
				}
				else
				{
					num3 = 0f;
				}
			}
			foreach (TabBarItem item in items)
			{
				if (!item.Visible)
				{
					continue;
				}
				num2 += item.Padding.Left;
				int num5 = (item.ShowText ? TextRenderer.MeasureText(item.Text, item.GetFont(Font), new Size(int.MaxValue, int.MaxValue), item.TextFormat).Width : 0) + 4;
				num5 += 2;
				num5 += FormUtility.ScaleDpiX(item.ImageSize.Width);
				if (item.CanClose && closeImage != null)
				{
					num5 += FormUtility.ScaleDpiX(closeImage.Width) + 4;
				}
				if (item.AdjustWidth)
				{
					num5 = (int)((float)num5 * num3);
					if (num5 < FormUtility.ScaleDpiX(item.MinimumWidth))
					{
						num5 = FormUtility.ScaleDpiX(item.MinimumWidth);
					}
				}
				item.Bounds = new Rectangle(num2, rc.Top, num5, rc.Height + 1);
				if (item.State != TabItemState.Selected)
				{
					item.Bounds = item.Bounds.Pad(0, 2, 0, drawBaseLine ? 2 : 0);
				}
				if (item.CanClose && closeImage != null && (item.State == TabItemState.Hot || item.State == TabItemState.Selected))
				{
					item.CloseBounds = new Rectangle(item.Bounds.Right - FormUtility.ScaleDpiX(closeImage.Width) - 2, item.Bounds.Top + 2, FormUtility.ScaleDpiX(closeImage.Width), FormUtility.ScaleDpiY(closeImage.Height));
				}
				else
				{
					item.CloseBounds = Rectangle.Empty;
				}
				num2 += item.Bounds.Width - 1;
				result = num2 - num;
				num2 += item.Padding.Right;
			}
			return result;
		}

		protected virtual void DrawBackground(Graphics gr, Rectangle bounds)
		{
			using (Brush brush = new SolidBrush(BackColor))
			{
				gr.FillRectangle(brush, bounds);
			}
		}

		protected virtual void DrawTab(Graphics gr, TabBarItem item)
		{
			if (!item.Visible)
			{
				return;
			}
			Rectangle bounds = item.Bounds;
			DrawTabItem(gr, bounds, item.State, buttonMode: false);
			bounds = bounds.Pad(2, 2, 2);
			if (Focused && item.State == TabItemState.Selected)
			{
				ControlPaint.DrawFocusRectangle(gr, bounds);
			}
			bounds = bounds.Pad(1);
			if (item.Image != null)
			{
				try
				{
					gr.DrawImage(item.Image, bounds.Left, bounds.Top, FormUtility.ScaleDpiX(item.ImageSize.Width), FormUtility.ScaleDpiY(item.ImageSize.Height));
				}
				catch (Exception)
				{
				}
				bounds = bounds.Pad(FormUtility.ScaleDpiX(item.ImageSize.Width), 0);
			}
			bounds = bounds.Pad(0, 2, item.CloseBounds.Width);
			TextRenderer.DrawText(gr, item.Text, item.GetFont(Font), bounds, ForeColor, item.TextFormat);
			if (!item.CloseBounds.IsEmpty)
			{
				gr.DrawImage(closeImage, item.CloseBounds, new BitmapAdjustment(item.CloseButtonHot ? 0f : (-0.8f)));
			}
		}

		protected virtual void DrawArrows(Graphics gr, Rectangle rc)
		{
			if (ShowArrows)
			{
				DrawButton(gr, GetLeftArrowBounds(rc), arrowLeft, LeftArrowState);
				DrawButton(gr, GetRightArrowBounds(rc), arrowRight, RightArrowState);
			}
		}

		protected virtual void DrawDropDown(Graphics gr, Rectangle rc)
		{
			Rectangle dropDownBounds = GetDropDownBounds(rc);
			if (!dropDownBounds.IsEmpty)
			{
				DrawButton(gr, GetDropDownBounds(dropDownBounds), arrowDown, DropDownState);
			}
		}

		protected virtual void DrawMarker(Graphics gr, Rectangle rc)
		{
			gr.DrawImage(insertArrow, rc.Left + rc.Width / 2 - insertArrow.Width / 2, rc.Top);
		}

		protected virtual void OnSelectedTabChanged(SelectedTabChangedEventArgs e)
		{
			if (this.SelectedTabChanged != null)
			{
				this.SelectedTabChanged(this, e);
			}
		}

		private void InitWindowsTouch()
		{
			try
			{
				gestureHandler = Factory.CreateHandler<GestureHandler>(this);
				gestureHandler.DisableGutter = true;
				gestureHandler.Pan += gestureHandler_Pan;
			}
			catch (Exception)
			{
			}
		}

		private void gestureHandler_Pan(object sender, GestureEventArgs e)
		{
			TabsOffset += e.PanTranslation.Width;
		}
	}
}
