using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace cYo.Common.Win32
{
	public static class AlternateDataStreamFile
	{
		private static class NativeMethods
		{
			public struct LARGE_INTEGER
			{
				public int LowPart;

				public int HighPart;

				public long QuadPart => HighPart * (long)Math.Pow(2.0, 32.0) + LowPart;
			}

			public struct WIN32_STREAM_ID
			{
				public int StreamID;

				public int StreamAttributes;

				public LARGE_INTEGER Size;

				public int StreamNameSize;
			}

			public const uint GENERIC_READ = 2147483648u;

			public const uint GENERIC_WRITE = 1073741824u;

			public const uint CREATE_NEW = 1u;

			public const uint CREATE_ALWAYS = 2u;

			public const uint OPEN_EXISTING = 3u;

			public const uint OPEN_ALWAYS = 4u;

			public const uint TRUNCATE_EXISTING = 5u;

			public const uint FILE_BEGIN = 0u;

			public const uint FILE_CURRENT = 1u;

			public const uint FILE_END = 2u;

			public const uint FILE_SHARE_NONE = 0u;

			public const uint FILE_SHARE_READ = 1u;

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern SafeFileHandle CreateFile(string filename, uint access, uint sharemode, IntPtr security_attributes, uint creation, uint flags, IntPtr template);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern int SetFilePointer(SafeFileHandle handle, int distanceToMove, int distanceToMoveHigh, uint moveMethod);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool BackupRead(SafeFileHandle handle, IntPtr lpBuffer, int numberOfBytesToRead, ref int numberOfBytesRead, [MarshalAs(UnmanagedType.U4)] bool abort, [MarshalAs(UnmanagedType.U4)] bool processSecurity, ref int context);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool BackupRead(SafeFileHandle handle, ref WIN32_STREAM_ID streamID, int numberOfBytesToRead, ref int numberOfBytesRead, [MarshalAs(UnmanagedType.U4)] bool abort, [MarshalAs(UnmanagedType.U4)] bool processSecurity, ref int context);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			public static extern bool BackupSeek(SafeFileHandle handle, int lowBytesToSeek, int highBytesToSeek, ref int low, ref int high, ref int context);

			[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern int DeleteFile(string filename);
		}

		public static StreamWriter AppendText(string path, string stream)
		{
			SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 1u, IntPtr.Zero, NativeMethods.OPEN_ALWAYS, 0u, IntPtr.Zero);
			NativeMethods.SetFilePointer(safeFileHandle, 0, 0, 2u);
			if (safeFileHandle.IsInvalid)
			{
				throw new FileNotFoundException("Unable to open stream: " + stream + " in file: " + path + ".", path);
			}
			FileStream stream2 = new FileStream(safeFileHandle, FileAccess.Write);
			return new StreamWriter(stream2);
		}

		public static FileStream Create(string path, string stream)
		{
			SafeFileHandle handle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 0u, IntPtr.Zero, NativeMethods.CREATE_ALWAYS, 0u, IntPtr.Zero);
			return new FileStream(handle, FileAccess.ReadWrite);
		}

		public static FileStream Create(string path, string stream, int bufferSize)
		{
			SafeFileHandle handle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 0u, IntPtr.Zero, NativeMethods.CREATE_ALWAYS, 0u, IntPtr.Zero);
			return new FileStream(handle, FileAccess.ReadWrite, bufferSize);
		}

		public static StreamWriter CreateText(string path, string stream)
		{
			SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 1u, IntPtr.Zero, NativeMethods.CREATE_ALWAYS, 0u, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				throw new FileNotFoundException("Unable to open stream: " + stream + " in file: " + path + ".", path);
			}
			FileStream stream2 = new FileStream(safeFileHandle, FileAccess.Write);
			return new StreamWriter(stream2);
		}

		public static FileStream Open(string path, string stream)
		{
			SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 0u, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0u, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				throw new FileNotFoundException("Unable to open stream: " + stream + " in file: " + path + ".", path);
			}
			return new FileStream(safeFileHandle, FileAccess.ReadWrite);
		}

		public static FileStream OpenRead(string path, string stream)
		{
			SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ, 1u, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0u, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				throw new FileNotFoundException("Unable to open stream: " + stream + " in file: " + path + ".", path);
			}
			return new FileStream(safeFileHandle, FileAccess.Read);
		}

		public static StreamReader OpenText(string path, string stream)
		{
			SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ, 1u, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0u, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				throw new FileNotFoundException("Unable to open stream: " + stream + " in file: " + path + ".", path);
			}
			FileStream stream2 = new FileStream(safeFileHandle, FileAccess.Read);
			return new StreamReader(stream2);
		}

		public static FileStream OpenWrite(string path, string stream)
		{
			SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ | NativeMethods.GENERIC_WRITE, 0u, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0u, IntPtr.Zero);
			if (safeFileHandle.IsInvalid)
			{
				throw new FileNotFoundException("Unable to open stream: " + stream + " in file: " + path + ".", path);
			}
			return new FileStream(safeFileHandle, FileAccess.ReadWrite);
		}

		public static void Delete(string path, string stream)
		{
			int num = 0;
			if ((num = NativeMethods.DeleteFile(path + ":" + stream)) != 0)
			{
				throw new IOException("Unable to delete stream: " + stream + " in file: " + path + ".", num);
			}
		}

		public static bool Exists(string path, string stream)
		{
			using (SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path + ":" + stream, NativeMethods.GENERIC_READ, 1u, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0u, IntPtr.Zero))
			{
				return !safeFileHandle.IsInvalid;
			}
		}

		public static IEnumerable<string> GetStreams(string path)
		{
			using (SafeFileHandle safeFileHandle = NativeMethods.CreateFile(path, NativeMethods.GENERIC_READ, 1u, IntPtr.Zero, NativeMethods.OPEN_EXISTING, 0u, IntPtr.Zero))
			{
				if (safeFileHandle.IsInvalid)
				{
					throw new FileNotFoundException("Unable to open file: " + path + ".", path);
				}
				NativeMethods.WIN32_STREAM_ID streamID = default(NativeMethods.WIN32_STREAM_ID);
				int num = Marshal.SizeOf((object)streamID);
				int numberOfBytesRead = 0;
				int context = 0;
				List<string> list = new List<string>();
				while (NativeMethods.BackupRead(safeFileHandle, ref streamID, num, ref numberOfBytesRead, abort: false, processSecurity: false, ref context))
				{
					if (numberOfBytesRead == num && streamID.StreamNameSize > 0)
					{
						numberOfBytesRead = 0;
						IntPtr intPtr = Marshal.AllocHGlobal(streamID.StreamNameSize);
						try
						{
							NativeMethods.BackupRead(safeFileHandle, intPtr, streamID.StreamNameSize, ref numberOfBytesRead, abort: false, processSecurity: false, ref context);
							char[] array = new char[streamID.StreamNameSize];
							Marshal.Copy(intPtr, array, 0, streamID.StreamNameSize);
							string text = new string(array);
							text = text.Substring(1, text.IndexOf(":", 1) - 1);
							list.Add(text);
						}
						finally
						{
							Marshal.FreeHGlobal(intPtr);
						}
					}
					int low = 0;
					int high = 0;
					if (!NativeMethods.BackupSeek(safeFileHandle, streamID.Size.LowPart, streamID.Size.HighPart, ref low, ref high, ref context))
					{
						break;
					}
				}
				return list;
			}
		}
	}
}
