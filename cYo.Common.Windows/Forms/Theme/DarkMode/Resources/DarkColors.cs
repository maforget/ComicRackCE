using cYo.Common.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Resources;

internal class DarkColors
{
    // WhiteSmoke replacement. WhiteSmoke is RGB 245 but RGB 10 would be too dark
    // Mainly visible as DisplayWorkspace background. Also used in PreferencesDialog as PanelOverlays background
    internal static readonly Color BlackSmoke = Color.FromArgb(48, 48, 48);

    private static class Common
    {
        public static readonly Color EditBack = Color.FromArgb(46, 46, 46); // RGB 46 HEX 2E : was RGB 56 HEX 38
        public static readonly Color ListBack = Color.FromArgb(40, 40, 40); // RGB 40 HEX 28 : framework uses RGB 46 HEX 2E; setting darker for Form contrast 
        public static readonly Color Border = SystemColors.ControlDarkDark; // RGB 90 HEX 5A
    }

    internal static class UIComponent
    {
        public static readonly Color Window = SystemColors.Window;           // RGB 50 HEX 32 : Form Background 
        public static readonly Color SidePanel = Color.FromArgb(25, 25, 25); // RGB 25 HEX 19 : ComicExplorerView.SidePanel [favView(ItemView) + ComicListBrowser + SmallComicPreview] 
        public static readonly Color Content = SystemColors.Control;         // RGB 32 HEX 20 : MainViewItemView + CollapsibleGroupBox + background  
    }

    #region Color.Empty
    internal static class ListBox
    {
        public static readonly Color Back = Common.ListBack;
    }

    internal static class TreeView
    {
        public static Color Back = Common.ListBack;
        public static Color Text = SystemColors.ControlText;
    }
    #endregion

    /// <summary>
    /// Colors used for all Controls of a type.
    /// </summary>
    #region Control Type Colors

    internal static class Button
    {
        public static readonly Color Back = SystemColors.Window; // RGB 50 HEX 32
        public static readonly Color CheckedBack = Color.FromArgb(102, 102, 102);
        public static readonly Color Text = SystemColors.ControlText;
        public static readonly Color Border = Color.FromArgb(155, 155, 155);
        public static readonly Color MouseOverBack = SystemColors.ButtonShadow; // RGB 70 HEX 46
    }

    // CheckBox
    internal static class CheckBox
    {
        public static Color Back = Color.FromArgb(0, 95, 184);
        public static Color BackCorner = Color.FromArgb(0, 95, 184);
        public static Color BackVertex = Color.FromArgb(0, 95, 184);
        public static Color Border = Color.FromArgb(0, 95, 184);
        public static Color BorderEdge = Color.FromArgb(4, 87, 166);
        public static Color BorderCorner = Color.FromArgb(28, 54, 74);
        public static Color UncheckedBorder = Color.FromArgb(98, 98, 98);
        public static Color UncheckedBorderEdge = Color.FromArgb(90, 90, 90);
        public static Color UncheckedBack = SystemColors.ControlLight;
        public static Color UncheckedBackCorner = Color.FromArgb(48, 48, 48);
        public static Color UncheckedBackVertex = SystemColors.ControlDark;
        public static Color UncheckedDisabledBorder = SystemColors.ControlDark;
        public static Color UncheckedDisabledBorderEdge = Color.FromArgb(60, 60, 60);
        public static Color UncheckedDisabledBackCorner = Color.FromArgb(100, 100, 100);
        public static Color UncheckedDisabledBackVertex = SystemColors.ControlDarkDark;
    }

    internal static class ComboBox
    {
        public static readonly Color Back = Common.EditBack;
        public static readonly Color Disabled = Color.FromArgb(64, 64, 64); // from .net ComboBox source
        //public static Color Separator => ColorTable.ComboBoxSeparator;
    }

    internal static class RatingControl
    {
        public static readonly Color Back = Common.ListBack;
        public static readonly Color Rated = SystemColors.ControlText;
        public static readonly Color Unrated = SystemColors.GrayText;
    }

    internal static class TabBar
    {
        public static readonly Color Back = SystemColors.Window; // RGB 50 HEX 32
        public static readonly Color Border = Color.Black;
        public static readonly Color SelectedBack = SystemColors.Window; // RGB 50 HEX 32
    }

    internal static class TextBox
    {
        public static readonly Color Back = Common.EditBack;
        public static readonly Color MouseOverBack = Color.FromArgb(34, 34, 34); // RGB 34 HEX 22 (was RGB 86)
        public static readonly Color EnterBack = Color.FromArgb(26, 26, 26);     // RGB 26 HEX 1A (was RGB 71)
    }

    internal static class ToolStrip
    {
        // currently this is purely for StatusStripLabel border.
        // TODO: move to renderer (will need to account for which borders need to be drawn)
        public static readonly Color BorderColor = Common.Border; //Color.FromArgb(100, 100, 100);
    }

    internal static class ToolTip
    {
        public static readonly Color Back = SystemColors.Window; // should be SystemColors.Info; needs alpha-aware tweaks
    }
    #endregion

    /// <summary>
    /// Colors used in multiple control types for a specific part
    /// </summary>
    #region Part Colors
    internal static class Border
    {
        public static readonly Color Darkest = Color.FromArgb(16, 16, 16);
        public static readonly Color Dark = Color.FromArgb(51, 51, 51);
        public static readonly Color Default = Color.FromArgb(93, 93, 93);
        public static readonly Color Light = Color.FromArgb(155, 155, 155);
        //public static readonly Color Dark = Color.FromArgb(155, 155, 155);
    }

