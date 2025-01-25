using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cYo.Common.Threading;

namespace cYo.Common.Collections
{
	public static class ListExtensions
	{
		public static bool ParallelEnabled = true;

		public static T[] CreateCopy<T>(this T[] data)
		{
			T[] array = new T[data.Length];
			data.CopyTo(array, 0);
			return array;
		}

		public static List<T> SafeAdd<T>(this List<T> list, T item)
		{
			if (list == null)
			{
				list = new List<T>();
			}
			list.Add(item);
			return list;
		}

		public static void Trim(this IList list, int length)
		{
			if (length > list.Count)
			{
				return;
			}
			if (length == 0)
			{
				list.Clear();
				return;
			}
			for (int num = list.Count - 1; num >= length; num--)
			{
				list.RemoveAt(num);
			}
		}

		public static IList<T> Randomize<T>(this IList<T> list, int seed = 0)
		{
			Random random = ((seed == 0) ? new Random() : new Random(seed));
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				int index = random.Next(count - 1);
				int index2 = random.Next(count - 1);
				T value = list[index];
				list[index] = list[index2];
				list[index2] = value;
			}
			return list;
		}

		public static void AddRange<T>(this ICollection<T> list, IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				list.Add(item);
			}
		}

		public static void AddRange<T, K>(this IDictionary<T, K> dict, IDictionary<T, K> values)
		{
			foreach (KeyValuePair<T, K> value in values)
			{
				dict[value.Key] = value.Value;
			}
		}

		public static void RemoveRange<T>(this ICollection<T> list, IEnumerable<T> items)
		{
			foreach (T item in items)
			{
				list.Remove(item);
			}
		}

		public static void RemoveAll<T>(this ICollection<T> collection, Predicate<T> filter)
		{
			collection.RemoveRange(collection.Where((T t) => filter(t)).ToArray());
		}

		public static void RemoveAll<T>(this IList collection, Predicate<T> filter)
		{
			T[] array = (from t in collection.OfType<T>()
				where filter(t)
				select t).ToArray();
			foreach (T val in array)
			{
				collection.Remove(val);
			}
		}

		public static T[] Sort<T>(this T[] array)
		{
			Array.Sort(array);
			return array;
		}

		public static IEnumerable<T> Recurse<T>(this IEnumerable list, Func<object, IEnumerable> getItems, bool bottomUp = false, int maxLevel = -1) where T : class
		{
			if (maxLevel == 0)
			{
				yield break;
			}
			foreach (object t in list)
			{
				T ti = t as T;
				if (ti != null && !bottomUp)
				{
					yield return ti;
				}
				IEnumerable enumerable = getItems?.Invoke(t);
				if (enumerable != null)
				{
					foreach (T item in enumerable.Recurse<T>(getItems, bottomUp, maxLevel - 1))
					{
						yield return item;
					}
				}
				if (ti != null && bottomUp)
				{
					yield return ti;
				}
			}
		}

		public static int GetChildLevel<T>(this IEnumerable list, T item, Func<object, IList> getItems, int level) where T : class
		{
			foreach (object item2 in list)
			{
				T val = item2 as T;
				if (val != null && val.Equals(item))
				{
					return level;
				}
				IList list2 = getItems?.Invoke(item2);
				if (list2 != null)
				{
					int childLevel = list2.GetChildLevel(item, getItems, level + 1);
					if (childLevel != -1)
					{
						return childLevel;
					}
				}
			}
			return -1;
		}

		public static void Dispose(this IEnumerable list)
		{
			list?.OfType<IDisposable>().SafeForEach(delegate(IDisposable d)
			{
				d.Dispose();
			});
		}

		public static void RemoveReference(this IList list, object value)
		{
			int num = list.IndexOfReference(value);
			if (num != -1)
			{
				list.RemoveAt(num);
			}
		}

		public static void Move(this IList list, int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				object value = list[oldIndex];
				list.RemoveAt(oldIndex);
				if (newIndex > oldIndex + 1)
				{
					newIndex--;
				}
				list.Insert(newIndex, value);
			}
		}

		public static IEnumerable<T> Lock<T>(this IEnumerable<T> list, bool useSyncRoot = false)
		{
			ICollection collection = (useSyncRoot ? (list as ICollection) : null);
			using (ItemMonitor.Lock((collection == null) ? list : collection.SyncRoot))
			{
				foreach (T item in list)
				{
					yield return item;
				}
			}
		}

		public static IEnumerable<T> AsEnumerable<T>(params T[] data)
		{
			return data;
		}

		public static T Max<T>(this IEnumerable<T> items, Comparison<T> comparision)
		{
			return items.Aggregate((T a, T b) => (comparision(a, b) <= 0) ? b : a);
		}

		public static IEnumerable<T> AddFirst<T>(this IEnumerable<T> list, T item)
		{
			IEnumerable<T> enumerable = AsEnumerable<T>(item);
			if (list != null)
			{
				return enumerable.Concat(list);
			}
			return enumerable;
		}

		public static IEnumerable<T> AddLast<T>(this IEnumerable<T> list, T item)
		{
			IEnumerable<T> enumerable = AsEnumerable<T>(item);
			if (list != null)
			{
				return list.Concat(enumerable);
			}
			return enumerable;
		}

		public static void ForFirst<T>(this IEnumerable<T> list, Action<T> action)
		{
			list?.Take(1).ForEach(action);
		}

		public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			if (list == null)
			{
				return;
			}
			foreach (T item in list)
			{
				action(item);
			}
		}

		public static void SafeForEach<T>(this IEnumerable<T> list, Action<T> action)
		{
			if (list == null)
			{
				return;
			}
			foreach (T item in list)
			{
				try
				{
					action(item);
				}
				catch (Exception)
				{
				}
			}
		}

		public static bool IsEmpty<T>(this IEnumerable<T> list)
		{
			if (list != null)
			{
				return !list.Any();
			}
			return true;
		}

		public static int FindIndex<T>(this IEnumerable<T> list, Predicate<T> predicate)
		{
			int num = 0;
			foreach (T item in list)
			{
				if (predicate(item))
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static T FirstOrValue<T>(this IEnumerable<T> list, T value)
		{
			using (IEnumerator<T> enumerator = list.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
				return value;
			}
		}

		public static int IndexOfReference(this IEnumerable list, object value)
		{
			int num = 0;
			foreach (object item in list)
			{
				if (item == value)
				{
					return num;
				}
				num++;
			}
			return -1;
		}

		public static ParallelQuery<T> AsParallelSafe<T>(this IEnumerable<T> list)
		{
			if (!(list is ParallelQuery<T>))
			{
				return list.AsParallel();
			}
			return (ParallelQuery<T>)list;
		}

		public static void ParallelForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			if (!ParallelEnabled)
			{
				items.ForEach(action);
				return;
			}
			Exception lastException = null;
			Parallel.ForEach(items, delegate(T item)
			{
				try
				{
					action(item);
				}
				catch (Exception ex)
				{
					lastException = ex;
				}
			});
			if (lastException == null)
			{
				return;
			}
			throw lastException;
		}

		public static void ParallelSort<T>(this IList<T> array, IComparer<T> comparer, bool forceSequential = false)
		{
			array.ParallelSort(comparer.Compare, forceSequential);
		}

		public static void ParallelSort<T>(this IList<T> array, Comparison<T> comparer, bool forceSequential = false)
		{
			array.ParallelSort(0, array.Count - 1, comparer, forceSequential);
		}

		private static void ParallelSort<T>(this IList<T> array, int left, int right, Comparison<T> comparer, bool forceSequential = false)
		{
			forceSequential |= !ParallelEnabled;
			if (left >= right || right <= left)
			{
				return;
			}
			Swap(array, left, (left + right) / 2);
			int last = left;
			for (int i = left + 1; i <= right; i++)
			{
				if (comparer(array[i], array[left]) < 0)
				{
					last++;
					Swap(array, last, i);
				}
			}
			Swap(array, left, last);
			if (forceSequential || last - left < 512)
			{
				array.ParallelSort(left, last - 1, comparer, forceSequential);
				array.ParallelSort(last + 1, right, comparer, forceSequential);
				return;
			}
			Parallel.Invoke(delegate
			{
				array.ParallelSort(left, last - 1, comparer, forceSequential);
			}, delegate
			{
				array.ParallelSort(last + 1, right, comparer, forceSequential);
			});
		}

		private static void Swap<T>(IList<T> array, int i, int j)
		{
			T value = array[i];
			array[i] = array[j];
			array[j] = value;
		}

		public static IEnumerable<T> CatchExceptions<T>(this IEnumerable<T> src, Action<Exception> action = null)
		{
			using (var enumerator = src.GetEnumerator())
			{
				bool next = true;

				while (next)
				{
					try
					{
						next = enumerator.MoveNext();
					}
					catch (Exception ex)
					{
						if (action != null)
							action(ex);
						continue;
					}

					if (next)
						yield return enumerator.Current;
				}
			}
		}
	}
}
