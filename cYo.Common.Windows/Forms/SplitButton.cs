using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Win32;

namespace cYo.Common.Windows.Forms
{
	public class SplitButton : Button
	{
		private const int PushButtonWidth = 14;

		private static readonly int BorderSize = SystemInformation.Border3DSize.Width * 2;

		private bool skipNextOpen;

		private Rectangle dropDownRectangle;

		private bool showSplit = true;

		private PushButtonState state;

		private ContextMenuStrip oldContextMenu;

		[DefaultValue(true)]
		public bool ShowSplit
		{
			set
			{
				if (value != showSplit)
				{
					showSplit = value;
					Invalidate();
					base.Parent?.PerformLayout();
				}
			}
		}

		private PushButtonState State
		{
			get
			{
				return state;
			}
			set
			{
				if (state != value)
				{
					state = value;
					Invalidate();
				}
			}
		}

		public event EventHandler ShowContextMenu;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				RemoveContextEvents();
			}
			base.Dispose(disposing);
		}

		public override Size GetPreferredSize(Size proposedSize)
		{
			if (!AutoSize)
			{
				return base.Size;
			}
			Size preferredSize = base.GetPreferredSize(proposedSize);
			if (showSplit && !string.IsNullOrEmpty(Text) && TextRenderer.MeasureText(Text, Font).Width + FormUtility.ScaleDpiX(PushButtonWidth) > preferredSize.Width)
			{
				return preferredSize + new Size(FormUtility.ScaleDpiX(PushButtonWidth) + BorderSize * 2, 0);
			}
			return preferredSize;
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData == Keys.Down && showSplit)
			{
				return true;
			}
			return base.IsInputKey(keyData);
		}

		protected override void OnGotFocus(EventArgs e)
		{
			if (!showSplit)
			{
				base.OnGotFocus(e);
			}
			else if (State != PushButtonState.Pressed && State != PushButtonState.Disabled)
			{
				State = PushButtonState.Default;
			}
		}

		protected override void OnKeyDown(KeyEventArgs kevent)
		{
			if (showSplit)
			{
				if (kevent.KeyCode == Keys.Down)
				{
					ShowContextMenuStrip();
				}
				else if (kevent.KeyCode == Keys.Space && kevent.Modifiers == Keys.None)
				{
					State = PushButtonState.Pressed;
				}
			}
			base.OnKeyDown(kevent);
		}

		protected override void OnKeyUp(KeyEventArgs kevent)
		{
			if (kevent.KeyCode == Keys.Space && Control.MouseButtons == MouseButtons.None)
			{
				State = PushButtonState.Normal;
			}
			base.OnKeyUp(kevent);
		}

		protected override void OnLostFocus(EventArgs e)
		{
			if (!showSplit)
			{
				base.OnLostFocus(e);
			}
			else if (State != PushButtonState.Pressed && State != PushButtonState.Disabled)
			{
				State = PushButtonState.Normal;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!showSplit)
			{
				base.OnMouseDown(e);
			}
			else if (dropDownRectangle.Contains(e.Location))
			{
				ShowContextMenuStrip();
			}
			else
			{
				State = PushButtonState.Pressed;
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			if (!showSplit)
			{
				base.OnMouseEnter(e);
			}
			else if (State != PushButtonState.Pressed && State != PushButtonState.Disabled)
			{
				State = PushButtonState.Hot;
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			if (!showSplit)
			{
				base.OnMouseLeave(e);
			}
			else if (State != PushButtonState.Pressed && State != PushButtonState.Disabled)
			{
				State = ((!Focused) ? PushButtonState.Normal : PushButtonState.Default);
			}
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			if (!showSplit)
			{
				base.OnMouseUp(mevent);
			}
			else if (ContextMenuStrip == null || !ContextMenuStrip.Visible)
			{
				SetButtonDrawState();
				if (base.Bounds.Contains(base.Parent.PointToClient(Cursor.Position)) && !dropDownRectangle.Contains(mevent.Location))
				{
					OnClick(EventArgs.Empty);
				}
			}
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);
			if (!showSplit)
			{
				return;
			}
			Graphics graphics = pevent.Graphics;
			Rectangle clientRectangle = base.ClientRectangle;
			if (State != PushButtonState.Pressed && base.IsDefault && !Application.RenderWithVisualStyles)
			{
				Rectangle bounds = clientRectangle;
				bounds.Inflate(-1, -1);
				ButtonRenderer.DrawButton(graphics, bounds, State);
				graphics.DrawRectangle(SystemPens.WindowFrame, 0, 0, clientRectangle.Width - 1, clientRectangle.Height - 1);
			}
			else
			{
				ButtonRenderer.DrawButton(graphics, clientRectangle, State);
			}
			dropDownRectangle = new Rectangle(clientRectangle.Right - FormUtility.ScaleDpiX(PushButtonWidth) - 1, BorderSize, FormUtility.ScaleDpiX(PushButtonWidth), clientRectangle.Height - BorderSize * 2);
			int borderSize = BorderSize;
			Rectangle rectangle = new Rectangle(borderSize, borderSize, clientRectangle.Width - dropDownRectangle.Width - borderSize, clientRectangle.Height - borderSize * 2);
			bool flag = State == PushButtonState.Hot || State == PushButtonState.Pressed || !Application.RenderWithVisualStyles;
			if (RightToLeft == RightToLeft.Yes)
			{
				dropDownRectangle.X = clientRectangle.Left + 1;
				rectangle.X = dropDownRectangle.Right;
				if (flag)
				{
					graphics.DrawLine(SystemPens.ButtonShadow, clientRectangle.Left + FormUtility.ScaleDpiX(PushButtonWidth), BorderSize, clientRectangle.Left + FormUtility.ScaleDpiX(PushButtonWidth), clientRectangle.Bottom - BorderSize);
					graphics.DrawLine(SystemPens.ButtonFace, clientRectangle.Left + FormUtility.ScaleDpiX(PushButtonWidth) + 1, BorderSize, clientRectangle.Left + FormUtility.ScaleDpiX(PushButtonWidth) + 1, clientRectangle.Bottom - BorderSize);
				}
			}
			else if (flag)
			{
				graphics.DrawLine(SystemPens.ButtonShadow, clientRectangle.Right - FormUtility.ScaleDpiX(PushButtonWidth), BorderSize, clientRectangle.Right - FormUtility.ScaleDpiX(PushButtonWidth), clientRectangle.Bottom - BorderSize);
				graphics.DrawLine(SystemPens.ButtonFace, clientRectangle.Right - FormUtility.ScaleDpiX(PushButtonWidth) - 1, BorderSize, clientRectangle.Right - FormUtility.ScaleDpiX(PushButtonWidth) - 1, clientRectangle.Bottom - BorderSize);
			}
			PaintArrow(graphics, dropDownRectangle);
			if (!string.IsNullOrEmpty(Text))
			{
				TextFormatFlags textFormatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;
				if (!base.UseMnemonic)
				{
					textFormatFlags |= TextFormatFlags.NoPrefix;
				}
				else if (!ShowKeyboardCues)
				{
					textFormatFlags |= TextFormatFlags.HidePrefix;
				}
				if (base.AutoEllipsis)
				{
					textFormatFlags |= TextFormatFlags.EndEllipsis;
				}
				if (Application.RenderWithVisualStyles)
				{
					VisualStyleRenderer visualStyleRenderer = new VisualStyleRenderer(base.Enabled ? VisualStyleElement.Button.PushButton.Default : VisualStyleElement.Button.PushButton.Disabled);
					using (FontDC dc = new FontDC(graphics, Font))
					{
						visualStyleRenderer.DrawText(dc, rectangle, Text, drawDisabled: true, textFormatFlags);
					}
				}
				else if (base.Enabled)
				{
					TextRenderer.DrawText(graphics, Text, Font, rectangle, ForeColor, textFormatFlags);
				}
				else
				{
					ControlPaint.DrawStringDisabled(graphics, Text, Font, ForeColor, rectangle, textFormatFlags);
				}
			}
			if (State != PushButtonState.Pressed && Focused)
			{
				ControlPaint.DrawFocusRectangle(graphics, rectangle);
			}
		}

		protected override void OnContextMenuStripChanged(EventArgs e)
		{
			base.OnContextMenuStripChanged(e);
			RemoveContextEvents();
			oldContextMenu = ContextMenuStrip;
			if (oldContextMenu != null)
			{
				ContextMenuStrip.Opening += ContextMenuStrip_Opening;
				ContextMenuStrip.Closing += ContextMenuStrip_Closing;
			}
		}

		protected virtual void OnShowContextMenu()
		{
			if (this.ShowContextMenu != null)
			{
				this.ShowContextMenu(this, EventArgs.Empty);
			}
		}

		private void RemoveContextEvents()
		{
			if (oldContextMenu != null)
			{
				oldContextMenu.Opening -= ContextMenuStrip_Opening;
				oldContextMenu.Closing -= ContextMenuStrip_Closing;
			}
		}

		private static void PaintArrow(Graphics g, Rectangle dropDownRect)
		{
			Point point = new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2), Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));
			point.X += dropDownRect.Width % 2;
			int num = FormUtility.ScaleDpiX(2);
			int num2 = FormUtility.ScaleDpiX(3);
			int num3 = FormUtility.ScaleDpiY(1);
			int num4 = FormUtility.ScaleDpiY(2);
			Point[] points = new Point[3]
			{
				new Point(point.X - num, point.Y - num3),
				new Point(point.X + num2, point.Y - num3),
				new Point(point.X, point.Y + num4)
			};
			g.FillPolygon(SystemBrushes.ControlText, points);
		}

		private void ShowContextMenuStrip()
		{
			if (skipNextOpen)
			{
				skipNextOpen = false;
				return;
			}
			State = PushButtonState.Pressed;
			ContextMenuStrip?.Show(this, new Point(0, base.Height), ToolStripDropDownDirection.BelowRight);
		}

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			OnShowContextMenu();
		}

		private void ContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
		{
			SetButtonDrawState();
			if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
			{
				skipNextOpen = dropDownRectangle.Contains(PointToClient(Cursor.Position));
			}
		}

		private void SetButtonDrawState()
		{
			if (base.Bounds.Contains(base.Parent.PointToClient(Cursor.Position)))
			{
				State = PushButtonState.Hot;
			}
			else
			{
				State = ((!Focused) ? PushButtonState.Normal : PushButtonState.Default);
			}
		}
	}
}
