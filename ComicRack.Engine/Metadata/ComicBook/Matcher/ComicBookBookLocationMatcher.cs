using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Location")]
	[ComicBookMatcherHint("BookLocation")]
	public class ComicBookBookLocationMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.BookLocation;
		}
	}
}
