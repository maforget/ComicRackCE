using cYo.Common.Windows.Forms.Theme.Resources;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Resources;

/// <summary>
/// Used to define what is required to turn a <see cref="Control"/> into a Dark Mode Control.
/// </summary>
internal class DarkControlDefinition : ThemeControlDefinition
{
    // TODO : add at least Paint EventHandler methods
    public Action<Control> UXTheme { get; set; } = control => Win32.UXTheme.SetControlTheme(control.Handle);

    /// <summary>
    /// Empty constructor for use in <see cref="Handlers.DarkControl.DefinitionTable"/>.
    /// </summary>
    public DarkControlDefinition()
    {
    }

    public DarkControlDefinition(DarkControlDefinition definition)
    {
        ForeColor = definition.ForeColor;
        BackColor = definition.BackColor;
        BorderStyle = definition.BorderStyle;
        Theme = definition.Theme;
        UXTheme = definition.UXTheme;
    }

    public DarkControlDefinition(ThemeControlDefinition definition)
        : this(definition, UIComponent.None)
    {
    }

    public DarkControlDefinition(ThemeControlDefinition definition, UIComponent component)
        : base(definition)
    {
        if (!definition.AllowUXTheme)
            UXTheme = null;

		if (component != UIComponent.None)
            BackColor = DarkColors.GetUIComponentColor(component);
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
        DarkControlDefinition definition = SetColor(control);
        this.ForeColor = definition.ForeColor;
        this.BackColor = definition.BackColor;
    }

    public DarkControlDefinition SetColor(Control control)
    {
        DarkControlDefinition definition = new DarkControlDefinition(this); // Make a copy so we don't modify the DefinitionTable.

        if (!definition.ForeColor.HasValue && TryGetSystemColor(control.ForeColor, out Color systemForeColor))
			definition.ForeColor = systemForeColor;

        if (control is ITheme itheme && itheme.UIComponent != UIComponent.None) // for controls that implement ITheme
			definition.BackColor = DarkColors.GetUIComponentColor(itheme.UIComponent);
		else if (control.GetAttachedTheme() is ITheme attached && attached != null && attached.UIComponent != UIComponent.None) // for controls where ITheme is dynamically attached (plugins)
			definition.BackColor = DarkColors.GetUIComponentColor(attached.UIComponent);
		else if (!definition.BackColor.HasValue && TryGetSystemColor(control.BackColor, out Color systemBackColor))
			definition.BackColor = systemBackColor;

        return definition;
    }
}
