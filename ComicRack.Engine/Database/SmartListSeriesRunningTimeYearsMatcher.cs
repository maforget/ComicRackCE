using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Running Time Years")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Year", DisableOptimizedUpdate = true)]
	public class SmartListSeriesRunningTimeYearsMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider != null) ? base.StatsProvider.GetSeriesStats(comicBook).RunningTimeYears : 0;
		}
	}
}
