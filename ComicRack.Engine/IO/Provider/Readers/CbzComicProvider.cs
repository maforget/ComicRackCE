using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("eComic (ZIP)", 2, ".cbz", EnableUpdate = true)]
	[FileFormat("ZIP Archive", 2, ".zip")]
	public class CbzComicProvider : ArchiveComicProvider
	{
		public CbzComicProvider()
		{
			switch (EngineConfiguration.Default.CbzUses)
			{
			default:
				SetArchive(new SevenZipEngine(2, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(2, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(2));
				break;
			case EngineConfiguration.CbEngines.SharpZip:
				SetArchive(new ZipSharpZipEngine());
				break;
			}
		}
	}
}
