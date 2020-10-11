using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookNumberComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.CompareNumber.CompareTo(y.CompareNumber);
		}
	}
}
