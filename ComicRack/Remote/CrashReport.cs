using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Viewer.Remote
{
	[GeneratedCode("wsdl", "2.0.50727.1432")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name = "CrashReportBinding", Namespace = "urn:CrashReport")]
	public class CrashReport : SoapHttpClientProtocol
	{
		private SendOrPostCallback SubmitReportOperationCompleted;

		public event SubmitReportCompletedEventHandler SubmitReportCompleted;

		public CrashReport()
		{
			base.Url = "http://comicrack.cyolito.com/services/CrashReport.php";
		}

		[SoapRpcMethod("urn:CrashReport#SubmitReport", RequestNamespace = "urn:CrashReport", ResponseNamespace = "urn:CrashReport")]
		[return: SoapElement("result")]
		public string SubmitReport(string appication, string report)
		{
			object[] array = Invoke("SubmitReport", new object[2]
			{
				appication,
				report
			});
			return (string)array[0];
		}

		public IAsyncResult BeginSubmitReport(string appication, string report, AsyncCallback callback, object asyncState)
		{
			return BeginInvoke("SubmitReport", new object[2]
			{
				appication,
				report
			}, callback, asyncState);
		}

		public string EndSubmitReport(IAsyncResult asyncResult)
		{
			object[] array = EndInvoke(asyncResult);
			return (string)array[0];
		}

		public void SubmitReportAsync(string appication, string report)
		{
			SubmitReportAsync(appication, report, null);
		}

		public void SubmitReportAsync(string appication, string report, object userState)
		{
			if (SubmitReportOperationCompleted == null)
			{
				SubmitReportOperationCompleted = OnSubmitReportOperationCompleted;
			}
			InvokeAsync("SubmitReport", new object[2]
			{
				appication,
				report
			}, SubmitReportOperationCompleted, userState);
		}

		private void OnSubmitReportOperationCompleted(object arg)
		{
			if (this.SubmitReportCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.SubmitReportCompleted(this, new SubmitReportCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}
	}
}
