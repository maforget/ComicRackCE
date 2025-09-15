using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.Sync;

namespace cYo.Projects.ComicRack.Engine
{
	public class QueueManager : DisposableObject
	{
		public interface IPendingTasks
		{
			string TasksImageKey
			{
				get;
			}

			string Group
			{
				get;
			}

			string AbortCommandText
			{
				get;
			}

			Action Abort
			{
				get;
			}

			IList GetPendingItems();
		}

		private class PendingTasksInfo : IPendingTasks
		{
			private readonly Func<IList> handler;

			public string TasksImageKey
			{
				get;
				private set;
			}

			public string AbortCommandText
			{
				get;
				private set;
			}

			public Action Abort
			{
				get;
				private set;
			}

			public string Group
			{
				get;
				private set;
			}

			public PendingTasksInfo(string imageKey, string group, Func<IList> infoHandler, string abortCommandText, Action abort)
			{
				TasksImageKey = imageKey;
				handler = infoHandler;
				AbortCommandText = abortCommandText;
				Abort = abort;
				Group = group;
			}

			public IList GetPendingItems()
			{
				return handler();
			}
		}

		private class PendingTasksInfo<T> : PendingTasksInfo
		{
			public PendingTasksInfo(string imageKey, string group, ProcessingQueue<T> queue, Func<IProcessingItem<T>, object> descriptionHandler, string abortText = null, Action abort = null)
				: base(imageKey, group, () => queue.PendingItemInfos.Select(descriptionHandler).ToList(), abortText, abort)
			{
			}
		}

		private class TaskInfo : IProgressState
		{
			private readonly string text;

			public ProgressState State
			{
				get;
				set;
			}

			public int ProgressPercentage
			{
				get;
				set;
			}

			public string ProgressMessage
			{
				get;
				set;
			}

			public bool ProgressAvailable
			{
				get;
				set;
			}

			public bool Abort
			{
				get;
				set;
			}

			public TaskInfo(IProgressState ps, string text)
			{
				this.text = text;
				State = ps.State;
				ProgressAvailable = ps.ProgressAvailable;
				ProgressPercentage = ps.ProgressPercentage;
				ProgressMessage = ps.ProgressMessage;
			}

			public override string ToString()
			{
				string text = this.text;
				if (!string.IsNullOrEmpty(ProgressMessage))
				{
					text = text + " - " + ProgressMessage;
				}
				return text;
			}
		}

		private ComicScanner scanner;

		private readonly SmartList<ComicExporter> exportErrors = new SmartList<ComicExporter>();

		private readonly SmartList<DeviceSyncError> deviceSyncErrors = new SmartList<DeviceSyncError>();

		private static string refreshInfoQueueMessage;

		private static string writeInfoQueueMessage;

		private static string fastThumbanilQueueMessage;

		private static string slowThumbnailQueueMessage;

		private static string slowThumbnailQueueUnlimitedMessage;

		private static string getImageQueueMessage;

		private static string updateDynamicQueueMessage;

		private static string exportQueueMessage;

		private static string exportAbortText;

		private static string scanComicQueueMessage;

		private static string scanComicAbortText;

		private static string deviceSyncQueueMessage;

		private static string deviceSyncAbortText;

		private static string taskGroupLoadThumbnails;

		private static string taskGroupCreateThumbnails;

		private static string taskGroupLoadPages;

		private static string taskGroupCreatePages;

		private static string taskGroupReadInfo;

		private static string taskGroupWriteInfo;

		private static string taskGroupUpdateDynamic;

		private static string taskGroupExport;

		private static string taskGroupScanning;

		private static string taskGroupDeviceSync;

		public DatabaseManager DatabaseManager
		{
			get;
			private set;
		}

		public CacheManager CacheManager
		{
			get;
			private set;
		}

		public IComicUpdateSettings Settings
		{
			get;
			private set;
		}