    internal static class Header
    {
        public static Color Back = SystemColors.Control;
        public static Color Separator = Color.FromArgb(99, 99, 99);
        public static Color Text = SystemColors.WindowText;
    }

    internal static class SelectedText
    {
        public static readonly Color Highlight = Color.FromArgb(52, 67, 86);
        public static readonly Color Focus = Color.FromArgb(40, 100, 180);
    }
    #endregion

    public static Color GetUIComponentColor(Theme.UIComponent component)
    {
        switch (component)
        {
            case Theme.UIComponent.SidePanel:
                return UIComponent.SidePanel;
            case Theme.UIComponent.Content:
                return UIComponent.Content;
            case Theme.UIComponent.Window:
                return UIComponent.Window;
            default:
                return Color.Empty; // Theme.UIComponent.None or null, if that's a possibility
        }
    }
}


internal static class DarkBrushes
{
    private static readonly Dictionary<Color, SolidBrush> cache = new();

    internal static class Button
    {
        public static Brush Back => FromDarkColor(DarkColors.Button.Back);
        public static Brush CheckedBack => FromDarkColor(DarkColors.Button.CheckedBack);
        public static Brush MouseOverBack => FromDarkColor(DarkColors.Button.MouseOverBack);
    }

    internal static class CheckBox
    {
        public static Brush Back => FromDarkColor(DarkColors.CheckBox.Back);
        public static Brush BackCorner => FromDarkColor(DarkColors.CheckBox.BackCorner);
        public static Brush BackVertex => FromDarkColor(DarkColors.CheckBox.BackVertex);
        //public static Brush Border => FromDarkColor(DarkColors.CheckBox.Border);
        public static Brush BorderEdge => FromDarkColor(DarkColors.CheckBox.BorderEdge);
        public static Brush BorderCorner => FromDarkColor(DarkColors.CheckBox.BorderCorner);
        //public static Brush UncheckedBorder => FromDarkColor(DarkColors.CheckBox.UncheckedBorder);
        public static Brush UncheckedBorderEdge => FromDarkColor(DarkColors.CheckBox.UncheckedBorderEdge);
        public static Brush UncheckedBack => FromDarkColor(DarkColors.CheckBox.UncheckedBack);
        public static Brush UncheckedBackCorner => FromDarkColor(DarkColors.CheckBox.UncheckedBackCorner);
        public static Brush UncheckedBackVertex => FromDarkColor(DarkColors.CheckBox.UncheckedBackVertex);
        //public static Brush UncheckedDisabledBorder => FromDarkColor(DarkColors.CheckBox.UncheckedDisabledBorder);
        public static Brush UncheckedDisabledBorderEdge => FromDarkColor(DarkColors.CheckBox.UncheckedDisabledBorderEdge);
        public static Brush UncheckedDisabledBackCorner => FromDarkColor(DarkColors.CheckBox.UncheckedDisabledBackCorner);
        public static Brush UncheckedDisabledBackVertex => FromDarkColor(DarkColors.CheckBox.UncheckedDisabledBackVertex);
    }

    internal static class Header
    {
        public static Brush Back => FromDarkColor(DarkColors.Header.Back);
        public static Brush Separator => FromDarkColor(DarkColors.Header.Separator);
        public static Brush Text => FromDarkColor(DarkColors.Header.Text);
    }

    internal static class SelectedText
    {
        public static Brush Highlight => FromDarkColor(DarkColors.SelectedText.Highlight);
    }

    internal static class TabBar
    {
        public static Brush Back = FromDarkColor(DarkColors.TabBar.Back);
        public static Brush SelectedBack = FromDarkColor(DarkColors.TabBar.SelectedBack);
    }

    internal static class ToolTip
    {
        public static Brush Back = FromDarkColor(DarkColors.ToolTip.Back);
    }

    public static Brush FromDarkColor(Color color)
    {
        if (!cache.TryGetValue(color, out var brush))
        {
            brush = new SolidBrush(color);
            cache[color] = brush;
        }
        return brush;
    }
}

internal static class DarkPens
{
    private static readonly Dictionary<Color, Pen> cache = new();

    internal static class CheckBox
    {
        public static Pen Border => FromDarkColor(DarkColors.CheckBox.Border);
        public static Pen UncheckedBorder => FromDarkColor(DarkColors.CheckBox.UncheckedBorder);
        public static Pen UncheckedDisabledBorder => FromDarkColor(DarkColors.CheckBox.UncheckedDisabledBorder);
    }

    internal static class Header
    {
        public static Pen Back => FromDarkColor(DarkColors.Header.Back);
        public static Pen Separator => FromDarkColor(DarkColors.Header.Separator);
    }

    internal static class SelectedText
    {
        public static Pen Focus => FromDarkColor(DarkColors.SelectedText.Focus);
    }

    internal static class TabBar
    {
        public static Pen Border = FromDarkColor(DarkColors.TabBar.Border);
    }

    public static Pen FromDarkColor(Color color)
    {
        if (!cache.TryGetValue(color, out var brush))
        {
            brush = new Pen(color);
            cache[color] = brush;
        }
        return brush;
    }
}