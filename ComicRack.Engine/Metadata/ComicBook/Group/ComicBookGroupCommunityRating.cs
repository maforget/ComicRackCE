using System;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupCommunityRating : ComicBookGroupRatingBase
	{
		protected override int GetRating(ComicBook item)
		{
			return (int)Math.Round(item.CommunityRating);
		}
	}
}
