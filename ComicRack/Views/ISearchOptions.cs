namespace cYo.Projects.ComicRack.Viewer.Views
{
	public interface ISearchOptions
	{
		bool SearchBrowserVisible
		{
			get;
			set;
		}

		void FocusQuickSearch();
	}
}
