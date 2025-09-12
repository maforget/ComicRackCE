using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Review")]
	[ComicBookMatcherHint("Review")]
	public class ComicBookReviewMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Review;
		}
	}
}
