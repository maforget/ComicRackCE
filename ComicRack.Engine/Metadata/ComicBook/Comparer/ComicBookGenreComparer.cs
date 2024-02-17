using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGenreComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Genre, y.Genre, ignoreCase: true);
		}
	}
}
