using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace cYo.Common.Text
{
	public static class NumberedString
	{
		private static readonly Regex rxBrackets = new Regex("\\s*\\((?<number>\\d+)\\)", RegexOptions.Compiled);

		private static Regex rxRange = new Regex("(?<from>\\d*)\\s*(?<range>-?)\\s*(?<to>\\d*)", RegexOptions.Compiled);

		public static int GetNumber(string s)
		{
			Match match = rxBrackets.Match(s ?? string.Empty);
			int result = 0;
			if (match.Success)
			{
				int.TryParse(match.Groups["number"].Value, out result);
			}
			return result;
		}

		public static string StripNumber(string s)
		{
			return rxBrackets.Replace(s ?? string.Empty, string.Empty);
		}

		public static int MaxNumber(IEnumerable<string> texts)
		{
			try
			{
				return texts.Max((string t) => GetNumber(t) + 1);
			}
			catch (Exception)
			{
				return 0;
			}
		}

		public static string Format(string s, int number)
		{
			if (number >= 2)
			{
				return $"{s} ({number})";
			}
			return s;
		}

		public static bool TestRangeString(this string s, int n)
		{
			if (s == null || s.Trim() == string.Empty)
			{
				return true;
			}
			MatchCollection matchCollection = rxRange.Matches(s);
			if (matchCollection.Count == 0)
			{
				return true;
			}
			foreach (Match item in matchCollection)
			{
				if (item.Length == 0)
				{
					continue;
				}
				int result = 0;
				bool flag = item.Groups["range"].Value == "-";
				int num = (int.TryParse(item.Groups["from"].Value, out result) ? result : int.MinValue);
				int num2 = (int.TryParse(item.Groups["to"].Value, out result) ? result : int.MaxValue);
				if (flag)
				{
					if (n >= num && n <= num2)
					{
						return true;
					}
				}
				else if (n == num || n == num2)
				{
					return true;
				}
			}
			return false;
		}
	}
}
