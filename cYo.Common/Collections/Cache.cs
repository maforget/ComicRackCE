using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using cYo.Common.ComponentModel;
using cYo.Common.Runtime;
using cYo.Common.Threading;

namespace cYo.Common.Collections
{
	public class Cache<K, T> : DisposableObject where T : class
	{
		private class CacheItem
		{
			private int lockCount;

			private long lastAccess;

			public int LockCount => lockCount;

			public bool IsLocked => lockCount > 0;

			public long LastAccess => lastAccess;

			public T Data
			{
				get;
				set;
			}

			public LinkedListNode<K> AccessNode
			{
				get;
				set;
			}

			public CacheItem(T data)
			{
				Data = data;
			}

			public ItemLock<T> GetLock()
			{
				ItemLock<T> itemLock = new ItemLock<T>(Data, this)
				{
					Item = Data,
					LockObject = this
				};
				lockCount++;
				itemLock.Disposed += LockItemDisposed;
				lastAccess = Machine.Ticks;
				return itemLock;
			}

			private void LockItemDisposed(object sender, EventArgs e)
			{
				if (--lockCount < 0)
				{
					lockCount = 0;
				}
			}
		}

		private readonly Dictionary<K, CacheItem> cache = new Dictionary<K, CacheItem>();

		private readonly LinkedList<K> accessList = new LinkedList<K>();

		private readonly Dictionary<K, CacheItem> pending = new Dictionary<K, CacheItem>();

		private readonly ReaderWriterLockSlim lockCache = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

		private volatile int itemCapacity;

		private long sizeCapacity;

		private int minimalTimeInCache = 5000;

		public int ItemCapacity
		{
			get
			{
				return itemCapacity;
			}
			set
			{
				if (itemCapacity != value)
				{
					itemCapacity = value;
					Trim();
				}
			}
		}

		public long SizeCapacity
		{
			get
			{
				return Interlocked.Read(ref sizeCapacity);
			}
			set
			{
				if (SizeCapacity != value)
				{
					Interlocked.Exchange(ref sizeCapacity, value);
					Trim();
				}
			}
		}

		public int MinimalTimeInCache
		{
			get
			{
				return minimalTimeInCache;
			}
			set
			{
				if (minimalTimeInCache != value)
				{
					minimalTimeInCache = value;
					Trim();
				}
			}
		}

		public int Count
		{
			get
			{
				using (ReadLock())
				{
					return accessList.Count;
				}
			}
		}

		public long Size
		{
			get
			{
				using (ReadLock())
				{
					return ((IEnumerable<CacheItem>)cache.Values).Sum((Func<CacheItem, long>)((CacheItem v) => GetDataSize(v.Data)));
				}
			}
		}

		public event EventHandler<CacheItemEventArgs<K, T>> ItemAdded;

		public event EventHandler<CacheItemEventArgs<K, T>> ItemRemoved;

		public event EventHandler SizeChanged;

		private IDisposable ReadLock()
		{
			return lockCache.ReadLock();
		}

		private IDisposable WriteLock()
		{
			return lockCache.WriteLock();
		}

		private IDisposable UpgradeableReadLock()
		{
			return lockCache.UpgradeableReadLock();
		}

		public Cache(int itemCapacity, int sizeCapacity)
		{
			this.itemCapacity = itemCapacity;
			this.sizeCapacity = sizeCapacity;
		}

		public Cache(int itemCapacity)
			: this(itemCapacity, int.MaxValue)
		{
		}

		public IItemLock<T> LockItem(K key, Func<K, T> create, bool dontUpdateAccess)
		{
			IItemLock<T> itemLock = OpenItem(key, dontUpdateAccess, create != null);
			if (itemLock != null && itemLock.Item == null)
			{
				T val = null;
				CacheItem cacheItem = null;
				try
				{
					val = create(key);
				}
				catch (Exception)
				{
				}
				using (WriteLock())
				{
					if (val != null)
					{
						cacheItem = itemLock.LockObject as CacheItem;
						T val4 = (cacheItem.Data = (itemLock.Item = val));
						cache[key] = cacheItem;
						while (accessList.Contains(key))
						{
							accessList.Remove(key);
						}
						accessList.AddLast(cacheItem.AccessNode);
					}
					else
					{
						itemLock.Dispose();
						itemLock = null;
					}
					pending.Remove(key);
				}
				if (val != null)
				{
					itemLock.Dispose();
					try
					{
						OnItemAdded(new CacheItemEventArgs<K, T>(key, val));
					}
					catch
					{
					}
					Trim(key);
					itemLock = cacheItem.GetLock();
				}
			}
			return itemLock;
		}

		public IItemLock<T> LockItem(K key, bool dontUpdateAccess)
		{
			return LockItem(key, null, dontUpdateAccess);
		}

		public IItemLock<T> LockItem(K key, Func<K, T> create)
		{
			return LockItem(key, create, dontUpdateAccess: false);
		}

		public IItemLock<T> LockItem(K key, T data)
		{
			return LockItem(key, (K k) => data);
		}

		public bool IsCached(K key)
		{
			using (ReadLock())
			{
				return cache.ContainsKey(key);
			}
		}

