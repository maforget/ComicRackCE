using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Drawing.ExtendedColors;

namespace cYo.Common.Windows.Forms.Theme.Resources;

/// <summary>
/// Theme-aware see cref="ToolStripProfessionalRenderer"/>.
/// </summary>
public class ThemeToolStripProRenderer : ToolStripProfessionalRenderer
{
    public ThemeToolStripProRenderer()
        : base(new ProfessionalColorTableEx())
    {
    }

    public ThemeToolStripProRenderer(ProfessionalColorTable professionalColorTable)
        : base(professionalColorTable)
    {
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        ThemeExtensions.InvokeAction(() => e.TextColor = Color.White);
        base.OnRenderItemText(e);
    }

    protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
    {
        ThemeExtensions.InvokeAction(() => e.ArrowColor = Color.White);
        base.OnRenderArrow(e);
    }

    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
        base.OnRenderItemCheck(e);
        ThemeExtensions.InvokeAction(() => RenderDarkItemCheck(e.Graphics, e.ImageRectangle, ColorTable.CheckPressedBackground));
    }

    protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
    {
        base.OnRenderOverflowButtonBackground(e);
        ThemeExtensions.InvokeAction(() => RenderDarkOverflowButton(e));
    }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        ThemeExtensions.InvokeAction(() => base.OnRenderToolStripBorder(e), isDefaultAction: true);
    }

    public static void RenderDarkItemCheck(Graphics graphics, Rectangle rect, Color background, Color? checkColor = null)
    {
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

    public static void RenderDarkOverflowButton(ToolStripItemRenderEventArgs e)
    {
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

    public static void SetToolStripItemThemeColor(ToolStripArrowRenderEventArgs e) => ThemeExtensions.InvokeAction(() => e.ArrowColor = Color.White);

    public static void SetToolStripItemThemeColor(ToolStripItemTextRenderEventArgs e) => ThemeExtensions.InvokeAction(() => e.TextColor = Color.White);
}