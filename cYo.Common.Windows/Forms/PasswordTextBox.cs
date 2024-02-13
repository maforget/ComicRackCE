using System;
using System.Windows.Forms;
using cYo.Common.Cryptography;

namespace cYo.Common.Windows.Forms
{
	public class PasswordTextBox : TextBox
	{
		private const string dummyPassword = "1234";

		private string password;

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				if (!(value == password))
				{
					Text = (string.IsNullOrEmpty(value) ? string.Empty : dummyPassword);
					password = value;
				}
			}
		}

		public PasswordTextBox()
		{
			base.UseSystemPasswordChar = true;
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			password = cYo.Common.Cryptography.Password.CreateHash(Text.Trim());
		}
	}
}
