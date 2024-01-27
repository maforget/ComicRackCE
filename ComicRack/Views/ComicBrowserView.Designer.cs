using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicBrowserView : SubView
	{
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.comicBrowser = new cYo.Projects.ComicRack.Viewer.Views.ComicBrowserControl();
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
            this.comicBrowser.Size = new System.Drawing.Size(643, 552);
            this.comicBrowser.TabIndex = 0;
            // 
            // ComicBrowserView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.comicBrowser);
            this.Name = "ComicBrowserView";
            this.Size = new System.Drawing.Size(643, 552);
            this.ResumeLayout(false);

		}

        private ComicBrowserControl comicBrowser;
    }
}

