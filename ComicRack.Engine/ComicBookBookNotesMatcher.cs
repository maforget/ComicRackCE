using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Notes")]
	[ComicBookMatcherHint("BookNotes")]
	public class ComicBookBookNotesMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.BookNotes;
		}
	}
}
