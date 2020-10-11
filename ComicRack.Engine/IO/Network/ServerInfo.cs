using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[Serializable]
	[GeneratedCode("wsdl", "2.0.50727.3038")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[SoapType(Namespace = "urn:ServerRegistration")]
	public class ServerInfo
	{
		public string Uri
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Comment
		{
			get;
			set;
		}

		public int Options
		{
			get;
			set;
		}
	}
}
