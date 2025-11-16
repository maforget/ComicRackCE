using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
	internal class BackupLocationProvider : IBackupLocationProvider
	{
		protected IEnumerable<string> paths;

		public bool IsFile { get; }
		public virtual string BaseFolder { get; }

		public BackupLocationProvider(IEnumerable<string> paths, bool isFile, string baseFolder)
		{
			this.paths = paths ?? throw new ArgumentNullException(nameof(paths));
			IsFile = isFile;
			BaseFolder = baseFolder ?? throw new ArgumentNullException(nameof(baseFolder));
		}

		public virtual IEnumerable<string> GetPaths()
		{
			var paths = this.paths;
			if (paths == null)
				return Enumerable.Empty<string>();

			return IsFile
				? paths.Where(File.Exists)
				: paths.Where(Directory.Exists);
		}
	}

	/// <summary>
	/// Locates backup items from the file system but includes all alternate configs
	/// </summary>
	internal class FullBackupLocationProvider : BackupLocationProvider
	{
		string currentAppData, currentBaseAppData;
		string newBaseFolder;

		public override string BaseFolder => newBaseFolder;

		public FullBackupLocationProvider(IEnumerable<string> paths, bool isFile, string baseFolder, SystemPaths systemPaths)
			: base(paths, isFile, baseFolder)
		{
			this.newBaseFolder = baseFolder;
			string baseAppData = SystemPaths.GetApplicationDataPath(systemPaths.UseLocal, string.Empty);
			string appData = SystemPaths.GetApplicationDataPath(systemPaths.UseLocal, systemPaths.AlternateConfig);

			string baseLocalAppData = SystemPaths.GetLocalApplicationDataPath(systemPaths.UseLocal, string.Empty);
			string localAppData = SystemPaths.GetLocalApplicationDataPath(systemPaths.UseLocal, systemPaths.AlternateConfig);

			currentAppData = BaseFolder == appData ? appData : localAppData;
			currentBaseAppData = currentAppData == appData ? baseAppData : baseLocalAppData;

			if (ReplaceBasePath(BaseFolder, currentBaseAppData, currentAppData, out string newBaseFolder)) // Replace the BaseFolder
				this.newBaseFolder = newBaseFolder;
		}

		public override IEnumerable<string> GetPaths()
		{
			List<string> newPaths = new List<string>();
			foreach (string p in paths)
			{
				bool wasTouched = ReplaceBasePath(p, currentBaseAppData, currentAppData, out string fixedPath); // Replaces the path of the file so it is always the absolute base
				newPaths.Add(fixedPath); // Add existing paths, but will be in the base isntead if it was an alternative config

				// Find the same file in the Configurations folder
				if (fixedPath.StartsWith(currentBaseAppData))
				{
					string relativePath = fixedPath.Substring(currentBaseAppData.Length + 1);
					foreach(string ac in EnumerateAlternateConfig())
					{
						string configPath = Path.Combine(BaseFolder, "Configurations", ac, relativePath); // New potential location of file/folder in Configurations folder
						newPaths.Add(configPath); // Add the new alternate config location to the paths
					}
				}
			}

			paths = newPaths;
			return base.GetPaths(); // Will call base GetPath that return only files that actually exists rather than only potentials
		}

		/// <summary>
		/// Replaces the path set it to the absolute AppData base folder if applicable, otherwise returns the same input path
		/// </summary>
		private static bool ReplaceBasePath(string path, string basePath, string configBasePath, out string newPath)
		{
			newPath = path;
			if (basePath != configBasePath && path.StartsWith(configBasePath)) // We are not in the root config so need to get the correct folder
			{
				newPath = path.Replace(configBasePath, basePath);
				return true;
			}
			return false;
		}

		private IEnumerable<string> EnumerateAlternateConfig()
		{
			try
			{
				if (Directory.Exists(currentBaseAppData))
					return Directory.EnumerateDirectories(Path.Combine(currentBaseAppData, "Configurations")).Select(f => new DirectoryInfo(f).Name);
			}
			catch
			{
			}
			return Enumerable.Empty<string>();
		}
	}
}