		public IEnumerable<DeviceSyncSettings> Devices
		{
			get;
			private set;
		}

		public ComicScanner Scanner
		{
			get
			{
				if (scanner == null)
				{
					scanner = new ComicScanner(DatabaseManager.BookFactory);
					scanner.ScanNotify += delegate(object s, ComicScanNotifyEventArgs e)
					{
						if (this.ComicScanned != null)
						{
							this.ComicScanned(s, e);
						}
					};
				}
				return scanner;
			}
		}

		public ProcessingQueue<ComicBook> UpdateComicBookDynamicQueue
		{
			get;
			private set;
		}

		public ProcessingQueue<ComicBook> ExportComicsQueue
		{
			get;
			private set;
		}

		public ProcessingQueue<ComicBook> ReadComicBookInfoFileQueue
		{
			get;
			private set;
		}

		public ProcessingQueue<ComicBook> WriteComicBookInfoFileQueue
		{
			get;
			private set;
		}

		public ProcessingQueue<DeviceSyncSettings> DeviceSyncQueue
		{
			get;
			private set;
		}

		public SmartList<ComicExporter> ExportErrors => exportErrors;

		public SmartList<DeviceSyncError> DeviceSyncErrors => deviceSyncErrors;

		public int PendingComicConversions => ExportComicsQueue.Count;

		public bool IsInComicConversion => ExportComicsQueue.IsActive;

		public bool IsInComicFileRefresh
		{
			get
			{
				if (!ReadComicBookInfoFileQueue.IsActive)
				{
					return UpdateComicBookDynamicQueue.IsActive;
				}
				return true;
			}
		}

		public bool IsInComicFileUpdate => WriteComicBookInfoFileQueue.IsActive;

		public int PendingDeviceSyncs => DeviceSyncQueue.Count;

		public bool IsInDeviceSync => DeviceSyncQueue.IsActive;

		public bool IsActive
		{
			get
			{
				if (!IsInComicFileUpdate && !IsInComicConversion)
				{
					return IsInDeviceSync;
				}
				return true;
			}
		}

		public event EventHandler<ComicScanNotifyEventArgs> ComicScanned;

		public QueueManager(DatabaseManager databaseManager, CacheManager cacheManager, IComicUpdateSettings settings, IEnumerable<DeviceSyncSettings> devices)
		{
			DatabaseManager = databaseManager;
			CacheManager = cacheManager;
			Settings = settings;
			Devices = devices;
			UpdateComicBookDynamicQueue = new ProcessingQueue<ComicBook>("Update Dynamic Books", ThreadPriority.Lowest)
			{
				DefaultProcessingQueueAddMode = ProcessingQueueAddMode.AddToTop
			};
			ExportComicsQueue = new ProcessingQueue<ComicBook>("Export Books", ThreadPriority.Lowest);
			ReadComicBookInfoFileQueue = new ProcessingQueue<ComicBook>("Read Book File Information", ThreadPriority.Lowest);
			WriteComicBookInfoFileQueue = new ProcessingQueue<ComicBook>("Write Book File Information", ThreadPriority.Lowest)
			{
				DefaultProcessingQueueAddMode = ProcessingQueueAddMode.AddToTop
			};
			DeviceSyncQueue = new ProcessingQueue<DeviceSyncSettings>("Synchronizing Devices", ThreadPriority.Lowest);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (scanner != null)
				{
					Scanner.Dispose();
				}
				UpdateComicBookDynamicQueue.Dispose();
				ExportComicsQueue.Dispose();
				ReadComicBookInfoFileQueue.Dispose();
				WriteComicBookInfoFileQueue.Stop(abort: false, 60000);
				WriteComicBookInfoFileQueue.Dispose();
				DeviceSyncQueue.Dispose();
			}
			base.Dispose(disposing);
		}

