using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookInkerComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Inker, y.Inker, ignoreCase: true);
		}
	}
}
