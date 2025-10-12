using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;
using cYo.Common.Mathematics;

namespace cYo.Common.Windows.Forms
{
	public class ThumbStickControl : Control
	{
		private readonly Timer scrollTimer;

		private Image stickImage;

		private Image stickImagePressed;

		private Size stickSize = new Size(6, 6);

		private SizeF sensitivity = new SizeF(4f, 4f);

		private float accel = 1f;

		private bool autoScroll = true;

		private int autoScrollInterval = 20;

		private PointF movement;

		private Point clickPoint;

		[Category("Display")]
		[DefaultValue(null)]
		public Image StickImage
		{
			get
			{
				return stickImage;
			}
			set
			{
				if (stickImage != value)
				{
					stickImage = value;
					if (!IsMouseDown)
					{
						Invalidate();
					}
				}
			}
		}

		[Category("Display")]
		[DefaultValue(null)]
		public Image StickImagePressed
		{
			get
			{
				return stickImagePressed;
			}
			set
			{
				if (stickImagePressed != value)
				{
					stickImagePressed = value;
					if (IsMouseDown)
					{
						Invalidate();
					}
				}
			}
		}

		[Category("Display")]
		[DefaultValue(typeof(Size), "6, 6")]
		public Size StickSize
		{
			get
			{
				return stickSize;
			}
			set
			{
				if (!(stickSize == value))
				{
					stickSize = value;
					Invalidate();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(typeof(SizeF), "4f, 4f")]
		public SizeF Sensitivity
		{
			get
			{
				return sensitivity;
			}
			set
			{
				if (!(sensitivity == value))
				{
					sensitivity = value;
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(1f)]
		public float Acceleration
		{
			get
			{
				return accel;
			}
			set
			{
				if (accel != value)
				{
					accel = value;
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(true)]
		public bool AutoScroll
		{
			get
			{
				return autoScroll;
			}
			set
			{
				autoScroll = value;
			}
		}

		[Category("Behavior")]
		[DefaultValue(20)]
		public int AutoScrollInterval
		{
			get
			{
				return autoScrollInterval;
			}
			set
			{
				autoScrollInterval = value;
			}
		}

		[Browsable(false)]
		public PointF Movement
		{
			get
			{
				return movement;
			}
			protected set
			{
				if (!(value == movement))
				{
					movement = value;
					Invalidate();
					OnMovementChanged();
				}
			}
		}

		private bool IsMouseDown => base.Capture;

		[field: NonSerialized]
		public event EventHandler MovementChanged;

		[field: NonSerialized]
		public event EventHandler Scroll;

		public ThumbStickControl()
		{
			scrollTimer = new Timer
			{
				Interval = 250
			};
			scrollTimer.Tick += delegate
			{
				OnScroll();
			};
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				scrollTimer.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			DrawStick(pe.Graphics, Movement);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			clickPoint = e.Location;
			Movement = PointF.Empty;
			Invalidate();
			if (autoScroll)
			{
				scrollTimer.Interval = autoScrollInterval;
				scrollTimer.Start();
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (IsMouseDown)
			{
				float d = ((float)(e.X - clickPoint.X) / sensitivity.Width).Clamp(-1f, 1f);
				float d2 = ((float)(e.Y - clickPoint.Y) / sensitivity.Height).Clamp(-1f, 1f);
				Movement = new PointF(CalcAccel(d), CalcAccel(d2));
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Movement = PointF.Empty;
			Invalidate();
			scrollTimer.Stop();
		}

		protected virtual void OnMovementChanged()
		{
			if (this.MovementChanged != null)
			{
				this.MovementChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnScroll()
		{
			if (this.Scroll != null)
			{
				this.Scroll(this, EventArgs.Empty);
			}
		}

		private void DrawStick(Graphics g, PointF movement)
		{
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Inflate(-StickSize.Width / 2, -StickSize.Height / 2);
			PointF location = new PointF((float)displayRectangle.Left + (float)displayRectangle.Width / 2f + (float)(displayRectangle.Width - 1) / 2f * movement.X - (float)(StickSize.Width / 2), (float)displayRectangle.Top + (float)displayRectangle.Height / 2f + (float)(displayRectangle.Height - 1) / 2f * movement.Y - (float)(StickSize.Height / 2));
			RectangleF rect = new RectangleF(location, stickSize);
			using (g.AntiAlias())
			{
				if (IsMouseDown)
				{
					if (stickImagePressed == null)
					{
						g.FillEllipse(SystemBrushesEx.ControlDarkDark, rect);
					}
					else
					{
						g.DrawImage(stickImage, rect);
					}
				}
				else if (stickImage == null)
				{
					g.FillEllipse(SystemBrushesEx.ControlDark, rect);
				}
				else
				{
					g.DrawImage(stickImagePressed, rect);
				}
			}
		}

		private float CalcAccel(float d)
		{
			return (float)Math.Pow(Math.Abs(d), accel) * (float)Math.Sign(d);
		}
	}
}
