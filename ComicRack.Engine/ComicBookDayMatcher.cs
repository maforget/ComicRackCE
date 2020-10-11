using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Day")]
	[ComicBookMatcherHint("Day")]
	public class ComicBookDayMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.Day;
		}
	}
}
