using System;

namespace cYo.Projects.ComicRack.Engine
{
    [Serializable]
    public abstract class ComicBookYesNoMatcher : ComicBookValueMatcher<YesNo>
    {
        private static readonly string[] opListNeutral = "equals yes|equals no|equals unknown".Split('|');

        private static readonly string[] opList = ComicBookMatcher.TRMatcher.GetStrings("YesNoOperators", "is Yes|is No|is Unknown", '|');

        public override string[] OperatorsListNeutral => opListNeutral;

        public override string[] OperatorsList => opList;

        public override int ArgumentCount => 0;

        protected override bool MatchBook(ComicBook comicBook, YesNo yesNo)
        {
            switch (MatchOperator)
            {
                default:
                    return yesNo == YesNo.Yes;
                case 1:
                    return yesNo == YesNo.No;
                case 2:
                    return yesNo == YesNo.Unknown;
            }
        }
    }
}
