using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Opened")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "OpenedTime", DisableOptimizedUpdate = true)]
	public class SmartListSeriesLastOpenedTimeMatcher : ComicBookDateMatcher
	{
		protected override DateTime GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).LastOpenedTime;
			}
			return DateTime.MinValue;
		}
	}
}
