using System;
using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	public class XmlInfoProviderInfo : IProviderInfo
	{
		public Type ProviderType
		{
			get;
			private set;
		}

		public XmlInfoFile XmlInfoFile
		{
			get;
			private set;
		}

		public XmlInfoProviderInfo(Type providerType, XmlInfoFile xmlInfofile)
		{
			XmlInfoFile = xmlInfofile;
			ProviderType = providerType;
		}
	}

	public record XmlInfoFile(string FileName, int Order);
}
