using System;
using System.IO;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("Book Information", 4, ".xml")]
	internal class XmlInfoStorageProvider : StorageProvider
	{
		protected override ComicInfo OnStore(IImageProvider provider, ComicInfo info, string target, StorageSetting setting)
		{
			if (info == null)
			{
				return info;
			}
			try
			{
				using (StreamWriter streamWriter = File.CreateText(target))
				{
					info.Serialize(streamWriter.BaseStream);
				}
				return info;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
