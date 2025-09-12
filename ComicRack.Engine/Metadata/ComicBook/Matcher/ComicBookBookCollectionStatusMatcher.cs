using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Collection Status")]
	[ComicBookMatcherHint("BookCollectionStatus")]
	public class ComicBookBookCollectionStatusMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.BookCollectionStatus;
		}
	}
}