		public void StartScan(bool all, bool removeMissing)
		{
			foreach (WatchFolder watchFolder in DatabaseManager.Database.WatchFolders)
			{
				Scanner.ScanFileOrFolder(watchFolder.Folder, all, removeMissing);
			}
		}

		public void AddBookToDynamicUpdate(ComicBook cb, bool refresh)
		{
			if (cb == null || !cb.IsDynamicSource)
			{
				return;
			}
			UpdateComicBookDynamicQueue.AddItem(cb, delegate(IAsyncResult ar)
			{
				if (refresh)
				{
					CacheManager.ImagePool.RemoveImages(cb.FilePath);
				}
				if (refresh || cb.EnableDynamicUpdate)
				{
					CacheManager.ImagePool.RefreshLastImage(cb.FilePath);
					cb.UpdateDynamicPageCount(refresh, ar as IProgressState);
				}
			});
		}

		public void ExportComic(ComicBook cb, ExportSetting setting, int sequence)
		{
			ExportComic(new ComicBook[1]
			{
				cb
			}, setting, sequence);
		}

		public void ExportComic(IEnumerable<ComicBook> cbs, ExportSetting setting, int sequence)
		{
			ComicBook kcb = cbs.FirstOrDefault();
			if (kcb == null || cbs.Any((ComicBook cb) => cb == null || !cb.EditMode.CanExport()))
			{
				return;
			}
			IProgressState ps = default(IProgressState);
			string outPath = default(string);
			ExportComicsQueue.AddItem(kcb, (IAsyncResult ar) =>
			{
				foreach (ComicBook cb in cbs)
				{
					cb.RefreshInfoFromFile();
				}
				ComicExporter comicExporter = new ComicExporter(cbs, setting, sequence);
				try
				{
					bool isLocal = kcb.EditMode.IsLocalComic();
					bool replace = setting.Target == ExportTarget.ReplaceSource;
					ps = ar as IProgressState;
					if (ps != null)
					{
						ps.ProgressAvailable = true;
						comicExporter.Progress += (object s, StorageProgressEventArgs e) =>
						{
							ps.ProgressPercentage = e.PercentDone;
							e.Cancel = ps.Abort;
						};
					}
					IEnumerable<string> source = (from cb in cbs
												  where cb.EditMode.IsLocalComic()
												  select cb.FilePath).ToArray();


					//Callback function to check if the file is already in the database, will be checked when calling Export
					comicExporter.FileIsInDatabase = (string targetPath, string sourceFile) =>
					{
						if (string.IsNullOrEmpty(targetPath))
							return false;

						// If the target path is the same as the source file, we assume it's not a duplicate
						return targetPath != sourceFile && DatabaseManager.Database.Books.FindItemByFile(targetPath) != null;
					};

					outPath = comicExporter.Export(CacheManager.ImagePool);
					if (outPath != null)
					{
						source = source.Where((string p) => !string.Equals(p, outPath, StringComparison.OrdinalIgnoreCase));
						if (isLocal && replace)
						{
							kcb.FilePath = outPath;
							kcb.RefreshFileProperties();
							kcb.SetInfo(comicExporter.ComicInfo, onlyUpdateEmpty: false);
							kcb.LastPageRead = kcb.LastPageRead.Clamp(0, kcb.PageCount - 1);
							kcb.CurrentPage = kcb.CurrentPage.Clamp(0, kcb.PageCount - 1);
							if (setting.ImageProcessingSource == ExportImageProcessingSource.FromComic)
							{
								kcb.ColorAdjustment = default(BitmapAdjustment);
							}
							foreach (string item in source)
							{
								ShellFile.DeleteFile(item);
								DatabaseManager.Database.Books.Remove(item);
							}
						}
						else
						{
							if (setting.DeleteOriginal && isLocal)
							{
								foreach (string item2 in source)
								{
									ShellFile.DeleteFile(item2);
									DatabaseManager.Database.Books.Remove(item2);
								}
							}
							if (setting.AddToLibrary || replace)
							{
								DatabaseManager.BookFactory.Create(outPath, CreateBookOption.AddToStorage, RefreshInfoOptions.DontReadInformation)?.SetInfo(comicExporter.ComicInfo, onlyUpdateEmpty: false);
							}
						}
					}
				}
				catch (OperationCanceledException) { } //Since we cancelled don't add it as an error, just ignore it.
				catch
				{
					ExportErrors.Add(comicExporter);
				}
			});
		}

