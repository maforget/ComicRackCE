using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookLinkedComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.IsLinked.CompareTo(y.IsLinked);
		}
	}
}
