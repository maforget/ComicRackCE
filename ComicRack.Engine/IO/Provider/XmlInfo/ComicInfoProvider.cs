using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	[XmlInfoFile("ComicInfo.xml", 0)]
	public class ComicInfoProvider : XmlInfoProvider<ComicInfo>
	{
		public override ComicInfo ToComicInfo(ComicInfo xmlInfo) => xmlInfo;
	}
}
