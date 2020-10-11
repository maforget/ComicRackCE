namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface IBrowseHistory
	{
		bool CanBrowsePrevious();

		bool CanBrowseNext();

		void BrowsePrevious();

		void BrowseNext();
	}
}
