using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Story Arc")]
	[ComicBookMatcherHint("StoryArc")]
	public class ComicBookStoryArcMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.StoryArc;
		}
	}
}
