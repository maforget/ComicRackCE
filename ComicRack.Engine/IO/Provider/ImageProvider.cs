using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Text;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public abstract class ImageProvider : FileProviderBase, IImageProvider, IDisposable
	{
		private class ReaderWriterLockItem
		{
			public int Count;

			public readonly ReaderWriterLock Lock = new ReaderWriterLock();
		}

		private const int sourceLockTimeout = 60000;

		private readonly object workLock = new object();

		private Thread parser;

		private volatile string source = string.Empty;

		private volatile ReaderWriterLock sourceLock;

		private volatile ImageProviderStatus status;

		private readonly List<ProviderImageInfo> imageInfos = new List<ProviderImageInfo>();

		private static readonly Dictionary<string, ReaderWriterLockItem> rwLocks = new Dictionary<string, ReaderWriterLockItem>();

		public string Source
		{
			get
			{
				return source;
			}
			set
			{
				ReleaseLock(source);
				source = value;
				sourceLock = GetLock(source);
			}
		}

		public ImageProviderStatus Status => status;

		public bool IsRetrievingIndex => status <= ImageProviderStatus.Running;

		public bool IsIndexRetrievalCompleted => status > ImageProviderStatus.Running;

		public int Count
		{
			get
			{
				using (ItemMonitor.Lock(imageInfos))
				{
					return imageInfos.Count;
				}
			}
		}

		public virtual ImageProviderCapabilities Capabilities => ImageProviderCapabilities.Nothing;

		public virtual bool IsSlow => false;

		public event EventHandler<ImageIndexReadyEventArgs> ImageReady;

		public event EventHandler<IndexRetrievalCompletedEventArgs> IndexRetrievalCompleted;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				using (ItemMonitor.Lock(workLock))
				{
					try
					{
						ThreadUtility.Abort(parser, 30000);
					}
					finally
					{
						Source = string.Empty;
					}
				}
			}
			base.Dispose(disposing);
		}

		public void Open(string source, bool async)
		{
			if (status != 0)
			{
				throw new InvalidOperationException("Provider already initialized");
			}
			Source = source;
			status = ImageProviderStatus.Running;
			OnOpen();
			if (!async)
			{
				Parse();
				return;
			}
			parser = ThreadUtility.CreateWorkerThread("ImageProvider Parsing", Parse, ThreadPriority.BelowNormal);
			parser.Start();
		}

		public void Open(bool async)
		{
			Open(Source, async);
		}

		public Bitmap GetImage(int index)
		{
			using (ItemMonitor.Lock(workLock))
			{
				return RetrieveSourceImage(index);
			}
		}

		public byte[] GetByteImage(int index)
		{
			using (ItemMonitor.Lock(workLock))
			{
				return RetrieveSourceByteImage(index);
			}
		}

        public ExportImageContainer GetByteImageForExport(int index)
        {
            using (ItemMonitor.Lock(workLock))
            {
                return new ExportImageContainer()
				{
					Data = RetrieveSourceByteImage(index, keepSourceFormat: true),
                    NeedsToConvert = false
				};
            }
        }

        public ThumbnailImage GetThumbnail(int index)
		{
			using (ItemMonitor.Lock(workLock))
			{
				return RetrieveThumbnailImage(index);
			}
		}

		public ProviderImageInfo GetImageInfo(int index)
		{
			if (index < 0 || index >= Count)
			{
				return null;
			}
			using (ItemMonitor.Lock(imageInfos))
			{
				return imageInfos[index];
			}
		}

		public bool FastFormatCheck(string source)
		{
			using (ItemMonitor.Lock(workLock))
			{
				ReaderWriterLock @lock = GetLock(source);
				try
				{
					try
					{
						@lock.AcquireReaderLock(sourceLockTimeout);
					}
					catch (Exception)
					{
						return true;
					}
					try
					{
						return OnFastFormatCheck(source);
					}
					catch (Exception)
					{
						return true;
					}
					finally
					{
						@lock.ReleaseReaderLock();
					}
				}
				finally
				{
					ReleaseLock(source);
				}
			}
		}

		public void ChangeSourceLocation(string newSourceLocation)
		{
			Source = newSourceLocation;
		}

		public abstract string CreateHash();

		private void Parse()
		{
			using (LockSource(readOnly: true))
			{
				try
				{
					OnCheckSource();
					OnParse();
				}
				catch (Exception)
				{
				}
			}
			try
			{
				FireIndexRetrievalCompleted();
			}
			catch
			{
			}
		}

		private Bitmap RetrieveSourceImage(int index)
		{
			try
			{
				return BitmapExtensions.BitmapFromBytes(RetrieveSourceByteImage(index));
			}
			catch (Exception)
			{
				return null;
			}
		}

		private byte[] RetrieveSourceByteImage(int n, bool keepSourceFormat = false)
		{
			if (n < 0 || n >= Count)
			{
				return null;
			}
			byte[] array = null;
			using (LockSource(readOnly: true))
			{
				try
				{
					array = OnRetrieveSourceByteImage(n);
					if(!keepSourceFormat)
					{
						array = DjVuImage.ConvertToJpeg(array);
						array = WebpImage.ConvertToJpeg(array);
						array = HeifAvifImage.ConvertToJpeg(array);
                    }
					return array;
				}
				catch (Exception)
				{
					return array;
				}
			}
		}

        private ThumbnailImage RetrieveThumbnailImage(int n)
		{
			if (n < 0 || n >= Count)
			{
				return null;
			}
			ThumbnailImage result = null;
			using (LockSource(readOnly: true))
			{
				try
				{
					result = OnRetrieveThumbnailImage(n);
					return result;
				}
				catch (Exception)
				{
					return result;
				}
			}
		}

		protected static string CreateHashFromImageList(IEnumerable<ProviderImageInfo> images)
		{
			using (MemoryStream output = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(output))
				{
					foreach (ProviderImageInfo image in images)
					{
						binaryWriter.Write(image.Name);
						binaryWriter.Write(image.Size);
					}
					binaryWriter.Flush();
					binaryWriter.Seek(0, SeekOrigin.Begin);
					return Base32.ToBase32String(new SHA1Managed().ComputeHash(binaryWriter.BaseStream));
				}
			}
		}

		protected IDisposable LockSource(bool readOnly)
		{
			try
			{
				if (readOnly)
				{
					sourceLock.AcquireReaderLock(sourceLockTimeout);
					return new Disposer(delegate
					{
						sourceLock.ReleaseReaderLock();
					}, eatErrors: true);
				}
				sourceLock.AcquireWriterLock(sourceLockTimeout);
				return new Disposer(delegate
				{
					sourceLock.ReleaseWriterLock();
				}, eatErrors: true);
			}
			catch (Exception)
			{
				return null;
			}
		}

		protected virtual void OnOpen()
		{
		}

		protected virtual void OnCheckSource()
		{
			if (!File.Exists(Source))
			{
				throw new ArgumentException("Source file does not exist!");
			}
		}

		protected virtual bool OnFastFormatCheck(string source)
		{
			return true;
		}

		protected virtual ThumbnailImage OnRetrieveThumbnailImage(int index)
		{
			return null;
		}

		protected abstract void OnParse();

		protected abstract byte[] OnRetrieveSourceByteImage(int index);

		protected virtual void OnIndexReady(ImageIndexReadyEventArgs e)
		{
			if (this.ImageReady != null)
			{
				this.ImageReady(this, e);
			}
		}

		protected virtual void OnIndexRetrievalCompleted(IndexRetrievalCompletedEventArgs e)
		{
			if (this.IndexRetrievalCompleted != null)
			{
				this.IndexRetrievalCompleted(this, e);
			}
		}

		protected bool FireIndexReady(ProviderImageInfo ii)
		{
			using (ItemMonitor.Lock(imageInfos))
			{
				imageInfos.Add(ii);
			}
			ImageIndexReadyEventArgs imageIndexReadyEventArgs = new ImageIndexReadyEventArgs(Count - 1, ii);
			try
			{
				OnIndexReady(imageIndexReadyEventArgs);
			}
			catch (Exception)
			{
			}
			return !imageIndexReadyEventArgs.Cancel;
		}

		private void FireIndexRetrievalCompleted()
		{
			if (status == ImageProviderStatus.Running)
			{
				status = ((Count == 0) ? ImageProviderStatus.Error : ImageProviderStatus.Completed);
			}
			OnIndexRetrievalCompleted(new IndexRetrievalCompletedEventArgs(Status, Count));
		}

		private static ReaderWriterLock GetLock(string source)
		{
			if (string.IsNullOrEmpty(source))
			{
				return null;
			}
			using (ItemMonitor.Lock(rwLocks))
			{
				if (!rwLocks.TryGetValue(source, out var value))
				{
					ReaderWriterLockItem readerWriterLockItem2 = (rwLocks[source] = new ReaderWriterLockItem());
					value = readerWriterLockItem2;
				}
				value.Count++;
				return value.Lock;
			}
		}

		private static void ReleaseLock(string source)
		{
			using (ItemMonitor.Lock(rwLocks))
			{
				if (rwLocks.TryGetValue(source, out var value) && --value.Count == 0)
				{
					rwLocks.Remove(source);
				}
			}
		}
    }
}
