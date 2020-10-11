using System.Net;
using System.Net.Sockets;

namespace cYo.Common.Net
{
	public static class IPAddressExtension
	{
		public static bool IsPrivate(this IPAddress address)
		{
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				return ((IPAddressV4)address).IsPrivate();
			}
			if (address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				if (!address.IsIPv6LinkLocal && !address.IsIPv6SiteLocal)
				{
					return IPAddress.IsLoopback(address);
				}
				return true;
			}
			return true;
		}
	}
}
