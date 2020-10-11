using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Presentation.Ceco.Builders;

namespace cYo.Common.Presentation.Ceco.Controls
{
	public class XHtmlControlRenderer : Component
	{
		private Control control;

		private readonly BodyBlock body = new BodyBlock();

		private Inline hotItem;

		public Control Control
		{
			get
			{
				return control;
			}
			set
			{
				if (control != value)
				{
					OnControlChanging();
					control = value;
					OnControlChanged();
				}
			}
		}

		public BodyBlock Body => body;

		public Inline HotItem => hotItem;

		public XHtmlControlRenderer()
		{
			body.PendingLayoutChanged += textBlock_PendingLayoutChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Body.Dispose();
				Control = null;
			}
			base.Dispose(disposing);
		}

		protected virtual void OnControlChanging()
		{
			if (control != null)
			{
				RegisterControl(register: false);
			}
		}

		protected virtual void OnControlChanged()
		{
			if (control != null)
			{
				RegisterControl(register: true);
				body.FontFamily = control.Font.FontFamily.Name;
				body.FontSizeEM = control.Font.Size;
				body.FontStyle = control.Font.Style;
				body.ForeColor = control.ForeColor;
			}
		}

		public Size GetPreferredSize(Size proposedSize)
		{
			if (proposedSize.Height == 0)
			{
				proposedSize.Height = int.MaxValue;
			}
			if (proposedSize.Width == 0)
			{
				proposedSize.Width = int.MaxValue;
			}
			body.Bounds = new Rectangle(Point.Empty, proposedSize);
			using (Graphics gr = control.CreateGraphics())
			{
				body.Measure(gr, body.Width);
			}
			return body.ActualSize;
		}

		private void RegisterControl(bool register)
		{
			if (register)
			{
				control.Paint += control_Paint;
				control.Resize += control_Resize;
				control.FontChanged += control_FontChanged;
				control.TextChanged += control_TextChanged;
				control.BackColorChanged += control_BackColorChanged;
				control.ForeColorChanged += control_ForeColorChanged;
				control.MouseMove += control_MouseMove;
				control.AutoSizeChanged += control_AutoSizeChanged;
			}
			else
			{
				control.Paint -= control_Paint;
				control.Resize -= control_Resize;
				control.FontChanged -= control_FontChanged;
				control.TextChanged -= control_TextChanged;
				control.BackColorChanged -= control_BackColorChanged;
				control.ForeColorChanged -= control_ForeColorChanged;
				control.MouseMove -= control_MouseMove;
				control.AutoSizeChanged += control_AutoSizeChanged;
			}
		}

		private void control_MouseMove(object sender, MouseEventArgs e)
		{
			Point location = e.Location;
			location.Offset(-control.DisplayRectangle.Location.X, -control.DisplayRectangle.Location.Y);
			Inline hitItem = body.GetHitItem(Point.Empty, location);
			if (hitItem == null || hitItem.MouseCursor == null)
			{
				Cursor.Current = control.Cursor;
			}
			else
			{
				Cursor.Current = hitItem.MouseCursor;
			}
			if (hotItem != hitItem)
			{
				if (hotItem != null)
				{
					hotItem.MouseLeave();
				}
				hitItem?.MouseEnter();
				hotItem = hitItem;
			}
		}

		private void control_ForeColorChanged(object sender, EventArgs e)
		{
			body.ForeColor = control.ForeColor;
			control.Invalidate();
		}

		private void control_BackColorChanged(object sender, EventArgs e)
		{
			control.Invalidate();
		}

		private void control_TextChanged(object sender, EventArgs e)
		{
			body.Inlines.Clear();
			try
			{
				body.Inlines.AddRange(XHtmlParser.Parse(control.Text).Inlines);
			}
			catch
			{
			}
		}

		private void control_FontChanged(object sender, EventArgs e)
		{
			body.FontFamily = control.Font.FontFamily.Name;
			body.FontSizeEM = control.Font.Size;
			body.FontStyle = control.Font.Style;
			control.Invalidate();
		}

		private void control_Resize(object sender, EventArgs e)
		{
			if (control.AutoSize)
			{
				control.Size = GetPreferredSize(control.PreferredSize);
			}
			control.Invalidate();
		}

		private void control_Paint(object sender, PaintEventArgs e)
		{
			body.Bounds = control.ClientRectangle;
			body.Draw(e.Graphics, control.DisplayRectangle.Location);
		}

		private void textBlock_PendingLayoutChanged(object sender, EventArgs e)
		{
			control.Invalidate();
		}

		private void control_AutoSizeChanged(object sender, EventArgs e)
		{
			if (control.AutoSize)
			{
				control.Size = GetPreferredSize(control.PreferredSize);
			}
		}
	}
}
