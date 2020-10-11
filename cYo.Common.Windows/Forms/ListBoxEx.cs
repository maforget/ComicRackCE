using System;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class ListBoxEx : ListBox
	{
		private Point dragDown;

		public event ItemDragEventHandler ItemDrag;

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			dragDown = e.Location;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			dragDown = Point.Empty;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button == MouseButtons.Left && (Math.Abs(e.X - dragDown.X) > SystemInformation.DragSize.Width || Math.Abs(e.Y - dragDown.Y) > SystemInformation.DragSize.Height) && base.SelectedItem != null)
			{
				OnItemDrag(new ItemDragEventArgs(e.Button, base.SelectedItem));
			}
		}

		protected virtual void OnItemDrag(ItemDragEventArgs e)
		{
			if (this.ItemDrag != null)
			{
				this.ItemDrag(this, e);
			}
		}
	}
}
