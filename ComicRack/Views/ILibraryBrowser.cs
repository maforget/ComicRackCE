using System;

namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface ILibraryBrowser
	{
		bool SelectList(Guid listId);

		bool CanBrowsePrevious();

		bool CanBrowseNext();

		void BrowsePrevious();

		void BrowseNext();
	}
}
