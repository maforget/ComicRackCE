using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookAlternateCountComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.AlternateCount.CompareTo(y.AlternateCount);
		}
	}
}
