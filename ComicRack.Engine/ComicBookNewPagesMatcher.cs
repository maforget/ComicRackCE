using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("New Pages")]
	[ComicBookMatcherHint("NewPages")]
	public class ComicBookNewPagesMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.NewPages;
		}
	}
}
