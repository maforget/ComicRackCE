using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.Properties;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public class ImagePool : DisposableObject, IPagePool, IThumbnailPool, ICustomThumbnail
	{
		private const int DefaultThumbCount = 20;

		private const int DefaultThumbSize = 5242880;

		private const int DefaultPageCount = 5;

		private readonly ThumbnailManager thumbs;

		private readonly ImageManager pages;

		private readonly ProcessingQueue<ImageKey> fastThumbnailQueue;

		private readonly ProcessingQueue<ImageKey> slowThumbnailQueue;

		private readonly ProcessingQueue<ImageKey> slowThumbnailQueueUnlimited;

		private readonly ProcessingQueue<ImageKey> slowPageQueue;

		private readonly ProcessingQueue<ImageKey> fastPageQueue;

		private bool cacheThumbnailPages = EngineConfiguration.Default.CacheThumbnailPages;

		private string customThumbnailFolder;

		public ThumbnailManager Thumbs => thumbs;

		public ImageManager Pages => pages;

		public ProcessingQueue<ImageKey> FastThumbnailQueue => fastThumbnailQueue;

		public ProcessingQueue<ImageKey> SlowThumbnailQueue => slowThumbnailQueue;

		public ProcessingQueue<ImageKey> SlowThumbnailQueueUnlimited => slowThumbnailQueueUnlimited;

		public ProcessingQueue<ImageKey> SlowPageQueue => slowPageQueue;

		public ProcessingQueue<ImageKey> FastPageQueue => fastPageQueue;

		public bool CacheThumbnailPages
		{
			get
			{
				return cacheThumbnailPages;
			}
			set
			{
				cacheThumbnailPages = value;
			}
		}

		public bool IsWorking
		{
			get
			{
				if (!slowPageQueue.IsActive && !slowThumbnailQueue.IsActive && !slowThumbnailQueueUnlimited.IsActive && !fastPageQueue.IsActive)
				{
					return fastThumbnailQueue.IsActive;
				}
				return true;
			}
		}

		public int MaximumMemoryItems => pages.MemoryCache.ItemCapacity;

		public string CustomThumbnailFolder
		{
			get
			{
				return customThumbnailFolder;
			}
			set
			{
				if (!(value == customThumbnailFolder))
				{
					if (!Directory.Exists(value))
					{
						Directory.CreateDirectory(value);
					}
					customThumbnailFolder = value;
				}
			}
		}

		public event EventHandler<ResourceThumbnailEventArgs> RequestResourceThumbnail;

		public event EventHandler<CacheItemEventArgs<ImageKey, PageImage>> PageCached;

		public event EventHandler<CacheItemEventArgs<ImageKey, ThumbnailImage>> ThumbnailCached;

		public ImagePool()
			: this(DefaultThumbCount, DefaultThumbSize, DefaultPageCount)
		{
		}

		public ImagePool(int thumbCount, long thumbSize, int pageCount)
		{
			thumbs = new ThumbnailManager(thumbCount, thumbSize);
			pages = new ImageManager(pageCount);
			int threadCount = Environment.ProcessorCount.Clamp(1, EngineConfiguration.Default.MaximumQueueThreads);
			fastPageQueue = new ProcessingQueue<ImageKey>("Background Fast Page Queue", ThreadPriority.BelowNormal, pageCount * 2);
			slowPageQueue = new ProcessingQueue<ImageKey>(threadCount, "Background Slow Page Queue", ThreadPriority.BelowNormal, pageCount * 2);
			fastThumbnailQueue = new ProcessingQueue<ImageKey>("Background Fast Thumbnails Queue", ThreadPriority.Lowest, 256);
			slowThumbnailQueue = new ProcessingQueue<ImageKey>(threadCount, "Background Slow Thumbnails Queue", ThreadPriority.Lowest, 256);
			slowThumbnailQueueUnlimited = new ProcessingQueue<ImageKey>(threadCount, "Background Slow Thumbnails Unlimited Queue", ThreadPriority.Lowest, int.MaxValue);
			pages.MemoryCache.ItemAdded += MemoryPageCacheItemAdded;
			thumbs.MemoryCache.ItemAdded += MemoryThumbnailCacheItemAdded;
			slowThumbnailQueue.DefaultProcessingQueueAddMode = slowThumbnailQueueUnlimited.DefaultProcessingQueueAddMode = (fastThumbnailQueue.DefaultProcessingQueueAddMode = (slowPageQueue.DefaultProcessingQueueAddMode = (fastPageQueue.DefaultProcessingQueueAddMode = ProcessingQueueAddMode.AddToTop)));
		}

		public bool AreImagesPending(string filePath)
		{
			if (!slowPageQueue.PendingItems.Any((ImageKey key) => key.Location == filePath) && !fastPageQueue.PendingItems.Any((ImageKey key) => key.Location == filePath) && !fastThumbnailQueue.PendingItems.Any((ImageKey key) => key.Location == filePath) && !slowThumbnailQueueUnlimited.PendingItems.Any((ImageKey key) => key.Location == filePath))
			{
				return slowThumbnailQueue.PendingItems.Any((ImageKey key) => key.Location == filePath);
			}
			return true;
		}

		public void AddThumbToQueue(ThumbnailKey key, object callbackKey, AsyncCallback asyncCallback)
		{
			if (thumbs.DiskCache.IsAvailable(key))
			{
				fastThumbnailQueue.AddItem(key, callbackKey, asyncCallback);
			}
			else
			{
				slowThumbnailQueue.AddItem(key, callbackKey, asyncCallback);
			}
		}

		public IItemLock<ThumbnailImage> GetThumbnail(ThumbnailKey key, IImageProvider provider, ComicBook comic)
		{
			using (IImageProvider provider2 = new ComicBookImageProvider(comic, provider, comic.TranslateImageIndexToPage(key.Index)))
			{
				return GetThumbnail(key, provider2, onErrorThrowException: false);
			}
		}

		public IItemLock<ThumbnailImage> GetThumbnail(ThumbnailKey key, ComicBook comic)
		{
			return GetThumbnail(key, null, comic);
		}

		public IItemLock<ThumbnailImage> GetThumbnail(ComicBook comic)
		{
			return GetThumbnail(comic.GetFrontCoverThumbnailKey(), null, comic);
		}

		public virtual Bitmap CreateErrorPage()
		{
			Bitmap errorPage = Resources.ErrorPage;
			using (Graphics graphics = Graphics.FromImage(errorPage))
			{
				graphics.DrawString(TR.Messages["PageFailedToLoad", "Page failed to load.\nTry refresh to load again..."], FC.Get(SystemFonts.MenuFont, 32f), Brushes.Black, 40f, 40f);
				return errorPage;
			}
		}

		public virtual Bitmap CreateErrorThumbnail(int height)
		{
			Bitmap bitmap = new Bitmap(height * 2 / 3, height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using (Image image = Resources.RedCross)
				{
					Rectangle rect = RectangleExtensions.Align(bounds: new Rectangle(0, 0, bitmap.Width, bitmap.Height), rectangle: new Rectangle(0, 0, image.Width * 3, image.Height * 3), alignment: ContentAlignment.MiddleCenter);
					graphics.Clear(Color.White);
					graphics.DrawImage(image, rect);
					return bitmap;
				}
			}
		}

		protected virtual void OnPageCached(CacheItemEventArgs<ImageKey, PageImage> e)
		{
			if (this.PageCached != null)
			{
				this.PageCached(this, e);
			}
		}

		protected virtual void OnThumbnailCached(CacheItemEventArgs<ImageKey, ThumbnailImage> e)
		{
			if (this.ThumbnailCached != null)
			{
				this.ThumbnailCached(this, e);
			}
		}

		protected virtual void OnRequestResourceThumbnail(ResourceThumbnailEventArgs e)
		{
			if (this.RequestResourceThumbnail != null)
			{
				this.RequestResourceThumbnail(this, e);
			}
		}

		protected virtual ThumbnailImage GetResourceThumbnail(ThumbnailKey key)
		{
			ResourceThumbnailEventArgs resourceThumbnailEventArgs = new ResourceThumbnailEventArgs(key);
			OnRequestResourceThumbnail(resourceThumbnailEventArgs);
			return resourceThumbnailEventArgs.Image;
		}

		private void MemoryPageCacheItemAdded(object sender, CacheItemEventArgs<ImageKey, PageImage> e)
		{
			OnPageCached(e);
		}

		private void MemoryThumbnailCacheItemAdded(object sender, CacheItemEventArgs<ImageKey, ThumbnailImage> e)
		{
			OnThumbnailCached(e);
		}

		public void AddPageToQueue(PageKey key, object callbackKey, AsyncCallback asyncCallback, bool bottom)
		{
			ProcessingQueueAddMode mode = ((!bottom) ? ProcessingQueueAddMode.AddToTop : ProcessingQueueAddMode.AddToBottom);
			if (pages.DiskCache.IsAvailable(key))
			{
				fastPageQueue.AddItem(key, callbackKey, asyncCallback, mode);
			}
			else
			{
				slowPageQueue.AddItem(key, callbackKey, asyncCallback, mode);
			}
		}

		public IItemLock<PageImage> GetPage(PageKey key, bool onlyMemory)
		{
			return pages.GetImage(key, onlyMemory);
		}

		public IItemLock<PageImage> GetPage(PageKey key, IImageProvider provider, bool onErrorThrowException = false)
		{
			IItemLock<PageImage> itemLock = pages.AddImage(key, delegate
			{
				try
				{
					if (provider != null)
					{
						Bitmap bitmap = null;
						Bitmap bitmap2 = null;
						BitmapAdjustment bitmapAdjustment = key.Adjustment;
						ImageRotation imageRotation = key.Rotation;
						if (bitmapAdjustment.IsEmpty && imageRotation == ImageRotation.None)
						{
							byte[] byteImage = provider.GetByteImage(key.Index);
							if (byteImage != null)
							{
								return PageImage.CreateFrom(byteImage);
							}
						}
						else
						{
							if ((bitmap2 = GetPartialDiskPage(key, ImageRotation.None, bitmapAdjustment)) != null)
							{
								bitmapAdjustment = BitmapAdjustment.Empty;
							}
							else if ((bitmap2 = GetPartialDiskPage(key, imageRotation, BitmapAdjustment.Empty)) != null)
							{
								imageRotation = ImageRotation.None;
							}
							else if ((bitmap2 = GetPartialDiskPage(key, ImageRotation.None, BitmapAdjustment.Empty)) != null)
							{
								imageRotation = ImageRotation.None;
								bitmapAdjustment = BitmapAdjustment.Empty;
							}
							bitmap = bitmap2;
						}
						if (bitmap2 == null)
						{
							bitmap2 = (bitmap = provider.GetImage(key.Index));
							if (bitmap2 != null && provider.IsSlow)
							{
								PageKey key2 = new PageKey(key.Source, key.Location, key.Size, key.Modified, key.Index, ImageRotation.None, BitmapAdjustment.Empty);
								using (PageImage item = PageImage.CreateFrom(bitmap2))
								{
									pages.DiskCache.AddItem(key2, item);
								}
							}
						}
						try
						{
							if (!bitmapAdjustment.IsEmpty)
							{
								bitmap = bitmap2.CreateAdjustedBitmap(bitmapAdjustment, PixelFormat.Format32bppArgb, alwaysClone: false);
							}
						}
						finally
						{
							if (bitmap2 != bitmap)
							{
								bitmap2.Dispose();
							}
						}
						if (imageRotation != 0)
						{
							Bitmap bitmap3 = bitmap.Rotate(imageRotation);
							bitmap.Dispose();
							bitmap = bitmap3;
						}
						return PageImage.Wrap(bitmap);
					}
				}
				catch
				{
				}
				return null;
			});
			if (itemLock != null)
			{
				return itemLock;
			}
			if (onErrorThrowException)
			{
				throw new Exception("Could not open image");
			}
			return pages.MemoryCache.LockItem(key, (ImageKey tk) => PageImage.Wrap(CreateErrorPage()));
		}

		private Bitmap GetPartialDiskPage(PageKey key, ImageRotation rot, BitmapAdjustment transform)
		{
			PageKey key2 = new PageKey(key.Source, key.Location, key.Size, key.Modified, key.Index, rot, transform);
			using (PageImage pageImage = pages.DiskCache.GetItem(key2))
			{
				if (pageImage != null && pageImage.Bitmap != null)
				{
					return pageImage.Detach();
				}
			}
			return null;
		}

		public IItemLock<PageImage> GetPage(PageKey key, ComicBook book)
		{
			using (IImageProvider provider = book.OpenProvider(key.Index))
			{
				return GetPage(key, provider);
			}
		}

		public void RefreshPage(PageKey key)
		{
			pages.RefreshImage(key);
		}

		public void RefreshLastImage(string source)
		{
			pages.RefreshLastImage(source);
			thumbs.RefreshLastImage(source);
		}

		public void RemoveImages(string source, int imageIndex = -1)
		{
			pages.RemoveKeys((ImageKey k) => string.Equals(k.Location, source, StringComparison.OrdinalIgnoreCase) && (imageIndex == -1 || k.Index == imageIndex));
			thumbs.RemoveKeys((ImageKey k) => string.Equals(k.Location, source, StringComparison.OrdinalIgnoreCase) && (imageIndex == -1 || k.Index == imageIndex));
		}

		public void CachePage(PageKey key, bool checkMemoryOnly, IImageProvider provider, bool bottom)
		{
			using (IItemLock<PageImage> itemLock = Pages.GetImage(key, checkMemoryOnly))
			{
				if (itemLock != null && itemLock.Item != null)
				{
					return;
				}
			}
			AddPageToQueue(key, null, delegate
			{
				try
				{
					if (key.Index < provider.Count)
					{
						GetPage(key, provider).SafeDispose();
					}
				}
				catch
				{
				}
			}, bottom);
		}

		public IItemLock<ThumbnailImage> GetThumbnail(ThumbnailKey key, bool onlyMemory)
		{
			return thumbs.GetImage(key, onlyMemory);
		}

		public IItemLock<ThumbnailImage> GetThumbnail(ThumbnailKey key, IImageProvider provider, bool onErrorThrowException)
		{
			IItemLock<ThumbnailImage> itemLock = thumbs.AddImage(key, delegate
			{
				PageKey key2 = new PageKey(key);
				if (!string.IsNullOrEmpty(key.ResourceType))
				{
					ThumbnailImage resourceThumbnail = GetResourceThumbnail(key);
					if (resourceThumbnail != null)
					{
						return resourceThumbnail;
					}
				}
				if (provider != null)
				{
					ThumbnailImage thumbnail = provider.GetThumbnail(key.Index);
					if (thumbnail != null)
					{
						return thumbnail;
					}
				}
				try
				{
					IItemLock<PageImage> itemLock2 = pages.GetImage(key2);
					Image image2 = null;
					try
					{
						Bitmap bitmap = null;
						if (itemLock2 != null && itemLock2.Item != null)
						{
							bitmap = itemLock2.Item.Bitmap;
						}
						else if (provider != null)
						{
							if (cacheThumbnailPages || provider.IsSlow)
							{
								itemLock2 = pages.AddImage(key2, provider);
								if (itemLock2 != null && itemLock2.Item != null)
								{
									bitmap = itemLock2.Item.Bitmap;
								}
							}
							else
							{
								byte[] byteImage = provider.GetByteImage(key.Index);
								bitmap = ((byteImage == null) ? provider.GetImage(key.Index) : BitmapExtensions.BitmapFromBytes(byteImage, PixelFormat.Undefined));
								if (bitmap != null && key.Rotation != 0)
								{
									Bitmap bitmap2 = bitmap.Rotate(key.Rotation);
									bitmap.Dispose();
									bitmap = bitmap2;
								}
								image2 = bitmap;
							}
						}
						if (bitmap != null)
						{
							return ThumbnailImage.CreateFrom(bitmap, bitmap.Size);
						}
					}
					finally
					{
						itemLock2?.Dispose();
						image2?.Dispose();
					}
				}
				catch
				{
				}
				return null;
			});
			if (itemLock != null)
			{
				return itemLock;
			}
			if (onErrorThrowException)
			{
				throw new InvalidOperationException("Could not load thumbnail");
			}
			return thumbs.MemoryCache.LockItem(key, delegate
			{
				using (Bitmap image = CreateErrorThumbnail(ThumbnailImage.MaxHeight))
				{
					return ThumbnailImage.CreateFrom(image, Size.Empty);
				}
			});
		}

		public void CacheThumbnail(ThumbnailKey key, bool checkMemoryOnly, IImageProvider provider)
		{
			using (IItemLock<ThumbnailImage> itemLock = thumbs.GetImage(key, checkMemoryOnly))
			{
				if (itemLock != null)
				{
					return;
				}
				AddThumbToQueue(key, null, delegate
				{
					try
					{
						if (key.Index < provider.Count)
						{
							GetThumbnail(key, provider, onErrorThrowException: true).SafeDispose();
						}
					}
					catch
					{
					}
				});
			}
		}

		public void GenerateFrontCoverThumbnail(ComicBook cb)
		{
			if (cb == null)
				return;

			ThumbnailKey key = cb.GetFrontCoverThumbnailKey();
			slowThumbnailQueueUnlimited.AddItem(key, null, delegate
			{
				if (thumbs.DiskCache.IsAvailable(key))
					return;

				try
				{
					GetThumbnail(key, cb).SafeDispose();
				}
				catch
				{
				}
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				fastThumbnailQueue.Dispose();
				slowThumbnailQueue.Dispose();
				slowThumbnailQueueUnlimited.Dispose();
				fastPageQueue.Dispose();
				slowPageQueue.Dispose();
				thumbs.Dispose();
				pages.Dispose();
			}
			base.Dispose(disposing);
		}

		public string AddCustomThumbnail(Bitmap bmp)
		{
			string text = Guid.NewGuid().ToString();
			string file = Path.Combine(CustomThumbnailFolder, text);
			using (ThumbnailImage thumbnailImage = ThumbnailImage.CreateFrom(bmp, bmp.Size, supportTransparent: true))
			{
				thumbnailImage.Save(file);
				return text;
			}
		}

		public IEnumerable<string> GetCustomThumbnailKeys()
		{
			return FileUtility.GetFiles(CustomThumbnailFolder, SearchOption.TopDirectoryOnly);
		}

		public bool RemoveCustomThumbnail(string key)
		{
			return FileUtility.SafeDelete(Path.Combine(CustomThumbnailFolder, key));
		}

		public ThumbnailImage GetCustomThumbnail(string key)
		{
			return ThumbnailImage.CreateFrom(Path.Combine(CustomThumbnailFolder, key));
		}

		public bool CustomThumbnailExists(string key)
		{
			return File.Exists(Path.Combine(CustomThumbnailFolder, key));
		}
	}
}
