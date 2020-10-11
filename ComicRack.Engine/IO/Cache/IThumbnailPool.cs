using System;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public interface IThumbnailPool
	{
		event EventHandler<CacheItemEventArgs<ImageKey, ThumbnailImage>> ThumbnailCached;

		void CacheThumbnail(ThumbnailKey key, bool checkMemoryOnly, IImageProvider provider);

		IItemLock<ThumbnailImage> GetThumbnail(ThumbnailKey key, bool onlyMemory);

		IItemLock<ThumbnailImage> GetThumbnail(ThumbnailKey key, IImageProvider provider, bool onErrorThrowException);
	}
}
