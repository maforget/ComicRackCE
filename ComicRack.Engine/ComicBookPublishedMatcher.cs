using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Published")]
	[ComicBookMatcherHint("Year", "Month", "Day", "FilePath", "EnableProposed")]
	public class ComicBookPublishedMatcher : ComicBookDateMatcher
	{
		protected override DateTime GetValue(ComicBook comicBook)
		{
			return comicBook.Published;
		}
	}
}
