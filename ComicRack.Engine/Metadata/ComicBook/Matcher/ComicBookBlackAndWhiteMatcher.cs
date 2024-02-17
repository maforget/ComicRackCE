using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Black and White")]
	[ComicBookMatcherHint("BlackAndWhite")]
	public class ComicBookBlackAndWhiteMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			return comicBook.BlackAndWhite;
		}
	}
}
