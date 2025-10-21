using cYo.Common.Drawing.ExtendedColors;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme.DarkMode;
using cYo.Common.Windows.Forms.Theme.Internal;
using cYo.Common.Windows.Forms.Theme.Resources;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace cYo.Common.Windows.Forms.Theme;

/// <summary>
/// Class to centralize application Theming. When theming is enabled, calls <see cref="KnownColorTableEx"/> on initialization to override built-in <see cref="System.Drawing.SystemColors"/> with theme-defined colors.<br/>
/// Exposes <see cref="Theme(Control)"/> for recursive <see cref="Control"/> theming by setting class fields and leveraging <see cref="UXTheme"/> for native Windows OS theming.
/// </summary>
public static class ThemeExtensions
{

	/// <summary>
	/// Themes a <see cref="Control"/>, recursively. The specifics are determined by the current <see cref="ThemeHandler"/>.
	/// </summary>
	/// <param name="control"><see cref="Control"/> to be themed.</param>
	public static void Theme(this Control control) => ThemeHandler.SetTheme(control, recursive: true);


	/// <summary>
	/// Runs the provided <see cref="Action"/> only if the Theme isn't <see cref="Themes.Default"/> or if <paramref name="isDefaultAction"/> is false and we are in the <see cref="Themes.Default"/> Theme, then it will always draw.
	/// </summary>
	/// <param name="action">The <see cref="Action"/> to run</param>
	/// <param name="onlyDrawIfDefault"></param>
	/// <returns>a <see cref="bool"/> if it was successful in drawing</returns>
	public static bool InvokeAction(Action action, bool isDefaultAction = false)
	{
		if (!ThemeColors.IsDefault && !isDefaultAction || ThemeColors.IsDefault && isDefaultAction) // We are themed or we have requested to only draw when in the default 
		{
			action();
			return true;
		}
		return false;
	}

	/// <summary>
	/// Runs one of two provided <see cref="Action"/>s depending on whether <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="true"/> or <paramref name="false"/>.
	/// </summary>
	/// <param name="defaultAction">The <see cref="Action"/> to run when <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="false"/></param>
	/// <param name="darkModeAction">The <see cref="Action"/> to run when <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="true"/> </param>
	public static void InvokeAction(Action defaultAction, Action darkModeAction)
	{
		if (ThemeManager.IsDarkModeEnabled)
			darkModeAction();
		else
			defaultAction();
	}

	internal static void WhenHandleCreated(this Control control, Action<Control> onHandleCreated)
	{
		if (control.IsHandleCreated)
		{
			onHandleCreated(control);
			return;
		}

		EventHandler handler = null!;
		handler = (s, e) =>
		{
			control.HandleCreated -= handler;
			onHandleCreated(s as Control);
		};
		control.HandleCreated += handler;
	}

	// Generally, Dark Mode theming
	#region Control Extensions
	/// <summary>
	/// Apply <see cref="ThemeColors.DarkMode.UIComponent.SidePanel"/> <typeparamref name="BackColor"/> to a <see cref="Control"/>.
	/// </summary>
	/// <remarks>
	/// This cannot be applied on the <see cref="System.Type"/> as it only applies to specific instances.
	/// </remarks>
	public static void SetSidePanelColor(this Control control) => InvokeAction(() => DarkThemeExtensions.SetSidePanelColor(control));

	public static void SetComboBoxButton(this Button button) => InvokeAction(() => DarkThemeExtensions.SetComboBoxButton(button));

	public static void SetTreeViewColor(this TreeView treeView) => InvokeAction(() => DarkThemeExtensions.SetTreeViewColor(treeView));
	#endregion

	// Generally, either draw default or draw Dark Mode version
	#region Drawing Extensions

	public static void DrawThemeFocusRectangle(this DrawItemEventArgs e) => InvokeAction(
		() => e.DrawFocusRectangle(),
		() => DarkThemeExtensions.ControlPaint.DrawFocusRectangle(e)
	);

