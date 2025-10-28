using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class FastScrollControl : UserControlEx
	{
		private int lineHeight = 16;

		private int columnWidth = 16;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollPositionX
		{
			get
			{
				return base.AutoScrollPosition.X;
			}
			set
			{
				base.AutoScrollPosition = new Point(value, -base.AutoScrollPosition.Y);
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ScrollPositionY
		{
			get
			{
				return -base.AutoScrollPosition.Y;
			}
			set
			{
				base.AutoScrollPosition = new Point(base.AutoScrollPosition.X, value);
			}
		}

		[DefaultValue(typeof(Point), "0, 0")]
		public Point ScrollPosition
		{
			get
			{
				return new Point(base.AutoScrollPosition.X, -base.AutoScrollPosition.Y);
			}
			set
			{
				base.AutoScrollPosition = new Point(value.X, value.Y);
			}
		}

		[DefaultValue(typeof(Size), "0, 0")]
		public Size VirtualSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		[DefaultValue(16)]
		public virtual int LineHeight
		{
			get
			{
				return lineHeight;
			}
			set
			{
				lineHeight = value;
			}
		}

		[DefaultValue(16)]
		public virtual int ColumnWidth
		{
			get
			{
				return columnWidth;
			}
			set
			{
				columnWidth = value;
			}
		}

		[DefaultValue(true)]
		public bool EnableStick
		{
			get;
			set;
		}

		public virtual Rectangle ViewRectangle
		{
			get
			{
				Rectangle displayRectangle = DisplayRectangle;
				displayRectangle.Offset(ScrollPosition);
				return displayRectangle;
			}
		}

		public event EventHandler<AutoScrollEventArgs> AutoScrolling;

		public event MouseEventHandler MouseHWheel;

		public FastScrollControl()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
		}

		protected override void OnScroll(ScrollEventArgs se)
		{
			base.OnScroll(se);
			OnScroll();
		}

		protected virtual void OnScroll()
		{
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
			if (this.AutoScrolling != null)
			{
				this.AutoScrolling(this, e);
			}
		}

		protected virtual void OnMouseHWheel(MouseEventArgs e)
		{
			if (this.MouseHWheel != null)
			{
				this.MouseHWheel(this, e);
			}
		}
	}
}
