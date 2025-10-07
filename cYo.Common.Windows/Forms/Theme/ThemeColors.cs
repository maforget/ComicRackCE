using cYo.Common.Drawing;
using System.Drawing;
using System.Windows.Forms.VisualStyles;

namespace cYo.Common.Windows.Forms
{
    public static class ThemeColors
    {
        public static ThemeColorTable ColorTable => colorTable;

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

        public static class ComicBook
        {
            public static Color Link => colorTable.ComicBookLink;
            public static Color VisitedLink => colorTable.ComicBookVisitedLink;
            public static Color PageViewer => colorTable.ComicBookPageViewerFore;
        }

        public static class MainForm
        {
            public static Color ToolStripBack => colorTable.MainFormToolStripBack;
        }

        public static class MainView
        {
            public static Color Back => colorTable.MainViewBack;
            public static Color ToolStripBack => colorTable.MainViewToolStripBack;
        }

        public static class Preferences
        {
            public static Color PanelReader => colorTable.PreferencesPanelReaderOverlay;
            public static Color LabelOverlays => colorTable.PreferencesLabelOverlays;
        }

        public static class ScrollbarPanel
        {
            public static Color Border => colorTable.ScrollbarPanelBorder;
        }

        public static class SmallComicPreview
        {
            public static Color PageViewer => colorTable.SmallComicPreviewPageViewerFore;
        }

        // specific parts

        public static class ItemView
        {
            public static Color GroupText => colorTable.ItemViewGroupText; 
            public static Color GroupSeparator => colorTable.ItemViewGroupSeparator;
        }

        public static class SmartQuery
        {
            public static readonly Color Back = colorTable.SmartQueryBack;
            public static readonly Color Exception = colorTable.SmartQueryException;
            public static readonly Color Fore = colorTable.SmartQueryFore;
            public static readonly Color Keyword = colorTable.SmartQueryKeyword;
            public static readonly Color Qualifier = colorTable.SmartQueryQualifier;
            public static readonly Color Negation = colorTable.SmartQueryNegation;
            public static readonly Color String = colorTable.SmartQueryString;

        }

        public static class TabBar
        {
            public static readonly Color SelectedBack = colorTable.TabBarSelectedBack;
            public static readonly Color SelectedBorder = colorTable.TabBarSelectedBorder;
            //public static readonly Color SelectedFore = SystemColors.InfoText; // currently using ForeColor
            public static readonly Color Back = colorTable.TabBarBack;
            public static readonly Color Border = colorTable.TabBarBorder;
            //public static readonly Color Fore = SystemColors.InfoText;  // currently using ForeColor
        }

        public static class ToolTip
        {
            public static readonly Color InfoText = colorTable.ToolTipFore;
            public static readonly Color Back = colorTable.ToolTipBack;
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
            public static readonly Color Fore = Color.White;
            public static readonly Color Border = Color.FromArgb(155, 155, 155);
            public static readonly Color MouseOverBack = SystemColors.ButtonShadow; // RGB 70 HEX 46
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
            public static readonly Color Back = Color.FromArgb(46, 46, 46); //Color.FromArgb(56, 56, 56);
            public static readonly Color MouseOverBack = SystemColorsEx.ControlLight; //Color.FromArgb(86, 86, 86);
            public static readonly Color EnterBack = SystemColorsEx.ControlLightLight; //Color.FromArgb(71, 71, 71);
        }

        public static class ToolStrip
        {
            // currently this is purely for statusstrip border.
            // TODO: move to renderer (will need to account for which borders need to be drawn)
            public static readonly Color BorderColor = Color.FromArgb(100, 100, 100);
        }

        public static class TreeView
        {
            public static readonly Color Back = Material.SidePanel; // was SystemColors.Window;
            public static readonly Color Fore = SystemColors.ControlText;
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
            public static readonly Color Window = SystemColors.Control; // RGB 32 HEX 20
            public static readonly Color SidePanel = SystemColors.Control; // RGB 32 HEX 20
                                                                           //public static readonly Color Content = SystemColors.ControlLight; // RGB 46 HEX 2E
                                                                           //public static readonly Color Dark = Color.FromArgb(155, 155, 155);
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
    public class ThemeColorTable
    {
        // ComicBookDialog
        public virtual Color ComicBookLink => Color.SteelBlue;
        public virtual Color ComicBookVisitedLink => Color.MediumOrchid;
        public virtual Color ComicBookPageViewerFore => Color.White;

        // ControlStyleColorTable
        public virtual Color ColorTableBorder => Color.Black;

        // MainForm
        public virtual Color MainFormToolStripBack => Color.Transparent;

        // MainView
        public virtual Color MainViewBack => Color.Transparent;
        public virtual Color MainViewToolStripBack => Color.Transparent;

