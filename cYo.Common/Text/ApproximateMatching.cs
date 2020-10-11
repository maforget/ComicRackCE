using System;

namespace cYo.Common.Text
{
	public static class ApproximateMatching
	{
		public static int LevenshteinDistance(string s, string t)
		{
			return LevenshteinDistance(s, t, ignoreCase: false);
		}

		public static int LevenshteinDistance(string s, string t, bool ignoreCase)
		{
			int length = s.Length;
			int length2 = t.Length;
			int[] b = new int[length2 + 1];
			int[] a = new int[length2 + 1];
			if (length == 0)
			{
				return length2;
			}
			if (length2 == 0)
			{
				return length;
			}
			if (ignoreCase)
			{
				s = s.ToLower();
				t = t.ToLower();
			}
			int num = 0;
			while (num <= length2)
			{
				b[num] = num++;
			}
			for (int i = 1; i <= length; i++)
			{
				a[0] = i;
				char c = s[i - 1];
				for (int j = 1; j <= length2; j++)
				{
					int num2 = ((t[j - 1] != c) ? 1 : 0);
					a[j] = Math.Min(Math.Min(b[j] + 1, a[j - 1] + 1), b[j - 1] + num2);
				}
				CloneUtility.Swap(ref a, ref b);
			}
			return b[length2];
		}
	}
}
