using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine.Database
{
	public interface ICachedComicBookList
	{
		void CommitCache(bool block);

		ISet<ComicBook> GetCache();
	}
}
