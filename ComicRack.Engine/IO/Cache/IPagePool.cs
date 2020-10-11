using System;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public interface IPagePool
	{
		bool IsWorking
		{
			get;
		}

		int MaximumMemoryItems
		{
			get;
		}

		event EventHandler<CacheItemEventArgs<ImageKey, PageImage>> PageCached;

		IItemLock<PageImage> GetPage(PageKey key, bool onlyMemory);

		IItemLock<PageImage> GetPage(PageKey key, IImageProvider provider, bool onErrorThrowException);

		void RefreshPage(PageKey key);

		void CachePage(PageKey key, bool fastMem, IImageProvider provider, bool bottom);

		void RemoveImages(string source, int index = -1);

		void RefreshLastImage(string p);
	}
}
