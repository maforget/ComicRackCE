using System;
using System.Drawing;
using System.Resources;
using cYo.Common.Collections;
using cYo.Common.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using cYo.Projects.ComicRack.Engine.Properties;

namespace cYo.Projects.ComicRack.Engine
{
	public class CacheManager
	{
		public const int MemoryThumbnailCacheSize = 4096;

		public DatabaseManager DatabaseManager
		{
			get;
			private set;
		}

		public FileCache InternetCache
		{
			get;
			private set;
		}

		public ImagePool ImagePool
		{
			get;
			private set;
		}

		public ResourceManager ResourceManager
		{
			get;
			private set;
		}

		public CacheManager(DatabaseManager databaseManager, SystemPaths paths, ICacheSettings settings, ResourceManager resourceManager = null)
		{
			CacheManager cacheManager = this;
			ResourceManager = Resources.ResourceManager;
			DatabaseManager = databaseManager;
			InternetCache = new FileCache(paths.FileCachePath, 100);
			ImagePool = new ImagePool(MemoryThumbnailCacheSize, settings.MemoryThumbCacheSizeMB, settings.MemoryPageCacheCount);
			ImagePool.Thumbs.DiskCache = new ThumbnailDiskCache<ImageKey>(paths.ThumbnailCachePath, settings.ThumbCacheSizeMB);
			ImagePool.Pages.DiskCache = new ImageDiskCache<ImageKey>(paths.ImageCachePath, settings.PageCacheSizeMB);
			ImagePool.CustomThumbnailFolder = paths.CustomThumbnailPath;
			ImagePool.RequestResourceThumbnail += RequestResourceThumbnail;
			ImagePool.PageCached += ImagePoolPageCached;
			ImagePool.ThumbnailCached += ImagePoolThumbnailCached;
			UpdateCacheSettings(settings);
			settings.CacheSettingsChanged += delegate
			{
				cacheManager.UpdateCacheSettings(settings);
			};
		}

		private void RequestResourceThumbnail(object sender, ResourceThumbnailEventArgs e)
		{
			try
			{
				switch (e.Key.ResourceType)
				{
				case ThumbnailKey.ResourceKey:
					if (ResourceManager != null)
					{
						using (Bitmap bitmap2 = ResourceManager.GetObject(e.Key.ResourceLocation) as Bitmap)
						{
							e.Image = ThumbnailImage.CreateFrom(bitmap2, bitmap2.Size);
						}
					}
					break;
				case ThumbnailKey.FileKey:
				{
					using (Bitmap bitmap = BitmapExtensions.BitmapFromFile(e.Key.ResourceLocation))
					{
						e.Image = ThumbnailImage.CreateFrom(bitmap, bitmap.Size);
					}
					break;
				}
				case ThumbnailKey.CustomKey:
					e.Image = ImagePool.GetCustomThumbnail(e.Key.ResourceLocation);
					break;
				default:
					throw new ArgumentException();
				}
			}
			catch (Exception)
			{
			}
		}

		private void ImagePoolPageCached(object sender, CacheItemEventArgs<ImageKey, PageImage> e)
		{
			UpdateComicBookPageData(e.Key, e.Item.Size);
		}

		private void ImagePoolThumbnailCached(object sender, CacheItemEventArgs<ImageKey, ThumbnailImage> e)
		{
			UpdateComicBookPageData(e.Key, e.Item.OriginalSize);
		}

		public void UpdateCacheSettings(ICacheSettings settings)
		{
			InternetCache.Enabled = settings.InternetCacheEnabled;
			InternetCache.CacheSizeMB = settings.InternetCacheSizeMB;
			ImagePool.Thumbs.DiskCache.CacheSizeMB = settings.ThumbCacheSizeMB;
			ImagePool.Thumbs.DiskCache.Enabled = settings.ThumbCacheEnabled;
			ImagePool.Pages.DiskCache.CacheSizeMB = settings.PageCacheSizeMB;
			ImagePool.Pages.DiskCache.Enabled = settings.PageCacheEnabled;
			ImagePool.Thumbs.MemoryCache.SizeCapacity = (long)settings.MemoryThumbCacheSizeMB * 1024L * 1024;
			ImagePool.Pages.MemoryCache.ItemCapacity = settings.MemoryPageCacheCount;
			ThumbnailImage.MemoryOptimized = settings.MemoryThumbCacheOptimized;
			PageImage.MemoryOptimized = settings.MemoryPageCacheOptimized;
		}

		private void UpdateComicBookPageData(ImageKey key, Size size)
		{
			UpdateComicBookPageData(DatabaseManager.Database.Books[key.Location], key, size);
			UpdateComicBookPageData(key.Source as ComicBook, key, size);
			UpdateComicBookPageData(key.Source as ComicBookNavigator, key, size);
		}

		private void UpdateComicBookPageData(ComicBook cb, ImageKey key, Size size)
		{
			if (cb != null && !size.IsEmpty)
			{
				int page = cb.TranslateImageIndexToPage(key.Index);
				cb.UpdatePageSize(page, size.Width, size.Height);
			}
		}

		private void UpdateComicBookPageData(ComicBookNavigator nav, ImageKey key, Size size)
		{
			if (nav != null)
			{
				UpdateComicBookPageData(nav.Comic, key, size);
			}
		}
	}
}
