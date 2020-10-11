using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSeriesStatsPageReadCountComparer : Comparer<ComicBookSeriesStatistics>
	{
		public override int Compare(ComicBookSeriesStatistics x, ComicBookSeriesStatistics y)
		{
			return x.PageReadCount.CompareTo(y.PageReadCount);
		}
	}
}
