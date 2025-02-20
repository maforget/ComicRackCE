using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using cYo.Common.Win32.FileOperations;

namespace cYo.Common.Win32
{
	public static class ShellFile
	{
		public static FileOperationsAPI DeleteAPI { get; set; } = FileOperationsAPI.IFileOperation;

		public static void DeleteFile(IWin32Window window, ShellFileDeleteOptions options, params string[] files)
		{
			if (files == null || files.Length == 0)
				return;

			foreach (string file in files)
			{
				if (string.IsNullOrEmpty(file) || !File.Exists(file))
					continue;

				using (FileOperation fileOperation = FileOperation.GetFileOperationAPI(window, DeleteAPI, options))
				{
					fileOperation.DeleteFile(file);
				}
			}
		}

		public static void DeleteFile(ShellFileDeleteOptions options, params string[] files)
		{
			DeleteFile(null, options, files);
		}

		public static void DeleteFile(params string[] files)
		{
			DeleteFile(ShellFileDeleteOptions.None, files);
		}
	}
}
