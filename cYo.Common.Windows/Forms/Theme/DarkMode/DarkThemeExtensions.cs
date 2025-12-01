using cYo.Common.Win32;
using cYo.Common.Windows.Forms.Theme.DarkMode.Rendering;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Resources;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms.Theme.DarkMode;

/// <summary>
/// Provides Dark Mode extension methods to be used outside of the <see cref="DarkMode"/> namespace.
/// </summary>
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

    internal static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        => DrawDarkListView.ColumnHeader(sender, e);

    /// <summary>
    /// Modifies an HTML page to add a style attribute to the body that will replace colors for a dark mode
    /// </summary>
    /// <param name="webPage">HTML web page to modify.</param>
    /// <returns></returns>
    internal static string ReplaceWebColors(this string webPage)
    {
        Regex rxWebBody = new Regex(@"<body(?=[^>]*)([^>]*?)\bstyle=""([^""]*)""([^>]*)>|<body([^>]*)>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        string rxWebBodyReplace = "<body$1 style=\"$2background-color:#383838;color:#eeeeee;scrollbar-face-color:#4d4d4d;scrollbar-track-color:#171717;scrollbar-shadow-color:#171717;scrollbar-arrow-color:#676767;\"$3>";

        return rxWebBody.Replace(webPage, rxWebBodyReplace);
    }
}

internal static class DarkControlPaint
{
    // Provides parity with ControlPaint API.
    // A 2D FixedSingle border will be drawn regardless of borderStyle
    public static void DrawBorder3D(Graphics graphics, Rectangle bounds, Border3DStyle borderStyle)
        => DrawBorder(graphics, bounds);

    public static void DrawBorder(Graphics graphics, Rectangle bounds)
        => DrawBorder(graphics, bounds, DarkColors.Border.Default);

    public static void DrawBorder(Graphics g, Rectangle bounds, Color borderColor)
    {
        g.DrawRectangle(DarkPens.FromDarkColor(borderColor), bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
    }

    public static void DrawFocusRectangle(Graphics graphics, Rectangle bounds)
        => DrawFocusRectangle(graphics, bounds, DarkColors.SelectedText.Focus);

    public static void DrawFocusRectangle(Graphics graphics, Rectangle bounds, Color focusColor)
    {
        if (focusColor == Color.Empty || bounds == Rectangle.Empty) return;

        bounds.Width--;
        bounds.Height--;
        graphics.DrawRectangle(DarkPens.FromDarkColor(focusColor), bounds);
    }

    // Provides parity with ControlPaint API.
    // SystemColorsEx.GrayText will be used regardless of color
    public static void DrawStringDisabled(Graphics graphics, string text, Font font, Color color, Rectangle bounds, TextFormatFlags textFormatFlags)
        => DrawStringDisabled(graphics, text, font, bounds, textFormatFlags);

    public static void DrawStringDisabled(Graphics graphics, string text, Font font, Rectangle bounds, TextFormatFlags textFormatFlags)
        => TextRenderer.DrawText(graphics, text, font, bounds, SystemColors.GrayText, textFormatFlags); 
}

/// <summary>
/// Draw <see cref="EventArgs"/> Extensions. Provide parity with native draw <see cref="EventArgs"/> methods, with <see cref="ThemeExtensions.InvokeAction"/> determining which method should be called.<br/>
/// Actual drawing is handled in <see cref="GraphicsExtensions"/> or <see cref="DarkControlPaint"/>.
/// </summary>
/// <remarks>
/// Mostly <see cref="DrawItemEventArgs"/> methods.
/// </remarks>
internal static class DrawEventExtensions
{
    public enum ItemHighlightStyle
    {
        // Item is not Selected/Focused, or item is disabled or ComboBox Edit.
        None,

        // Item is Selected. Item or Control has Focus
        Active,

        // Item is Selected. Neither Item nor Control has Focus
        Inactive

        // Item is not Selected but has (keyboard) Focus is not handled
    }

    internal static void DrawDarkFocusRectangle(this DrawItemEventArgs e, Rectangle bounds, ItemHighlightStyle style)
    {
        // if ((e.State & DrawItemState.Focus) == DrawItemState.Focus && (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
        DarkControlPaint.DrawFocusRectangle(e.Graphics, bounds, GetFocusColor(style));
    }

    internal static void DrawDarkBackground(this DrawItemEventArgs e, Color backColor, bool controlFocused = false)
    {
        // Re-evaluate the backColor using the Control focus state as well as the Item focus state
        backColor = GetBackColor(e.State, backColor, controlFocused);
        e.Graphics.FillRectangle(DarkBrushes.FromDarkColor(backColor),e.Bounds);
    }

