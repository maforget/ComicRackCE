using cYo.Common.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Windows.Forms.Theme.DarkMode.Resources;

internal class DarkColors
{

    private static class Common
    {
        public static readonly Color Edit = Color.FromArgb(0xFF, 0x2E, 0x2E, 0x2E); //Color.FromArgb(56, 56, 56);
        public static readonly Color ListViewBack = Color.FromArgb(46, 46, 46);
        public static readonly Color Border = Color.FromArgb(93, 93, 93);
    }

    #region Color.Empty
    public static class ListBox
    {
        public static readonly Color Back = Common.ListViewBack; // to match ListView BackColor
    }

    public static class TreeView
    {
        public static Color Back = Common.Edit;
        public static Color Text = SystemColors.ControlText;
    }
    #endregion

    /// <summary>
    /// Colors used for all Controls of a type.
    /// </summary>
    #region Control Type Colors

    public static class Button
    {
        public static readonly Color Back = SystemColors.Window; // RGB 50 HEX 32
        public static readonly Color CheckedBack = Color.FromArgb(102, 102, 102);
        public static readonly Color Text = Color.White;
        public static readonly Color Border = Color.FromArgb(155, 155, 155);
        public static readonly Color MouseOverBack = SystemColors.ButtonShadow; // RGB 70 HEX 46
    }

    // CheckBox
    public static class CheckBox
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

    public static class ComboBox
    {
        public static readonly Color Back = Common.Edit; //Color.FromArgb(56, 56, 56); // to match ComboBox button
        public static readonly Color Disabled = Color.FromArgb(64, 64, 64); // from .net ComboBox source
        //public static Color Separator => ColorTable.ComboBoxSeparator;
    }

    public static class TextBox
    {
        public static readonly Color Back = Common.Edit;
        public static readonly Color MouseOverBack = Color.FromArgb(0xFF, 0x22, 0x22, 0x22); //Color.FromArgb(86, 86, 86);
        public static readonly Color EnterBack = Color.FromArgb(unchecked((int)0xFF1A1A1A)); //Color.FromArgb(71, 71, 71);
    }

    public static class ToolStrip
    {
        // currently this is purely for StatusStrip border.
        // TODO: move to renderer (will need to account for which borders need to be drawn)
        public static readonly Color BorderColor = Common.Border; //Color.FromArgb(100, 100, 100);
    }
    #endregion

    /// <summary>
    /// Colors used in multiple control types for a specific part
    /// </summary>
    #region Part Colors
    public static class Border
    {
        public static readonly Color Darkest = Color.FromArgb(16, 16, 16);
        public static readonly Color Dark = Color.FromArgb(51, 51, 51);
        public static readonly Color Default = Color.FromArgb(93, 93, 93);
        public static readonly Color Light = Color.FromArgb(155, 155, 155);
        //public static readonly Color Dark = Color.FromArgb(155, 155, 155);
    }

    public static class Header
    {
        public static Color Back = SystemColors.Control;
        public static Color Separator = Color.FromArgb(99, 99, 99);
        public static Color Text = SystemColors.WindowText;
    }

    public static class SelectedText
    {
        public static readonly Color Highlight = Color.FromArgb(52, 67, 86);
        public static readonly Color Focus = Color.FromArgb(40, 100, 180);
    }

    #endregion

    public static class UIComponent
    {
        public static readonly Color Window = Color.FromArgb(unchecked((int)0xFF333333));    // Form Background. SystemColors.Control; // RGB 32 HEX 20
        public static readonly Color SidePanel = Color.FromArgb(unchecked((int)0xFF191919)); // RGB 25 HEX 19
        public static readonly Color Content = SystemColors.Control;                         // MainViewItemView + CollapsibleGroupBox + background
    }

    //// ComicBookDialog
    //public Color ComicBookLink => Color.LightBlue;
    ////public Color ComicBookVisitedLink => Color.MediumOrchid;
    ////public Color ComicBookPageViewerText => SystemColors.Control;
    ////public Color ComicBookPanelBack => Color.Red;
    ////public Color ComicBookWhereSeparator => Color.Cyan;

