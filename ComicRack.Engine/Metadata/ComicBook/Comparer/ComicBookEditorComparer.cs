using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookEditorComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return string.Compare(x.Editor, y.Editor, ignoreCase: true);
		}
	}
}
