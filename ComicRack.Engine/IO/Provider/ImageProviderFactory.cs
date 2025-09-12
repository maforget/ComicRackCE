using System.CodeDom;
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

        protected override FileFormat GetActualSourceFormat(string source)
        {
			//returns the actual file format based on the actual providers used.
            return CreateSourceProvider(source).DefaultFileFormat;
        }
    }
}
