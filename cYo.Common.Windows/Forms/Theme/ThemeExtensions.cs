using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme.DarkMode;
using cYo.Common.Windows.Forms.Theme.Internal;
using cYo.Common.Windows.Forms.Theme.Resources;

namespace cYo.Common.Windows.Forms.Theme;

/// <summary>
/// Provides extended Theme-related functionality for use outside of the <see cref="Forms.Theme"/> namespace, most notably <see cref="Theme"/>.
/// </summary>
public static class ThemeExtensions
{
    /// <summary>
    /// Themes a <see cref="Control"/>, recursively. The specifics are determined by the current <see cref="ThemeHandler"/>.
    /// </summary>
    /// <param name="control"><see cref="Control"/> to be themed. Child controls will be themed.</param>
    public static void Theme(this Control control) => ThemeHandler.SetTheme(control, recursive: true);

    /// <summary>
    /// Themes a <see cref="Control"/>. The specifics are determined by the current <see cref="ThemeHandler"/>.
    /// </summary>
    /// <param name="control"><see cref="Control"/> to be themed. Child controls will not be themed.</param>
    public static void ThemeControl(this Control control) => ThemeHandler.SetTheme(control, recursive: false);

    /// <summary>
    /// Runs the provided <see cref="Action"/> only if the Theme isn't <see cref="Themes.Default"/> or if <paramref name="isDefaultAction"/> is false and we are in the <see cref="Themes.Default"/> Theme, then it will always draw.
    /// </summary>
    /// <param name="action">The <see cref="Action"/> to run</param>
    /// <param name="isDefaultAction"></param>
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
    /// <param name="defaultAction"><see cref="Action"/> to run when <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="false"/></param>
    /// <param name="darkModeAction"><see cref="Action"/> to run when <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="true"/> </param>
    public static void InvokeAction(Action defaultAction, Action darkModeAction)
    {
        if (ThemeManager.IsDarkModeEnabled)
            darkModeAction();
        else
            defaultAction();
    }

    /// <summary>
    /// Runs one of two provided <see cref="Func{TResult}"/> depending on whether <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="true"/> or <paramref name="false"/>.
    /// </summary>
    /// <param name="defaultFunc"><see cref="Func{TResult}"/> to run when <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="false"/></param>
    /// <param name="darkModeFunc"><see cref="Func{TResult}"/> to run when <see cref="ThemeManager.IsDarkModeEnabled"/> is <paramref name="true"/> </param>
    public static TResult InvokeFunc<TResult>(Func<TResult> defaultFunc, Func<TResult> darkModeFunc)
    {
        if (ThemeManager.IsDarkModeEnabled)
            return darkModeFunc();
        else
            return defaultFunc();
    }

    /// <summary>
    /// Invokes <see cref="Action{Control}"/> if <see cref="Control.IsHandleCreated"/>, and subscribes to
    /// <see cref="Control.OnHandleCreated(EventArgs)"/> otherwise.
    /// </summary>
    internal static void WhenHandleCreated(this Control control, Action<Control> handleCreatedAction)
    {
        if (handleCreatedAction == null)
            return;

        if (control.IsHandleCreated)
        {
            handleCreatedAction(control);
            return;
        }

        EventHandler handler = null!;
        handler = (s, e) =>
        {
            control.HandleCreated -= handler;
            handleCreatedAction(s as Control);
        };
        control.HandleCreated += handler;
    }

    // Generally, Dark Mode instance theming
    #region Control Extensions
    /// <summary>
    /// Apply <see cref="UIComponent.SidePanel"/> <typeparamref name="BackColor"/> to a <see cref="Control"/>.
    /// </summary>
    /// <remarks>
    /// This cannot be applied on the <see cref="System.Type"/> as it only applies to specific instances.
    /// </remarks>
    public static void SetSidePanelColor(this Control control)
        => InvokeAction(() => DarkThemeExtensions.SetUIComponentColor(control, UIComponent.SidePanel));

    /// <summary>
    /// Apply <see cref="UIComponent.Content"/> <typeparamref name="BackColor"/> to a <see cref="Control"/>.
    /// </summary>
    public static void SetContentColor(this Control control)
        => InvokeAction(() => DarkThemeExtensions.SetUIComponentColor(control, UIComponent.Content));

    /// <summary>
    /// Apply <see cref="UIComponent.Window"/> <typeparamref name="BackColor"/> to a <see cref="Control"/>.
    /// </summary>
    public static void SetWindowColor(this Control control)
        => InvokeAction(() => DarkThemeExtensions.SetUIComponentColor(control, UIComponent.Window));

