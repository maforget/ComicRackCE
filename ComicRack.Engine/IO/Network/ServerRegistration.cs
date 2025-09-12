using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	//[GeneratedCode("wsdl", "2.0.50727.3038")]
	//[DebuggerStepThrough]
	//[DesignerCategory("code")]
	//[WebServiceBinding(Name = "ServerRegistrationBinding", Namespace = "urn:ServerRegistration")]
	//[SoapInclude(typeof(ServerInfo))]
	//public class ServerRegistration : SoapHttpClientProtocol
	//{
	//	private SendOrPostCallback RegisterOperationCompleted;

	//	private SendOrPostCallback UnregisterOperationCompleted;

	//	private SendOrPostCallback RefreshOperationCompleted;

	//	private SendOrPostCallback GetListOperationCompleted;

	//	public event RegisterCompletedEventHandler RegisterCompleted;

	//	public event UnregisterCompletedEventHandler UnregisterCompleted;

	//	public event RefreshCompletedEventHandler RefreshCompleted;

	//	public event GetListCompletedEventHandler GetListCompleted;

	//	public ServerRegistration()
	//	{
	//		base.Url = "http://comicrack.cyolito.com/services/ServerRegistration2.php";
	//	}

	//	[SoapRpcMethod("urn:ServerRegistration#Register", RequestNamespace = "urn:ServerRegistration", ResponseNamespace = "urn:ServerRegistration")]
	//	[return: SoapElement("result")]
	//	public bool Register(string uri, string name, string comment, int options, string password)
	//	{
	//		object[] array = Invoke("Register", new object[5]
	//		{
	//			uri,
	//			name,
	//			comment,
	//			options,
	//			password
	//		});
	//		return (bool)array[0];
	//	}

	//	public IAsyncResult BeginRegister(string uri, string name, string comment, int options, string password, AsyncCallback callback, object asyncState)
	//	{
	//		return BeginInvoke("Register", new object[5]
	//		{
	//			uri,
	//			name,
	//			comment,
	//			options,
	//			password
	//		}, callback, asyncState);
	//	}

	//	public bool EndRegister(IAsyncResult asyncResult)
	//	{
	//		object[] array = EndInvoke(asyncResult);
	//		return (bool)array[0];
	//	}

	//	public void RegisterAsync(string uri, string name, string comment, int options, string password)
	//	{
	//		RegisterAsync(uri, name, comment, options, password, null);
	//	}

	//	public void RegisterAsync(string uri, string name, string comment, int options, string password, object userState)
	//	{
	//		if (RegisterOperationCompleted == null)
	//		{
	//			RegisterOperationCompleted = OnRegisterOperationCompleted;
	//		}
	//		InvokeAsync("Register", new object[5]
	//		{
	//			uri,
	//			name,
	//			comment,
	//			options,
	//			password
	//		}, RegisterOperationCompleted, userState);
	//	}

	//	private void OnRegisterOperationCompleted(object arg)
	//	{
	//		if (this.RegisterCompleted != null)
	//		{
	//			InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
	//			this.RegisterCompleted(this, new RegisterCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
	//		}
	//	}

	//	[SoapRpcMethod("urn:ServerRegistration#Unregister", RequestNamespace = "urn:ServerRegistration", ResponseNamespace = "urn:ServerRegistration")]
	//	[return: SoapElement("result")]
	//	public bool Unregister(string uri)
	//	{
	//		object[] array = Invoke("Unregister", new object[1]
	//		{
	//			uri
	//		});
	//		return (bool)array[0];
	//	}

	//	public IAsyncResult BeginUnregister(string uri, AsyncCallback callback, object asyncState)
	//	{
	//		return BeginInvoke("Unregister", new object[1]
	//		{
	//			uri
	//		}, callback, asyncState);
	//	}

	//	public bool EndUnregister(IAsyncResult asyncResult)
	//	{
	//		object[] array = EndInvoke(asyncResult);
	//		return (bool)array[0];
	//	}

	//	public void UnregisterAsync(string uri)
	//	{
	//		UnregisterAsync(uri, null);
	//	}

	//	public void UnregisterAsync(string uri, object userState)
	//	{
	//		if (UnregisterOperationCompleted == null)
	//		{
	//			UnregisterOperationCompleted = OnUnregisterOperationCompleted;
	//		}
	//		InvokeAsync("Unregister", new object[1]
	//		{
	//			uri
	//		}, UnregisterOperationCompleted, userState);
	//	}

	//	private void OnUnregisterOperationCompleted(object arg)
	//	{
	//		if (this.UnregisterCompleted != null)
	//		{
	//			InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
	//			this.UnregisterCompleted(this, new UnregisterCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
	//		}
	//	}

	//	[SoapRpcMethod("urn:ServerRegistration#Refresh", RequestNamespace = "urn:ServerRegistration", ResponseNamespace = "urn:ServerRegistration")]
	//	[return: SoapElement("result")]
	//	public bool Refresh(string uri)
	//	{
	//		object[] array = Invoke("Refresh", new object[1]
	//		{
	//			uri
	//		});
	//		return (bool)array[0];
	//	}

	//	public IAsyncResult BeginRefresh(string uri, AsyncCallback callback, object asyncState)
	//	{
	//		return BeginInvoke("Refresh", new object[1]
	//		{
	//			uri
	//		}, callback, asyncState);
	//	}

	//	public bool EndRefresh(IAsyncResult asyncResult)
	//	{
	//		object[] array = EndInvoke(asyncResult);
	//		return (bool)array[0];
	//	}

	//	public void RefreshAsync(string uri)
	//	{
	//		RefreshAsync(uri, null);
	//	}

	//	public void RefreshAsync(string uri, object userState)
	//	{
	//		if (RefreshOperationCompleted == null)
	//		{
	//			RefreshOperationCompleted = OnRefreshOperationCompleted;
	//		}
	//		InvokeAsync("Refresh", new object[1]
	//		{
	//			uri
	//		}, RefreshOperationCompleted, userState);
	//	}

	//	private void OnRefreshOperationCompleted(object arg)
	//	{
	//		if (this.RefreshCompleted != null)
	//		{
	//			InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
	//			this.RefreshCompleted(this, new RefreshCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
	//		}
	//	}

	//	[SoapRpcMethod("urn:ServerRegistration#GetList", RequestNamespace = "urn:ServerRegistration", ResponseNamespace = "urn:ServerRegistration")]
	//	[return: SoapElement("result")]
	//	public ServerInfo[] GetList(int mask, string password)
	//	{
	//		object[] array = Invoke("GetList", new object[2]
	//		{
	//			mask,
	//			password
	//		});
	//		return (ServerInfo[])array[0];
	//	}

	//	public IAsyncResult BeginGetList(int mask, string password, AsyncCallback callback, object asyncState)
	//	{
	//		return BeginInvoke("GetList", new object[2]
	//		{
	//			mask,
	//			password
	//		}, callback, asyncState);
	//	}

	//	public ServerInfo[] EndGetList(IAsyncResult asyncResult)
	//	{
	//		object[] array = EndInvoke(asyncResult);
	//		return (ServerInfo[])array[0];
	//	}

	//	public void GetListAsync(int mask, string password)
	//	{
	//		GetListAsync(mask, password, null);
	//	}

	//	public void GetListAsync(int mask, string password, object userState)
	//	{
	//		if (GetListOperationCompleted == null)
	//		{
	//			GetListOperationCompleted = OnGetListOperationCompleted;
	//		}
	//		InvokeAsync("GetList", new object[2]
	//		{
	//			mask,
	//			password
	//		}, GetListOperationCompleted, userState);
	//	}

	//	private void OnGetListOperationCompleted(object arg)
	//	{
	//		if (this.GetListCompleted != null)
	//		{
	//			InvokeCompletedEventArgs invokeCompletedEventArgs = (InvokeCompletedEventArgs)arg;
	//			this.GetListCompleted(this, new GetListCompletedEventArgs(invokeCompletedEventArgs.Results, invokeCompletedEventArgs.Error, invokeCompletedEventArgs.Cancelled, invokeCompletedEventArgs.UserState));
	//		}
	//	}

	//	public new void CancelAsync(object userState)
	//	{
	//		base.CancelAsync(userState);
	//	}
	//}
}
