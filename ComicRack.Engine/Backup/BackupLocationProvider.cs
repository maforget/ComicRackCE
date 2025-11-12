using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using cYo.Common.Runtime;

namespace cYo.Projects.ComicRack.Engine.Backup
{
	/// <summary>
	/// Defines a strategy for locating backup items.
	/// </summary>
	internal interface IBackupLocationProvider
	{
		IEnumerable<string> GetPaths();
		bool IsFile { get; }
		string BaseFolder { get; }
	}

	/// <summary>
	/// Locates backup items from the file system.
	/// </summary>
	internal class FileBackupLocationProvider : IBackupLocationProvider
	{
		private readonly Func<IEnumerable<string>> pathProvider;

		public bool IsFile { get; }
		public string BaseFolder { get; }

		public FileBackupLocationProvider(Func<IEnumerable<string>> pathProvider, bool isFile, string baseFolder)
		{
			this.pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
			IsFile = isFile;
			BaseFolder = baseFolder ?? throw new ArgumentNullException(nameof(baseFolder));
		}

		public IEnumerable<string> GetPaths()
		{
			var paths = pathProvider();
			if (paths == null)
				return Enumerable.Empty<string>();

			return IsFile
				? paths.Where(File.Exists)
				: paths.Where(Directory.Exists);
		}
	}

	/// <summary>
	/// Resolves the appropriate backup location provider based on backup type.
	/// </summary>
	internal class BackupLocationProviderFactory
	{
		private readonly SystemPaths systemPaths;
		private readonly string configFile;
		private readonly string defaultListsFile;
		private readonly string defaultIconPackagesPath;

		public BackupLocationProviderFactory(
			SystemPaths systemPaths,
			string configFile,
			string defaultListsFile,
			string defaultIconPackagesPath)
		{
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
			this.configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
			this.defaultListsFile = defaultListsFile ?? throw new ArgumentNullException(nameof(defaultListsFile));
			this.defaultIconPackagesPath = defaultIconPackagesPath ?? throw new ArgumentNullException(nameof(defaultIconPackagesPath));
		}

		public IBackupLocationProvider Create(BackupOptions backupOption)
		{
			return backupOption switch
			{
				BackupOptions.Database => new FileBackupLocationProvider(
					() => [$"{systemPaths.DatabasePath}.xml", $"{systemPaths.DatabasePath}.xml.bak"],
					isFile: true,
					systemPaths.ApplicationDataPath),

				BackupOptions.Config => new FileBackupLocationProvider(
					() => [configFile],
					isFile: true,
					systemPaths.ApplicationDataPath),

				BackupOptions.ComicRackINI => new FileBackupLocationProvider(
					() => IniFile.GetUserLocations(GetComicRackIniFileName()),
					isFile: true,
					systemPaths.ApplicationDataPath),

				BackupOptions.DefaultLists => new FileBackupLocationProvider(
					() => IniFile.GetUserLocations(defaultListsFile),
					isFile: true,
					systemPaths.ApplicationDataPath),

				BackupOptions.Scripts => new FileBackupLocationProvider(
					() => [systemPaths.ScriptPathSecondary],
					isFile: false,
					systemPaths.ApplicationDataPath),

				BackupOptions.Resources => new FileBackupLocationProvider(
					() => IniFile.GetUserLocations(defaultIconPackagesPath),
					isFile: false,
					systemPaths.ApplicationDataPath),

				BackupOptions.CustomThumbnails => new FileBackupLocationProvider(
					() => [systemPaths.CustomThumbnailPath],
					isFile: false,
					systemPaths.LocalApplicationDataPath),

				BackupOptions.Cache => new FileBackupLocationProvider(
					() => [systemPaths.ImageCachePath, systemPaths.ThumbnailCachePath, systemPaths.FileCachePath],
					isFile: false,
					systemPaths.LocalApplicationDataPath),

				_ => throw new ArgumentException($"Unsupported backup option: {backupOption}", nameof(backupOption))
			};
		}

		private string GetComicRackIniFileName()
		{
			return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) + ".ini";
		}
	}
}
