using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    /// <summary>
    /// source: <a href="https://github.com/dotnet/winforms">dotnet/winforms</a>. (.NET Foundation, MIT license)<br/>
    /// <c>src/System.Windows.Forms/System/Windows/Forms/DarkProfessionalColors.cs</c>
    /// </summary>
    /// <remarks>These are defaults and could do with checking/tweaking.</remarks>
    internal sealed class DarkProfessionalColorsEx : ProfessionalColorTable
    {
        public override Color MenuItemPressedGradientBegin
            => Color.FromArgb(0xFF, 0x60, 0x60, 0x60);

        public override Color MenuItemPressedGradientMiddle
            => Color.FromArgb(0xFF, 0x60, 0x60, 0x60);

        public override Color MenuItemPressedGradientEnd
            => Color.FromArgb(0xFF, 0x60, 0x60, 0x60);

        public override Color MenuItemSelected
            //=> SystemColors.ControlText; // this means white highlight
            => Color.FromArgb(52, 67, 86);

        public override Color MenuItemSelectedGradientBegin
            => Color.FromArgb(0xFF, 0x40, 0x40, 0x40);

        public override Color MenuItemSelectedGradientEnd
            => Color.FromArgb(0xFF, 0x40, 0x40, 0x40);

        public override Color MenuStripGradientBegin
            => SystemColors.Control;

        public override Color MenuStripGradientEnd
            => SystemColors.Control;

        public override Color StatusStripGradientBegin
            => SystemColors.Control;

        public override Color StatusStripGradientEnd
            => SystemColors.Control;

        public override Color ToolStripDropDownBackground
            => SystemColors.Control;

        public override Color ImageMarginGradientBegin
            => SystemColors.Control;

        public override Color ImageMarginGradientMiddle
            => SystemColors.Control;

        public override Color ImageMarginGradientEnd
            => SystemColors.Control;
    }
}