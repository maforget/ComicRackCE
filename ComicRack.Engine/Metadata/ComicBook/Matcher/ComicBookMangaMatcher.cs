using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
    [Serializable]
    [Description("Manga")]
    [ComicBookMatcherHint("Manga")]
    public class ComicBookMangaMatcher : ComicBookValueMatcher<MangaYesNo>
    {
        private static readonly string[] opListNeutral = "equals yes|equals ltr|equals no|equals unknown".Split('|');

        private static readonly string[] opList = ComicBookMatcher.TRMatcher.GetStrings("MangaYesNoOperators", "is Yes|is Yes (Left to Right)|is No|is Unknown", '|');

        public override string[] OperatorsListNeutral => opListNeutral;

        public override string[] OperatorsList => opList;

        public override int ArgumentCount => 0;

        protected override MangaYesNo GetValue(ComicBook comicBook)
        {
            return comicBook.Manga;
        }

        protected override bool MatchBook(ComicBook comicBook, MangaYesNo yesNo)
        {
            switch (MatchOperator)
            {
                default:
                    return yesNo == MangaYesNo.Yes;
                case 1:
                    return yesNo == MangaYesNo.YesAndRightToLeft;
                case 2:
                    return yesNo == MangaYesNo.No;
                case 3:
                    return yesNo == MangaYesNo.Unknown;
            }
        }
    }
}
