using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Editor")]
	[ComicBookMatcherHint("Editor")]
	public class ComicBookEditorMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Editor;
		}
	}
}
