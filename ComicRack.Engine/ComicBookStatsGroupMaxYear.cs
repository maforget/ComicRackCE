using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookStatsGroupMaxYear : SingleGrouper<ComicBookSeriesStatistics>
	{
		public override IGroupInfo GetGroup(ComicBookSeriesStatistics item)
		{
			string maxYearAsText = item.MaxYearAsText;
			return new GroupInfo(string.IsNullOrEmpty(maxYearAsText) ? GroupInfo.Unspecified : maxYearAsText);
		}
	}
}
