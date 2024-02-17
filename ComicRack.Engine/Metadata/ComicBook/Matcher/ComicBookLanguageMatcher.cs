using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Language")]
	[ComicBookMatcherHint("LanguageISO")]
	public class ComicBookLanguageMatcher : ComicBookStringMatcher
	{
		protected override string GetValue(ComicBook comicBook)
		{
			return comicBook.LanguageAsText;
		}

		protected override string GetMatchValue(ComicBook cb)
		{
			string text = base.GetMatchValue(cb);
			if (text.Length == 2)
			{
				string languageName = ComicBook.GetLanguageName(text);
				if (!string.IsNullOrEmpty(languageName))
				{
					text = languageName;
				}
			}
			return text;
		}
	}
}
