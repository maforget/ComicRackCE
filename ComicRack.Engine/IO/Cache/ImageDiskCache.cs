using cYo.Common.IO;

namespace cYo.Projects.ComicRack.Engine.IO.Cache
{
	public class ImageDiskCache<K> : DiskCache<K, PageImage>
	{
		public ImageDiskCache(string cacheFolder, int cacheSizeMB)
			: base(cacheFolder, cacheSizeMB, 10)
		{
		}

		protected override PageImage LoadItem(string file)
		{
			return PageImage.CreateFrom(file);
		}

		protected override void StoreItem(string file, PageImage item)
		{
			item.Save(file);
		}
	}
}
