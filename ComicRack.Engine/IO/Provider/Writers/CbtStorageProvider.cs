using System.IO;
using ICSharpCode.SharpZipLib.Tar;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("eComic (TAR)", 5, ".cbt")]
	public class CbtStorageProvider : PackedStorageProvider
	{
		private const int BufferSize = 131072;

		private Stream file;

		private TarOutputStream tos;

		protected override void OnCreateFile(string target, StorageSetting setting)
		{
			file = File.Create(target, 131072);
			tos = new TarOutputStream(file);
		}

		protected override void OnCloseFile()
		{
			tos.Close();
			file.Close();
		}

		protected override void AddEntry(string name, byte[] data)
		{
			TarHeader tarHeader = new TarHeader();
			tarHeader.Name = name;
			tarHeader.UserName = "ComicRack";
			tarHeader.UserId = 666;
			tarHeader.Size = data.Length;
			tos.PutNextEntry(new TarEntry(tarHeader));
			tos.Write(data, 0, data.Length);
			tos.CloseEntry();
		}
	}
}
