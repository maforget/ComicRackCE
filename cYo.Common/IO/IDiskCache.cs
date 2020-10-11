using System;

namespace cYo.Common.IO
{
	public interface IDiskCache<K, T> : IDisposable
	{
		bool Enabled
		{
			get;
			set;
		}

		int CacheSizeMB
		{
			get;
			set;
		}

		long Size
		{
			get;
		}

		int Count
		{
			get;
		}

		event EventHandler SizeChanged;

		T GetItem(K key);

		bool AddItem(K key, T item);

		void RemoveItem(K key);

		bool IsAvailable(K key);

		void UpdateKeys(Func<K, bool> select, Action<K> update);

		void RemoveKeys(Func<K, bool> select);

		K[] GetKeys();

		void CleanUp();

		void Clear();
	}
}
