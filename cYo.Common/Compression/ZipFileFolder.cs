using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Common.Compression
{
	public class ZipFileFolder : DisposableObject, IVirtualFolder
	{
		private ZipFile zipFile;

		public ZipFileFolder(Stream zipStream)
		{
			zipFile = new ZipFile(zipStream);
		}

		public ZipFileFolder(string zipFile)
		{
            //437 is the default Zip char Encoding. 850 is the default western windows
            this.zipFile = new ZipFile(zipFile, StringCodec.FromCodePage(437));
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				zipFile.Close();
			}
			base.Dispose(disposing);
		}

		public Stream OpenRead(string path)
		{
			ZipEntry entry = zipFile.GetEntry(path);
			if (entry == null)
			{
				return null;
			}
			using (Stream stream = zipFile.GetInputStream(entry))
			{
				byte[] array = new byte[entry.Size];
				stream.Read(array, 0, array.Length);
				return new MemoryStream(array);
			}
		}

		public bool FileExists(string path)
		{
			return zipFile.GetEntry(path) != null;
		}

		public IEnumerable<string> GetFiles(string path)
		{
			return from ze in zipFile.OfType<ZipEntry>()
				where ze.Name.StartsWith(path)
				select ze.Name;
		}

		public Stream Create(string path)
		{
			throw new NotImplementedException();
		}

		public bool CreateFolder(string path)
		{
			throw new NotImplementedException();
		}

		public static ZipFileFolder CreateFromFile(string file)
		{
			try
			{
				return new ZipFileFolder(file);
			}
			catch
			{
				return null;
			}
		}

		public static IEnumerable<ZipFileFolder> CreateFromFiles(IEnumerable<string> folders, string searchPattern)
		{
			return folders.SelectMany((string folder) => (from f in FileUtility.SafeGetFiles(folder, searchPattern)
				orderby f
				select f).Select(CreateFromFile)).ToArray();
		}
	}
}
