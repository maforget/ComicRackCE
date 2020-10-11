using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Highest Count")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Count", DisableOptimizedUpdate = true)]
	public class SmartListSeriesMaxCountMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider == null) ? (-1) : base.StatsProvider.GetSeriesStats(comicBook).MaxCount;
		}
	}
}
