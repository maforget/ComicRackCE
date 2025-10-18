
using cYo.Common.Windows.Forms.Theme.DarkMode.Handlers;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode;

internal class DarkThemeHandler : IThemeHandler
{
    /// <summary>
    /// Contains custom steps required to apply Dark Mode for a given <see cref="Control"/> <see cref="Type"/>, where required.
    /// </summary>
    /// <remarks>
    /// Default Dark Mode is applied to Control Types when are not listed here. 
    /// </remarks>
    private static Dictionary<Type, DarkControlDefinition> DarkControlTable => DarkControl.DefinitionTable;

    /// <summary>
    /// Retrieves the custom <see cref="DarkControlDefinition"/> for a given <see cref="Control"/> <see cref="Type"/> or <see cref="Type.BaseType"/>.
    /// </summary>
    private static bool TryGetDarkControlDefinition(Type controlType, out DarkControlDefinition controlDefinition)
    {
        if (DarkControlTable.TryGetValue(controlType, out controlDefinition))
            return true;
        else
            return DarkControlTable.TryGetValue(controlType.BaseType, out controlDefinition);
    }

    /// <summary>
    /// Handles applying Dark Mode as detailed in <see cref="DarkControlDefinition"/>.
    /// </summary>
    public void Handle(Control control)
    {
        // Get a custom DarkControlDefinition if one exists; fallback to default DarkControlDefinition if it doesn't
        if (!TryGetDarkControlDefinition(control.GetType(), out var darkControlDefinition))
            darkControlDefinition = new DarkControlDefinition(control);

        SetDarkMode(control, darkControlDefinition);

        //darkControl.Apply(control);
        //control.WhenHandleCreated(darkControl.UXTheme); // would be nice if this could live in DarkControlDefinition.Apply
    }

    private void SetDarkMode(Control control, DarkControlDefinition dark)
    {
        SafeSet(() => control.GetType().GetProperty("BorderStyle")?.SetValue(control, (BorderStyle)dark.BorderStyle));
        SafeSet(() => control.ForeColor = (Color)dark.ForeColor);
        SafeSet(() => control.BackColor = (Color)dark.BackColor);

        // Invoke custom Theme handler method if required. This is often due to branching property-value logic, or custom Draw/Paint EventHandlers
        dark.Theme?.Invoke(control);

        // Apply UXTheme
        control.WhenHandleCreated(dark.UXTheme);
    }

    private static void SafeSet(Action action) { try { action(); } catch { } }
    private static void SafeSet(Action action, bool shouldSet) { if (shouldSet) { try { action(); } catch { } } }

    #region unused

    //private static Color GetSystemColor(Color color)
    //        => KnownColorTableEx.GetSystemColor(color.ToKnownColor());

    //internal static void TryReplaceBackColor(Control control, Color backColor)
    //    => SafeSet(() => control.BackColor = backColor, true);

    //internal static void TryReplaceForeColor(Control control, Color foreColor)
    //    => SafeSet(() => control.ForeColor = foreColor);
    #endregion
}
