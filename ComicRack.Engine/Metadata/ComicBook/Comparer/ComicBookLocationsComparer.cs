using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookLocationsComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Locations, y.Locations, ignoreCase: true);
		}
	}
}
