using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cYo.Common;

namespace cYo.Projects.ComicRack.Engine
{
	public class ValuesStore
	{
		private static readonly IEqualityComparer<string> keyEquality = StringComparer.OrdinalIgnoreCase;

		private readonly Dictionary<string, string> lookup = new Dictionary<string, string>(keyEquality);

		public ValuesStore(string store)
		{
			if (store == null)
			{
				return;
			}
			foreach (StringPair value in GetValues(store))
			{
				lookup.Add(value.Key, value.Value);
			}
		}

		public string Get(string key)
		{
			if (!lookup.TryGetValue(key, out var value))
			{
				return null;
			}
			return value;
		}

		public ValuesStore Add(string key, string value)
		{
			if (!string.IsNullOrEmpty(key))
			{
				lookup[key] = value;
			}
			return this;
		}

		public ValuesStore Remove(string key)
		{
			lookup.Remove(key);
			return this;
		}

		public ValuesStore Clear()
		{
			lookup.Clear();
			return this;
		}

		public IEnumerable<StringPair> GetValues()
		{
			return lookup.Keys.Select((string key) => new StringPair(key, lookup[key]));
		}

		public override string ToString()
		{
			if (lookup.Keys.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string item in lookup.Keys.OrderBy((string s) => s))
			{
				if (item.Length != 0)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(Encode(item));
				stringBuilder.Append("=");
				stringBuilder.Append(Encode(lookup[item]));
			}
			return stringBuilder.ToString();
		}

		private static string Decode(string s)
		{
			return s?.Replace("&#61;", "=").Replace("&#44;", ",");
		}

		private static string Encode(string s)
		{
			return s?.Replace("=", "&#61;").Replace(",", "&#44;");
		}

		public static IEnumerable<StringPair> GetValues(string store)
		{
			if (store == null)
			{
				return Enumerable.Empty<StringPair>();
			}
			return from l in store.Split(',')
				select l.Split('=') into kvp
				where kvp.Length == 2
				select new StringPair(Decode(kvp[0]), Decode(kvp[1]));
		}

		public static string GetValue(string store, string key)
		{
			return (from vp in GetValues(store)
				where keyEquality.Equals(vp.Key, key)
				select vp.Value).FirstOrDefault();
		}
	}
}
