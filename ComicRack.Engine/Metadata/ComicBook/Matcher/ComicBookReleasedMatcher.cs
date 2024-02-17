using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Released")]
	[ComicBookMatcherHint("ReleasedTime")]
	public class ComicBookReleasedMatcher : ComicBookDateMatcher
	{
		protected override DateTime GetValue(ComicBook comicBook)
		{
			return comicBook.ReleasedTime;
		}
	}
}
