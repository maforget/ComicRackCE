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
    #region Custom Draw Event Handlers

    
    private static void SetUncheckedBrushes(CheckBox checkBox, out Pen borderPen, out Brush borderEdgeBrush, out Brush backCornerBrush, out Brush backVertexBrush)
    {
        borderPen = ThemePens.CheckBox.UncheckedBorder;

        borderEdgeBrush = ThemeBrushes.CheckBox.UncheckedBorderEdge;
        backCornerBrush = ThemeBrushes.CheckBox.UncheckedBackCorner;
        backVertexBrush = ThemeBrushes.CheckBox.UncheckedBackVertex;

        if (!checkBox.Enabled)
        {
            borderPen = ThemePens.CheckBox.UncheckedDisabledBorder;

            borderEdgeBrush = ThemeBrushes.CheckBox.UncheckedDisabledBorderEdge;
            backCornerBrush = ThemeBrushes.CheckBox.UncheckedDisabledBackCorner;
            backVertexBrush = ThemeBrushes.CheckBox.UncheckedDisabledBackVertex;
        }
    }

    private static void DrawDarkCheckMark(CheckBox checkBox, Graphics g, Rectangle boxRect)
    {
        Point[] checkMark = new Point[6]
            {
                new Point(boxRect.Left + 2, boxRect.Top + (boxRect.Height/2)+2),
                new Point(boxRect.Left + boxRect.Width / 2 -1, boxRect.Bottom - 1),
                new Point(boxRect.Right - 2, boxRect.Top + 5),
                new Point(boxRect.Right - 2, boxRect.Top + 3),
                new Point(boxRect.Left + boxRect.Width / 2 - 1, boxRect.Bottom - 3),
                new Point(boxRect.Left + 2, boxRect.Top + (boxRect.Height / 2)),
             };

        g.DrawPolygon(checkBox.Enabled ? SystemPens.ControlText : SystemPens.GrayText, checkMark);
        g.FillPolygon(checkBox.Enabled ? SystemBrushes.ControlText : SystemBrushes.GrayText, checkMark);
    }

    private static void DrawDarkCheckBox(CheckBox checkBox, Graphics g, Rectangle boxRect)
    {
        g.Clear(checkBox.BackColor);

        Brush backBrush = checkBox.Checked ? ThemeBrushes.CheckBox.Back : ThemeBrushes.CheckBox.UncheckedBack;
        g.FillRectangle(backBrush, new Rectangle(boxRect.X + 2, boxRect.Y, boxRect.Width - 3, boxRect.Height));
        g.FillRectangle(backBrush, new Rectangle(boxRect.X, boxRect.Y + 2, boxRect.Width, boxRect.Height - 3));

        //boxRect.Inflate(-2, -3);
        // Fill background
        // Checkmark if checked
        if (checkBox.Checked)
            DrawDarkCheckMark(checkBox, g, boxRect);

        int left = boxRect.X;
        int right = boxRect.X + boxRect.Width;
        int top = boxRect.Y;
        int bottom = boxRect.Y + boxRect.Height;

        Pen borderPen = ThemePens.CheckBox.Border;

        Brush borderEdgeBrush = ThemeBrushes.CheckBox.BorderEdge;
        Brush backCornerBrush = ThemeBrushes.CheckBox.BackCorner;
        Brush backVertexBrush = ThemeBrushes.CheckBox.BackVertex;

        if (!checkBox.Checked)
            SetUncheckedBrushes(checkBox, out borderPen, out borderEdgeBrush, out backCornerBrush, out backVertexBrush);

        // Main Border
        g.DrawLine(borderPen, new Point(left + 3, top), new Point(right - 3, top));
        g.DrawLine(borderPen, new Point(left + 3, bottom), new Point(right - 3, bottom));
        g.DrawLine(borderPen, new Point(left, top + 3), new Point(left, bottom - 3));
        g.DrawLine(borderPen, new Point(right, top + 3), new Point(right, bottom - 3));

        // Border Edges
        // Top - Left + Right
        g.FillRectangle(borderEdgeBrush, left + 2, top, 1, 1);
        g.FillRectangle(borderEdgeBrush, right - 2, top, 1, 1);
        // Bottom - Left + Right
        g.FillRectangle(borderEdgeBrush, left + 2, bottom, 1, 1);
        g.FillRectangle(borderEdgeBrush, right - 2, bottom, 1, 1);
        // Left - Top + Bottom
        g.FillRectangle(borderEdgeBrush, left, top + 2, 1, 1);
        g.FillRectangle(borderEdgeBrush, left, bottom - 2, 1, 1);
        // Right - Top + Bottom
        g.FillRectangle(borderEdgeBrush, right, top + 2, 1, 1);
        g.FillRectangle(borderEdgeBrush, right, bottom - 2, 1, 1);

        // Inner Corners
        g.FillRectangle(backCornerBrush, left + 1, top + 1, 1, 1);
        g.FillRectangle(backCornerBrush, right - 1, top + 1, 1, 1);
        g.FillRectangle(backCornerBrush, right - 1, bottom - 1, 1, 1);
        g.FillRectangle(backCornerBrush, left + 1, bottom - 1, 1, 1);

        // Inner Vertices
        // Top - Left + Right
        g.FillRectangle(backVertexBrush, left + 2, top + 1, 1, 1);
        g.FillRectangle(backVertexBrush, right - 2, top + 1, 1, 1);
        // Bottom - Left + Right
        g.FillRectangle(backVertexBrush, left + 2, bottom - 1, 1, 1);
        g.FillRectangle(backVertexBrush, right - 2, bottom - 1, 1, 1);
        // Left - Top + Bottom
        g.FillRectangle(backVertexBrush, left + 1, top + 2, 1, 1);
        g.FillRectangle(backVertexBrush, left + 1, bottom - 2, 1, 1);
        // Right - Top + Bottom
        g.FillRectangle(backVertexBrush, right - 1, top + 2, 1, 1);
        g.FillRectangle(backVertexBrush, right - 1, bottom - 2, 1, 1);

        if (checkBox.Checked)
        {
            Brush cornerBrush = ThemeBrushes.CheckBox.BorderCorner;
            g.FillRectangle(cornerBrush, left + 1, top, 1, 1);
            g.FillRectangle(cornerBrush, left + 1, bottom, 1, 1);
            g.FillRectangle(cornerBrush, left, top + 1, 1, 1);
            g.FillRectangle(cornerBrush, left, bottom - 1, 1, 1);
            g.FillRectangle(cornerBrush, right - 1, top, 1, 1);
            g.FillRectangle(cornerBrush, right - 1, bottom, 1, 1);
            g.FillRectangle(cornerBrush, right, top + 1, 1, 1);
            g.FillRectangle(cornerBrush, right, bottom - 1, 1, 1);
        }
        // ... and that's how you draw a ✓. simples, right?
        // ... and that's how you draw a ✓. simples, right?

    }

    // Use custom SelectedText Highlight color.
    // related: cYo.Common.Windows.Forms.ComboBoxSkinner.comboBox_DrawItem (private, requires instantiation, comes with ComboBoxSkinner baggage)
    private static void ComboBox_DrawItemHighlight(object sender, DrawItemEventArgs e)
    {
        if (e.Index < 0)
            return;

        ComboBox comboBox = (ComboBox)sender;
        var item = comboBox.Items[e.Index];

        // override SelectedText highlighting
        if (comboBox.DroppedDown)
        {
            e.DrawBackground();
            if (e.State.HasFlag(DrawItemState.Selected))
            {
                using (Brush highlightBrush = new SolidBrush(ThemeColors.SelectedText.Highlight))
                {
                    e.Graphics.FillRectangle(highlightBrush, e.Bounds);
                }
                ControlPaint.DrawBorder(e.Graphics, e.Bounds, ThemeColors.SelectedText.Focus, ButtonBorderStyle.Solid);
            }
        }
        using (Brush brush = new SolidBrush(e.ForeColor))
        {
            using (StringFormat format = new StringFormat(StringFormatFlags.NoWrap)
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            })
            {
                e.Graphics.DrawString(comboBox.GetItemText(item), comboBox.Font, brush, e.Bounds, format);
            }
        }
    }


    /// <summary>
    /// <para><see cref="ListView.OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs)"/> method to handle dark <see cref="ColumnHeader"/> text on dark background.</para>
    /// <para><c>DrawDefault</c> is executed unless <see cref="IsDarkModeEnabled"/> is <paramref name="true"/> and <see cref="ListView.View"/> is set to <see cref="View.Details"/></para>
    /// </summary>
    public static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
    {
        // explicit IsDarkModeEnabled check as public (referenced in TaskDialog and ListStyles)
        if (!IsDarkModeEnabled || (sender as ListView).View != View.Details)
        {
            e.DrawDefault = true;
            return;
        }

        e.DrawDefault = false;
        using (Brush bgBrush = new SolidBrush(ThemeColors.Header.Back))
        {
            e.Graphics.FillRectangle(bgBrush, e.Bounds);
        }

        using (Pen sepPen = new Pen(ThemeColors.Header.Separator))
        {
            int x = e.Bounds.Right - 2;
            int y1 = e.Bounds.Top;
            int y2 = e.Bounds.Bottom;
            e.Graphics.DrawLine(sepPen, x, y1, x, y2);
        }

        using (Brush textBrush = new SolidBrush(ThemeColors.Header.Text))
        {
            // Draw the header text with custom color and font
            e.Graphics.DrawString(
                e.Header.Text,
                e.Font,
                textBrush,
                e.Bounds,
                new StringFormat
                {
                    Alignment = StringAlignment.Near, // left align text
                    LineAlignment = StringAlignment.Center, // vertically center text
                    Trimming = StringTrimming.EllipsisCharacter
                });
        }
    }

    private static void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
    {
        if (e.Item.ListView.View != View.Details)
            e.DrawDefault = true;

    }

    private static void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
        if (e.Item.ListView.View != View.Details || e.Item == null)
        {
            e.DrawDefault = true;
            return;
        }
        ListView listView = (ListView)sender;
        Rectangle bounds = e.Bounds;
        Rectangle textRect = bounds; // Updated based on whether we have images
        ImageList imageList = listView.SmallImageList;

        Color backColor = e.Item.BackColor;
        Color textColor = e.Item.ForeColor;

        if (e.Item.Selected)
        {
            backColor = ThemeColors.SelectedText.Highlight;
            textColor = SystemColors.HighlightText; // currently same as ForeColor
        }

        // Fill background
        using (var backBrush = new SolidBrush(backColor))
            e.Graphics.FillRectangle(backBrush, bounds);
        if (e.Item.Selected)
            ControlPaint.DrawBorder(e.Graphics, e.Bounds, ThemeColors.SelectedText.Focus, ButtonBorderStyle.Solid);

        // Handle first column having images
        if (e.ColumnIndex == 0 && imageList != null)
        {
            int imageWidth = imageList.ImageSize.Width;
            int imageHeight = imageList.ImageSize.Height;
            int imageY = bounds.Top + (bounds.Height - imageHeight) / 2;
            int imageX = bounds.Left + 4; // small padding from left

            // Shift text based on imageList != null, whether this item has an image or not
            textRect.X += imageX + imageWidth;

            // Draw the image if there is one to draw
            if (e.Item.ImageIndex >= 0 && e.Item.ImageIndex < imageList.Images.Count)
                imageList.Draw(e.Graphics, imageX, imageY, imageWidth, imageHeight, e.Item.ImageIndex);
        }

        textRect.X += 2;
        textRect.Width = bounds.Right - textRect.X;

        // Draw text
        TextRenderer.DrawText(
            e.Graphics,
            e.SubItem.Text,
            e.SubItem.Font,
            textRect,
            textColor,
            backColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

        // Draw column divider
        if (e.ColumnIndex < listView.Columns.Count)
        {
            using (var pen = new Pen(Color.FromArgb(77, 77, 77)))
                e.Graphics.DrawLine(pen, bounds.Right - 2, bounds.Top, bounds.Right - 2, bounds.Bottom);
        }
    }

    #endregion

}
