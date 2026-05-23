using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	[XmlInfoFile("ComicInfo.xml", 0, XmlInfoType.ComicInfo)]
	public class ComicInfoProvider : XmlInfoProvider<ComicInfo, ComicInfo>
	{
		public override ComicInfo ToXml(ComicInfo xmlInfo) => xmlInfo;
	}
}
