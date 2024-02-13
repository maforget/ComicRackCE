using System;
using System.Xml.Serialization;
using cYo.Common;

namespace cYo.Projects.ComicRack.Engine
{
    [Serializable]
    public abstract class ComicBookDateMatcher : ComicBookValueMatcher<DateTime>
    {
        public const int OperatorEquals = 0;

        public const int OperatorIsAfter = 1;

        public const int OperatorIsBefore = 2;

        public const int OperatorIsInLastDays = 3;

        public const int OperatorIsInRange = 4;

        private static readonly string[] opListNeutral = "equals|is after|is before|is in last days|is in range".Split('|');

        private static readonly string[] opList = ComicBookMatcher.TRMatcher.GetStrings("DateOperators", "is|is after|is before|is in the last|is in the range", '|');

        private static readonly string daysText = ComicBookMatcher.TRMatcher["Days", "Days"];

        public override int ArgumentCount
        {
            get
            {
                if (MatchOperator != OperatorIsInRange)
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
                if (MatchOperator == OperatorIsInLastDays)
                {
                    return daysText;
                }
                return base.UnitDescription;
            }
        }

        public override bool TimeDependant => MatchOperator == OperatorIsInLastDays;

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
                case OperatorEquals:
                    return num == 0;
                case OperatorIsAfter:
                    return num == 1;
                case OperatorIsBefore:
                    return num == -1;
                case OperatorIsInLastDays:
                    return num >= 0;
                case OperatorIsInRange:
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
            if (DateTime.TryParse(input, out var result))
            {
                return result;
            }
            int.TryParse(input, out var result2);
            return DateTime.Now - TimeSpan.FromDays(result2);
        }
    }
}