    /// <summary>
    /// Resizes <see cref="Button"/> pretending to be a <see cref="ComboBox"/> control.
    /// </summary>
    public static void SetComboBoxButton(this Button button)
        => InvokeAction(() => DarkThemeExtensions.SetComboBoxButton(button));

    /// <summary>
    /// <term>DarkMode <see cref="Action"/></term>
    /// <description>Set <see cref="TreeView.ForeColor"/> and <see cref="TreeView.BackColor"/></description>
    /// </summary>
    public static void SetTreeViewColor(this TreeView treeView)
        => InvokeAction(() => DarkThemeExtensions.SetTreeViewColor(treeView));
    #endregion
    
    public static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        => InvokeAction(
            () => e.DrawDefault = true,
            () => DarkThemeExtensions.ListView_DrawColumnHeader(sender, e)
        );

    public static string ReplaceWebColors(this string webPage)
        => InvokeFunc(
            () => webPage,
            () => DarkThemeExtensions.ReplaceWebColors(webPage)
    );
}

/// <summary>
/// Provides methods used to render a <see cref="Button"/> control with visual styles. Supports <see cref="Themes"/>.
/// </summary>
public static class ButtonRendererEx
{
    /// <summary>
    /// Draws a <b>themed</b> <see cref="Button"/> control in the specified <paramref name="state"/> and <paramref name="bounds"/>.
    /// </summary>
    /// <param name="g">The <see cref="Graphics"/> used to draw the button.</param>
    /// <param name="bounds">The <see cref="Rectangle"/> that specifies the bounds of the button.</param>
    /// <param name="state">One of the <see cref="PushButtonState"/> values that specifies the visual state of the button.</param>
    public static void DrawButton(Graphics g, Rectangle bounds, PushButtonState state)
        => ThemeExtensions.InvokeAction(
            () => ButtonRenderer.DrawButton(g, bounds, state),
            () => DarkRenderer.DrawButton(g, bounds, state)
        );
}

/// <summary>
/// Provides methods used to paint common Windows <see cref="Control"/>s and their elements. Supports <see cref="Themes"/>.
/// </summary>
public static class ControlPaintEx
{
    public static void DrawBorder3D(Graphics graphics, Rectangle rectangle, Border3DStyle borderStyle)
        => ThemeExtensions.InvokeAction(
            () => ControlPaint.DrawBorder3D(graphics, rectangle, borderStyle),
            () => DarkControlPaint.DrawBorder(graphics, rectangle)
        );
    
    /// <summary>
    /// Draws a <b>themed</b> focus rectangle on the specified <paramref name="graphics"/> surface and within the specified <paramref name="bounds"/>.
    /// </summary>
    /// <param name="graphics">The <see cref="Graphics"/> to draw on.</param>
    /// <param name="bounds">The <see cref="Rectangle"/> that represents the dimensions of the grab handle glyph. (WTH is a "grab handle glyph"?)</param>
    public static void DrawFocusRectangle(Graphics graphics, Rectangle bounds)
        => ThemeExtensions.InvokeAction(
            () => ControlPaint.DrawFocusRectangle(graphics, bounds),
            () => DarkControlPaint.DrawFocusRectangle(graphics, bounds)
        );

    /// <summary>
    /// Draws the specified <paramref name="text"/> in a <b>themed</b> disabled state on the specified <paramref name="graphics"/> surface,
    ///  within the specified <paramref name="bounds"/>, and in the specified <paramref name="font"/>, <paramref name="color"/>, and <paramref name="textFormatFlags"/>,
    ///  using the specified GDI-based <see cref="TextRenderer"/>.
    /// </summary>
    /// <param name="graphics">The <see cref="Graphics"/> to draw on. And apparently the <see cref="TextRenderer"/></param>
    /// <param name="text">The <see cref="string"/> to draw.</param>
    /// <param name="font">The <see cref="Font"/> to draw the string with.</param>
    /// <param name="color">The <see cref="Color"/> of the background behind the string.</param>
    /// <param name="bounds">The <see cref="Rectangle"/> that represents the dimensions of the string.</param>
    /// <param name="textFormatFlags">The <see cref="TextFormatFlags"/> to apply to the string.</param>
    public static void DrawStringDisabled(Graphics graphics, string text, Font font, Color color, Rectangle bounds, TextFormatFlags textFormatFlags)
        => ThemeExtensions.InvokeAction(
            () => ControlPaint.DrawStringDisabled(graphics, text, font, color, bounds, textFormatFlags),
            () => DarkControlPaint.DrawStringDisabled(graphics, text, font, bounds, textFormatFlags)
        );
}

