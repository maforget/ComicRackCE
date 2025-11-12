
using cYo.Common.Windows.Forms.Theme.DarkMode.Handlers;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode;

/// <summary>
/// Handles applying Dark Mode theming to <see cref="Control"/>.
/// </summary>
internal class DarkThemeHandler : IThemeHandler
{
    /// <summary>
    /// Contains custom steps required to apply Dark Mode for a given <see cref="Control"/> <see cref="Type"/>, where required.
    /// </summary>
    private static Dictionary<Type, DarkControlDefinition> DarkControlTable => DarkControl.DefinitionTable;

    /// <summary>
    /// Retrieves the custom <see cref="DarkControlDefinition"/> for a given <see cref="Control"/> <see cref="Type"/> or <see cref="Type.BaseType"/>.
    /// </summary>
    private static bool TryGetDarkControlDefinition(Type controlType, out DarkControlDefinition controlDefinition)
    {
        if (DarkControlTable.TryGetValue(controlType, out controlDefinition))
            return true;
        else
            controlDefinition = DarkControlTable.FirstOrDefault(definition => controlType.IsSubclassOf(definition.Key)).Value;
        return controlDefinition != null;
    }

    private static bool TryGetDarkCustomControlDefinition(Control control, out DarkControlDefinition customDefinition)
    {
        if (typeof(IThemeCustom).IsAssignableFrom(control.GetType()))
        {
            IThemeCustom customControl = (IThemeCustom)control;
            if (customControl.ControlDefinition != null)
            {
                customDefinition = new DarkControlDefinition(customControl.ControlDefinition);
                if (TryGetDarkControlDefinition(control.GetType(), out var darkControlDefinition))
                    customDefinition.UXTheme = darkControlDefinition.UXTheme;
                return true;
            }
        }

        customDefinition = null;
        return false;
    }

    /// <summary>
    /// Handles applying Dark Mode as detailed in <see cref="DarkControlDefinition"/>.
    /// </summary>
    public void Handle(Control control)
    {
		// Get a custom ThemeDefinition if one exists. 
		if (!TryGetDarkCustomControlDefinition(control, out var darkControlDefinition))
		{
		    // Get a Dark Mode DarkControlDefinition if one exists. 
			if (TryGetDarkControlDefinition(control.GetType(), out darkControlDefinition))
				darkControlDefinition = darkControlDefinition.SetColor(control);
			// Fall back to default DarkControlDefinition
			else
				darkControlDefinition = new DarkControlDefinition(control);
		}

		SetDarkMode(control, darkControlDefinition!);
    }

    /// <summary>
    /// Set Dark Mode for a <see cref="Control"/> by applying <see cref="DarkControlDefinition"/>.
    /// </summary>
    /// <param name="control">Control to set Dark Mode for</param>
    /// <param name="dark">Definition of how to set Dark Mode for <paramref name="control"/></param>
    private void SetDarkMode(Control control, DarkControlDefinition dark)
	{
		TrySetValue(dark.BorderStyle, b => control.GetType().GetProperty("BorderStyle")?.SetValue(control, b));
		TrySetValue(dark.ForeColor, c => control.ForeColor = c);
		TrySetValue(dark.BackColor, c => control.BackColor = c);

		// Invoke custom Theme handler method if required. This is often due to branching property-value logic, or custom Draw/Paint EventHandlers
		dark.Theme?.Invoke(control);

		// Apply UXTheme
		control.WhenHandleCreated(dark.UXTheme);
	}

    #region Helpers
    private void TrySetValue<T>(T? value, Action<T> action) where T : struct
        => SafeSet(() => action(value.Value), value is not null);

	private void TrySetValue<T>(T value, Action<T> action) where T : class
        => SafeSet(() => action(value), value is not null);

    private static void SafeSet(Action action) { try { action(); } catch { } }
    private static void SafeSet(Action action, bool shouldSet) { if (shouldSet) { try { action(); } catch { } } }
    #endregion
}
