using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms.Theme.DarkMode;

internal static class DarkThemeExtensions
{
    internal static void SetSidePanelColor(this Control control)
    {
        control.BackColor = DarkColors.UIComponent.SidePanel;
        if (control.GetType() == typeof(TreeView) || control.GetType() == typeof(TreeViewEx))
            TreeViewEx.SetColor((TreeView)control, DarkColors.UIComponent.SidePanel);
    }

    internal static void SetTreeViewColor(this TreeView treeView)
    {
        treeView.BackColor = DarkColors.TreeView.Back;
        treeView.ForeColor = DarkColors.TreeView.Text;
        TreeViewEx.SetColor(treeView, DarkColors.TreeView.Back, DarkColors.TreeView.Text);
    }

    // Removing this might require using <see cref="UXTheme"/> + OpenThemeData + GetTheme*, which is not worth the effort.
    // Alternatively could change the Designer Location/Size, but that will affect the default theme.
    internal static void SetComboBoxButton(this Button button)
    {
        button.Location = new Point(button.Location.X, button.Location.Y + 1);
        button.Size = new Size(button.Size.Width, button.Size.Height - 2);
        button.BackColor = DarkColors.Button.Back;
        button.ForeColor = DarkColors.Button.Text;
    }

    #region DarkMode.Rendering
    internal static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) => DrawDarkListView.ColumnHeader(sender, e);

    internal static void DrawSplitButtonBase(Graphics graphics, Rectangle rect, PushButtonState state) => DrawDark.ButtonBase(graphics, rect, state);
    internal static void DrawTabItem(Graphics g, Rectangle rect, TabItemState tabItemState, bool buttonMode) => DrawDark.TabItem(g, rect, tabItemState, buttonMode);

    internal static class ControlPaint
    {
        internal static void DrawBackground(PaintEventArgs e, Color backColor) => DarkControlPaint.DrawBackground(e, backColor);
        internal static void DrawBackground(DrawItemEventArgs e) => DarkControlPaint.DrawBackground(e);
        internal static void DrawBackground(DrawToolTipEventArgs e) => DarkControlPaint.DrawBackground(e);
        internal static void DrawBorder(Graphics g, Rectangle bounds) => DarkControlPaint.DrawBorder(g, bounds);
        internal static void DrawBorder(DrawToolTipEventArgs e) => DarkControlPaint.DrawBorder(e);

        internal static void DrawFocusRectangle(DrawItemEventArgs e) => DarkControlPaint.DrawFocusRectangle(e);

        internal static void DrawFocusRectangle(Graphics g, Rectangle rect) => DarkControlPaint.DrawFocusRectangle(g, rect);
    }
    #endregion
}
