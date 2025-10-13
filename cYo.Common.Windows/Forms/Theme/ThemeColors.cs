using cYo.Common.Drawing;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
    public static class ThemeColors
    {
		internal static ThemeColorTable ColorTable => colorTable ??= new ThemeColorTable();

		private static ThemeColorTable colorTable;

        public static bool IsDefault => ColorTable?.GetType() == typeof(ThemeColorTable);

		internal static void Register<T>() where T : ThemeColorTable, new()
		{
			colorTable = new T();
		}

        // WhiteSmoke is RGB 245 but RGB 10 would be too dark
        // WhiteSmoke - RGB 245 - DisplayWorkspace Background and PreferencesDialog 
        public static readonly Color BlackSmoke = Color.FromArgb(48, 48, 48);

        // Gainsboro  - RGB 220 - PreferencesDialog label(Navigation|Status|Page|VisiblePart)Overlay
        public static readonly Color Lossboro = SystemColorsEx.ControlDarkDark;

        /// <summary>
        /// Colors used for specific components with the app. i.e. hardcoded.
        /// </summary>
        /// <remarks>
        /// KnownColor replacement is temporary. We should replace them where they are used in code, ideally by defining a Default color scheme.
        /// </remarks>
        #region App Colors

        #region Singles
        public static Color ItemDrawInfoText => ColorTable.ItemDrawInfoText;
        //public static Color ControlStyleColorTableBorder => ColorTable.ControlStyleColorTableBorder;

        public static Color MatcherGroupEditor => ColorTable.MatcherGroupEditor; // double reference - should add MatcherGroupEditorControlBack
        #endregion

        #region General
        //public static class ScrollbarPanel
        //{
        //    public static Color Border => colorTable.ScrollbarPanelBorder;
        //}

        public static class CollapsibleGroupBox
        {
            public static Color Back => ColorTable.CollapsibleGroupBoxBack;
            public static Color HeaderGradientStart => ColorTable.CollapsibleGroupBoxHeaderBackGradientStart;
            public static Color HeaderGradientEnd => ColorTable.CollapsibleGroupBoxHeaderBackGradientEnd;
            public static Color HeaderText => ColorTable.CollapsibleGroupBoxHeaderText;
        }

        public static class StyledRenderer
        {
            public static Color Selection => ColorTable.StyledSelectionBack;
            public static Color SelectionFocused => ColorTable.StyledSelectionFocusedBack;
        }

        public static class ThumbRenderer
        {
            public static Color SelectionBack => ColorTable.ThumbRendererSelectionBack;
        }

        public static class ThumbTileRenderer
        {
            public static Color Emboss => ColorTable.ThumbTileRendererEmboss;
            public static Color TitleText => ColorTable.ThumbTileRendererTitleText;
            public static Color BodyText => ColorTable.ThumbTileRendererBodyText;
        }

        public static class ThumbnailViewItem
        {
            public static Color Back => ColorTable.ThumbnailViewItemBack;
            public static Color HighlightText => ColorTable.ThumbnailViewItemHighlightText;
            public static Color Border => ColorTable.ThumbnailViewItemBorder;
        }

        public static class Caption
        {
            public static Color Back => ColorTable.CaptionBack;
            public static Color Text => ColorTable.CaptionText;
        }

        public static class RatingControl
        {
            public static Color Back => ColorTable.RatingControlBack;
            public static Color Rated => ColorTable.RatingControlRated;
            public static Color Unrated => ColorTable.RatingControlUnrated;
        }

        public static class SplitButton
        {
            public static Color SeparatorLeft => ColorTable.SplitButtonSeparatorLeft;
            public static Color SeparatorRight => ColorTable.SplitButtonSeparatorRight;
        }

        public static class CheckBox
        {
            public static Color Back => ColorTable.CheckBoxBack;
            public static Color BackCorner => ColorTable.CheckBoxBackCorner;
            public static Color BackVertex => ColorTable.CheckBoxBackVertex;
            public static Color Border => ColorTable.CheckBoxBorder;
            public static Color BorderEdge => ColorTable.CheckBoxBorderEdge;
            public static Color BorderCorner => ColorTable.CheckBoxBorderCorner;
            public static Color UncheckedBorder => ColorTable.CheckBoxUncheckedBorder;
            public static Color UncheckedBorderEdge => ColorTable.CheckBoxUncheckedBorderEdge;
            public static Color UncheckedBack => ColorTable.CheckBoxUncheckedBack;
            public static Color UncheckedBackCorner => ColorTable.CheckBoxUncheckedBackCorner;
            public static Color UncheckedBackVertex => ColorTable.CheckBoxUncheckedBackVertex;
            public static Color UncheckedDisabledBorder => ColorTable.CheckBoxUncheckedDisabledBorder;
            public static Color UncheckedDisabledBorderEdge => ColorTable.CheckBoxUncheckedDisabledBorderEdge;
            public static Color UncheckedDisabledBackCorner => ColorTable.CheckBoxUncheckedDisabledBackCorner;
            public static Color UncheckedDisabledBackVertex => ColorTable.CheckBoxUncheckedDisabledBackVertex;
        }

        #endregion

        #region MainForm

        //public static class MainForm
        //{
        //    public static Color ToolStripBack => colorTable.MainFormToolStripBack;
        //}

        //public static class MainView
        //{
        //    public static Color Back => colorTable.MainViewBack;
        //    public static Color ToolStripBack => colorTable.MainViewToolStripBack;
        //}

        public static class ComicListFolderFilesBrowser
        {
            public static Color FavViewBack => ColorTable.ComicListFolderFilesBrowserFavViewBack;
        }

        public static class ComicListLibraryBrowser
        {
            public static Color FavViewBack => ColorTable.ComicListLibraryBrowserFavViewBack;
        }

        public static class ItemView
        {
            public static Color DefaultBack => ColorTable.ItemViewDefaultBack;
            public static Color MainBack => ColorTable.ItemViewMainBack;
            public static Color GroupText => ColorTable.ItemViewGroupText;
            public static Color GroupSeparator => ColorTable.ItemViewGroupSeparator;
        }

        public static class LibraryTree
        {
            public static Color TotalBack => ColorTable.LibraryTreeTotalBookCountBack;
            public static Color TotalText => ColorTable.LibraryTreeTotalBookCountText;
            public static Color UnreadBack => ColorTable.LibraryTreeUnreadBookCountBack;
            public static Color UnreadText => ColorTable.LibraryTreeUnreadBookCountText;
            public static Color NewBack => ColorTable.LibraryTreeNewBookCountBack;
            public static Color NewText => ColorTable.LibraryTreeNewBookCountText;
        }

        public static class SmallComicPreview
        {
            public static Color PageViewerBack => ColorTable.SmallComicPreviewPageViewerBack;
            public static Color PageViewerText => ColorTable.SmallComicPreviewPageViewerText;
        }

        public static class TabBar
        {
            public static readonly Color Back = ColorTable.TabBarBack;
            public static readonly Color DefaultBorder = ColorTable.TabBarDefaultBorder;
            public static readonly Color SelectedBack = ColorTable.TabBarSelectedBack;
        }

        public static class ToolTip
		{
            public static readonly Color InfoText = ColorTable.ToolTipText;
            public static readonly Color Back = ColorTable.ToolTipBack;
        }
        #endregion

        #region Dialogs
        public static class ComicBook
        {
            public static Color Link => ColorTable.ComicBookLink;
            public static Color VisitedLink => ColorTable.ComicBookVisitedLink;
            public static Color PageViewer => ColorTable.ComicBookPageViewerText;
            public static Color PanelBack => ColorTable.ComicBookPanelBack;
            public static Color Separator => ColorTable.ComicBookWhereSeparator;
        }

        public static class DeviceEditControl
        {
            public static Color Back => ColorTable.DeviceEditControlBack;
        }

        public static class Preferences
        {
            public static Color PanelReader => ColorTable.PreferencesPanelReaderOverlay;
            public static Color LabelOverlays => ColorTable.PreferencesLabelOverlays;
            public static Color ServerEditControl => ColorTable.PreferencesServerEditControl;
        }

        public static class SmartQuery
        {
            public static readonly Color Back = ColorTable.SmartQueryBack;
            public static readonly Color Exception = ColorTable.SmartQueryException;
            public static readonly Color Text = ColorTable.SmartQueryText;
            public static readonly Color Keyword = ColorTable.SmartQueryKeyword;
            public static readonly Color Qualifier = ColorTable.SmartQueryQualifier;
            public static readonly Color Negation = ColorTable.SmartQueryNegation;
            public static readonly Color String = ColorTable.SmartQueryString;
        }
        #endregion

        #region Color.Empty
        public static class ListBox
        {
            public static Color Back => ColorTable.ListBoxBack;
        }

        public static class TreeView
        {
            public static Color Back => ColorTable.TreeViewBack;
            public static Color Text => ColorTable.TreeViewText;
        }
        #endregion

        #endregion

        /// <summary>
        /// Colors used for all Controls of a type.
        /// </summary>
        #region Control Type Colors

        public static class Button
        {
            public static readonly Color Back = SystemColorsEx.Window; // RGB 50 HEX 32
            public static readonly Color CheckedBack = Color.FromArgb(102, 102, 102);
            public static readonly Color Text = Color.White;
            public static readonly Color Border = Color.FromArgb(155, 155, 155);
            public static readonly Color MouseOverBack = SystemColorsEx.ButtonShadow; // RGB 70 HEX 46
        }

        public static class ComboBox
        {
            public static readonly Color Back = Color.FromArgb(56, 56, 56); // to match ComboBox button
            public static readonly Color Disabled = Color.FromArgb(64, 64, 64); // from .net combobox source
            public static Color Separator => ColorTable.ComboBoxSeparator;
        }

        public static class TextBox
        {
            public static readonly Color Back = Color.FromArgb(unchecked((int)0xFF2E2E2E)); //Color.FromArgb(56, 56, 56);
            public static readonly Color MouseOverBack = Color.FromArgb(unchecked((int)0xFF222222)); //Color.FromArgb(86, 86, 86);
            public static readonly Color EnterBack = Color.FromArgb(unchecked((int)0xFF1A1A1A)); //Color.FromArgb(71, 71, 71);
        }

        public static class ToolStrip
        {
            // currently this is purely for statusstrip border.
            // TODO: move to renderer (will need to account for which borders need to be drawn)
            public static readonly Color BorderColor = Color.FromArgb(100, 100, 100);
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
            public static Color Back => ColorTable.HeaderBack;
            public static Color Separator => ColorTable.HeaderSeparator;
            public static Color Text => ColorTable.HeaderText;
        }

        public static class SelectedText
        {
            public static readonly Color Highlight = Color.FromArgb(52, 67, 86);
            public static readonly Color Focus = Color.FromArgb(40, 100, 180);
        }

        #endregion

        public static class Material
        {
            public static readonly Color Window = Color.FromArgb(unchecked((int)0xFF333333));    // Form Background. SystemColorsEx.Control; // RGB 32 HEX 20
            public static readonly Color SidePanel = Color.FromArgb(unchecked((int)0xFF191919)); // RGB 25 HEX 19
            public static readonly Color Content = SystemColorsEx.Control;                         // MainViewItemView + CollapsibleGroupBox + background
        }
    }

    public static class ThemePens
    {
        private static readonly Dictionary<Color, Pen> cache = new();

        public static class CheckBox
        {
            public static Pen Border => FromThemeColor(ThemeColors.CheckBox.Border);
            public static Pen UncheckedBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedBorder);
            public static Pen UncheckedDisabledBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBorder);
        }

        public static class ComboBox
        {
            public static Pen Separator => FromThemeColor(ThemeColors.ComboBox.Separator);
        }

        public static class SplitButton
        {
            public static Pen SeparatorLeft => FromThemeColor(ThemeColors.SplitButton.SeparatorLeft);
            public static Pen SeparatorRight => FromThemeColor(ThemeColors.SplitButton.SeparatorRight);
        }

        public static Pen FromThemeColor(Color color)
        {
            if (!cache.TryGetValue(color, out var pen))
            {
                pen = new Pen(color);
                cache[color] = pen;
            }
            return pen;
        }

        public static void Reset()
        {
            foreach (var pen in cache.Values)
                pen.Dispose();
            cache.Clear();
        }
    }

    public static class ThemeBrushes
    {
        private static readonly Dictionary<Color, SolidBrush> cache = new();

        public static class Caption
        {
            public static Brush Back => FromThemeColor(ThemeColors.Caption.Back);
        }

        public static class CheckBox
        {
            public static Brush Back => FromThemeColor(ThemeColors.CheckBox.Back);
            public static Brush BackCorner => FromThemeColor(ThemeColors.CheckBox.BackCorner);
            public static Brush BackVertex => FromThemeColor(ThemeColors.CheckBox.BackVertex);
            //public static Brush Border => FromThemeColor(ThemeColors.CheckBox.Border);
            public static Brush BorderEdge => FromThemeColor(ThemeColors.CheckBox.BorderEdge);
            public static Brush BorderCorner => FromThemeColor(ThemeColors.CheckBox.BorderCorner);
            //public static Brush UncheckedBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedBorder);
            public static Brush UncheckedBorderEdge => FromThemeColor(ThemeColors.CheckBox.UncheckedBorderEdge);
            public static Brush UncheckedBack => FromThemeColor(ThemeColors.CheckBox.UncheckedBack);
            public static Brush UncheckedBackCorner => FromThemeColor(ThemeColors.CheckBox.UncheckedBackCorner);
            public static Brush UncheckedBackVertex => FromThemeColor(ThemeColors.CheckBox.UncheckedBackVertex);
            //public static Brush UncheckedDisabledBorder => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBorder);
            public static Brush UncheckedDisabledBorderEdge => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBorderEdge);
            public static Brush UncheckedDisabledBackCorner => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBackCorner);
            public static Brush UncheckedDisabledBackVertex => FromThemeColor(ThemeColors.CheckBox.UncheckedDisabledBackVertex);
        }

        public static class CollapsibleGroupBox
        {
            public static Brush HeaderText => FromThemeColor(ThemeColors.CollapsibleGroupBox.HeaderText);
        }

        public static class Header
        {
            public static Brush Back => FromThemeColor(ThemeColors.Header.Back);
        }

        public static class SelectedText
        {
            public static Brush Highlight => FromThemeColor(ThemeColors.SelectedText.Highlight);
        }

        public static Brush FromThemeColor(Color color)
        {
            if (!cache.TryGetValue(color, out var brush))
            {
                brush = new SolidBrush(color);
                cache[color] = brush;
            }
            return brush;
        }

        public static void Reset()
        {
            foreach (var brush in cache.Values)
                brush.Dispose();
            cache.Clear();
        }
    }


    /// <summary>
    /// Base class, with default application <see cref="Color"/> values.
    /// </summary>
    /// <remarks>
    /// This only includes <see cref="Color"/> values which are directly referenced (i.e. hardcoded) in various places in the app.<br/>
    /// It does not include Theme colors which are only used for Dark Mode.<br/>
    /// It also omits ImageTransparentColor, which tends to be <see cref="Color.Magenta"/> and shouldn't be Theme-dependant.
    /// </remarks>
    internal class ThemeColorTable
    {
		// ComicBookDialog
		public virtual Color ComicBookLink => Color.SteelBlue;
        public virtual Color ComicBookVisitedLink => Color.MediumOrchid;
        public virtual Color ComicBookPageViewerText => Color.White;
        public virtual Color ComicBookPanelBack => SystemColorsEx.ButtonShadow;
        public virtual Color ComicBookWhereSeparator => SystemColorsEx.ButtonShadow;

        // ComicListFolderFilesBrowser
        public virtual Color ComicListFolderFilesBrowserFavViewBack => SystemColorsEx.Window;

        // ComicListLibraryBrowserF
        public virtual Color ComicListLibraryBrowserFavViewBack => SystemColorsEx.Window;

        // CollapsibleGroupBox
        public virtual Color CollapsibleGroupBoxBack => Color.Transparent;
        public virtual Color CollapsibleGroupBoxHeaderBackGradientStart => SystemColorsEx.Control;
        public virtual Color CollapsibleGroupBoxHeaderBackGradientEnd => SystemColorsEx.ControlLight;
        public virtual Color CollapsibleGroupBoxHeaderText => Control.DefaultForeColor;

        // DeviceEditControl
        public virtual Color DeviceEditControlBack => Color.Transparent;

        // ControlStyleColorTable
        public virtual Color ControlStyleColorTableBorder => Color.Black;

        // MainForm
        public virtual Color MainFormToolStripBack => Color.Transparent;

        // MainView
        public virtual Color MainViewBack => Color.Transparent;
        public virtual Color MainViewToolStripBack => Color.Transparent;

        public virtual Color MatcherGroupEditor => SystemColorsEx.Control;

        // SimpleScrollbarPanel
        public virtual Color ScrollbarPanelBorder => Color.LightGray;

        // SmallComicPreview
        public virtual Color SmallComicPreviewPageViewerBack => SystemColorsEx.Window;
        public virtual Color SmallComicPreviewPageViewerText => Color.LightGray;

        // ThumbTileRenderer
        public virtual Color ThumbTileRendererEmboss => Color.White;
        public virtual Color ThumbTileRendererTitleText => Color.Black;
        public virtual Color ThumbTileRendererBodyText => Color.Gray;

        // ThumbRenderer
        public virtual Color ThumbRendererSelectionBack => SystemColorsEx.Highlight;

        // ThumbnailViewItem
        public virtual Color ThumbnailViewItemBack => Color.LightGray; // CoverViewItem, FavoriteViewitem, FolderViewItem
        public virtual Color ThumbnailViewItemHighlightText => SystemColorsEx.HighlightText; // FavoriteViewitem, FolderViewItem
        public virtual Color ThumbnailViewItemBorder => Color.FromArgb(48, SystemColorsEx.ControlDark);

        // PreferencesDialog
        public virtual Color PreferencesPanelReaderOverlay => Color.WhiteSmoke;
        public virtual Color PreferencesLabelOverlays => Color.Gainsboro;
        public virtual Color PreferencesServerEditControl => Color.Transparent;

        // StyledRenderer
        public virtual Color StyledSelectionBack => Color.Gray;
        public virtual Color StyledSelectionFocusedBack => SystemColorsEx.Highlight;

        // ItemView
        public virtual Color ItemViewMainBack => SystemColorsEx.Window;
        public virtual Color ItemViewDefaultBack => SystemColorsEx.Window;
        public virtual Color ItemViewGroupText => Color.DarkBlue;
        public virtual Color ItemViewGroupSeparator => SystemColorsEx.ControlDark;

        // ItemDrawInformation
        public virtual Color ItemDrawInfoText => Color.Black;

        // LibraryTreeSkin
        public virtual Color LibraryTreeTotalBookCountBack => Color.Green;
        public virtual Color LibraryTreeTotalBookCountText => Color.White;
        public virtual Color LibraryTreeUnreadBookCountBack => Color.Orange;
        public virtual Color LibraryTreeUnreadBookCountText => Color.White;
        public virtual Color LibraryTreeNewBookCountBack => Color.Red;
        public virtual Color LibraryTreeNewBookCountText => Color.White;

        // SmartQuery
        public virtual Color SmartQueryBack => SystemColorsEx.Window;
        public virtual Color SmartQueryException => Color.LightGray;
        public virtual Color SmartQueryText => SystemColorsEx.WindowText;
        public virtual Color SmartQueryKeyword => Color.Green;
        public virtual Color SmartQueryQualifier => Color.Blue;
        public virtual Color SmartQueryNegation => Color.DarkRed;
        public virtual Color SmartQueryString => Color.Red;

        // TabBar
        public virtual Color TabBarBack => Color.Transparent; // TabBar.BackColor
        public virtual Color TabBarDefaultBorder => SystemColorsEx.AppWorkspace; // replaced when RenderWithVisualStyles enabled 
        public virtual Color TabBarSelectedBack => SystemColorsEx.ControlLightLight;

        // ToolTip
        public virtual Color ToolTipBack => Color.Empty;
        public virtual Color ToolTipText => SystemColorsEx.InfoText;

        // TreeView
        public virtual Color TreeViewBack => Color.Empty;
        public virtual Color TreeViewText => Color.Empty;

        // ComboBox
        public virtual Color ComboBoxSeparator => SystemColorsEx.ControlLight;

        // Caption
        public virtual Color CaptionBack => Color.White;
        public virtual Color CaptionText => SystemColorsEx.ActiveCaptionText;

        // Header
        public virtual Color HeaderBack => Color.White;
        public virtual Color HeaderSeparator => SystemColorsEx.ControlDark;
        public virtual Color HeaderText => Color.Empty; // only used in ThemeExtension.ListView_DrawColumnHeader, which is only used in dark mode

        // Control defaults
        public virtual Color ListBoxBack => Color.Empty; // BackColor - ListBox.BackColor - SystemColorsEx.Window

        // RatingControl
        public virtual Color RatingControlBack => Color.Empty; // BackColor - Control.DefaultBackColor - SystemColorsEx.Control
        public virtual Color RatingControlRated => Color.Empty; // ForeColor - Control.DefaultForeColor - SystemColorsEx.ControlText
        public virtual Color RatingControlUnrated => Color.LightGray;

        // SplitButton
        public virtual Color SplitButtonSeparatorLeft => SystemColorsEx.ButtonShadow;
        public virtual Color SplitButtonSeparatorRight => SystemColorsEx.ButtonFace;

        // CheckBox
        public virtual Color CheckBoxBack => Color.Empty;
        public virtual Color CheckBoxBackCorner => Color.Empty;
        public virtual Color CheckBoxBackVertex => Color.Empty;
        public virtual Color CheckBoxBorder => Color.Empty;
        public virtual Color CheckBoxBorderEdge => Color.Empty;
        public virtual Color CheckBoxBorderCorner => Color.Empty;
        public virtual Color CheckBoxUncheckedBorder => Color.Empty;
        public virtual Color CheckBoxUncheckedBorderEdge => Color.Empty;
        public virtual Color CheckBoxUncheckedBack => Color.Empty;
        public virtual Color CheckBoxUncheckedBackCorner => Color.Empty;
        public virtual Color CheckBoxUncheckedBackVertex => Color.Empty;
        public virtual Color CheckBoxUncheckedDisabledBorder => Color.Empty;
        public virtual Color CheckBoxUncheckedDisabledBorderEdge => Color.Empty;
        public virtual Color CheckBoxUncheckedDisabledBack => Color.Empty;
        public virtual Color CheckBoxUncheckedDisabledBackCorner => Color.Empty;
        public virtual Color CheckBoxUncheckedDisabledBackVertex => Color.Empty;
    }

    /// <summary>
    /// Dark Mode aplication <see cref="Color"/> values.<br/>
    /// </summary>
    /// <remarks>
    /// Does not include <see cref="SystemColors"/> or <see cref="System.Windows.Forms.Control"/> colors.
    /// </remarks>
    internal class DarkThemeColorTable : ThemeColorTable
    {
        // ComicBookDialog
        public override Color ComicBookLink => Color.LightBlue;
        //public override Color ComicBookVisitedLink => Color.MediumOrchid;
        //public override Color ComicBookPageViewerText => SystemColorsEx.Control;
        //public override Color ComicBookPanelBack => Color.Red;
        //public override Color ComicBookWhereSeparator => Color.Cyan;

        // CollapsibleGroupBox
        public override Color CollapsibleGroupBoxBack => ThemeColors.Material.Content;
        public override Color CollapsibleGroupBoxHeaderBackGradientStart => SystemColorsEx.Control;
        public override Color CollapsibleGroupBoxHeaderBackGradientEnd => SystemColorsEx.ControlDark;
        public override Color CollapsibleGroupBoxHeaderText => SystemColorsEx.ControlText;

        // ComicListFolderFilesBrowser
        public override Color ComicListFolderFilesBrowserFavViewBack => ThemeColors.Material.SidePanel;

        // ComicListLibraryBrowserF
        public override Color ComicListLibraryBrowserFavViewBack => ThemeColors.Material.SidePanel;

        // ControlStyleColorTable
        //public override Color ControlStyleColorTableBorder => Color.Black;

        // DeviceEditControl
        public override Color DeviceEditControlBack => SystemColorsEx.Control;

        // MainForm
        //public override Color MainFormToolStripBack => Color.Transparent;

        //MainView
        //public override Color MainViewBack => Color.Transparent;
        //public override Color MainViewToolStripBack => Color.Transparent;

        public override Color MatcherGroupEditor => ThemeColors.Material.Window;

        // SimpleScrollbarPanel
        //public override Color ScrollbarPanelBorder => ThemeColors.BlackSmoke;

        // SmallComicPreview
        public override Color SmallComicPreviewPageViewerBack => ThemeColors.Material.SidePanel;
        public override Color SmallComicPreviewPageViewerText => ThemeColors.BlackSmoke;

        // StyledRenderer
        public override Color StyledSelectionBack => Color.Gray;
        public override Color StyledSelectionFocusedBack => SystemColorsEx.Highlight;

        // ThumbTileRenderer
        public override Color ThumbTileRendererEmboss => ThemeColors.BlackSmoke; // Color.FromArgb(unchecked((int)0xFF303030));
        public override Color ThumbTileRendererTitleText => Color.White; // Color.FromArgb(unchecked((int)0xFFD0D0D0));
        public override Color ThumbTileRendererBodyText => Color.LightGray;

        // ThumbRenderer
        public override Color ThumbRendererSelectionBack => SystemColorsEx.Highlight;

        // ThumbnailViewItem
        public override Color ThumbnailViewItemBack => Color.FromArgb(72,72,72); // CoverViewItem, FavoriteViewitem, FolderViewItem
        //public override Color ThumbnailViewItemHighlightText => SystemColorsEx.HighlightText; // FavoriteViewitem, FolderViewItem
        public override Color ThumbnailViewItemBorder => Color.FromArgb(64, 190,190,190);

        // PreferencesDialog
        public override Color PreferencesPanelReaderOverlay => ThemeColors.BlackSmoke;
        public override Color PreferencesLabelOverlays => ThemeColors.Lossboro;
        public override Color PreferencesServerEditControl => SystemColorsEx.Control;

        // ItemView
        public override Color ItemViewMainBack => ThemeColors.Material.Content;
        public override Color ItemViewDefaultBack => SystemColorsEx.Window;
        public override Color ItemViewGroupText => Color.LightSkyBlue;
        public override Color ItemViewGroupSeparator => Color.FromArgb(190, 190, 190);

        // LibraryTreeSkin
        public override Color LibraryTreeTotalBookCountBack => Color.FromArgb(unchecked((int)0xFF00C000));
        //public override Color LibraryTreeTotalBookCountText => ThemeColors.BlackSmoke;
        //public override Color LibraryTreeUnreadBookCountBack => Color.Orange;
        //public override Color LibraryTreeUnreadBookCountText => ThemeColors.BlackSmoke;
        //public override Color LibraryTreeNewBookCountBack => Color.Red;
        //public override Color LibraryTreeNewBookCountText => ThemeColors.BlackSmoke;

        // SmartQuery
        public override Color SmartQueryBack => ThemeColors.TextBox.Back;
        public override Color SmartQueryException => Color.FromArgb(125, 31, 31);
        public override Color SmartQueryText => SystemColorsEx.ControlText;
        public override Color SmartQueryKeyword => Color.FromArgb(250, 198, 0);
        public override Color SmartQueryQualifier => Color.FromArgb(76, 163, 255);
        public override Color SmartQueryNegation => Color.Red;
        public override Color SmartQueryString => Color.FromArgb(255, 125, 125);

        // TabBar
        public override Color TabBarBack => SystemColorsEx.Window; // RGB 50 HEX 32
        public override Color TabBarDefaultBorder => Color.Black;
        public override Color TabBarSelectedBack => SystemColorsEx.Window; // RGB 50 HEX 32

        // ToolTip
        public override Color ToolTipBack => SystemColorsEx.Window; // should be SystemColorsEx.Info; needs alpha-aware tweaks
        public override Color ToolTipText => SystemColorsEx.ControlText; // should be SystemColorsEx.InfoText; needs alpha-aware tweaks

        // TreeView
        public override Color TreeViewBack => ThemeColors.TextBox.Back;
        public override Color TreeViewText => SystemColorsEx.ControlText;

        // ComboBox
        public override Color ComboBoxSeparator => SystemColorsEx.ControlText;

		// Caption
		public override Color CaptionBack => Color.Black;
        //public override Color CaptionText => SystemColorsEx.ActiveCaptionText;

        // Header
        public override Color HeaderBack => SystemColorsEx.Control;
        public override Color HeaderSeparator => Color.FromArgb(99, 99, 99);
        public override Color HeaderText => SystemColorsEx.WindowText;

        // Control defaults
        public override Color ListBoxBack => Color.FromArgb(46, 46, 46); // to match ListView BackColor

        // RatingControl
        public override Color RatingControlBack => ListBoxBack;
        public override Color RatingControlRated => SystemColorsEx.ControlText;
        public override Color RatingControlUnrated => SystemColorsEx.GrayText;

        public override Color SplitButtonSeparatorLeft => SystemColorsEx.WindowText;
        public override Color SplitButtonSeparatorRight => SystemColorsEx.ButtonFace;

        // CheckBox
        public override Color CheckBoxBack => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBackCorner => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBackVertex => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBorder => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBorderEdge => Color.FromArgb(4, 87, 166);
        public override Color CheckBoxBorderCorner => Color.FromArgb(28, 54, 74);
        public override Color CheckBoxUncheckedBorder => Color.FromArgb(98, 98, 98);
        public override Color CheckBoxUncheckedBorderEdge => Color.FromArgb(90, 90, 90);
        public override Color CheckBoxUncheckedBack => SystemColorsEx.ControlLight;
        public override Color CheckBoxUncheckedBackCorner => Color.FromArgb(48, 48, 48);
        public override Color CheckBoxUncheckedBackVertex => SystemColorsEx.ControlDark;
        public override Color CheckBoxUncheckedDisabledBorder => SystemColorsEx.ControlDark;
        public override Color CheckBoxUncheckedDisabledBorderEdge => Color.FromArgb(60, 60, 60);
        public override Color CheckBoxUncheckedDisabledBackCorner => Color.FromArgb(100, 100, 100);
        public override Color CheckBoxUncheckedDisabledBackVertex => SystemColorsEx.ControlDarkDark;
    }

}
