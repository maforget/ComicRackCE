using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Age Rating")]
	[ComicBookMatcherHint("AgeRating")]
	public class ComicBookAgeRatingMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.AgeRating;
		}
	}
}
