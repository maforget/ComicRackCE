using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using cYo.Common.Collections;

namespace cYo.Common.Text
{
	public static class StringUtility
	{
		[Flags]
		public enum PrefixOptions
		{
			None = 0x0,
			RemovePrefixes = 0x1,
			UseCaptialWordStarts = 0x2,
			UseSmallWordStarts = 0x4,
			UseNonStartingLetters = 0x8,
			Default = 0xF
		}

		public enum ShortenTextOptions
		{
			None = 0,
			RemoveSpaces = 1,
			RemoveNonWordLetters = 2,
			RemoveUppercaseWords = 4,
			RemoveFillwWords = 8,
			Default = 0xF
		}

		public static readonly char[] CommonSeparators;

		private static string[] articles;

		private static string[] articlesWithSpaces;

		private static readonly Regex rxFloat;

		private static readonly Regex rxInt;

		private static readonly Regex rxUpperWordStart;

		private static readonly Regex rxLowerWordStart;

		private static readonly Regex rxmiddleLetters;

		private static readonly Regex rxRemoveFillWords;

		private static readonly Regex rxStartWord;

		private static readonly Regex rxNonWordLetters;

		private static readonly Regex rxSpace;

		private static readonly Regex rxMultiSpace;

		public static string Articles
		{
			get
			{
				return articles.ToListString(", ");
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string[] array = value.Split(',');
				foreach (string text in array)
				{
					string text2 = text.Trim();
					if (!string.IsNullOrEmpty(text2))
					{
						list.Add(text2);

						string articleWithSpace = text2.EndsWith("\'") ? text2 : $"{text2} ";
                        list2.Add(articleWithSpace);
					}
				}
				articles = list.ToArray();
				articlesWithSpaces = list2.ToArray();
			}
		}

