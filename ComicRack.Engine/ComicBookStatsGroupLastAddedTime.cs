using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookStatsGroupLastAddedTime : SingleGrouper<ComicBookSeriesStatistics>
	{
		public override IGroupInfo GetGroup(ComicBookSeriesStatistics item)
		{
			return GroupInfo.GetDateGroup(item.LastAddedTime);
		}
	}
}
