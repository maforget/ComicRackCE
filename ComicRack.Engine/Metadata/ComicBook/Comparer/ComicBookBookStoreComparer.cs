using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookBookStoreComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.BookStore, y.BookStore, ignoreCase: true);
		}
	}
}
