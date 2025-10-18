﻿
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.AxHost;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;


internal static class DrawDark
{

    internal static void ButtonBase(Graphics graphics, Rectangle clientRectangle, PushButtonState state)
    {
        if (state == PushButtonState.Hot)
        {
            using (Brush b = new SolidBrush(DarkColors.Button.MouseOverBack))
                graphics.FillRectangle(b, clientRectangle);
        }
        else if (state == PushButtonState.Pressed)
        {
            using (Brush b = new SolidBrush(DarkColors.Button.CheckedBack))
                graphics.FillRectangle(b, clientRectangle);
        }
        else
        {
            using (Brush b = new SolidBrush(DarkColors.Button.Back))
                graphics.FillRectangle(b, clientRectangle);
        }
        ControlPaint.DrawBorder(graphics, clientRectangle, DarkColors.Button.Border, ButtonBorderStyle.Solid);
    }

    internal static void TabItem(Graphics g, Rectangle rect, TabItemState tabItemState, bool buttonMode)
    {
        using (Brush backgroundBrush = new SolidBrush((tabItemState == TabItemState.Selected) ? ThemeColors.TabBar.SelectedBack : ThemeColors.TabBar.Back))
        {
            g.FillRectangle(backgroundBrush, rect);
        }
        TabBorder(g, rect, tabItemState);
    }

    private static void TabBorder(Graphics gr, Rectangle rect, TabItemState tabItemState)
    {
        if (tabItemState == TabItemState.Selected)
        {
            using (Pen selectedBorderPen = new Pen(ThemeColors.TabBar.DefaultBorder))
            {
                gr.DrawLine(selectedBorderPen, rect.Left, rect.Bottom - 1, rect.Left, rect.Top);           // Left
                gr.DrawLine(selectedBorderPen, rect.Left, rect.Top, rect.Right - 1, rect.Top);             // Top
                gr.DrawLine(selectedBorderPen, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom - 1); // Right
            }
            using (Pen borderPen = new Pen(ThemeColors.TabBar.DefaultBorder))
            {
                gr.DrawLine(borderPen, rect.Right - 1, rect.Top - 1, rect.Right - 1, rect.Bottom - 1);
            }
            return;
        }
        using (Pen borderPen = new Pen(ThemeColors.TabBar.DefaultBorder))
        {
            gr.DrawLine(borderPen, rect.Left, rect.Bottom - 1, rect.Left, rect.Top);           // Left
            gr.DrawLine(borderPen, rect.Left, rect.Top, rect.Right - 1, rect.Top);             // Top
            gr.DrawLine(borderPen, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom - 1); // Right
        }
    }

    private static void SetUncheckedBrushes(CheckBox checkBox, out Pen borderPen, out Brush borderEdgeBrush, out Brush backCornerBrush, out Brush backVertexBrush)
    {
        borderPen = DarkPens.CheckBox.UncheckedBorder;

        borderEdgeBrush = DarkBrushes.CheckBox.UncheckedBorderEdge;
        borderEdgeBrush = DarkBrushes.CheckBox.UncheckedBorderEdge;
        backCornerBrush = DarkBrushes.CheckBox.UncheckedBackCorner;
        backVertexBrush = DarkBrushes.CheckBox.UncheckedBackVertex;

        if (!checkBox.Enabled)
        {
            borderPen = DarkPens.CheckBox.UncheckedDisabledBorder;

            borderEdgeBrush = DarkBrushes.CheckBox.UncheckedDisabledBorderEdge;
            backCornerBrush = DarkBrushes.CheckBox.UncheckedDisabledBackCorner;
            backVertexBrush = DarkBrushes.CheckBox.UncheckedDisabledBackVertex;
        }
    }

    private static void CheckMark(CheckBox checkBox, Graphics g, Rectangle boxRect)
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

    internal static void CheckBox(CheckBox checkBox, Graphics g, Rectangle boxRect)
    {
        g.Clear(checkBox.BackColor);

        Brush backBrush = checkBox.Checked ? DarkBrushes.CheckBox.Back : DarkBrushes.CheckBox.UncheckedBack;
        g.FillRectangle(backBrush, new Rectangle(boxRect.X + 2, boxRect.Y, boxRect.Width - 3, boxRect.Height));
        g.FillRectangle(backBrush, new Rectangle(boxRect.X, boxRect.Y + 2, boxRect.Width, boxRect.Height - 3));

        //boxRect.Inflate(-2, -3);
        // Fill background
        // Draw CheckMark, if checked
        if (checkBox.Checked)
            CheckMark(checkBox, g, boxRect);

        int left = boxRect.X;
        int right = boxRect.X + boxRect.Width;
        int top = boxRect.Y;
        int bottom = boxRect.Y + boxRect.Height;

        Pen borderPen = DarkPens.CheckBox.Border;

        Brush borderEdgeBrush = DarkBrushes.CheckBox.BorderEdge;
        Brush backCornerBrush = DarkBrushes.CheckBox.BackCorner;
        Brush backVertexBrush = DarkBrushes.CheckBox.BackVertex;

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
            Brush cornerBrush = DarkBrushes.CheckBox.BorderCorner;
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
    }
}
