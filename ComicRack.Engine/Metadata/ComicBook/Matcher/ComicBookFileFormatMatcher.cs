using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("File Format")]
	[ComicBookMatcherHint("FileFormat", "FilePath")]
	public class ComicBookFileFormatMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.FileFormat;
		}
	}
}
