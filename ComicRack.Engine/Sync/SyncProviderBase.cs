using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Cache;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public abstract class SyncProviderBase : ISyncProvider
	{
		public class SyncReadingList
		{
			private readonly List<Guid> books = new List<Guid>();

			[XmlAttribute]
			public string Name
			{
				get;
				set;
			}

			[DefaultValue("")]
			public string Description
			{
				get;
				set;
			}

			[XmlArrayItem("Id")]
			public List<Guid> Books => books;

			public SyncReadingList()
			{
				Description = string.Empty;
			}

			public SyncReadingList(ComicIdListItem list, Func<Guid, bool> validate = null)
			{
				Name = list.Name;
				Description = list.Description;
				books.AddRange(list.BookIds.Where((Guid id) => validate == null || validate(id)));
			}
		}

		public class SyncInformation
		{
			private readonly List<SyncReadingList> lists = new List<SyncReadingList>();

			public string Name
			{
				get;
				set;
			}

			public int Version
			{
				get;
				set;
			}

			[XmlArrayItem("List")]
			public List<SyncReadingList> Lists => lists;

			public SyncInformation()
			{
				Version = 1;
				Name = "ComicRack";
			}
		}

		public const int MinimumAndroidFreeVersion = 100;

		public const int MinimumAndroidFullVersion = 89;

		public const int MinimumIOsVersion = 1;

		public const string SyncInformationFile = "sync_information.xml";

		public const string MarkerFile = "comicrack.ini";

		public const string SyncFormatExtension = ".cbp";

		private readonly ProcessingQueue<ComicBook> writeQueue = new ProcessingQueue<ComicBook>("Write books to Device");

		private Exception pendingException;

		private readonly object deviceAccessLock = new object();

		protected ComicBookCollection BooksOnDevice
		{
			get;
			set;
		}

		public DeviceInfo Device
		{
			get;
			private set;
		}

		protected abstract void OnStart();

		protected abstract void OnCompleted();

		protected abstract bool FileExists(string file);

		protected abstract void WriteFile(string file, Stream data);

		protected abstract Stream ReadFile(string file);

		protected abstract void DeleteFile(string fileName);

		protected abstract long GetFreeSpace();

		protected abstract IEnumerable<string> GetFileList();

		protected virtual bool OnProgress(int percent)
		{
			return true;
		}

		public virtual IEnumerable<ComicBook> GetBooks()
		{
			ComicBookCollection comicBookCollection = new ComicBookCollection();
			string[] array = GetFileList().Where(IsValidSyncFile).ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				ComicBook comicBook = DeserializeBook(array[i]);
				if (comicBook != null && comicBookCollection.FindItemById(comicBook.Id) == null)
				{
					comicBookCollection.Add(comicBook);
					continue;
				}
				DeleteFile(array[i]);
				DeleteFile(MakeSidecar(array[i]));
			}
			return BooksOnDevice = comicBookCollection;
		}

		public virtual void ValidateDevice(DeviceInfo device)
		{
			int num;
			switch (device.Edition)
			{
			case SyncAppEdition.AndroidFree:
				device.BookSyncLimit = 100;
				num = MinimumAndroidFreeVersion;
				break;
			case SyncAppEdition.AndroidFull:
				num = MinimumAndroidFullVersion;
				break;
			case SyncAppEdition.iOS:
				num = MinimumIOsVersion;
				break;
			default:
				num = int.MaxValue;
				break;
			}
			if (device.Version < num)
			{
				throw new StorageSync.FatalSyncException("Invalid device");
			}
		}

		public void Start()
		{
			OnStart();
		}

		public void Add(ComicBook book, bool optimize, IPagePool pagePool, Action workingCallback, Action sendCallback, Action completedCallback)
		{
			if (pendingException != null)
			{
				Exception ex = pendingException;
				pendingException = null;
				throw ex;
			}
			book = (ComicBook)book.Clone();
			book.Series = book.ShadowSeries;
			book.Title = book.ShadowTitle;
			book.Volume = book.ShadowVolume;
			book.Number = book.ShadowNumber;
			book.Count = book.ShadowCount;
			book.Format = book.ShadowFormat;
			ComicBook existing = BooksOnDevice.FindItemById(book.Id);
			if (existing != null && PagesAreSame(existing, book))
			{
				if (ContentIsSame(existing, book) && !existing.ComicInfoIsDirty)
				{
					if (completedCallback != null)
					{
						writeQueue.AddItem(book, delegate
						{
							completedCallback();
						});
					}
					return;
				}
				for (int i = 0; i < book.Pages.Count; i++)
				{
					ComicPageInfo page = book.GetPage(i);
					existing.UpdatePageType(i, page.PageType);
					existing.UpdateBookmark(i, page.Bookmark);
				}
				book.Pages.Clear();
				book.Pages.AddRange(existing.Pages);
				writeQueue.AddItem(book, delegate
				{
					using (ItemMonitor.Lock(deviceAccessLock))
					{
						WriteBookInfo(book, Path.GetFileName(existing.FilePath));
					}
					if (completedCallback != null)
					{
						completedCallback();
					}
				});
				return;
			}
			ExportSetting portableFormat = GetPortableFormat(Device, optimize);
			string temp = EngineConfiguration.Default.GetTempFileName();
			string fileBaseName = portableFormat.GetTargetFileName(book, 0);
			portableFormat.ForcedName = Path.GetFileName(temp);
			portableFormat.TargetFolder = Path.GetDirectoryName(temp);
			while (writeQueue.Count > Math.Max(EngineConfiguration.Default.SyncQueueLength, 5))
			{
				Thread.Sleep(1000);
			}
			ComicExporter export = new ComicExporter(ListExtensions.AsEnumerable<ComicBook>(book), portableFormat, 0);
			export.Progress += delegate
			{
				if (writeQueue.Count == 0 && workingCallback != null)
				{
					workingCallback();
				}
			};
			try
			{
				export.Export(pagePool);
			}
			catch (Exception ex2)
			{
				FileUtility.SafeDelete(temp);
				throw new InvalidOperationException(book.Caption + ": " + ex2.Message, ex2);
			}
			writeQueue.AddItem(book, delegate
			{
				using (ItemMonitor.Lock(deviceAccessLock))
				{
					try
					{
						if (pendingException == null)
						{
							if (completedCallback != null)
							{
								sendCallback();
							}
							if (existing != null)
							{
								Remove(existing);
							}
							long num = GetFreeSpace() - (long)EngineConfiguration.Default.FreeDeviceMemoryMB * 1024L * 1024;
							if (FileUtility.GetSize(temp) > num)
							{
								throw new StorageSync.DeviceOutOfSpaceException(StringUtility.Format(TR.Messages["DeviceOutOfSpace", "Device '{0}' does not have enough free space"], Device.Name));
							}
							string uniqueFileName = GetUniqueFileName(fileBaseName);
							using (FileStream data = File.OpenRead(temp))
							{
								WriteFile(uniqueFileName, data);
							}
							book.Pages.Clear();
							book.Pages.AddRange(export.ComicInfo.Pages);
							book.FilePath = uniqueFileName;
							WriteBookInfo(book, uniqueFileName);
							BooksOnDevice.Add(book);
							if (completedCallback != null)
							{
								completedCallback();
							}
						}
					}
					catch (Exception ex3)
					{
						pendingException = ex3;
					}
					finally
					{
						FileUtility.SafeDelete(temp);
					}
				}
			});
		}

		public void WaitForWritesCompleted()
		{
			while (writeQueue.IsActive)
			{
				Thread.Sleep(1000);
			}
			if (pendingException != null)
			{
				Exception ex = pendingException;
				pendingException = null;
				throw ex;
			}
		}

		public void Remove(ComicBook book)
		{
			ComicBook comicBook = BooksOnDevice.FindItemById(book.Id);
			if (comicBook != null)
			{
				DeleteFile(comicBook.FilePath);
				DeleteFile(MakeSidecar(comicBook.FilePath));
				BooksOnDevice.Remove(comicBook);
			}
		}

		public void SetLists(IEnumerable<ComicIdListItem> lists)
		{
			SyncInformation syncInformation = new SyncInformation();
			syncInformation.Lists.AddRange(from cli in lists
				select new SyncReadingList(cli) into cli
				where cli.Books.Count > 0
				select cli);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				XmlUtility.Store(memoryStream, syncInformation, compressed: false);
				memoryStream.Position = 0L;
				WriteFile(SyncInformationFile, memoryStream);
			}
		}

		public bool Progress(int progress)
		{
			ThreadUtility.KeepAlive();
			return OnProgress(progress);
		}

		public void Completed()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Stream stream = ReadFile(MarkerFile))
				{
					stream.CopyTo(memoryStream);
				}
				memoryStream.Position = 0L;
				WriteFile(MarkerFile, memoryStream);
			}
			OnCompleted();
		}

		private void WriteBookInfo(ComicBook book, string fileName)
		{
			book.ComicInfoIsDirty = false;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				book.SerializeFull(memoryStream);
				memoryStream.Position = 0L;
				WriteFile(MakeSidecar(fileName), memoryStream);
			}
		}

		protected static bool IsValidSyncFile(string file)
		{
			if (!string.IsNullOrEmpty(file))
			{
				return string.Equals(Path.GetExtension(file), SyncFormatExtension, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		protected ComicBook DeserializeBook(string comicPath, Stream inputStream = null)
		{
			string text = MakeSidecar(comicPath);
			try
			{
				using (Stream stream = inputStream ?? ReadFile(text))
				{
					ComicBook comicBook = ComicBook.DeserializeFull(stream);
					comicBook.FilePath = comicPath;
					return comicBook;
				}
			}
			catch (Exception)
			{
				if (inputStream == null)
				{
					DeleteFile(text);
					DeleteFile(comicPath);
				}
				return null;
			}
		}

		protected bool ReadMarkerFile(string deviceKey)
		{
			try
			{
				using (Stream stream = ReadFile(MarkerFile))
				{
					Dictionary<string, string> values = IniFile.GetValues(new StreamReader(stream));
					Device = new DeviceInfo(values);
				}
			}
			catch (Exception)
			{
				return false;
			}
			if (deviceKey != null)
			{
				return Device.Key == deviceKey;
			}
			return true;
		}

		private string GetUniqueFileName(string baseName)
		{
			baseName = Path.GetFileNameWithoutExtension(baseName);
			string text = baseName + SyncFormatExtension;
			if (FileExists(text))
			{
				int num = 1;
				while (FileExists(text = $"{baseName} ({++num}){SyncFormatExtension}"))
				{
				}
			}
			return text;
		}

		public static bool PagesAreSame(ComicBook device, ComicBook library, bool withBookmarks = false)
		{
			ComicPageInfo[] array = device.Pages.ToArray();
			ComicPageInfo[] array2 = library.Pages.ToArray();
			if (array.Length < array2.Length)
			{
				return false;
			}
			for (int i = 0; i < Math.Min(array.Length, array2.Length); i++)
			{
				ComicPageInfo comicPageInfo = array[i];
				ComicPageInfo comicPageInfo2 = array2[i];
				if (comicPageInfo.PageType != comicPageInfo2.PageType || (comicPageInfo2.ImageHeight != 0 && comicPageInfo2.ImageWidth != 0 && comicPageInfo.IsDoublePage != comicPageInfo2.IsDoublePage) || comicPageInfo.PagePosition != comicPageInfo2.PagePosition || (withBookmarks && comicPageInfo.Bookmark != comicPageInfo2.Bookmark))
				{
					return false;
				}
			}
			return true;
		}

		public static bool ContentIsSame(ComicBook a, ComicBook b)
		{
			if (a.IsSameContent(b, withPages: false) && a.Rating == b.Rating && a.OpenedCount == b.OpenedCount && a.LastPageRead == b.LastPageRead && a.OpenedTime == b.OpenedTime && a.AddedTime == b.AddedTime && a.ReleasedTime == b.ReleasedTime)
			{
				return PagesAreSame(a, b, withBookmarks: true);
			}
			return false;
		}

		public static string MakeSidecar(string fileName)
		{
			return fileName + ".xml";
		}

		public static ExportSetting GetPortableFormat(DeviceInfo device, bool optimized)
		{
			bool flag = device.Capabilites.HasFlag(DeviceCapabilites.WebP);
			ExportSetting exportSetting = new ExportSetting
			{
				Naming = ExportNaming.Caption,
				Target = ExportTarget.NewFolder,
				FormatId = 2,
				PageType = ((!flag || !EngineConfiguration.Default.SyncWebP) ? StoragePageType.Jpeg : StoragePageType.Webp),
				EmbedComicInfo = false,
				AddKeyToPageInfo = true,
				Overwrite = true,
				RemovePages = false,
				Resampling = EngineConfiguration.Default.SyncResamping
			};
			if (optimized)
			{
				exportSetting.PageCompression = EngineConfiguration.Default.SyncOptimizeQuality;
				exportSetting.CreateThumbnails = EngineConfiguration.Default.SyncCreateThumbnails;
				exportSetting.PageResize = StoragePageResize.Height;
				exportSetting.PageHeight = EngineConfiguration.Default.SyncOptimizeMaxHeight;
				exportSetting.DontEnlarge = true;
				if (flag)
				{
					exportSetting.PageType = ((!EngineConfiguration.Default.SyncOptimizeWebP) ? StoragePageType.Jpeg : StoragePageType.Webp);
				}
				if (EngineConfiguration.Default.SyncOptimizeSharpen)
				{
					exportSetting.ImageProcessingSource = ExportImageProcessingSource.FromComic;
					exportSetting.ImageProcessing = new BitmapAdjustment(0f, 0f, 0f, 0f, BitmapAdjustmentOptions.None, 1);
				}
			}
			return exportSetting;
		}
	}
}
