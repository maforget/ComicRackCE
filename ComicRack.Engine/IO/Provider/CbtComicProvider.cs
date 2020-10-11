using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	[FileFormat("eComic (TAR)", 5, ".cbt", EnableUpdate = true)]
	[FileFormat("TAR Archive", 5, ".tar")]
	public class CbtComicProvider : ArchiveComicProvider
	{
		public CbtComicProvider()
		{
			switch (EngineConfiguration.Default.CbtUses)
			{
			default:
				SetArchive(new SevenZipEngine(5, libraryMode: true));
				break;
			case EngineConfiguration.CbEngines.SevenZipExe:
				SetArchive(new SevenZipEngine(5, libraryMode: false));
				break;
			case EngineConfiguration.CbEngines.SharpCompress:
				SetArchive(new SharpCompressEngine(5));
				break;
			case EngineConfiguration.CbEngines.SharpZip:
				SetArchive(new TarSharpZipEngine());
				break;
			}
		}
	}
}
