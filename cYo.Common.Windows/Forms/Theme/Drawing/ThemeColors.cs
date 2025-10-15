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
        public static readonly Color Lossboro = SystemColors.ControlDarkDark;

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

        public static class NiceTreeSkin
        {
            public static Color Separator => ColorTable.NiceTreeSkinDragSeparator;
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
            public static readonly Color Back = SystemColors.Window; // RGB 50 HEX 32
            public static readonly Color CheckedBack = Color.FromArgb(102, 102, 102);
            public static readonly Color Text = Color.White;
            public static readonly Color Border = Color.FromArgb(155, 155, 155);
            public static readonly Color MouseOverBack = SystemColors.ButtonShadow; // RGB 70 HEX 46
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
            public static readonly Color Window = Color.FromArgb(unchecked((int)0xFF333333));    // Form Background. SystemColors.Control; // RGB 32 HEX 20
            public static readonly Color SidePanel = Color.FromArgb(unchecked((int)0xFF191919)); // RGB 25 HEX 19
            public static readonly Color Content = SystemColors.Control;                         // MainViewItemView + CollapsibleGroupBox + background
        }
    }

}
