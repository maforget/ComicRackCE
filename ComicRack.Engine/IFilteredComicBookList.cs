namespace cYo.Projects.ComicRack.Engine
{
	public interface IFilteredComicBookList
	{
		bool ShowFiltered
		{
			get;
			set;
		}

		bool IsFiltered(ComicBook ci);

		void SetFiltered(ComicBook ci, bool filtered);

		void ClearFiltered();
	}
}
