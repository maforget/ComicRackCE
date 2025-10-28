using System.Drawing;
using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Internal;

namespace cYo.Common.Windows.Forms.Theme.Resources;

/// <summary>
/// TODO : ADD SUMMARY
/// </summary>
public static class ThemeColors
{
	internal static ThemeColorTable ColorTable => colorTable ??= new ThemeColorTable();

	private static ThemeColorTable colorTable;

    public static bool IsDefault => ColorTable?.GetType() == typeof(ThemeColorTable);

	internal static void Register<T>() where T : ThemeColorTable, new()
	{
		colorTable = new T();
	}

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
        //public static Color Back => ColorTable.RatingControlBack;
        //public static Color Rated => ColorTable.RatingControlRated;
        public static Color Unrated => ColorTable.RatingControlUnrated;
    }

    public static class SplitButton
    {
        public static Color SeparatorLeft => ColorTable.SplitButtonSeparatorLeft;
        public static Color SeparatorRight => ColorTable.SplitButtonSeparatorRight;
    }

    public static class CheckBox
    {
        //public static Color Back => ColorTable.CheckBoxBack;
        //public static Color BackCorner => ColorTable.CheckBoxBackCorner;
        //public static Color BackVertex => ColorTable.CheckBoxBackVertex;
        //public static Color Border => ColorTable.CheckBoxBorder;
        //public static Color BorderEdge => ColorTable.CheckBoxBorderEdge;
        //public static Color BorderCorner => ColorTable.CheckBoxBorderCorner;
        //public static Color UncheckedBorder => ColorTable.CheckBoxUncheckedBorder;
        //public static Color UncheckedBorderEdge => ColorTable.CheckBoxUncheckedBorderEdge;
        //public static Color UncheckedBack => ColorTable.CheckBoxUncheckedBack;
        //public static Color UncheckedBackCorner => ColorTable.CheckBoxUncheckedBackCorner;
        //public static Color UncheckedBackVertex => ColorTable.CheckBoxUncheckedBackVertex;
        //public static Color UncheckedDisabledBorder => ColorTable.CheckBoxUncheckedDisabledBorder;
        //public static Color UncheckedDisabledBorderEdge => ColorTable.CheckBoxUncheckedDisabledBorderEdge;
        //public static Color UncheckedDisabledBackCorner => ColorTable.CheckBoxUncheckedDisabledBackCorner;
        //public static Color UncheckedDisabledBackVertex => ColorTable.CheckBoxUncheckedDisabledBackVertex;
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
        //public static readonly Color Back = ColorTable.ToolTipBack;
        public static readonly Color InfoText = ColorTable.ToolTipText;
    }

    public static class Stack
    {
        public static readonly Color Fill = ColorTable.StackFill;
        public static readonly Color Border = ColorTable.StackBorder;
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

    public static class ComboBox
    {
        public static Color Separator => ColorTable.ComboBoxSeparator;
    }

    public static class Header
    {
        public static Color Back => ColorTable.HeaderBack;
        public static Color Separator => ColorTable.HeaderSeparator;
        //public static Color Text => ColorTable.HeaderText;
    }

    #endregion


    // These are colors only used in Dark Mode. They should not be directly part of ThemeColors
    #region Dark Mode Colors

    public static class DarkMode
    {
        public static readonly Color BlackSmoke = DarkColors.BlackSmoke;
        //public static class Button
        //{
        //    public static readonly Color Back = DarkColors.Button.Back; // RGB 50 HEX 32
        //    public static readonly Color Text = DarkColors.Button.Text;
        //}

        public static class ComboBox
        {
            public static readonly Color Disabled = DarkColors.ComboBox.Disabled;
        }

        public static class RatingControl
        {
            public static readonly Color Back = DarkColors.RatingControl.Back;
            public static readonly Color Rated = DarkColors.RatingControl.Rated;
        }

        //public static class SelectedText
        //{
        //    public static readonly Color Focus = DarkColors.SelectedText.Focus;
        //    public static readonly Color Highlight = DarkColors.SelectedText.Highlight;
        //}

        //public static class TreeView
        //{
        //    public static readonly Color Back = DarkColors.TreeView.Back;
        //    public static readonly Color Text = DarkColors.TreeView.Text;
        //}

        //public static class UIComponent
        //{
        //    public static readonly Color SidePanel = DarkColors.UIComponent.SidePanel;
        //}
    }

    #endregion
}
