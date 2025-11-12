using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Win32
{
	public static class TrueVisibility
	{
		private static class Native
		{
			public const ulong WS_VISIBLE = 0x10000000;

            public const int GWL_STYLE = -16;

			[DllImport("user32.dll")]
			public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		}

		public static bool IsVisibleSet(this Control c)
		{
			//BUG: Without !c.IsHandleCreated an exception might be thrown when an handle isn't created, but adding it changes the behavior of this function 
			if (c == null || c.IsDisposed) // || !c.IsHandleCreated)
			{
				return false;
			}
			return ((ulong)Native.GetWindowLong(c.Handle, Native.GWL_STYLE) & Native.WS_VISIBLE) != 0;
		}
	}
}
