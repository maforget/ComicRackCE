using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Penciller")]
	[ComicBookMatcherHint("Penciller")]
	public class ComicBookPencillerMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Penciller;
		}
	}
}
