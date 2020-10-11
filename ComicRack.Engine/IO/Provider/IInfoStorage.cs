namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public interface IInfoStorage
	{
		bool StoreInfo(ComicInfo comicInfo);

		ComicInfo LoadInfo(InfoLoadingMethod method);
	}
}
