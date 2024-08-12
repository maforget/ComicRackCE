using System;
using System.IO;
using Force.DeepCloner;

namespace cYo.Common
{
	public static class CloneUtility
	{
		public static T Clone<T>(T data) where T : class
		{
			if (data == null)
			{
				return null;
			}
			return data.DeepClone();
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T val = a;
			a = b;
			b = val;
		}

		public static T Clone<T>(this ICloneable data) where T : class
		{
			if (data != null)
			{
				return data.Clone() as T;
			}
			return null;
		}
	}
}