		public void AddBookToRefreshComicData(ComicBook cb)
		{
			if (cb == null || !cb.EditMode.IsLocalComic())
			{
				return;
			}
			ReadComicBookInfoFileQueue.AddItem(cb, delegate
			{
				try
				{
					cb.RefreshInfoFromFile(RefreshInfoOptions.GetPageCount);
				}
				catch (Exception)
				{
				}
			});
		}

		public void AddBookToFileUpdate(ComicBook cb, bool alwaysWrite)
		{
			if (cb == null || !cb.IsLinked || !cb.ComicInfoIsDirty || !cb.FileInfoRetrieved || !Settings.UpdateComicFiles || !(Settings.AutoUpdateComicsFiles || alwaysWrite))
			{
				return;
			}
			WriteComicBookInfoFileQueue.AddItem(cb, delegate
			{
				if (cb.ComicInfoIsDirty && Settings.UpdateComicFiles && (Settings.AutoUpdateComicsFiles || alwaysWrite))
				{
					WriteInfoToFileWithCacheUpdate(cb);
				}
			});
			}

		public void AddBookToFileUpdate(ComicBook cb)
		{
			AddBookToFileUpdate(cb, alwaysWrite: false);
		}

		public void WriteInfoToFileWithCacheUpdate(ComicBook cb)
		{
			try
			{
				while (CacheManager.ImagePool.AreImagesPending(cb.FilePath))
				{
					Thread.Sleep(1000);
				}
				long oldSize = cb.FileSize;
				DateTime oldWrite = cb.FileModifiedTime;
				if (cb.WriteInfoToFile(withRefreshFileProperties: false))
				{
					CacheManager.ImagePool.Pages.UpdateKeys((ImageKey key) => key.IsSameFile(cb.FilePath, oldSize, oldWrite), delegate(ImageKey key)
					{
						key.UpdateFileInfo();
					});
					CacheManager.ImagePool.Thumbs.UpdateKeys((ImageKey key) => key.IsSameFile(cb.FilePath, oldSize, oldWrite), delegate(ImageKey key)
					{
						key.UpdateFileInfo();
					});
					cb.RefreshFileProperties();
					cb.ComicInfoIsDirty = false;
				}
			}
			catch (Exception)
			{
			}
		}

		public bool SynchronizeDevices()
		{
			foreach (DeviceSyncSettings device in Devices)
			{
				SynchronizeDevice(device);
			}
			return Devices.Any();
		}

		public void SynchronizeDevice(string key, IPAddress address)
		{
			SynchronizeDevice(Devices.FirstOrDefault((DeviceSyncSettings s) => s.DeviceKey == key), address);
		}

