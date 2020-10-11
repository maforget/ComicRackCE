using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
	public class TarSharpZipEngine : FileBasedAccessor
	{
		public const int BufferSize = 131072;

		public TarSharpZipEngine()
			: base(5)
		{
		}

		public override bool IsFormat(string source)
		{
			try
			{
				using (FileStream inputStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 131072))
				{
					using (TarInputStream tarInputStream = new TarInputStream(inputStream))
					{
						return tarInputStream.GetNextEntry() != null;
					}
				}
			}
			catch
			{
				return false;
			}
		}

		public override IEnumerable<ProviderImageInfo> GetEntryList(string source)
		{
			using (FileStream fs = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 131072))
			{
				using (TarInputStream tis = new TarInputStream(fs))
				{
					while (true)
					{
						TarEntry nextEntry = tis.GetNextEntry();
						if (nextEntry == null)
						{
							break;
						}
						if (!nextEntry.IsDirectory)
						{
							yield return new ProviderImageInfo(0, nextEntry.Name, nextEntry.Size);
						}
					}
				}
			}
		}

		public override byte[] ReadByteImage(string source, ProviderImageInfo info)
		{
			try
			{
				using (FileStream inputStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 131072))
				{
					using (TarInputStream tarInputStream = new TarInputStream(inputStream))
					{
						TarEntry nextEntry;
						do
						{
							nextEntry = tarInputStream.GetNextEntry();
						}
						while (!(nextEntry.Name == info.Name));
						byte[] array = new byte[info.Size];
						if (tarInputStream.Read(array, 0, array.Length) != array.Length)
						{
							throw new IOException();
						}
						return array;
					}
				}
			}
			catch
			{
				return null;
			}
		}

		public override ComicInfo ReadInfo(string source)
		{
			try
			{
				using (FileStream inputStream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read, 131072))
				{
					using (TarInputStream tarInputStream = new TarInputStream(inputStream))
					{
						TarEntry nextEntry;
						do
						{
							nextEntry = tarInputStream.GetNextEntry();
						}
						while (string.Compare(Path.GetFileName(nextEntry.Name), "ComicInfo.xml", ignoreCase: true) != 0);
						byte[] array = new byte[nextEntry.Size];
						tarInputStream.Read(array, 0, array.Length);
						using (MemoryStream inStream = new MemoryStream(array))
						{
							return ComicInfo.Deserialize(inStream);
						}
					}
				}
			}
			catch
			{
				return null;
			}
		}
	}
}
