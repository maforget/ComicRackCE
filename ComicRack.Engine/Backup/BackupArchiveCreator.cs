using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using cYo.Common.IO;
using ICSharpCode.SharpZipLib.Zip;

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
		public string Path { get; set; }
		public string BasePath { get; set; }

		public string RelativePath
		{
			get
			{
				string basePath = Directory.GetParent(BasePath).FullName;
				return Path.Substring(basePath.Length);
			}
		}

		public BackupFileEntry(string basePath, string path)
		{
			BasePath = basePath;
			Path = path;
		}
	}
}
