using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Last Year")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Year", DisableOptimizedUpdate = true)]
	public class SmartListSeriesMaxYearMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider != null) ? base.StatsProvider.GetSeriesStats(comicBook).LastYear : 0;
		}
	}
}
