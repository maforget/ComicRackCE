using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms.Theme.Internal;

/// <summary>
/// Base class, with default application <see cref="Color"/> values.
/// </summary>
/// <remarks>
/// This only includes <see cref="Color"/> values which are directly referenced (i.e. hard-coded) in various places in the app.<br/>
/// It does not include Theme colors which are only used for Dark Mode.<br/>
/// It also omits ImageTransparentColor, which tends to be <see cref="Color.Magenta"/> and shouldn't be Theme-dependent.
/// </remarks>
internal class ThemeColorTable
{
    #region ComicBookDialog
    public virtual Color ComicBookLink => Color.SteelBlue;
    public virtual Color ComicBookVisitedLink => Color.MediumOrchid;
    public virtual Color ComicBookPageViewerText => Color.White;
    public virtual Color ComicBookPanelBack => System.Drawing.SystemColors.ButtonShadow;
    public virtual Color ComicBookWhereSeparator => System.Drawing.SystemColors.ButtonShadow;
    #endregion

    // ComicListFolderFilesBrowser
    public virtual Color ComicListFolderFilesBrowserFavViewBack => System.Drawing.SystemColors.Window;

    // ComicListLibraryBrowserF
    public virtual Color ComicListLibraryBrowserFavViewBack => System.Drawing.SystemColors.Window;

    #region CollapsibleGroupBox
    public virtual Color CollapsibleGroupBoxBack => Color.Transparent;
    public virtual Color CollapsibleGroupBoxHeaderBackGradientStart => System.Drawing.SystemColors.Control;
    public virtual Color CollapsibleGroupBoxHeaderBackGradientEnd => System.Drawing.SystemColors.ControlLight;
    public virtual Color CollapsibleGroupBoxHeaderText => Control.DefaultForeColor;
    #endregion

    // DeviceEditControl
    public virtual Color DeviceEditControlBack => Color.Transparent;

    // ControlStyleColorTable
    //public virtual Color ControlStyleColorTableBorder => Color.Black;

    // MainForm
    //public virtual Color MainFormToolStripBack => Color.Transparent;

    // MainView
    //public virtual Color MainViewBack => Color.Transparent;
    //public virtual Color MainViewToolStripBack => Color.Transparent;

    public virtual Color MatcherGroupEditor => System.Drawing.SystemColors.Control;

    // SimpleScrollbarPanel
    //public virtual Color ScrollbarPanelBorder => Color.LightGray;

    // SmallComicPreview
    public virtual Color SmallComicPreviewPageViewerBack => System.Drawing.SystemColors.Window;
    public virtual Color SmallComicPreviewPageViewerText => Color.LightGray;

    // ThumbTileRenderer
    public virtual Color ThumbTileRendererEmboss => Color.White;
    public virtual Color ThumbTileRendererTitleText => Color.Black;
    public virtual Color ThumbTileRendererBodyText => Color.Gray;

    // ThumbRenderer
    public virtual Color ThumbRendererSelectionBack => System.Drawing.SystemColors.Highlight;

    // ThumbnailViewItem
    public virtual Color ThumbnailViewItemBack => Color.LightGray; // CoverViewItem, FavoriteViewitem, FolderViewItem
    public virtual Color ThumbnailViewItemHighlightText => System.Drawing.SystemColors.HighlightText; // FavoriteViewitem, FolderViewItem
    public virtual Color ThumbnailViewItemBorder => Color.FromArgb(48, System.Drawing.SystemColors.ControlDark);

    // PreferencesDialog
    public virtual Color PreferencesPanelReaderOverlay => Color.WhiteSmoke;
    public virtual Color PreferencesLabelOverlays => Color.Gainsboro;
    public virtual Color PreferencesServerEditControl => Color.Transparent;

    // StyledRenderer
    public virtual Color StyledSelectionBack => Color.Gray;
    public virtual Color StyledSelectionFocusedBack => System.Drawing.SystemColors.Highlight;

    // ItemView
    public virtual Color ItemViewMainBack => System.Drawing.SystemColors.Window;
    public virtual Color ItemViewDefaultBack => System.Drawing.SystemColors.Window;
    public virtual Color ItemViewGroupText => Color.DarkBlue;
    public virtual Color ItemViewGroupSeparator => System.Drawing.SystemColors.ControlDark;

    // ItemDrawInformation
    public virtual Color ItemDrawInfoText => Color.Black;

    // NiceTreeSkin
    public virtual Color NiceTreeSkinDragSeparator => Color.Black;

    #region LibraryTreeSkin
    public virtual Color LibraryTreeTotalBookCountBack => Color.Green;
    public virtual Color LibraryTreeTotalBookCountText => Color.White;
    public virtual Color LibraryTreeUnreadBookCountBack => Color.Orange;
    public virtual Color LibraryTreeUnreadBookCountText => Color.White;
    public virtual Color LibraryTreeNewBookCountBack => Color.Red;
    public virtual Color LibraryTreeNewBookCountText => Color.White;
    #endregion

    #region SmartQuery
    public virtual Color SmartQueryBack => System.Drawing.SystemColors.Window;
    public virtual Color SmartQueryException => Color.LightGray;
    public virtual Color SmartQueryText => System.Drawing.SystemColors.WindowText;
    public virtual Color SmartQueryKeyword => Color.Green;
    public virtual Color SmartQueryQualifier => Color.Blue;
    public virtual Color SmartQueryNegation => Color.DarkRed;
    public virtual Color SmartQueryString => Color.Red;
    #endregion

    // TabBar
    public virtual Color TabBarBack => Color.Transparent; // TabBar.BackColor
    public virtual Color TabBarDefaultBorder => System.Drawing.SystemColors.AppWorkspace; // replaced when RenderWithVisualStyles enabled 
    public virtual Color TabBarSelectedBack => System.Drawing.SystemColors.ControlLightLight;

    // ToolTip
    //public virtual Color ToolTipBack => Color.Empty;
    public virtual Color ToolTipText => System.Drawing.SystemColors.InfoText;

    // TreeView
    //public virtual Color TreeViewBack => Color.Empty;
    //public virtual Color TreeViewText => Color.Empty;

    // ComboBox
    public virtual Color ComboBoxSeparator => System.Drawing.SystemColors.ControlLight;

    // Caption
    public virtual Color CaptionBack => Color.White;
    public virtual Color CaptionText => System.Drawing.SystemColors.ActiveCaptionText;

    // Header
    public virtual Color HeaderBack => Color.White;
    public virtual Color HeaderSeparator => System.Drawing.SystemColors.ControlDark;
    //public virtual Color HeaderText => Color.Empty; // only used in ThemeExtension.ListView_DrawColumnHeader, which is only used in dark mode

    // Control defaults
    //public virtual Color ListBoxBack => Color.Empty; // BackColor - ListBox.BackColor - System.Drawing.SystemColors.Window

    // RatingControl
    //public virtual Color RatingControlBack => Color.Empty; // BackColor - Control.DefaultBackColor - System.Drawing.SystemColors.Control
    //public virtual Color RatingControlRated => Color.Empty; // ForeColor - Control.DefaultForeColor - System.Drawing.SystemColors.ControlText
    public virtual Color RatingControlUnrated => Color.LightGray;

    // SplitButton
    public virtual Color SplitButtonSeparatorLeft => System.Drawing.SystemColors.ButtonShadow;
    public virtual Color SplitButtonSeparatorRight => System.Drawing.SystemColors.ButtonFace;

    // Stacks
    public virtual Color StackFill => System.Drawing.SystemColors.Control;
    public virtual Color StackBorder => Color.Black;
}
