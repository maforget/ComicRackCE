using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class XmlInfoFileAttribute : Attribute
	{
		public string XmlInfoFile { get; set; }

		public int Order { get; set; }

        public XmlInfoType XmlInfoType { get; set; }

        public XmlInfoFileAttribute(string xmlInfoFile, int order, XmlInfoType xmlInfoType = XmlInfoType.ComicInfo)
		{
			XmlInfoFile = xmlInfoFile;
			Order = order;
            XmlInfoType = xmlInfoType;
        }
	}
}
