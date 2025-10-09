using cYo.Common.Drawing;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms
{
    public static class ThemeColors
    {
        internal static ThemeColorTable ColorTable => colorTable;

        private static ThemeColorTable colorTable = new ThemeColorTable();
        public static void Default() => colorTable = new ThemeColorTable();
        public static void Dark() => colorTable = new DarkThemeColorTable();


        // WhiteSmoke is RGB 245 but RGB 10 would be too dark
        // WhiteSmoke - RGB 245 - DisplayWorkspace Background and PreferencesDialog 
        public static readonly Color BlackSmoke = Color.FromArgb(48, 48, 48);

        // Gainsboro  - RGB 220 - PreferencesDialog label(Navigation|Status|Page|VisiblePart)Overlay
        public static readonly Color Lossboro = SystemColors.ControlDarkDark;

        /// <summary>
        /// Colors used for specific components with the app. i.e. hardcoded.
        /// </summary>
        /// <remarks>
        /// KnownColor replacement is temporary. We should replace them where they are used in code, ideally by defining a Default color scheme.
        /// </remarks>
        #region App Colors

        #region Singles
        public static Color ItemDrawInfoText => colorTable.ItemDrawInfoText;
        //public static Color ControlStyleColorTableBorder => colorTable.ControlStyleColorTableBorder;
        #endregion

        #region General
        //public static class ScrollbarPanel
        //{
        //    public static Color Border => colorTable.ScrollbarPanelBorder;
        //}

        public static class CollapsibleGroupBox
        {
            public static Color HeaderGradientStart => colorTable.CollapsibleGroupBoxHeaderBackGradientStart;
            public static Color HeaderGradientEnd => colorTable.CollapsibleGroupBoxHeaderBackGradientEnd;
            public static Color HeaderText => colorTable.CollapsibleGroupBoxHeaderText;
        }

        public static class StyledRenderer
        {
            public static Color Selection => colorTable.StyledSelectionBack;
            public static Color SelectionFocused => colorTable.StyledSelectionFocusedBack;
        }

        public static class ThumbRenderer
        {
            public static Color SelectionBack => colorTable.ThumbRendererSelectionBack;
        }

        public static class ThumbTileRenderer
        {
            public static Color Emboss => colorTable.ThumbTileRendererEmboss;
            public static Color TitleText => colorTable.ThumbTileRendererTitleText;
            public static Color BodyText => colorTable.ThumbTileRendererBodyText;
        }

        public static class ThumbnailViewItem
        {
            public static Color Back => colorTable.ThumbnailViewItemBack;
            public static Color HighlightText => colorTable.ThumbnailViewItemHighlightText;
            public static Color Border => colorTable.ThumbnailViewItemBorder;
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
            public static Color FavViewBack => colorTable.ComicListFolderFilesBrowserFavViewBack;
        }

        public static class ComicListLibraryBrowser
        {
            public static Color FavViewBack => colorTable.ComicListLibraryBrowserFavViewBack;
        }

        public static class ItemView
        {
            public static Color DefaultBack => colorTable.ItemViewDefaultBack;
            public static Color MainBack => colorTable.ItemViewMainBack;
            public static Color GroupText => colorTable.ItemViewGroupText;
            public static Color GroupSeparator => colorTable.ItemViewGroupSeparator;
        }

        public static class LibraryTree
        {
            public static Color TotalBack => colorTable.LibraryTreeTotalBookCountBack;
            public static Color TotalText => colorTable.LibraryTreeTotalBookCountText;
            public static Color UnreadBack => colorTable.LibraryTreeUnreadBookCountBack;
            public static Color UnreadText => colorTable.LibraryTreeUnreadBookCountText;
            public static Color NewBack => colorTable.LibraryTreeNewBookCountBack;
            public static Color NewText => colorTable.LibraryTreeNewBookCountText;
        }

        public static class SmallComicPreview
        {
            public static Color PageViewerBack => colorTable.SmallComicPreviewPageViewerBack;
            public static Color PageViewerText => colorTable.SmallComicPreviewPageViewerText;
        }

        public static class TabBar
        {
            public static readonly Color Back = colorTable.TabBarBack;
            public static readonly Color DefaultBorder = colorTable.TabBarDefaultBorder;
            public static readonly Color SelectedBack = colorTable.TabBarSelectedBack;
        }

        public static class ToolTip
        {
            public static readonly Color InfoText = colorTable.ToolTipText;
            public static readonly Color Back = colorTable.ToolTipBack;
        }
        #endregion

        #region Dialogs
        public static class ComicBook
        {
            public static Color Link => colorTable.ComicBookLink;
            public static Color VisitedLink => colorTable.ComicBookVisitedLink;
            public static Color PageViewer => colorTable.ComicBookPageViewerText;
            public static Color PanelBack => colorTable.ComicBookPanelBack;
            public static Color Separator => colorTable.ComicBookWhereSeparator;
        }

        public static class DeviceEditControl
        {
            public static Color Back => colorTable.DeviceEditControlBack;
        }

        public static class Preferences
        {
            public static Color PanelReader => colorTable.PreferencesPanelReaderOverlay;
            public static Color LabelOverlays => colorTable.PreferencesLabelOverlays;
            public static Color ServerEditControl => colorTable.PreferencesServerEditControl;
        }

        public static class SmartQuery
        {
            public static readonly Color Back = colorTable.SmartQueryBack;
            public static readonly Color Exception = colorTable.SmartQueryException;
            public static readonly Color Text = colorTable.SmartQueryText;
            public static readonly Color Keyword = colorTable.SmartQueryKeyword;
            public static readonly Color Qualifier = colorTable.SmartQueryQualifier;
            public static readonly Color Negation = colorTable.SmartQueryNegation;
            public static readonly Color String = colorTable.SmartQueryString;
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
        }

        public static class List
        {
            public static readonly Color Back = Color.FromArgb(56, 56, 56); // to match ComboBox button
            public static readonly Color Disabled = Color.FromArgb(64, 64, 64); // from .net combobox source
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

        public static class TreeView
        {
            public static readonly Color Back = ThemeColors.TextBox.Back;
            public static readonly Color Text = SystemColors.ControlText;
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
            public static readonly Color Back = SystemColors.Control; // RGB 32 HEX 20
            public static readonly Color Separator = Color.FromArgb(99, 99, 99);
            public static readonly Color Text = SystemColors.WindowText;
        }

        public static class SelectedText
        {
            public static readonly Color HighLight = Color.FromArgb(52, 67, 86);
            public static readonly Color Focus = Color.FromArgb(40, 100, 180);
        }
        #endregion

        public static class Material
        {
            public static readonly Color Window = Color.FromArgb(unchecked((int)0xFF333333));    // Form Background. SystemColors.Control; // RGB 32 HEX 20
            public static readonly Color SidePanel = Color.FromArgb(unchecked((int)0xFF191919)); // RGB 25 HEX 19
            public static readonly Color Content = SystemColors.Control;                         // MainViewItemView + CollapsibleGroupBox + background
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
        public virtual Color ComicBookPanelBack => SystemColors.ButtonShadow;
        public virtual Color ComicBookWhereSeparator => SystemColors.ButtonShadow;

        // ComicListFolderFilesBrowser
        public virtual Color ComicListFolderFilesBrowserFavViewBack => SystemColors.Window;

        // ComicListLibraryBrowserF
        public virtual Color ComicListLibraryBrowserFavViewBack => SystemColors.Window;

        // CollapsibleGroupBox
        public virtual Color CollapsibleGroupBoxHeaderBackGradientStart => SystemColors.Control;
        public virtual Color CollapsibleGroupBoxHeaderBackGradientEnd => SystemColors.ControlLight;
        public virtual Color CollapsibleGroupBoxHeaderText => Color.Empty; //ForeColor

        // DeviceEditControl
        public virtual Color DeviceEditControlBack => Color.Transparent;

        // ControlStyleColorTable
        public virtual Color ControlStyleColorTableBorder => Color.Black;

        // MainForm
        public virtual Color MainFormToolStripBack => Color.Transparent;

        // MainView
        public virtual Color MainViewBack => Color.Transparent;
        public virtual Color MainViewToolStripBack => Color.Transparent;

        // SimpleScrollbarPanel
        public virtual Color ScrollbarPanelBorder => Color.LightGray;

        // SmallComicPreview
        public virtual Color SmallComicPreviewPageViewerBack => SystemColors.Window;
        public virtual Color SmallComicPreviewPageViewerText => Color.LightGray;

        // ThumbTileRenderer
        public virtual Color ThumbTileRendererEmboss => Color.White;
        public virtual Color ThumbTileRendererTitleText => Color.Black;
        public virtual Color ThumbTileRendererBodyText => Color.Gray;

        // ThumbRenderer
        public virtual Color ThumbRendererSelectionBack => SystemColors.Highlight;

        // ThumbnailViewItem
        public virtual Color ThumbnailViewItemBack => Color.LightGray; // CoverViewItem, FavoriteViewitem, FolderViewItem
        public virtual Color ThumbnailViewItemHighlightText => SystemColors.HighlightText; // FavoriteViewitem, FolderViewItem
        public virtual Color ThumbnailViewItemBorder => Color.FromArgb(48, SystemColors.ControlDark);

        // PreferencesDialog
        public virtual Color PreferencesPanelReaderOverlay => Color.WhiteSmoke;
        public virtual Color PreferencesLabelOverlays => Color.Gainsboro;
        public virtual Color PreferencesServerEditControl => Color.Transparent;

        // StyledRenderer
        public virtual Color StyledSelectionBack => Color.Gray;
        public virtual Color StyledSelectionFocusedBack => SystemColors.Highlight;

        // ItemView
        public virtual Color ItemViewMainBack => SystemColors.Window;
        public virtual Color ItemViewDefaultBack => SystemColors.Window;
        public virtual Color ItemViewGroupText => Color.DarkBlue;
        public virtual Color ItemViewGroupSeparator => SystemColors.ControlDark;

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
        public virtual Color SmartQueryBack => SystemColors.Window;
        public virtual Color SmartQueryException => Color.LightGray;
        public virtual Color SmartQueryText => SystemColors.WindowText;
        public virtual Color SmartQueryKeyword => Color.Green;
        public virtual Color SmartQueryQualifier => Color.Blue;
        public virtual Color SmartQueryNegation => Color.DarkRed;
        public virtual Color SmartQueryString => Color.Red;

        // TabBar
        public virtual Color TabBarBack => Color.Transparent; // TabBar.BackColor
        public virtual Color TabBarDefaultBorder => SystemColors.AppWorkspace; // replaced when RenderWithVisualStyles enabled 
        public virtual Color TabBarSelectedBack => SystemColors.ControlLightLight;

        // ToolTip
        public virtual Color ToolTipBack => Color.Empty;
        public virtual Color ToolTipText => SystemColors.InfoText;
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
        //public override Color ComicBookPageViewerText => SystemColors.Control;
        //public override Color ComicBookPanelBack => Color.Red;
        //public override Color ComicBookWhereSeparator => Color.Cyan;

        // CollapsibleGroupBox
        public override Color CollapsibleGroupBoxHeaderBackGradientStart => SystemColors.Control;
        public override Color CollapsibleGroupBoxHeaderBackGradientEnd => SystemColors.ControlDark;
        public override Color CollapsibleGroupBoxHeaderText => SystemColors.ControlText;

        // ComicListFolderFilesBrowser
        public override Color ComicListFolderFilesBrowserFavViewBack => ThemeColors.Material.SidePanel;

        // ComicListLibraryBrowserF
        public override Color ComicListLibraryBrowserFavViewBack => ThemeColors.Material.SidePanel;

        // ControlStyleColorTable
        //public override Color ControlStyleColorTableBorder => Color.Black;

        // DeviceEditControl
        public override Color DeviceEditControlBack => SystemColors.Control;

        // MainForm
        //public override Color MainFormToolStripBack => Color.Transparent;

        //MainView
        //public override Color MainViewBack => Color.Transparent;
        //public override Color MainViewToolStripBack => Color.Transparent;

        // SimpleScrollbarPanel
        //public override Color ScrollbarPanelBorder => ThemeColors.BlackSmoke;

        // SmallComicPreview
        public override Color SmallComicPreviewPageViewerBack => ThemeColors.Material.SidePanel;
        public override Color SmallComicPreviewPageViewerText => ThemeColors.BlackSmoke;

        // StyledRenderer
        public override Color StyledSelectionBack => Color.Gray;
        public override Color StyledSelectionFocusedBack => SystemColors.Highlight;

        // ThumbTileRenderer
        public override Color ThumbTileRendererEmboss => ThemeColors.BlackSmoke; // Color.FromArgb(unchecked((int)0xFF303030));
        public override Color ThumbTileRendererTitleText => Color.White; // Color.FromArgb(unchecked((int)0xFFD0D0D0));
        public override Color ThumbTileRendererBodyText => Color.LightGray;

        // ThumbRenderer
        public override Color ThumbRendererSelectionBack => SystemColors.Highlight;

        // ThumbnailViewItem
        public override Color ThumbnailViewItemBack => Color.FromArgb(72,72,72); // CoverViewItem, FavoriteViewitem, FolderViewItem
        //public override Color ThumbnailViewItemHighlightText => SystemColors.HighlightText; // FavoriteViewitem, FolderViewItem
        public override Color ThumbnailViewItemBorder => Color.FromArgb(64, 190,190,190);

        // PreferencesDialog
        public override Color PreferencesPanelReaderOverlay => ThemeColors.BlackSmoke;
        public override Color PreferencesLabelOverlays => ThemeColors.Lossboro;
        public override Color PreferencesServerEditControl => SystemColors.Control;

        // ItemView
        public override Color ItemViewMainBack => ThemeColors.Material.Content;
        public override Color ItemViewDefaultBack => SystemColors.Window;
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
        public override Color SmartQueryText => SystemColors.ControlText;
        public override Color SmartQueryKeyword => Color.FromArgb(250, 198, 0);
        public override Color SmartQueryQualifier => Color.FromArgb(76, 163, 255);
        public override Color SmartQueryNegation => Color.Red;
        public override Color SmartQueryString => Color.FromArgb(255, 125, 125);

        // TabBar
        public override Color TabBarBack => SystemColors.Window; // RGB 50 HEX 32
        public override Color TabBarDefaultBorder => Color.Black;
        public override Color TabBarSelectedBack => SystemColors.Window; // RGB 50 HEX 32

        // ToolTip
        public override Color ToolTipBack => SystemColors.Window; // should be SystemColors.Info; needs alpha-aware tweaks
        public override Color ToolTipText => SystemColors.ControlText; // should be SystemColors.InfoText; needs alpha-aware tweaks
    }

}
