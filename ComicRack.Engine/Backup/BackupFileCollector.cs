using System.Collections.Generic;
using System.IO;
using System.Linq;
using cYo.Common.IO;

namespace cYo.Projects.ComicRack.Engine.Backup
{
	/// <summary>
	/// Collects backup files from multiple locations.
	/// </summary>
	internal class BackupFileCollector
	{
		public IEnumerable<BackupFileEntry> CollectFiles(IEnumerable<BackupLocation> locations)
		{
			var files = new List<BackupFileEntry>();

			foreach (BackupLocation bl in locations)
			{
				var paths = bl.Provider.GetPaths();
				var allFiles = bl.Provider.IsFile
					? paths
					: paths.SelectMany(x => FileUtility.GetFiles(x, SearchOption.AllDirectories));

				foreach (var file in allFiles)
				{
					files.Add(new BackupFileEntry(bl.Provider.BaseFolder, file));
				}
			}

			return files;
		}
	}
}
