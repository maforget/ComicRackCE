using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Book Price")]
	[ComicBookMatcherHint("BookPrice")]
	public class ComicBookBookPriceMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.BookPrice;
		}
	}
}
