using System.Collections.Generic;

namespace cYo.Common.ComponentModel
{
	public class ReverseComparer<T> : Comparer<T>
	{
		private readonly IComparer<T> comparer;

		public ReverseComparer(IComparer<T> comparer)
		{
			this.comparer = comparer;
		}

		public override int Compare(T x, T y)
		{
			if (comparer != null)
			{
				return comparer.Compare(y, x);
			}
			return 0;
		}
	}
}
