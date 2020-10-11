using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Book released")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "ReleasedTime", DisableOptimizedUpdate = true)]
	public class SmartListSeriesLastReleasedTimeMatcher : ComicBookDateMatcher
	{
		protected override DateTime GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).LastReleasedTime;
			}
			return DateTime.MinValue;
		}
	}
}