		public void SynchronizeDevice(DeviceSyncSettings dss, IPAddress address = null)
		{
			if (dss == null)
			{
				return;
			}
			DeviceSyncSettings ssc = new DeviceSyncSettings(dss);
			ComicBookContainer library = new ComicBookContainer();
			library.Books.AddRange(DatabaseManager.Database.Books);
			DeviceSyncQueue.AddItem(ssc, delegate(IAsyncResult ar)
			{
				try
				{
					ISyncProvider syncProvider;
					if (address == null)
					{
						syncProvider = DeviceSyncFactory.Create(ssc.DeviceKey);
					}
					else
					{
						ISyncProvider syncProvider2 = new WirelessSyncProvider(address, ssc.DeviceKey);
						syncProvider = syncProvider2;
					}
					ISyncProvider syncProvider3 = syncProvider;
					if (syncProvider3 != null)
					{
						StorageSync storageSync = new StorageSync(syncProvider3);
						storageSync.Error = (EventHandler<StorageSync.SyncErrorEventArgs>)Delegate.Combine(storageSync.Error, (EventHandler<StorageSync.SyncErrorEventArgs>)delegate(object s, StorageSync.SyncErrorEventArgs e)
						{
							DeviceSyncErrors.Add(new DeviceSyncError(ssc.DeviceName, e.Message));
						});
						storageSync.Synchronize(ssc, library, DatabaseManager.Database.ComicLists, CacheManager.ImagePool, ar as IProgressState);
					}
				}
				catch (Exception)
				{
				}
			});
		}

