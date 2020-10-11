#define TRACE
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Windows.Forms
{
	public class ScrollControl : Control
	{
		private readonly HScrollBar hScrollbar = new HScrollBar();

		private readonly VScrollBar vScrollBar = new VScrollBar();

		private readonly ThumbStickControl thumbStick = new ThumbStickControl();

		private readonly Timer scrollTimer = new Timer();

		private readonly Timer scrollEndTimer = new Timer();

		private bool hVisible;

		private bool vVisible;

		private ExtendedBorderStyle borderStyle = ExtendedBorderStyle.Flat;

		private AutoScrollMode autoScrollMode = AutoScrollMode.Drag;

		private bool enableStick = true;

		private int dragScrollRegion = 10;

		private int oldValue = -100000;

		private Size virtualSize;

		private bool blockOwnResize;

		private bool blockUpdateScrollbars;

		private Point panStart;

		private Point panStartScrollPosition;

		private Point scrollDelta;

		[DefaultValue(ExtendedBorderStyle.Flat)]
		public ExtendedBorderStyle BorderStyle
		{
			get
			{
				return borderStyle;
			}
			set
			{
				if (borderStyle != value)
				{
					borderStyle = value;
					UpdateScrollbars();
					Invalidate();
				}
			}
		}

		[DefaultValue(AutoScrollMode.Drag)]
		public AutoScrollMode AutoScrollMode
		{
			get
			{
				return autoScrollMode;
			}
			set
			{
				autoScrollMode = value;
			}
		}

		[DefaultValue(true)]
		public bool EnableStick
		{
			get
			{
				return enableStick;
			}
			set
			{
				if (enableStick != value)
				{
					enableStick = value;
				}
			}
		}

		[DefaultValue(null)]
		public Cursor PanCursor
		{
			get;
			set;
		}

		[DefaultValue(10)]
		public int DragScrollRegion
		{
			get
			{
				return dragScrollRegion;
			}
			set
			{
				dragScrollRegion = value;
			}
		}

		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle result = BorderUtility.AdjustBorder(base.DisplayRectangle, borderStyle);
				result.Width -= (vVisible ? vScrollBar.Width : 0);
				result.Height -= (hVisible ? hScrollbar.Height : 0);
				if (result.Width < 0)
				{
					result.Width = 0;
				}
				if (result.Height < 0)
				{
					result.Height = 0;
				}
				return result;
			}
		}

		public virtual Rectangle ViewRectangle => DisplayRectangle;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollPositionX
		{
			get
			{
				return hScrollbar.Value;
			}
			set
			{
				hScrollbar.Value = value.Clamp(hScrollbar.Minimum, hScrollbar.Maximum - hScrollbar.LargeChange);
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollPositionY
		{
			get
			{
				return vScrollBar.Value;
			}
			set
			{
				if (oldValue > value)
				{
					Trace.WriteLine("Scroll Underflow");
				}
				oldValue = value;
				vScrollBar.Value = value.Clamp(vScrollBar.Minimum, vScrollBar.Maximum - vScrollBar.LargeChange);
			}
		}

		[DefaultValue(typeof(Point), "0, 0")]
		public Point ScrollPosition
		{
			get
			{
				return new Point(ScrollPositionX, ScrollPositionY);
			}
			set
			{
				ScrollPositionX = value.X;
				ScrollPositionY = value.Y;
			}
		}

		[DefaultValue(typeof(Size), "0, 0")]
		public Size VirtualSize
		{
			get
			{
				return virtualSize;
			}
			set
			{
				if (!(virtualSize == value))
				{
					virtualSize = value;
					UpdateScrollbars();
				}
			}
		}

		[DefaultValue(16)]
		public virtual int LineHeight
		{
			get;
			set;
		} = 16;


		[DefaultValue(16)]
		public virtual int ColumnWidth
		{
			get;
			set;
		} = 16;


		[DefaultValue(false)]
		public bool ScrollResizeRefresh
		{
			get;
			set;
		}

		public bool InScrollOrResize
		{
			get;
			private set;
		}

		protected override bool ScaleChildren => false;

		public event EventHandler<AutoScrollEventArgs> AutoScrolling;

		public event EventHandler Scroll;

		public event EventHandler ViewResized;

		public ScrollControl()
		{
			SetStyle(ControlStyles.Selectable, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			scrollTimer.Interval = 20;
			scrollTimer.Tick += ScrollTimerTick;
			scrollTimer.Enabled = false;
			scrollEndTimer.Interval = 250;
			scrollEndTimer.Enabled = false;
			scrollEndTimer.Tick += delegate
			{
				scrollEndTimer.Stop();
				InScrollOrResize = false;
				Invalidate();
			};
			hScrollbar.TabStop = (vScrollBar.TabStop = (thumbStick.TabStop = false));
			hScrollbar.Visible = (vScrollBar.Visible = (thumbStick.Visible = true));
			hVisible = (vVisible = true);
			thumbStick.Sensitivity = new SizeF(32f, 32f);
			thumbStick.Acceleration = 4f;
			hScrollbar.ValueChanged += ScrollValueChanged;
			vScrollBar.ValueChanged += ScrollValueChanged;
			hScrollbar.VisibleChanged += ScrollbarVisibilityChanged;
			vScrollBar.VisibleChanged += ScrollbarVisibilityChanged;
			thumbStick.Scroll += ThumbStickScroll;
			base.Controls.Add(hScrollbar);
			base.Controls.Add(vScrollBar);
			base.Controls.Add(thumbStick);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				hScrollbar.Dispose();
				vScrollBar.Dispose();
				thumbStick.Dispose();
				scrollTimer.Dispose();
				scrollEndTimer.Dispose();
			}
			base.Dispose(disposing);
		}

		public Point Translate(Point pt, bool fromClient)
		{
			if (fromClient)
			{
				pt.Offset(ScrollPosition.X, ScrollPosition.Y);
				pt.Offset(-ViewRectangle.X, -ViewRectangle.Y);
			}
			else
			{
				pt.Offset(-ScrollPosition.X, -ScrollPosition.Y);
				pt.Offset(ViewRectangle.X, ViewRectangle.Y);
			}
			return pt;
		}

		public Rectangle Translate(Rectangle rc, bool fromClient)
		{
			rc.Location = Translate(rc.Location, fromClient);
			return rc;
		}

		protected virtual void OnAutoScrolling(AutoScrollEventArgs e)
		{
			this.AutoScrolling?.Invoke(this, e);
		}

		private void ScrollValueChanged(object sender, EventArgs e)
		{
			Invalidate();
			OnScroll();
		}

		private void ScrollbarVisibilityChanged(object sender, EventArgs e)
		{
			Invalidate();
		}

		private void ThumbStickScroll(object sender, EventArgs e)
		{
			if (vVisible)
			{
				vScrollBar.Value = (vScrollBar.Value + (int)(thumbStick.Movement.Y * (float)vScrollBar.LargeChange * 10f)).Clamp(vScrollBar.Minimum, vScrollBar.Maximum - vScrollBar.LargeChange);
			}
			if (hVisible)
			{
				hScrollbar.Value = (hScrollbar.Value + (int)(thumbStick.Movement.X * (float)hScrollbar.LargeChange * 10f)).Clamp(hScrollbar.Minimum, hScrollbar.Maximum - hScrollbar.LargeChange);
			}
		}

		protected virtual void OnScroll()
		{
			if (ScrollResizeRefresh)
			{
				InScrollOrResize = true;
				scrollEndTimer.Stop();
				scrollEndTimer.Start();
			}
			this.Scroll?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnViewResized()
		{
			if (ScrollResizeRefresh)
			{
				InScrollOrResize = true;
				scrollEndTimer.Stop();
				scrollEndTimer.Start();
			}
			OnResize(EventArgs.Empty);
			this.ViewResized?.Invoke(this, EventArgs.Empty);
		}

		protected virtual bool OnPanHitTest(MouseButtons buttons, Point location)
		{
			return true;
		}

		private void UpdateScrollbars()
		{
			if (blockUpdateScrollbars)
			{
				return;
			}
			blockUpdateScrollbars = true;
			try
			{
				for (int i = 0; i < 2; i++)
				{
					Rectangle viewRectangle = ViewRectangle;
					Rectangle viewRectangle2 = ViewRectangle;
					if (virtualSize.Width <= viewRectangle2.Width)
					{
						hVisible = false;
						hScrollbar.Value = 0;
						hScrollbar.Maximum = int.MaxValue;
					}
					else
					{
						hVisible = true;
						hScrollbar.Minimum = 0;
						hScrollbar.Maximum = virtualSize.Width;
						hScrollbar.LargeChange = viewRectangle2.Width;
						hScrollbar.SmallChange = ColumnWidth;
						hScrollbar.Value = hScrollbar.Value.Clamp(hScrollbar.Minimum, hScrollbar.Maximum - hScrollbar.LargeChange);
					}
					if (virtualSize.Height <= viewRectangle2.Height)
					{
						vVisible = false;
						vScrollBar.Value = 0;
						vScrollBar.Maximum = int.MaxValue;
					}
					else
					{
						vVisible = true;
						vScrollBar.Minimum = 0;
						vScrollBar.Maximum = virtualSize.Height;
						vScrollBar.LargeChange = viewRectangle2.Height;
						vScrollBar.SmallChange = LineHeight;
						vScrollBar.Value = vScrollBar.Value.Clamp(vScrollBar.Minimum, vScrollBar.Maximum - vScrollBar.LargeChange);
					}
					if (viewRectangle == ViewRectangle)
					{
						break;
					}
					blockOwnResize = true;
					try
					{
						OnViewResized();
					}
					finally
					{
						blockOwnResize = false;
					}
				}
			}
			finally
			{
				blockUpdateScrollbars = false;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			hScrollbar.Visible = hVisible;
			vScrollBar.Visible = vVisible;
			base.OnPaint(e);
			BorderUtility.DrawBorder(e.Graphics, base.DisplayRectangle, borderStyle);
			e.Graphics.IntersectClip(DisplayRectangle);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			if (blockOwnResize)
			{
				return;
			}
			Rectangle rectangle = BorderUtility.AdjustBorder(base.DisplayRectangle, borderStyle);
			UpdateScrollbars();
			hScrollbar.SetBounds(rectangle.Left, rectangle.Bottom - hScrollbar.Height, rectangle.Width - (vVisible ? vScrollBar.Width : 0), SystemInformation.HorizontalScrollBarHeight);
			vScrollBar.SetBounds(rectangle.Right - vScrollBar.Width, rectangle.Top, SystemInformation.VerticalScrollBarWidth, rectangle.Height - (hVisible ? hScrollbar.Height : 0));
			thumbStick.Width = vScrollBar.Width;
			thumbStick.Height = hScrollbar.Height;
			if (enableStick && (vVisible || hVisible))
			{
				thumbStick.Visible = true;
				thumbStick.Location = new Point(rectangle.Right - thumbStick.Width, rectangle.Bottom - thumbStick.Height);
				if (!vVisible || !hVisible)
				{
					if (vVisible)
					{
						vScrollBar.Height -= thumbStick.Height;
					}
					if (hVisible)
					{
						hScrollbar.Width -= thumbStick.Width;
					}
				}
			}
			else
			{
				thumbStick.Visible = false;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Focus();
			switch (autoScrollMode)
			{
			case AutoScrollMode.Pan:
				if (OnPanHitTest(e.Button, e.Location))
				{
					panStartScrollPosition = ScrollPosition;
					panStart = e.Location;
					if (PanCursor != null)
					{
						Cursor.Current = PanCursor;
					}
				}
				break;
			case AutoScrollMode.Drag:
				if (e.Button == MouseButtons.Left)
				{
					scrollDelta = GetDelta(DisplayRectangle, e.Location);
					scrollTimer.Start();
				}
				break;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (panStart.IsEmpty)
			{
				scrollDelta = GetDelta(DisplayRectangle, e.Location);
				return;
			}
			if (PanCursor != null)
			{
				Cursor.Current = PanCursor;
			}
			Point scrollPosition = panStartScrollPosition;
			scrollPosition.Offset(panStart);
			scrollPosition.Offset(-e.X, -e.Y);
			ScrollPosition = scrollPosition;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			panStart = Point.Empty;
			scrollTimer.Stop();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			scrollTimer.Stop();
		}

		protected override void OnDragOver(DragEventArgs e)
		{
			base.OnDragOver(e);
			scrollDelta = GetDelta(ViewRectangle.Pad(dragScrollRegion), PointToClient(new Point(e.X, e.Y)));
			scrollTimer.Start();
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
			scrollTimer.Stop();
		}

		private static Point GetDelta(Rectangle rc, Point pos)
		{
			Point empty = Point.Empty;
			if (pos.X < rc.Left)
			{
				empty.X = pos.X - rc.Left;
			}
			else if (pos.X > rc.Right)
			{
				empty.X = pos.X - rc.Right;
			}
			if (pos.Y < rc.Top)
			{
				empty.Y = pos.Y - rc.Top;
			}
			else if (pos.Y > rc.Bottom)
			{
				empty.Y = pos.Y - rc.Bottom;
			}
			return empty;
		}

		private void ScrollTimerTick(object sender, EventArgs e)
		{
			Point scrollPosition = ScrollPosition;
			Point delta = scrollDelta;
			int num = Math.Sign(delta.X);
			int num2 = Math.Sign(delta.Y);
			delta.X = Math.Abs(delta.X);
			delta.Y = Math.Abs(delta.Y);
			if (delta.X > 10)
			{
				delta.X = 10 + (delta.X - 10) / 10;
			}
			if (delta.Y > 10)
			{
				delta.Y = 10 + (delta.Y - 10) / 10;
			}
			delta.X *= num;
			delta.Y *= num2;
			AutoScrollEventArgs autoScrollEventArgs = new AutoScrollEventArgs
			{
				Delta = delta
			};
			OnAutoScrolling(autoScrollEventArgs);
			if (!autoScrollEventArgs.Cancel)
			{
				scrollPosition.Offset(autoScrollEventArgs.Delta);
				ScrollPosition = scrollPosition;
			}
		}
	}
}
