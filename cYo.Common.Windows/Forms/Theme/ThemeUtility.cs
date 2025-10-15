using cYo.Common.Drawing;
using cYo.Common.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms;

/// <summary>
/// Class to centralisation application Theming. When theming is enabled, calls <see cref="KnownColorTableEx"/> on initialization to override built-in <see cref="System.Drawing.SystemColors"/> with theme-defined colors.<br/>
/// Exposes <see cref="Theme(Control)"/> for recursive <see cref="Control"/> theming by setting class fields and leveraging <see cref="UXTheme"/> for native Windows OS theming.
/// </summary>
public static partial class ThemeExtensions
{
    #region External Helper Functions

    public static void SetSidePanelColor(Control control)
    {
        if (!IsDarkModeEnabled) return;
        control.BackColor = ThemeColors.Material.SidePanel;
        if (control.GetType() == typeof(TreeView) || control.GetType() == typeof(TreeViewEx))
        {
            TreeViewEx.SetColor((TreeView)control, ThemeColors.Material.SidePanel);
        }
    }

    public static void SetBorderStyle(Control control, BorderStyle? borderStyle = null)
    {
        if (!IsDarkModeEnabled) return;
        control.GetType().GetProperty("BorderStyle")?.SetValue(control, borderStyle ?? BorderStyle.None);
    }
    #endregion
}
