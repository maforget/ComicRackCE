using System;

namespace cYo.Projects.ComicRack.Engine
{
	public class BookEventArgs : EventArgs
	{
		private readonly ComicBook book;

		public ComicBook Book => book;

		public BookEventArgs(ComicBook book)
		{
			this.book = book;
		}
	}
}
