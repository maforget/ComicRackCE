using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookSizeComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.FileSize.CompareTo(y.FileSize);
		}
	}
}
