using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public partial class TwitterPinDialog : Form
	{
		public TwitterPinDialog()
		{
			InitializeComponent();
			LocalizeUtility.Localize(this, null);
		}

		public static string GetPin(IWin32Window parent)
		{
			using (TwitterPinDialog twitterPinDialog = new TwitterPinDialog())
			{
				return (twitterPinDialog.ShowDialog(parent) == DialogResult.OK) ? twitterPinDialog.textPin.Text : null;
			}
		}

	}
}
