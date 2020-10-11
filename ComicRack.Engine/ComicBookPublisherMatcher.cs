using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Publisher")]
	[ComicBookMatcherHint("Publisher")]
	public class ComicBookPublisherMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Publisher;
		}
	}
}