    //// CollapsibleGroupBox
    //public Color CollapsibleGroupBoxBack => DarkColors.Material.Content;
    //public Color CollapsibleGroupBoxHeaderBackGradientStart => SystemColors.Control;
    //public Color CollapsibleGroupBoxHeaderBackGradientEnd => SystemColors.ControlDark;
    //public Color CollapsibleGroupBoxHeaderText => SystemColors.ControlText;

    //// ComicListFolderFilesBrowser
    //public Color ComicListFolderFilesBrowserFavViewBack => DarkColors.Material.SidePanel;

    //// ComicListLibraryBrowserF
    //public Color ComicListLibraryBrowserFavViewBack => DarkColors.Material.SidePanel;

    //// ControlStyleColorTable
    ////public Color ControlStyleColorTableBorder => Color.Black;

    //// DeviceEditControl
    //public Color DeviceEditControlBack => SystemColors.Control;

    //// MainForm
    ////public Color MainFormToolStripBack => Color.Transparent;

    ////MainView
    ////public Color MainViewBack => Color.Transparent;
    ////public Color MainViewToolStripBack => Color.Transparent;

    //public Color MatcherGroupEditor => DarkColors.Material.Window;

    //// SimpleScrollbarPanel
    ////public Color ScrollbarPanelBorder => DarkColors.BlackSmoke;

    //// SmallComicPreview
    //public Color SmallComicPreviewPageViewerBack => DarkColors.Material.SidePanel;
    //public Color SmallComicPreviewPageViewerText => DarkColors.BlackSmoke;

    //// StyledRenderer
    //public Color StyledSelectionBack => Color.Gray;
    //public Color StyledSelectionFocusedBack => SystemColors.Highlight;

    //// ThumbTileRenderer
    //public Color ThumbTileRendererEmboss => DarkColors.BlackSmoke; // Color.FromArgb(unchecked((int)0xFF303030));
    //public Color ThumbTileRendererTitleText => Color.White; // Color.FromArgb(unchecked((int)0xFFD0D0D0));
    //public Color ThumbTileRendererBodyText => Color.LightGray;

    //// ThumbRenderer
    //public Color ThumbRendererSelectionBack => SystemColors.Highlight;

    //// ThumbnailViewItem
    //public Color ThumbnailViewItemBack => Color.FromArgb(72, 72, 72); // CoverViewItem, FavoriteViewitem, FolderViewItem
    ////public Color ThumbnailViewItemHighlightText => SystemColors.HighlightText; // FavoriteViewitem, FolderViewItem
    //public Color ThumbnailViewItemBorder => Color.FromArgb(64, 190, 190, 190);

    //// PreferencesDialog
    //public Color PreferencesPanelReaderOverlay => DarkColors.BlackSmoke;
    //public Color PreferencesLabelOverlays => DarkColors.Lossboro;
    //public Color PreferencesServerEditControl => SystemColors.Control;

    //// ItemView
    //public Color ItemViewMainBack => DarkColors.Material.Content;
    //public Color ItemViewDefaultBack => SystemColors.Window;
    //public Color ItemViewGroupText => Color.LightSkyBlue;
    //public Color ItemViewGroupSeparator => Color.FromArgb(190, 190, 190);

    //// NiceTreeSkin
    //public Color NiceTreeSkinDragSeparator => Color.FromArgb(190, 190, 190);

    //// LibraryTreeSkin
    //public Color LibraryTreeTotalBookCountBack => Color.FromArgb(unchecked((int)0xFF00C000));
    ////public Color LibraryTreeTotalBookCountText => DarkColors.BlackSmoke;
    ////public Color LibraryTreeUnreadBookCountBack => Color.Orange;
    ////public Color LibraryTreeUnreadBookCountText => DarkColors.BlackSmoke;
    ////public Color LibraryTreeNewBookCountBack => Color.Red;
    ////public Color LibraryTreeNewBookCountText => DarkColors.BlackSmoke;

    //// SmartQuery
    //public Color SmartQueryBack => DarkColors.TextBox.Back;
    //public Color SmartQueryException => Color.FromArgb(125, 31, 31);
    //public Color SmartQueryText => SystemColors.ControlText;
    //public Color SmartQueryKeyword => Color.FromArgb(250, 198, 0);
    //public Color SmartQueryQualifier => Color.FromArgb(76, 163, 255);
    //public Color SmartQueryNegation => Color.Red;
    //public Color SmartQueryString => Color.FromArgb(255, 125, 125);

