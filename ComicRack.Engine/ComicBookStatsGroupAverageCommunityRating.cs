using System;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookStatsGroupAverageCommunityRating : SingleGrouper<ComicBookSeriesStatistics>
	{
		public override IGroupInfo GetGroup(ComicBookSeriesStatistics item)
		{
			return ComicBookGroupRatingBase.GetRatingGroup((int)Math.Round(item.AverageCommunityRating));
		}
	}
}
