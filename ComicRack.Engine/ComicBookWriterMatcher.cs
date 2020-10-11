using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Writer")]
	[ComicBookMatcherHint("Writer")]
	public class ComicBookWriterMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Writer;
		}
	}
}
