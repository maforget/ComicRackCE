using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Colorist")]
	[ComicBookMatcherHint("Colorist")]
	public class ComicBookColoristMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Colorist;
		}
	}
}
