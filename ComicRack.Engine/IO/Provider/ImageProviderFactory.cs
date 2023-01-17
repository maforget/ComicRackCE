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
            ImageProvider provider = CreateSourceProvider(source);
            ProviderInfo chosenProviderInfo = GetProviderInfos().FirstOrDefault(x => x.ProviderType == provider.GetType());
            FileFormat chosenFileFormat = chosenProviderInfo.Formats.FirstOrDefault((FileFormat ff) => ff.Supports(source));
			return chosenFileFormat ?? provider.DefaultFileFormat;
        }
	}
}
