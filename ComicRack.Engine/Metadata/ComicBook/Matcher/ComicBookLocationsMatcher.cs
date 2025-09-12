using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Locations")]
	[ComicBookMatcherHint("Locations")]
	public class ComicBookLocationsMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Locations;
		}
	}
}
