namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public static class Providers
	{
		private static ImageProviderFactory readersFactory;

		private static ProviderFactory<StorageProvider> writersFactory;

		public static ImageProviderFactory Readers
		{
			get
			{
				if (readersFactory == null)
				{
					readersFactory = new ImageProviderFactory();
					readersFactory.RegisterProviders();
				}
				return readersFactory;
			}
		}

		public static ProviderFactory<StorageProvider> Writers
		{
			get
			{
				if (writersFactory == null)
				{
					writersFactory = new ProviderFactory<StorageProvider>();
					writersFactory.RegisterProviders();
				}
				return writersFactory;
			}
		}
	}
}
