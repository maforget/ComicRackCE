using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("eComic (ZIP)", 2, ".cbz")]
	public class CbzStorageProvider : PackedStorageProvider
	{
		private const bool Zip64 = false;

		private Stream file;

		private ZipOutputStream zos;

		protected override void OnCreateFile(string target, StorageSetting setting)
		{
			file = File.Create(target, 100000);
			zos = new ZipOutputStream(file);
			zos.UseZip64 = UseZip64.Off;
			switch (setting.ComicCompression)
			{
			default:
				zos.SetLevel(0);
				break;
			case ExportCompression.Medium:
				zos.SetLevel(5);
				break;
			case ExportCompression.Strong:
				zos.SetLevel(9);
				break;
			}
		}

		protected override void OnCloseFile()
		{
			zos.Close();
			file.Close();
		}

		protected override void AddEntry(string name, byte[] data)
		{
			zos.PutNextEntry(new ZipEntry(name));
			zos.Write(data, 0, data.Length);
			zos.CloseEntry();
		}
	}
}
