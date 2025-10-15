using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
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
        public virtual Color CollapsibleGroupBoxBack => Color.Transparent;
        public virtual Color CollapsibleGroupBoxHeaderBackGradientStart => SystemColors.Control;
        public virtual Color CollapsibleGroupBoxHeaderBackGradientEnd => SystemColors.ControlLight;
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

        public virtual Color MatcherGroupEditor => SystemColors.Control;

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

        // NiceTreeSkin
        public virtual Color NiceTreeSkinDragSeparator => Color.Black;

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

        // TreeView
        public virtual Color TreeViewBack => Color.Empty;
        public virtual Color TreeViewText => Color.Empty;

        // ComboBox
        public virtual Color ComboBoxSeparator => SystemColors.ControlLight;

        // Caption
        public virtual Color CaptionBack => Color.White;
        public virtual Color CaptionText => SystemColors.ActiveCaptionText;

        // Header
        public virtual Color HeaderBack => Color.White;
        public virtual Color HeaderSeparator => SystemColors.ControlDark;
        public virtual Color HeaderText => Color.Empty; // only used in ThemeExtension.ListView_DrawColumnHeader, which is only used in dark mode

        // Control defaults
        public virtual Color ListBoxBack => Color.Empty; // BackColor - ListBox.BackColor - SystemColors.Window

        // RatingControl
        public virtual Color RatingControlBack => Color.Empty; // BackColor - Control.DefaultBackColor - SystemColors.Control
        public virtual Color RatingControlRated => Color.Empty; // ForeColor - Control.DefaultForeColor - SystemColors.ControlText
        public virtual Color RatingControlUnrated => Color.LightGray;

        // SplitButton
        public virtual Color SplitButtonSeparatorLeft => SystemColors.ButtonShadow;
        public virtual Color SplitButtonSeparatorRight => SystemColors.ButtonFace;

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
}
