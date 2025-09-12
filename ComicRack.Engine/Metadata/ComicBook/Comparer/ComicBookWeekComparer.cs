using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookWeekComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.Day.CompareTo(y.Week);
		}
	}
}
