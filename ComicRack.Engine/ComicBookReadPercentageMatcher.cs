using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Read Percentage")]
	[ComicBookMatcherHint("PageCount", "LastPageRead")]
	public class ComicBookReadPercentageMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.ReadPercentage;
		}
	}
}
