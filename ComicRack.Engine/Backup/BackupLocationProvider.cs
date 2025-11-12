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
	/// Describes how to create a backup location provider for a specific backup type.
	/// Implements the Strategy pattern for provider creation.
	/// </summary>
	internal interface IBackupLocationProviderStrategy
	{
		BackupOptions SupportedBackupOption { get; }
		IBackupLocationProvider CreateProvider();
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
	/// Registry for backup location provider creation strategies.
	/// Implements the Registry pattern to achieve OCP compliance.
	/// New backup types can be added without modifying this class.
	/// </summary>
	internal class BackupLocationProviderRegistry
	{
		private readonly Dictionary<BackupOptions, IBackupLocationProviderStrategy> strategies =
			new Dictionary<BackupOptions, IBackupLocationProviderStrategy>();

		/// <summary>
		/// Registers a backup location provider strategy.
		/// </summary>
		public void Register(IBackupLocationProviderStrategy strategy)
		{
			if (strategy == null)
				throw new ArgumentNullException(nameof(strategy));

			strategies[strategy.SupportedBackupOption] = strategy;
		}

		/// <summary>
		/// Creates a backup location provider for the specified backup option.
		/// </summary>
		public IBackupLocationProvider Create(BackupOptions backupOption)
		{
			if (strategies.TryGetValue(backupOption, out var strategy))
				return strategy.CreateProvider();

			throw new ArgumentException(
				$"Unsupported backup option: {backupOption}. No strategy registered.",
				nameof(backupOption));
		}
	}

	/// <summary>
	/// Factory that initializes and provides the backup location provider registry.
	/// Responsible for bootstrapping all default backup location strategies.
	/// </summary>
	internal class BackupLocationProviderFactory
	{
		private readonly BackupLocationProviderRegistry registry;

		public BackupLocationProviderFactory(
			SystemPaths systemPaths,
			string configFile,
			string defaultListsFile,
			string defaultIconPackagesPath)
		{
			registry = new BackupLocationProviderRegistry();
			InitializeStrategies(systemPaths, configFile, defaultListsFile, defaultIconPackagesPath);
		}

		/// <summary>
		/// Creates a backup location provider for the specified backup option.
		/// </summary>
		public IBackupLocationProvider Create(BackupOptions backupOption)
		{
			return registry.Create(backupOption);
		}

		/// <summary>
		/// Initializes all default backup location provider strategies.
		/// This is where extension points can be added for custom backup types.
		/// </summary>
		private void InitializeStrategies(
			SystemPaths systemPaths,
			string configFile,
			string defaultListsFile,
			string defaultIconPackagesPath)
		{
			// Database backup
			registry.Register(new DatabaseBackupStrategy(systemPaths));

			// Config backup
			registry.Register(new ConfigBackupStrategy(configFile, systemPaths));

			// ComicRack INI backup
			registry.Register(new ComicRackIniBackupStrategy(systemPaths));

			// Default lists backup
			registry.Register(new DefaultListsBackupStrategy(defaultListsFile, systemPaths));

			// Scripts backup
			registry.Register(new ScriptsBackupStrategy(systemPaths));

			// Resources backup
			registry.Register(new ResourcesBackupStrategy(defaultIconPackagesPath, systemPaths));

			// Custom thumbnails backup
			registry.Register(new CustomThumbnailsBackupStrategy(systemPaths));

			// Cache backup
			registry.Register(new CacheBackupStrategy(systemPaths));
		}
	}

	#region Concrete Backup Strategy Implementations

	/// <summary>
	/// Strategy for backing up the database files.
	/// </summary>
	internal class DatabaseBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.Database;

		public DatabaseBackupStrategy(SystemPaths systemPaths)
		{
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => [$"{systemPaths.DatabasePath}.xml", $"{systemPaths.DatabasePath}.xml.bak"],
				isFile: true,
				systemPaths.ApplicationDataPath);
		}
	}

	/// <summary>
	/// Strategy for backing up the config file.
	/// </summary>
	internal class ConfigBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly string configFile;
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.Config;

		public ConfigBackupStrategy(string configFile, SystemPaths systemPaths)
		{
			this.configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => [configFile],
				isFile: true,
				systemPaths.ApplicationDataPath);
		}
	}

	/// <summary>
	/// Strategy for backing up the ComicRack INI file.
	/// </summary>
	internal class ComicRackIniBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.ComicRackINI;

		public ComicRackIniBackupStrategy(SystemPaths systemPaths)
		{
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => IniFile.GetUserLocations(GetComicRackIniFileName()),
				isFile: true,
				systemPaths.ApplicationDataPath);
		}

		private string GetComicRackIniFileName()
		{
			return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) + ".ini";
		}
	}

	/// <summary>
	/// Strategy for backing up default lists files.
	/// </summary>
	internal class DefaultListsBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly string defaultListsFile;
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.DefaultLists;

		public DefaultListsBackupStrategy(string defaultListsFile, SystemPaths systemPaths)
		{
			this.defaultListsFile = defaultListsFile ?? throw new ArgumentNullException(nameof(defaultListsFile));
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => IniFile.GetUserLocations(defaultListsFile),
				isFile: true,
				systemPaths.ApplicationDataPath);
		}
	}

	/// <summary>
	/// Strategy for backing up script files.
	/// </summary>
	internal class ScriptsBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.Scripts;

		public ScriptsBackupStrategy(SystemPaths systemPaths)
		{
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => [systemPaths.ScriptPathSecondary],
				isFile: false,
				systemPaths.ApplicationDataPath);
		}
	}

	/// <summary>
	/// Strategy for backing up resource files.
	/// </summary>
	internal class ResourcesBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly string defaultIconPackagesPath;
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.Resources;

		public ResourcesBackupStrategy(string defaultIconPackagesPath, SystemPaths systemPaths)
		{
			this.defaultIconPackagesPath = defaultIconPackagesPath ?? throw new ArgumentNullException(nameof(defaultIconPackagesPath));
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => IniFile.GetUserLocations(defaultIconPackagesPath),
				isFile: false,
				systemPaths.ApplicationDataPath);
		}
	}

	/// <summary>
	/// Strategy for backing up custom thumbnail files.
	/// </summary>
	internal class CustomThumbnailsBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.CustomThumbnails;

		public CustomThumbnailsBackupStrategy(SystemPaths systemPaths)
		{
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => [systemPaths.CustomThumbnailPath],
				isFile: false,
				systemPaths.LocalApplicationDataPath);
		}
	}

	/// <summary>
	/// Strategy for backing up cache files.
	/// </summary>
	internal class CacheBackupStrategy : IBackupLocationProviderStrategy
	{
		private readonly SystemPaths systemPaths;

		public BackupOptions SupportedBackupOption => BackupOptions.Cache;

		public CacheBackupStrategy(SystemPaths systemPaths)
		{
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		public IBackupLocationProvider CreateProvider()
		{
			return new FileBackupLocationProvider(
				() => [systemPaths.ImageCachePath, systemPaths.ThumbnailCachePath, systemPaths.FileCachePath],
				isFile: false,
				systemPaths.LocalApplicationDataPath);
		}
	}

	#endregion
}
