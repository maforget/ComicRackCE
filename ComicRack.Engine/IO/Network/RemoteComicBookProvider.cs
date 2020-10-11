using System;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	public class RemoteComicBookProvider : ImageProvider
	{
		private readonly Guid comicId;

		private readonly ComicLibraryClient client;

		private readonly object retrieveLock = new object();

		public override ImageProviderCapabilities Capabilities => base.Capabilities;

		public override int FormatId => -1;

		public RemoteComicBookProvider(Guid comicId, ComicLibraryClient client)
		{
			this.comicId = comicId;
			this.client = client;
		}

		protected override void OnParse()
		{
			int imageCount = client.RemoteLibrary.GetImageCount(comicId);
			for (int i = 0; i < imageCount; i++)
			{
				FireIndexReady(new ProviderImageInfo());
			}
		}

		protected override byte[] OnRetrieveSourceByteImage(int index)
		{
			try
			{
				using (ItemMonitor.Lock(retrieveLock))
				{
					return client.RemoteLibrary.GetImage(comicId, index);
				}
			}
			catch
			{
				return null;
			}
		}

		protected override void OnCheckSource()
		{
			if (client == null || client.RemoteLibrary == null)
			{
				throw new InvalidOperationException("No valid remote library");
			}
		}

		public override string CreateHash()
		{
			return string.Empty;
		}

		protected override ThumbnailImage OnRetrieveThumbnailImage(int index)
		{
			try
			{
				return ThumbnailImage.CreateFrom(client.RemoteLibrary.GetThumbnailImage(comicId, index));
			}
			catch
			{
				return null;
			}
		}
	}
}
