using System.Collections.Generic;
using System.IO;
using cYo.Common.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Projects.ComicRack.Engine
{
	public class PackedLocalize : IVirtualFolder
	{
		private readonly IVirtualFolder loc;

		public PackedLocalize(IVirtualFolder loc)
		{
			this.loc = loc;
		}

		public Stream OpenRead(string path)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(path);
				string path2 = directoryName + ".zip";
				string fileName = Path.GetFileName(path);
				if (loc.FileExists(path2))
				{
					using (Stream stream = loc.OpenRead(path2))
					{
						using (ZipFile zipFile = new ZipFile(stream))
						{
							ZipEntry entry = zipFile.GetEntry(fileName);
							if (entry != null)
							{
								using (Stream stream2 = zipFile.GetInputStream(entry))
								{
									byte[] array = new byte[entry.Size];
									stream2.Read(array, 0, array.Length);
									return new MemoryStream(array);
								}
							}
						}
					}
				}
			}
			catch
			{
			}
			return loc.OpenRead(path);
		}

		public Stream Create(string path)
		{
			return loc.Create(path);
		}

		public bool FileExists(string path)
		{
			string directoryName = Path.GetDirectoryName(path);
			string path2 = directoryName + ".zip";
			string fileName = Path.GetFileName(path);
			if (loc.FileExists(path2))
			{
				using (Stream stream = loc.OpenRead(path2))
				{
					using (ZipFile zipFile = new ZipFile(stream))
					{
						return zipFile.GetEntry(fileName) != null;
					}
				}
			}
			return loc.FileExists(path);
		}

		public bool CreateFolder(string path)
		{
			return loc.CreateFolder(path);
		}

		public IEnumerable<string> GetFiles(string path)
		{
			try
			{
				string path2 = path + ".zip";
				if (loc.FileExists(path2))
				{
					List<string> list = new List<string>();
					using (Stream stream = loc.OpenRead(path2))
					{
						using (ZipFile zipFile = new ZipFile(stream))
						{
							foreach (ZipEntry item in zipFile)
							{
								list.Add(Path.Combine(path, item.Name));
							}
						}
					}
					return list;
				}
			}
			catch
			{
			}
			return loc.GetFiles(path);
		}
	}
}
