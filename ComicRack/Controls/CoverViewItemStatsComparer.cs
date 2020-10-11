using System.Collections.Generic;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Controls
{
	public class CoverViewItemStatsComparer<T> : CoverViewItemComparer where T : IComparer<ComicBookSeriesStatistics>, new()
	{
		private readonly T comparer = new T();

		protected override int OnCompare(CoverViewItem x, CoverViewItem y)
		{
			T val = comparer;
			return val.Compare(x.SeriesStats, y.SeriesStats);
		}
	}
}
