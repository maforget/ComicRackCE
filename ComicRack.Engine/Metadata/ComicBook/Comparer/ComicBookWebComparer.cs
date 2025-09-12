using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookWebComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Web, y.Web, ignoreCase: true);
		}
	}
}
