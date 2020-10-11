using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public static class ToolStripExtensions
	{
		public static ToolStripMenuItem Clone(this ToolStripMenuItem item)
		{
			return new ToolStripMenuItem(item.Text, item.Image, null, item.ShortcutKeys)
			{
				Name = item.Name
			};
		}

		public static void FixSeparators(this ToolStripItemCollection items)
		{
			ToolStripItem toolStripItem = null;
			foreach (ToolStripItem item in items)
			{
				if (item is ToolStripSeparator)
				{
					item.Visible = toolStripItem == null;
					toolStripItem = item;
				}
				else if (item.Available)
				{
					toolStripItem = null;
				}
			}
			if (toolStripItem != null)
			{
				toolStripItem.Visible = false;
			}
		}

		public static void FixSeparators(this ContextMenuStrip contextMenu)
		{
			contextMenu.Items.FixSeparators();
		}
	}
}
