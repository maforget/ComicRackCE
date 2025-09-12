using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookBookAgeComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.BookAge.CompareTo(y.BookAge);
		}
	}
}
