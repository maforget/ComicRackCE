using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;

namespace cYo.Common.ComponentModel
{
	public class ChainedComparer<T> : Comparer<T>
	{
		private readonly IComparer<T>[] comparers;

		private readonly int len;

		public ChainedComparer(IEnumerable<IComparer<T>> comparers)
		{
			this.comparers = comparers.Where((IComparer<T> c) => c != null).Distinct(Equality<IComparer<T>>.TypeEquality).ToArray();
			len = this.comparers.Length;
		}

		public ChainedComparer(params IComparer<T>[] comparers)
			: this((IEnumerable<IComparer<T>>)comparers)
		{
		}

		public override int Compare(T x, T y)
		{
			int result = 0;
			for (int i = 0; i < len; i++)
			{
				IComparer<T> comparer = comparers[i];
				if (comparer != null && (result = comparer.Compare(x, y)) != 0)
				{
					break;
				}
			}
			return result;
		}
	}
}
