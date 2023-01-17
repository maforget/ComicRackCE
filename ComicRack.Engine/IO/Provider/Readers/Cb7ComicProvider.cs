using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (7z)", KnownFileFormats.CB7, ".cb7", EnableUpdate = true)]
	[FileFormat("7z Archive", KnownFileFormats.CB7, ".7z")]
	public class Cb7ComicProvider : ArchiveComicProvider
	{
		public Cb7ComicProvider()
		{
			switch (EngineConfiguration.Default.Cb7Uses)
			{
			default:
				SetArchive(new SevenZipEngine(KnownFileFormats.CB7, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(KnownFileFormats.CB7, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(KnownFileFormats.CB7));
				break;
			}
		}
	}
}
