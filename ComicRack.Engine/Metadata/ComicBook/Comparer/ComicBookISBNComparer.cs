using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookISBNComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.ISBN, y.ISBN, ignoreCase: true);
		}
	}
}
