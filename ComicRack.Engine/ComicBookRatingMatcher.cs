using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("My Rating")]
	[ComicBookMatcherHint("Rating")]
	public class ComicBookRatingMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.Rating;
		}

		protected override float GetInvalidValue()
		{
			return 0f;
		}
	}
}
