using cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms.Theme.DarkMode;

/// <summary>
/// Provides Dark Mode extension methods to be used outside the <see cref="DarkMode"/> namespace.
/// </summary>
/// <summary>
/// Provides extended Dark Mode-related functionality for use outside of the <see cref="Theme.DarkMode"/> namespace.
/// </summary>
/// <remarks>
/// Methods generally extend <see cref="Control"/> or forward delegates to <see cref="DrawDark"/>/<see cref="GraphicsExtensions"/>
/// </remarks>
internal static class DarkThemeExtensions
{
    #region Control Extensions
    // Provide a way to color controls based on their UI "role" as oppose to their Type
    internal static void SetUIComponentColor(this Control control, UIComponent component)
    {
        Color backColor = DarkColors.GetUIComponentColor(component);
        if (backColor != Color.Empty)
        {
            control.BackColor = backColor;
            if (control.GetType().IsSubclassOf(typeof(TreeView)))
                TreeViewEx.SetColor((TreeView)control, backColor);
        }
    }

    /// <summary>
	/// Sets <see cref="TreeView.ForeColor"/> and <see cref="TreeView.BackColor"/>.
	/// </summary>
    /// <remarks>
    /// This is not done purely via <see cref="Theme.Resources.ThemeColors"/> as additional Win32 method call is required.<br/>
    /// <see cref="DarkControlDefinition"/> is based on <see cref="System.Type"/> and is not suitable for instance-specific settings.
    /// </remarks>
    // REVIEW : Review why this is required when TreeView base calls SetColor() equivalent
    internal static void SetTreeViewColor(this TreeView treeView)
    {
        treeView.BackColor = DarkColors.TreeView.Back;
        treeView.ForeColor = DarkColors.TreeView.Text;
        TreeViewEx.SetColor(treeView, DarkColors.TreeView.Back, DarkColors.TreeView.Text);
    }

    // HACK: Re-size button because its "theme parts" are larger in Dark Mode than in Light Mode
    // Removing this might require using <see cref="UXTheme"/> + OpenThemeData + GetTheme*, which is not worth the effort.
    // Alternatively could change the Designer Location/Size, but that will affect the default theme.
    /// <summary>
	/// Resizes <see cref="Button"/> pretending to be a <see cref="ComboBox"/> control.<br/>
    /// Because it's larger than the Light Mode counterpart but needs to fit in the same bounds (otherwise borders are chopped off)
	/// </summary>
    internal static void SetComboBoxButton(this Button button)
    {
        button.Location = new Point(button.Location.X, button.Location.Y + 1);
        button.Size = new Size(button.Size.Width, button.Size.Height - 2);
        button.BackColor = DarkColors.Button.Back;
        button.ForeColor = DarkColors.Button.Text;
    }
    #endregion

    // Provide a single point of entry for ThemeExtensions methods which need to make use of DarkMode internal methods
    #region Drawing Extensions [DarkMode.Rendering]
    internal static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        => DrawDarkListView.ColumnHeader(sender, e);

    internal static void DrawSplitButtonBase(Graphics graphics, Rectangle rect, PushButtonState state)
        => DrawDark.ButtonBase(graphics, rect, state);

    internal static void DrawTabItem(Graphics g, Rectangle rect, TabItemState tabItemState, bool buttonMode)
        => DrawDark.TabItem(g, rect, tabItemState, buttonMode);

    // ControlPaintEx -> DarkControlPaint bridge.
    // REVIEW : Maybe should just subclass...? ControlPaintEx : DarkControlPaint
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

	#region Other
    // Modifies an HTML page to add a style attribute to the body that will replace colors for a dark mode
	internal static string ReplaceWebColors(this string webPage)
	{
		Regex rxWebBody = new Regex(@"<body(?=[^>]*)([^>]*?)\bstyle=""([^""]*)""([^>]*)>|<body([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
		string rxWebBodyReplace = "<body$1 style=\"$2background-color:#383838;color:#eeeeee;scrollbar-face-color:#4d4d4d;scrollbar-track-color:#171717;scrollbar-shadow-color:#171717;scrollbar-arrow-color:#676767;\"$3>";

		return rxWebBody.Replace(webPage, rxWebBodyReplace);
	}
	#endregion
}
