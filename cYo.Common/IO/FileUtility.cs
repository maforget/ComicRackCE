using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using cYo.Common.Win32;

namespace cYo.Common.IO
{
	public static class FileUtility
	{
		[Flags]
		public enum FileFolderAction
		{
			Default = 0x0,
			IgnoreFile = 0x1,
			IgnoreFolder = 0x2,
			IgnoreSubFolders = 0x4
		}

		private static class Native
		{
			public const int GCT_INVALID = 0;

			public const int GCT_LFNCHAR = 1;

			public const int GCT_SHORTCHAR = 2;

			public const int GCT_WILD = 4;

			public const int GCT_SEPARATOR = 8;

			[DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
			public static extern int PathGetCharType(char c);
		}

		private static FileFolderAction SafeValidator(Func<string, bool, FileFolderAction> validator, string path, bool isPath)
		{
			return validator?.Invoke(path, isPath) ?? FileFolderAction.Default;
		}

		public static IEnumerable<string> GetFolders(string path, int levels)
		{
			if (!SafeDirectoryExists(path))
			{
				yield break;
			}
			foreach (string sub in Directory.EnumerateDirectories(path))
			{
				yield return Path.Combine(path, sub);
				if (levels <= 0)
				{
					continue;
				}
				foreach (string folder in GetFolders(sub, levels - 1))
				{
					yield return folder;
				}
			}
		}

		public static IEnumerable<string> GetFiles(string path, SearchOption searchOption, Func<string, bool, FileFolderAction> validator = null, params string[] extensions)
		{
			FileFolderAction pathAction = SafeValidator(validator, path, isPath: true);
			if (pathAction.HasFlag(FileFolderAction.IgnoreFolder) || string.IsNullOrEmpty(path))
			{
				yield break;
			}
			foreach (string item in new FileSystemEnumerable(path))
			{
				string checkFile = item;
				FileFolderAction fileFolderAction = SafeValidator(validator, checkFile, isPath: false);
				if (!fileFolderAction.HasFlag(FileFolderAction.IgnoreFile) && (extensions == null || extensions.Length == 0 || extensions.Any((string ext) => checkFile.EndsWith(ext, StringComparison.OrdinalIgnoreCase))))
				{
					yield return item;
				}
			}
			if (searchOption != SearchOption.AllDirectories || pathAction.HasFlag(FileFolderAction.IgnoreSubFolders))
			{
				yield break;
			}
			foreach (string item2 in new FileSystemEnumerable(path, FileSystemEnumeratorType.Folder))
			{
				foreach (string file in GetFiles(item2, searchOption, validator, extensions))
				{
					yield return file;
				}
			}
		}

		public static IEnumerable<string> GetFiles(string path, SearchOption searchOption, params string[] extensions)
		{
			return GetFiles(path, searchOption, null, extensions);
		}

		public static IEnumerable<string> GetFiles(IEnumerable<string> paths, SearchOption searchOption, params string[] extensions)
		{
			foreach (string path in paths)
			{
				foreach (string file in GetFiles(path, searchOption, null, extensions))
				{
					yield return file;
				}
			}
		}

		public static bool ForeachFile(string path, SearchOption searchOption, Predicate<string> action)
		{
			return GetFiles(path, searchOption).Any((string file) => !action(file));
		}

		public static bool ForeachFile(IEnumerable<string> paths, SearchOption searchOption, Predicate<string> action)
		{
			return GetFiles(paths, searchOption).Any((string file) => !action(file));
		}

		private static string NetMakeValidFilename(string name, char safe)
		{
			StringBuilder stringBuilder = new StringBuilder(name);
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			foreach (char oldChar in invalidFileNameChars)
			{
				stringBuilder.Replace(oldChar, safe);
			}
			return stringBuilder.ToString();
		}

		public static string MakeValidFilename(string name, char safe = '_')
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char c in name)
				{
					int num = Native.PathGetCharType(c);
					if (num == Native.GCT_INVALID || ((uint)num & 0xCu) != 0)
					{
						stringBuilder.Append(safe);
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				return stringBuilder.ToString();
			}
			catch (Exception)
			{
				return NetMakeValidFilename(name, safe);
			}
		}

		public static string GetSafeFileName(string file)
		{
			try
			{
				return Path.GetFileName(file);
			}
			catch (ArgumentException)
			{
				return file;
			}
		}

		public static bool CreateEmpty(string path)
		{
			try
			{
				using (File.CreateText(path))
				{
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static long GetSize(string temp)
		{
			try
			{
				return new FileInfo(temp).Length;
			}
			catch (Exception)
			{
			}
			return 0L;
		}

		public static bool SafeFileExists(string path)
		{
			try
			{
				return File.Exists(path);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static bool SafeDirectoryExists(string path)
		{
			try
			{
				return Directory.Exists(path);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static byte[] ReadAllBytes(this Stream s)
		{
			try
			{
				int num = (int)(s.Length - s.Position);
				byte[] array = new byte[num];
				if (s.Read(array, 0, num) == num)
				{
					return array;
				}
				return null;
			}
			catch
			{
				return null;
			}
		}

		public static void WriteStream(string file, Stream data)
		{
			using (Stream destination = File.Create(file))
			{
				data.CopyTo(destination);
			}
		}

		public static DriveInfo GetDriveInfo(string path)
		{
			return DriveInfo.GetDrives().FirstOrDefault((DriveInfo di) => string.Equals(Path.GetPathRoot(path), di.RootDirectory.Name, StringComparison.OrdinalIgnoreCase));
		}

		public static DriveType GetDriveType(string path)
		{
			return GetDriveInfo(path)?.DriveType ?? DriveType.Unknown;
		}

		public static bool SafeDelete(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			try
			{
				File.Delete(path);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool SafeDirectoryDelete(string path, bool recursive = true)
		{
			try
			{
				Directory.Delete(path, recursive);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static IEnumerable<string> SafeGetFiles(string folder, string searchPattern = "*.*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			try
			{
				return Directory.GetFiles(folder, searchPattern, searchOption);
			}
			catch (Exception)
			{
				return Enumerable.Empty<string>();
			}
		}

		public static IEnumerable<string> ReadLines(this TextReader tr)
		{
			while (true)
			{
				string text;
				string line = (text = tr.ReadLine());
				if (text != null)
				{
					yield return line;
					continue;
				}
				break;
			}
		}

		public static IEnumerable<string> ReadLines(this Stream s)
		{
			using (StreamReader sw = new StreamReader(s))
			{
				foreach (string item in sw.ReadLines())
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<string> ReadLines(string file)
		{
			using (StreamReader s = File.OpenText(file))
			{
				foreach (string item in s.ReadLines())
				{
					yield return item;
				}
			}
		}
	}
}
