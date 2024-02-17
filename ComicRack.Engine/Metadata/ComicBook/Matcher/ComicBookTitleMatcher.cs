using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Title")]
	[ComicBookMatcherHint("Title", "FilePath", "EnableProposed")]
	public class ComicBookTitleMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.ShadowTitle;
		}
	}
}
