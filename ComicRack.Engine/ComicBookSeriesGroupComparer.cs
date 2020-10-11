using System.Collections.Generic;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSeriesGroupComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return ExtendedStringComparer.Compare(x.SeriesGroup, y.SeriesGroup, ExtendedStringComparison.IgnoreArticles | ExtendedStringComparison.IgnoreCase);
		}
	}
}
