using System;
using System.IO;
using cYo.Common;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("Book Information", KnownFileFormats.XML, ".xml")]
	internal class XmlInfoStorageProvider : StorageProvider
	{
		protected override ComicInfo OnStore(IImageProvider provider, ComicInfo info, string target, StorageSetting setting)
		{
			if (info == null)
				return info;

			try
			{
				ComicInfo comicInfo = setting.EmbedComicInfo && setting.EmbedComicBook && info is ComicBook cb ? cb.Clone<ComicBook>() : info.GetInfo(); // Force the type depending on the settings
                using (StreamWriter streamWriter = File.CreateText(target))
				{
                    comicInfo.Serialize(streamWriter.BaseStream);
                }
				return comicInfo;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
