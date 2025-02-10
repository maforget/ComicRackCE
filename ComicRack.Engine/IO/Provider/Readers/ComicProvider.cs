using System;
using System.IO;
using System.Linq;
using cYo.Common.IO;
using cYo.Common.Reflection;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	public abstract class ComicProvider : ImageProvider, IInfoStorage
	{
		private static readonly string[] supportedTypes = new string[]
		{
			"jpg",
			"jpeg",
			"jif",
			"jiff",
			"gif",
			"png",
			"tif",
			"tiff",
			"bmp",
			"djvu",
			"webp",
			"heic",
			"heif",
			"avif",
			//"jxl"
		};

		public bool UpdateEnabled => GetType().GetAttributes<FileFormatAttribute>().FirstOrDefault((FileFormatAttribute f) => f.Format.Supports(base.Source))?.EnableUpdate ?? false;

		private bool disableNtfs = false;
		protected bool DisableNtfs
		{
			get
			{
				if (disableNtfs)
					return true;

				return EngineConfiguration.Default.DisableNTFS;
			}

			set => disableNtfs = value;
		}

		protected bool DisableSidecar
		{
			get;
			set;
		}

		public ComicInfo LoadInfo(InfoLoadingMethod method)
		{
			using (LockSource(readOnly: true))
			{
				ComicInfo comicInfo = (DisableNtfs ? null : NtfsInfoStorage.LoadInfo(base.Source));
				if (comicInfo == null && !DisableSidecar)
				{
					comicInfo = ComicInfo.LoadFromSidecar(base.Source);
				}
				if (comicInfo != null && method == InfoLoadingMethod.Fast)
				{
					return comicInfo;
				}
				ComicInfo comicInfo2 = OnLoadInfo();
				return comicInfo2 ?? comicInfo;
			}
		}

		public bool StoreInfo(ComicInfo comicInfo)
		{
			bool flag = false;
			using (LockSource(readOnly: false))
			{
				if (UpdateEnabled)
				{
					if (!OnStoreInfo(comicInfo))
					{
						return false;
					}
					flag = true;
				}
				if (!DisableNtfs)
				{
					flag |= NtfsInfoStorage.StoreInfo(base.Source, comicInfo);
				}
				return flag;
			}
		}

		protected virtual ComicInfo OnLoadInfo()
		{
			return null;
		}

		protected virtual bool OnStoreInfo(ComicInfo comicInfo)
		{
			return false;
		}

		protected virtual bool IsSupportedImage(ProviderImageInfo file)
        {
            if(IsImageThumbnailFolder(file.Name) || IsFileTooSmall(file.Size))
				return false;

            string fileExt = Path.GetExtension(FileUtility.MakeValidFilename(file.Name));
            return supportedTypes.Any((string ext) => string.Equals(fileExt, "." + ext, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsImageThumbnailFolder(string file)
        {
            string[] ignore = { ".DS_Store\\", "__MACOSX\\" };
            return ignore.Any(item => file.Contains(item));
        }

        private static bool IsFileTooSmall(long size)
        {
			long minSize = 256;
            return size < minSize;
        }
    }
}
