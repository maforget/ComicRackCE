using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Book added")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "AddedTime", DisableOptimizedUpdate = true)]
	public class SmartListSeriesLastAddedTimeMatcher : ComicBookDateMatcher
	{
		protected override DateTime GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).LastAddedTime;
			}
			return DateTime.MinValue;
		}
	}
}
