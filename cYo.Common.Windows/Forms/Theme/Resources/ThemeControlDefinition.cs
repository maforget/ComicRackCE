using cYo.Common.Drawing.ExtendedColors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.Resources;

/// <summary>
/// Used to define what is required to turn a <see cref="Control"/> into a Themed Control.
/// </summary>
public class ThemeControlDefinition
{
    public bool AllowUXTheme { get; set; } = true;
	public Color? ForeColor { get; set; }
    public Color? BackColor { get; set; }
    public BorderStyle? BorderStyle { get; set; }
    public Action<Control> Theme { get; set; }

    public ThemeControlDefinition()
    {
    }

    public ThemeControlDefinition(ThemeControlDefinition definition)
    {
        ForeColor = definition.ForeColor;
        BackColor = definition.BackColor;
        BorderStyle = definition.BorderStyle;
        Theme = definition.Theme;
    }

    #region Helpers
    protected static bool TryGetSystemColor(Color color, out Color systemColor)
    {
        if (color.IsSystemColor)
        {
            systemColor = KnownColorTableEx.GetSystemColor(color.ToKnownColor());
            return true;
        }
        systemColor = Color.Empty;
        return false;
    }
    #endregion
}
