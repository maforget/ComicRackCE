using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookCommunityRatingComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.CommunityRating.CompareTo(y.CommunityRating);
		}
	}
}
