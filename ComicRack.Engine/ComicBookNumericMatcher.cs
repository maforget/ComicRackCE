using System;

namespace cYo.Projects.ComicRack.Engine
{
    [Serializable]
    public abstract class ComicBookNumericMatcher : ComicBookValueMatcher<float>
    {
        public const int Equal = 0;

        public const int Greater = 1;

        public const int Lesser = 2;

        public const int InRange = 3;

        private static readonly string[] opListNeutral = "equals|is greater|is smaller|in range".Split('|');

        private static readonly string[] opList = ComicBookMatcher.TRMatcher.GetStrings("NumericOperators", "is|is greater|is smaller|is in the range", '|');

        public override int ArgumentCount
        {
            get
            {
                if (MatchOperator != InRange)
                {
                    return 1;
                }
                return 2;
            }
        }

        public override string[] OperatorsListNeutral => opListNeutral;

        public override string[] OperatorsList => opList;

        protected override bool MatchBook(ComicBook comicBook, float n)
        {
            switch (MatchOperator)
            {
                case Equal:
                    return GetMatchValue(comicBook) == n;
                case Greater:
                    return n > GetMatchValue(comicBook);
                case Lesser:
                    return n < GetMatchValue(comicBook);
                case InRange:
                    if (n >= GetMatchValue(comicBook))
                    {
                        return n <= GetMatchValue2(comicBook);
                    }
                    return false;
                default:
                    return false;
            }
        }

        protected override float ConvertMatchValue(string value)
        {
            if (!float.TryParse(value, out var result))
            {
                return GetInvalidValue();
            }
            return result;
        }

        protected override float GetInvalidValue()
        {
            return -1f;
        }
    }
}
