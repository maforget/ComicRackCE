using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Series")]
	[ComicBookMatcherHint("Series", "FilePath", "EnableProposed")]
	public class ComicBookSeriesMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.ShadowSeries;
		}
	}
}
