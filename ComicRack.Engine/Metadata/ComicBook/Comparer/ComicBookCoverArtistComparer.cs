using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookCoverArtistComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.CoverArtist, y.CoverArtist, ignoreCase: true);
		}
	}
}
