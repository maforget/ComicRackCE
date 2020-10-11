using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Menus
{
	internal class ToolStripThumbSize : ToolStripControlHost
	{
		public TrackBarLite TrackBar => base.Control as TrackBarLite;

		public ToolStripThumbSize()
			: base(new TrackBarLite())
		{
			base.Control.BackColor = Color.Transparent;
			TrackBar.ThumbSize = new Size(6, 14);
			TrackBar.EnableFocusIndicator = false;
		}

		public override Size GetPreferredSize(Size constrainingSize)
		{
			return new Size(120, 16).ScaleDpi();
		}

		public void SetSlider(int min, int max, int val)
		{
			TrackBar.SetRange(min, max);
			TrackBar.Value = val;
		}
	}
}
