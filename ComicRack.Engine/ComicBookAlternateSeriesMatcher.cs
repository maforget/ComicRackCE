using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Alternate Series")]
	[ComicBookMatcherHint("AlternateSeries")]
	public class ComicBookAlternateSeriesMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.AlternateSeries;
		}
	}
}
