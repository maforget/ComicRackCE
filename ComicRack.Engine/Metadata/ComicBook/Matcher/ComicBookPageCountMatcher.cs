using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Page Count")]
	[ComicBookMatcherHint("PageCount")]
	public class ComicBookPageCountMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.PageCount;
		}
	}
}
