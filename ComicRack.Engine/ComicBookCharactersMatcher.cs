using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Characters")]
	[ComicBookMatcherHint("Characters")]
	public class ComicBookCharactersMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Characters;
		}
	}
}
