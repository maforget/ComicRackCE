using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookPageCountComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.PageCount.CompareTo(y.PageCount);
		}
	}
}
