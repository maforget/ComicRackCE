using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing;

namespace cYo.Common.Windows.Forms
{
	public class ControlStyleColorTable : ProfessionalColorTable
	{
		private readonly Color backgroundColor = SystemColorsEx.Control;

		private readonly Color lightColor = SystemColorsEx.ControlLight;

		private readonly Color darkColor = SystemColorsEx.ControlDark;

		private readonly Color borderColor = Color.Black;

		public override Color ToolStripGradientBegin => backgroundColor;

		public override Color ToolStripBorder => backgroundColor;

		public override Color ToolStripGradientEnd => backgroundColor;

		public override Color ToolStripGradientMiddle => backgroundColor;

		public override Color ToolStripPanelGradientBegin => backgroundColor;

		public override Color ToolStripPanelGradientEnd => backgroundColor;

		public override Color ToolStripContentPanelGradientBegin => backgroundColor;

		public override Color ToolStripContentPanelGradientEnd => backgroundColor;

		public override Color StatusStripGradientBegin => backgroundColor;

		public override Color StatusStripGradientEnd => backgroundColor;

		public override Color MenuStripGradientBegin => backgroundColor;

		public override Color MenuBorder => borderColor;

		public override Color MenuStripGradientEnd => backgroundColor;

		public override Color GripDark => darkColor;

		public override Color GripLight => lightColor;

		public override Color OverflowButtonGradientBegin => backgroundColor;

		public override Color OverflowButtonGradientEnd => backgroundColor;

		public override Color OverflowButtonGradientMiddle => backgroundColor;
	}
}
