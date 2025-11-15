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
		private readonly Func<IEnumerable<string>> pathProvider;

		public bool IsFile { get; }
		public string BaseFolder { get; }

		public BackupLocationProvider(Func<IEnumerable<string>> pathProvider, bool isFile, string baseFolder)
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
}
