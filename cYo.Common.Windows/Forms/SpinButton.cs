using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Mathematics;
using cYo.Common.Text;
using cYo.Common.Win32;

namespace cYo.Common.Windows.Forms
{
	public class SpinButton : Control
	{
		public enum SpinButtonType
		{
			None,
			Up,
			Down
		}

		private Timer repeatTimer = new Timer();

		private bool upEnabled = true;

		private bool downEnabled = true;

		private bool flat = true;

		private SpinButtonType hit;

		private SpinButtonType hot;

		[DefaultValue(250)]
		public int RepeatInterval
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool UpEnabled
		{
			get
			{
				return upEnabled;
			}
			set
			{
				if (value != upEnabled)
				{
					upEnabled = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(true)]
		public bool DownEnabled
		{
			get
			{
				return downEnabled;
			}
			set
			{
				if (value != downEnabled)
				{
					downEnabled = value;
					Invalidate();
				}
			}
		}

		[DefaultValue(true)]
		public bool Flat
		{
			get
			{
				return flat;
			}
			set
			{
				if (value != flat)
				{
					flat = value;
					Invalidate();
				}
			}
		}

		public event EventHandler ButtonUp;

		public event EventHandler ButtonDown;

		public SpinButton()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, value: true);
			SetStyle(ControlStyles.Selectable, value: true);
			repeatTimer.Tick += repeatTimer_Tick;
			RepeatInterval = 250;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				repeatTimer.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (hit == SpinButtonType.None)
			{
				SpinButtonType spinButtonType = HitTest(e.Location);
				if (spinButtonType != hot)
				{
					hot = spinButtonType;
					Invalidate();
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (hot != 0)
			{
				hot = SpinButtonType.None;
				Invalidate();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Focus();
			hit = HitTest(e.Location);
			if (hit != 0)
			{
				Invalidate();
				OnButtonPressed(hit);
				if (RepeatInterval != 0)
				{
					repeatTimer.Interval = RepeatInterval;
					repeatTimer.Start();
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (hit != 0)
			{
				hit = SpinButtonType.None;
				repeatTimer.Stop();
				Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Draw(e.Graphics, base.ClientRectangle, styleMode: true, hit, hot, flat, base.Enabled && upEnabled, base.Enabled && downEnabled);
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData == Keys.Up || keyData == Keys.Down)
			{
				return true;
			}
			return base.IsInputKey(keyData);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			e.Handled = HandleKey(e.KeyCode);
			base.OnKeyDown(e);
		}

		protected virtual void OnButtonPressed(SpinButtonType button)
		{
			switch (button)
			{
			case SpinButtonType.Up:
				if (upEnabled)
				{
					OnButtonUp();
				}
				break;
			case SpinButtonType.Down:
				if (downEnabled)
				{
					OnButtonDown();
				}
				break;
			}
		}

		protected virtual void OnButtonUp()
		{
			if (this.ButtonUp != null)
			{
				this.ButtonUp(this, EventArgs.Empty);
			}
		}

		protected virtual void OnButtonDown()
		{
			if (this.ButtonDown != null)
			{
				this.ButtonDown(this, EventArgs.Empty);
			}
		}

		private void repeatTimer_Tick(object sender, EventArgs e)
		{
			OnButtonPressed(hit);
		}

		private bool IsUpKey(Keys keyCode)
		{
			switch (keyCode)
			{
			case Keys.Prior:
			case Keys.Up:
			case Keys.Add:
			case Keys.Oemplus:
				return true;
			default:
				return false;
			}
		}

		private bool IsDownKey(Keys keyCode)
		{
			switch (keyCode)
			{
			case Keys.Next:
			case Keys.Down:
			case Keys.Subtract:
			case Keys.OemMinus:
				return true;
			default:
				return false;
			}
		}

		private bool HandleKey(Keys keyCode)
		{
			if (IsUpKey(keyCode))
			{
				OnButtonPressed(SpinButtonType.Up);
				return true;
			}
			if (IsDownKey(keyCode))
			{
				OnButtonPressed(SpinButtonType.Down);
				return true;
			}
			return false;
		}

		private SpinButtonType HitTest(Point location)
		{
			return HitTest(base.ClientRectangle, location, upEnabled, downEnabled);
		}

		private Rectangle GetButtonBounds(SpinButtonType button)
		{
			return GetButtonBounds(base.ClientRectangle, button);
		}

		private static Rectangle GetButtonBounds(Rectangle rc, SpinButtonType button)
		{
			rc.Height /= 2;
			if (button == SpinButtonType.Down)
			{
				rc.Y += rc.Height;
			}
			return rc;
		}

		public static SpinButtonType HitTest(Rectangle rc, Point location, bool upEnabled = true, bool downEnabled = true)
		{
			if (GetButtonBounds(rc, SpinButtonType.Up).Contains(location) && upEnabled)
			{
				return SpinButtonType.Up;
			}
			if (GetButtonBounds(rc, SpinButtonType.Down).Contains(location) && downEnabled)
			{
				return SpinButtonType.Down;
			}
			return SpinButtonType.None;
		}

		public static void Draw(Graphics gr, Rectangle rc, bool styleMode = true, SpinButtonType hit = SpinButtonType.None, SpinButtonType hot = SpinButtonType.None, bool flat = true, bool upEnabled = true, bool downEnabled = true)
		{
			Rectangle buttonBounds = GetButtonBounds(rc, SpinButtonType.Up);
			Rectangle buttonBounds2 = GetButtonBounds(rc, SpinButtonType.Down);
			if (styleMode && Application.RenderWithVisualStyles)
			{
				VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer((!upEnabled) ? VisualStyleElement.Spin.Up.Disabled : ((hit == SpinButtonType.Up) ? VisualStyleElement.Spin.Up.Pressed : ((hot == SpinButtonType.Up) ? VisualStyleElement.Spin.Up.Hot : VisualStyleElement.Spin.Up.Normal)));
				visualStyleRenderer.DrawBackground(gr, buttonBounds);
				visualStyleRenderer = new VisualStyleRenderer((!downEnabled) ? VisualStyleElement.Spin.Down.Disabled : ((hit == SpinButtonType.Down) ? VisualStyleElement.Spin.Down.Pressed : ((hot == SpinButtonType.Down) ? VisualStyleElement.Spin.Down.Hot : VisualStyleElement.Spin.Down.Normal)));
				visualStyleRenderer.DrawBackground(gr, buttonBounds2);
			}
			else
			{
				ControlPaint.DrawScrollButton(gr, buttonBounds, ScrollButton.Up, (flat ? ButtonState.Flat : ButtonState.Normal) | ((!upEnabled) ? ButtonState.Inactive : ((hit == SpinButtonType.Up) ? ButtonState.Pushed : ButtonState.Normal)));
				ControlPaint.DrawScrollButton(gr, buttonBounds2, ScrollButton.Down, (flat ? ButtonState.Flat : ButtonState.Normal) | ((!downEnabled) ? ButtonState.Inactive : ((hit == SpinButtonType.Down) ? ButtonState.Pushed : ButtonState.Normal)));
			}
		}

		public static void AddUpDown(TextBoxBase textBox, int start = 1, int min = int.MinValue, int max = int.MaxValue, int increment = 1, bool registerKeys = false, bool hidden = false, bool visuallyLinkToParent = false, AnchorStyles anchorStyles = AnchorStyles.Top | AnchorStyles.Left)
		{
			SpinButton sb = new SpinButton
			{
				Width = FormUtility.ScaleDpiX(11),
				Enabled = textBox.Enabled,
				Visible = !hidden && textBox.Visible, // && textBox.IsVisibleSet(),    // Doesn't work correctly because IsVisibleSet() only works if an handle was already created.
				Anchor = anchorStyles
			};

			// We can only set this when the textbox is fully visible. It's parent might not be fully visible yet or may be hidden in another tab
			textBox.VisibleChanged += (s, e) => sb.Visible = !hidden && textBox.Visible;
			Action position = delegate
			{
				sb.Top = textBox.Top;
				sb.Height = textBox.Height;
				sb.Left = textBox.Right + 1;
			};
			textBox.Width -= sb.Width;
			textBox.Parent.Controls.Add(sb);
			textBox.Parent.Controls.SetChildIndex(sb, textBox.Parent.Controls.IndexOf(textBox) + 1);
			if (visuallyLinkToParent)
			{
				textBox.SizeChanged += delegate
				{
					if (textBox.Parent == sb.Parent)
					{
						position();
					}
				};
				textBox.LocationChanged += delegate
				{
					if (textBox.Parent == sb.Parent)
					{
						position();
					}
				};
				textBox.VisibleChanged += delegate
				{
					if (textBox.Parent == sb.Parent)
					{
						sb.Visible = textBox.IsVisibleSet();
					}
				};
				textBox.EnabledChanged += delegate
				{
					if (textBox.Parent == sb.Parent)
					{
						sb.Enabled = textBox.Enabled;
					}
				};
			}
			sb.ButtonUp += delegate
			{
				SetTextBoxValue(textBox, start, min, max, increment);
			};
			sb.ButtonDown += delegate
			{
				SetTextBoxValue(textBox, start, min, max, -increment);
			};
			position();
			if (registerKeys)
			{
				textBox.PreviewKeyDown += delegate(object s, PreviewKeyDownEventArgs e)
				{
					e.IsInputKey |= sb.IsUpKey(e.KeyCode) || sb.IsDownKey(e.KeyCode);
				};
				textBox.KeyDown += delegate(object s, KeyEventArgs e)
				{
					e.Handled = sb.HandleKey(e.KeyCode);
				};
			}
		}

		private static void TextBox_VisibleChanged(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private static void SetTextBoxValue(TextBoxBase textBox, int start, int min, int max, int increment)
		{
			int n;
			bool flag = textBox.Text.TryParse(out n, invariant: true);
			if (!flag && textBox is IPromptText)
			{
				flag = ((IPromptText)textBox).PromptText.TryParse(out n, invariant: true);
			}
			textBox.Text = ((!flag) ? start : (n + increment).Clamp(min, max)).ToString();
			textBox.SelectAll();
		}
	}
}
