using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using cYo.Common.Win32;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Viewer.Controls;
namespace cYo.Projects.ComicRack.Viewer.Views
{
	public partial class SubView : CaptionControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IdleProcess.Idle -= Application_Idle;
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
            // SubView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "SubView";
            this.ResumeLayout(false);

		}
	}
}