    //// TabBar
    //public Color TabBarBack => SystemColors.Window; // RGB 50 HEX 32
    //public Color TabBarDefaultBorder => Color.Black;
    //public Color TabBarSelectedBack => SystemColors.Window; // RGB 50 HEX 32

    //// ToolTip
    //public Color ToolTipBack => SystemColors.Window; // should be SystemColors.Info; needs alpha-aware tweaks
    //public Color ToolTipText => SystemColors.ControlText; // should be SystemColors.InfoText; needs alpha-aware tweaks

    //// TreeView
    //public Color TreeViewBack => DarkColors.TextBox.Back;
    //public Color TreeViewText => SystemColors.ControlText;

    //// ComboBox
    //public Color ComboBoxSeparator => SystemColors.ControlText;

    //// Caption
    //public Color CaptionBack => Color.Black;
    ////public Color CaptionText => SystemColors.ActiveCaptionText;

    //// Header
    //public Color HeaderBack => SystemColors.Control;
    //public Color HeaderSeparator => Color.FromArgb(99, 99, 99);
    //public Color HeaderText => SystemColors.WindowText;

    //// Control defaults
    //public Color ListBoxBack => Color.FromArgb(46, 46, 46); // to match ListView BackColor

    //// RatingControl
    //public Color RatingControlBack => ListBoxBack;
    //public Color RatingControlRated => SystemColors.ControlText;
    //public Color RatingControlUnrated => SystemColors.GrayText;

    //public Color SplitButtonSeparatorLeft => SystemColors.WindowText;
    //public Color SplitButtonSeparatorRight => SystemColors.ButtonFace;

    //// CheckBox
    //public Color CheckBoxBack => Color.FromArgb(0, 95, 184);
    //public Color CheckBoxBackCorner => Color.FromArgb(0, 95, 184);
    //public Color CheckBoxBackVertex => Color.FromArgb(0, 95, 184);
    //public Color CheckBoxBorder => Color.FromArgb(0, 95, 184);
    //public Color CheckBoxBorderEdge => Color.FromArgb(4, 87, 166);
    //public Color CheckBoxBorderCorner => Color.FromArgb(28, 54, 74);
    //public Color CheckBoxUncheckedBorder => Color.FromArgb(98, 98, 98);
    //public Color CheckBoxUncheckedBorderEdge => Color.FromArgb(90, 90, 90);
    //public Color CheckBoxUncheckedBack => SystemColors.ControlLight;
    //public Color CheckBoxUncheckedBackCorner => Color.FromArgb(48, 48, 48);
    //public Color CheckBoxUncheckedBackVertex => SystemColors.ControlDark;
    //public Color CheckBoxUncheckedDisabledBorder => SystemColors.ControlDark;
    //public Color CheckBoxUncheckedDisabledBorderEdge => Color.FromArgb(60, 60, 60);
    //public Color CheckBoxUncheckedDisabledBackCorner => Color.FromArgb(100, 100, 100);
    //public Color CheckBoxUncheckedDisabledBackVertex => SystemColors.ControlDarkDark;
}

internal static class DarkBrushes
{
    private static readonly Dictionary<Color, SolidBrush> cache = new();

    public static class CheckBox
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

    public static class Header
    {
        public static Brush Back => FromDarkColor(DarkColors.Header.Back);
        public static Brush Separator => FromDarkColor(DarkColors.Header.Separator);
        public static Brush Text => FromDarkColor(DarkColors.Header.Text);
    }

    public static class SelectedText
    {
        public static Brush Highlight => FromDarkColor(DarkColors.SelectedText.Highlight);
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

    public static class CheckBox
    {
        public static Pen Border => FromDarkColor(DarkColors.CheckBox.Border);
        public static Pen UncheckedBorder => FromDarkColor(DarkColors.CheckBox.UncheckedBorder);
        public static Pen UncheckedDisabledBorder => FromDarkColor(DarkColors.CheckBox.UncheckedDisabledBorder);
    }

    public static class Header
    {
        public static Pen Back => FromDarkColor(DarkColors.Header.Back);
        public static Pen Separator => FromDarkColor(DarkColors.Header.Separator);
    }

    public static class SelectedText
    {
        public static Pen Focus => FromDarkColor(DarkColors.SelectedText.Focus);
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