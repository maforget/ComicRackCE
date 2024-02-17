using System.Collections.Generic;
using cYo.Common.ComponentModel;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSeriesComparer : Comparer<ComicBook>
	{
		private static readonly IComparer<ComicBook>[] list = new IComparer<ComicBook>[3]
		{
			new ComicBookFormatComparer(),
			new ComicBookVolumeComparer(),
			new ComicBookNumberComparer()
		};

		public override int Compare(ComicBook x, ComicBook y)
		{
			int num = ExtendedStringComparer.Compare(x.ShadowSeries, y.ShadowSeries, ExtendedStringComparison.IgnoreArticles | ExtendedStringComparison.IgnoreCase);
			if (num != 0)
			{
				return num;
			}
			return list.Compare(x, y);
		}
	}
}
