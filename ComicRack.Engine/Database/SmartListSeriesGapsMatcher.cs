using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Gaps")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Number", DisableOptimizedUpdate = true)]
	public class SmartListSeriesGapsMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).GapCount;
			}
			return 0f;
		}
	}
}
