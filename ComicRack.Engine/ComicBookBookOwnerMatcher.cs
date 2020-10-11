using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Owner")]
	[ComicBookMatcherHint("BookOwner")]
	public class ComicBookBookOwnerMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.BookOwner;
		}
	}
}
