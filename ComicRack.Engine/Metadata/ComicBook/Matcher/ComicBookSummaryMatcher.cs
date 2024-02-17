using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Summary")]
	[ComicBookMatcherHint("Summary")]
	public class ComicBookSummaryMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Summary;
		}
	}
}
