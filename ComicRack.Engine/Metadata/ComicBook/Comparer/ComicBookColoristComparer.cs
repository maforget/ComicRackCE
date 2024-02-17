using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookColoristComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Colorist, y.Colorist, ignoreCase: true);
		}
	}
}
