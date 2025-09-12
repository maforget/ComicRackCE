using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Win32
{
	public static class TrueVisibility
	{
		private static class Native
		{
			public const uint WS_VISIBLE = 268435456u;

			public const int GWL_STYLE = -16;

			[DllImport("user32.dll")]
			public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		}

		public static bool IsVisibleSet(this Control c)
		{
			if (c == null || c.IsDisposed)
			{
				return false;
			}
			return ((ulong)Native.GetWindowLong(c.Handle, Native.GWL_STYLE) & 0x10000000uL) != 0;
		}
	}
}
