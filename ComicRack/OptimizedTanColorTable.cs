using System.Drawing;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer
{
	public class OptimizedTanColorTable : TanColorTable
	{
		public override Color MenuStripGradientEnd => base.MenuStripGradientBegin;

		public override Color ToolStripBorder => Color.Empty;
	}
}
