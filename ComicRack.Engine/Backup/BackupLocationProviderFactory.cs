using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using cYo.Common.Runtime;

namespace cYo.Projects.ComicRack.Engine.Backup
{
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
		public IBackupLocationProvider Create(ISupportedBackupOption backupOption, bool includeAllConfigs)
		{
			return registry.Create(backupOption, includeAllConfigs);
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
}
