using cYo.Common.IO;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public class ThumbnailDiskCache<K> : DiskCache<K, ThumbnailImage>
	{
		public ThumbnailDiskCache(string cacheFolder, int cacheSizeMB)
			: base(cacheFolder, cacheSizeMB, 10)
		{
		}

		protected override ThumbnailImage LoadItem(string file)
		{
			return ThumbnailImage.CreateFrom(file);
		}

		protected override void StoreItem(string file, ThumbnailImage item)
		{
			item.Save(file);
		}
	}
}
