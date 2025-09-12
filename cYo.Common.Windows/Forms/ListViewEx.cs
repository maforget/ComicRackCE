using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class ListViewEx : ListView
	{
		private static class Native
		{
			public const int WM_HSCROLL = 276;

			public const int WM_VSCROLL = 277;

			public const int WM_PAINT = 15;
		}

		public class MouseReorderEventArgs : CancelEventArgs
		{
			public ListViewItem Item
			{
				get;
				private set;
			}

			public int FromIndex
			{
				get;
				private set;
			}

			public int ToIndex
			{
				get;
				private set;
			}

			public MouseReorderEventArgs(ListViewItem item, int from, int to)
			{
				Item = item;
				FromIndex = from;
				ToIndex = to;
			}
		}

		private const int ScrollMargin = 10;

		private ListViewItem dragItem;

		private Timer scrollTimer;

		private Color insertLineColor = Color.Black;

		private int insertLineBefore = -1;

		private int insertLineAfter = -1;

		[DefaultValue(false)]
		public bool EnableMouseReorder
		{
			get;
			set;
		}

		[DefaultValue(typeof(Color), "Black")]
		public Color InsertLineColor
		{
			get
			{
				return insertLineColor;
			}
			set
			{
				if (!(insertLineColor == value))
				{
					insertLineColor = value;
					if (insertLineBefore >= 0 || insertLineAfter >= 0)
					{
						Invalidate();
					}
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int InsertLineBefore
		{
			get
			{
				return insertLineBefore;
			}
			set
			{
				if (insertLineBefore != value)
				{
					insertLineBefore = value;
					Invalidate();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int InsertLineAfter
		{
			get
			{
				return insertLineAfter;
			}
			set
			{
				if (insertLineAfter != value)
				{
					insertLineAfter = value;
					Invalidate();
				}
			}
		}

		public event EventHandler Scroll;

		public event EventHandler<MouseReorderEventArgs> MouseReorder;

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == Native.WM_HSCROLL || m.Msg == Native.WM_VSCROLL)
			{
				OnScroll();
			}
			if (m.Msg == Native.WM_PAINT)
			{
				if (InsertLineBefore >= 0 && InsertLineBefore < base.Items.Count)
				{
					Rectangle bounds = base.Items[InsertLineBefore].GetBounds(ItemBoundsPortion.Entire);
					DrawInsertionLine(bounds.Left, bounds.Right, bounds.Top);
				}
				if (InsertLineAfter >= 0 && InsertLineBefore < base.Items.Count)
				{
					Rectangle bounds2 = base.Items[InsertLineAfter].GetBounds(ItemBoundsPortion.Entire);
					DrawInsertionLine(bounds2.Left, bounds2.Right, bounds2.Bottom);
				}
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			OnScroll();
			base.OnMouseWheel(e);
		}

		protected override void OnCreateControl()
		{
			base.OnCreateControl();
			DoubleBuffered = true;
			scrollTimer = new Timer
			{
				Interval = 100,
				Enabled = false
			};
			scrollTimer.Tick += scrollTimer_Tick;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && scrollTimer != null)
			{
				scrollTimer.Dispose();
			}
			base.Dispose(disposing);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (EnableMouseReorder && base.View == View.Details)
			{
				dragItem = GetItemAt(e.X, e.Y);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (dragItem != null)
			{
				Cursor = Cursors.Hand;
				UpdateInsertMarkers();
				if (InsertLineBefore == base.TopItem.Index || e.Y > base.Height - ScrollMargin)
				{
					scrollTimer.Start();
				}
				else
				{
					scrollTimer.Stop();
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (dragItem == null)
			{
				return;
			}
			scrollTimer.Stop();
			try
			{
				int y = Math.Min(e.Y, base.Items[base.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);
				ListViewItem itemAt = GetItemAt(0, y);
				if (itemAt == null)
				{
					return;
				}
				int num = -1;
				if (InsertLineBefore >= 0)
				{
					num = InsertLineBefore;
				}
				else if (InsertLineAfter >= 0)
				{
					num = Math.Min(InsertLineAfter + 1, base.Items.Count);
				}
				if (dragItem.Index < num)
				{
					num--;
				}
				if (num >= 0 && dragItem.Index != num)
				{
					MouseReorderEventArgs mouseReorderEventArgs = new MouseReorderEventArgs(dragItem, dragItem.Index, num);
					OnMouseReorder(mouseReorderEventArgs);
					if (!mouseReorderEventArgs.Cancel)
					{
						base.Items.Remove(dragItem);
						base.Items.Insert(num, dragItem);
					}
				}
				int num4 = (InsertLineAfter = (InsertLineBefore = -1));
			}
			finally
			{
				dragItem = null;
				Cursor = Cursors.Default;
			}
		}

		protected virtual void OnScroll()
		{
			if (this.Scroll != null)
			{
				this.Scroll(this, EventArgs.Empty);
			}
		}

		protected virtual void OnMouseReorder(MouseReorderEventArgs e)
		{
			if (this.MouseReorder != null)
			{
				this.MouseReorder(this, e);
			}
		}

		private void scrollTimer_Tick(object sender, EventArgs e)
		{
			Point point = PointToClient(Cursor.Position);
			if (base.Items.Count != 0)
			{
				int index = base.TopItem.Index;
				index = ((point.Y >= base.Height / 2) ? (index + 1) : (index - 1));
				if (index >= 0 && index < base.Items.Count)
				{
					base.TopItem = base.Items[index];
				}
				UpdateInsertMarkers();
			}
		}

		private bool UpdateInsertMarkers()
		{
			if (dragItem == null)
			{
				return false;
			}
			Point point = PointToClient(Cursor.Position);
			int y = Math.Min(point.Y, base.Items[base.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);
			ListViewItem itemAt = GetItemAt(0, y);
			if (itemAt == null)
			{
				return false;
			}
			Rectangle bounds = itemAt.GetBounds(ItemBoundsPortion.Entire);
			if (point.Y < bounds.Top + bounds.Height / 2)
			{
				InsertLineBefore = itemAt.Index;
				InsertLineAfter = -1;
			}
			else
			{
				InsertLineBefore = -1;
				InsertLineAfter = itemAt.Index;
			}
			return true;
		}

		private void DrawInsertionLine(int x1, int x2, int y)
		{
			using (Graphics graphics = CreateGraphics())
			{
				using (Pen pen = new Pen(InsertLineColor))
				{
					using (Brush brush = new SolidBrush(InsertLineColor))
					{
						graphics.DrawLine(pen, x1, y, x2 - 1, y);
						Point[] points = new Point[3]
						{
							new Point(x1, y - 4),
							new Point(x1 + 7, y),
							new Point(x1, y + 4)
						};
						Point[] points2 = new Point[3]
						{
							new Point(x2, y - 4),
							new Point(x2 - 8, y),
							new Point(x2, y + 4)
						};
						graphics.FillPolygon(brush, points);
						graphics.FillPolygon(brush, points2);
					}
				}
			}
		}
	}
}
