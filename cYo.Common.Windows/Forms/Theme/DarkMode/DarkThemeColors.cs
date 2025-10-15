using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Windows.Forms
{
    /// <summary>
    /// Dark Mode aplication <see cref="Color"/> values.<br/>
    /// </summary>
    /// <remarks>
    /// Does not include <see cref="System.Drawing.SystemColors"/> or <see cref="System.Windows.Forms.Control"/> colors.
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
        public override Color CollapsibleGroupBoxBack => ThemeColors.Material.Content;
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

        public override Color MatcherGroupEditor => ThemeColors.Material.Window;

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
        public override Color ThumbnailViewItemBack => Color.FromArgb(72, 72, 72); // CoverViewItem, FavoriteViewitem, FolderViewItem
        //public override Color ThumbnailViewItemHighlightText => SystemColors.HighlightText; // FavoriteViewitem, FolderViewItem
        public override Color ThumbnailViewItemBorder => Color.FromArgb(64, 190, 190, 190);

        // PreferencesDialog
        public override Color PreferencesPanelReaderOverlay => ThemeColors.BlackSmoke;
        public override Color PreferencesLabelOverlays => ThemeColors.Lossboro;
        public override Color PreferencesServerEditControl => SystemColors.Control;

        // ItemView
        public override Color ItemViewMainBack => ThemeColors.Material.Content;
        public override Color ItemViewDefaultBack => SystemColors.Window;
        public override Color ItemViewGroupText => Color.LightSkyBlue;
        public override Color ItemViewGroupSeparator => Color.FromArgb(190, 190, 190);

        // NiceTreeSkin
        public override Color NiceTreeSkinDragSeparator => Color.FromArgb(190, 190, 190);

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

        // TreeView
        public override Color TreeViewBack => ThemeColors.TextBox.Back;
        public override Color TreeViewText => SystemColors.ControlText;

        // ComboBox
        public override Color ComboBoxSeparator => SystemColors.ControlText;

        // Caption
        public override Color CaptionBack => Color.Black;
        //public override Color CaptionText => SystemColors.ActiveCaptionText;

        // Header
        public override Color HeaderBack => SystemColors.Control;
        public override Color HeaderSeparator => Color.FromArgb(99, 99, 99);
        public override Color HeaderText => SystemColors.WindowText;

        // Control defaults
        public override Color ListBoxBack => Color.FromArgb(46, 46, 46); // to match ListView BackColor

        // RatingControl
        public override Color RatingControlBack => ListBoxBack;
        public override Color RatingControlRated => SystemColors.ControlText;
        public override Color RatingControlUnrated => SystemColors.GrayText;

        public override Color SplitButtonSeparatorLeft => SystemColors.WindowText;
        public override Color SplitButtonSeparatorRight => SystemColors.ButtonFace;

        // CheckBox
        public override Color CheckBoxBack => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBackCorner => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBackVertex => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBorder => Color.FromArgb(0, 95, 184);
        public override Color CheckBoxBorderEdge => Color.FromArgb(4, 87, 166);
        public override Color CheckBoxBorderCorner => Color.FromArgb(28, 54, 74);
        public override Color CheckBoxUncheckedBorder => Color.FromArgb(98, 98, 98);
        public override Color CheckBoxUncheckedBorderEdge => Color.FromArgb(90, 90, 90);
        public override Color CheckBoxUncheckedBack => SystemColors.ControlLight;
        public override Color CheckBoxUncheckedBackCorner => Color.FromArgb(48, 48, 48);
        public override Color CheckBoxUncheckedBackVertex => SystemColors.ControlDark;
        public override Color CheckBoxUncheckedDisabledBorder => SystemColors.ControlDark;
        public override Color CheckBoxUncheckedDisabledBorderEdge => Color.FromArgb(60, 60, 60);
        public override Color CheckBoxUncheckedDisabledBackCorner => Color.FromArgb(100, 100, 100);
        public override Color CheckBoxUncheckedDisabledBackVertex => SystemColors.ControlDarkDark;
    }
}
