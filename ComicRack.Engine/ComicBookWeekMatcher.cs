using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Week")]
	[ComicBookMatcherHint("Year", "Month", "Day", "FilePath", "EnableProposed")]
	public class ComicBookWeekMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.Week;
		}
	}
}
