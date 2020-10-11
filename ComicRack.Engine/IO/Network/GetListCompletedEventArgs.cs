using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[GeneratedCode("wsdl", "2.0.50727.3038")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class GetListCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public ServerInfo[] Result
		{
			get
			{
				RaiseExceptionIfNecessary();
				return (ServerInfo[])results[0];
			}
		}

		internal GetListCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
