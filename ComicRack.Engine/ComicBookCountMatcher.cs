using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Count")]
	[ComicBookMatcherHint("Count", "FilePath", "EnableProposed")]
	public class ComicBookCountMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.ShadowCount;
		}
	}
}
