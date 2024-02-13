using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Threading;
using cYo.Common.Windows.Forms;
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine.Database.Storage;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class ComicDatabase : ComicLibrary, IBlackList
	{
		private readonly WatchFolderCollection ownWatchFolders;

		[NonSerialized]
		private ComicStorage comicStorage;

		[NonSerialized]
		private ComicBookContainerUndo undo;

		private WatchFolderCollection watchFolders = new WatchFolderCollection();

		private HashSet<string> blackList = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		public static int BackupCount = 10;

		private const string BackupDatabaseName = "ComicDb.xml";

		private const string ThumbnailsBackupFolder = "Thumbnails";

		[XmlIgnore]
		public ComicStorage ComicStorage
		{
			get
			{
				return comicStorage;
			}
			set
			{
				comicStorage = value;
			}
		}

		[XmlIgnore]
		public ComicBookContainerUndo Undo
		{
			get
			{
				return undo;
			}
			set
			{
				undo = value;
			}
		}

		public WatchFolderCollection WatchFolders => watchFolders;

		[XmlArrayItem("File")]
		public HashSet<string> BlackList => blackList;

		public ComicDatabase(bool enableWatchfolders)
		{
			ownWatchFolders = watchFolders;
			if (enableWatchfolders)
			{
				watchFolders.Changed += watchFolders_Changed;
			}
			Undo = new ComicBookContainerUndo
			{
				Container = this
			};
		}

		public ComicDatabase()
			: this(enableWatchfolders: true)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ownWatchFolders.Changed -= watchFolders_Changed;
				ownWatchFolders.ForEach(delegate(WatchFolder wf)
				{
					wf.Dispose();
				});
				if (ComicStorage != null)
				{
					ComicStorage.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		public void FinalizeLoading()
		{
			if (base.Books.Count > 0)
			{
				int count = base.Books.Count;
				int i = 0;
				AutomaticProgressDialog.Process(null, TR.Messages["CheckingDatabase", "Checking Database"], TR.Messages["CheckingDatabaseText", "Checking and updating missing data in Database"], 10000, delegate
				{
					base.Books.ForEach(delegate(ComicBook cb)
					{
						cb.ValidateData();
						cb.FileInfoRetrieved = true;
						AutomaticProgressDialog.Value = i++ * 100 / count;
					});
				}, AutomaticProgressDialogOptions.None);
			}
			if (ComicLibrary.IsQueryCacheEnabled)
			{
				(from cli in base.ComicLists.GetItems<ComicListItem>()
					where cli.NewBookCount > 0 && cli.NewBookCountDate.CompareTo(DateTime.UtcNow, ignoreTime: true) != 0
					select cli).ForEach(delegate(ComicListItem cli)
				{
					cli.NotifyCacheRetrieval();
				});
			}
			base.IsLoaded = true;
			base.TemporaryFolder.Items.Clear();
			base.TemporaryFolder.Refresh();
			base.IsDirty = false;
		}

		public void CreateDummyEntries(string baseName, int count, int pages)
		{
			for (int i = 0; i < count; i++)
			{
				ComicBook comicBook = new ComicBook
				{
					Number = (i + 1).ToString(),
					Series = baseName + " Series",
					Volume = 1,
					Year = DateTime.Now.Year,
					Month = i % 12 + 1,
					Day = 1,
					Count = count,
					Writer = "John Writer",
					Inker = "John Inker",
					Penciller = "John Penciller",
					Publisher = baseName + " Publisher",
					Notes = "This is a dummy Book entry"
				};
				comicBook.Title = "Title of " + comicBook.Series + " Book " + comicBook.Number;
				comicBook.FilePath = $"C:\\Documents and Settings\\Documents\\Books\\{comicBook.Series} Series\\{comicBook.Series} #{comicBook.Number} of {comicBook.Count} ({comicBook.Year}) - {comicBook.Publisher}.cbz";
				comicBook.PageCount = pages;
				for (int j = 0; j < pages; j++)
				{
					comicBook.UpdatePageSize(j, 800, 600);
				}
				comicBook.Pages.TrimExcess();
				base.Books.Add(comicBook);
			}
		}

		private void watchFolders_Changed(object sender, SmartListChangedEventArgs<WatchFolder> e)
		{
			switch (e.Action)
			{
			case SmartListAction.Insert:
				e.Item.FileSystemWatcher.Renamed += watcherRenamedNotification;
				break;
			case SmartListAction.Remove:
				e.Item.FileSystemWatcher.Renamed -= watcherRenamedNotification;
				break;
			}
		}

		private void watcherRenamedNotification(object sender, RenamedEventArgs e)
		{
			try
			{
				if (Directory.Exists(e.FullPath))
				{
					foreach (ComicBook book in base.Books)
					{
						if (book.FilePath.StartsWith(e.OldFullPath, StringComparison.OrdinalIgnoreCase))
						{
							book.FilePath = e.FullPath + book.FilePath.Substring(e.OldFullPath.Length);
						}
					}
				}
				else if (string.Equals(Path.GetExtension(e.OldFullPath), Path.GetExtension(e.FullPath), StringComparison.OrdinalIgnoreCase))
				{
					ComicBook comicBook = base.Books[e.OldFullPath];
					if (comicBook != null)
					{
						comicBook.FilePath = e.FullPath;
					}
				}
			}
			catch (PathTooLongException)
			{
			}
		}

		public void AddToBlackList(string path)
		{
			using (ItemMonitor.Lock(blackList))
			{
				if (!blackList.Contains(path))
				{
					blackList.Add(path);
					base.IsDirty = true;
				}
			}
		}

		public bool IsBlacklisted(string path)
		{
			using (ItemMonitor.Lock(blackList))
			{
				return blackList.Contains(path);
			}
		}

		public void ClearBlackList()
		{
			using (ItemMonitor.Lock(blackList))
			{
				blackList.Clear();
			}
		}

		public IEnumerable<string> GetRecentFiles(int count)
		{
			return from x in (from x in base.Books
					where x != null
					orderby x.OpenedTime
					select x).Reverse().Take(count)
				select x.FilePath;
		}

		public string GetLastFile()
		{
			return GetRecentFiles(1).FirstOrDefault();
		}

		public void ConsolidateCustomThumbnails(string customThumbnailsPath)
		{
			try
			{
				HashSet<string> files = (Directory.Exists(customThumbnailsPath) ? new HashSet<string>(Directory.GetFiles(customThumbnailsPath), StringComparer.OrdinalIgnoreCase) : new HashSet<string>());
				Dictionary<string, IGrouping<string, ComicBook>> bookKeyGroups = base.Books.Where((ComicBook cb) => !string.IsNullOrEmpty(cb.CustomThumbnailKey)).GroupBy((ComicBook cb) => cb.CustomThumbnailKey, StringComparer.OrdinalIgnoreCase).ToDictionary((IGrouping<string, ComicBook> gr) => gr.Key, StringComparer.OrdinalIgnoreCase);
				Dictionary<string, IGrouping<string, StacksConfig.StackConfigItem>> stackKeyGroups = (from sc in (from cli in base.ComicLists.GetItems<ComicListItem>()
						select cli.Display.StackConfig into sc
						where sc != null
						select sc).SelectMany((StacksConfig sc) => sc.Configs)
					where !string.IsNullOrEmpty(sc.ThumbnailKey)
					select sc).GroupBy((StacksConfig.StackConfigItem sc) => sc.ThumbnailKey, StringComparer.OrdinalIgnoreCase).ToDictionary((IGrouping<string, StacksConfig.StackConfigItem> gr) => gr.Key, StringComparer.OrdinalIgnoreCase);
				foreach (string item in files.Where((string f) => !bookKeyGroups.ContainsKey(Path.GetFileName(f)) && !stackKeyGroups.ContainsKey(Path.GetFileName(f))))
				{
					FileUtility.SafeDelete(item);
				}
				foreach (string item2 in bookKeyGroups.Keys.Where((string k) => !files.Contains(Path.Combine(customThumbnailsPath, k))))
				{
					bookKeyGroups[item2].ForEach(delegate(ComicBook cb)
					{
						cb.CustomThumbnailKey = null;
					});
				}
				foreach (string item3 in stackKeyGroups.Keys.Where((string k) => !files.Contains(Path.Combine(customThumbnailsPath, k))))
				{
					stackKeyGroups[item3].ForEach(delegate(StacksConfig.StackConfigItem sc)
					{
						sc.ThumbnailKey = null;
					});
				}
			}
			catch (Exception)
			{
			}
		}

		public static ComicDatabase LoadXml(string file, Func<Stream, ComicDatabase> deserializer, Action<int> progress = null)
		{
			try
			{
				if (!File.Exists(file))
				{
					return CreateNew();
				}
				using (FileStream fileStream = File.OpenRead(file))
				{
					if (progress == null)
					{
						return deserializer(fileStream);
					}
					long len = fileStream.Length;
					long total = 0L;
					int percent = 0;
					ProgressStream progressStream = new ProgressStream(fileStream, baseStreamOwned: false);
					progressStream.DataRead += delegate(object sender, ProgressStreamReadEventArgs e)
					{
						total += e.Count;
						int num = (int)(total * 100 / len);
						if (num != percent)
						{
							percent = num;
							progress(percent);
						}
					};
					return deserializer(progressStream);
				}
			}
			catch (Exception inner)
			{
				throw new FileLoadException("Could not open the Database", inner);
			}
		}

		public static ComicDatabase LoadXml(string file, Action<int> progress = null)
		{
			return LoadXml(file, (Stream fs) => XmlUtility.Load<ComicDatabase>(fs, compressed: false), progress);
		}

		public static ComicDatabase CreateNew()
		{
			ComicDatabase comicDatabase = new ComicDatabase();
			comicDatabase.InitializeDefaultLists();
			return comicDatabase;
		}

		public void Save(string file, Action<Stream> serializer, bool commitCache = true)
		{
			string text = file + ".bak";
			try
			{
				if (commitCache)
				{
					base.ComicLists.GetItems<ComicListItem>().ForEach(delegate(ComicListItem cli)
					{
						cli.StoreCache();
					});
				}
				using (ItemMonitor.Lock(BlackList))
				{
					using (FileStream obj = File.Create(text))
					{
						serializer(obj);
					}
				}
			}
			catch
			{
				FileUtility.SafeDelete(text);
				throw;
			}
			File.Copy(text, file, overwrite: true);
			base.IsDirty = false;
		}

		public void SaveXml(string file, bool commitCache = true)
		{
			Save(file, delegate(Stream fs)
			{
				XmlUtility.Store(fs, this, compressed: false);
			}, commitCache);
		}

		public static ComicDatabase Attach(ComicDatabase copy, bool withBooks)
		{
			ComicDatabase comicDatabase = new ComicDatabase(enableWatchfolders: false)
			{
				Id = copy.Id,
				Name = copy.Name
			};
			if (withBooks)
			{
				comicDatabase.AttachBooks(new ComicBookCollection(copy.Books, updateDictionaries: false));
			}
			comicDatabase.AttachComicLists(copy.ComicLists);
			comicDatabase.blackList = copy.BlackList;
			comicDatabase.watchFolders = copy.WatchFolders;
			return comicDatabase;
		}

		public static void Backup(string backupFile, string databaseFile, string customThumbnailsFolder)
		{
			using (ZipFile zipFile = ZipFile.Create(backupFile))
			{
				zipFile.BeginUpdate();
				zipFile.SetComment("ComicRack Backup");
				zipFile.Add(databaseFile, Path.GetFileName(databaseFile));
				string[] files = Directory.GetFiles(customThumbnailsFolder);
				foreach (string text in files)
				{
					zipFile.Add(text, Path.Combine(ThumbnailsBackupFolder, Path.GetFileName(text)));
				}
				zipFile.CommitUpdate();
			}
		}

		public static void RestoreBackup(string backupFile, string databaseFile, string customThumbnailsFolder)
		{
			using (ZipFile zipFile = new ZipFile(backupFile))
			{
				try
				{
					using (FileStream destination = File.Create(databaseFile))
					{
						zipFile.GetInputStream(zipFile.GetEntry(BackupDatabaseName)).CopyTo(destination);
					}
				}
				catch (Exception)
				{
					FileUtility.SafeDelete(databaseFile);
					throw;
				}
				foreach (ZipEntry item in zipFile)
				{
					if (!item.Name.StartsWith(ThumbnailsBackupFolder))
					{
						continue;
					}
					string path = Path.Combine(customThumbnailsFolder, Path.GetFileName(item.Name));
					try
					{
						using (FileStream destination2 = File.Create(path))
						{
							zipFile.GetInputStream(item).CopyTo(destination2);
						}
					}
					catch (Exception)
					{
						FileUtility.SafeDelete(path);
						throw;
					}
				}
			}
		}
	}
}
