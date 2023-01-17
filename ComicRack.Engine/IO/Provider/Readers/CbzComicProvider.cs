using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (ZIP)", KnownFileFormats.CBZ, ".cbz", EnableUpdate = true)]
	[FileFormat("ZIP Archive", KnownFileFormats.CBZ, ".zip")]
	public class CbzComicProvider : ArchiveComicProvider
	{
		public CbzComicProvider()
		{
			switch (EngineConfiguration.Default.CbzUses)
			{
			default:
				SetArchive(new SevenZipEngine(KnownFileFormats.CBZ, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(KnownFileFormats.CBZ, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(KnownFileFormats.CBZ));
				break;
			case EngineConfiguration.CbEngines.SharpZip:
				SetArchive(new ZipSharpZipEngine());
				break;
			}
		}
	}
}
