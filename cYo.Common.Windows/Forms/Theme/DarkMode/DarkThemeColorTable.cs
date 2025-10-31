using cYo.Common.Windows.Forms.Theme.DarkMode.Resources;
using cYo.Common.Windows.Forms.Theme.Internal;
using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;

namespace cYo.Common.Windows.Forms.Theme.DarkMode;

/// <summary>
/// Dark Mode application <see cref="Color"/> values.<br/>
/// </summary>
/// <remarks>
/// Does not include <see cref="System.Drawing.SystemColors"/> or <see cref="System.Windows.Forms.Control"/> colors.
/// </remarks>
internal class DarkThemeColorTable : ThemeColorTable
{
    #region ComicBookDialog
    public override Color ComicBookLink => Color.LightBlue;
    //public override Color ComicBookVisitedLink => Color.MediumOrchid;
    //public override Color ComicBookPageViewerText => SystemColors.Control;
    //public override Color ComicBookPanelBack => Color.Red;
    //public override Color ComicBookWhereSeparator => Color.Cyan;
    #endregion

    #region CollapsibleGroupBox
    public override Color CollapsibleGroupBoxBack => DarkColors.UIComponent.Content;
    public override Color CollapsibleGroupBoxHeaderBackGradientStart => SystemColors.Control;
    public override Color CollapsibleGroupBoxHeaderBackGradientEnd => SystemColors.ControlDark;
    public override Color CollapsibleGroupBoxHeaderText => SystemColors.ControlText;
    #endregion

    // ComicListFolderFilesBrowser
    public override Color ComicListFolderFilesBrowserFavViewBack => DarkColors.UIComponent.SidePanel;

    // ComicListLibraryBrowserF
    public override Color ComicListLibraryBrowserFavViewBack => DarkColors.UIComponent.SidePanel;

    // ControlStyleColorTable
    //public override Color ControlStyleColorTableBorder => Color.Black;

    // DeviceEditControl
    public override Color DeviceEditControlBack => SystemColors.Control;

    // MainForm
    //public override Color MainFormToolStripBack => Color.Transparent;

    //MainView
    //public override Color MainViewBack => Color.Transparent;
    //public override Color MainViewToolStripBack => Color.Transparent;

    public override Color MatcherGroupEditor => DarkColors.UIComponent.Window;

    // SimpleScrollbarPanel
    //public override Color ScrollbarPanelBorder => ThemeColors.BlackSmoke;

    // SmallComicPreview
    public override Color SmallComicPreviewPageViewerBack => DarkColors.UIComponent.SidePanel;
    public override Color SmallComicPreviewPageViewerText => DarkColors.BlackSmoke;

    // StyledRenderer
    public override Color StyledSelectionBack => Color.Gray;
    public override Color StyledSelectionFocusedBack => SystemColors.Highlight;

    // ThumbTileRenderer
    public override Color ThumbTileRendererEmboss => DarkColors.BlackSmoke; // Color.FromArgb(unchecked((int)0xFF303030));
    public override Color ThumbTileRendererTitleText => Color.White; // Color.FromArgb(unchecked((int)0xFFD0D0D0));
    public override Color ThumbTileRendererBodyText => Color.LightGray;

    // ThumbRenderer
    public override Color ThumbRendererSelectionBack => SystemColors.Highlight;

    // ThumbnailViewItem
    public override Color ThumbnailViewItemBack => Color.FromArgb(72, 72, 72); // CoverViewItem, FavoriteViewitem, FolderViewItem
    //public override Color ThumbnailViewItemHighlightText => SystemColors.HighlightText; // FavoriteViewitem, FolderViewItem
    public override Color ThumbnailViewItemBorder => Color.FromArgb(64, SystemColors.InactiveCaptionText);

    // PreferencesDialog
    public override Color PreferencesPanelReaderOverlay => DarkColors.BlackSmoke;
    public override Color PreferencesLabelOverlays => ThemeColors.Lossboro;
    public override Color PreferencesServerEditControl => SystemColors.Control;

    // ItemView
    public override Color ItemViewMainBack => DarkColors.UIComponent.Content;
    public override Color ItemViewDefaultBack => SystemColors.Window;
    public override Color ItemViewGroupText => Color.LightSkyBlue;
    public override Color ItemViewGroupSeparator => SystemColors.InactiveCaptionText; //Color.FromArgb(190, 190, 190);

    // NiceTreeSkin
    public override Color NiceTreeSkinDragSeparator => SystemColors.InactiveCaptionText; // Color.FromArgb(190, 190, 190);

    // LibraryTreeSkin
    public override Color LibraryTreeTotalBookCountBack => Color.FromArgb(0xFF, 0x00, 0xC0, 0x00);
    //public override Color LibraryTreeTotalBookCountText => ThemeColors.BlackSmoke;
    //public override Color LibraryTreeUnreadBookCountBack => Color.Orange;
    //public override Color LibraryTreeUnreadBookCountText => ThemeColors.BlackSmoke;
    //public override Color LibraryTreeNewBookCountBack => Color.Red;
    //public override Color LibraryTreeNewBookCountText => ThemeColors.BlackSmoke;

    // SmartQuery
    public override Color SmartQueryBack => DarkColors.TextBox.Back;
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
    //public override Color ToolTipBack => SystemColors.Window; // should be SystemColors.Info; needs alpha-aware tweaks
    public override Color ToolTipText => SystemColors.ControlText; // should be SystemColors.InfoText; needs alpha-aware tweaks

    // TreeView
    //public override Color TreeViewBack => DarkColors.TreeView.Back;
    //public override Color TreeViewText => DarkColors.TreeView.Text;

    // ComboBox
    public override Color ComboBoxSeparator => SystemColors.ControlText;

    // Caption
    public override Color CaptionBack => Color.Black;
    public override Color CaptionText => SystemColors.ActiveCaptionText;

    // Header
    public override Color HeaderBack => DarkColors.Header.Back;
    public override Color HeaderSeparator => DarkColors.Header.Separator; //Color.FromArgb(99, 99, 99);
    //public override Color HeaderText => SystemColors.WindowText; // only used in ThemeExtension.ListView_DrawColumnHeader, which is only used in dark mode

    // Control defaults
    //public override Color ListBoxBack => Color.FromArgb(46, 46, 46); // to match ListView BackColor

    // RatingControl
    //public override Color RatingControlBack => DarkColors.ListBox.Back;
    //public override Color RatingControlRated => SystemColors.ControlText;
    public override Color RatingControlUnrated => SystemColors.GrayText;

    // SplitButton
    public override Color SplitButtonSeparatorLeft => SystemColors.WindowText;
    public override Color SplitButtonSeparatorRight => SystemColors.ButtonFace;

    // Stacks
    public override Color StackFill => Color.LightGray;
	public override Color StackBorder => Color.Black;
}
