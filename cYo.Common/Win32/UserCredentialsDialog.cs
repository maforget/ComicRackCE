using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace cYo.Common.Win32
{
	[ToolboxItem(true)]
	[DesignerCategory("Dialogs")]
	public class UserCredentialsDialog : CommonDialog
	{
		[SuppressUnmanagedCodeSecurity]
		private sealed class Win32Native
		{
			internal enum CredUIReturnCodes
			{
				NO_ERROR = 0,
				ERROR_CANCELLED = 1223,
				ERROR_NO_SUCH_LOGON_SESSION = 1312,
				ERROR_NOT_FOUND = 1168,
				ERROR_INVALID_ACCOUNT_NAME = 1315,
				ERROR_INSUFFICIENT_BUFFER = 122,
				ERROR_INVALID_PARAMETER = 87,
				ERROR_INVALID_FLAGS = 1004
			}

			internal struct CredUIInfo
			{
				internal int cbSize;

				internal IntPtr hwndParent;

				[MarshalAs(UnmanagedType.LPWStr)]
				internal string pszMessageText;

				[MarshalAs(UnmanagedType.LPWStr)]
				internal string pszCaptionText;

				internal IntPtr hbmBanner;

				internal CredUIInfo(IntPtr owner, string caption, string message, Image banner)
				{
					cbSize = Marshal.SizeOf(typeof(CredUIInfo));
					hwndParent = owner;
					pszCaptionText = caption;
					pszMessageText = message;
					if (banner != null)
					{
						hbmBanner = new Bitmap(banner, 320, 60).GetHbitmap();
					}
					else
					{
						hbmBanner = IntPtr.Zero;
					}
				}
			}

			internal const int CREDUI_MAX_MESSAGE_LENGTH = 100;

			internal const int CREDUI_MAX_CAPTION_LENGTH = 100;

			internal const int CREDUI_MAX_GENERIC_TARGET_LENGTH = 100;

			internal const int CREDUI_MAX_DOMAIN_TARGET_LENGTH = 100;

			internal const int CREDUI_MAX_USERNAME_LENGTH = 100;

			internal const int CREDUI_MAX_PASSWORD_LENGTH = 100;

			internal const int CREDUI_BANNER_HEIGHT = 60;

			internal const int CREDUI_BANNER_WIDTH = 320;

			[DllImport("gdi32.dll")]
			internal static extern bool DeleteObject(IntPtr hObject);

			[DllImport("credui.dll", CharSet = CharSet.Unicode, EntryPoint = "CredUIPromptForCredentialsW", SetLastError = true)]
			internal static extern CredUIReturnCodes CredUIPromptForCredentials(ref CredUIInfo creditUR, string targetName, IntPtr reserved1, int iError, StringBuilder userName, int maxUserName, StringBuilder password, int maxPassword, ref bool iSave, UserCredentialsDialogFlags flags);

			[DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern CredUIReturnCodes CredUIParseUserNameW(string userName, StringBuilder user, int userMaxChars, StringBuilder domain, int domainMaxChars);

			[DllImport("credui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern CredUIReturnCodes CredUIConfirmCredentialsW(string targetName, bool confirm);
		}

		private string user;

		private SecureString password;

		private string domain;

		private string target;

		private string message;

		private string caption;

		private Image banner;

		private bool saveChecked;

		private UserCredentialsDialogFlags flags;

		public string User
		{
			get
			{
				return user;
			}
			set
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentException($"The user name has a maximum length of {100} characters.", "User");
				}
				user = value;
			}
		}

		public SecureString Password
		{
			get
			{
				return password;
			}
			set
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentException($"The password has a maximum length of {100} characters.", "Password");
				}
				password = value;
			}
		}

		public string Domain
		{
			get
			{
				return domain;
			}
			set
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentException($"The domain name has a maximum length of {100} characters.", "Domain");
				}
				domain = value;
			}
		}

		public string Target
		{
			get
			{
				return target;
			}
			set
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentException($"The target has a maximum length of {100} characters.", "Target");
				}
				target = value;
			}
		}

		public string Message
		{
			get
			{
				return message;
			}
			set
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentException($"The message has a maximum length of {100} characters.", "Message");
				}
				message = value;
			}
		}

		public string Caption
		{
			get
			{
				return caption;
			}
			set
			{
				if (value != null && value.Length > 100)
				{
					throw new ArgumentException($"The caption has a maximum length of {100} characters.", "Caption");
				}
				caption = value;
			}
		}

		public Image Banner
		{
			get
			{
				return banner;
			}
			set
			{
				if (value != null)
				{
					if (value.Width != 320)
					{
						throw new ArgumentException($"The banner image width must be {320} pixels.", "Banner");
					}
					if (value.Height != 60)
					{
						throw new ArgumentException($"The banner image height must be {60} pixels.", "Banner");
					}
				}
				banner = value;
			}
		}

		public bool SaveChecked
		{
			get
			{
				return saveChecked;
			}
			set
			{
				saveChecked = value;
			}
		}

		public UserCredentialsDialogFlags Flags
		{
			get
			{
				return flags;
			}
			set
			{
				flags = value;
			}
		}

		public UserCredentialsDialog(string target = null, string caption = null, string message = null, Image banner = null)
		{
			Reset();
			Target = target;
			Caption = caption;
			Message = message;
			Banner = banner;
		}

		public void ConfirmCredentials(bool confirm)
		{
			new UIPermission(UIPermissionWindow.SafeSubWindows).Demand();
			Win32Native.CredUIReturnCodes credUIReturnCodes = Win32Native.CredUIConfirmCredentialsW(target, confirm);
			if (credUIReturnCodes != 0 && credUIReturnCodes != Win32Native.CredUIReturnCodes.ERROR_NOT_FOUND && credUIReturnCodes != Win32Native.CredUIReturnCodes.ERROR_INVALID_PARAMETER)
			{
				throw new InvalidOperationException(TranslateReturnCode(credUIReturnCodes));
			}
		}

		public string PasswordToString()
		{
			IntPtr intPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
			try
			{
				return Marshal.PtrToStringUni(intPtr);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
			}
		}

		protected override bool RunDialog(IntPtr hwndOwner)
		{
			if (Environment.OSVersion.Version.Major < 5)
			{
				throw new PlatformNotSupportedException("The Credential Management API requires Windows XP / Windows Server 2003 or later.");
			}
			Win32Native.CredUIInfo creditUR = new Win32Native.CredUIInfo(hwndOwner, caption, message, banner);
			StringBuilder stringBuilder = new StringBuilder(100);
			StringBuilder stringBuilder2 = new StringBuilder(100);
			if (!string.IsNullOrEmpty(User))
			{
				if (!string.IsNullOrEmpty(Domain))
				{
					stringBuilder.Append(Domain + "\\");
				}
				stringBuilder.Append(User);
			}
			if (Password != null)
			{
				stringBuilder2.Append(PasswordToString());
			}
			try
			{
				Win32Native.CredUIReturnCodes credUIReturnCodes = Win32Native.CredUIPromptForCredentials(ref creditUR, target, IntPtr.Zero, 0, stringBuilder, 100, stringBuilder2, 100, ref saveChecked, flags);
				switch (credUIReturnCodes)
				{
				case Win32Native.CredUIReturnCodes.NO_ERROR:
					LoadUserDomainValues(stringBuilder);
					LoadPasswordValue(stringBuilder2);
					return true;
				case Win32Native.CredUIReturnCodes.ERROR_CANCELLED:
					User = null;
					Password = null;
					return false;
				default:
					throw new InvalidOperationException(TranslateReturnCode(credUIReturnCodes));
				}
			}
			finally
			{
				stringBuilder.Remove(0, stringBuilder.Length);
				stringBuilder2.Remove(0, stringBuilder2.Length);
				if (banner != null)
				{
					Win32Native.DeleteObject(creditUR.hbmBanner);
				}
			}
		}

		public override void Reset()
		{
			target = Application.ProductName ?? AppDomain.CurrentDomain.FriendlyName;
			user = null;
			password = null;
			domain = null;
			caption = null;
			message = null;
			banner = null;
			saveChecked = false;
			flags = UserCredentialsDialogFlags.Default;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (password != null)
			{
				password.Dispose();
				password = null;
			}
		}

		private static string TranslateReturnCode(Win32Native.CredUIReturnCodes result)
		{
			return $"Invalid operation: {result.ToString()}";
		}

		private void LoadPasswordValue(StringBuilder password)
		{
			char[] array = new char[password.Length];
			SecureString secureString = new SecureString();
			try
			{
				password.CopyTo(0, array, 0, array.Length);
				char[] array2 = array;
				foreach (char c in array2)
				{
					secureString.AppendChar(c);
				}
				secureString.MakeReadOnly();
				Password = secureString.Copy();
			}
			finally
			{
				Array.Clear(array, 0, array.Length);
			}
		}

		private void LoadUserDomainValues(StringBuilder principalName)
		{
			StringBuilder stringBuilder = new StringBuilder(100);
			StringBuilder stringBuilder2 = new StringBuilder(100);
			if (Win32Native.CredUIParseUserNameW(principalName.ToString(), stringBuilder, 100, stringBuilder2, 100) == Win32Native.CredUIReturnCodes.NO_ERROR)
			{
				User = stringBuilder.ToString();
				Domain = stringBuilder2.ToString();
			}
			else
			{
				User = principalName.ToString();
				Domain = Environment.MachineName;
			}
		}
	}
}
