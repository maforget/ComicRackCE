using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
	public class ZipSharpZipEngine : FileBasedAccessor
	{
		public const int BufferSize = 131072;

		public ZipSharpZipEngine()
			: base(2)
		{
		}

		public override IEnumerable<ProviderImageInfo> GetEntryList(string source)
		{
			using (FileStream fs = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
			{
				using (ZipFile zf = new ZipFile(fs))
				{
					foreach (ZipEntry item in zf)
					{
						yield return new ProviderImageInfo((int)item.ZipFileIndex, item.Name, item.Size);
					}
				}
			}
		}

		public override byte[] ReadByteImage(string source, ProviderImageInfo info)
		{
			try
			{
				using (FileStream file = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
				{
					using (ZipFile zipFile = new ZipFile(file))
					{
						ZipEntry entry = zipFile.GetEntry(info.Name);
						using (Stream stream = zipFile.GetInputStream(entry))
						{
							byte[] array = new byte[(int)entry.Size];
							if (stream.Read(array, 0, array.Length) != array.Length)
							{
								throw new IOException();
							}
							return array;
						}
					}
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public override ComicInfo ReadInfo(string source)
		{
			try
			{
				using (FileStream file = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize))
				{
					using (ZipFile zipFile = new ZipFile(file))
					{
						int num = zipFile.FindEntry("ComicInfo.xml", ignoreCase: true);
						if (num != -1)
						{
							using (Stream inStream = zipFile.GetInputStream(num))
							{
								return ComicInfo.Deserialize(inStream);
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
