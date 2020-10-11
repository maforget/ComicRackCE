using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
    [FileFormat("eComic (RAR5)", 9, ".cbr")]
    [FileFormat("RAR5 Archive", 9, ".rar")]
	public class Rar5ComicProvider : ArchiveComicProvider
	{
		public Rar5ComicProvider()
		{
			switch (EngineConfiguration.Default.CbrUses)
			{
				default:
					SetArchive(new SevenZipEngine(9, libraryMode: true));
					break;
				case EngineConfiguration.CbEngines.SevenZipExe:
					SetArchive(new SevenZipEngine(9, libraryMode: false));
					break;
				case EngineConfiguration.CbEngines.SharpCompress:
					SetArchive(new SharpCompressEngine(9));
					break;
			}
		}
	}
}

