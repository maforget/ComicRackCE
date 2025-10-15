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

namespace cYo.Common.Windows.Forms
{
    /// <summary>
    /// Class to centralisation application Theming. When theming is enabled, calls <see cref="KnownColorTableEx"/> on initialization to override built-in <see cref="System.Drawing.SystemColors"/> with theme-defined colors.<br/>
    /// Exposes <see cref="Theme(Control)"/> for recursive <see cref="Control"/> theming by setting class fields and leveraging <see cref="UXTheme"/> for native Windows OS theming.
    /// </summary>
    public static partial class ThemeExtensions
    {
        public static Bitmap RenderDarkCheckbox(CheckBoxState state, Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                CheckBoxRenderer.DrawCheckBox(g, new Point(0, 0), state);
            }
            ImageProcessing.Invert(bmp);
            return bmp;
        }

        public static void SetToolStripItemColor(ToolStripArrowRenderEventArgs e)
        {
            if (!IsDarkModeEnabled) return;
            e.ArrowColor = Color.White;
        }

        public static void SetToolStripItemColor(ToolStripItemTextRenderEventArgs e)
        {
            if (!IsDarkModeEnabled) return;
            e.TextColor = Color.White;
        }

        public static void RenderItemCheck(Graphics graphics, Rectangle rect, Color background, Color? checkColor = null)
        {
            if (!IsDarkModeEnabled) return;

            using (Brush brush = new SolidBrush(background))
            {
                graphics.FillRectangle(brush, rect);
            }
            using (var pen = new Pen(checkColor ?? Color.White, 2))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.DrawLines(pen, new[]
                {
                    new Point(rect.Left + 4, rect.Top + rect.Height/2 - 1),
                    new Point(rect.Left + rect.Width/3 + rect.Width/6, rect.Bottom - 5),
                    new Point(rect.Right - 4, rect.Top + 3)
                });
            }
        }

        public static void RenderOverflowButton(ToolStripItemRenderEventArgs e)
        {
            if (!IsDarkModeEnabled) return;

            var g = e.Graphics;
            var item = e.Item as ToolStripOverflowButton;

            const int overflowButtonWidth = 12;
            Rectangle overflowArrowRect = new Rectangle(item.Width - overflowButtonWidth + 1, item.Height - 8, 9, 5);

            Point middle = new Point(overflowArrowRect.Left + overflowArrowRect.Width / 2, overflowArrowRect.Top + overflowArrowRect.Height / 2);
            Point[] arrow = new Point[] {
                    new Point(middle.X - 2, middle.Y - 1),
                    new Point(middle.X + 3, middle.Y - 1),
                    new Point(middle.X,     middle.Y + 2)
                };

            g.FillPolygon(SystemBrushes.ControlText, arrow);
            g.DrawLine(SystemPens.ControlText, overflowArrowRect.Right - 7, overflowArrowRect.Y - 2, overflowArrowRect.Right - 3, overflowArrowRect.Y - 2);
        }
    }
}
