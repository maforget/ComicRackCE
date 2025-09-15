using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicScanner : DisposableObject
	{
		private Thread scanningThread;

		private readonly ComicBookFactory factory;

		private bool scanCompleted;

		private readonly SmartList<ScanItem> scanQueue = new SmartList<ScanItem>();

		private volatile ComicScanOptions scanOptions;

		private volatile string currentLocation;

		private volatile bool abortScanning;

		public bool IsScanning
		{
			get
			{
				if (scanningThread != null && scanningThread.IsAlive)
				{
					return !scanCompleted;
				}
				return false;
			}
		}

		public SmartList<ScanItem> ScanQueue => scanQueue;

		public ComicScanOptions ScanOptions
		{
			get
			{
				return scanOptions;
			}
			set
			{
				scanOptions = value;
			}
		}

		public string CurrentLocation => currentLocation;

		public event EventHandler<ComicScanNotifyEventArgs> ScanNotify;

		public ComicScanner(ComicBookFactory factory)
		{
			this.factory = factory;
		}

		public void Scan()
		{
			if (!IsScanning)
			{
				scanCompleted = false;
				scanningThread = ThreadUtility.CreateWorkerThread("Book Scanner", ScanFolderQueue, ThreadPriority.Lowest);
				scanningThread.Start();
			}
		}

		public void ScanFileOrFolder(string fileOrFolder, bool all, bool removeMissing, bool forceRefreshInfo = false)
		{
			if (!string.IsNullOrEmpty(fileOrFolder))
			{
				scanQueue.Add(new ScanItemFileOrFolder(fileOrFolder, all, removeMissing, forceRefreshInfo));
				Scan();
			}
		}

		public void ScanFilesOrFolders(IEnumerable<string> filesOrFolders, bool all, bool removeMissing, bool forceRefreshInfo = false)
		{
			filesOrFolders.ForEach(delegate(string folder)
			{
				ScanFileOrFolder(folder, all, removeMissing, forceRefreshInfo);
			});
		}

		public void Stop(bool clearQueue)
		{
			if (IsScanning)
			{
				abortScanning = true;
				if (!scanningThread.Join(10000))
				{
					scanningThread.Abort();
					scanningThread.Join();
				}
			}
			if (clearQueue)
			{
				scanQueue.Clear();
			}
			scanningThread = null;
		}

		private void ScanFolderQueue()
		{
			bool flag = false;
			bool flag2 = false;
			try
			{
				scanCompleted = false;
				while (scanQueue.Count > 0)
				{
					ScanItem scanItem = scanQueue[0];
					flag2 |= scanItem.AutoRemove;
					try
					{
						foreach (string scanFile in scanItem.GetScanFiles())
						{
							try
							{
								currentLocation = Path.GetFullPath(scanFile);
								if (File.Exists(scanFile))
								{
									ComicScanNotifyEventArgs comicScanNotifyEventArgs = new ComicScanNotifyEventArgs(scanFile);
									OnScanNotify(comicScanNotifyEventArgs);
									if (comicScanNotifyEventArgs.Cancel || abortScanning)
									{
										flag = comicScanNotifyEventArgs.ClearQueue;
										return;
									}
									if (!comicScanNotifyEventArgs.IgnoreFile)
									{
										OnProcessScannedFile(scanFile, scanOptions, scanItem.ForceRefreshInfo);
									}
								}
							}
							catch (Exception)
							{
							}
						}
					}
					catch
					{
					}
					scanQueue.Remove(scanItem);
				}
				if (!flag2)
				{
					return;
				}
				DriveChecker driveChecker = new DriveChecker();
				List<ComicBook> list = null;
				ComicBook[] array = factory.Storage.ToArray();
				foreach (ComicBook comicBook in array)
				{
					if (!comicBook.IsLinked)
					{
						continue;
					}
					string filePath = comicBook.FilePath;
					if (driveChecker.IsConnected(filePath) && !File.Exists(filePath))
					{
						if (list == null)
						{
							list = new List<ComicBook>();
						}
						list.Add(comicBook);
					}
					ComicScanNotifyEventArgs comicScanNotifyEventArgs2 = new ComicScanNotifyEventArgs(filePath);
					OnScanNotify(comicScanNotifyEventArgs2);
					if (comicScanNotifyEventArgs2.Cancel || abortScanning)
					{
						flag = comicScanNotifyEventArgs2.ClearQueue;
						return;
					}
				}
				if (list != null)
				{
					factory.Storage.RemoveRange(list);
				}
			}
			catch (ThreadAbortException)
			{
			}
			finally
			{
				scanCompleted = true;
				OnScanNotify(new ComicScanNotifyEventArgs(string.Empty));
				if (flag)
				{
					scanQueue.Clear();
				}
			}
		}

		protected virtual void OnScanNotify(ComicScanNotifyEventArgs e)
		{
			if (this.ScanNotify != null)
			{
				this.ScanNotify(this, e);
			}
		}

		protected virtual void OnProcessScannedFile(string file, ComicScanOptions scanOptions, bool forceRefreshInfo = false)
		{
			RefreshInfoOptions refreshInfoOptions = forceRefreshInfo ? RefreshInfoOptions.ForceRefresh : RefreshInfoOptions.DontReadInformation;
			ComicBook comicBook = factory.Create(file, CreateBookOption.DoNotAdd, refreshInfoOptions);
			if (comicBook != null && !comicBook.IsInContainer && !forceRefreshInfo)
			{
				ComicBook comicBook2 = factory.Storage.FindItemByFileNameSize(comicBook.FilePath);
				if (comicBook2 != null && !File.Exists(comicBook2.FilePath))
				{
					comicBook2.FilePath = comicBook.FilePath;
				}
				else if (factory.Storage[comicBook.FilePath] == null)
				{
					comicBook.AddedTime = DateTime.Now;
					comicBook.RefreshInfoFromFile();
					factory.Storage.Add(comicBook);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stop(clearQueue: true);
			}
			base.Dispose(disposing);
		}
	}
}
