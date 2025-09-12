using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("File Directory")]
	[ComicBookMatcherHint("FilePath")]
	public class ComicBookDirectoryMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.FileDirectory;
		}
	}
}
