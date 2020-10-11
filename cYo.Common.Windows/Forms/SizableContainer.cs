using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Runtime;
using cYo.Common.Windows.Properties;

namespace cYo.Common.Windows.Forms
{
	public class SizableContainer : ScrollableControl
	{
		public enum GripPosition
		{
			None,
			Top,
			Left,
			Right,
			Bottom
		}

		public enum GripStyle
		{
			None,
			Mozilla,
			Office
		}

		private enum GripState
		{
			None,
			Hoovered,
			Pressed
		}

		public static bool EnableAnimation;

		private Size dockSize;

		private bool autoGripPosition;

		private bool forceLayout = true;

		private int gripWidth = 6;

		private GripPosition grip = GripPosition.Top;

		private bool expanded = true;

		private bool shieldExpanded;

		private int expandedWidth;

		private bool keepHandleVisible = true;

		private readonly Color hotColor = SystemColors.ControlLight;

		private Color pressedColor = SystemColors.Highlight;

		private ExtendedBorderStyle borderStyle;

		private Point mousePressLocation;

		private Rectangle mousePressBounds;

		private int mousePressWidth;

		private bool first = true;

		private static readonly Bitmap gripImage = Resources.MozillaGrip;

		private GripState handleState;

		private bool inAnimation;

		private IContainer components;

		[DefaultValue(typeof(Size), "0, 0")]
		public Size DockSize
		{
			get
			{
				return dockSize;
			}
			set
			{
				if (!(dockSize == value))
				{
					dockSize = value;
					SetDockSize(value);
				}
			}
		}

		[DefaultValue(false)]
		public bool AutoGripPosition
		{
			get
			{
				return autoGripPosition;
			}
			set
			{
				if (autoGripPosition != value)
				{
					autoGripPosition = value;
					UpdateAutoGripPosition();
				}
			}
		}

		[DefaultValue(true)]
		public bool ForceLayout
		{
			get
			{
				return forceLayout;
			}
			set
			{
				forceLayout = value;
			}
		}

		[DefaultValue(6)]
		public int GripWidth
		{
			get
			{
				return gripWidth;
			}
			set
			{
				if (gripWidth != value)
				{
					gripWidth = value;
					PerformLayout();
				}
			}
		}

		[DefaultValue(GripPosition.Top)]
		public GripPosition Grip
		{
			get
			{
				return grip;
			}
			set
			{
				if (grip != value)
				{
					grip = value;
					PerformLayout();
				}
			}
		}

		public bool IsVertical
		{
			get
			{
				if (Grip != GripPosition.Right)
				{
					return Grip == GripPosition.Left;
				}
				return true;
			}
		}

