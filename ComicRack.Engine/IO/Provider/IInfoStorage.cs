using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public interface IInfoStorage
	{
        event EventHandler<ErrorEventArgs> Error;

        bool StoreInfo(ComicInfo comicInfo);

		ComicInfo LoadInfo(InfoLoadingMethod method);
	}
}
