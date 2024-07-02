using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Text;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
	public abstract class ArchiveComicProvider : ComicProvider
	{
		private List<ProviderImageInfo> foundImageList = new List<ProviderImageInfo>();

		private IComicAccessor imageArchive;

		private static Cache<FileKey, List<ProviderImageInfo>> imageInfoCache;

		public override ImageProviderCapabilities Capabilities => ImageProviderCapabilities.FastPageInfo | ImageProviderCapabilities.FastFormatCheck;

		public override string CreateHash()
		{
			return ImageProvider.CreateHashFromImageList(foundImageList);
		}

		protected IEnumerable<ProviderImageInfo> GetFileList()
		{
			return imageArchive.GetEntryList(base.Source);
		}

		protected override byte[] OnRetrieveSourceByteImage(int index)
		{
			return imageArchive.ReadByteImage(base.Source, GetFile(index));
		}

		protected override ComicInfo OnLoadInfo()
		{
			return imageArchive.ReadInfo(base.Source);
		}

		protected override bool OnStoreInfo(ComicInfo comicInfo)
		{
			return imageArchive.WriteInfo(base.Source, comicInfo);
		}

		protected override bool OnFastFormatCheck(string source)
		{
			return imageArchive.IsFormat(source);
		}

		protected override void OnParse()
		{
			using (IItemLock<List<ProviderImageInfo>> itemLock = GetCachedFileList())
			{
				List<ProviderImageInfo> list = new List<ProviderImageInfo>(itemLock.Item.Where((ProviderImageInfo ii) => IsSupportedImage(ii)));
                list.Sort((a, b) => cYo.Common.Text.ExtendedStringComparer.Compare(a.Name, b.Name, ExtendedStringComparison.IgnoreCase));
                foundImageList = list;
			}
			foreach (ProviderImageInfo foundImage in foundImageList)
			{
				if (!FireIndexReady(foundImage))
				{
					break;
				}
			}
		}

		protected void SetArchive(IComicAccessor imageArchive)
		{
			this.imageArchive = imageArchive;
		}

		private IItemLock<List<ProviderImageInfo>> GetCachedFileList()
		{
			using (ItemMonitor.Lock(typeof(ArchiveComicProvider)))
			{
				if (imageInfoCache == null)
				{
					imageInfoCache = new Cache<FileKey, List<ProviderImageInfo>>(100);
				}
			}
			return imageInfoCache.LockItem(new FileKey(base.Source), (FileKey fi) => GetFileList().ToList());
		}

		public ProviderImageInfo GetFile(int index)
		{
			return foundImageList[index];
		}
	}
}
