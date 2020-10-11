using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSeriesStatsGapCountComparer : Comparer<ComicBookSeriesStatistics>
	{
		public override int Compare(ComicBookSeriesStatistics x, ComicBookSeriesStatistics y)
		{
			return x.GapCount.CompareTo(y.GapCount);
		}
	}
}
