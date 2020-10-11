using System;
using System.Net;

namespace cYo.Common.Net
{
	public class BroadcastEventArgs<T> : EventArgs
	{
		private readonly T data;

		private readonly IPAddress address;

		public T Data => data;

		public IPAddress Address => address;

		public BroadcastEventArgs(T data, IPAddress address)
		{
			this.data = data;
			this.address = address;
		}
	}
}
