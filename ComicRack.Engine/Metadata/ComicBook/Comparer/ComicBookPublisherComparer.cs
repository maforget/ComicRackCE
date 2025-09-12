using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookPublisherComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Publisher, y.Publisher, ignoreCase: true);
		}
	}
}
