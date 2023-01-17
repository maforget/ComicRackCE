using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	[FileFormat("eComic (TAR)", KnownFileFormats.CBT, ".cbt", EnableUpdate = true)]
	[FileFormat("TAR Archive", KnownFileFormats.CBT, ".tar")]
	public class CbtComicProvider : ArchiveComicProvider
	{
		public CbtComicProvider()
		{
			switch (EngineConfiguration.Default.CbtUses)
			{
			default:
				SetArchive(new SevenZipEngine(KnownFileFormats.CBT, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(KnownFileFormats.CBT, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(KnownFileFormats.CBT));
				break;
			case EngineConfiguration.CbEngines.SharpZip:
				SetArchive(new TarSharpZipEngine());
				break;
			}
		}
	}
}
