using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Translator")]
	[ComicBookMatcherHint("Translator")]
	public class ComicBookTranslatorMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.Translator;
		}
	}
}
