using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;

namespace cYo.Common.ComponentModel
{
	public static class ComparerExtension
	{
		private class WrapComparer<T> : IComparer<T>
		{
			private readonly IComparer comparer;

			public WrapComparer(IComparer comparer)
			{
				this.comparer = comparer;
			}

			public int Compare(T x, T y)
			{
				return comparer.Compare(x, y);
			}
		}

		private class CastComparer<T> : IComparer<T>
		{
			private readonly IComparer comparer;

			public CastComparer(IComparer comparer)
			{
				this.comparer = comparer;
			}

			public int Compare(T x, T y)
			{
				return comparer.Compare(x, y);
			}
		}

		public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
		{
			return new ReverseComparer<T>(comparer);
		}

		public static IComparer<T> Wrap<T>(this IComparer comparer)
		{
			return new WrapComparer<T>(comparer);
		}

		public static IComparer<T> Chain<T>(this IEnumerable<IComparer<T>> comparerList)
		{
			return new ChainedComparer<T>(comparerList);
		}

		public static IComparer<T> Chain<T>(this IComparer<T> comparer, params IComparer<T>[] comparers)
		{
			return comparers.AddFirst(comparer).Chain();
		}

		public static int Compare<T>(this IEnumerable<IComparer<T>> comparerList, T a, T b)
		{
			return comparerList.Select((IComparer<T> c) => c.Compare(a, b)).FirstOrDefault((int r) => r != 0);
		}

		public static IComparer<T> Cast<T>(this IComparer comparer)
		{
			return new CastComparer<T>(comparer);
		}

		public static int Compare<T>(T a, T b, params Comparison<T>[] comparers)
		{
			return comparers.Select((Comparison<T> c) => c(a, b)).FirstOrDefault((int n) => n != 0);
		}
	}
}
