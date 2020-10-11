using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public class ImageIndexReadyEventArgs : CancelEventArgs
	{
		private readonly int imageNumber;

		private readonly ProviderImageInfo imageInfo;

		public int ImageNumber => imageNumber;

		public ProviderImageInfo ImageInfo => imageInfo;

		public ImageIndexReadyEventArgs(int imageNumber, ProviderImageInfo ii)
		{
			this.imageNumber = imageNumber;
			imageInfo = ii;
		}
	}
}
