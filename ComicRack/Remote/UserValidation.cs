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
	[GeneratedCode("wsdl", "2.0.50727.3038")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[WebServiceBinding(Name = "UserValidationBinding", Namespace = "urn:UserValidation")]
	public class UserValidation : SoapHttpClientProtocol
	{
		private SendOrPostCallback HasDonatedOperationCompleted;

		public event HasDonatedCompletedEventHandler HasDonatedCompleted;

		public UserValidation()
		{
			base.Url = "http://comicrack.cyolito.com/services/UserValidation.php";
		}

		[SoapRpcMethod("urn:UserValidation#HasDonated", RequestNamespace = "urn:UserValidation", ResponseNamespace = "urn:UserValidation")]
		[return: SoapElement("result")]
		public bool HasDonated(string user)
		{
			object[] array = Invoke("HasDonated", new object[1]
			{
				user
			});
			return (bool)array[0];
		}

		public IAsyncResult BeginHasDonated(string user, AsyncCallback callback, object asyncState)
		{
			return BeginInvoke("HasDonated", new object[1]
			{
				user
			}, callback, asyncState);
		}

		public bool EndHasDonated(IAsyncResult asyncResult)
		{
			object[] array = EndInvoke(asyncResult);
			return (bool)array[0];
		}

		public void HasDonatedAsync(string user)
		{
			HasDonatedAsync(user, null);
		}

		public void HasDonatedAsync(string user, object userState)
		{
			if (HasDonatedOperationCompleted == null)
			{
				HasDonatedOperationCompleted = OnHasDonatedOperationCompleted;
			}
			InvokeAsync("HasDonated", new object[1]
			{
				user
			}, HasDonatedOperationCompleted, userState);
		}

		private void OnHasDonatedOperationCompleted(object arg)
		{
			if (this.HasDonatedCompleted != null)
			{
				InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
				this.HasDonatedCompleted(this, new HasDonatedCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
			}
		}

		public new void CancelAsync(object userState)
		{
			base.CancelAsync(userState);
		}
	}
}
