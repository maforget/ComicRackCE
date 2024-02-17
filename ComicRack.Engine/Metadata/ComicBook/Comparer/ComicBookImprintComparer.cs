using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookImprintComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Imprint, y.Imprint, ignoreCase: true);
		}
	}
}
