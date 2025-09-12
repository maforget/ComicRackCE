using System.Collections.Generic;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookAlternateSeriesComparer : Comparer<ComicBook>
	{
		private static readonly ComicBookAlternateNumberComparer numComp = new ComicBookAlternateNumberComparer();

		public override int Compare(ComicBook x, ComicBook y)
		{
			int num = ExtendedStringComparer.Compare(x.AlternateSeries, y.AlternateSeries, ExtendedStringComparison.IgnoreArticles | ExtendedStringComparison.IgnoreCase);
			if (num != 0)
			{
				return num;
			}
			return numComp.Compare(x, y);
		}
	}
}
