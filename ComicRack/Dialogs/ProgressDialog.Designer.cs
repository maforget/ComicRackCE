using System.ComponentModel;
using System.Windows.Forms;


namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
    public partial class ProgressDialog
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			progressBar = new System.Windows.Forms.ProgressBar();
			btCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			progressBar.Location = new System.Drawing.Point(16, 24);
			progressBar.Name = "progressBar";
			progressBar.Size = new System.Drawing.Size(384, 24);
			progressBar.TabIndex = 0;
			btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btCancel.Location = new System.Drawing.Point(304, 54);
			btCancel.Name = "btCancel";
			btCancel.Size = new System.Drawing.Size(96, 24);
			btCancel.TabIndex = 1;
			btCancel.Text = "Cancel";
			btCancel.Click += new System.EventHandler(btCancel_Click);
			AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			base.CancelButton = btCancel;
			base.ClientSize = new System.Drawing.Size(410, 88);
			base.ControlBox = false;
			base.Controls.Add(btCancel);
			base.Controls.Add(progressBar);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProgressDialog";
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "ProgressDialog";
			ResumeLayout(false);
		}

		private ProgressBar progressBar;

		private Button btCancel;

		private Container components;
	}
}
