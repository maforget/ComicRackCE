namespace cYo.Projects.ComicRack.Engine
{
	public class BookPageEventArgs : BookEventArgs
	{
		private readonly int oldPage;

		private readonly int page;

		private readonly ComicPageInfo pageInfo;

		private readonly string pageKey = string.Empty;

		public int OldPage => oldPage;

		public int Page => page;

		public ComicPageInfo PageInfo => pageInfo;

		public string PageKey => pageKey;

		public BookPageEventArgs(ComicBook book, int oldPage, int page, ComicPageInfo pageInfo, string pageKey)
			: base(book)
		{
			this.oldPage = oldPage;
			this.page = page;
			this.pageInfo = pageInfo;
			this.pageKey = pageKey;
		}
	}
}
