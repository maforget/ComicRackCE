using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookCharactersComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Characters, y.Characters, ignoreCase: true);
		}
	}
}
