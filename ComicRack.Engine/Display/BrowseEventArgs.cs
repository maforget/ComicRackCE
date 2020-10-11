using System;

namespace cYo.Projects.ComicRack.Engine.Display
{
	public class BrowseEventArgs : EventArgs
	{
		private readonly PageSeekOrigin seekOrigin;

		private readonly int offset;

		public PageSeekOrigin SeekOrigin => seekOrigin;

		public int Offset => offset;

		public BrowseEventArgs(PageSeekOrigin origin, int offset)
		{
			seekOrigin = origin;
			this.offset = offset;
		}
	}
}
