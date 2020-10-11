using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Only Duplicates")]
	[ComicBookMatcherHint("FilePath, EnableProposed, Series, Format, Count, Number, Volume, LanguageISO, Year, Month, Day", DisableOptimizedUpdate = true)]
	public class ComicBookDuplicateMatcher : ComicBookValueMatcher
	{
		private static readonly ComicBookDublicateComparer duplicateComparer = new ComicBookDublicateComparer();

		private static readonly string[] opListNeutral = "on|off".Split('|');

		private static readonly string[] opList = ComicBookMatcher.TRMatcher.GetStrings("OnOffOperators", "On|Off", '|');

		public override string[] OperatorsListNeutral => opListNeutral;

		public override string[] OperatorsList => opList;

		public override int ArgumentCount => 0;

		public override IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items)
		{
			if (MatchOperator != 0)
			{
				return items;
			}
			List<ComicBook> list = items.ToList();
			List<ComicBook> list2 = new List<ComicBook>();
			list.Sort(duplicateComparer);
			ComicBook comicBook = null;
			ComicBook comicBook2 = null;
			foreach (ComicBook item in list)
			{
				if (comicBook == null || duplicateComparer.Compare(comicBook, item) != 0)
				{
					comicBook = (comicBook2 = item);
					continue;
				}
				if (comicBook2 != null)
				{
					list2.Add(comicBook2);
				}
				comicBook2 = null;
				list2.Add(item);
			}
			return list2;
		}
	}
}
