using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cYo.Common.Localize;
using cYo.Common.Text;

namespace cYo.Common.ComponentModel
{
	public class GroupInfo : IGroupInfo, IComparable<IGroupInfo>
	{
		public static readonly TR TRGroup = TR.Load("Groups");

		public static readonly string Unspecified = TR.Default["Unspecified", "Unspecified"];

		private static readonly string[] DateGroups = TRGroup.GetStrings("DateGroups", "Never|Today|Yesterday|Two Days Ago|Three Days Ago|This Week|Last Week|Two Weeks Ago|Three Weeks Ago|This Month|Last Month|Two Months ago|Three Months ago|Four Months ago|Five Months ago|Six Months ago|This Year|Last Year|Two Years ago|Three Years ago|Older than Three Years|Next Year|In the Future", '|');

		private static readonly string[] alphabetCaptions = TRGroup.GetStrings("AlphabetGroups", "0-9|A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z|Other", '|');

		private static readonly string[] sizeGroups = TRGroup.GetStrings("SizeGroups", "Empty|Very Small|Small|Medium|Big|Huge", '|');

		public object Key
		{
			get;
			set;
		}

		public int Index
		{
			get;
			set;
		}

		public string Caption
		{
			get;
			set;
		}

		public GroupInfo(object key, string caption, int index = -1)
		{
			Key = key;
			Caption = caption;
			Index = index;
		}

		public GroupInfo(string caption, int index)
			: this(caption, caption, index)
		{
		}

		public GroupInfo(string caption)
			: this(caption, caption)
		{
		}

		public static IGroupInfo GetDateGroup(DateTime date, string unknown = null)
		{
			try
			{
				DateTime now = DateTime.Now;
				int nowMonth = now.Year * 12 + now.Month;
				int nowDayOfYear = now.Year * 366 + now.DayOfYear;
				int dateMonth = date.Year * 12 + date.Month;
				int dateDayOfYear = date.Year * 366 + date.DayOfYear;

				if (date.CompareTo(DateTime.MinValue, ignoreTime: true) == 0) // Never
					return new GroupInfo(unknown ?? DateGroups[0], 1000);

				if (nowDayOfYear == dateDayOfYear) //Today
					return new GroupInfo(DateGroups[1], 0);

				if (nowDayOfYear - 1 == dateDayOfYear) // Yesterday
					return new GroupInfo(DateGroups[2], 1);

				if (nowDayOfYear - 2 == dateDayOfYear) // Two Days Ago
					return new GroupInfo(DateGroups[3], 2); 

				if (nowDayOfYear - 3 == dateDayOfYear) // Three Days Ago
					return new GroupInfo(DateGroups[4], 3);

				if (nowDayOfYear / 7 == dateDayOfYear / 7) // This Week
					return new GroupInfo(DateGroups[5], 10);

				if (nowDayOfYear / 7 - 1 == dateDayOfYear / 7) // Last Week
					return new GroupInfo(DateGroups[6], 11);

				if (nowDayOfYear / 7 - 2 == dateDayOfYear / 7) // Two Weeks Ago
					return new GroupInfo(DateGroups[7], 12);

				if (nowDayOfYear / 7 - 3 == dateDayOfYear / 7) // Three Weeks Ago
					return new GroupInfo(DateGroups[8], 13);

				if (nowMonth == dateMonth) // This Month
					return new GroupInfo(DateGroups[9], 20);

				if (nowMonth - 1 == dateMonth) // Last Month
					return new GroupInfo(DateGroups[10], 21);

				if (nowMonth - 2 == dateMonth) // Two Months Ago
					return new GroupInfo(DateGroups[11], 22);

				if (nowMonth - 3 == dateMonth) // Three Months Ago
					return new GroupInfo(DateGroups[12], 23);

				if (nowMonth - 4 == dateMonth) // Four Months Ago
					return new GroupInfo(DateGroups[13], 24);

				if (nowMonth - 5 == dateMonth) // Five Months Ago
					return new GroupInfo(DateGroups[14], 25);

				if (nowMonth - 6 == dateMonth) // Six Months Ago
					return new GroupInfo(DateGroups[15], 26);

				if (now.Year == date.Year) // This Year
					return new GroupInfo(DateGroups[16], 30);

				if (now.Year - 1 == date.Year) // Last Year
					return new GroupInfo(DateGroups[17], 31);

				if (now.Year - 2 == date.Year) // Two Years Ago
					return new GroupInfo(DateGroups[18], 32);

				if (now.Year - 3 == date.Year) // Three Years Ago
					return new GroupInfo(DateGroups[19], 33);

				if (now.Year + 1 == date.Year) // Next Year
					return new GroupInfo(DateGroups[21], 34);

				if (now.Year < date.Year) // In the Future
					return new GroupInfo(DateGroups[22], 35);

				return new GroupInfo(DateGroups[20], 34); // Older than Three Years
			}
			catch
			{
				return new GroupInfo("Missing Group Value", 100);
			}
		}

		public static string CompressedName(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = text.Split(StringUtility.CommonSeparators);
			foreach (string text2 in array)
			{
				if (!string.IsNullOrEmpty(text2) && !text2.IsArticle())
				{
					stringBuilder.Append(text2);
				}
			}
			return stringBuilder.ToString();
		}

		public static IGroupInfo GetCompressedNameGroup(string text)
		{
			string key = CompressedName(text).ToUpper();
			return new GroupInfo(key, string.IsNullOrEmpty(text) ? Unspecified : text);
		}

		public static IEnumerable<IGroupInfo> GetCompressedNameGroups(string text)
		{
			if (!text.Contains(","))
			{
				return new IGroupInfo[1]
				{
					GetCompressedNameGroup(text)
				};
			}
			return text.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(GetCompressedNameGroup);
		}

		public static IGroupInfo GetAlphabetGroup(string text, bool articleAware)
		{
			int num;
			if (string.IsNullOrEmpty(text))
			{
				num = alphabetCaptions.Length - 1;
			}
			else
			{
				int index = (articleAware ? text.IndexAfterArticle() : 0);
				char c = char.ToUpper(text[index]).Normalize();
				if (char.IsDigit(c))
				{
					num = 0;
				}
				else
				{
					num = c - 65 + 1;
					if (num < 1 || num > alphabetCaptions.Length - 2)
					{
						num = alphabetCaptions.Length - 1;
					}
				}
			}
			return new GroupInfo(alphabetCaptions[num], num);
		}

		public static IGroupInfo GetFileSizeGroup(long size)
		{
			if (size <= 0)
			{
				return new GroupInfo(sizeGroups[0], 0);
			}
			if (size < 1048576)
			{
				return new GroupInfo(sizeGroups[1], 1);
			}
			if (size < 10485760)
			{
				return new GroupInfo(sizeGroups[2], 2);
			}
			if (size < 52428800)
			{
				return new GroupInfo(sizeGroups[3], 3);
			}
			if (size < 104857600)
			{
				return new GroupInfo(sizeGroups[4], 4);
			}
			return new GroupInfo(sizeGroups[5], 5);
		}

		public static int Compare(IGroupInfo x, IGroupInfo y)
		{
			if (x == null && y == null)
			{
				return 0;
			}
			if (x == null)
			{
				return -1;
			}
			if (y == null)
			{
				return 1;
			}
			if (x.Index == y.Index)
			{
				return ExtendedStringComparer.Compare(x.Caption, y.Caption, ExtendedStringComparison.IgnoreArticles);
			}
			return x.Index.CompareTo(y.Index);
		}

		public int CompareTo(IGroupInfo other)
		{
			return Compare(this, other);
		}
	}
}
