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
		private static readonly ComicBookDuplicateComparer.EqualityComparer duplicateComparer = new ComicBookDuplicateComparer.EqualityComparer();
		private static readonly ComicBookDuplicateComparer.FilePathComparer filePathComparer = new ComicBookDuplicateComparer.FilePathComparer();

		private static readonly string[] opListNeutral = "on|off".Split('|');

		private static readonly string[] opList = ComicBookMatcher.TRMatcher.GetStrings("OnOffOperators", "On|Off", '|');

		public override string[] OperatorsListNeutral => opListNeutral;

		public override string[] OperatorsList => opList;

		public override int ArgumentCount => 0;

		public override IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items)
		{
			if (MatchOperator != 0)
				return items;

			// Duplicates by metadata
			var metaGroups = items
				.GroupBy(b => b, duplicateComparer)
				.Where(g => g.Count() > 1)
				.SelectMany(g => g).ToList();

			// Duplicates by path
			var pathGroups = items
				.GroupBy(b => b, filePathComparer)
				.Where(g => g.Count() > 1)
				.SelectMany(g => g).ToList();

			// Union to remove overlaps and duplicates
			return metaGroups
				.Concat(pathGroups)
				.Distinct() // Removes books that are both metadata AND path duplicates
				.ToList();
		}
	}
}
