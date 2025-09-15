using System;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace cYo.Common.Win32.FileOperations
{
	internal class SHFileOperation : FileOperation
	{
		private readonly IWin32Window window;

		private static class Native
		{
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			public struct SHFILEOPSTRUCT
			{
				public IntPtr hwnd;
				public FileOperationType wFunc;
				[MarshalAs(UnmanagedType.LPTStr)]
				public string pFrom;
				[MarshalAs(UnmanagedType.LPTStr)]
				public string pTo;
				public FileOperationFlags fFlags;
				public bool fAnyOperationsAborted;
				public IntPtr hNameMappings;
				[MarshalAs(UnmanagedType.LPTStr)]
				public string lpszProgressTitle;
			}

			/// <summary>
			/// File Operation Function Type for SHFileOperation
			/// </summary>
			public enum FileOperationType : uint
			{
				/// <summary>
				/// Move the objects
				/// </summary>
				FO_MOVE = 0x0001,
				/// <summary>
				/// Copy the objects
				/// </summary>
				FO_COPY = 0x0002,
				/// <summary>
				/// Delete (or recycle) the objects
				/// </summary>
				FO_DELETE = 0x0003,
				/// <summary>
				/// Rename the object(s)
				/// </summary>
				FO_RENAME = 0x0004,
			}

			[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
			public static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);
		}

		public SHFileOperation(IWin32Window window, ShellFileDeleteOptions options) : base(options)
		{
			this.window = window;
		}

		public override void DeleteFile(string file)
		{
			VerifyFile(file);

			Native.SHFILEOPSTRUCT lpFileOp = default;
			lpFileOp.hwnd = window?.Handle ?? IntPtr.Zero;
			lpFileOp.wFunc = Native.FileOperationType.FO_DELETE;
			lpFileOp.fFlags = GetDeleteFileFlags();
			lpFileOp.pFrom = $"{file}\0\0";
			lpFileOp.fAnyOperationsAborted = false;
			lpFileOp.hNameMappings = IntPtr.Zero;
			if (Native.SHFileOperation(ref lpFileOp) != 0)
				throw new Win32Exception();
		}
	}
}
