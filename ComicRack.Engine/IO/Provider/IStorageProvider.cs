using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public interface IStorageProvider
	{
		string DefaultExtension
		{
			get;
		}

		FileFormat DefaultFileFormat
		{
			get;
		}

		int FormatId
		{
			get;
		}

		event EventHandler<StorageProgressEventArgs> Progress;

		ComicInfo Store(IImageProvider provider, ComicInfo info, string target, StorageSetting setting);
	}
}
