using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Cover Artist")]
	[ComicBookMatcherHint("CoverArtist")]
	public class ComicBookCoverArtistMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.CoverArtist;
		}
	}
}
