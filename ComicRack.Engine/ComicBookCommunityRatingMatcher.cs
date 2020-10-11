using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Community Rating")]
	[ComicBookMatcherHint("CommunityRating")]
	public class ComicBookCommunityRatingMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.CommunityRating;
		}

		protected override float GetInvalidValue()
		{
			return 0f;
		}
	}
}
