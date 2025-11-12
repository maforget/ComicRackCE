using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Projects.ComicRack.Engine
{
	public class BackupManager : DisposableObject
	{
		#region Private Classes
		private record BackupLocation
		{
			public IEnumerable<string> InputPaths { get; set; }
			public bool IsFile { get; set; } = false;
			public bool IsAlternateConfig { get; set; } = false;
			public BackupOptions BackupType { get; set; }
			public string BaseFolder { get; set; }

			public IEnumerable<string> Files
			{
				get
				{
					if (IsFile)
						return InputPaths;
					else
						return InputPaths.SelectMany(x => FileUtility.GetFiles(x, SearchOption.AllDirectories));
				}
			}
		}

		private record BackupFileEntry
		{
			public string Path { get; set; }
			public string BasePath { get; set; }
			public string RelativePath => GetRelativePath();

			public BackupFileEntry(string basePath, string path)
			{
				BasePath = basePath;
				Path = path;
			}

			private string GetRelativePath()
			{
				string basePath = Directory.GetParent(BasePath).FullName;
				return Path.Substring(basePath.Length);
			}
		}

		private class BackupManagerItem : List<BackupLocation>
		{

		}
		#endregion

		private readonly ProcessingQueue<BackupManagerItem> backupQueue = new ProcessingQueue<BackupManagerItem>("Backup Queue", ThreadPriority.Lowest);
		private readonly SystemPaths paths;
		private readonly string configFile;
		private readonly string defaultListsFile;
		private readonly string defaultIconPackagesPath;
		private static readonly string appName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
		private readonly string comicRackINI = appName + ".ini";

		public BackupManagerOptions Options { get; }

		public BackupManager(BackupManagerOptions options, SystemPaths paths, string configFile, string defaultListsFile, string defaultIconPackagesPath)
		{
			this.Options = options;
			this.paths = paths;
			this.configFile = configFile;
			this.defaultListsFile = defaultListsFile;
			this.defaultIconPackagesPath = defaultIconPackagesPath;
		}

		public void RunBackup(bool useQueue = true)
		{
			if (string.IsNullOrEmpty(Options.Location))
				return;

			CleanBackups(); // Deletes the extra backups from the folder

			// Backup either using the queue (usually on Startup) or without when the queue isn't available anymore or is closing (on Exit)
			BackupManagerItem backupManagerItem = GetBackupManagerItem();
			if (useQueue)
				backupQueue.AddItem(backupManagerItem, _ => Backup(backupManagerItem));
			else
				Backup(backupManagerItem);

			var paths = backupManagerItem.SelectMany(x => x.InputPaths);
		}

		private BackupManagerItem GetBackupManagerItem()
		{
			BackupManagerItem backupManagerItem = new BackupManagerItem();
			foreach (BackupOptions flag in Enum.GetValues(typeof(BackupOptions)))
			{
				if (flag != BackupOptions.None && flag != BackupOptions.Full && Options.Options.HasFlag(flag) is bool flagValue && flagValue)
				{
					BackupLocation backupLocation = GetBackupLocation(flag);
					backupManagerItem.Add(backupLocation);
				}
			}

			return backupManagerItem;
		}

		private BackupLocation GetBackupLocation(BackupOptions backupOptions)
		{
			BackupLocation backupLocation = new BackupLocation();
			backupLocation.BackupType = backupOptions;

			switch (backupOptions)
			{
				case BackupOptions.Database:
					backupLocation.InputPaths = [$"{paths.DatabasePath}.xml", $"{paths.DatabasePath}.xml.bak"];
					backupLocation.IsFile = true;
					backupLocation.BaseFolder = paths.ApplicationDataPath;
					break;
				case BackupOptions.Config:
					backupLocation.InputPaths = [configFile];
					backupLocation.IsFile = true;
					backupLocation.BaseFolder = paths.ApplicationDataPath;
					break;
				case BackupOptions.ComicRackINI:
					backupLocation.InputPaths = IniFile.GetUserLocations(comicRackINI);
					backupLocation.IsFile = true;
					backupLocation.BaseFolder = paths.ApplicationDataPath;
					break;
				case BackupOptions.DefaultLists:
					backupLocation.InputPaths = IniFile.GetUserLocations(defaultListsFile);
					backupLocation.IsFile = true;
					backupLocation.BaseFolder = paths.ApplicationDataPath;
					break;
				case BackupOptions.Scripts:
					backupLocation.InputPaths =[paths.ScriptPathSecondary];
					backupLocation.BaseFolder = paths.ApplicationDataPath;
					break;
				case BackupOptions.Resources:
					backupLocation.InputPaths = IniFile.GetUserLocations(defaultIconPackagesPath);
					backupLocation.BaseFolder = paths.ApplicationDataPath;
					break;
				case BackupOptions.CustomThumbnails:
					backupLocation.InputPaths = [paths.CustomThumbnailPath]; // Only inludes the Customthumbnails
					backupLocation.BaseFolder = paths.LocalApplicationDataPath;
					break;
				case BackupOptions.Cache:
					//backupLocation.InputPath = [Path.Combine(paths.LocalApplicationDataPath, "Cache")]; // This includes the full cache
					backupLocation.InputPaths = [paths.ImageCachePath, paths.ThumbnailCachePath, paths.FileCachePath];
					backupLocation.BaseFolder = paths.LocalApplicationDataPath;
					break;
				default:
					backupLocation.InputPaths = Enumerable.Empty<string>();
					break;
			}

			backupLocation.InputPaths = VerifyFileExists(backupLocation);
			return backupLocation;
		}

		private IEnumerable<string> VerifyFileExists(BackupLocation backupLocation)
		{
			if (backupLocation?.InputPaths?.Any() == false)
				return Enumerable.Empty<string>();

			return backupLocation.IsFile ? backupLocation.InputPaths.Where(File.Exists) : backupLocation.InputPaths.Where(Directory.Exists);
		}

		private void CleanBackups()
		{
			var backupFiles = FileUtility.GetFiles(Options.Location, SearchOption.TopDirectoryOnly, ".zip").OrderByDescending(x => x);
			var filesToDelete = Options.BackupsToKeep > 0 ? backupFiles.Skip(Options.BackupsToKeep - 1) : Enumerable.Empty<string>();

			try
			{
				filesToDelete?.ForEach(x => ShellFile.DeleteFile(x));
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void Backup(BackupManagerItem backupManagerItem)
		{
			List<BackupFileEntry> allFiles = new List<BackupFileEntry>();
			foreach (BackupLocation bl in backupManagerItem)
			{
				foreach (string file in bl.Files)
				{
					BackupFileEntry bfe = new BackupFileEntry(bl.BaseFolder, file);
					allFiles.Add(bfe);
				}
			}

			string backupFile = Path.Combine(Options.Location, $"{appName} Backup - {FileUtility.MakeValidFilename($"{DateTime.Now:s}")}.zip");

			try
			{
				if (!Directory.Exists(Options.Location))
					Directory.CreateDirectory(Options.Location);

				using (ZipFile zipFile = ZipFile.Create(backupFile))
				{
					zipFile.BeginUpdate();
					zipFile.SetComment(GetComment(backupManagerItem));
					foreach (BackupFileEntry file in allFiles)
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

		private static string GetComment(BackupManagerItem backupManagerItem)
		{
			var backupTypes = backupManagerItem.Select(x => x.BackupType);
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"ComicRack Backup done on {DateTime.Now:s}");
			sb.AppendLine();
			sb.AppendLine("Includes:");
			backupTypes.ForEach(x => sb.AppendLine(x.ToString()));
			return sb.ToString();
		}
	}

	#region Settings Classes
	[Flags]
	public enum BackupOptions
	{
		None = 0,
		Database = 1 << 0,
		Config = 1 << 1,
		ComicRackINI = 1 << 2,
		DefaultLists = 1 << 3,
		Scripts = 1 << 4,
		Resources = 1 << 5,
		CustomThumbnails = 1 << 6,
		Cache = 1 << 7, // excludes Custom Thumbnails
		Full = 0x7F, // all except for Cache
		FullWithCache = 0xFF, // Full but also includes Cache
	}

	public class BackupManagerOptions
	{
		public string Location { get; set; } = null;

		[DefaultValue(BackupOptions.Full)]
		public BackupOptions Options { get; set; } = BackupOptions.Full;

		/// <summary>
		/// Includes the <see cref="BackupOptions"/> from Alternate Configurations<br/>
		/// Otherwise will only backup the current config
		/// </summary>
		/// 
		[DefaultValue(true)]
		public bool IncludeAlternateConfig { get; set; } = true;

		[DefaultValue(5)]
		public int BackupsToKeep { get; set; } = 5;

		public TimeSpan Interval { get; set; } = TimeSpan.FromDays(7);

		public DateTime LastBackupDate { get; set; } = DateTime.MinValue;

		public bool OnStartup { get; set; } = false;

		public bool OnExit { get; set; } = false;
	} 
	#endregion
}
