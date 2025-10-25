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

    public void SetColor(Control control)
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
    public void Apply(Control control)
    {
        // Set the Dark variant of SystemColors based on the Control existing Color. Only applies if the Definition doesn't providde a BackColor or ForeColor
		SetColor(control);

		// Set BorderStyle, ForeColor, BackColor
		TrySetValue(BorderStyle, b => control.GetType().GetProperty("BorderStyle")?.SetValue(control, b));
		TrySetValue(ForeColor, c => control.ForeColor = c);
		TrySetValue(BackColor, c => control.BackColor = c);

		// Invoke custom Theme handler method if required. This is often due to branching property-value logic, or custom Draw/Paint EventHandlers
		SafeInvokeTheme(control);

		// Apply UXTheme
		control.WhenHandleCreated(UXTheme);
	}

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

	private static void TrySetValue<T>(T? value, Action<T> action) where T : struct
	{
		if (value is null)
			return;

		SafeSet(() => action(value.Value));
	}

	private static void TrySetValue<T>(T value, Action<T> action) where T : class
	{
		if (value is null)
			return;

		SafeSet(() => action(value));
	}

    private void SafeInvokeTheme(Control control)
    {
        if (Theme is null) return;

		if (control?.InvokeRequired == true)
            control?.Invoke(Theme);
        else
			Theme(control);
    }

	private static void SafeSet(Action action) { try { action(); } catch { } }
	private static void SafeSet(Action action, bool shouldSet) { if (shouldSet) { try { action(); } catch { } } }
	#endregion
}
