using System;

namespace cYo.Common.Collections
{
	public class CacheItemEventArgs<K, T> : EventArgs
	{
		private readonly K key;

		private readonly T item;

		public K Key => key;

		public T Item => item;

		public CacheItemEventArgs(K key, T item)
		{
			this.key = key;
			this.item = item;
		}
	}
}
