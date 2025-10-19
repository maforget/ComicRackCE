using System.Drawing;
using cYo.Common.Drawing.ExtendedColors;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Resources;

/// <summary>
/// Default Dark Mode <see cref="ProfessionalColorTableEx"/>.
/// </summary>
/// <remarks>
/// Based on <a href="https://github.com/dotnet/winforms">dotnet/winforms</a>. <c>src/System/Windows/Forms/DarkProfessionalColors.cs</c> (.NET Foundation, MIT license)<br/>
/// </remarks>
internal sealed class DarkProfessionalColors : ProfessionalColorTableEx
{
    public override Color MenuItemPressedGradientBegin
        => Color.FromArgb(0xFF, 0x60, 0x60, 0x60);

    public override Color MenuItemPressedGradientMiddle
        => Color.FromArgb(0xFF, 0x60, 0x60, 0x60);

    public override Color MenuItemPressedGradientEnd
        => Color.FromArgb(0xFF, 0x60, 0x60, 0x60);

    public override Color MenuItemSelected => DarkColors.SelectedText.Highlight;
    //=> SystemColors.ControlText; // this means white highlight

    public override Color MenuItemSelectedGradientBegin
        => Color.FromArgb(0xFF, 0x40, 0x40, 0x40);

    public override Color MenuItemSelectedGradientEnd
        => Color.FromArgb(0xFF, 0x40, 0x40, 0x40);

    public override Color MenuStripGradientBegin => SystemColors.ControlLightLight;
    //=> SystemColors.Control;

    public override Color MenuStripGradientEnd => SystemColors.ControlLightLight;
    //=> SystemColors.Control;

    public override Color StatusStripGradientBegin => Color.FromArgb(51, 51, 51);
    //=> SystemColors.Control;

    public override Color StatusStripGradientEnd => Color.FromArgb(51, 51, 51);
    //=> SystemColors.Control;

    public override Color ToolStripDropDownBackground => Color.FromArgb(43, 43, 43);
    //=> SystemColors.Control;

    public override Color ImageMarginGradientBegin => Color.FromArgb(51, 51, 51);
    //=> SystemColors.Control;

    public override Color ImageMarginGradientMiddle => Color.FromArgb(51, 51, 51);
    //=> SystemColors.Control;

    public override Color ImageMarginGradientEnd => Color.FromArgb(51, 51, 51);
    //=> SystemColors.Control;

    public override Color ToolStripBorder => DarkColors.Border.Dark;

    public override Color ToolStripGradientBegin => SystemColors.ControlLight;

    public override Color ToolStripGradientMiddle => SystemColors.ControlLight;

    public override Color ToolStripGradientEnd => SystemColors.ControlLight;
}