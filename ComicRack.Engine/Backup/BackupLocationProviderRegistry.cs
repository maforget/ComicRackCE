using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using cYo.Common.Runtime;

namespace cYo.Projects.ComicRack.Engine.Backup
{
	/// <summary>
	/// Describes how to create a backup location provider for a specific backup type.
	/// Implements the Strategy pattern for provider creation.
	/// </summary>
	internal interface IBackupLocationProviderStrategy
	{
		ISupportedBackupOption SupportedBackupOption { get; }
		IBackupLocationProvider CreateProvider(bool includeAllConfigs);
	}

	internal interface ISupportedBackupOption
	{
		BackupOptions BackupOption { get; }
	}

	internal record SupportedBackupOption(BackupOptions BackupOption) : ISupportedBackupOption;

	/// <summary>
	/// Registry for backup location provider creation strategies.
	/// Implements the Registry pattern to achieve OCP compliance.
	/// New backup types can be added without modifying this class.
	/// </summary>
	internal class BackupLocationProviderRegistry
	{
		private readonly Dictionary<ISupportedBackupOption, IBackupLocationProviderStrategy> strategies =
			new Dictionary<ISupportedBackupOption, IBackupLocationProviderStrategy>();

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
		public IBackupLocationProvider Create(ISupportedBackupOption backupOption, bool includeAllConfigs)
		{
			if (strategies.TryGetValue(backupOption, out var strategy))
				return strategy.CreateProvider(includeAllConfigs);

			throw new ArgumentException(
				$"Unsupported backup option: {backupOption}. No strategy registered.",
				nameof(backupOption));
		}
	}

	#region Concrete Backup Strategy Implementations

	internal abstract class BackupLocationProviderStrategy : IBackupLocationProviderStrategy
	{
		protected readonly SystemPaths systemPaths;

		public abstract ISupportedBackupOption SupportedBackupOption { get; }
		public abstract IBackupLocationProvider CreateProvider(bool includeAllConfigs);

		protected BackupLocationProviderStrategy(SystemPaths systemPaths)
		{
			this.systemPaths = systemPaths ?? throw new ArgumentNullException(nameof(systemPaths));
		}

		protected IBackupLocationProvider GetBackupLocationProvider(IEnumerable<string> pathProvider, bool isFile, string baseFolder, bool includeAllConfigs)
		{
			if (includeAllConfigs)
				return new FullBackupLocationProvider(pathProvider, isFile, baseFolder, systemPaths);
			else
				return new BackupLocationProvider(pathProvider, isFile, baseFolder);
		}
	}

	/// <summary>
	/// Strategy for backing up the database files.
	/// </summary>
	internal class DatabaseBackupStrategy : BackupLocationProviderStrategy
	{
		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.Database);

		public DatabaseBackupStrategy(SystemPaths systemPaths) : base(systemPaths)
		{
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				[$"{systemPaths.DatabasePath}.xml", $"{systemPaths.DatabasePath}.xml.bak"],
				isFile: true,
				systemPaths.ApplicationDataPath,
				includeAllConfigs);
		}
	}

	/// <summary>
	/// Strategy for backing up the config file.
	/// </summary>
	internal class ConfigBackupStrategy : BackupLocationProviderStrategy
	{
		private readonly string configFile;

		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.Config);

		public ConfigBackupStrategy(string configFile, SystemPaths systemPaths) : base (systemPaths)
		{
			this.configFile = configFile ?? throw new ArgumentNullException(nameof(configFile));
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				[configFile],
				isFile: true,
				systemPaths.ApplicationDataPath,
				includeAllConfigs);
		}
	}

	/// <summary>
	/// Strategy for backing up the ComicRack INI file.
	/// </summary>
	internal class ComicRackIniBackupStrategy : BackupLocationProviderStrategy
	{
 		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.ComicRackINI);

		public ComicRackIniBackupStrategy(SystemPaths systemPaths) : base(systemPaths)
		{
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				IniFile.GetUserLocations(GetComicRackIniFileName()),
				isFile: true,
				systemPaths.ApplicationDataPath,
				includeAllConfigs);
		}

		private string GetComicRackIniFileName()
		{
			return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location) + ".ini";
		}
	}

	/// <summary>
	/// Strategy for backing up default lists files.
	/// </summary>
	internal class DefaultListsBackupStrategy : BackupLocationProviderStrategy
	{
		private readonly string defaultListsFile;

		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.DefaultLists);

		public DefaultListsBackupStrategy(string defaultListsFile, SystemPaths systemPaths) : base(systemPaths)
		{
			this.defaultListsFile = defaultListsFile ?? throw new ArgumentNullException(nameof(defaultListsFile));
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				IniFile.GetUserLocations(defaultListsFile),
				isFile: true,
				systemPaths.ApplicationDataPath,
				includeAllConfigs);
		}
	}

	/// <summary>
	/// Strategy for backing up script files.
	/// </summary>
	internal class ScriptsBackupStrategy : BackupLocationProviderStrategy
	{
		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.Scripts);

		public ScriptsBackupStrategy(SystemPaths systemPaths) : base(systemPaths)
		{
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				[systemPaths.ScriptPathSecondary],
				isFile: false,
				systemPaths.ApplicationDataPath,
				includeAllConfigs);
		}
	}

	/// <summary>
	/// Strategy for backing up resource files.
	/// </summary>
	internal class ResourcesBackupStrategy : BackupLocationProviderStrategy
	{
		private readonly string defaultIconPackagesPath;
		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.Resources);

		public ResourcesBackupStrategy(string defaultIconPackagesPath, SystemPaths systemPaths) : base (systemPaths)
		{
			this.defaultIconPackagesPath = defaultIconPackagesPath ?? throw new ArgumentNullException(nameof(defaultIconPackagesPath));
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				IniFile.GetUserLocations(defaultIconPackagesPath),
				isFile: false,
				systemPaths.ApplicationDataPath,
				includeAllConfigs);
		}
	}

	/// <summary>
	/// Strategy for backing up custom thumbnail files.
	/// </summary>
	internal class CustomThumbnailsBackupStrategy : BackupLocationProviderStrategy
	{
		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.CustomThumbnails);

		public CustomThumbnailsBackupStrategy(SystemPaths systemPaths) : base(systemPaths)
		{
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				[systemPaths.CustomThumbnailPath],
				isFile: false,
				systemPaths.LocalApplicationDataPath,
				includeAllConfigs);
		}
	}

	/// <summary>
	/// Strategy for backing up cache files.
	/// </summary>
	internal class CacheBackupStrategy : BackupLocationProviderStrategy
	{
		public override ISupportedBackupOption SupportedBackupOption => new SupportedBackupOption(BackupOptions.Cache);

		public CacheBackupStrategy(SystemPaths systemPaths) : base(systemPaths)
		{
		}

		public override IBackupLocationProvider CreateProvider(bool includeAllConfigs)
		{
			return GetBackupLocationProvider(
				[systemPaths.ImageCachePath, systemPaths.ThumbnailCachePath, systemPaths.FileCachePath],
				isFile: false,
				systemPaths.LocalApplicationDataPath,
				includeAllConfigs);
		}
	}

	#endregion
}
