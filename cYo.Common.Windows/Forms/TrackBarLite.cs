using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Mathematics;
using cYo.Common.Windows.Forms.Theme;
using Microsoft.Win32;

namespace cYo.Common.Windows.Forms
{
	public class TrackBarLite : Control
	{
		private delegate void DrawHandler(Graphics gr, Rectangle rc);

		private bool enableVisualStyles = true;

		private bool enableFocusIndicator = true;

		private int minimum;

		private int maximum = 100;

		private int value;

		private int largeChange = 10;

		private int smallChange = 1;

		private int barThickness = 4;

		private int barMargin = 4;

		private Size thumbSize = new Size(12, 24);

		private TickStyle tickStyle;

		private int tickThickness = 4;

		private int tickFrequency = 10;

		private TrackBarThumbState trackBarThumbState = TrackBarThumbState.Normal;

		private DrawHandler DrawThumb;

		private DrawHandler DrawFocus;

		private DrawHandler DrawBar;

		private DrawHandler DrawTicks;

		private bool mouseDown;

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool EnableVisualStyles
		{
			get
			{
				return enableVisualStyles;
			}
			set
			{
				if (enableVisualStyles != value)
				{
					enableVisualStyles = value;
					SetDrawHandlers(enableVisualStyles);
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(true)]
		public bool EnableFocusIndicator
		{
			get
			{
				return enableFocusIndicator;
			}
			set
			{
				if (enableFocusIndicator != value)
				{
					enableFocusIndicator = value;
					Invalidate();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return minimum;
			}
			set
			{
				if (minimum != value)
				{
					minimum = value;
					Value = Value;
					Invalidate();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return maximum;
			}
			set
			{
				if (maximum != value)
				{
					maximum = value;
					Value = Value;
					Invalidate();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(0)]
		public int Value
		{
			get
			{
				return value;
			}
			set
			{
				value = Clamp(value);
				if (this.value != value)
				{
					Rectangle thumbRectangle = GetThumbRectangle();
					this.value = value;
					Rectangle thumbRectangle2 = GetThumbRectangle();
					if (thumbRectangle != thumbRectangle2)
					{
						Invalidate(thumbRectangle);
						Invalidate(thumbRectangle2);
						Update();
					}
					OnValueChanged();
				}
			}
		}

		[Category("Behavior")]
		[DefaultValue(10)]
		public int LargeChange
		{
			get
			{
				return largeChange;
			}
			set
			{
				largeChange = value;
			}
		}

		[Category("Behavior")]
		[DefaultValue(1)]
		public int SmallChange
		{
			get
			{
				return smallChange;
			}
			set
			{
				smallChange = value;
			}
		}

		[Category("Appearance")]
		[DefaultValue(4)]
		public int BarThickness
		{
			get
			{
				return barThickness;
			}
			set
			{
				if (barThickness != value)
				{
					barThickness = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(4)]
		public int BarMargin
		{
			get
			{
				return barMargin;
			}
			set
			{
				if (barMargin != value)
				{
					barMargin = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(typeof(Size), "12, 24")]
		public Size ThumbSize
		{
			get
			{
				return thumbSize;
			}
			set
			{
				if (!(thumbSize == value))
				{
					InvalidateThumb();
					thumbSize = value;
					InvalidateThumb();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(TickStyle.None)]
		public TickStyle TickStyle
		{
			get
			{
				return tickStyle;
			}
			set
			{
				if (tickStyle != value)
				{
					tickStyle = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(4)]
		public int TickThickness
		{
			get
			{
				return tickThickness;
			}
			set
			{
				if (tickThickness != value)
				{
					tickThickness = value;
					Invalidate();
				}
			}
		}

		[Category("Appearance")]
		[DefaultValue(10)]
		public int TickFrequency
		{
			get
			{
				return tickFrequency;
			}
			set
			{
				if (tickFrequency != value)
				{
					tickFrequency = value;
					Invalidate();
				}
			}
		}

		protected TrackBarThumbState TrackBarThumbState
		{
			get
			{
				return trackBarThumbState;
			}
			set
			{
				if (trackBarThumbState != value)
				{
					InvalidateThumb();
					trackBarThumbState = value;
					InvalidateThumb();
				}
			}
		}

		protected int TicksCount => (Maximum - Minimum) / tickFrequency + 1;

		[Category("Behavior")]
		public event EventHandler Scroll;

		[Category("Behavior")]
		public event EventHandler ValueChanged;

		public TrackBarLite()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
			SetStyle(ControlStyles.UserPaint, value: true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, value: true);
			SetStyle(ControlStyles.ResizeRedraw, value: true);
			SetStyle(ControlStyles.Selectable, value: true);
			SetDrawHandlers(visualStyles: true);
			SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
			}
			base.Dispose(disposing);
		}

		public void SetRange(int minimum, int maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		protected virtual void OnScroll()
		{
			if (this.Scroll != null)
			{
				this.Scroll(this, EventArgs.Empty);
			}
		}

		protected virtual void OnValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, EventArgs.Empty);
			}
		}

		public Rectangle GetBarRectangle(Rectangle rc)
		{
			rc.Inflate(-barMargin * 2, -barMargin * 2);
			return new Rectangle(rc.X, rc.Y + (rc.Height - barThickness) / 2, rc.Width, barThickness);
		}

		public Rectangle GetBarRectangle()
		{
			return GetBarRectangle(base.ClientRectangle);
		}

		public Rectangle GetTicksRectangle()
		{
			Rectangle barRectangle = GetBarRectangle();
			Rectangle result = barRectangle;
			int width = GetThumbRectangle().Width;
			result.X += width / 2;
			result.Width -= width;
			switch (tickStyle)
			{
			case TickStyle.TopLeft:
				result.Height = tickThickness;
				result.Y = barRectangle.Top - result.Height - BarMargin;
				break;
			case TickStyle.BottomRight:
				result.Height = tickThickness;
				result.Y = barRectangle.Bottom + BarMargin;
				break;
			default:
				return Rectangle.Empty;
			}
			return result;
		}

		public Rectangle GetThumbRectangle(Rectangle rc, Size sz)
		{
			if (minimum - maximum == 0)
			{
				return Rectangle.Empty;
			}
			Rectangle barRectangle = GetBarRectangle(rc);
			int num = barRectangle.Width - sz.Width;
			int y = barRectangle.Y + (barRectangle.Height - sz.Height) / 2;
			return new Rectangle(barRectangle.X + num * (value - minimum) / (maximum - minimum), y, sz.Width, sz.Height);
		}

		public Rectangle GetThumbRectangle(Rectangle rc)
		{
			return GetThumbRectangle(rc, ThumbSize.ScaleDpi());
		}

		public Rectangle GetThumbRectangle()
		{
			return GetThumbRectangle(base.ClientRectangle);
		}

		private int GetValueFromMouse(Rectangle rc, Point pt)
		{
			Rectangle barRectangle = GetBarRectangle(rc);
			Size size = ThumbSize.ScaleDpi();
			int num = maximum - minimum;
			int num2 = barRectangle.Width - size.Width / 2;
			int num3 = ((num != 0) ? (barRectangle.Width / num) : 0);
			return minimum + ((num2 != 0) ? ((pt.X - barRectangle.Left + num3 / 2) * num / num2) : 0);
		}

		private int GetValueFromMouse(Point pt)
		{
			return GetValueFromMouse(base.ClientRectangle, pt);
		}

		private int Clamp(int value)
		{
			return value.Clamp(minimum, maximum);
		}

		private void SetDrawHandlers(bool visualStyles)
		{
			if (visualStyles && TrackBarRenderer.IsSupported)
			{
				DrawThumb = DrawThumbWithVisualStyle;
				DrawFocus = DrawFocusWithVisualStyle;
				DrawBar = DrawBarWithVisualStyle;
				DrawTicks = DrawTicksWithVisualStyle;
			}
			else
			{
				DrawThumb = DrawThumbPlain;
				DrawFocus = DrawFocusPlain;
				DrawBar = DrawBarPlain;
				DrawTicks = DrawTicksPlain;
			}
		}

		private void InvalidateThumb()
		{
			Invalidate(GetThumbRectangle(base.ClientRectangle, ThumbSize.ScaleDpi()));
		}

		private void DrawThumbWithVisualStyle(Graphics gr, Rectangle rc)
		{
			try
			{
				TrackBarThumbState state = TrackBarThumbState;
				Size sz = ThumbSize.ScaleDpi();
				switch (tickStyle)
				{
				case TickStyle.BottomRight:
					TrackBarRenderer.DrawBottomPointingThumb(gr, GetThumbRectangle(rc, sz), state);
					break;
				case TickStyle.TopLeft:
					TrackBarRenderer.DrawTopPointingThumb(gr, GetThumbRectangle(rc, sz), state);
					break;
				default:
					TrackBarRenderer.DrawHorizontalThumb(gr, GetThumbRectangle(rc, sz), state);
					break;
				}
			}
			catch
			{
				DrawThumbPlain(gr, rc);
			}
		}

		private static void DrawFocusWithVisualStyle(Graphics gr, Rectangle rc)
		{
            //ControlPaint.DrawFocusRectangle(gr, rc);
            ControlPaintEx.DrawFocusRectangle(gr, rc);
        }

		private void DrawBarWithVisualStyle(Graphics gr, Rectangle rc)
		{
			try
			{
				TrackBarRenderer.DrawHorizontalTrack(gr, GetBarRectangle(rc));
			}
			catch
			{
				DrawBarPlain(gr, rc);
			}
		}

		private void DrawTicksWithVisualStyle(Graphics gr, Rectangle rc)
		{
			try
			{
				Rectangle ticksRectangle = GetTicksRectangle();
				ticksRectangle.Inflate(1, 0);
				TrackBarRenderer.DrawHorizontalTicks(gr, ticksRectangle, TicksCount, EdgeStyle.Bump);
			}
			catch
			{
				DrawTicksPlain(gr, rc);
			}
		}

		private void DrawThumbPlain(Graphics gr, Rectangle rc)
		{
			Size sz = ThumbSize.ScaleDpi();
			ButtonState state;
			switch (TrackBarThumbState)
			{
			case TrackBarThumbState.Disabled:
				state = ButtonState.Inactive;
				break;
			case TrackBarThumbState.Pressed:
				state = ButtonState.Pushed;
				break;
			default:
				state = ButtonState.Normal;
				break;
			}
			Rectangle thumbRectangle = GetThumbRectangle(rc, sz);
			if (thumbRectangle.Width >= 2 && thumbRectangle.Height >= 2)
			{
				ControlPaint.DrawButton(gr, thumbRectangle, state);
			}
		}

		private static void DrawFocusPlain(Graphics gr, Rectangle rc)
		{
            //ControlPaint.DrawFocusRectangle(gr, rc);
            ControlPaintEx.DrawFocusRectangle(gr, rc);
        }

		private void DrawBarPlain(Graphics gr, Rectangle rc)
		{
			Rectangle barRectangle = GetBarRectangle(rc);
			if (barRectangle.Width >= 2 && barRectangle.Height >= 2)
			{
                //ControlPaint.DrawBorder3D(gr, barRectangle, Border3DStyle.SunkenInner);
                ControlPaintEx.DrawBorder3D(gr, barRectangle, Border3DStyle.SunkenInner);
            }
		}

		private void DrawTicksPlain(Graphics gr, Rectangle rc)
		{
			if (TicksCount == 0)
			{
				return;
			}
			Rectangle ticksRectangle = GetTicksRectangle();
			using (Pen pen = new Pen(ForeColor))
			{
				for (int i = 0; i <= TicksCount; i++)
				{
					int num = ticksRectangle.Left + ticksRectangle.Width * i / TicksCount;
					gr.DrawLine(pen, num, ticksRectangle.Top, num, ticksRectangle.Bottom);
				}
			}
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			TrackBarThumbState = (base.Enabled ? TrackBarThumbState.Normal : TrackBarThumbState.Disabled);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
			TrackBarThumbState = TrackBarThumbState.Hot;
			Invalidate();
		}

		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			TrackBarThumbState = TrackBarThumbState.Normal;
			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Graphics graphics = e.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			DrawBar(graphics, clientRectangle);
			if (!GetTicksRectangle().IsEmpty)
			{
				DrawTicks(graphics, clientRectangle);
			}
			DrawThumb(graphics, clientRectangle);
			if (Focused && EnableFocusIndicator)
			{
				DrawFocus(graphics, clientRectangle);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Focus();
			mouseDown = true;
			HandleScroll(e.Location);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (mouseDown)
			{
				HandleScroll(e.Location);
			}
			else if (GetThumbRectangle().Contains(e.Location) || Focused)
			{
				TrackBarThumbState = TrackBarThumbState.Hot;
			}
			else
			{
				TrackBarThumbState = TrackBarThumbState.Normal;
			}
		}

		private void HandleScroll(Point pt)
		{
			TrackBarThumbState = TrackBarThumbState.Pressed;
			int num = Clamp(GetValueFromMouse(pt));
			if (num != Value)
			{
				Value = num;
				OnScroll();
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			mouseDown = false;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData == Keys.Left || keyData == Keys.Right)
			{
				return true;
			}
			return base.IsInputKey(keyData);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			int num = Value;
			switch (e.KeyCode)
			{
			case Keys.Left:
				num -= SmallChange;
				e.Handled = true;
				break;
			case Keys.Right:
				num += SmallChange;
				e.Handled = true;
				break;
			case Keys.Prior:
				num -= LargeChange;
				e.Handled = true;
				break;
			case Keys.Next:
				num += LargeChange;
				e.Handled = true;
				break;
			case Keys.Home:
				num = Minimum;
				e.Handled = true;
				break;
			case Keys.End:
				num = Maximum;
				e.Handled = true;
				break;
			default:
				base.OnKeyDown(e);
				break;
			}
			num = Clamp(num);
			if (Value != num)
			{
				Value = num;
				OnScroll();
			}
		}

		private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			SetDrawHandlers(visualStyles: true);
		}
	}
}
