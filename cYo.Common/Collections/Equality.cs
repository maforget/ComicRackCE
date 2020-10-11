using System;
using System.Collections.Generic;

namespace cYo.Common.Collections
{
	public class Equality<T> : EqualityComparer<T>
	{
		private readonly Func<T, T, bool> comparer;

		private readonly Func<T, int> hashCode;

		public static Equality<T> TypeEquality => new Equality<T>((T a, T b) => a.GetType() == b.GetType(), (T a) => a.GetType().GetHashCode());

		public Equality(Func<T, T, bool> comparer, Func<T, int> hashCode)
		{
			this.comparer = comparer;
			this.hashCode = hashCode;
		}

		public override int GetHashCode(T x)
		{
			return hashCode(x);
		}

		public override bool Equals(T x, T y)
		{
			return comparer(x, y);
		}
	}
}