		static StringUtility()
		{
			CommonSeparators = new char[15]
			{
				' ',
				'\t',
				'\n',
				'\r',
				'-',
				'~',
				',',
				'.',
				';',
				':',
				'/',
				'\\',
				'\'',
				'\u00b4',
				'`'
			};
			rxFloat = new Regex("[-\\+]?\\d*\\.?\\d+", RegexOptions.Compiled);
			rxInt = new Regex("[-\\+]?\\d+", RegexOptions.Compiled);
			rxUpperWordStart = new Regex("\\b(?<letter>)[A-Z0-9]", RegexOptions.Compiled);
			rxLowerWordStart = new Regex("\\b(?<letter>[a-z])", RegexOptions.Compiled);
			rxmiddleLetters = new Regex("\\B(?<letter>[a-z0-9])", RegexOptions.Compiled);
			rxRemoveFillWords = new Regex("(?<!\\A)\\b(for|the|of|to|in|at|by|and|or)\\b", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			rxStartWord = new Regex("\\b\\w", RegexOptions.Compiled);
			rxNonWordLetters = new Regex("[^\\s\\w]", RegexOptions.Compiled);
			rxSpace = new Regex("\\s", RegexOptions.Compiled);
			rxMultiSpace = new Regex("\\s{2,}", RegexOptions.Compiled);
			Articles = "the, der, die, das, le, la, les, l'";
		}

		public static void ConvertIndexToLineAndColumn(string text, int index, out int line, out int column)
		{
			line = 1;
			column = 1;
			foreach (char c in text)
			{
				if (index-- == 0)
				{
					break;
				}
				switch (c)
				{
				case '\n':
					line++;
					column = 1;
					break;
				default:
					column++;
					break;
				case '\r':
					break;
				}
			}
		}

		public static string Escape(this string text, IEnumerable<char> characters, char escape)
		{
			foreach (char item in ListExtensions.AsEnumerable<char>(escape).Concat(characters))
			{
				string text2 = item.ToString();
				text = text.Replace(text2, escape + text2);
			}
			return text;
		}

		public static string Escape(this string text)
		{
			return text.Escape("\"", '\\');
		}

		public static string Unescape(this string text, IEnumerable<char> characters, char escape)
		{
			foreach (char item in characters.Concat(ListExtensions.AsEnumerable<char>(escape)))
			{
				string text2 = item.ToString();
				text = text.Replace(escape + text2, text2);
			}
			return text;
		}

		public static string Unescape(this string text)
		{
			return text.Unescape("\"", '\\');
		}

		public static string[] Split(this string s, char c, StringSplitOptions options)
		{
			return s.Split(new char[1]
			{
				c
			}, options);
		}

		public static string[] Split(this string s, string c, StringSplitOptions options)
		{
			return s.Split(new string[1]
			{
				c
			}, options);
		}

		public static string[] Split(this string s, int lengthFirstPart, int between)
		{
			return new string[2]
			{
				s.Left(lengthFirstPart),
				s.Substring(lengthFirstPart + between)
			};
		}

		public static string[] Split(this string s, int lengthFirstPart)
		{
			return s.Split(lengthFirstPart, 0);
		}

		public static string ToXmlString(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return SecurityElement.Escape(s);
		}

		public static bool IsArticle(this string text)
		{
			return articles.Any((string a) => string.Compare(text, a, ignoreCase: true) == 0);
		}

		public static bool Contains(this string s, string search, StringComparison comparison)
		{
			if (s != null)
			{
				return s.IndexOf(search, comparison) != -1;
			}
			return false;
		}

		public static int IndexAfterArticle(this string s)
		{
			for (int i = 0; i < articlesWithSpaces.Length; i++)
			{
				string text = articlesWithSpaces[i];
				if (s.StartsWith(text, StringComparison.OrdinalIgnoreCase))
				{
					return text.Length;
				}
			}
			return 0;
		}

		public static string Remove(this string s, string text)
		{
			return s.Replace(text, string.Empty);
		}

		public static string RemoveArticle(this string s)
		{
			int num = s.IndexAfterArticle();
			if (num != -1)
			{
				return s.Substring(num);
			}
			return s;
		}

		public static int ExtendedCompareTo(this string a, string b, ExtendedStringComparison mode)
		{
			return ExtendedStringComparer.Compare(a, b, mode);
		}

		public static int ExtendedCompareTo(this string a, string b, bool ignoreCase = false)
		{
			return a.ExtendedCompareTo(b, ignoreCase ? ExtendedStringComparison.IgnoreCase : ExtendedStringComparison.Default);
		}

		public static bool StartsWith(this string a, string b, StringComparison comparisonType, bool ignoreArticles)
		{
			if (!ignoreArticles)
			{
				return a.StartsWith(b, comparisonType);
			}
			int num = a.IndexAfterArticle();
			return a.IndexOf(b, num, Math.Min(b.Length, a.Length - num), comparisonType) == num;
		}

		public static string PascalToSpaced(this string pascalFormattedString)
		{
			if (pascalFormattedString.Contains(" "))
			{
				return pascalFormattedString;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			bool flag2 = false;
			foreach (char c in pascalFormattedString)
			{
				bool flag3 = char.IsUpper(c);
				bool flag4 = char.IsDigit(c);
				if (flag3 && !flag2 && stringBuilder.Length != 0)
				{
					stringBuilder.Append(' ');
				}
				if (flag4 && !flag)
				{
					stringBuilder.Append(' ');
				}
				flag = flag4;
				flag2 = flag3;
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		public static string StartToUpper(this string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in s)
			{
				stringBuilder.Append((stringBuilder.Length == 0) ? char.ToUpper(c) : c);
			}
			return stringBuilder.ToString();
		}

		public static int CompareNumberString(this string a, string b, StringComparison ct, bool invariantNumber)
		{
			if (a.TryParse(out float f, invariantNumber) && b.TryParse(out float f2, invariantNumber))
			{
				return f.CompareTo(f2);
			}
			return string.Compare(a, b, ct);
		}

		public static bool TryParse(this string number, out float f, bool invariant)
		{
			Match match = rxFloat.Match(number ?? string.Empty);
			CultureInfo provider = (invariant ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			f = 0f;
			if (match.Success)
			{
				return float.TryParse(match.Value, NumberStyles.Float, provider, out f);
			}
			return false;
		}

		public static bool TryParse(this string number, out int n, bool invariant)
		{
			Match match = rxInt.Match(number ?? string.Empty);
			CultureInfo provider = (invariant ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture);
			n = 0;
			if (match.Success)
			{
				return int.TryParse(match.Value, NumberStyles.Integer, provider, out n);
			}
			return false;
		}

		public static string ToListString(this IEnumerable list, string separator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object item in list)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append(separator);
				}
				stringBuilder.Append(item.ToString());
			}
			return stringBuilder.ToString();
		}

		public static IEnumerable<string> FromListString(this string listString, char separator)
		{
			if (string.IsNullOrEmpty(listString))
			{
				return Enumerable.Empty<string>();
			}
			if (!listString.Contains(separator.ToString()))
			{
				return ListExtensions.AsEnumerable<string>(listString.Trim());
			}
			return from s in listString.Split(separator)
				select s.Trim() into s
				where !string.IsNullOrEmpty(s)
				select s;
		}

		public static HashSet<string> ListStringToSet(this string listString, char separator)
		{
			return new HashSet<string>(listString.FromListString(separator));
		}

		public static string Format(string format, params object[] args)
		{
			try
			{
				return string.Format(format, args);
			}
			catch
			{
				return format;
			}
		}

		private static string Prefix(char prefix, string s, Regex search, HashSet<char> used)
		{
			foreach (Match item2 in search.Matches(s))
			{
				Group group = item2.Groups[0];
				char item = char.ToUpper(group.Value[0]);
				if (!used.Contains(item))
				{
					used.Add(item);
					return s.Insert(group.Index, prefix.ToString());
				}
			}
			return s;
		}

		public static void Prefix(IList<string> texts, char prefix, PrefixOptions options = PrefixOptions.Default)
		{
			HashSet<char> used = new HashSet<char>();
			Regex regex = new Regex($"\\{prefix}(?=[a-zA-Z0-9])");
			for (int i = 0; i < texts.Count; i++)
			{
				string text = texts[i];
				if ((options & PrefixOptions.RemovePrefixes) != 0)
				{
					text = regex.Replace(text, string.Empty);
				}
				string text2 = text;
				if ((options & PrefixOptions.UseCaptialWordStarts) != 0)
				{
					text2 = Prefix(prefix, text, rxUpperWordStart, used);
				}
				if (text2 == text && (options & PrefixOptions.UseSmallWordStarts) != 0)
				{
					text2 = Prefix(prefix, text, rxLowerWordStart, used);
				}
				if (text2 == text && (options & PrefixOptions.UseNonStartingLetters) != 0)
				{
					text2 = Prefix(prefix, text, rxmiddleLetters, used);
				}
				texts[i] = text2;
			}
		}

		public static bool HasLetters(this IEnumerable<char> text)
		{
			return text.Any((char c) => char.IsLetter(c));
		}

		public static string OnlyDigits(this string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			string text2 = string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (char.IsDigit(c) || c == '.' || c == ',')
				{
					text2 += c;
				}
			}
			return text2;
		}

