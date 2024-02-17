using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookTeamsComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Teams, y.Teams, ignoreCase: true);
		}
	}
}
