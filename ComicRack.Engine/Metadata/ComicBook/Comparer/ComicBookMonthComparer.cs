using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookMonthComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.Month.CompareTo(y.Month);
		}
	}
}
