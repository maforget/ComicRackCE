using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Win32
{
	/// <summary>
	/// Helper class to interact with Windows File Explorer, e.g. to open folders or select files.
	/// </summary>
	public static class FileExplorer
	{
		static class NativeMethods
	{

		[DllImport("shell32.dll")]
		public static extern void ILFree(IntPtr pidlList);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern IntPtr ILCreateFromPath(string pszPath);

		[DllImport("shell32.dll")]
		public static extern uint SHOpenFolderAndSelectItems(IntPtr pidlList, uint cild, IntPtr children, uint dwFlags);

		public enum HRESULT : uint
		{
			S_OK = 0,
			S_FALSE = 1,
			E_ABORT = 0x80004004,
			E_ACCESSDENIED = 0x80070005,
			E_FAIL = 0x80004005,
			E_HANDLE = 0x80070006,
			E_INVALIDARG = 0x80070057,
			E_NOTIMPL = 0x80004001,
			E_NOINTERFACE = 0x80004002,
			E_OUTOFMEMORY = 0x8007000E,
			E_POINTER = 0x80004003,
			E_UNEXPECTED = 0x8000FFFF
		}
	}

		/// <summary>
		/// Opens Windows File Explorer and selects the specified file (or directory).
		/// </summary>
		/// <param name="path">Path for the file (or directory)</param>
		/// <param name="useAPI">Set to true to open using the Windows API instead of creating a new explorer process</param>
		/// <returns>Returns if the operation was successful (or not)</returns>
		public static bool OpenFolderAndSelect(string path, bool useAPI = true)
		{
			if (useAPI)
				return SelectUsingAPI(path);
			else
				return SelectUsingExplorer(path);
		}

		/// <summary>
		/// Opens Windows File Explorer at the specified folder path.
		/// </summary>
		/// <param name="folderPath">Path for the folder</param>
		/// <param name="useAPI">Set to true to open using the Windows API instead of creating a new explorer process</param>
		/// <returns>Returns if the operation was successful (or not)</returns>
		public static bool OpenFolder(string folderPath, bool useAPI = true)
		{
			if (useAPI)
				return OpenFolderUsingAPI(folderPath);
			else
				return OpenFolderUsingExplorer(folderPath);
		}

		// This method is more reliable && doesn't spam the explorer process
		// ref: https://stackoverflow.com/questions/3018002/c-how-to-use-shopenfolderandselectitems
		private static bool SelectUsingAPI(string path)
		{
			if (string.IsNullOrEmpty(path))
				return false;

			string fullPath = Path.GetFullPath(path);
			bool exists = File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory) ? Directory.Exists(fullPath) : File.Exists(fullPath);
			if (!exists)
				return false;

			IntPtr pidlList = NativeMethods.ILCreateFromPath(fullPath);
			if (pidlList != IntPtr.Zero)
			{
				try
				{
					uint result = NativeMethods.SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0);
					return (NativeMethods.HRESULT)result == NativeMethods.HRESULT.S_OK;
				}
				catch (Exception)
				{
					return false;
				}
				finally
				{
					NativeMethods.ILFree(pidlList);
				}
			}

			return false;
		}

		private static bool SelectUsingExplorer(string path)
		{
			if (string.IsNullOrEmpty(path))
				return false;

			string fullPath = Path.GetFullPath(path);
			bool exists = File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory) ? Directory.Exists(fullPath) : File.Exists(fullPath);
			if (!exists)
				return false;

			try
			{
				Process.Start("explorer.exe", $"/n,/select,\"{fullPath}\""); // Open explorer and select file
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		private static bool OpenFolderUsingExplorer(string folderPath)
		{
			if (string.IsNullOrEmpty(folderPath))
				return false;

			folderPath = Path.GetFullPath(folderPath);
			if (!Directory.Exists(folderPath))
				return false;

			try
			{
				Process.Start("explorer.exe", $"\"{folderPath}\""); // Open explorer at directory
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}

		// This method doesn't spam the explorer process & handles long paths
		// ref: https://stackoverflow.com/questions/1132422/open-a-folder-using-process-start
		private static bool OpenFolderUsingAPI(string folderPath)
		{
			if (string.IsNullOrEmpty(folderPath))
				return false;

			folderPath = Path.GetFullPath(folderPath);
			if (!Directory.Exists(folderPath))
				return false;

			try
			{
				Process.Start(new ProcessStartInfo()
				{
					FileName = folderPath,
					UseShellExecute = true,
					Verb = "open"
				});
				return true;
			}
			catch (Exception)
			{
			}
			return false;
		}
	}
}
