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
                    SetArchive(new SevenZipEngine(KnownFileFormats.CBR, libraryMode: false, standalone: false)); // The standalone mode of SevenZip does not support RAR, so we set it to false to use the 32bit Console version instead.
                    break;
                case EngineConfiguration.CbEngines.SharpCompress:
                    SetArchive(new SharpCompressEngine(KnownFileFormats.CBR));
                    break;
            }
        }
    }
}
