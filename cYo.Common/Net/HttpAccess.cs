using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Threading;
using cYo.Common.Win32;

namespace cYo.Common.Net
{
	public class HttpAccess
	{
		public const string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";

		private static HashSet<string> ignored;

		public string UserAgent
		{
			get;
			set;
		}

		public bool AskProxyCredentials
		{
			get;
			set;
		}

		public NetworkCredential ProxyCredentials
		{
			get;
			set;
		}

		public bool AskSecureCredentials
		{
			get;
			set;
		}

		public NetworkCredential SecureCredentials
		{
			get;
			set;
		}

		private static HashSet<string> Ignored
		{
			get
			{
				if (ignored == null)
				{
					ignored = new HashSet<string>();
				}
				return ignored;
			}
		}

		public HttpAccess()
		{
			AskProxyCredentials = true;
			AskSecureCredentials = false;
			UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";
		}

		public T GetResponse<T>(Action create, Action<ICredentials> setProxyCredentials, Action<ICredentials> setSecureCredentials, Func<Uri> getProxyUri, Func<Uri> getConnectionUri, Func<T> connect)
		{
			while (true)
			{
				create();
				try
				{
					if (ProxyCredentials != null)
					{
						setProxyCredentials(ProxyCredentials);
					}
					if (SecureCredentials != null)
					{
						CredentialCache credentialCache = new CredentialCache();
						SecureCredentials.Domain = string.Empty;
						credentialCache.Add(getConnectionUri(), "BASIC", SecureCredentials);
						setSecureCredentials(credentialCache);
					}
					return connect();
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("407"))
					{
						if (!AskProxyCredentials)
						{
							throw;
						}
						NetworkCredential networkCredential = HandleCredentials(ProxyCredentials, getProxyUri());
						if (networkCredential == null)
						{
							throw;
						}
						ProxyCredentials = networkCredential;
						continue;
					}
					if (ex.Message.Contains("401"))
					{
						if (!AskSecureCredentials)
						{
							throw;
						}
						NetworkCredential networkCredential2 = HandleCredentials(SecureCredentials, getConnectionUri());
						if (networkCredential2 == null)
						{
							throw;
						}
						SecureCredentials = networkCredential2;
						continue;
					}
					throw;
				}
			}
		}

		//public void WrapSoap<T>(T request, Action<T> call) where T : SoapHttpClientProtocol
		//{
		//	GetResponse(delegate
		//	{
		//		request.Proxy = WebRequest.DefaultWebProxy;
		//	}, delegate(ICredentials c)
		//	{
		//		if (request.Proxy != null)
		//		{
		//			request.Proxy.Credentials = c;
		//		}
		//	}, delegate(ICredentials c)
		//	{
		//		request.Credentials = c;
		//	}, () => request.Proxy.GetProxy(new Uri(request.Url)), () => new Uri(request.Url), delegate
		//	{
		//		request.UserAgent = UserAgent;
		//		call(request);
		//		return true;
		//	});
		//}

		public Stream GetStream(Uri uri)
		{
			HttpWebRequest request = null;
			return GetResponse((Action)delegate
			{
				request = (HttpWebRequest)WebRequest.Create(uri);
			}, (Action<ICredentials>)delegate(ICredentials c)
			{
				if (request.Proxy != null)
				{
					request.Proxy.Credentials = c;
				}
			}, (Action<ICredentials>)delegate(ICredentials c)
			{
				request.Credentials = c;
			}, (Func<Uri>)(() => request.Proxy.GetProxy(uri)), (Func<Uri>)(() => uri), (Func<Stream>)delegate
			{
				request.UserAgent = UserAgent;
				request.KeepAlive = true;
				request.Accept = "*/*";
				WebResponse response = request.GetResponse();
				try
				{
					StreamEx streamEx = new StreamEx(response.GetResponseStream());
					streamEx.Closed += delegate
					{
						response.SafeDispose();
					};
					return streamEx;
				}
				catch
				{
					response.SafeDispose();
					throw;
				}
			});
		}

		private static NetworkCredential HandleCredentials(ICredentials current, Uri uri)
		{
			string text = uri.Authority;
			if (uri.AbsolutePath != "/")
			{
				text += uri.AbsolutePath;
			}
			using (ItemMonitor.Lock(typeof(HttpAccess)))
			{
				using (UserCredentialsDialog userCredentialsDialog = new UserCredentialsDialog(text))
				{
					if (current == null)
					{
						userCredentialsDialog.Flags &= ~UserCredentialsDialogFlags.AlwaysShowUI;
					}
					else if (Ignored.Contains(text))
					{
						return null;
					}
					if (userCredentialsDialog.ShowDialog() != DialogResult.OK)
					{
						Ignored.Add(text);
						return null;
					}
					if (userCredentialsDialog.SaveChecked)
					{
						userCredentialsDialog.ConfirmCredentials(confirm: true);
					}
					return new NetworkCredential(userCredentialsDialog.User, userCredentialsDialog.PasswordToString());
				}
			}
		}

		public static string ReadText(string uri)
		{
			Uri uri2 = new Uri(uri);
			if (uri2.IsFile)
			{
				return File.ReadAllText(uri2.LocalPath);
			}
			HttpAccess httpAccess = new HttpAccess();
			using (Stream stream = httpAccess.GetStream(uri2))
			{
				using (StreamReader streamReader = new StreamReader(stream))
				{
					return streamReader.ReadToEnd();
				}
			}
		}

		public static byte[] ReadBinary(string uri)
		{
			Uri uri2 = new Uri(uri);
			if (uri2.IsFile)
			{
				return File.ReadAllBytes(uri2.LocalPath);
			}
			HttpAccess httpAccess = new HttpAccess();
			using (Stream stream = httpAccess.GetStream(uri2))
			{
				MemoryStream memoryStream = new MemoryStream();
				byte[] array = new byte[10000];
				int count;
				while ((count = stream.Read(array, 0, array.Length)) > 0)
				{
					memoryStream.Write(array, 0, count);
				}
				return memoryStream.ToArray();
			}
		}

		//public static K CallSoap<T, K>(Func<T, K> call) where T : SoapHttpClientProtocol, new()
		//{
		//	using (T soap = new T())
		//	{
		//		return CallSoap(soap, call);
		//	}
		//}

		//public static K CallSoap<T, K>(T soap, Func<T, K> call) where T : SoapHttpClientProtocol
		//{
		//	K result = default(K);
		//	new HttpAccess().WrapSoap(soap, delegate(T r)
		//	{
		//		result = call(r);
		//	});
		//	return result;
		//}
	}
}
