using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Tags")]
	[ComicBookMatcherHint("Tags")]
	public class ComicBookTagsMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Tags;
		}
	}
}
