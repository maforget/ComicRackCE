using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
    [FileFormat("eComic (RAR5)", KnownFileFormats.RAR5, ".cbr")]
    [FileFormat("RAR5 Archive", KnownFileFormats.RAR5, ".rar")]
	public class Rar5ComicProvider : ArchiveComicProvider
	{
		public Rar5ComicProvider()
		{
			switch (EngineConfiguration.Default.CbrUses)
			{
				default:
					SetArchive(new SevenZipEngine(KnownFileFormats.RAR5, libraryMode: true));
					break;
				case EngineConfiguration.CbEngines.SevenZipExe:
					SetArchive(new SevenZipEngine(KnownFileFormats.RAR5, libraryMode: false, standalone: false)); // The standalone mode of SevenZip does not support RAR, so we set it to false to use the 32bit Console version instead.
                    break;
				case EngineConfiguration.CbEngines.SharpCompress:
					SetArchive(new SharpCompressEngine(KnownFileFormats.RAR5));
					break;
			}
		}
	}
}