		public IEnumerable<IPendingTasks> GetQueues()
		{
			List<IPendingTasks> list = new List<IPendingTasks>();
			if (exportQueueMessage == null)
			{
				exportQueueMessage = TR.Messages["ExportQueueMessage", "Export Book '{0}'"];
				refreshInfoQueueMessage = TR.Messages["RefreshInfoQueueMessage", "Refresh information for Book '{0}'"];
				writeInfoQueueMessage = TR.Messages["WriteInfoQueueMessage", "Write information to Book file '{0}'"];
				fastThumbanilQueueMessage = TR.Messages["FastThumbanilQueueMessage", "Retrieve cached thumbnail for page {0} in file '{1}'"];
				slowThumbnailQueueMessage = TR.Messages["SlowThumbnailQueueMessage", "Create thumbnail for page {0} in file '{1}'"];
				slowThumbnailQueueUnlimitedMessage = TR.Messages["SlowThumbnailQueueUnlimitedMessage", "Create thumbnail for cover in file '{0}'"];
				getImageQueueMessage = TR.Messages["GetImageQueueMessage", "Get page {0} in file '{1}'"];
				scanComicQueueMessage = TR.Messages["ScanComicQueueMessage", "Scanning '{0}'"];
				deviceSyncQueueMessage = TR.Messages["DeviceSyncQueueMessage", "Syncing Device '{0}'"];
				updateDynamicQueueMessage = TR.Messages["UpdateComicDynamicMessage", "Updating Web Comic '{0}'"];
				exportAbortText = TR.Messages["AbortExport", "Abort Export"];
				scanComicAbortText = TR.Messages["AbortScan", "Abort Scanning"];
				deviceSyncAbortText = TR.Messages["AbortDeviceSync", "Abort syncinc Devices"];
				taskGroupLoadThumbnails = TR.Messages["TaskGroupLoadThumbnails", "Load Thumbnails"];
				taskGroupCreateThumbnails = TR.Messages["TaskGroupCreateThumbnails", "Create Thumbnails"];
				taskGroupLoadPages = TR.Messages["TaskGroupLoadPages", "Load Pages"];
				taskGroupCreatePages = TR.Messages["TaskGroupCreatePages", "Create Pages"];
				taskGroupReadInfo = TR.Messages["TaskGroupReadInfo", "Read Info"];
				taskGroupWriteInfo = TR.Messages["TaskGroupWriteInfo", "Write Info"];
				taskGroupUpdateDynamic = TR.Messages["TaskGroupUpdateDynamic", "Update Web Comics"];
				taskGroupExport = TR.Messages["TaskGroupExport", "Export Books"];
				taskGroupScanning = TR.Messages["TaskGroupScanning", "Scanning"];
				taskGroupDeviceSync = TR.Messages["TaskGroupDeviceSync", "Syncing Devices"];
			}
			list.Add(new PendingTasksInfo<ImageKey>("ReadPagesAnimation", taskGroupLoadThumbnails, CacheManager.ImagePool.FastThumbnailQueue, (IProcessingItem<ImageKey> ik) => new TaskInfo(ik, StringUtility.Format(fastThumbanilQueueMessage, ik.Item.Index + 1, Path.GetFileName(ik.Item.Location)))));
			list.Add(new PendingTasksInfo<ImageKey>("ReadPagesAnimation", taskGroupCreateThumbnails, CacheManager.ImagePool.SlowThumbnailQueue, (IProcessingItem<ImageKey> ik) => new TaskInfo(ik, StringUtility.Format(slowThumbnailQueueMessage, ik.Item.Index + 1, Path.GetFileName(ik.Item.Location)))));
			list.Add(new PendingTasksInfo<ImageKey>("ReadPagesAnimation", taskGroupCreateThumbnails, CacheManager.ImagePool.SlowThumbnailQueueUnlimited, (IProcessingItem<ImageKey> ik) => new TaskInfo(ik, StringUtility.Format(slowThumbnailQueueUnlimitedMessage, Path.GetFileName(ik.Item.Location))), "Abort Cover Generation", CacheManager.ImagePool.SlowThumbnailQueueUnlimited.Clear));
			list.Add(new PendingTasksInfo<ImageKey>("ReadPagesAnimation", taskGroupCreatePages, CacheManager.ImagePool.SlowPageQueue, (IProcessingItem<ImageKey> ik) => new TaskInfo(ik, StringUtility.Format(getImageQueueMessage, ik.Item.Index + 1, Path.GetFileName(ik.Item.Location)))));
			list.Add(new PendingTasksInfo<ImageKey>("ReadPagesAnimation", taskGroupLoadPages, CacheManager.ImagePool.FastPageQueue, (IProcessingItem<ImageKey> ik) => new TaskInfo(ik, StringUtility.Format(getImageQueueMessage, ik.Item.Index + 1, Path.GetFileName(ik.Item.Location)))));
			list.Add(new PendingTasksInfo<ComicBook>("ReadInfoAnimation", taskGroupReadInfo, ReadComicBookInfoFileQueue, (IProcessingItem<ComicBook> cb) => new TaskInfo(cb, StringUtility.Format(refreshInfoQueueMessage, cb.Item.Caption))));
			list.Add(new PendingTasksInfo<ComicBook>("UpdateInfoAnimation", taskGroupWriteInfo, WriteComicBookInfoFileQueue, (IProcessingItem<ComicBook> cb) => new TaskInfo(cb, StringUtility.Format(writeInfoQueueMessage, cb.Item.Caption))));
			list.Add(new PendingTasksInfo<ComicBook>("ReadInfoAnimation", taskGroupUpdateDynamic, UpdateComicBookDynamicQueue, (IProcessingItem<ComicBook> cb) => new TaskInfo(cb, StringUtility.Format(updateDynamicQueueMessage, cb.Item.Caption))));
			list.Add(new PendingTasksInfo<ComicBook>("ExportAnimation", taskGroupExport, ExportComicsQueue, (IProcessingItem<ComicBook> cb) => new TaskInfo(cb, StringUtility.Format(exportQueueMessage, cb.Item.Caption)), exportAbortText, ExportComicsQueue.Clear));
			list.Add(new PendingTasksInfo<DeviceSyncSettings>("DeviceSyncAnimation", taskGroupDeviceSync, DeviceSyncQueue, (IProcessingItem<DeviceSyncSettings> sds) => new TaskInfo(sds, StringUtility.Format(deviceSyncQueueMessage, sds.Item.DeviceName)), deviceSyncAbortText, DeviceSyncQueue.Clear));
			list.Add(new PendingTasksInfo("ScanAnimation", taskGroupScanning, () => Scanner.IsScanning ? new string[1]
			{
				StringUtility.Format(scanComicQueueMessage, Scanner.CurrentLocation)
			} : new string[0], scanComicAbortText, delegate
			{
				Scanner.Stop(clearQueue: true);
			}));
			return list;
		}
	}
}
