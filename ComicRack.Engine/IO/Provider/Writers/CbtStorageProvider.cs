using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Tar;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("eComic (TAR)", KnownFileFormats.CBT, ".cbt")]
	public class CbtStorageProvider : PackedStorageProvider
	{
		private const int BufferSize = 131072;

		private Stream file;

		private TarOutputStream tos;

		protected override void OnCreateFile(string target, StorageSetting setting)
		{
			file = File.Create(target, BufferSize);
            tos = new TarOutputStream(file, Encoding.UTF8);
		}

		protected override void OnCloseFile()
		{
			tos.Close();
			file.Close();
		}

		protected override void AddEntry(string name, byte[] data)
		{
			TarHeader tarHeader = new TarHeader
            {
                Name = name,
                UserName = "ComicRack",
                UserId = 666,
                Size = data.Length
            };
            tos.PutNextEntry(new TarEntry(tarHeader));
			tos.Write(data, 0, data.Length);
			tos.CloseEntry();
		}
	}
}