	public static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e) => InvokeAction(
		() => e.DrawDefault = true,
		() => DarkThemeExtensions.ListView_DrawColumnHeader(sender, e)
	);

	public static void DrawThemeBackground(this VisualStyleRenderer visualStyleRenderer, PaintEventArgs e, Rectangle rect, Color backColor) => InvokeAction(
		() => visualStyleRenderer.DrawBackground(e.Graphics, rect),
		() => DarkThemeExtensions.ControlPaint.DrawBackground(e, backColor)
	);

	public static void DrawThemeBackground(this VisualStyleRenderer visualStyleRenderer, DrawToolTipEventArgs e) => InvokeAction(
		() => visualStyleRenderer.DrawBackground(e.Graphics, e.Bounds),
		() => DarkThemeExtensions.ControlPaint.DrawBackground(e)
	);

	public static void DrawThemeText(this VisualStyleRenderer visualStyleRenderer, Graphics g, Color foreColor, Font font, Rectangle rect, string text, bool drawDisabled, TextFormatFlags textFormatFlags) => InvokeAction(
		() => { using (FontDC dc = new FontDC(g, font)) { visualStyleRenderer.DrawText(dc, rect, text, drawDisabled, textFormatFlags); } },
		() => TextRenderer.DrawText(g, text, font, rect, foreColor, textFormatFlags)
	);

	public static void DrawThemeText(this VisualStyleRenderer visualStyleRenderer, DrawToolTipEventArgs e, FontDC dc, Rectangle rect) => InvokeAction(
		() => visualStyleRenderer.DrawText(dc, rect, e.ToolTipText),
		() => TextRenderer.DrawText(e.Graphics, e.ToolTipText, e.Font, rect, ThemeColors.ToolTip.InfoText)
	);

	//public static void DrawThemeBackground(this DrawToolTipEventArgs e) => InvokeAction(
	//    () => e.DrawBackground(),
	//    () => DarkThemeExtensions.ControlPaint.DrawBackground(e)
	//);

	public static void DrawThemeBackground(this DrawItemEventArgs e) => InvokeAction(
		() => e.DrawBackground(),
		() => DarkThemeExtensions.ControlPaint.DrawBackground(e)
	);

	public static void DrawThemeBorder(this DrawToolTipEventArgs e) => InvokeAction(
		() => e.DrawBorder(),
		() => DarkThemeExtensions.ControlPaint.DrawBorder(e)
	);


	public static void DrawSplitButtonBase(Graphics g, Rectangle rect, PushButtonState state) => InvokeAction(() => DarkThemeExtensions.DrawSplitButtonBase(g, rect, state));
	public static void DrawTabItem(Graphics g, Rectangle rect, TabItemState tabItemState, bool buttonMode) => InvokeAction(() => DarkThemeExtensions.DrawTabItem(g, rect, tabItemState, buttonMode));
	#endregion

	#region Other Extensions
	public static string ReplaceWebColors(this string webPage)
	{
		//TODO: Add a way to disable the repalce if the plugin supports theme and doesn't need you to replace the colors
		Regex rxWebBody = new Regex(@"<body(?=[^>]*)([^>]*?)\bstyle=""([^""]*)""([^>]*)>|<body([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        string rxWebBodyReplace = "<body$1 style=\"$2background-color:#383838;color:#eeeeee;scrollbar-face-color:#4d4d4d;scrollbar-track-color:#171717;scrollbar-shadow-color:#171717;scrollbar-arrow-color:#676767;\"$3>";

        if (ThemeManager.IsDarkModeEnabled)
			return rxWebBody.Replace(webPage, rxWebBodyReplace);
		else
			return webPage;
	}

	public static Color GetTabBarBorderColor(this VisualStyleRenderer visualStyleRenderer, ColorProperty colorProperty)
	{
		if (ThemeManager.IsDarkModeEnabled)
			return ThemeColors.TabBar.DefaultBorder;
		else
			return visualStyleRenderer.GetColor(colorProperty);
	}

	#endregion

	// TODO: ToolStripRenderers should be better defined. Current ToolStripRenderer should be provided by registered ThemeHandler
	//public static ToolStripRenderer GetToolStripRenderer(bool systemToolBars, ProfessionalColorTable vintageColorTable)
	//{
	//    if (IsDarkModeEnabled)
	//        return new ThemeToolStripProRenderer(new DarkProfessionalColors());

	//    if (systemToolBars)
	//        return new ToolStripSystemRenderer();

	//    ToolStripRenderer renderer = new ToolStripProfessionalRenderer(vintageColorTable)
	//    {
	//        RoundedEdges = false
	//    };
	//    return renderer;
	//}

	// TODO: ToolStripRenderers should be better defined. These methods should not be required
	#region ToolStripHelpers
	//public static void SetToolStripItemColor(ToolStripArrowRenderEventArgs e) => Helpers.ToolStripHelpers.SetToolStripItemColor(e);

	//public static void SetToolStripItemColor(ToolStripItemTextRenderEventArgs e) => Helpers.ToolStripHelpers.SetToolStripItemColor(e);

	//public static void RenderItemCheck(Graphics g, Rectangle r, Color b, Color? cc = null) => Helpers.ToolStripHelpers.RenderItemCheck(g, r, b, cc);
	#endregion


}

