using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Engine.Display.Forms;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
    public partial class SmallComicPreview : CaptionControl, IRefreshDisplay
    {
        private readonly CommandMapper commands = new CommandMapper();

        private readonly string noneSelectedText;

        private readonly string previewOnlyForComics;

        private ComicDisplay comicDisplay;

        public bool TwoPageDisplay
        {
            get
            {
                return pageViewer.PageLayout != PageLayoutMode.Single;
            }
            set
            {
                pageViewer.PageLayout = (value ? PageLayoutMode.DoubleAdaptive : PageLayoutMode.Single);
            }
        }

        public event EventHandler CloseClicked;

        public SmallComicPreview()
        {
            InitializeComponent();
            if (components == null)
            {
                components = new Container();
            }
            components.Add(commands);
            LocalizeUtility.Localize(this, components);
            noneSelectedText = TR.Load(base.Name)[pageViewer.Name, pageViewer.Text];
            previewOnlyForComics = TR.Load(base.Name)["PreviewOnlyForComics", "Preview is only available for Books"];
            if (ThemeExtensions.IsDarkModeEnabled)
                pageViewer.BackColor = ThemeColors.Material.SidePanel;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!base.DesignMode)
            {
                comicDisplay = new ComicDisplay(pageViewer);
                comicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveUp", "", "Up", comicDisplay.ScrollUp, CommandKey.Up, CommandKey.MouseWheelUp));
                comicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("MoveDown", "", "Down", comicDisplay.ScrollDown, CommandKey.Down, CommandKey.MouseWheelDown));
                comicDisplay.KeyboardMap.Commands.Add(new KeyboardCommand("OpenBook", "", "Open", OpenBook, CommandKey.MouseDoubleLeft));
                commands.Add(comicDisplay.DisplayFirstPage, () => comicDisplay.Book != null && comicDisplay.Book.CanNavigate(-1), tsbFirst);
                commands.Add(delegate
                {
                    comicDisplay.DisplayPreviousPage(ComicDisplay.PagingMode.Double);
                }, () => comicDisplay.Book != null && comicDisplay.Book.CanNavigate(-1), tsbPrev);
                commands.Add(delegate
                {
                    comicDisplay.DisplayNextPage(ComicDisplay.PagingMode.Double);
                }, () => comicDisplay.Book != null && comicDisplay.Book.CanNavigate(1), tsbNext);
                commands.Add(comicDisplay.DisplayLastPage, () => comicDisplay.Book != null && comicDisplay.Book.CanNavigate(1), tsbLast);
                commands.Add(delegate
                {
                    TwoPageDisplay = !TwoPageDisplay;
                }, () => comicDisplay.Book != null, () => TwoPageDisplay, tsbTwoPages);
                commands.Add(RefreshDisplay, () => comicDisplay.Book != null, tsbRefresh);
                commands.Add(OnCloseClicked, tbClose);
                commands.Add(OpenBook, () => comicDisplay.Book != null, tsbOpen);
                pageViewer.ContextMenuStrip = null;
                pageViewer.PagePool = Program.ImagePool;
                Program.Settings.PageImageDisplayOptionsChanged += Settings_DisplayOptionsChanged;
                UpdateSettings();
            }
        }

        private void Settings_DisplayOptionsChanged(object sender, EventArgs e)
        {
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            pageViewer.ImageBackgroundMode = ImageBackgroundMode.Color;
        }

        private void OpenBook()
        {
            IMain main = this.FindParentService<IMain>();
            if (main != null && pageViewer.Book != null && pageViewer.Book.Comic != null)
            {
                main.OpenBooks.Open(pageViewer.Book.Comic.FilePath, OpenComicOptions.OpenInNewSlot);
            }
        }

        protected virtual void OnCloseClicked()
        {
            if (this.CloseClicked != null)
            {
                this.CloseClicked(this, EventArgs.Empty);
            }
        }

        public void ShowPreview(ComicBook comicBook)
        {
            if (comicBook == null)
            {
                if (pageViewer.Book != null)
                {
                    pageViewer.Book.Dispose();
                }
                pageViewer.Book = null;
            }
            else if (pageViewer.Book == null || comicBook.FilePath != pageViewer.Book.Comic.FilePath)
            {
                if (pageViewer.Book != null)
                {
                    pageViewer.Book.Dispose();
                    pageViewer.Book = null;
                }
                pageViewer.Book = NavigatorManager.OpenComic(comicBook, 0, OpenComicOptions.DisableAll);
            }
            if (pageViewer.Book != null)
            {
                pageViewer.Text = string.Empty;
            }
            else
            {
                pageViewer.Text = ((comicBook == null || comicBook.IsLinked) ? noneSelectedText : previewOnlyForComics);
            }
        }

        public void RefreshDisplay()
        {
            comicDisplay.RefreshDisplay();
        }
    }
}
