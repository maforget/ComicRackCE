using System;

namespace cYo.Projects.ComicRack.Engine
{
	public interface ICacheSettings
	{
		bool InternetCacheEnabled
		{
			get;
			set;
		}

		int InternetCacheSizeMB
		{
			get;
			set;
		}

		int MemoryPageCacheCount
		{
			get;
			set;
		}

		bool MemoryPageCacheOptimized
		{
			get;
			set;
		}

		bool MemoryThumbCacheOptimized
		{
			get;
			set;
		}

		int MemoryThumbCacheSizeMB
		{
			get;
			set;
		}

		bool PageCacheEnabled
		{
			get;
			set;
		}

		int PageCacheSizeMB
		{
			get;
			set;
		}

		bool ThumbCacheEnabled
		{
			get;
			set;
		}

		int ThumbCacheSizeMB
		{
			get;
			set;
		}

		event EventHandler CacheSettingsChanged;
	}
}
