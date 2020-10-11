using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (7z)", 6, ".cb7", EnableUpdate = true)]
	[FileFormat("7z Archive", 6, ".7z")]
	public class Cb7ComicProvider : ArchiveComicProvider
	{
		public Cb7ComicProvider()
		{
			switch (EngineConfiguration.Default.Cb7Uses)
			{
			default:
				SetArchive(new SevenZipEngine(6, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(6, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(6));
				break;
			}
		}
	}
}
