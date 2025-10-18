using cYo.Common.Drawing.ExtendedColors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Resources;

/// <summary>
/// Used to define what is required to turn a <see cref="Control"/> into a Dark Mode Control.
/// </summary>
internal class DarkControlDefinition
{
    public Color? ForeColor { get; set; }
    public Color? BackColor { get; set; }
    public BorderStyle? BorderStyle { get; set; }
    public Action<Control> Theme { get; set; }
    public Action<Control> UXTheme { get; set; } = control => Win32.UXTheme.SetControlTheme(control.Handle); //Handlers.SetControlUXTheme(control);

    public DarkControlDefinition()
    {
    }

    public DarkControlDefinition(DarkControlDefinition darkControlDefinition)
    {
        ForeColor = darkControlDefinition.ForeColor;
        BackColor = darkControlDefinition.BackColor;
        BorderStyle = darkControlDefinition.BorderStyle;
        Theme = darkControlDefinition.Theme;
        UXTheme = darkControlDefinition.UXTheme;
    }

    /// <summary>
    /// Create new instance. Set replacement <typeparamref name="ForeColor"/>/<typeparamref name="BackColor"/> based on <see cref="Control.ForeColor"/>/<see cref="Control.BackColor"/>.
    /// </summary>
    /// <param name="control">Control to instantiate Dark Mode definition for.</param>
    /// <remarks>
    /// Replacement <see cref="Color"/> is only set when <see cref="Color.IsSystemColor"/> is <paramref name="true"/>.
    /// </remarks>
    public DarkControlDefinition(Control control)
    {
        //if (TryGetDarkControlDefinition(control.GetType(), out var darkControl))
        SetColor(control);
    }

    private void SetColor(Control control)
    {
        Color systemForeColor;
        Color systemBackColor;

        if (!ForeColor.HasValue && TryGetSystemColor(control.ForeColor, out systemForeColor))
            ForeColor = systemForeColor;

        if (!BackColor.HasValue && TryGetSystemColor(control.BackColor, out systemBackColor))
            BackColor = systemBackColor;
    }

    /// <summary>
    /// Apply the current <see cref="DarkControlDefinition"/> to <paramref name="control"/>
    /// </summary>
    /// <param name="control">Control to apply Dark Mode to.</param>
    //public void Apply(Control control)
    //{
        // Set BorderStyle, ForeColor, BackColor
        //SafeSet(() => control.GetType().GetProperty("BorderStyle")?.SetValue(control, (BorderStyle)BorderStyle), BorderStyle != null);
        //SafeSet(() => control.ForeColor = (Color)ForeColor, ForeColor != null);
        //SafeSet(() => control.BackColor = (Color)BackColor, BackColor != null);



        // TODO: add at least Paint EventHandler methods

        // Invoke custom Theme handler method if required. This
        //Theme?.Invoke(control);

        // Apply UXTheme
        //control.WhenHandleCreated(UXTheme);
    //}

    #region Helpers
    private static bool TryGetSystemColor(Color color, out Color systemColor)
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
