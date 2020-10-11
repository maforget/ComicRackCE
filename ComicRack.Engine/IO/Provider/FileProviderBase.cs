using cYo.Common.ComponentModel;
using cYo.Common.Reflection;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public abstract class FileProviderBase : DisposableObject
	{
		private FileFormat defaultFileFormat;

		public FileFormat DefaultFileFormat
		{
			get
			{
				if (defaultFileFormat == null)
				{
					FileFormatAttribute attribute = GetType().GetAttribute<FileFormatAttribute>();
					if (attribute != null)
					{
						defaultFileFormat = attribute.Format;
					}
				}
				return defaultFileFormat;
			}
		}

		public virtual int FormatId => DefaultFileFormat.Id;
	}
}
