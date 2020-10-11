using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Book Count")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", DisableOptimizedUpdate = true)]
	public class SmartListSeriesCountMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider != null) ? base.StatsProvider.GetSeriesStats(comicBook).Count : 0;
		}
	}
}
