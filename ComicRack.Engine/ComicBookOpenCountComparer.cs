using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookOpenCountComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.OpenedCount.CompareTo(y.OpenedCount);
		}
	}
}
