using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Average Community Rating")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "CommunityRating", DisableOptimizedUpdate = true)]
	public class SmartListSeriesAverageCommunityRatingMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).AverageCommunityRating;
			}
			return 0f;
		}
	}
}