public static class ControlPaintEx
{
	public static void DrawBorder3D(Graphics graphics, Rectangle bounds, Border3DStyle borderStyle)
	{
		ThemeExtensions.InvokeAction(
			() => ControlPaint.DrawBorder3D(graphics, bounds, borderStyle),
			() => DarkThemeExtensions.ControlPaint.DrawBorder(graphics, bounds)
		);
	}

	public static void DrawFocusRectangle(Graphics graphics, Rectangle rect)
	{
		ThemeExtensions.InvokeAction(
			() => ControlPaint.DrawFocusRectangle(graphics, rect),
			() => DarkThemeExtensions.ControlPaint.DrawFocusRectangle(graphics, rect)
		);
	}

	public static void DrawStringDisabled(Graphics g, string text, Font font, Color color, Rectangle rect, TextFormatFlags textFormatFlags)
	{
		ThemeExtensions.InvokeAction(
			() => ControlPaint.DrawStringDisabled(g, text, font, color, rect, textFormatFlags),
			() => TextRenderer.DrawText(g, text, font, rect, SystemColors.GrayText, textFormatFlags)
		);
	}

	//public static void DrawFocusRectangle(DrawItemEventArgs e, bool shouldDrawFocus, bool shouldDrawBackground = false)
	//{
	//    if (ThemeExtensions.IsDarkModeEnabled && shouldDrawFocus && shouldDrawBackground)
	//        e.Graphics.FillRectangle(ThemeBrushes.DarkMode.SelectedText.Highlight, e.Bounds);

	//    if (ThemeExtensions.IsDarkModeEnabled && shouldDrawFocus)
	//        ControlPaint.DrawBorder(e.Graphics, e.Bounds, ThemeColors.DarkMode.SelectedText.Focus, ButtonBorderStyle.Solid);
	//    else
	//        e.DrawFocusRectangle();
	//}

	//public static void DrawFocusRectangle(Graphics graphics, Rectangle rect, bool shouldDrawBackground = false)
	//{
	//    if (ThemeExtensions.IsDarkModeEnabled && shouldDrawBackground)
	//        graphics.FillRectangle(ThemeBrushes.DarkMode.SelectedText.Highlight, rect);

	//    if (ThemeExtensions.IsDarkModeEnabled)
	//        ControlPaint.DrawBorder(graphics, rect, ThemeColors.DarkMode.SelectedText.Focus, ButtonBorderStyle.Solid);
	//    else
	//        ControlPaint.DrawFocusRectangle(graphics, rect);
	//}
}