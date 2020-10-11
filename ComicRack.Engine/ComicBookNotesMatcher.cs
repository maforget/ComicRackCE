using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Notes")]
	[ComicBookMatcherHint("Notes")]
	public class ComicBookNotesMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Notes;
		}
	}
}
