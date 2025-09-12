using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Condition")]
	[ComicBookMatcherHint("BookCondition")]
	public class ComicBookBookConditionMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.BookCondition;
		}
	}
}
