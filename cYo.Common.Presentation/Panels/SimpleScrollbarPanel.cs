using System;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Mathematics;

namespace cYo.Common.Presentation.Panels
{
	public class SimpleScrollbarPanel : OverlayPanel
	{
		private int borderWidth = 1;

		private Color borderColor = Color.LightGray;

		private Color backColor = Color.Black;

		private bool mirror;

		private Color knobColor = Color.White;

		private int value;

		private int minimum;

		private int maximum;

		private ScalableBitmap background;

		private ScalableBitmap knob;

		public int BorderWidth
		{
			get
			{
				return borderWidth;
			}
			set
			{
				if (borderWidth != value)
				{
					borderWidth = value;
					Invalidate();
				}
			}
		}

		public Color BorderColor
		{
			get
			{
				return borderColor;
			}
			set
			{
				if (!(borderColor == value))
				{
					borderColor = value;
					Invalidate();
				}
			}
		}

		public Color BackColor
		{
			get
			{
				return backColor;
			}
			set
			{
				if (!(backColor == value))
				{
					backColor = value;
					Invalidate();
				}
			}
		}

		public bool Mirror
		{
			get
			{
				return mirror;
			}
			set
			{
				if (mirror != value)
				{
					mirror = value;
					Invalidate();
				}
			}
		}

		public Color KnobColor
		{
			get
			{
				return knobColor;
			}
			set
			{
				if (!(knobColor == value))
				{
					knobColor = value;
					Invalidate();
				}
			}
		}

		public int Value
		{
			get
			{
				return value;
			}
			set
			{
				value = value.Clamp(minimum, maximum);
				if (this.value != value)
				{
					this.value = value;
					Invalidate();
					OnValueChanged();
				}
			}
		}

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
					Invalidate();
					OnMinimumChanged();
				}
			}
		}

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
					Invalidate();
					OnMaximumChanged();
				}
			}
		}

		public ScalableBitmap Background
		{
			get
			{
				return background;
			}
			set
			{
				if (background != value)
				{
					background = value;
					Invalidate();
				}
			}
		}

		public ScalableBitmap Knob
		{
			get
			{
				return knob;
			}
			set
			{
				if (knob != value)
				{
					knob = value;
					Invalidate();
				}
			}
		}

		public event EventHandler Scroll;

		public event EventHandler ValueChanged;

		public event EventHandler MinimumChanged;

		public event EventHandler MaximumChanged;

		public SimpleScrollbarPanel(Size sz)
			: base(sz)
		{
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			ScrollToPoint(e.Location);
			base.OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (base.PanelState == PanelState.Selected)
			{
				ScrollToPoint(e.Location);
			}
			base.OnMouseMove(e);
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			Invalidate();
		}

		private void ScrollToPoint(Point pt)
		{
			int num = (minimum + (pt.X - borderWidth) * (maximum - minimum + 1) / (base.Width - 2 * borderWidth)).Clamp(minimum, maximum);
			if (mirror)
			{
				num = maximum - (num - minimum);
			}
			if (num != Value)
			{
				Value = num;
				OnScroll();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			IBitmapRenderer gr = new BitmapGdiRenderer(graphics);
			Rectangle clientRectangle = base.ClientRectangle;
			Rectangle rectangle = clientRectangle;
			rectangle.Inflate(-borderWidth, -borderWidth);
			graphics.Clear(BackColor);
			if (background != null)
			{
				background.Draw(gr, rectangle);
			}
			else
			{
				using (Pen pen = new Pen(borderColor, borderWidth))
				{
					graphics.DrawRectangle(pen, rectangle);
				}
			}
			int num = maximum - minimum;
			if (num == 0)
			{
				return;
			}
			if (knob != null)
			{
				int num2 = clientRectangle.Height - 1;
				int num3 = knob.Bitmap.Width * num2 / knob.Bitmap.Height;
				int num4 = (value - minimum) * (clientRectangle.Width - num3) / num;
				num4 = ((!Mirror) ? (clientRectangle.Left + num4) : (clientRectangle.Right - num4 - num3));
				knob.Draw(gr, new Rectangle(num4, clientRectangle.Top, num3, num2));
			}
			else
			{
				using (Brush brush = new SolidBrush(knobColor))
				{
					int num5 = (value - minimum) * clientRectangle.Width / num;
					int num6 = (value + 1 - minimum) * clientRectangle.Width / num;
					int width = num6 - num5;
					graphics.FillRectangle(brush, num5, clientRectangle.Top, width, clientRectangle.Height);
				}
			}
		}

		protected virtual void OnValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnScroll()
		{
			if (this.Scroll != null)
			{
				this.Scroll(this, EventArgs.Empty);
			}
		}

		protected virtual void OnMinimumChanged()
		{
			if (this.MinimumChanged != null)
			{
				this.MinimumChanged(this, EventArgs.Empty);
			}
		}

		protected virtual void OnMaximumChanged()
		{
			if (this.MaximumChanged != null)
			{
				this.MaximumChanged(this, EventArgs.Empty);
			}
		}
	}
}
