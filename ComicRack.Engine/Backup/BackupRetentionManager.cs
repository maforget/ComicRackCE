using System;
using System.IO;
using System.Linq;
using cYo.Common.IO;
using cYo.Common.Win32;

namespace cYo.Projects.ComicRack.Engine.Backup
{
	/// <summary>
	/// Manages backup retention policy.
	/// </summary>
	internal class BackupRetentionManager
	{
		public void CleanOldBackups(string backupLocation, int backupsToKeep)
		{
			if (backupsToKeep <= 0 || !Directory.Exists(backupLocation))
				return;

			var backupFiles = FileUtility.GetFiles(backupLocation, SearchOption.TopDirectoryOnly, ".zip")
				.OrderByDescending(x => x)
				.Skip(backupsToKeep - 1);

			try
			{
				foreach (var file in backupFiles)
				{
					ShellFile.DeleteFile(file);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
