using System;
using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookLanguageComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.LanguageISO, y.LanguageISO, StringComparison.OrdinalIgnoreCase);
		}
	}
}
