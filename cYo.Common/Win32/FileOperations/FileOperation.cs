using System;
using System.Windows.Forms;
using cYo.Common.ComponentModel;

namespace cYo.Common.Win32.FileOperations
{
	internal abstract class FileOperation : DisposableObject
	{
		protected ShellFileDeleteOptions _options;

		protected FileOperation(ShellFileDeleteOptions options)
		{
			_options = options;
		}

		public static FileOperation GetFileOperationAPI(IWin32Window window, FileOperationsAPI api, ShellFileDeleteOptions options)
		{
			return api switch
			{
				FileOperationsAPI.Shell => new Shell(options),
				FileOperationsAPI.SHFileOperation => new SHFileOperation(window, options),
				FileOperationsAPI.NetFramework => new NetFramework(),
				FileOperationsAPI.VisualBasic => new VisualBasic(options),
				_ => new IFileOperation(new IFileOperation.FileOperationProgressSink(), window, options),
			};
		}

		internal FileOperationFlags GetDeleteFileFlags()
		{
			FileOperationFlags fFlags = FileOperationFlags.FOF_SILENT | FileOperationFlags.FOF_NOERRORUI;
			if (!_options.HasFlag(ShellFileDeleteOptions.NoRecycleBin))
			{
				fFlags |= FileOperationFlags.FOF_ALLOWUNDO;
			}
			if (!_options.HasFlag(ShellFileDeleteOptions.Confirmation))
			{
				fFlags |= FileOperationFlags.FOF_NOCONFIRMATION;
			}

			return fFlags;
		}

		public abstract void DeleteFile(string file);

		protected virtual void VerifyFile(string file)
		{
			if (string.IsNullOrWhiteSpace(file) || !System.IO.File.Exists(file))
				throw new ArgumentException("Invalid file path");
		}
	}
}
