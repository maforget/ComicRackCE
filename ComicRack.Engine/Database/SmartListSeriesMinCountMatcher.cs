using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Lowest Count")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Count", DisableOptimizedUpdate = true)]
	public class SmartListSeriesMinCountMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return (base.StatsProvider == null) ? (-1) : base.StatsProvider.GetSeriesStats(comicBook).MinCount;
		}
	}
}
