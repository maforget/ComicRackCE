using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Month")]
	[ComicBookMatcherHint("Month")]
	public class ComicBookMonthMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.Month;
		}
	}
}
