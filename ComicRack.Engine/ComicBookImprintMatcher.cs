using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Imprint")]
	[ComicBookMatcherHint("Imprint")]
	public class ComicBookImprintMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Imprint;
		}
	}
}
