using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: First Year")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Year", DisableOptimizedUpdate = true)]
	public class SmartListSeriesMinYearMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider != null) ? base.StatsProvider.GetSeriesStats(comicBook).FirstYear : 0;
		}
	}
}
