using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookStatsGroupMinYear : SingleGrouper<ComicBookSeriesStatistics>
	{
		public override IGroupInfo GetGroup(ComicBookSeriesStatistics item)
		{
			string minYearAsText = item.MinYearAsText;
			return new GroupInfo(string.IsNullOrEmpty(minYearAsText) ? GroupInfo.Unspecified : minYearAsText);
		}
	}
}
