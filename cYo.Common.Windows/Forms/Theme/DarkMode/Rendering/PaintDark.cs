using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static cYo.Common.Windows.Forms.Theme.DarkMode.Rendering.Helpers;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;

internal static class PaintDark
{
    // TODO: handle disabled CheckBoxes with images/icons (currently they're wiped)
    public static void CheckBox(object sender, PaintEventArgs e)
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
            DrawDark.CheckBox(checkBox, e.Graphics, boxRect);

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

    /// <summary>
    /// Paint disabled Label text.
    /// </summary>
    internal static void Label(object sender, PaintEventArgs e)
    {
        Label label = sender as Label;

        if (!label.Enabled)
        {
            TextFormatFlags textFormatFlags = GetTextFormatFlags(label);
            TextRenderer.DrawText(e.Graphics, label.Text, label.Font, label.ClientRectangle, SystemColors.GrayText, label.BackColor, textFormatFlags);
        }
    }

    // SOON : Use ControlPaintEx
    /// <summary>
    /// Paint ToolStripStatusLabel border
    /// </summary>
    internal static void ToolStripStatusLabel(object sender, PaintEventArgs e)
    {
        using (var pen = new Pen(DarkColors.ToolStrip.BorderColor, 1))
        {
            e.Graphics.DrawRectangle(pen, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
        }
    }

}
