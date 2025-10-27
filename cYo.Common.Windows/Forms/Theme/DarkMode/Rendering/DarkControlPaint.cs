using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;

internal static class DarkControlPaint
{

    #region DrawBorder
    internal static void DrawBorder(Graphics g, Rectangle bounds, Border3DStyle borderStyle) => DrawBorder(g, bounds);

    internal static void DrawBorder(Graphics g, Rectangle bounds)
    {
        ControlPaint.DrawBorder(g, bounds, DarkColors.Border.Default, ButtonBorderStyle.Solid);
    }
    #endregion

    #region DrawFocusRectangle
    internal static void DrawFocusRectangle(DrawItemEventArgs e)
    {
        if ((e.State & DrawItemState.Focus) == DrawItemState.Focus && (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
        {
            Rectangle bounds = e.Bounds;
            bounds.Width--;
            bounds.Height--;
            //ControlPaint.DrawFocusRectangle(e.Graphics, e.Bounds, e.ForeColor, e.BackColor);
            e.Graphics.DrawRectangle(DarkPens.SelectedText.Focus, bounds);
        }
    }

    public static void DrawFocusRectangle(Graphics graphics, Rectangle rectangle)
    {
        DrawFocusRectangle(graphics, rectangle, SystemColors.ControlText, SystemColors.Control);
    }

    public static void DrawFocusRectangle(Graphics graphics, Rectangle rectangle, Color foreColor, Color backColor)
    {
        DrawFocusRectangle(graphics, rectangle, backColor, highContrast: false);
    }

    private static void DrawFocusRectangle(Graphics graphics, Rectangle rectangle, Color color, bool highContrast)
    {
        rectangle.Width--;
        rectangle.Height--;
        graphics.DrawRectangle(DarkPens.SelectedText.Focus, rectangle);
    }
    #endregion

    #region DrawBackground
    internal static void DrawBackground(PaintEventArgs e, Color backColor)
    {
        //e.Graphics.Clear(backColor); // emulating non-VisualStyleRenderer path for now
        e.Graphics.Clear(backColor);
    }

    internal static void DrawBackground(DrawItemEventArgs e)
    {
        //e.Graphics.Clear(backColor); // emulating non-VisualStyleRenderer path for now
        //e.DrawThemeBackground();
        e.DrawBackground();

        if (e.State.HasFlag(DrawItemState.Selected) || e.State.HasFlag(DrawItemState.HotLight) || e.State.HasFlag(DrawItemState.Focus))
            e.Graphics.FillRectangle(DarkBrushes.SelectedText.Highlight, e.Bounds);
    }

    internal static void DrawBackground(DrawToolTipEventArgs e)
    {
        e.Graphics.FillRectangle(DarkBrushes.ToolTip.Back, new Rectangle(Point.Empty, e.Bounds.Size));
    }

    internal static void DrawBorder(DrawToolTipEventArgs e)
    {

    }
    #endregion
}
