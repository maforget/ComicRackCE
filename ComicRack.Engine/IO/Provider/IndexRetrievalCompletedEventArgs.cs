using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public class IndexRetrievalCompletedEventArgs : EventArgs
	{
		private readonly ImageProviderStatus status;

		private readonly int imageCount;

		public ImageProviderStatus Status => status;

		public int ImageCount => imageCount;

		public IndexRetrievalCompletedEventArgs(ImageProviderStatus status, int imageCount)
		{
			this.imageCount = imageCount;
			this.status = status;
		}
	}
}
