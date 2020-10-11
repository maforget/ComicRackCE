using System;
using System.Collections;
using System.Collections.Generic;

namespace cYo.Common.Text
{
	public sealed class ExtendedStringComparer : IComparer<string>
	{
		private readonly ExtendedStringComparison compareMode;

		private static readonly IComparer<string> defaultComparer = new ExtendedStringComparer();

		private static readonly IComparer<string> defaultZeroesFirst = new ExtendedStringComparer(ExtendedStringComparison.ZeroesFirst);

		private static readonly IComparer<string> defaultNoArticles = new ExtendedStringComparer(ExtendedStringComparison.IgnoreArticles);

		public static IComparer<string> Default => defaultComparer;

		public static IComparer<string> DefaultZeroesFirst => defaultZeroesFirst;

		public static IComparer<string> DefaultNoArticles => defaultNoArticles;

		public ExtendedStringComparer(ExtendedStringComparison compareMode)
		{
			this.compareMode = compareMode;
		}

		public ExtendedStringComparer()
			: this(ExtendedStringComparison.Default)
		{
		}

		int IComparer<string>.Compare(string x, string y)
		{
			return Compare(x, y, compareMode);
		}

		public int Compare(object x, object y)
		{
			string text = x as string;
			string text2 = y as string;
			if (text != null && text2 != null)
			{
				return Compare(text, text2, compareMode);
			}
			return Comparer.Default.Compare(x, y);
		}

		public static int Compare(string s1, string s2)
		{
			return Compare(s1, s2, ExtendedStringComparison.Default);
		}

		public static int Compare(string s1, string s2, ExtendedStringComparison compareMode)
		{
			if (string.IsNullOrEmpty(s1))
			{
				if (!string.IsNullOrEmpty(s2))
				{
					return -1;
				}
				return 0;
			}
			if (string.IsNullOrEmpty(s2))
			{
				return 1;
			}
			if (s1 == s2)
			{
				return 0;
			}
			int i = 0;
			int i2 = 0;
			bool zeroesFirst = (compareMode & ExtendedStringComparison.ZeroesFirst) != 0;
			bool ignoreCase = (compareMode & ExtendedStringComparison.IgnoreCase) != 0;
			bool flag = (compareMode & ExtendedStringComparison.Ordinal) != 0;
			bool flag2 = (compareMode & ExtendedStringComparison.IgnoreArticles) != 0;
			int result = 0;
			if (flag2)
			{
				i = s1.IndexAfterArticle();
				i2 = s2.IndexAfterArticle();
				result = Math.Sign(i2 - i);
			}
			int length = s1.Length;
			int length2 = s2.Length;
			bool flag3 = char.IsLetterOrDigit(s1[i]);
			bool flag4 = char.IsLetterOrDigit(s2[i2]);
			if (flag3 && !flag4)
			{
				return 1;
			}
			if (!flag3 && flag4)
			{
				return -1;
			}
			do
			{
				char c = s1[i];
				char c2 = s2[i2];
				flag3 = char.IsDigit(c);
				flag4 = char.IsDigit(c2);
				if (!flag3 && !flag4)
				{
					if (c != c2)
					{
						bool flag5 = char.IsLetter(c);
						bool flag6 = char.IsLetter(c2);
						if (flag5 && flag6)
						{
							int num = (flag ? (char.ToUpper(c) - char.ToUpper(c2)) : string.Compare(s1, i, s2, i2, 1, ignoreCase));
							if (num != 0)
							{
								return num;
							}
						}
						else
						{
							if (flag5 || flag6)
							{
								if (flag5)
								{
									return 1;
								}
								return -1;
							}
							int num2 = (flag ? (c - c2) : string.Compare(s1, i, s2, i2, 1));
							if (num2 != 0)
							{
								return num2;
							}
						}
					}
				}
				else
				{
					if (!(flag3 && flag4))
					{
						if (flag3)
						{
							return -1;
						}
						return 1;
					}
					int num3 = CompareNumbers(s1, length, ref i, s2, length2, ref i2, zeroesFirst);
					if (num3 != 0)
					{
						return num3;
					}
				}
				i++;
				i2++;
				if (i >= length)
				{
					if (i2 >= length2)
					{
						return result;
					}
					return -1;
				}
			}
			while (i2 < length2);
			return 1;
		}

		private static int CompareNumbers(string s1, int s1Length, ref int i1, string s2, int s2Length, ref int i2, bool zeroesFirst)
		{
			int nzStart = i1;
			int nzStart2 = i2;
			int end = i1;
			int end2 = i2;
			ScanNumber(s1, s1Length, i1, ref nzStart, ref end);
			ScanNumber(s2, s2Length, i2, ref nzStart2, ref end2);
			int num = i1;
			i1 = end - 1;
			int num2 = i2;
			i2 = end2 - 1;
			if (zeroesFirst)
			{
				int num3 = nzStart - num;
				int num4 = nzStart2 - num2;
				if (num3 > num4)
				{
					return -1;
				}
				if (num3 < num4)
				{
					return 1;
				}
			}
			int num5 = end2 - nzStart2;
			int num6 = end - nzStart;
			if (num5 == num6)
			{
				int num7 = nzStart;
				int num8 = nzStart2;
				while (num7 <= i1)
				{
					int num9 = s1[num7] - s2[num8];
					if (num9 != 0)
					{
						return num9;
					}
					num7++;
					num8++;
				}
				num5 = end - num;
				num6 = end2 - num2;
				if (num5 == num6)
				{
					return 0;
				}
			}
			if (num5 > num6)
			{
				return -1;
			}
			return 1;
		}

		private static void ScanNumber(string s, int length, int start, ref int nzStart, ref int end)
		{
			nzStart = start;
			end = start;
			bool flag = true;
			char c = s[end];
			do
			{
				if (flag)
				{
					if ('0' == c)
					{
						nzStart++;
					}
					else
					{
						flag = false;
					}
				}
				end++;
				if (end < length)
				{
					c = s[end];
					continue;
				}
				break;
			}
			while (char.IsDigit(c));
		}
	}
}
