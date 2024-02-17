using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Opened")]
	[ComicBookMatcherHint("OpenedTime")]
	public class ComicBookOpenedMatcher : ComicBookDateMatcher
	{
		protected override DateTime GetValue(ComicBook comicBook)
		{
			return comicBook.OpenedTime;
		}
	}
}