		public static string RemoveDigits(this string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			string text2 = string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (!char.IsDigit(c))
				{
					text2 += c;
				}
			}
			return text2;
		}

		public static string AppendWithSeparator(this string text, string separator, params string[] texts)
		{
			text = text ?? string.Empty;
			foreach (string text2 in texts)
			{
				if (!string.IsNullOrEmpty(text2))
				{
					if (!string.IsNullOrEmpty(text))
					{
						text += separator;
					}
					text += text2;
				}
			}
			return text;
		}

		public static string MakeEditBoxMultiline(string text)
		{
			return text?.Replace("\r\n", "\n").Replace("\n", "\r\n");
		}

		public static IEnumerable<string> TrimStrings(this IEnumerable<string> list)
		{
			return list.Select((string x) => x.Trim());
		}

		public static IEnumerable<string> TrimEndStrings(this IEnumerable<string> list)
		{
			return list.Select((string x) => x.TrimEnd());
		}

		public static IEnumerable<string> RemoveEmpty(this IEnumerable<string> list)
		{
			return list.Where((string x) => !string.IsNullOrEmpty(x));
		}

		public static string CutOff(this string text, params char[] delimiters)
		{
			int num = text.IndexOfAny(delimiters);
			if (num == -1)
			{
				return text;
			}
			return text.Substring(0, num);
		}

		public static bool IsNumber(this string text)
		{
			foreach (char c in text)
			{
				if (!char.IsNumber(c))
				{
					return false;
				}
			}
			return true;
		}

