using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Inker")]
	[ComicBookMatcherHint("Inker")]
	public class ComicBookInkerMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Inker;
		}
	}
}
