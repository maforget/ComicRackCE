using cYo.Common.Win32;
using cYo.Projects.ComicRack.Engine;
using System.ComponentModel;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Viewer.Views
{
    public partial class ComicBrowserForm
	{
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
			comicBrowser = new cYo.Projects.ComicRack.Viewer.Views.ComicBrowserControl();
			statusStrip = new System.Windows.Forms.StatusStrip();
			tsText = new System.Windows.Forms.ToolStripStatusLabel();
			statusStrip.SuspendLayout();
			SuspendLayout();
			comicBrowser.CaptionMargin = new System.Windows.Forms.Padding(2);
			comicBrowser.DisableViewConfigUpdate = true;
			comicBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			comicBrowser.Location = new System.Drawing.Point(0, 0);
			comicBrowser.Name = "comicBrowser";
			comicBrowser.Size = new System.Drawing.Size(589, 410);
			comicBrowser.TabIndex = 0;
			statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				tsText
			});
			statusStrip.Location = new System.Drawing.Point(0, 410);
			statusStrip.Name = "statusStrip1";
			statusStrip.Size = new System.Drawing.Size(589, 22);
			statusStrip.TabIndex = 1;
			statusStrip.Text = "statusStrip";
			tsText.Name = "tsText";
			tsText.Size = new System.Drawing.Size(574, 17);
			tsText.Spring = true;
			tsText.Text = "Ready";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(589, 432);
			base.Controls.Add(comicBrowser);
			base.Controls.Add(statusStrip);
			base.Name = "ComicBrowserForm";
			Text = "ComicBrowserForm";
			statusStrip.ResumeLayout(false);
			statusStrip.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		private IContainer components;

		private ComicBrowserControl comicBrowser;

		private StatusStrip statusStrip;

		private ToolStripStatusLabel tsText;
	}
}
