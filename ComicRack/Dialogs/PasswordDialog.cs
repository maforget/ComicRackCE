using System.ComponentModel;
using System.Drawing;
using cYo.Common.Windows.Forms;
using cYo.Common.Windows;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class PasswordDialog : FormEx
	{
		public string Description
		{
			get
			{
				return lblDescription.Text;
			}
			set
			{
				lblDescription.Text = value;
			}
		}

		public string Password => txPassword.Text;

		public bool RememberPassword => chkRemember.Checked;

		public PasswordDialog()
		{
			LocalizeUtility.UpdateRightToLeft(this);
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
		}
	}
}
