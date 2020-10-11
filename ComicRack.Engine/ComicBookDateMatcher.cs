using System;
using System.Xml.Serialization;
using cYo.Common;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public abstract class ComicBookDateMatcher : ComicBookValueMatcher<DateTime>
	{
		private static readonly string[] opListNeutral = "equals|is after|is before|is in last days|is in range".Split('|');

		private static readonly string[] opList = ComicBookMatcher.TRMatcher.GetStrings("DateOperators", "is|is after|is before|is in the last|is in the range", '|');

		private static readonly string daysText = ComicBookMatcher.TRMatcher["Days", "Days"];

		public override int ArgumentCount
		{
			get
			{
				if (MatchOperator != 4)
				{
					return 1;
				}
				return 2;
			}
		}

		public override string[] OperatorsListNeutral => opListNeutral;

		public override string[] OperatorsList => opList;

		public override string UnitDescription
		{
			get
			{
				if (MatchOperator == 3)
				{
					return daysText;
				}
				return base.UnitDescription;
			}
		}

		public override bool TimeDependant => MatchOperator == 3;

		[XmlIgnore]
		public bool IgnoreTime
		{
			get;
			set;
		}

		public ComicBookDateMatcher()
		{
			IgnoreTime = true;
		}

		protected override bool MatchBook(ComicBook comicBook, DateTime date)
		{
			int num = date.CompareTo(GetMatchValue(comicBook), IgnoreTime);
			switch (MatchOperator)
			{
			case 0:
				return num == 0;
			case 1:
				return num == 1;
			case 2:
				return num == -1;
			case 3:
				return num >= 0;
			case 4:
				if (num >= 0)
				{
					return date.CompareTo(GetMatchValue2(comicBook), IgnoreTime) <= 0;
				}
				return false;
			default:
				return false;
			}
		}

		protected override DateTime ConvertMatchValue(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return DateTime.MinValue;
			}
			if (DateTime.TryParse(MatchValue, out var result))
			{
				return result;
			}
			int.TryParse(MatchValue, out var result2);
			return DateTime.Now - TimeSpan.FromDays(result2);
		}
	}
}
