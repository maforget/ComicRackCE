using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Windows.Forms
{
	public class SplitterEx : Control
	{
		private Control sibling;

		private Point clickOffset;

		[DefaultValue(null)]
		public Control Sibling
		{
			get
			{
				return sibling;
			}
			set
			{
				sibling = value;
			}
		}

		public bool IsHorizontal
		{
			get
			{
				if (Dock != DockStyle.Top)
				{
					return Dock == DockStyle.Bottom;
				}
				return true;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Rectangle displayRectangle = DisplayRectangle;
			displayRectangle.Width++;
			displayRectangle.Height++;
			if (base.Capture)
			{
				ControlPaint.DrawBorder3D(e.Graphics, displayRectangle, Border3DStyle.SunkenInner);
			}
			else
			{
				ControlPaint.DrawBorder3D(e.Graphics, displayRectangle, Border3DStyle.RaisedInner);
			}
		}

		protected static void DrawGrip(Graphics gr, Rectangle bounds, Pen pen, Brush brush)
		{
			for (int i = bounds.Left + 1; i < bounds.Right; i += 8)
			{
				Rectangle bounds2 = new Rectangle(i, 0, 6, bounds.Height);
				using (GraphicsPath path = PathUtility.GetArrowPath(bounds2, up: true))
				{
					gr.FillPath(brush, path);
					gr.DrawPath(pen, path);
				}
			}
		}

		protected override void OnDockChanged(EventArgs e)
		{
			base.OnDockChanged(e);
			Cursor = (IsHorizontal ? Cursors.HSplit : Cursors.VSplit);
		}

		private Control GetSibling(int offset)
		{
			if (sibling != null)
			{
				return sibling;
			}
			Control parent = base.Parent;
			int num = parent.Controls.IndexOf(this) + offset;
			if (num >= 0 && num < parent.Controls.Count)
			{
				return parent.Controls[num];
			}
			return null;
		}

		protected virtual void OnSplitterMoving(SplitterEventArgs sevent)
		{
			int num = 0;
			Control control;
			do
			{
				switch (Dock)
				{
				default:
					return;
				case DockStyle.Bottom:
				case DockStyle.Right:
					num++;
					break;
				case DockStyle.Top:
				case DockStyle.Left:
					num--;
					break;
				}
				control = GetSibling(num);
			}
			while (control != null && control.Dock != Dock);
			if (control == null)
			{
				return;
			}
			SuspendLayout();
			try
			{
				switch (Dock)
				{
				case DockStyle.Bottom:
					control.Height = control.Bottom - sevent.SplitY - base.Height;
					break;
				case DockStyle.Left:
					control.Width = sevent.SplitX - base.Width - control.Left;
					break;
				case DockStyle.Right:
					control.Width = control.Right - sevent.SplitX - base.Width;
					break;
				case DockStyle.Top:
					control.Height = sevent.SplitY - base.Height - control.Top;
					break;
				}
			}
			finally
			{
				ResumeLayout();
				base.Parent.Update();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			Invalidate();
			clickOffset = e.Location;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (base.Capture)
			{
				Point location = base.Location;
				Point location2 = e.Location;
				location2.X -= clickOffset.X;
				location2.Y -= clickOffset.Y;
				if (IsHorizontal)
				{
					location.Y += location2.Y;
				}
				else
				{
					location.X += location2.X;
				}
				OnSplitterMoving(new SplitterEventArgs(e.X, e.Y, location.X, location.Y));
			}
		}
	}
}
