using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("ISBN")]
	[ComicBookMatcherHint("ISBN")]
	public class ComicBookISBNMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.ISBN;
		}
	}
}