        // SimpleScrollbarPanel
        public virtual Color ScrollbarPanelBorder => Color.LightGray;

        // SmallComicPreview
        public virtual Color SmallComicPreviewPageViewerFore => Color.LightGray;

        // CoverViewItem, FavoriteViewitem, FolderViewItem
        public virtual Color ThumbTileRendererBack => Color.LightGray;

        // PreferencesDialog
        public virtual Color PreferencesPanelReaderOverlay => Color.WhiteSmoke;
        public virtual Color PreferencesLabelOverlays => Color.Gainsboro;

        // StyledRenderer
        public virtual Color SelectionBack => Color.Gray;
        public virtual Color SelectionFocusedBack => SystemColors.Highlight;

        // ItemView
        public virtual Color ItemViewGroupText => Color.DarkBlue;
        public virtual Color ItemViewGroupSeparator => SystemColors.ControlDark;

        public virtual Color ItemDrawInformationText => Color.Black;

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
        public virtual Color SmartQueryFore => SystemColors.WindowText;
        public virtual Color SmartQueryKeyword => Color.Green;
        public virtual Color SmartQueryQualifier => Color.Blue;
        public virtual Color SmartQueryNegation => Color.DarkRed;
        public virtual Color SmartQueryString => Color.Red;

        // TabBar
        public virtual Color TabBarBack => Color.Empty; // TabBar.BackColor
        public virtual Color TabBarBorder => Color.Empty; // TabBar.BorderColor
        public virtual Color TabBarSelectedBack => SystemColors.ControlLightLight;
        public virtual Color TabBarSelectedBorder => Color.Empty; // TabBar.BorderColor

        // ToolTip
        public virtual Color ToolTipBack => Color.Empty;
        public virtual Color ToolTipFore => SystemColors.InfoText;
    }

    /// <summary>
    /// Dark Mode aplication <see cref="Color"/> values.<br/>
    /// </summary>
    /// <remarks>
    /// Does not include <see cref="SystemColors"/> or <see cref="System.Windows.Forms.Control"/> colors.
    /// </remarks>
    public class DarkThemeColorTable : ThemeColorTable
    {
        // ComicBookDialog
        public override Color ComicBookLink => Color.SteelBlue;
        public override Color ComicBookVisitedLink => Color.MediumOrchid;
        public override Color ComicBookPageViewerFore => SystemColors.Control;

        // ControlStyleColorTable
        public override Color ColorTableBorder => Color.Black;

        // MainForm
        public override Color MainFormToolStripBack => Color.Transparent;

        //MainView
        public override Color MainViewBack => Color.Transparent;
        public override Color MainViewToolStripBack => Color.Transparent;

        // SimpleScrollbarPanel
        public override Color ScrollbarPanelBorder => Color.LightGray;

        // SmallComicPreview
        public override Color SmallComicPreviewPageViewerFore => Color.LightGray;

        // StyledRenderer
        public override Color SelectionBack => Color.Gray;
        public override Color SelectionFocusedBack => SystemColors.Highlight;

        // CoverViewItem, FavoriteViewitem, FolderViewItem
        public override Color ThumbTileRendererBack => Color.LightGray;

        // PreferencesDialog
        public override Color PreferencesPanelReaderOverlay => ThemeColors.BlackSmoke;
        public override Color PreferencesLabelOverlays => ThemeColors.Lossboro;

        // ItemView
        public override Color ItemViewGroupText => Color.LightSkyBlue;
        public override Color ItemViewGroupSeparator => Color.FromArgb(190, 190, 190);

        // SmartQuery
        public override Color SmartQueryBack => ThemeColors.TextBox.Back;
        public override Color SmartQueryException => Color.FromArgb(125, 31, 31);
        public override Color SmartQueryFore => SystemColors.ControlText;
        public override Color SmartQueryKeyword => Color.FromArgb(250, 198, 0);
        public override Color SmartQueryQualifier => Color.FromArgb(76, 163, 255);
        public override Color SmartQueryNegation => Color.Red;
        public override Color SmartQueryString => Color.FromArgb(255, 125, 125);

        // TabBar
        public override Color TabBarBack => SystemColors.Window; // RGB 50 HEX 32
        public override Color TabBarBorder => Color.Black;
        public override Color TabBarSelectedBack => SystemColors.Window; // RGB 50 HEX 32
        public override Color TabBarSelectedBorder => Color.Black;

        // ToolTip
        public override Color ToolTipBack => SystemColors.Window; // should be SystemColors.Info; needs alpha-aware tweaks
        public override Color ToolTipFore => SystemColors.ControlText; // should be SystemColors.InfoText; needs alpha-aware tweaks
    }

}
