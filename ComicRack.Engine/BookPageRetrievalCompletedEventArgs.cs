using System;
using System.Drawing;

namespace cYo.Projects.ComicRack.Engine
{
	public class BookPageRetrievalCompletedEventArgs : EventArgs
	{
		private readonly int page;

		private readonly bool twoPage;

		public Bitmap Bitmap
		{
			get;
			set;
		}

		public int Page => page;

		public bool TwoPage => twoPage;

		public BookPageRetrievalCompletedEventArgs(Bitmap bitmap, int page, bool twoPage)
		{
			Bitmap = bitmap;
			this.page = page;
			this.twoPage = twoPage;
		}
	}
}
