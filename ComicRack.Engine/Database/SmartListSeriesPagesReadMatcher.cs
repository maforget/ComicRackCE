using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Pages Read")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "LastPageRead", DisableOptimizedUpdate = true)]
	public class SmartListSeriesPagesReadMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider != null) ? base.StatsProvider.GetSeriesStats(comicBook).PageReadCount : 0;
		}
	}
}