/// <summary>
/// Provides <see cref="Control"/>.Draw <see cref="EventArgs"/> extension methods. Supports <see cref="Themes"/>.
/// </summary>
public static class DrawEventExtensions
{
    /// <summary>
    /// Draws the <b>themed</b> background within the bounds specified in the <see cref="DrawItemEventArgs"/> constructor and with the appropriate color.
    /// </summary>
    /// <remarks>
    /// <see cref="DrawItemEventArgs.State"/> can be inaccurate; <see cref="Control.Focused"/> should be passed via <paramref name="focused"/>
    ///  to handle those cases.<br/>Seems to be for a limited number of Win32 controls: <see cref="ComboBox"/> and <see cref="ListBox"/>.
    /// </remarks>
    /// <param name="e">The <see cref="DrawItemEventArgs"/> instance containing the event data.</param>
    /// <param name="focused">Indicates <see cref="Control.Focused"/>. Used in addition to <see cref="DrawItemEventArgs.State"/> to determine appropriate color.</param>
    public static void DrawThemeBackground(this DrawItemEventArgs e, bool focused = false)
        => ThemeExtensions.InvokeAction(
            () => e.DrawBackground(),
            () => e.DrawDarkBackground(controlFocused: focused)
        );

    /// <summary>
    /// Draws a <b>themed</b> focus rectangle within the bounds specified in the <see cref="DrawItemEventArgs"/> constructor and with the appropriate color.
    /// </summary>
    /// <remarks>
    /// <see cref="DrawItemEventArgs.State"/> can be inaccurate; <see cref="Control.Focused"/> should be passed via <paramref name="focused"/>
    ///  to handle those cases.<br/>Seems to be for a limited number of Win32 controls: <see cref="ComboBox"/> and <see cref="ListBox"/>.
    /// </remarks>
    /// <param name="e">The <see cref="DrawItemEventArgs"/> instance containing the event data.</param>
    /// <param name="focused">Indicates <see cref="Control.Focused"/>. Used in addition to <see cref="DrawItemEventArgs.State"/> to determine appropriate color.</param>
    public static void DrawThemeFocusRectangle(this DrawItemEventArgs e, bool focused = false)
        => ThemeExtensions.InvokeAction(
            () => e.DrawFocusRectangle(),
            () => e.DrawDarkFocusRectangle(controlFocused: focused)
        );

    /// <summary>
    /// Draws a <b>themed</b> focus rectangle within the specified <paramref name="bounds"/> specified and with the appropriate color.
    /// </summary>
    /// <remarks>
    /// <see cref="DrawItemEventArgs.State"/> can be inaccurate; <see cref="Control.Focused"/> should be passed via <paramref name="focused"/>
    ///  to handle those cases.<br/>Seems to be for a limited number of Win32 controls: <see cref="ComboBox"/> and <see cref="ListBox"/>.
    /// </remarks>
    /// <param name="e">The <see cref="DrawItemEventArgs"/> instance containing the event data.</param>
    /// <param name="bounds">The <see cref="Rectangle"/> that specifies the bounds of the focus rectangle.</param>
    /// <param name="focused">Indicates <see cref="Control.Focused"/>. Used in addition to <see cref="DrawItemEventArgs.State"/> to determine appropriate color.</param>
    public static void DrawThemeFocusRectangle(this DrawItemEventArgs e, Rectangle bounds, bool focused = false)
        => ThemeExtensions.InvokeAction(
            () => e.DrawFocusRectangle(bounds), // CheckedListBoxEx.OnDrawItem
            () => e.DrawDarkFocusRectangle(bounds, controlFocused: focused)
        );

    /// <summary>
    /// Draws the <b>themed</b> background of the <see cref="ListViewItem.ListViewSubItem"/> using its current background color.
    /// </summary>
    public static void DrawThemeBackground(this DrawListViewSubItemEventArgs e)
        => ThemeExtensions.InvokeAction(
            () => e.DrawBackground(),
            () => e.DrawDarkBackground()
        );

    /// <summary>
    /// Draws the <b>themed</b> background of the <see cref="ToolTip"/> using the appropriate color.
    /// </summary>
    public static void DrawThemeBackground(this DrawToolTipEventArgs e)
        => ThemeExtensions.InvokeAction(
            () => e.DrawBackground(),
            () => e.DrawDarkBackground()
        );

    /// <summary>
    /// Draws the <b>themed</b> border of the <see cref="ToolTip"/> using the appropriate color.
    /// </summary>
    public static void DrawThemeBorder(this DrawToolTipEventArgs e)
        => ThemeExtensions.InvokeAction(
            () => e.DrawBorder(),
            () => e.DrawDarkBorder()
        );

    // override e.Bounds used by native e.DrawFocusRectangle()
    private static void DrawFocusRectangle(this DrawItemEventArgs e, Rectangle bounds)
    {
        if ((e.State & DrawItemState.Focus) != 0 && (e.State & DrawItemState.NoFocusRect) == 0)
            ControlPaint.DrawFocusRectangle(e.Graphics, bounds);
    }
}

