using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSeriesStatsMaxYearComparer : Comparer<ComicBookSeriesStatistics>
	{
		public override int Compare(ComicBookSeriesStatistics x, ComicBookSeriesStatistics y)
		{
			return x.LastYear.CompareTo(y.LastYear);
		}
	}
}