    //internal static void DrawDarkString(this DrawItemEventArgs e, string text)
    //{
    //    using StringFormat format = new StringFormat(StringFormatFlags.NoWrap)
    //    {
    //        LineAlignment = StringAlignment.Center,
    //        Alignment = StringAlignment.Near
    //    };
    //    e.Graphics.DrawDarkString(text, e.Font, e.ForeColor, e.Bounds, format);
    //}

    #region DrawItem
    internal static void DrawDarkFocusRectangle(this DrawItemEventArgs e, bool controlFocused = false)
        => e.DrawDarkFocusRectangle(e.Bounds, GetItemHighlight(e.State, controlFocused));

    internal static void DrawDarkFocusRectangle(this DrawItemEventArgs e, Rectangle bounds, bool controlFocused = false)
        => e.DrawDarkFocusRectangle(bounds, GetItemHighlight(e.State, controlFocused));

    internal static void DrawDarkBackground(this DrawItemEventArgs e, bool controlFocused = false)
        => e.DrawDarkBackground(e.BackColor, controlFocused);
    #endregion

    #region DrawToolTip
    internal static void DrawDarkBackground(this DrawToolTipEventArgs e)
        => e.Graphics.FillRectangle(DarkBrushes.ToolTip.Back, e.Bounds);

    internal static void DrawDarkBorder(this DrawToolTipEventArgs e)
        => DarkControlPaint.DrawBorder(e.Graphics, e.Bounds);

    internal static void DrawDarkText(this DrawToolTipEventArgs e, Color? textColor = null)
        => TextRenderer.DrawText(e.Graphics, e.ToolTipText, e.Font, e.Bounds, textColor ?? ThemeColors.ToolTip.InfoText);

    internal static void DrawDarkText(this DrawToolTipEventArgs e, Rectangle bounds, Color? textColor = null)
        => TextRenderer.DrawText(e.Graphics, e.ToolTipText, e.Font, bounds, textColor ?? ThemeColors.ToolTip.InfoText);
    #endregion

    #region ListViewColumnHeader
    //internal static void DrawDarkBackground(this DrawListViewColumnHeaderEventArgs e)
    //    => e.Graphics.FillRectangle(DarkBrushes.Header.Back, e.Bounds);

    //internal static void DrawDarkSeparator(this DrawListViewColumnHeaderEventArgs e)
    //{
    //    int x = e.Bounds.Right - 2;
    //    e.Graphics.DrawLine(DarkPens.FromDarkColor(DarkColors.Header.Separator), new Point(x, e.Bounds.Top), new Point(x, e.Bounds.Bottom));
    //}

    //internal static void DrawDarkString(this DrawListViewColumnHeaderEventArgs e)
    //{
    //    using StringFormat stringFormat = new StringFormat
    //    {
    //        Alignment = StringAlignment.Near,           // left align text (or right align, in RTL)
    //        LineAlignment = StringAlignment.Center,     // vertically center text
    //        Trimming = StringTrimming.EllipsisCharacter
    //    };
    //    e.Graphics.DrawDarkString(e.Header.Text, e.Font, DarkColors.Header.Text, e.Bounds, stringFormat);
    //}
    #endregion

    #region ListViewSubItem
    //internal static void DrawDarkBackground(this DrawListViewSubItemEventArgs e, Color backColor)
    //    => e.Graphics.DrawDarkBackground(e.Bounds, backColor);

    internal static void DrawDarkBackground(this DrawListViewSubItemEventArgs e)
    {
        //Color color = ((itemIndex == -1) ? item.BackColor : subItem.BackColor); // internal implementation
        Color backColor = e.Item.Selected ? DarkColors.SelectedText.Highlight : e.Item.BackColor;
        e.Graphics.FillRectangle(DarkBrushes.FromDarkColor(backColor), e.Bounds);
    }

    internal static void DrawDarkFocusRectangle(this DrawListViewSubItemEventArgs e)
    {
        if ((e.ItemState & ListViewItemStates.Focused) == ListViewItemStates.Focused)
            //ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(e.Bounds, -1, -1), e.Item.ForeColor, e.Item.BackColor); // internal implementation
            DarkControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(e.Bounds, -1, -1));
    }
    #endregion

