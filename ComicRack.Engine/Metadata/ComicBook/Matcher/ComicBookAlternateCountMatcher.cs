using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Alternate Count")]
	[ComicBookMatcherHint("AlternateCount")]
	public class ComicBookAlternateCountMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.AlternateCount;
		}
	}
}