		public bool IsHorizontal
		{
			get
			{
				if (Grip != GripPosition.Top)
				{
					return Grip == GripPosition.Bottom;
				}
				return true;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Expanded
		{
			get
			{
				return expanded;
			}
			set
			{
				if (shieldExpanded)
				{
					return;
				}
				shieldExpanded = true;
				try
				{
					if (expanded != value)
					{
						expanded = value;
						if (expanded)
						{
							OnExpandedChanged();
						}
						UpdateExpanded(AnimateExpand && base.IsHandleCreated);
						if (!expanded)
						{
							OnExpandedChanged();
						}
					}
				}
				finally
				{
					shieldExpanded = false;
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Collapsed
		{
			get
			{
				return !Expanded;
			}
			set
			{
				Expanded = !value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int ExpandedWidth
		{
			get
			{
				return expandedWidth;
			}
			set
			{
				if (expandedWidth != value)
				{
					SetExpandedWidthInternal(value);
					if (expanded)
					{
						SetExpandedWidth(expandedWidth);
					}
				}
			}
		}

		[DefaultValue(true)]
		public bool KeepGripVisible
		{
			get
			{
				return keepHandleVisible;
			}
			set
			{
				if (keepHandleVisible != value)
				{
					keepHandleVisible = value;
					if (!expanded)
					{
						SetExpandedWidth(0);
					}
				}
			}
		}

		[DefaultValue(true)]
		public bool AnimateExpand
		{
			get;
			set;
		}

		[DefaultValue(100)]
		public int SlideTime
		{
			get;
			set;
		}

		[DefaultValue(typeof(Color), "ControlLight")]
		public Color HotColor
		{
			get
			{
				return hotColor;
			}
			set
			{
				if (!(hotColor == value) && State == GripState.Hoovered)
				{
					Invalidate(GripRectangle);
				}
			}
		}

		[DefaultValue(typeof(Color), "Highlight")]
		public Color PressedColor
		{
			get
			{
				return pressedColor;
			}
			set
			{
				if (!(pressedColor == value))
				{
					pressedColor = value;
					if (State == GripState.Pressed)
					{
						Invalidate(GripRectangle);
					}
				}
			}
		}

		[DefaultValue(ExtendedBorderStyle.None)]
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
					PerformLayout();
					Invalidate();
				}
			}
		}

		public override Rectangle DisplayRectangle
		{
			get
			{
				Rectangle bounds = base.DisplayRectangle;
				switch (grip)
				{
				case GripPosition.Top:
					bounds = new Rectangle(bounds.Left, gripWidth, bounds.Width, bounds.Height - gripWidth);
					break;
				case GripPosition.Left:
					bounds = new Rectangle(gripWidth, bounds.Top, bounds.Width - gripWidth, bounds.Height);
					break;
				case GripPosition.Right:
					bounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width - gripWidth, bounds.Height);
					break;
				case GripPosition.Bottom:
					bounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height - gripWidth);
					break;
				}
				return BorderUtility.AdjustBorder(bounds, borderStyle);
			}
		}

		public virtual Rectangle GripRectangle
		{
			get
			{
				Rectangle clientRectangle = base.ClientRectangle;
				switch (grip)
				{
				default:
					return clientRectangle;
				case GripPosition.Top:
					return new Rectangle(clientRectangle.Left, clientRectangle.Top, clientRectangle.Width, gripWidth);
				case GripPosition.Left:
					return new Rectangle(clientRectangle.Left, clientRectangle.Top, gripWidth, clientRectangle.Height);
				case GripPosition.Right:
					return new Rectangle(clientRectangle.Width - gripWidth, clientRectangle.Top, gripWidth, clientRectangle.Height);
				case GripPosition.Bottom:
					return new Rectangle(clientRectangle.Left, clientRectangle.Height - gripWidth, clientRectangle.Width, gripWidth);
				}
			}
		}

		private GripState State
		{
			get
			{
				return handleState;
			}
			set
			{
				if (handleState != value)
				{
					handleState = value;
					switch (handleState)
					{
					default:
						Cursor = Cursors.Default;
						break;
					case GripState.Pressed:
						mousePressLocation = Cursor.Position;
						mousePressBounds = base.Bounds;
						mousePressWidth = ExpandedWidth;
						Cursor = (IsVertical ? Cursors.VSplit : Cursors.HSplit);
						break;
					case GripState.Hoovered:
						Cursor = (IsVertical ? Cursors.VSplit : Cursors.HSplit);
						break;
					}
					Invalidate(GripRectangle);
				}
			}
		}

		public event EventHandler ExpandedChanged;

		public event PaintEventHandler PaintGrip;

		public SizableContainer()
		{
			SlideTime = 100;
			AnimateExpand = true;
			InitializeComponent();
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			OnPaintGrip(e);
			Rectangle displayRectangle = DisplayRectangle;
			if (base.ClientRectangle.IntersectsWith(displayRectangle))
			{
				BorderUtility.DrawBorder(e.Graphics, BorderUtility.AdjustBorder(displayRectangle, borderStyle, inwards: false), borderStyle);
			}
		}

		private void DrawGrip(Graphics gr, Image image, Rectangle rc)
		{
			float num = 0f;
			if (Expanded)
			{
				num += 180f;
			}
			switch (Grip)
			{
			case GripPosition.Left:
				num -= 90f;
				break;
			case GripPosition.Right:
				num += 90f;
				break;
			case GripPosition.Bottom:
				num += 180f;
				break;
			}
			switch (State)
			{
			default:
			{
				using (Brush brush3 = new SolidBrush(BackColor))
				{
					gr.FillRectangle(brush3, GripRectangle);
				}
				break;
			}
			case GripState.Hoovered:
			{
				using (Brush brush2 = new SolidBrush(HotColor))
				{
					gr.FillRectangle(brush2, GripRectangle);
				}
				break;
			}
			case GripState.Pressed:
			{
				using (Brush brush = new SolidBrush(PressedColor))
				{
					gr.FillRectangle(brush, GripRectangle);
				}
				break;
			}
			}
			using (gr.SaveState())
			{
				gr.TranslateTransform(rc.Left + rc.Width / 2, rc.Top + rc.Height / 2);
				gr.RotateTransform(num);
				gr.TranslateTransform(-image.Width / 2, -image.Height / 2);
				gr.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height));
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			if (!inAnimation)
			{
				State = (HitTest() ? GripState.Hoovered : GripState.None);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!inAnimation && (e.Button & MouseButtons.Left) != 0 && HitTest(e.Location))
			{
				State = GripState.Pressed;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (inAnimation)
			{
				return;
			}
			if (State != GripState.Pressed)
			{
				State = (HitTest(e.Location) ? GripState.Hoovered : GripState.None);
				return;
			}
			Point position = Cursor.Position;
			Point point = mousePressLocation;
			int num;
			switch (Grip)
			{
			case GripPosition.Top:
				num = mousePressBounds.Height - (position.Y - point.Y);
				break;
			case GripPosition.Left:
				num = mousePressBounds.Width - (position.X - point.X);
				break;
			case GripPosition.Right:
				num = mousePressBounds.Width + (position.X - point.X);
				break;
			case GripPosition.Bottom:
				num = mousePressBounds.Height + (position.Y - point.Y);
				break;
			default:
				num = 0;
				break;
			}
			if (num < 0)
			{
				num = 0;
			}
			if (num != 0 && !expanded)
			{
				expanded = true;
				OnExpandedChanged();
			}
			if (num == 0 && Expanded)
			{
				expanded = false;
				SetExpandedWidthInternal(mousePressWidth);
				OnExpandedChanged();
			}
			SetExpandedWidth(num, clip: true, keepHandle: true);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!inAnimation && (e.Button & MouseButtons.Left) != 0)
			{
				State = (HitTest(e.Location) ? GripState.Hoovered : GripState.None);
				if (Math.Abs(Cursor.Position.X - mousePressLocation.X) < 4 && Math.Abs(Cursor.Position.Y - mousePressLocation.Y) < 4)
				{
					Expanded = !Expanded;
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!inAnimation && State != GripState.Pressed)
			{
				State = GripState.None;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (Dock == DockStyle.Fill)
			{
				OnExpandedChanged();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (!shieldExpanded && expanded && !base.DesignMode)
			{
				expandedWidth = (IsVertical ? base.Width : base.Height);
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			if (first && !base.DesignMode)
			{
				first = false;
				UpdateExpanded(animate: false);
			}
		}

		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
			UpdateAutoGripPosition();
			if (!Expanded)
			{
				SetExpandedWidth(0);
			}
			SetDockSize(dockSize);
		}

		protected virtual void OnPaintGrip(PaintEventArgs e)
		{
			DrawGrip(e.Graphics, gripImage, GripRectangle);
			if (this.PaintGrip != null)
			{
				this.PaintGrip(this, e);
			}
		}

		protected virtual void OnExpandedChanged()
		{
			if (this.ExpandedChanged != null)
			{
				this.ExpandedChanged(this, EventArgs.Empty);
			}
		}

		private void SetExpandedWidthInternal(int value)
		{
			expandedWidth = value;
			dockSize = GetDockSize(dockSize);
		}

		private Size GetDockSize(Size dockSize)
		{
			switch (Dock)
			{
			case DockStyle.Top:
			case DockStyle.Bottom:
				dockSize = new Size(dockSize.Width, ExpandedWidth);
				break;
			case DockStyle.Left:
			case DockStyle.Right:
				dockSize = new Size(ExpandedWidth, dockSize.Height);
				break;
			}
			return dockSize;
		}

		private void SetDockSize(Size dockSize)
		{
			switch (Dock)
			{
			case DockStyle.Top:
			case DockStyle.Bottom:
				if (dockSize.Height != 0)
				{
					ExpandedWidth = dockSize.Height;
				}
				break;
			case DockStyle.Left:
			case DockStyle.Right:
				if (dockSize.Width != 0)
				{
					ExpandedWidth = dockSize.Width;
				}
				break;
			case DockStyle.None:
			case DockStyle.Fill:
				break;
			}
		}

		private bool HitTest(Point pt)
		{
			return GripRectangle.Contains(pt);
		}

		private bool HitTest()
		{
			return HitTest(PointToClient(Cursor.Position));
		}

		private Rectangle GetExpandedBounds(int width)
		{
			Rectangle bounds = base.Bounds;
			switch (grip)
			{
			case GripPosition.Top:
				bounds.Y = bounds.Bottom - width;
				bounds.Height = width;
				break;
			case GripPosition.Left:
				bounds.X = bounds.Right - width;
				bounds.Width = width;
				break;
			case GripPosition.Right:
				bounds.Width = width;
				break;
			case GripPosition.Bottom:
				bounds.Height = width;
				break;
			}
			return bounds;
		}

		private void SetExpandedWidth(int width, bool clip, bool keepHandle)
		{
			width = Math.Max(keepHandle ? gripWidth : 0, width);
			Rectangle expandedBounds = GetExpandedBounds(width);
			SetBounds(expandedBounds, clip);
			dockSize = GetDockSize(dockSize);
		}

		private void SetExpandedWidth(int width, bool clip)
		{
			SetExpandedWidth(width, clip, keepHandleVisible);
		}

		private void SetExpandedWidth(int width)
		{
			SetExpandedWidth(width, clip: false);
		}

		private void UpdateExpanded(bool animate)
		{
			animate &= EnableAnimation;
			if (Dock == DockStyle.Fill)
			{
				base.Visible = expanded;
			}
			else if (expanded)
			{
				if (animate)
				{
					AnimateWindow(0, expandedWidth, SlideTime);
				}
				else
				{
					SetExpandedWidth(expandedWidth, clip: true);
				}
			}
			else if (animate)
			{
				AnimateWindow(expandedWidth, 0, SlideTime);
			}
			else
			{
				SetExpandedWidth(0, clip: true);
			}
		}

		private void AnimateWindow(int start, int end, int slideTime)
		{
			inAnimation = true;
			Size minimumSize = base.Controls[0].MinimumSize;
			try
			{
				int num = start;
				int num2 = end - start;
				long num3 = slideTime;
				long ticks = Machine.Ticks;
				if (base.Controls.Count == 1)
				{
					base.Controls[0].MinimumSize = ((num2 < 0) ? base.Controls[0].Size : GetExpandedBounds(end).Size);
				}
				while (num != end)
				{
					long num4 = Machine.Ticks - ticks;
					float num5 = Math.Min((float)num4 / (float)num3, 1f);
					num = start + (int)((float)num2 * num5);
					SetExpandedWidth(num, clip: true);
				}
			}
			finally
			{
				base.Controls[0].MinimumSize = minimumSize;
				inAnimation = false;
			}
		}

		private void SetBounds(Rectangle rc, bool clip)
		{
			Control parent = base.Parent;
			if (clip && parent != null)
			{
				rc = Rectangle.Intersect(parent.DisplayRectangle, rc);
			}
			if (rc == base.Bounds || rc.IsEmpty)
			{
				return;
			}
			base.Bounds = rc;
			if (parent != null)
			{
				parent.PerformLayout(this, "Bounds");
				if (forceLayout)
				{
					parent.Update();
				}
			}
		}

		private void UpdateAutoGripPosition()
		{
			if (autoGripPosition)
			{
				switch (Dock)
				{
				case DockStyle.Bottom:
					Grip = GripPosition.Top;
					break;
				case DockStyle.Left:
					Grip = GripPosition.Right;
					break;
				case DockStyle.Right:
					Grip = GripPosition.Left;
					break;
				case DockStyle.Top:
					Grip = GripPosition.Bottom;
					break;
				default:
					Grip = GripPosition.None;
					break;
				}
			}
		}

		private void InitializeComponent()
		{
			SuspendLayout();
			base.Name = "SplitContainer";
			base.Size = new System.Drawing.Size(336, 285);
			ResumeLayout(false);
		}
	}
}
