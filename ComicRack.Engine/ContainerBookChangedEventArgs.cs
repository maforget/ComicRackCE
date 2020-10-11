using System;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ContainerBookChangedEventArgs : BookChangedEventArgs
	{
		public ComicBook Book
		{
			get;
			set;
		}

		public ContainerBookChangedEventArgs(ComicBook book, BookChangedEventArgs e)
			: base(e)
		{
			Book = book;
		}
	}
}
