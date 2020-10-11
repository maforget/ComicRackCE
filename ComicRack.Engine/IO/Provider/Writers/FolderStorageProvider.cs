using System.IO;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Writers
{
	[FileFormat("Image Folder", 100, ".")]
	public class FolderStorageProvider : PackedStorageProvider
	{
		private string target;

		protected override void OnCreateFile(string target, StorageSetting setting)
		{
			this.target = target;
			Directory.CreateDirectory(target);
		}

		protected override void AddEntry(string name, byte[] data)
		{
			File.WriteAllBytes(Path.Combine(target, name), data);
		}

		protected override void OnCloseFile()
		{
		}
	}
}
