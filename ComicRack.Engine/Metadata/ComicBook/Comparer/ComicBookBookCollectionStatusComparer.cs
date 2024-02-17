using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookBookCollectionStatusComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.BookCollectionStatus, y.BookCollectionStatus, ignoreCase: true);
		}
	}
}
