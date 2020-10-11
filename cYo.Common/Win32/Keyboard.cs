using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace cYo.Common.Win32
{
	public static class Keyboard
	{
		private static class Native
		{
			[DllImport("user32.dll")]
			public static extern int GetKeyNameText(uint scanCode, [Out] StringBuilder lpString, int nSize);

			[DllImport("user32.dll")]
			public static extern uint MapVirtualKey(uint uCode, uint uMapType);
		}

		public static string GetLocalizedKeyName(Keys key)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			uint uCode = (uint)key;
			uint scanCode = Native.MapVirtualKey(uCode, 0u) << 16;
			Native.GetKeyNameText(scanCode, stringBuilder, stringBuilder.Capacity);
			if (stringBuilder.Length != 0)
			{
				return stringBuilder.ToString();
			}
			return key.ToString();
		}
	}
}
