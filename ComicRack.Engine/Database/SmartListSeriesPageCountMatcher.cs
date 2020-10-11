using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Pages")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "PageCount", DisableOptimizedUpdate = true)]
	public class SmartListSeriesPageCountMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider != null) ? base.StatsProvider.GetSeriesStats(comicBook).PageCount : 0;
		}
	}
}
