using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Letterer")]
	[ComicBookMatcherHint("Letterer")]
	public class ComicBookLettererMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Letterer;
		}
	}
}
