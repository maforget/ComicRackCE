using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Percent Read")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "PageCount", "LastPageRead", DisableOptimizedUpdate = true)]
	public class SmartListSeriesPercentReadMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider != null) ? base.StatsProvider.GetSeriesStats(comicBook).ReadPercentage : 0;
		}
	}
}
