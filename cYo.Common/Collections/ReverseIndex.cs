using System;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Threading;

namespace cYo.Common.Collections
{
	[Serializable]
	public class ReverseIndex<T, K>
	{
		private readonly List<T> complete = new List<T>();

		private readonly Dictionary<K, ICollection<T>> index = new Dictionary<K, ICollection<T>>();

		public int Size => index.Count;

		public IEnumerable<K> Keys => index.Keys;

		public void Add(T item, K key)
		{
			ICollection<T> value;
			using (ItemMonitor.Lock(index))
			{
				if (!index.TryGetValue(key, out value))
				{
					value = (index[key] = new List<T>());
				}
			}
			using (ItemMonitor.Lock(value))
			{
				value.Add(item);
			}
			using (ItemMonitor.Lock(complete))
			{
				complete.Add(item);
			}
		}

		public void Add(T item, IEnumerable<K> keys)
		{
			foreach (K key in keys)
			{
				Add(item, key);
			}
		}

		public void AddRange(IEnumerable<T> items, Func<T, K> predicate)
		{
			foreach (T t in items)
			{
				this.Add(t, predicate(t));
			}
		}

		public void AddRange(IEnumerable<T> items, Func<T, IEnumerable<K>> predicate)
		{
			items.ParallelForEach(delegate (T item)
			{
				foreach (K key in predicate(item))
				{
					this.Add(item, key);
				}
			});
		}

		public void Remove(T item)
		{
			using (ItemMonitor.Lock(index))
			{
				KeyValuePair<K, ICollection<T>>[] array = index.Where((KeyValuePair<K, ICollection<T>> kvp) => kvp.Value.Contains(item)).ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<K, ICollection<T>> keyValuePair = array[i];
					using (ItemMonitor.Lock(keyValuePair.Value))
					{
						keyValuePair.Value.Remove(item);
						if (keyValuePair.Value.Count == 0)
						{
							index.Remove(keyValuePair.Key);
						}
					}
				}
			}
			using (ItemMonitor.Lock(complete))
			{
				complete.Remove(item);
			}
		}

		public IEnumerable<T> Match(K key)
		{
			using (ItemMonitor.Lock(index))
			{
				ICollection<T> value;
				return index.TryGetValue(key, out value) ? value.ToArray() : new T[0];
			}
		}

		public IEnumerable<T> Where(Func<K, bool> predicate)
		{
			using (ItemMonitor.Lock(index))
			{
				return index.Where((x) => predicate(x.Key)) is var value && value.Any() ? value.SelectMany(x => x.Value) : new T[0];
			}
		}
	}
}
