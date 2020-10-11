using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[GeneratedCode("wsdl", "2.0.50727.3038")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class UnregisterCompletedEventArgs : AsyncCompletedEventArgs
	{
		private object[] results;

		public bool Result
		{
			get
			{
				RaiseExceptionIfNecessary();
				return (bool)results[0];
			}
		}

		internal UnregisterCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