		public static char Normalize(this char c)
		{
			int num = "ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõöøùúûüýþÿÖöÜüÄä".IndexOf(c);
			if (num == -1)
			{
				return c;
			}
			return "AAAAAAACEEEEIIIIDNOOOOOOUUUUYPSaaaaaaaceeeeiiiionoooooouuuuybyOoUuAa"[num];
		}

		public static string ReplaceAny(this string s, string search, char newChar)
		{
			if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(search))
			{
				return s;
			}
			search.ForEach(delegate(char c)
			{
				s = s.Replace(c, newChar);
			});
			return s;
		}

		public static string ReplaceAny(this string s, string search, string newChar)
		{
			if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(search))
			{
				return s;
			}
			search.ForEach(delegate(char c)
			{
				s = s.Replace(c.ToString(), newChar);
			});
			return s;
		}

		public static bool ContainsAny(this string s, string characters)
		{
			if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(characters))
			{
				return false;
			}
			return characters.Any((char c) => s.Contains(c));
		}

		public static string Left(this string s, int len)
		{
			try
			{
				return s.Substring(0, len);
			}
			catch (Exception)
			{
				return s;
			}
		}

		public static string Ellipsis(this string s, int len, string append, int minLength = 5)
		{
			if (s == null)
			{
				return s;
			}
			len -= append.Length;
			if (len < minLength)
			{
				len = minLength;
			}
			if (s.Length < len)
			{
				return s;
			}
			return s.Left(len) + append;
		}

		public static string LineBreak(this string s, int lineLength)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}
			string[] array = s.Replace(Environment.NewLine, "\n").Split('\n');
			foreach (string text in array)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(Environment.NewLine);
				}
				bool flag = true;
				string[] array2 = text.Split(' ');
				foreach (string value in array2)
				{
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.Append(" ");
					}
					stringBuilder2.Append(value);
					if (stringBuilder2.Length > lineLength)
					{
						if (!flag)
						{
							stringBuilder.Append(Environment.NewLine);
						}
						stringBuilder.Append(stringBuilder2);
						stringBuilder2.Clear();
						flag = false;
					}
				}
				if (stringBuilder2.Length > 0)
				{
					if (!flag)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					stringBuilder.Append(stringBuilder2);
					stringBuilder2.Clear();
				}
			}
			return stringBuilder.ToString();
		}

		public static string Intent(this string s, int intention)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value = new string(' ', intention);
			string[] array = s.Replace(Environment.NewLine, "\n").Split('\n');
			for (int i = 0; i < array.Length; i++)
			{
				string value2 = array[i];
				stringBuilder.Append(value);
				stringBuilder.Append(value2);
				if (i != array.Length - 1)
				{
					stringBuilder.Append(Environment.NewLine);
				}
			}
			return stringBuilder.ToString();
		}

		public static string ToHexString(this byte[] data, bool trimZeros = false)
		{
			StringBuilder stringBuilder = new StringBuilder(data.Length * 2);
			string format = (trimZeros ? "{0:x}" : "{0:x2}");
			foreach (byte b in data)
			{
				stringBuilder.AppendFormat(format, b);
			}
			return stringBuilder.ToString();
		}

		public static string SafeFormat(this string format, params object[] objects)
		{
			try
			{
				return string.Format(format, objects);
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		public static string SafeTrim(this string s)
		{
			return s?.Trim();
		}

		public static string ShortenText(this string text, int maxLength = -1, ShortenTextOptions options = ShortenTextOptions.Default)
		{
			if (options.HasFlag(ShortenTextOptions.RemoveFillwWords))
			{
				text = rxRemoveFillWords.Replace(text, string.Empty);
			}
			if (options.HasFlag(ShortenTextOptions.RemoveUppercaseWords))
			{
				char[] array = text.ToCharArray();
				Match match = rxStartWord.Match(text);
				while (match.Success)
				{
					array[match.Index] = char.ToUpper(array[match.Index]);
					match = match.NextMatch();
				}
				text = new string(array);
			}
			if (options.HasFlag(ShortenTextOptions.RemoveNonWordLetters))
			{
				text = rxNonWordLetters.Replace(text, string.Empty);
			}
			text = ((!options.HasFlag(ShortenTextOptions.RemoveSpaces)) ? rxMultiSpace.Replace(text, " ") : rxSpace.Replace(text, string.Empty));
			return text.Trim().Left(maxLength);
		}
	}
}
