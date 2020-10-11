using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookAgeRatingComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.AgeRating, y.AgeRating, ignoreCase: true);
		}
	}
}
