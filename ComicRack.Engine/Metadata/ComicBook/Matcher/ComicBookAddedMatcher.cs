using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Added")]
	[ComicBookMatcherHint("AddedTime")]
	public class ComicBookAddedMatcher : ComicBookDateMatcher
	{
		protected override DateTime GetValue(ComicBook comicBook)
		{
			return comicBook.AddedTime;
		}
	}
}