/// <summary>
/// Provides methods used to render a <see cref="TabControl"/> with visual styles. Supports <see cref="Themes"/>.
/// </summary>
public static class TabRendererEx
{
    /// <summary>
    /// Draws a <b>themed</b> tab in the specified <paramref name="state"/> and <paramref name="bounds"/>.
    /// </summary>
    /// <param name="g">The <see cref="Graphics"/> used to draw the tab.</param>
    /// <param name="bounds">The <see cref="Rectangle"/> that specifies the bounds of the tab.</param>
    /// <param name="state">One of the <see cref="TabItemState"/> values that specifies the visual state of the tab.</param>
    public static void DrawTabItem(Graphics g, Rectangle bounds, TabItemState state)
        => ThemeExtensions.InvokeAction(
            () => TabRenderer.DrawTabItem(g, bounds, state),
            () => DarkRenderer.DrawTabItem(g, bounds, state)
        );
}

/// <summary>
/// Provides methods for drawing and getting information about a <see cref="VisualStyleElement"/>. Supports <see cref="Themes"/>.
/// </summary>
public static class VisualStyleRendererEx
{
    /// <summary>
    /// Draws the <b>themed</b> background image of the current visual style element within the specified bounding rectangle.
    /// </summary>
    /// <param name="dc">The <see cref="IDeviceContext"/> used to draw the background image.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> in which the background image is drawn.</param>
    public static void DrawThemeBackground(this VisualStyleRenderer vsr, IDeviceContext dc, Rectangle bounds)
        => ThemeExtensions.InvokeAction(
            () => vsr.DrawBackground(dc, bounds),
            () => vsr.DrawDarkBackground(dc, bounds)
        );

    /// <summary>
    /// Draws <b>themed</b> text in the specified bounding rectangle with the option of displaying disabled text and applying other text formatting.
    /// </summary>
    /// <param name="dc">The <see cref="IDeviceContext"/> used to draw the text.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> in which to draw the text.</param>
    /// <param name="textToDraw">The text to draw.</param>
    /// <param name="drawDisabled">true to draw grayed-out text; otherwise, false.</param>
    /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags"/> values.</param>
    public static void DrawThemeText(this VisualStyleRenderer vsr, FontDC dc, Rectangle bounds, string textToDraw, bool drawDisabled, TextFormatFlags flags)
        => ThemeExtensions.InvokeAction(
            () => vsr.DrawText(dc, bounds, textToDraw, drawDisabled, flags),
            () => vsr.DrawDarkText(dc, bounds, textToDraw, drawDisabled, flags)
        );

    /// <summary>
    /// Draws <b>themed</b> text in the specified bounds using default formatting.
    /// </summary>
    /// <param name="dc">The <see cref="IDeviceContext"/> used to draw the text.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> in which to draw the text.</param>
    /// <param name="textToDraw">The text to draw.</param>
    public static void DrawThemeText(this VisualStyleRenderer vsr, FontDC dc, Rectangle bounds, string textToDraw)
        => ThemeExtensions.InvokeAction(
            () => vsr.DrawText(dc, bounds, textToDraw),
            () => vsr.DrawDarkText(dc, bounds, textToDraw)
        );

    /// <summary>
    /// Returns the value of the specified color <paramref name="property"/>  for the current visual style element and theme.
    /// </summary>
    /// <param name="property">
    /// One of the <see cref="ColorProperty"/> values that specifies which property value to retrieve for the current visual style element.
    /// </param>
    /// <returns>
    /// A <see cref="Color"/> that contains the value of the property specified by the <paramref name="property"/> parameter for the current visual style element and theme.
    /// </returns>
    public static Color GetThemeColor(this VisualStyleRenderer vsr, ColorProperty property)
        => ThemeExtensions.InvokeFunc(
            () => vsr.GetColor(property),
            () => vsr.GetDarkColor(property)
        );
}

/// <summary>
/// Extension methods used to attach <see cref="ITheme"/> to <see cref="Control"/> objects.<br/>
/// Used for outside objects like plugins that need it for selecting the <see cref="UIComponent"/>
/// </summary>
public static class ThemeBinding
{
    private static readonly ConditionalWeakTable<Control, ITheme> _attachedThemes = new();

    public static void AttachTheme(this Control control, ITheme theme)
    {
        if (control == null || theme == null)
            return;

        _attachedThemes.Remove(control);
        _attachedThemes.Add(control, theme);
    }

    public static ITheme GetAttachedTheme(this Control control)
    {
        return control != null && _attachedThemes.TryGetValue(control, out var theme) ? theme : null;
    }
}