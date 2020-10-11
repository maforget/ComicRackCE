using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookStatsGroupPageReadCount : SingleGrouper<ComicBookSeriesStatistics>
	{
		public override IGroupInfo GetGroup(ComicBookSeriesStatistics item)
		{
			return ItemGroupCount.GetNumberGroup(item.PageReadCount);
		}
	}
}
