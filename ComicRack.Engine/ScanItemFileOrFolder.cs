using System.Collections.Generic;
using System.IO;
using cYo.Common.IO;
using cYo.Common.Runtime;

namespace cYo.Projects.ComicRack.Engine
{
	public class ScanItemFileOrFolder : ScanItem
	{
		private readonly string fileOrFolder;

		private readonly bool all = true;

		public ScanItemFileOrFolder(string fileOrFolder, bool all, bool removeMissing, bool forceRefreshInfo = false)
		{
			this.fileOrFolder = fileOrFolder;
			base.AutoRemove = removeMissing;
			this.all = all;
			base.ForceRefreshInfo = forceRefreshInfo;
		}

		public override IEnumerable<string> GetScanFiles()
		{
			if (File.Exists(fileOrFolder))
			{
				return new string[1]
				{
					fileOrFolder
				};
			}
			return FileUtility.GetFiles(fileOrFolder, all ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly, ValidateFolder);
		}

		public override string ToString()
		{
			return fileOrFolder;
		}

		private static FileUtility.FileFolderAction ValidateFolder(string path, bool isPath)
		{
			if (!isPath)
			{
				return FileUtility.FileFolderAction.Default;
			}
			string text = Path.Combine(path, "comicrackscanner.ini");
			if (!File.Exists(text))
			{
				return FileUtility.FileFolderAction.Default;
			}
			return IniFile.GetValue(text, "options", FileUtility.FileFolderAction.Default);
		}
	}
}
