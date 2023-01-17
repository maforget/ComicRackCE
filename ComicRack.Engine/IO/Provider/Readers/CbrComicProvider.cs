using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (RAR)", KnownFileFormats.CBR, ".cbr")]
	[FileFormat("RAR Archive", KnownFileFormats.CBR, ".rar")]
	public class CbrComicProvider : ArchiveComicProvider
	{
		public CbrComicProvider()
		{
			switch (EngineConfiguration.Default.CbrUses)
			{
			default:
				SetArchive(new SevenZipEngine(KnownFileFormats.CBR, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(KnownFileFormats.CBR, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(KnownFileFormats.CBR));
				break;
			}
		}
	}
}
