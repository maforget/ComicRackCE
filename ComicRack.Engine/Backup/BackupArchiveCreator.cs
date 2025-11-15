using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using cYo.Common.IO;
using cYo.Common.Runtime;
using ICSharpCode.SharpZipLib.Zip;
using SysPath = System.IO.Path;

namespace cYo.Projects.ComicRack.Engine.Backup
{
	/// <summary>
	/// Creates backup archives.
	/// </summary>
	internal class BackupArchiveCreator
	{
		private readonly string appName;

		public BackupArchiveCreator()
		{
			this.appName = GetApplicationName();
		}

		public void CreateBackup(string backupLocation, IEnumerable<BackupFileEntry> files, IEnumerable<BackupOptions> backupTypes)
		{
			if (!files.Any())
				return;

			string backupFile = GenerateBackupFilePath(backupLocation);

			try
			{
				if (!Directory.Exists(backupLocation))
					Directory.CreateDirectory(backupLocation);

				using (ZipFile zipFile = ZipFile.Create(backupFile))
				{
					zipFile.BeginUpdate();
					zipFile.SetComment(CreateBackupComment(backupTypes));

					foreach (var file in files)
					{
						zipFile.Add(file.Path, file.RelativePath);
					}

					zipFile.CommitUpdate();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		private string GenerateBackupFilePath(string backupLocation)
		{
			return Path.Combine(backupLocation, $"{appName} Backup - {FileUtility.MakeValidFilename($"{DateTime.Now:s}")}.zip");
		}

		private string CreateBackupComment(IEnumerable<BackupOptions> backupTypes)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"ComicRack Backup done on {DateTime.Now:s}");
			sb.AppendLine();
			sb.AppendLine("Includes:");

			foreach (var type in backupTypes)
			{
				sb.AppendLine(type.ToString());
			}

			return sb.ToString();
		}

		private static string GetApplicationName()
		{
			return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
		}
	}

	/// <summary>
	/// Represents a file to be backed up with its path information.
	/// </summary>
	internal class BackupFileEntry
	{
		private readonly string CommonApplicationDataPath = IniFile.CommonApplicationDataFolder;
		public string Path { get; set; }
		public string BasePath { get; set; }

		public string RelativePath
		{
			get
			{
				if (Path.StartsWith(BasePath))
				{
					return Path.Substring(BasePath.Length);
				}
				else
				{
					//string dir = SysPath.GetDirectoryName(Path);
					//var test = Path.Substring(SysPath.GetPathRoot(Path).Length); // Removes the root and keeps the rest of the folder structure.
					//var test2 = Path.Substring(Directory.GetParent(Path).FullName.Length); // This just uses the filename, so it may overwrite another file
					//var test3 = Path.Substring(Directory.GetParent(dir).FullName.Length); // keeps the parent folder & filename
					//var test4 = test.Replace(productAppData, ""); // Removes the company & product name from the path

					string productAppData = SysPath.Combine(Application.CompanyName, Application.ProductName);  // Usually cYo\ComicRack Community Edition to remove from path names
					string relativePath = Path.StartsWith(CommonApplicationDataPath)
						? $"ProgramData\\{Path.Substring(CommonApplicationDataPath.Length)}" // When in ProgramData force ProgramData as the archive root folder
						: Path.Substring(SysPath.GetPathRoot(Path).Length).Replace(productAppData, ""); // Removes the root and keeps the rest of the folder structure while taking out the company & product name from the path
					return relativePath;
				}
			}
		}

		public BackupFileEntry(string basePath, string path)
		{
			BasePath = basePath;
			Path = path;
		}
	}
}
