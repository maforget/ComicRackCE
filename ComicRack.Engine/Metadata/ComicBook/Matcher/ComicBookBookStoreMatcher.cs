using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Store")]
	[ComicBookMatcherHint("BookStore")]
	public class ComicBookBookStoreMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.BookStore;
		}
	}
}
