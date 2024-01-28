using System.Linq;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public class ImageProviderFactory : ProviderFactory<ImageProvider>
	{
		public override ImageProvider CreateSourceProvider(string source)
		{
			try
			{
				ImageProvider imageProvider = base.CreateSourceProvider(source);
				if (imageProvider == null)
				{
					return null;
				}
				imageProvider.Source = source;
				if ((imageProvider.Capabilities & ImageProviderCapabilities.FastFormatCheck) == 0 || imageProvider.FastFormatCheck(source))
				{
					return imageProvider;
				}
				ImageProvider imageProvider2 = CreateProviders().FirstOrDefault((ImageProvider t) => (t.Capabilities & ImageProviderCapabilities.FastFormatCheck) != 0 && t.FastFormatCheck(source));
				if (imageProvider2 != null)
				{
					imageProvider2.Source = source;
					return imageProvider2;
				}
			}
			catch
			{
			}
			return null;
		}

		public override FileFormat GetSourceFormat(string source)
		{
            return CreateSourceProvider(source).DefaultFileFormat;
		}
	}
}
