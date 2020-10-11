using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Genre")]
	[ComicBookMatcherHint("Genre")]
	public class ComicBookGenreMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Genre;
		}
	}
}
