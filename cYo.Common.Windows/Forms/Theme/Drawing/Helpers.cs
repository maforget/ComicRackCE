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
    private static Rectangle GetCheckRectangle(CheckBox checkBox, Graphics g)
    {
        Size glyphSize = CheckBoxRenderer.GetGlyphSize(g, CheckBoxState.UncheckedNormal);
        Point checkPosition = GetImageAlignmentPoint(checkBox.ClientRectangle, glyphSize, checkBox.CheckAlign);

        Rectangle boxRect = new Rectangle(checkPosition, glyphSize);

        if (checkBox.CheckAlign == System.Drawing.ContentAlignment.MiddleRight)
            boxRect.X -= 1;
        return boxRect;
    }

    private static Rectangle GetTextRectangle(CheckBox checkBox, Graphics g, Rectangle boxRect)
    {
        // this is the kind of thing Microsoft love to change between OS versions
        int padTextY = 1, padBoxRtl = 2, padBoxLtr = 3;

        Rectangle textRect = checkBox.ClientRectangle;
        textRect.Y -= FormUtility.ScaleDpiX(padTextY);

        if (checkBox.CheckAlign == System.Drawing.ContentAlignment.MiddleRight)
        {
            textRect.X += 1;
            textRect.Width = checkBox.Bounds.Width - FormUtility.ScaleDpiX(boxRect.Width) - FormUtility.ScaleDpiX(padBoxRtl);
        }
        else
        {
            textRect.X = boxRect.Right + FormUtility.ScaleDpiX(padBoxLtr);
            textRect.Width = checkBox.Bounds.Right - FormUtility.ScaleDpiX(textRect.X);
        }
        return textRect;
    }

    internal static Point GetImageAlignmentPoint(Rectangle bounds, Size imageSize, System.Drawing.ContentAlignment alignment)
    {
        int x = 0;
        int y = 0;

        switch (alignment)
        {
            case System.Drawing.ContentAlignment.TopLeft:
                x = bounds.Left;
                y = bounds.Top;
                break;
            case System.Drawing.ContentAlignment.TopCenter:
                x = bounds.Left + (bounds.Width - imageSize.Width) / 2;
                y = bounds.Top;
                break;
            case System.Drawing.ContentAlignment.TopRight:
                x = bounds.Right - imageSize.Width;
                y = bounds.Top;
                break;
            case System.Drawing.ContentAlignment.MiddleLeft:
                x = bounds.Left;
                y = bounds.Top + (bounds.Height - imageSize.Height) / 2;
                break;
            case System.Drawing.ContentAlignment.MiddleCenter:
                x = bounds.Left + (bounds.Width - imageSize.Width) / 2;
                y = bounds.Top + (bounds.Height - imageSize.Height) / 2;
                break;
            case System.Drawing.ContentAlignment.MiddleRight:
                x = bounds.Right - imageSize.Width;
                y = bounds.Top + (bounds.Height - imageSize.Height) / 2;
                break;
            case System.Drawing.ContentAlignment.BottomLeft:
                x = bounds.Left;
                y = bounds.Bottom - imageSize.Height;
                break;
            case System.Drawing.ContentAlignment.BottomCenter:
                x = bounds.Left + (bounds.Width - imageSize.Width) / 2;
                y = bounds.Bottom - imageSize.Height;
                break;
            case System.Drawing.ContentAlignment.BottomRight:
                x = bounds.Right - imageSize.Width;
                y = bounds.Bottom - imageSize.Height;
                break;
        }

        return new Point(x, y);
    }

    private static TextFormatFlags GetTextFormatFlags(ButtonBase button)
    {
        TextFormatFlags flags = TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;

        switch (button.TextAlign)
        {
            case System.Drawing.ContentAlignment.TopLeft:
                flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                break;
            case System.Drawing.ContentAlignment.TopCenter:
                flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                break;
            case System.Drawing.ContentAlignment.TopRight:
                flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                break;
            case System.Drawing.ContentAlignment.MiddleLeft:
                flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                break;
            case System.Drawing.ContentAlignment.MiddleCenter:
                flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                break;
            case System.Drawing.ContentAlignment.MiddleRight:
                flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                break;
            case System.Drawing.ContentAlignment.BottomLeft:
                flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                break;
            case System.Drawing.ContentAlignment.BottomCenter:
                flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                break;
            case System.Drawing.ContentAlignment.BottomRight:
                flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                break;
        }

        // Right-to-left text handling
        if (button.RightToLeft == RightToLeft.Yes)
            flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;

        // Mnemonics (Alt+char underlines)
        if (button.UseMnemonic)
            flags &= ~TextFormatFlags.NoPrefix;
        else
            flags |= TextFormatFlags.NoPrefix;

        // Ellipsis
        if (button.AutoEllipsis)
            flags |= TextFormatFlags.EndEllipsis;

        return flags;
    }

    private static TextFormatFlags GetTextFormatFlags(Label label)
    {
        TextFormatFlags flags =
            TextFormatFlags.WordBreak |
            TextFormatFlags.TextBoxControl |
            TextFormatFlags.PreserveGraphicsTranslateTransform |
            TextFormatFlags.PreserveGraphicsClipping;

        // Alignment
        switch (label.TextAlign)
        {
            case System.Drawing.ContentAlignment.TopLeft:
                flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                break;
            case System.Drawing.ContentAlignment.TopCenter:
                flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                break;
            case System.Drawing.ContentAlignment.TopRight:
                flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                break;
            case System.Drawing.ContentAlignment.MiddleLeft:
                flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                break;
            case System.Drawing.ContentAlignment.MiddleCenter:
                flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                break;
            case System.Drawing.ContentAlignment.MiddleRight:
                flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                break;
            case System.Drawing.ContentAlignment.BottomLeft:
                flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                break;
            case System.Drawing.ContentAlignment.BottomCenter:
                flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                break;
            case System.Drawing.ContentAlignment.BottomRight:
                flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                break;
        }

        // Mnemonics
        if (!label.UseMnemonic)
            flags |= TextFormatFlags.NoPrefix;
        //else if (!label.ShowKeyboardCues)
        //    flags |= TextFormatFlags.HidePrefix;

        // Right-to-left layout
        if (label.RightToLeft == RightToLeft.Yes)
            flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;

        // AutoEllipsis (only when not multiline)
        if (!label.AutoSize && label.AutoEllipsis)
            flags |= TextFormatFlags.EndEllipsis;

        return flags;
    }
}