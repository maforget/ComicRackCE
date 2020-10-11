using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Age")]
	[ComicBookMatcherHint("BookAge")]
	public class ComicBookBookAgeMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.BookAge;
		}
	}
}
