using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Series complete")]
	[ComicBookMatcherHint("SeriesComplete")]
	public class ComicBookSeriesCompleteMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			return comicBook.SeriesComplete;
		}
	}
}
