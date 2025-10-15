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
    // TODO: handle disabled checkboxes with images/icons (currently they're wiped)
    public static void CheckBox_Paint(object sender, PaintEventArgs e)
    {
        CheckBox checkBox = sender as CheckBox;
        bool onlyDrawDisabledText = OsVersionEx.IsWindows11_OrGreater() || checkBox.Appearance == Appearance.Button;
        TextFormatFlags textFormatFlags = GetTextFormatFlags(checkBox);

        // default OS drawing for enabled Win11 CheckBox or Appearance.Button
        if (onlyDrawDisabledText && checkBox.Enabled) return;

        Rectangle boxRect = GetCheckRectangle(checkBox, e.Graphics);
        Rectangle textRect = checkBox.Appearance == Appearance.Button ? e.ClipRectangle : GetTextRectangle(checkBox, e.Graphics, boxRect);

        //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

        // Draw actual Check (Box + Mark)
        if (!onlyDrawDisabledText)
            DrawDarkCheckBox(checkBox, e.Graphics, boxRect);

        // Clear text area
        using (var backBrush = new SolidBrush(checkBox.BackColor))
            e.Graphics.FillRectangle(backBrush, textRect);

        // Draw text
        TextRenderer.DrawText(
            e.Graphics,
            checkBox.Text,
            checkBox.Font,
            textRect,
            checkBox.Enabled ? checkBox.ForeColor : SystemColors.GrayText,
            textFormatFlags
        );
    }

    private static void Label_Paint(object sender, PaintEventArgs e)
    {
        Label label = sender as Label;

        if (!label.Enabled)
        {
            TextFormatFlags textFormatFlags = GetTextFormatFlags(label);
            TextRenderer.DrawText(e.Graphics, label.Text, label.Font, label.ClientRectangle, SystemColors.GrayText, label.BackColor, textFormatFlags);
        }
    }

    #region Custom Paint Event Handlers
    private static void ToolStripStatusLabel_Paint(object sender, PaintEventArgs e)
    {
        using (var pen = new Pen(ThemeColors.ToolStrip.BorderColor, 1))
        {
            e.Graphics.DrawRectangle(pen, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
        }
    }

    #endregion
}
