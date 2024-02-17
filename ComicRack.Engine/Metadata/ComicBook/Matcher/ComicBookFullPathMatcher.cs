using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("File Path")]
	[ComicBookMatcherHint("FilePath")]
	public class ComicBookFullPathMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.FilePath;
		}
	}
}
