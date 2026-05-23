using System;
using System.IO;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
    public abstract class XmlInfoProvider
    {
    }

    public abstract class XmlInfoProvider<TXmlInfoResult>: XmlInfoProvider where TXmlInfoResult : class
    {
		public abstract TXmlInfoResult Deserialize(Func<string, Stream> getDataDelegate, string filename);
	}

    public abstract class XmlInfoProvider<TSourceProvider, TXmlInfoResult> : XmlInfoProvider<TXmlInfoResult>
		where TSourceProvider : class
		where TXmlInfoResult : class
	{
		public abstract TXmlInfoResult ToXml(TSourceProvider xmlInfo);

		public override TXmlInfoResult Deserialize(Func<string, Stream> getDataDelegate, string filename)
		{
			try
			{
				using (Stream inStream = getDataDelegate(filename))
				{
					return ToXml(XmlUtility.GetSerializer<TSourceProvider>().Deserialize(inStream) as TSourceProvider);
				}
			}
			catch
			{
				return default;
			}
		}
	}
}