using System;
using System.Linq;
using System.Threading;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public abstract class ImageManagerBase<T> : DisposableObject where T : class, IDisposable
	{
		private readonly Cache<ImageKey, T> memoryCache = new Cache<ImageKey, T>(256);

		public IDiskCache<ImageKey, T> DiskCache
		{
			get;
			set;
		}

		public Cache<ImageKey, T> MemoryCache => memoryCache;

		protected ImageManagerBase()
		{
			memoryCache.ItemRemoved += memoryCache_ItemRemoved;
		}

		protected ImageManagerBase(int itemCapacity, long sizeCapacity)
			: this()
		{
			memoryCache.ItemCapacity = itemCapacity;
			memoryCache.SizeCapacity = sizeCapacity;
		}

		protected ImageManagerBase(int itemCapacity)
			: this(itemCapacity, long.MaxValue)
		{
		}

		public virtual IItemLock<T> GetImage(ImageKey key, bool memoryOnly = false, bool dontUpdateAccess = false)
		{
			return MemoryCache.LockItem(key, (memoryOnly || DiskCache == null) ? null : new Func<ImageKey, T>(DiskCache.GetItem), dontUpdateAccess);
		}

		public void RefreshImage(ImageKey key)
		{
			memoryCache.RemoveItem(key);
			if (DiskCache != null)
			{
				DiskCache.RemoveItem(key);
			}
		}

		public void RefreshLastImage(string source)
		{
			Func<ImageKey, bool> predicate = (ImageKey k) => string.Equals(k.Location, source, StringComparison.OrdinalIgnoreCase);
			try
			{
				RefreshImage(MemoryCache.GetKeys().Where(predicate).Max((ImageKey a, ImageKey b) => Math.Sign(a.Index - b.Index)));
			}
			catch
			{
			}
			try
			{
				RefreshImage(DiskCache.GetKeys().Where(predicate).Max((ImageKey a, ImageKey b) => Math.Sign(a.Index - b.Index)));
			}
			catch
			{
			}
		}

		public IItemLock<T> AddImage(ImageKey key, Func<ImageKey, T> getImage)
		{
			return MemoryCache.LockItem(key, delegate
			{
				T item;
				if (DiskCache != null)
				{
					item = DiskCache.GetItem(key);
					if (item != null)
					{
						return item;
					}
				}
				item = getImage(key);
				if (item != null && DiskCache != null)
				{
					DiskCache.AddItem(key, item);
				}
				return item;
			});
		}

		public IItemLock<T> AddImage(ImageKey key, IImageProvider imageProvider)
		{
			return AddImage(key, (ImageKey k) => CreateNewFromProvider(k, imageProvider));
		}

		public bool IsAvailable(ImageKey key, bool memoryOnly)
		{
			using (IItemLock<T> itemLock = GetImage(key, memoryOnly))
			{
				return itemLock != null && itemLock.Item != null;
			}
		}

		public bool IsAvailable(ImageKey key)
		{
			return IsAvailable(key, memoryOnly: false);
		}

		public void UpdateKeys(Func<ImageKey, bool> select, Action<ImageKey> update)
		{
			MemoryCache.UpdateKeys(select, update);
			if (DiskCache != null)
			{
				DiskCache.UpdateKeys(select, update);
			}
		}

		public void RemoveKeys(Func<ImageKey, bool> select)
		{
			MemoryCache.RemoveKeys(select);
			if (DiskCache != null)
			{
				DiskCache.RemoveKeys(select);
			}
		}

		private void memoryCache_ItemRemoved(object sender, CacheItemEventArgs<ImageKey, T> e)
		{
			if (e.Item == null)
			{
				return;
			}
			ImageKey key = e.Key;
			T item = e.Item;
			ThreadPool.QueueUserWorkItem(delegate
			{
				using (ItemMonitor.Lock(item))
				{
					item.Dispose();
				}
			});
		}

		protected abstract T CreateNewFromProvider(ImageKey key, IImageProvider provider);

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				memoryCache.Dispose();
				DiskCache.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
