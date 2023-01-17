using System;
using System.IO;
using cYo.Common.Win32;
using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;
using cYo.Projects.ComicRack.Engine.IO.Provider.Writers;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (7z)", KnownFileFormats.CB7, ".cb7")]
	public class Cb7StorageProvider : PackedStorageProvider
	{
		private string file;

		protected override void OnCreateFile(string target, StorageSetting setting)
		{
			file = target;
			File.Delete(file);
		}

		protected override void OnCloseFile()
		{
		}

		protected override void AddEntry(string name, byte[] data)
		{
			string parameters = $"a -t7z \"-si{name}\" \"{file}\"";
			try
			{
				ExecuteProcess.Result result = ExecuteProcess.Execute(SevenZipEngine.PackExe, parameters, data, null, ExecuteProcess.Options.None);
				if (result.ExitCode != 0)
				{
					throw new IOException();
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
