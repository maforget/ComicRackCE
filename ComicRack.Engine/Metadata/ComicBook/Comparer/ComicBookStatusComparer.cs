using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookStatusComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.Status.CompareTo(y.Status);
		}
	}
}
