using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Windows;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class ProgressDialog : Form
	{
		private bool cancel;

		public ProgressDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
		}

		public bool Progress(int percentDone)
		{
			progressBar.Value = percentDone;
			Application.DoEvents();
			return cancel;
		}

		private void btCancel_Click(object sender, EventArgs e)
		{
			cancel = true;
		}
	}
}
