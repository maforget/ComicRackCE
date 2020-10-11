using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Average Rating")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Rating", DisableOptimizedUpdate = true)]
	public class SmartListSeriesAverageRatingMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).AverageRating;
			}
			return 0f;
		}
	}
}
