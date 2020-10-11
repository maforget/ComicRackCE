using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (RAR)", 3, ".cbr")]
	[FileFormat("RAR Archive", 3, ".rar")]
	public class CbrComicProvider : ArchiveComicProvider
	{
		public CbrComicProvider()
		{
			switch (EngineConfiguration.Default.CbrUses)
			{
			default:
				SetArchive(new SevenZipEngine(3, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(3, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(3));
				break;
			}
		}
	}
}
