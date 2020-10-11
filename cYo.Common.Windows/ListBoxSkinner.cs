using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Windows.Forms;

namespace cYo.Common.Windows
{
	public class ListBoxSkinner : Component
	{
		private readonly ListBox listBox;

		public ListBoxSkinner(ListBox listBox)
		{
			this.listBox = listBox;
			listBox.DrawMode = DrawMode.OwnerDrawFixed;
			listBox.DrawItem += DrawItem;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				listBox.DrawItem -= DrawItem;
			}
			base.Dispose(disposing);
		}

		private void DrawItem(object sender, DrawItemEventArgs e)
		{
			string itemText = listBox.GetItemText(listBox.Items[e.Index]);
			e.DrawBackground();
			e.Graphics.DrawStyledRectangle(e.Bounds, StyledRenderer.GetAlphaStyle(e.State.HasFlag(DrawItemState.Selected), e.State.HasFlag(DrawItemState.HotLight), e.State.HasFlag(DrawItemState.Focus)), StyledRenderer.GetSelectionColor(listBox.Focused));
			using (SolidBrush brush = new SolidBrush(e.ForeColor))
			{
				e.Graphics.DrawString(itemText, e.Font, brush, e.Bounds);
			}
		}
	}
}
