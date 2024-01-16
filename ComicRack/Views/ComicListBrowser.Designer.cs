using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class ComicListBrowser : SubView, IRefreshDisplay
	{

		private IContainer components;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				BookList = null;
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.SuspendLayout();
            // 
            // ComicListBrowser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "ComicListBrowser";
            this.Size = new System.Drawing.Size(448, 342);
            this.ResumeLayout(false);

		}
	}
}

