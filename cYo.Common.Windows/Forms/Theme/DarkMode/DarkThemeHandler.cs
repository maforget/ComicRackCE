
using cYo.Common.Windows.Forms.Theme.DarkMode.Handlers;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
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
		controlDefinition = DarkControlTable.FirstOrDefault(x => x.Key.IsAssignableFrom(controlType)).Value;
        return controlDefinition != null;
        //if (DarkControlTable.TryGetValue(controlType, out controlDefinition))
        //    return true;
        //else
        //    return DarkControlTable.TryGetValue(controlType.BaseType, out controlDefinition);
    }

    /// <summary>
    /// Handles applying Dark Mode as detailed in <see cref="DarkControlDefinition"/>.
    /// </summary>
    public void Handle(Control control)
    {
        // Get a custom DarkControlDefinition if one exists; fallback to default DarkControlDefinition if it doesn't
        if (!TryGetDarkControlDefinition(control.GetType(), out var darkControlDefinition))
            darkControlDefinition = new DarkControlDefinition(control);

        darkControlDefinition.SetColor(control);

        SetDarkMode(control, darkControlDefinition);

        //darkControl.Apply(control);
        //control.WhenHandleCreated(darkControl.UXTheme); // would be nice if this could live in DarkControlDefinition.Apply
    }

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
	{
		if (value is null)
			return;

		SafeSet(() => action(value.Value));
	}

	private void TrySetValue<T>(T value, Action<T> action) where T : class
	{
		if (value is null)
			return;

		SafeSet(() => action(value));
	}

	private static void SafeSet(Action action) { try { action(); } catch { } }
    private static void SafeSet(Action action, bool shouldSet) { if (shouldSet) { try { action(); } catch { } } }
    #endregion
}
