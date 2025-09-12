using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookBookPriceComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.BookPrice.CompareTo(y.BookPrice);
		}
	}
}