		public bool IsLocked(K key)
		{
			using (ReadLock())
			{
				CacheItem value;
				return cache.TryGetValue(key, out value) && value.IsLocked;
			}
		}

		public bool IsCaching(K key)
		{
			using (ReadLock())
			{
				return pending.ContainsKey(key);
			}
		}

		public void RemoveItem(K key, bool evenWhenLocked)
		{
			CacheItem cacheItem = RemoveItemInternal(key, evenWhenLocked);
			if (cacheItem != null)
			{
				OnItemRemoved(new CacheItemEventArgs<K, T>(key, cacheItem.Data));
			}
		}

		public void RemoveItem(K key)
		{
			RemoveItem(key, evenWhenLocked: false);
		}

		public void UpdateKeys(Func<K, bool> select, Action<K> update)
		{
			using (WriteLock())
			{
				foreach (K item in cache.Keys.ToArray().Where(select))
				{
					CacheItem value = cache[item];
					cache.Remove(item);
					update(item);
					cache.Add(item, value);
				}
			}
		}

		public void RemoveKeys(Func<K, bool> select)
		{
			using (WriteLock())
			{
				cache.Keys.ToArray().Where(select).ForEach(RemoveItem);
			}
		}

		public K[] GetKeys()
		{
			using (ReadLock())
			{
				return cache.Keys.ToArray();
			}
		}

		public void Clear(bool evenLocked)
		{
			K[] list;
			using (ReadLock())
			{
				list = cache.Keys.Where((K k) => !IsLocked(k) || evenLocked).ToArray();
			}
			list.ForEach(delegate(K key)
			{
				RemoveItem(key, evenWhenLocked: true);
			});
		}

		protected virtual void OnItemAdded(CacheItemEventArgs<K, T> cacheItemEventArgs)
		{
			if (this.ItemAdded != null)
			{
				this.ItemAdded(this, cacheItemEventArgs);
			}
			OnSizeChanged();
		}

		protected virtual void OnItemRemoved(CacheItemEventArgs<K, T> cacheItemEventArgs)
		{
			if (this.ItemRemoved != null)
			{
				this.ItemRemoved(this, cacheItemEventArgs);
			}
			OnSizeChanged();
		}

		protected virtual void OnSizeChanged()
		{
			if (this.SizeChanged != null)
			{
				this.SizeChanged(this, EventArgs.Empty);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Clear(evenLocked: true);
			}
			base.Dispose(disposing);
		}

		private IItemLock<T> OpenItem(K key, bool dontUpdateAccess, bool addNew)
		{
			CacheItem value;
			using (UpgradeableReadLock())
			{
				if (cache.TryGetValue(key, out value))
				{
					if (!dontUpdateAccess)
					{
						using (WriteLock())
						{
							UpdateAccess(value);
						}
					}
				}
				else if (addNew && !pending.TryGetValue(key, out value))
				{
					value = new CacheItem(null);
					value.AccessNode = new LinkedListNode<K>(key);
					using (WriteLock())
					{
						pending[key] = value;
					}
				}
			}
			return value?.GetLock();
		}

		private CacheItem RemoveItemInternal(K key, bool evenWhenLocked, bool onlyExpired)
		{
			using (UpgradeableReadLock())
			{
				if (!cache.TryGetValue(key, out var value) || (value.IsLocked && !evenWhenLocked) || (DateTime.Now.Ticks - value.LastAccess < MinimalTimeInCache && onlyExpired))
				{
					return null;
				}
				using (WriteLock())
				{
					cache.Remove(key);
					accessList.Remove(value.AccessNode);
				}
				return value;
			}
		}

		private CacheItem RemoveItemInternal(K key, bool evenWhenLocked)
		{
			return RemoveItemInternal(key, evenWhenLocked, onlyExpired: false);
		}

		private void UpdateAccess(CacheItem ci)
		{
			try
			{
				accessList.Remove(ci.AccessNode);
			}
			catch
			{
			}
			accessList.AddLast(ci.AccessNode);
		}

		private static int GetDataSize(T o)
		{
			return (o as IDataSize)?.DataSize ?? 0;
		}

		private void Trim(K keep)
		{
			List<CacheItemEventArgs<K, T>> list = new List<CacheItemEventArgs<K, T>>();
			long ticks = Machine.Ticks;
			using (UpgradeableReadLock())
			{
				long num = Size;
				LinkedListNode<K> linkedListNode = accessList.First;
				while (linkedListNode != null && (accessList.Count > itemCapacity || num > sizeCapacity))
				{
					LinkedListNode<K> next = linkedListNode.Next;
					CacheItem cacheItem = (object.Equals(linkedListNode.Value, keep) ? null : RemoveItemInternal(linkedListNode.Value, evenWhenLocked: false, onlyExpired: true));
					if (cacheItem != null)
					{
						num -= GetDataSize(cacheItem.Data);
						list.Add(new CacheItemEventArgs<K, T>(linkedListNode.Value, cacheItem.Data));
					}
					linkedListNode = next;
				}
			}
			foreach (CacheItemEventArgs<K, T> item in list)
			{
				OnItemRemoved(item);
			}
		}

		public void Trim()
		{
			Trim(default(K));
		}
	}
}
