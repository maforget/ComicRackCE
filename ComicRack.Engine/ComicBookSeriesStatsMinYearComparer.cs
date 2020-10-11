using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSeriesStatsMinYearComparer : Comparer<ComicBookSeriesStatistics>
	{
		public override int Compare(ComicBookSeriesStatistics x, ComicBookSeriesStatistics y)
		{
			return x.FirstYear.CompareTo(y.FirstYear);
		}
	}
}
