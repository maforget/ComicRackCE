using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace cYo.Common.Net.Search
{
	public abstract class CachedSearch : INetSearch
	{
		public struct CacheKey
		{
			public string Hint;

			public string Text;

			public int Limit;

			public override bool Equals(object obj)
			{
				CacheKey cacheKey = (CacheKey)obj;
				if (Hint == cacheKey.Hint && Limit == cacheKey.Limit)
				{
					return Text == cacheKey.Text;
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (Text + Hint + Limit).GetHashCode();
			}
		}

		private Dictionary<CacheKey, SearchResult[]> cache = new Dictionary<CacheKey, SearchResult[]>();

		public abstract string Name
		{
			get;
		}

		public abstract Image Image
		{
			get;
		}

		public IEnumerable<SearchResult> Search(string hint, string text, int limit)
		{
			CacheKey cacheKey = default(CacheKey);
			cacheKey.Hint = hint;
			cacheKey.Text = text;
			cacheKey.Limit = limit;
			CacheKey key = cacheKey;
			SearchResult[] value;
			try
			{
				if (cache.TryGetValue(key, out value))
				{
					return value;
				}
				value = OnSearch(hint, text, limit).ToArray();
			}
			catch (Exception)
			{
				value = new SearchResult[0];
			}
			cache[key] = value;
			return value;
		}

		public string GenericSearchLink(string hint, string text)
		{
			try
			{
				return OnGenericSearchLink(hint, text);
			}
			catch (Exception)
			{
				return null;
			}
		}

		protected abstract IEnumerable<SearchResult> OnSearch(string hint, string text, int limit);

		protected abstract string OnGenericSearchLink(string hint, string text);
	}
}
