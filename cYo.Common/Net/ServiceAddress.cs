using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using cYo.Common.Text;

namespace cYo.Common.Net
{
	public class ServiceAddress
	{
		private static string wanIp;

		private static Regex rxUrlSplit = new Regex("^(?<host>[^:/]+)(:(?<port>\\d+))?(/(?<path>.*))?");

		public string Host
		{
			get;
			set;
		}

		public string Port
		{
			get;
			set;
		}

		public string Service
		{
			get;
			set;
		}

		public bool IsValid
		{
			get;
			private set;
		}

		public ServiceAddress(IPAddress address)
		{
			Host = address.ToString();
			IsValid = true;
		}

		public ServiceAddress(string host, string port, string path)
		{
			Host = host;
			Port = port;
			Service = path;
			IsValid = true;
		}

		public ServiceAddress(string serviceAddress)
		{
			IsValid = TryParse(serviceAddress, out var host, out var port, out var path);
			Host = host;
			Port = port;
			Service = path;
		}

		public override string ToString()
		{
			return Host.AppendWithSeparator(":", Port).AppendWithSeparator("/", Service);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static string GetWanAddress()
		{
			return GetWanAddress(refresh: false);
		}

		public static string GetWanAddress(bool refresh)
		{
			if (!refresh && wanIp != null)
			{
				return wanIp;
			}
			try
			{
				//HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://comicrack.cyolito.com/services/ClientAddress.php");
				//httpWebRequest.Accept = "*/*";
				//using (WebResponse webResponse = httpWebRequest.GetResponse())
				//{
				//	using (Stream stream = webResponse.GetResponseStream())
				//	{
				//		using (StreamReader streamReader = new StreamReader(stream))
				//		{
				//			wanIp = streamReader.ReadToEnd();
				//		}
				//	}
				//}
			}
			catch (Exception)
			{
				wanIp = string.Empty;
			}
			if (!string.IsNullOrEmpty(wanIp))
			{
				return wanIp;
			}
			return null;
		}

		public static string CompletePortAndPath(string host, string newPort, string newPath)
		{
			if (string.IsNullOrEmpty(host))
			{
				return host;
			}
			if (!TryParse(host, out var host2, out var port, out var path))
			{
				throw new ArgumentException("is not valid", "host");
			}
			if (string.IsNullOrEmpty(port))
			{
				port = newPort;
			}
			if (string.IsNullOrEmpty(path))
			{
				path = newPath;
			}
			return host2.AppendWithSeparator(":", port).AppendWithSeparator("/", path);
		}

		public static string Append(IPAddress address, string newPort, string newPath)
		{
			return CompletePortAndPath(address.ToString(), newPort, newPath);
		}

		public static bool TryParse(string address, out string host, out string port, out string path)
		{
			Match match = rxUrlSplit.Match(address ?? string.Empty);
			if (!match.Success)
			{
				host = (port = (path = null));
				return false;
			}
			host = match.Groups["host"].Value.Trim();
			port = match.Groups["port"].Value.Trim();
			path = match.Groups["path"].Value.Trim();
			return !string.IsNullOrEmpty(host);
		}

		public static bool IsPrivate(string address)
		{
			try
			{
				ServiceAddress serviceAddress = new ServiceAddress(address);
				return Dns.GetHostAddresses(serviceAddress.Host).All((IPAddress ip) => ip.IsPrivate());
			}
			catch (Exception)
			{
			}
			return true;
		}
	}
}
