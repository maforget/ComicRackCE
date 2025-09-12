using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Year")]
	[ComicBookMatcherHint("Year", "FilePath", "EnableProposed")]
	public class ComicBookYearMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.ShadowYear;
		}
	}
}
