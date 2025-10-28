using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows.Forms.Theme.Resources;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Engine.Display.Forms;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Views
{
    public partial class SmallComicPreview : CaptionControl, IRefreshDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        private void InitializeComponent()
        {
            this.pageViewer = new cYo.Projects.ComicRack.Engine.Display.Forms.ComicDisplayControl();
            this.toolStripPreview = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbFirst = new System.Windows.Forms.ToolStripButton();
            this.tsbPrev = new System.Windows.Forms.ToolStripButton();
            this.tsbNext = new System.Windows.Forms.ToolStripButton();
            this.tsbLast = new System.Windows.Forms.ToolStripButton();
            this.tsbTwoPages = new System.Windows.Forms.ToolStripButton();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.tbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageViewer
            // 
            this.pageViewer.BackColor = ThemeColors.SmallComicPreview.PageViewerBack;
            this.pageViewer.DisableHardwareAcceleration = true;
            this.pageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pageViewer.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pageViewer.ForeColor = ThemeColors.SmallComicPreview.PageViewerText;
            this.pageViewer.HardwareFiltering = false;
            this.pageViewer.Location = new System.Drawing.Point(0, 25);
            this.pageViewer.MagnifierSize = new System.Drawing.Size(400, 300);
            this.pageViewer.Name = "pageViewer";
            this.pageViewer.PaperTextureLayout = System.Windows.Forms.ImageLayout.None;
            this.pageViewer.PreCache = false;
            this.pageViewer.Size = new System.Drawing.Size(285, 267);
            this.pageViewer.TabIndex = 2;
            this.pageViewer.Text = "Nothing Selected";
            // 
            // toolStripPreview
            // 
            this.toolStripPreview.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripPreview.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.toolStripSeparator3,
            this.tsbFirst,
            this.tsbPrev,
            this.tsbNext,
            this.tsbLast,
            this.toolStripSeparator2,
            this.tsbTwoPages,
            this.toolStripSeparator1,
            this.tsbRefresh,
            this.tbClose});
            this.toolStripPreview.Location = new System.Drawing.Point(0, 0);
            this.toolStripPreview.Name = "toolStripPreview";
            this.toolStripPreview.Size = new System.Drawing.Size(285, 25);
            this.toolStripPreview.TabIndex = 3;
            this.toolStripPreview.Text = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Open;
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.Text = "Open";
            // 
            // tsbFirst
            // 
            this.tsbFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFirst.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoFirst;
            this.tsbFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFirst.Name = "tsbFirst";
            this.tsbFirst.Size = new System.Drawing.Size(23, 22);
            this.tsbFirst.Text = "First";
            this.tsbFirst.ToolTipText = "Go to first page";
            // 
            // tsbPrev
            // 
            this.tsbPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPrev.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoPrevious;
            this.tsbPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrev.Name = "tsbPrev";
            this.tsbPrev.Size = new System.Drawing.Size(23, 22);
            this.tsbPrev.Text = "Previous";
            this.tsbPrev.ToolTipText = "Go to previous page";
            // 
            // tsbNext
            // 
            this.tsbNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNext.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoNext;
            this.tsbNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNext.Name = "tsbNext";
            this.tsbNext.Size = new System.Drawing.Size(23, 22);
            this.tsbNext.Text = "Next";
            this.tsbNext.ToolTipText = "Go to next page";
            // 
            // tsbLast
            // 
            this.tsbLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbLast.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.GoLast;
            this.tsbLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLast.Name = "tsbLast";
            this.tsbLast.Size = new System.Drawing.Size(23, 22);
            this.tsbLast.Text = "Last";
            this.tsbLast.ToolTipText = "Go to last page";
            // 
            // tsbTwoPages
            // 
            this.tsbTwoPages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTwoPages.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.TwoPage;
            this.tsbTwoPages.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTwoPages.Name = "tsbTwoPages";
            this.tsbTwoPages.Size = new System.Drawing.Size(23, 22);
            this.tsbTwoPages.Text = "Two Pages";
            this.tsbTwoPages.ToolTipText = "Show one or two pages";
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.Refresh;
            this.tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(23, 22);
            this.tsbRefresh.Text = "Refresh";
            // 
            // tbClose
            // 
            this.tbClose.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbClose.Image = global::cYo.Projects.ComicRack.Viewer.Properties.Resources.SmallClose;
            this.tbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbClose.Name = "tbClose";
            this.tbClose.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.tbClose.Size = new System.Drawing.Size(23, 22);
            this.tbClose.Text = "Close";
            // 
            // SmallComicPreview
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pageViewer);
            this.Controls.Add(this.toolStripPreview);
            this.Name = "SmallComicPreview";
            this.Size = new System.Drawing.Size(285, 292);
            this.toolStripPreview.ResumeLayout(false);
            this.toolStripPreview.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
    }
}
