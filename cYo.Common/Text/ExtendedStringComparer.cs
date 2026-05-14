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
                return !string.IsNullOrEmpty(s2) ? -1 : 0;

            if (string.IsNullOrEmpty(s2))
				return 1;

			if (s1 == s2)
				return 0;

			int i1 = 0;
			int i2 = 0;
			bool zeroesFirst = (compareMode & ExtendedStringComparison.ZeroesFirst) != 0;
			bool ignoreCase = (compareMode & ExtendedStringComparison.IgnoreCase) != 0;
			bool ordinal = (compareMode & ExtendedStringComparison.Ordinal) != 0;
			bool ignoreArticles = (compareMode & ExtendedStringComparison.IgnoreArticles) != 0;
			int result = 0;

			if (ignoreArticles)
			{
				i1 = s1.IndexAfterArticle();
				i2 = s2.IndexAfterArticle();
				result = Math.Sign(i2 - i1);
            }

			int length = s1.Length;
			int length2 = s2.Length;
			bool isLetterOrDigit_i1 = char.IsLetterOrDigit(s1[i1]);
			bool isLetterOrDigit_i2 = char.IsLetterOrDigit(s2[i2]);

            if (isLetterOrDigit_i1 && !isLetterOrDigit_i2)
                return 1;

            if (!isLetterOrDigit_i1 && isLetterOrDigit_i2)
                return -1;

            do
			{
				char c1 = s1[i1];
				char c2 = s2[i2];
                bool isDigit_c1 = char.IsDigit(c1);
                bool isDigit_c2 = char.IsDigit(c2);

                if (!isDigit_c1 && !isDigit_c2)
				{
					if (c1 != c2)
					{
						bool isLetter_c1 = char.IsLetter(c1);
						bool isLetter_c2 = char.IsLetter(c2);
						if (isLetter_c1 && isLetter_c2)
						{
							int num = ordinal 
								? (char.ToUpper(c1) - char.ToUpper(c2)) 
								: string.Compare(s1, i1, s2, i2, 1, ignoreCase);

							if (num != 0)
								return num;
						}
						else
						{
                            if (isLetter_c1 || isLetter_c2)
                                return isLetter_c1 ? 1 : -1;

                            int num2 = ordinal
								? (c1 - c2) 
								: string.Compare(s1, i1, s2, i2, 1);

							if (num2 != 0)
								return num2;
						}
					}
				}
				else
				{
                    if (!(isDigit_c1 && isDigit_c2))
                        return isDigit_c1 ? -1 : 1;

                    int num3 = CompareNumbers(s1, length, ref i1, s2, length2, ref i2, zeroesFirst);
                    if (num3 != 0)
                        return num3;
                }

				i1++;
				i2++;

                if (i1 >= length)
                    return i2 >= length2 ? result : -1;
            }
			while (i2 < length2);
			return 1;
		}

		private static int CompareNumbers(string s1, int s1Length, ref int i1, string s2, int s2Length, ref int i2, bool zeroesFirst)
		{
			int nzStart1 = i1;
			int nzStart2 = i2;
			int end1 = i1;
			int end2 = i2;
			ScanNumber(s1, s1Length, i1, ref nzStart1, ref end1);
			ScanNumber(s2, s2Length, i2, ref nzStart2, ref end2);

			int start1 = i1;
			int start2 = i2;

            i1 = end1 - 1;
			i2 = end2 - 1;

			if (zeroesFirst)
			{
                int leadingZeroCount1 = nzStart1 - start1;
				int leadingZeroCount2 = nzStart2 - start2;

				if (leadingZeroCount1 > leadingZeroCount2)
					return -1;
				if (leadingZeroCount1 < leadingZeroCount2)
					return 1;
			}
			int significandLength1 = end2 - nzStart2;
			int significandLength2 = end1 - nzStart1;

			if (significandLength1 == significandLength2)
			{
				int cursor1 = nzStart1;
				int cursor2 = nzStart2;

				while (cursor1 <= i1)
				{
					int digitDiff = s1[cursor1] - s2[cursor2];
					if (digitDiff != 0)
						return digitDiff;

					cursor1++;
					cursor2++;
				}

                // Significands equal; use total length (including leading zeros) as tiebreaker
                int totalLength1 = end1 - start1;
                int totalLength2 = end2 - start2;

                if (totalLength1 == totalLength2)
					return 0;
                if (totalLength1 > totalLength2)
                    return -1;
                return 1;
            }

            if (significandLength1 > significandLength2)
                return -1;
            return 1;
        }

		private static void ScanNumber(string s, int length, int start, ref int nzStart, ref int end)
		{
			nzStart = start;
			end = start;
			bool parsingLeadingZeros = true;
			char c = s[end];
			do
			{
				if (parsingLeadingZeros)
				{
					if ('0' == c)
					{
						nzStart++;
					}
					else
					{
						parsingLeadingZeros = false;
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
