using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("File")]
	[ComicBookMatcherHint("FilePath")]
	public class ComicBookFileMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.FileName;
		}
	}
}
