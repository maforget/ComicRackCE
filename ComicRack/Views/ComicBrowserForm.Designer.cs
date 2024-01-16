using cYo.Common.Win32;
using cYo.Projects.ComicRack.Engine;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Views
{
    public partial class ComicBrowserForm
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IdleProcess.Idle -= OnIdle;
				IComicBookListProvider bookList = BookList;
				comicBrowser.BookList = null;
				if (BookListOwned)
				{
					bookList.Dispose();
				}
				base.Icon.Dispose();
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.comicBrowser = new cYo.Projects.ComicRack.Viewer.Views.ComicBrowserControl();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.tsText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // comicBrowser
            // 
            this.comicBrowser.Caption = "";
            this.comicBrowser.CaptionMargin = new System.Windows.Forms.Padding(2);
            this.comicBrowser.DisableViewConfigUpdate = true;
            this.comicBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comicBrowser.Location = new System.Drawing.Point(0, 0);
            this.comicBrowser.Name = "comicBrowser";
            this.comicBrowser.Size = new System.Drawing.Size(589, 410);
            this.comicBrowser.TabIndex = 0;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsText});
            this.statusStrip.Location = new System.Drawing.Point(0, 410);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(589, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip";
            // 
            // tsText
            // 
            this.tsText.Name = "tsText";
            this.tsText.Size = new System.Drawing.Size(574, 17);
            this.tsText.Spring = true;
            this.tsText.Text = "Ready";
            // 
            // ComicBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 432);
            this.Controls.Add(this.comicBrowser);
            this.Controls.Add(this.statusStrip);
            this.Name = "ComicBrowserForm";
            this.Text = "ComicBrowserForm";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private ComicBrowserControl comicBrowser;
		private StatusStrip statusStrip;
		private ToolStripStatusLabel tsText;
	}
}
