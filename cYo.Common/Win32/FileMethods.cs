using System.Runtime.InteropServices;
using System.Text;

namespace cYo.Common.Win32
{
	public static class FileMethods
	{
		private static class NativeMethods
		{
			[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
			public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string path, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath, int shortPathLength);
		}

		public static string GetShortName(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(255);
			if (NativeMethods.GetShortPathName(path, stringBuilder, stringBuilder.Capacity) == 0)
			{
				return path;
			}
			return stringBuilder.ToString();
		}
	}
}
