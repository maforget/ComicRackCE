using cYo.Common.Windows.Forms.Theme;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms
{
	public class CheckedListBoxEx : CheckedListBox
	{
		private bool customDrawing = true;

		private int downIndex = -1;

		private bool downCheck;

		private bool pendingCheck;

		[DefaultValue(true)]
		public bool CustomDrawing
		{
			get
			{
				return customDrawing;
			}
			set
			{
				customDrawing = value;
			}
		}

		public event DrawItemEventHandler DrawItemText;

        protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index >= base.Items.Count || e.Index < 0)
			{
				return;
			}
			if (!customDrawing)
			{
				base.OnDrawItem(e);
				return;
			}
			using (Brush brush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRectangle(brush, e.Bounds);
			}
			CheckState itemCheckState = GetItemCheckState(e.Index);
			Size size;
			if (Application.RenderWithVisualStyles)
			{
				CheckBoxState state = ((itemCheckState == CheckState.Unchecked || itemCheckState != CheckState.Checked) ? CheckBoxState.UncheckedNormal : CheckBoxState.CheckedNormal);
				size = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);
				Point glyphLocation = new Point(e.Bounds.X + 1, e.Bounds.Y + (e.Bounds.Height - size.Height) / 2);
				CheckBoxRenderer.DrawCheckBox(e.Graphics, glyphLocation, state);
			}
			else
			{
				ButtonState state2 = ((itemCheckState != 0 && itemCheckState == CheckState.Checked) ? ButtonState.Checked : ButtonState.Normal);
				size = new Size(14, 14);
				Rectangle rectangle = new Rectangle(e.Bounds.X + 1, e.Bounds.Y + (e.Bounds.Height - size.Height) / 2, size.Width, size.Height);
				ControlPaint.DrawCheckBox(e.Graphics, rectangle, state2);
			}
			Rectangle rectangle2 = new Rectangle(e.Bounds.X + size.Width + 2, e.Bounds.Y, e.Bounds.Width - (size.Width + 2), e.Bounds.Height);
			using (Brush brush2 = new SolidBrush(e.BackColor))
			{
				e.Graphics.FillRectangle(brush2, rectangle2);
			}
			OnDrawItemText(new DrawItemEventArgs(e.Graphics, e.Font, rectangle2, e.Index, e.State, e.ForeColor, e.BackColor));
			if ((e.State & DrawItemState.Focus) != 0 && (e.State & DrawItemState.NoFocusRect) == 0)
			{
				ControlPaintEx.DrawFocusRectangle(e.Graphics, rectangle2);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (!base.CheckOnClick)
			{
				downIndex = IndexFromPoint(e.Location);
				if (downIndex != -1)
				{
					Rectangle itemRectangle = GetItemRectangle(downIndex);
					itemRectangle.Width = 14;
					pendingCheck = itemRectangle.Contains(e.Location);
					downCheck = GetItemChecked(downIndex);
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (downIndex != -1 && pendingCheck)
			{
				SetItemChecked(downIndex, !downCheck);
			}
		}

		protected virtual void OnDrawItemText(DrawItemEventArgs e)
		{
			if (this.DrawItemText != null)
			{
				this.DrawItemText(this, e);
			}
			else
			{
				DrawDefaultItemText(e);
			}
		}

		public void DrawDefaultItemText(DrawItemEventArgs e)
		{
			using (StringFormat format = new StringFormat
			{
				LineAlignment = StringAlignment.Center
			})
			{
				DrawDefaultItemText(e, format);
			}
		}

		public virtual void DrawDefaultItemText(DrawItemEventArgs e, StringFormat format)
		{
			using (Brush brush = new SolidBrush(e.ForeColor))
			{
				e.Graphics.DrawString(GetItemText(base.Items[e.Index]), Font, brush, e.Bounds, format);
			}
		}
	}
}
