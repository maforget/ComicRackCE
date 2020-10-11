using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Series Group")]
	[ComicBookMatcherHint("SeriesGroup")]
	public class ComicBookSeriesGroupMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.SeriesGroup;
		}
	}
}
