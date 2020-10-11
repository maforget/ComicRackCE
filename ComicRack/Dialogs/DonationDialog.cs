using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using cYo.Common.Cryptography;
using cYo.Common.Localize;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Viewer.Properties;

namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public partial class DonationDialog : Form
	{
		public static TR Texts = TR.Load("DonationDialog");

		private static readonly Image validated = Resources.Validated;

		private static readonly Image notValidated = Resources.NotValidated;

		public DonationDialog()
		{
			InitializeComponent();
			LocalizeUtility.Localize(Texts, this);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			UpdateActivation();
			UpdateValidationButton();
		}

		public static string GetDateHash()
		{
			return Password.CreateHash(DateTime.Now.ToShortDateString());
		}

		public static string GetTriggerText()
		{
			string text = string.Empty;
			if (Program.Settings.RunCount > 50)
			{
				text += string.Format(Texts["TriggerRun", "You have started ComicRack {0} times!"] + "\n", Program.Settings.RunCount);
			}
			if (Program.Database.Books.Count > 100)
			{
				text += string.Format(Texts["TriggerBooks", "You are managing {0} books with ComicRack!"] + "\n", Program.Database.Books.Count);
			}
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			return text;
		}

		public static bool IsTriggered()
		{
			if (!string.IsNullOrEmpty(GetTriggerText()))
			{
				if (!(Program.Settings.DonationShown != GetDateHash()))
				{
					return Program.Settings.RunCount > 200;
				}
				return true;
			}
			return false;
		}

		public static void Validate(string email)
		{
			try
			{
				using (new WaitCursor())
				{
					Program.ValidateActivation(email);
				}
			}
			catch (Exception)
			{
				MessageBox.Show(Texts["ServerError", "Could not contact the ComicRack server to validate the Email address. Please check your internet connection!"], Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		public static void Show(IWin32Window parent, bool alwaysShow)
		{
			if (alwaysShow || (IsTriggered() && !Program.Settings.IsActivated))
			{
				using (DonationDialog donationDialog = new DonationDialog())
				{
					Program.Settings.DonationShown = GetDateHash();
					donationDialog.labelDonationText.Text = GetTriggerText() + donationDialog.labelDonationText.Text;
					donationDialog.textEmail.Text = Program.Settings.UserEmail;
					donationDialog.ShowDialog(parent);
				}
			}
		}


		private void UpdateActivation()
		{
			bool isActivated = Program.Settings.IsActivated;
			validationIcon.Image = (isActivated ? validated : notValidated);
			btOK.Enabled = isActivated;
			btCancel.Visible = !isActivated;
			labelThankYou.Visible = isActivated;
		}

		private void UpdateValidationButton()
		{
			btValidate.Enabled = !string.IsNullOrEmpty(textEmail.Text.Trim());
		}

		private void textEmail_TextChanged(object sender, EventArgs e)
		{
			UpdateValidationButton();
		}

		private void btDonate_Click(object sender, EventArgs e)
		{
			Program.ShowPayPal();
		}

		private void btValidate_Click(object sender, EventArgs e)
		{
			btValidate.Enabled = false;
			Validate(textEmail.Text.Trim());
			UpdateActivation();
			timerValidation.Start();
		}

		private void timerValidation_Tick(object sender, EventArgs e)
		{
			timerValidation.Stop();
			UpdateValidationButton();
		}
	}
}
