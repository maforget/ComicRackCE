using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace cYo.Common.Windows.Forms
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
	public class ToolStripSearchTextBox : ToolStripControlHost
	{
		public SearchTextBox TextBox => base.Control as SearchTextBox;

		public ToolStripSearchTextBox()
			: base(new SearchTextBox())
		{
		}

		public override Size GetPreferredSize(Size constrainingSize)
		{
			return new Size(130, 20).ScaleDpi();
		}
	}
}
