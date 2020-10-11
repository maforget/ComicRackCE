using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookStatsGroupRunningTimeYears : SingleGrouper<ComicBookSeriesStatistics>
	{
		public override IGroupInfo GetGroup(ComicBookSeriesStatistics item)
		{
			return ItemGroupCount.GetNumberGroup(item.RunningTimeYears);
		}
	}
}
