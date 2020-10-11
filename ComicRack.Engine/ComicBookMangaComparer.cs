using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookMangaComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.Manga.CompareTo(y.Manga);
		}
	}
}
