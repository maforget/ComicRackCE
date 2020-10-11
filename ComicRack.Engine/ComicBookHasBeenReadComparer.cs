using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookHasBeenReadComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.HasBeenRead.CompareTo(y.HasBeenRead);
		}
	}
}
