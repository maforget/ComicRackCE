using System;
using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public class ProviderInfo : IProviderInfo
	{
		public Type ProviderType
		{
			get;
			private set;
		}

		public IEnumerable<FileFormat> Formats
		{
			get;
			private set;
		}

		public ProviderInfo(Type providerType, IEnumerable<FileFormat> formats)
		{
			Formats = formats;
			ProviderType = providerType;
		}
	}
}
