using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace cYo.Common.Win32
{
	public class FileSystemEnumerable : IEnumerable<string>, IEnumerable
	{
		private static class Native
		{
			public struct FILETIME
			{
				public uint dwLowDateTime;

				public uint dwHighDateTime;
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct WIN32_FIND_DATA
			{
				public FileAttributes dwFileAttributes;

				public FILETIME ftCreationTime;

				public FILETIME ftLastAccessTime;

				public FILETIME ftLastWriteTime;

				public int nFileSizeHigh;

				public int nFileSizeLow;

				public int dwReserved0;

				public int dwReserved1;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
				public string cFileName;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
				public string cAlternate;
			}

			public const int MAX_PATH = 260;

			public const int MAX_ALTERNATE = 14;

			public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

			[DllImport("kernel32", CharSet = CharSet.Unicode)]
			public static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

			[DllImport("kernel32", CharSet = CharSet.Unicode)]
			public static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

			[DllImport("kernel32")]
			public static extern bool FindClose(IntPtr hFindFile);
		}

		private readonly string path;

		private readonly string findMask;

		private readonly FileSystemEnumeratorType entryType = FileSystemEnumeratorType.File;

		public FileSystemEnumerable(string path, string findMask, FileSystemEnumeratorType entryType)
		{
			this.entryType = entryType;
			this.path = path;
			this.findMask = findMask;
		}

		public FileSystemEnumerable(string path, string findMask)
			: this(path, findMask, FileSystemEnumeratorType.File)
		{
		}

		public FileSystemEnumerable(string path, FileSystemEnumeratorType entryType)
			: this(path, "*.*", entryType)
		{
		}

		public FileSystemEnumerable(string path)
			: this(path, FileSystemEnumeratorType.File)
		{
		}

		public IEnumerator<string> GetEnumerator()
		{
			Native.WIN32_FIND_DATA currentFindData;
			IntPtr currentFindHandle = Native.FindFirstFile(Path.Combine(path, findMask), out currentFindData);
			if (currentFindHandle == Native.INVALID_HANDLE_VALUE)
			{
				yield break;
			}
			try
			{
				do
				{
					if (((entryType & FileSystemEnumeratorType.IncludeSystem) == 0 && (currentFindData.dwFileAttributes & FileAttributes.System) != 0) || ((entryType & FileSystemEnumeratorType.IncludeHidden) == 0 && (currentFindData.dwFileAttributes & FileAttributes.Hidden) != 0))
					{
						continue;
					}
					if ((currentFindData.dwFileAttributes & FileAttributes.Directory) != 0)
					{
						if ((entryType & FileSystemEnumeratorType.Folder) != 0 && currentFindData.cFileName != "." && currentFindData.cFileName != "..")
						{
							yield return Path.Combine(path, currentFindData.cFileName);
						}
					}
					else if ((entryType & FileSystemEnumeratorType.File) != 0)
					{
						yield return Path.Combine(path, currentFindData.cFileName);
					}
				}
				while (Native.FindNextFile(currentFindHandle, out currentFindData));
			}
			finally
			{
				Native.FindClose(currentFindHandle);
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
