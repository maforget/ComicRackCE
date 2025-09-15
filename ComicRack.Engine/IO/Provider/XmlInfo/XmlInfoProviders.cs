namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	public static class XmlInfoProviders
	{
		private static XmlInfoProviderFactory readersFactory;

		public static XmlInfoProviderFactory Readers
		{
			get
			{
				if (readersFactory == null)
				{
					readersFactory = new XmlInfoProviderFactory();
					readersFactory.RegisterProviders();
				}
				return readersFactory;
			}
		}
	}
}
