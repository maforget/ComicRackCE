using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using cYo.Common.Collections;
using cYo.Common.Threading;

namespace cYo.Common.Text
{
	public class ReverseTextIndex<T>
	{
		public const int MinimumKeyLength = 3;

		private readonly List<T> complete = new List<T>();

		private readonly Dictionary<string, ICollection<T>> index = new Dictionary<string, ICollection<T>>(StringComparer.OrdinalIgnoreCase);

		private static char[] wordSeparators = " \r\n\t,;.:!?()[]{}-'\u00b4`\\/\"Â\u00a0‘’“”…".ToArray();

		private static Regex rxWords = new Regex("\\w+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private static readonly Dictionary<string, string[]> wordMap = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

		public ReverseTextIndexMode ReverseIndexMode
		{
			get;
			private set;
		}

		public int Size => index.Count;

		public IEnumerable<string> Keys => index.Keys;

		public ReverseTextIndex(ReverseTextIndexMode mode = ReverseTextIndexMode.Words)
		{
			ReverseIndexMode = mode;
		}

		public void Add(T item, string text)
		{
			foreach (string item2 in Split(text))
			{
				ICollection<T> value;
				using (ItemMonitor.Lock(index))
				{
					if (!index.TryGetValue(item2, out value))
					{
						value = (index[item2] = new List<T>());
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
		}

		public void Add(T item, IEnumerable<string> texts)
		{
			foreach (string text in texts)
			{
				Add(item, text);
			}
		}

		public void AddRange(IEnumerable<T> items, Func<T, string> predicate)
		{
			foreach (T t in items)
			{
				this.Add(t, predicate(t));
			}
		}

		public void AddRange(IEnumerable<T> items, Func<T, IEnumerable<string>> predicate)
		{
			items.ParallelForEach(delegate (T item)
			{
				foreach (string text in predicate(item))
				{
					this.Add(item, text);
				}
			});
		}

		public void Remove(T item)
		{
			using (ItemMonitor.Lock(index))
			{
				KeyValuePair<string, ICollection<T>>[] array = index.Where((KeyValuePair<string, ICollection<T>> kvp) => kvp.Value.Contains(item)).ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					KeyValuePair<string, ICollection<T>> keyValuePair = array[i];
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

		public IEnumerable<T> Match(string text, bool matchAny = false)
		{
			IEnumerable<T> enumerable = null;
			foreach (string item in SplitToWords(text))
			{
				IEnumerable<T> enumerable2;
				using (ItemMonitor.Lock(index))
				{
					if (item.Length < 3)
					{
						enumerable2 = complete.Lock();
					}
					else
					{
						IEnumerable<T> enumerable3;
						if (!index.TryGetValue(item, out var value))
						{
							enumerable3 = Enumerable.Empty<T>();
						}
						else
						{
							IEnumerable<T> enumerable4 = value;
							enumerable3 = enumerable4;
						}
						enumerable2 = enumerable3;
					}
				}
				if (enumerable == null)
				{
					enumerable = enumerable2.Lock();
					continue;
				}
				if (matchAny)
				{
					enumerable = enumerable.Union(enumerable2.Lock());
					if (enumerable.Count() != complete.Count())
					{
						continue;
					}
					return enumerable;
				}
				if (enumerable2.Count() < enumerable.Count())
				{
					enumerable = enumerable.Intersect(enumerable2.Lock());
				}
				if (enumerable.Count() != 0)
				{
					continue;
				}
				return enumerable;
			}
			return enumerable;
		}

		private static IEnumerable<string> SplitToWords(string text)
		{
			return text.Split(wordSeparators, StringSplitOptions.RemoveEmptyEntries);
		}

		private static IEnumerable<string> SplitWordParts(string w, int mkl)
		{
			int j = w.Length;
			while (mkl <= j)
			{
				for (int i = 0; i <= j - mkl; i++)
				{
					yield return w.Substring(i, mkl);
				}
				mkl++;
			}
		}

		private IEnumerable<string> Split(string text)
		{
			switch (ReverseIndexMode)
			{
			case ReverseTextIndexMode.Letters:
				foreach (string item in from w in SplitToWords(text)
					where w.Length >= 3
					select w)
				{
					string[] value;
					using (ItemMonitor.Lock(wordMap))
					{
						if (!wordMap.TryGetValue(item, out value))
						{
							value = (wordMap[item] = SplitWordParts(item, 3).ToArray());
						}
					}
					string[] array2 = value;
					for (int i = 0; i < array2.Length; i++)
					{
						yield return array2[i];
					}
				}
				yield break;
			case ReverseTextIndexMode.Key:
				yield return text;
				yield break;
			}
			foreach (string item2 in from w in SplitToWords(text)
				where w.Length >= 3
				select w)
			{
				yield return item2;
			}
		}
	}
}
