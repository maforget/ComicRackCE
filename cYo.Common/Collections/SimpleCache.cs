using System;
using System.Collections.Generic;

namespace cYo.Common.Collections
{
	public class SimpleCache<K, T>
	{
		private Dictionary<K, T> dict = new Dictionary<K, T>();

		public T Get(K key, Func<K, T> create)
		{
			if (dict == null)
			{
				dict = new Dictionary<K, T>();
			}
			if (!dict.TryGetValue(key, out var value))
			{
				return dict[key] = create(key);
			}
			return value;
		}
	}
}
