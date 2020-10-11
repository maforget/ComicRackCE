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
	public class SmallComicPreview : CaptionControl, IRefreshDisplay
	{
		private readonly CommandMapper commands = new CommandMapper();

		private readonly string noneSelectedText;

		private readonly string previewOnlyForComics;

		private ComicDisplay comicDisplay;

		private IContainer components;

		private ComicDisplayControl pageViewer;

		private ToolStrip toolStripPreview;

		private ToolStripButton tsbFirst;

		private ToolStripButton tsbPrev;

		private ToolStripButton tsbNext;

		private ToolStripButton tsbLast;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripButton tsbTwoPages;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripButton tsbRefresh;

		private ToolStripButton tbClose;

		private ToolStripButton tsbOpen;

		private ToolStripSeparator toolStripSeparator3;

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
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (!base.DesignMode)
				{
					Program.Settings.PageImageDisplayOptionsChanged -= Settings_DisplayOptionsChanged;
				}
				if (comicDisplay != null)
				{
					comicDisplay.Dispose();
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
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

		private void InitializeComponent()
		{
			pageViewer = new cYo.Projects.ComicRack.Engine.Display.Forms.ComicDisplayControl();
			toolStripPreview = new System.Windows.Forms.ToolStrip();
			tsbOpen = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			tsbFirst = new System.Windows.Forms.ToolStripButton();
			tsbPrev = new System.Windows.Forms.ToolStripButton();
			tsbNext = new System.Windows.Forms.ToolStripButton();
			tsbLast = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			tsbTwoPages = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			tsbRefresh = new System.Windows.Forms.ToolStripButton();
			tbClose = new System.Windows.Forms.ToolStripButton();
			toolStripPreview.SuspendLayout();
			SuspendLayout();
			pageViewer.BackColor = System.Drawing.SystemColors.Window;
			pageViewer.DisableHardwareAcceleration = true;
			pageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			pageViewer.Font = new System.Drawing.Font("Arial", 15.75f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			pageViewer.ForeColor = System.Drawing.Color.LightGray;
			pageViewer.HardwareFiltering = false;
			pageViewer.Location = new System.Drawing.Point(0, 25);
			pageViewer.MagnifierSize = new System.Drawing.Size(400, 300);
			pageViewer.Name = "pageViewer";
			pageViewer.PaperTextureLayout = System.Windows.Forms.ImageLayout.None;
			pageViewer.PreCache = false;
			pageViewer.Size = new System.Drawing.Size(285, 267);
			pageViewer.TabIndex = 2;
			pageViewer.Text = "Nothing Selected";
			toolStripPreview.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			toolStripPreview.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
			{
				tsbOpen,
				toolStripSeparator3,
				tsbFirst,
				tsbPrev,
				tsbNext,
				tsbLast,
				toolStripSeparator2,
				tsbTwoPages,
				toolStripSeparator1,
				tsbRefresh,
				tbClose
			});
			toolStripPreview.Location = new System.Drawing.Point(0, 0);
			toolStripPreview.Name = "toolStripPreview";
			toolStripPreview.Size = new System.Drawing.Size(285, 25);
			toolStripPreview.TabIndex = 3;
			toolStripPreview.Text = "toolStrip1";
			tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbOpen.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
			tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbOpen.Name = "tsbOpen";
			tsbOpen.Size = new System.Drawing.Size(23, 22);
			tsbOpen.Text = "Open";
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			tsbFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbFirst.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
			tsbFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbFirst.Name = "tsbFirst";
			tsbFirst.Size = new System.Drawing.Size(23, 22);
			tsbFirst.Text = "First";
			tsbFirst.ToolTipText = "Go to first page";
			tsbPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbPrev.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
			tsbPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbPrev.Name = "tsbPrev";
			tsbPrev.Size = new System.Drawing.Size(23, 22);
			tsbPrev.Text = "Previous";
			tsbPrev.ToolTipText = "Go to previous page";
			tsbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbNext.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
			tsbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbNext.Name = "tsbNext";
			tsbNext.Size = new System.Drawing.Size(23, 22);
			tsbNext.Text = "Next";
			tsbNext.ToolTipText = "Go to next page";
			tsbLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbLast.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
			tsbLast.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbLast.Name = "tsbLast";
			tsbLast.Size = new System.Drawing.Size(23, 22);
			tsbLast.Text = "Last";
			tsbLast.ToolTipText = "Go to last page";
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			tsbTwoPages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbTwoPages.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
			tsbTwoPages.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbTwoPages.Name = "tsbTwoPages";
			tsbTwoPages.Size = new System.Drawing.Size(23, 22);
			tsbTwoPages.Text = "Two Pages";
			tsbTwoPages.ToolTipText = "Show one or two pages";
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tsbRefresh.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
			tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbRefresh.Name = "tsbRefresh";
			tsbRefresh.Size = new System.Drawing.Size(23, 22);
			tsbRefresh.Text = "Refresh";
			tbClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			tbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			tbClose.Image = cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallClose;
			tbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			tbClose.Name = "tbClose";
			tbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
			tbClose.Size = new System.Drawing.Size(23, 22);
			tbClose.Text = "Close";
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.Controls.Add(pageViewer);
			base.Controls.Add(toolStripPreview);
			base.Name = "SmallComicPreview";
			base.Size = new System.Drawing.Size(285, 292);
			toolStripPreview.ResumeLayout(false);
			toolStripPreview.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
