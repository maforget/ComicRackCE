using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Text;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.Backup
{
	/// <summary>
	/// Coordinates the backup process.
	/// </summary>
	public class BackupManager : DisposableObject
	{
		private readonly ProcessingQueue<IEnumerable<BackupFileEntry>> backupQueue;
		private readonly BackupLocationProviderFactory locationProviderFactory;
		private readonly BackupFileCollector fileCollector = new BackupFileCollector();
		private readonly BackupArchiveCreator archiveCreator = new BackupArchiveCreator();
		private readonly BackupRetentionManager retentionManager = new BackupRetentionManager();

		private readonly BackupManagerOptions options;

		public BackupManager(
			BackupManagerOptions options,
			SystemPaths paths,
			string configFile,
			string defaultListsFile,
			string defaultIconPackagesPath)
		{
			this.options = options ?? throw new ArgumentNullException(nameof(options));

			locationProviderFactory = new BackupLocationProviderFactory(paths, configFile, defaultListsFile, defaultIconPackagesPath);
			backupQueue = new ProcessingQueue<IEnumerable<BackupFileEntry>>("Backup Queue", ThreadPriority.Lowest);
		}

		public void RunBackup(bool useQueue = true)
		{
			if (string.IsNullOrEmpty(options.Location))
				return;

			retentionManager.CleanOldBackups(options.Location, options.BackupsToKeep);

			var backupLocations = GetBackupLocations(options.IncludeAllAlternateConfigs);
			if(backupLocations.Any())
			{
				var files = fileCollector.CollectFiles(backupLocations);

				if (useQueue)
					backupQueue.AddItem(files, _ => ExecuteBackup(files, backupLocations));
				else
					ExecuteBackup(files, backupLocations);
			}
		}

		private void ExecuteBackup(IEnumerable<BackupFileEntry> files, IEnumerable<BackupLocation> locations)
		{
			if (!files.Any())
				return;

			var backupTypes = locations.Select(x => x.BackupType);
			archiveCreator.CreateBackup(options.Location, files, backupTypes);
		}

		private IEnumerable<BackupLocation> GetBackupLocations(bool includeAllConfigs)
		{
			var locations = new List<BackupLocation>();

			foreach (BackupOptions flag in Enum.GetValues(typeof(BackupOptions)))
			{
				if (flag == BackupOptions.None || flag == BackupOptions.Full || flag == BackupOptions.FullWithCache || !options.Options.HasFlag(flag))
					continue;

				try
				{
					var provider = locationProviderFactory.Create(new SupportedBackupOption(flag), includeAllConfigs);
					var location = new BackupLocation(provider, flag);
					locations.Add(location);
				}
				catch (ArgumentException)
				{
					// Skip unsupported backup options
				}
			}

			return locations;
		}
	}

	internal record BackupLocation(IBackupLocationProvider Provider, BackupOptions BackupType) { }
}
