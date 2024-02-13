using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public static class TextBoxExtensions
	{
		private static class Native
		{
			public const int ECM_FIRST = 5376;

			public const int EM_SETCUEBANNER = 5377;

			public const int EM_GETCUEBANNER = 5378;

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, string lParam);
		}

		public const string OnlyNumberKeys = "0123456789.,";

		public static void SetCueText(this TextBox tb, string text)
		{
			Native.SendMessage(tb.Handle, Native.EM_SETCUEBANNER, IntPtr.Zero, text);
		}

		public static void EnableKeys(this TextBoxBase tb, IEnumerable<char> enabledKeys)
		{
			tb.KeyPress += delegate(object s, KeyPressEventArgs e)
			{
				e.Handled = !enabledKeys.Contains(e.KeyChar) && !"\b".Contains(e.KeyChar);
			};
		}

		public static void EnableOnlyNumberKeys(this TextBoxBase tb)
		{
			tb.EnableKeys("0123456789.,");
		}
	}
}
