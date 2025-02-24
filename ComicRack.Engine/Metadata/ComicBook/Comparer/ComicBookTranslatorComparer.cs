using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookTranslatorComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Translator, y.Translator, ignoreCase: true);
		}
	}
}
