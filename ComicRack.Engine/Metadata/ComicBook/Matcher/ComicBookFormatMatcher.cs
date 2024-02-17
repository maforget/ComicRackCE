using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Format")]
	[ComicBookMatcherHint("Format", "FilePath", "EnableProposed")]
	public class ComicBookFormatMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.ShadowFormat;
		}
	}
}
