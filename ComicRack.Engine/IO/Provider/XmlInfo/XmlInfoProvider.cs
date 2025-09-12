using System;
using System.IO;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	public abstract class XmlInfoProvider
	{
		public abstract ComicInfo Deserialize(Func<string, Stream> getDataDelegate, string filename);
	}

	public abstract class XmlInfoProvider<T> : XmlInfoProvider where T : class
	{
		public abstract ComicInfo ToComicInfo(T xmlInfo);

		public override ComicInfo Deserialize(Func<string, Stream> getDataDelegate, string filename)
		{
			try
			{
				using (Stream inStream = getDataDelegate(filename))
				{
					return ToComicInfo(XmlUtility.GetSerializer<T>().Deserialize(inStream) as T);
				}
			}
			catch
			{
				return null;
			}
		}
	}
}