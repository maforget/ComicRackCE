using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace cYo.Common.Win32
{
	public static class ShellIcon
	{
		private static class Unsafe
		{
			[Flags]
			public enum EnumFileInfoFlags : uint
			{
				LARGEICON = 0x0u,
				SMALLICON = 0x1u,
				OPENICON = 0x2u,
				SHELLICONSIZE = 0x4u,
				PIDL = 0x8u,
				USEFILEATTRIBUTES = 0x10u,
				ADDOVERLAYS = 0x20u,
				OVERLAYINDEX = 0x40u,
				ICON = 0x100u,
				DISPLAYNAME = 0x200u,
				TYPENAME = 0x400u,
				ATTRIBUTES = 0x800u,
				ICONLOCATION = 0x1000u,
				EXETYPE = 0x2000u,
				SYSICONINDEX = 0x4000u,
				LINKOVERLAY = 0x8000u,
				SELECTED = 0x10000u,
				ATTR_SPECIFIED = 0x20000u
			}

			public struct ShellFileInfo
			{
				public const int conNameSize = 80;

				public IntPtr hIcon;

				public int iIndex;

				public uint dwAttributes;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
				public string szDisplayName;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = conNameSize)]
				public string szTypeName;
			}

			public const int MAX_PATH = 260;

			public const uint FILE_ATTRIBUTE_DIRECTORY = 16u;

			public const uint FILE_ATTRIBUTE_NORMAL = 128u;

			[DllImport("User32.dll")]
			public static extern int DestroyIcon(IntPtr hIcon);

			[DllImport("Shell32.dll")]
			public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref ShellFileInfo psfi, uint cbFileInfo, uint uFlags);
		}

		public static Icon GetFolderIcon(string path, ShellIconType iconType)
		{
			Unsafe.EnumFileInfoFlags enumFileInfoFlags = Unsafe.EnumFileInfoFlags.USEFILEATTRIBUTES | Unsafe.EnumFileInfoFlags.ICON;
			if ((iconType & ShellIconType.Link) != 0)
			{
				enumFileInfoFlags |= Unsafe.EnumFileInfoFlags.LINKOVERLAY;
			}
			if ((iconType & ShellIconType.Open) != 0)
			{
				enumFileInfoFlags |= Unsafe.EnumFileInfoFlags.OPENICON;
			}
			enumFileInfoFlags = (((iconType & ShellIconType.Large) != 0) ? (enumFileInfoFlags | Unsafe.EnumFileInfoFlags.LARGEICON) : (enumFileInfoFlags | Unsafe.EnumFileInfoFlags.SMALLICON));
			Unsafe.ShellFileInfo psfi = default(Unsafe.ShellFileInfo);
			uint dwFileAttributes = (((iconType & ShellIconType.Directory) != 0) ? Unsafe.FILE_ATTRIBUTE_DIRECTORY : Unsafe.FILE_ATTRIBUTE_NORMAL);
			Unsafe.SHGetFileInfo(path, dwFileAttributes, ref psfi, (uint)Marshal.SizeOf((object)psfi), (uint)enumFileInfoFlags);
			Icon result = Icon.FromHandle(psfi.hIcon).Clone() as Icon;
			Unsafe.DestroyIcon(psfi.hIcon);
			return result;
		}
	}
}
