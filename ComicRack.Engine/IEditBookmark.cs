namespace cYo.Projects.ComicRack.Engine
{
	public interface IEditBookmark
	{
		bool CanBookmark
		{
			get;
		}

		string BookmarkProposal
		{
			get;
		}

		string Bookmark
		{
			get;
			set;
		}
	}
}