    #region Helpers
    private static ItemHighlightStyle GetItemHighlight(DrawItemState itemState, bool controlFocused = false)
    {
        bool selected = (itemState & DrawItemState.Selected) == DrawItemState.Selected;
        bool focused = (itemState & DrawItemState.Focus) == DrawItemState.Focus;
        bool inactive = (itemState & DrawItemState.Inactive) == DrawItemState.Inactive;
        bool hot = (itemState & DrawItemState.HotLight) == DrawItemState.HotLight;
        bool disabled = (itemState & DrawItemState.Disabled) == DrawItemState.Disabled;
        bool comboEdit = (itemState & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;

        if (disabled || comboEdit)
            return ItemHighlightStyle.None;
        else if ((!inactive && selected && (focused || controlFocused)) || hot)
            return ItemHighlightStyle.Active;
        else if (selected)
            return ItemHighlightStyle.Inactive;

        return ItemHighlightStyle.None;
    }

    private static Color GetFocusColor(ItemHighlightStyle style)
    {
        return style switch
        {
            ItemHighlightStyle.Active => DarkColors.SelectedText.Focus,
            ItemHighlightStyle.Inactive => DarkColors.SelectedText.InactiveFocus,
            ItemHighlightStyle.None => Color.Empty,
            _ => Color.Empty
        };
    }

    private static Color GetBackColor(DrawItemState state, Color backColor, bool controlFocused)
        => GetBackColor(state, GetItemHighlight(state, controlFocused), backColor);

    private static Color GetBackColor(DrawItemState state, ItemHighlightStyle style, Color backColor)
    {
        return style switch
        {
            ItemHighlightStyle.Active => DarkColors.SelectedText.Highlight,
            ItemHighlightStyle.Inactive => DarkColors.SelectedText.InactiveHighlight,
            ItemHighlightStyle.None => IsComboEdit(state)
                ? IsDisabled(state) ? DarkColors.ComboBox.Disabled : DarkColors.ComboBox.Back
                : backColor,
            _ => backColor
        };
    }

    private static bool IsComboEdit(DrawItemState state)
        => (state & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;

    private static bool IsDisabled(DrawItemState state)
        => (state & DrawItemState.Disabled) == DrawItemState.Disabled;
    #endregion
}

/// <summary>
/// <see cref="Graphics"/> Extensions. Allow custom colors used for simple drawing.<br/>
/// The logic around calculations/geometry is the same as internal .NET methods; changes may be required due to Dark Mode having different part sizes.
/// </summary>
//internal static class GraphicsExtensions
//{
//    internal static void DrawDarkStringDisabled(this Graphics g, string text, Font font, Color color, Rectangle bounds, TextFormatFlags textFormatFlags)
//        => TextRenderer.DrawText(g, text, font, bounds, SystemColors.GrayText, textFormatFlags);

//    internal static void DrawDarkString(this Graphics g, string text, Font font, Color color, Rectangle bounds, StringFormat stringFormat)
//        => g.DrawString(text, font, DarkBrushes.FromDarkColor(color), bounds, stringFormat);
//}

/// <summary>
/// Provides <see cref="DarkMode"/> extension methods for drawing and getting information about a <see cref="VisualStyleElement"/>.
/// </summary>
/// <remarks>
/// This class is stub. Only the methods and Visual Style Elements currently used are implemented/supported.<br/>
/// It will need to be extended to support additional methods or Visual Style Elements.
/// </remarks>
internal static class VisualStyleRendererExtensions
{
    /// <summary>
    /// Draws the <see cref="DarkMode"/> background image of the current visual style element within the specified bounding rectangle.
    /// </summary>
    /// <param name="dc">The <see cref="IDeviceContext"/> used to draw the background image.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> in which the background image is drawn.</param>
    public static void DrawDarkBackground(this VisualStyleRenderer vsr, IDeviceContext dc, Rectangle bounds)
        => vsr.DrawDarkBackground((Graphics)dc, bounds);

    /// <summary>
    /// Draws the <see cref="DarkMode"/> background image of the current visual style element within the specified bounding rectangle.
    /// </summary>
    /// <param name="g">The <see cref="Graphics"/> used to draw the background image.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> in which the background image is drawn.</param>
    public static void DrawDarkBackground(this VisualStyleRenderer vsr, Graphics g, Rectangle bounds)
    {
        g.Clear(vsr.GetPartColor(ColorProperty.FillColor));

        // Draw border if VisualStyleRenderer indicates content area is smaller than bounds
        Rectangle content = vsr.GetBackgroundContentRectangle(g, bounds);
        if (content.Width < bounds.Width || content.Height < bounds.Height)
            g.DrawRectangle(DarkPens.FromDarkColor(vsr.GetPartColor(ColorProperty.BorderColor)), bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
    }

    /// <summary>
    /// Draws <see cref="DarkMode"/> text in the specified bounding rectangle with the option of displaying disabled text and applying other text formatting.
    /// </summary>
    /// <param name="dc">The <see cref="IDeviceContext"/> used to draw the text.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> in which to draw the text.</param>
    /// <param name="textToDraw">The text to draw.</param>
    /// <param name="drawDisabled">true to draw grayed-out text; otherwise, false.</param>
    /// <param name="flags">A bitwise combination of the <see cref="TextFormatFlags"/> values.</param>
    public static void DrawDarkText(this VisualStyleRenderer vsr, FontDC dc, Rectangle bounds, string textToDraw, bool drawDisabled, TextFormatFlags flags)
    {
        if (vsr.State != (int)PushButtonState.Disabled || vsr.State == (int)PushButtonState.Disabled && drawDisabled)
            TextRenderer.DrawText(dc, textToDraw, dc.Font, bounds, vsr.GetPartColor(ColorProperty.TextColor), flags);
    }

    /// <summary>
    /// Draws <see cref="DarkMode"/> text in the specified bounds using default formatting.
    /// </summary>
    /// <param name="dc">The <see cref="IDeviceContext"/> used to draw the text.</param>
    /// <param name="bounds">A <see cref="Rectangle"/> in which to draw the text.</param>
    /// <param name="textToDraw">The text to draw.</param>
    public static void DrawDarkText(this VisualStyleRenderer vsr, FontDC dc, Rectangle bounds, string textToDraw)
    {
        TextRenderer.DrawText(dc, textToDraw, dc.Font, bounds, vsr.GetPartColor(ColorProperty.TextColor));
    }

    /// <summary>
    /// Returns the <see cref="DarkMode"/> value of the specified color <paramref name="property"/>  for the current visual style element.
    /// </summary>
    /// <param name="property">
    /// One of the <see cref="ColorProperty"/> values that specifies which property value to retrieve for the current visual style element.
    /// </param>
    /// <returns>
    /// A <see cref="Color"/> that contains the value of the property specified by the <paramref name="property"/> parameter for the current visual style element.
    /// </returns>
    public static Color GetDarkColor(this VisualStyleRenderer vsr, ColorProperty property)
        => vsr.GetPartColor(property);

    private static Color GetPartColor(this VisualStyleRenderer vsr, ColorProperty property)
        => GetPartColor(vsr.Class, vsr.Part, vsr.State, property);

    private static Color GetPartColor(string className, int part, int state, ColorProperty property)
    {
        // TODO : investigate whether there is a VisualStyleElement Class enum. Internally strings are used
        return className switch
        {
            "BUTTON" => GetButtonColor(part, state, property),
            "EDIT" => GetTextBoxColor(part, state, property),
            "TAB" => GetTabColor(part, state, property),
            "TOOLTIP" => GetToolTipColor(part, state, property),
            _ => Color.Empty
        };
    }

    #region Button
    private static Color GetButtonColor(int part, int state, ColorProperty property)
    {
        return part switch
        {
            1 => GetPushButtonColor(state, property),
            _ => Color.Empty
        };
    }

    private static Color GetPushButtonColor(int state, ColorProperty property)
    {
        return property switch
        {
            ColorProperty.TextColor
                => state == (int)PushButtonState.Disabled
                    ? SystemColors.GrayText
                    : DarkColors.Button.Text,
            _ => Color.Empty
        };
    }
    #endregion

    #region Tab
    private static Color GetTabColor(int part, int state, ColorProperty property)
    {
        return part switch
        {
            9 => GetTabPaneColor(state, property),
            10 => GetTabBodyColor(state, property),
            _ => Color.Empty
        };
    }

    private static Color GetTabPaneColor(int state, ColorProperty property)
    {
        return property switch
        {
            ColorProperty.BorderColorHint => ThemeColors.TabBar.DefaultBorder,
            _ => Color.Empty
        };
    }

    private static Color GetTabBodyColor(int state, ColorProperty property)
    {
        return property switch
        {
            ColorProperty.TextColor => SystemColors.ControlText,
            ColorProperty.FillColor => ThemeColors.CollapsibleGroupBox.Back, // this is called by CollapsibleGroupBox control
            ColorProperty.BorderColor => DarkColors.TabBar.Border,
            _ => Color.Empty
        };
    }
    #endregion

    #region TextBox
    private static Color GetTextBoxColor(int part, int state, ColorProperty property)
    {
        return part switch
        {
            1 => GetTextBoxTextEditColor(state, property),
            _ => Color.Empty
        };
    }

    private static Color GetTextBoxTextEditColor(int state, ColorProperty property)
    {
        return property switch
        {
            ColorProperty.FillColor => DarkColors.TextBox.Back,
            ColorProperty.BorderColor => DarkColors.Border.Default,
            _ => Color.Empty
        };
    }
    #endregion

    #region ToolTip
    private static Color GetToolTipColor(int part, int state, ColorProperty property)
    {
        return part switch
        {
            1 => GetToolTipStandardColor(state, property),
            _ => Color.Empty
        };
    }

    private static Color GetToolTipStandardColor(int state, ColorProperty property)
    {
        return property switch
        {
            ColorProperty.TextColor => ThemeColors.ToolTip.InfoText,
            ColorProperty.FillColor => DarkColors.ToolTip.Back,
            ColorProperty.BorderColor => DarkColors.Border.Default,
            _ => Color.Empty
        };
    }
    #endregion
}

/// <summary>
/// Provides methods used to render a <see cref="DarkMode"/> <see cref="Control"/> with visual styles.
/// </summary>
internal static class DarkRenderer
{
    /// <summary>
    /// Draws a <see cref="DarkMode"/> <see cref="Button"/> control in the specified <paramref name="state"/> and <paramref name="bounds"/>.
    /// </summary>
    /// <param name="g">The <see cref="Graphics"/> used to draw the button.</param>
    /// <param name="bounds">The <see cref="Rectangle"/> that specifies the bounds of the button.</param>
    /// <param name="state">One of the <see cref="PushButtonState"/> values that specifies the visual state of the button.</param>
    public static void DrawButton(Graphics g, Rectangle bounds, PushButtonState state)
    {
        g.FillRectangle(GetBackBrush(state), bounds);
        DarkControlPaint.DrawBorder(g, bounds, DarkColors.Button.Border);
    }

    /// <summary>
    /// Draws a <see cref="DarkMode"/> tab in the specified <paramref name="state"/> and <paramref name="bounds"/>.
    /// </summary>
    /// <param name="g">The <see cref="Graphics"/> used to draw the tab.</param>
    /// <param name="bounds">The <see cref="Rectangle"/> that specifies the bounds of the tab.</param>
    /// <param name="state">One of the <see cref="TabItemState"/> values that specifies the visual state of the tab.</param>
    internal static void DrawTabItem(Graphics g, Rectangle bounds, TabItemState state)
    {
        g.FillRectangle(GetBackBrush(state), bounds);
        DrawTabBorder(g, bounds, state);
    }

    private static void DrawTabBorder(Graphics gr, Rectangle rect, TabItemState state)
    {
        Pen borderPen = GetBorderPen(state);

        gr.DrawLine(borderPen, rect.Left, rect.Bottom - 1, rect.Left, rect.Top);           // Left
        gr.DrawLine(borderPen, rect.Left, rect.Top, rect.Right - 1, rect.Top);             // Top
        gr.DrawLine(borderPen, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom - 1); // Right
    }

    #region Button Helpers
    private static Brush GetBackBrush(PushButtonState state)
        => DarkBrushes.FromDarkColor(GetBackColor(state));

    private static Color GetBackColor(PushButtonState state)
    {
        return state switch
        {
            PushButtonState.Hot => DarkColors.Button.MouseOverBack,
            PushButtonState.Pressed => DarkColors.Button.CheckedBack,
            _ => DarkColors.Button.Back
        };
    }

    private static ButtonState ConvertToButtonState(PushButtonState state)
    {
        return state switch
        {
            PushButtonState.Pressed => ButtonState.Pushed,
            PushButtonState.Disabled => ButtonState.Inactive,
            _ => ButtonState.Normal,
        };
    }
    #endregion

    #region Tab Helpers
    private static Pen GetBorderPen(TabItemState state)
        => DarkPens.FromDarkColor(GetBorderColor(state));

    private static Brush GetBackBrush(TabItemState state)
        => DarkBrushes.FromDarkColor(GetBackColor(state));

    private static Color GetBorderColor(TabItemState state)
    {
        return state switch
        {
            TabItemState.Selected => ThemeColors.TabBar.DefaultBorder,
            _ => ThemeColors.TabBar.DefaultBorder
        };
    }

    private static Color GetBackColor(TabItemState state)
    {
        return state switch
        {
            TabItemState.Selected => ThemeColors.TabBar.SelectedBack,
            _ => ThemeColors.TabBar.Back
        };
    }
    #endregion
}
