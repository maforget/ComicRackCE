using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;

namespace cYo.Projects.ComicRack.Viewer.Remote
{
	[GeneratedCode("wsdl", "2.0.50727.1432")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	public class SubmitReportCompletedEventArgs : AsyncCompletedEventArgs
	{
		private readonly object[] results;

		public string Result
		{
			get
			{
				RaiseExceptionIfNecessary();
				return (string)results[0];
			}
		}

		internal SubmitReportCompletedEventArgs(object[] results, Exception exception, bool cancelled, object userState)
			: base(exception, cancelled, userState)
		{
			this.results = results;
		}
	}
}
