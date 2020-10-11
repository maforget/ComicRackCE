using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using cYo.Common.ComponentModel;
using cYo.Common.Xml;

namespace cYo.Common.Net
{
	public class Broadcaster<T> : DisposableObject, IBroadcast<T>
	{
		private int port;

		private bool listen;

		private UdpClient listener;

		private IPEndPoint listenerEP;

		public int Port
		{
			get
			{
				return port;
			}
			set
			{
				if (port != value)
				{
					port = value;
					if (listen)
					{
						StopListening();
						StartListening();
					}
				}
			}
		}

		public bool Listen
		{
			get
			{
				return listen;
			}
			set
			{
				if (listen != value)
				{
					listen = value;
					if (listen)
					{
						StartListening();
					}
					else
					{
						StopListening();
					}
				}
			}
		}

		public IEnumerable<IPEndPoint> LocalEndpoints => from ipa in Dns.GetHostAddresses(string.Empty)
			where ipa.AddressFamily == AddressFamily.InterNetwork
			select new IPEndPoint(ipa, 0);

		public event EventHandler<BroadcastEventArgs<T>> Recieved;

		public Broadcaster()
		{
		}

		public Broadcaster(int port)
		{
			this.port = port;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				StopListening();
			}
			base.Dispose(disposing);
		}

		private bool StartListening()
		{
			if (listener != null)
			{
				return true;
			}
			try
			{
				listener = new UdpClient(port);
				listenerEP = new IPEndPoint(IPAddress.Any, port);
				listener.EnableBroadcast = true;
				listener.BeginReceive(OnReceivedData, this);
				return true;
			}
			catch (Exception)
			{
				StopListening();
				return false;
			}
		}

		private void StopListening()
		{
			try
			{
				UdpClient udpClient = listener;
				if (udpClient != null)
				{
					listener = null;
					udpClient.Close();
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				listener = null;
			}
		}

		protected virtual void OnReceivedData(IAsyncResult ar)
		{
			try
			{
				if (listener == null)
				{
					return;
				}
				try
				{
					byte[] bytes = listener.EndReceive(ar, ref listenerEP);
					if (LocalEndpoints.FirstOrDefault((IPEndPoint ep) => ep.Address.Equals(listenerEP.Address)) == null)
					{
						T data = XmlUtility.Load<T>(bytes);
						OnRecieved(new BroadcastEventArgs<T>(data, listenerEP.Address));
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					listener.BeginReceive(OnReceivedData, this);
				}
			}
			catch
			{
			}
		}

		protected virtual void OnRecieved(BroadcastEventArgs<T> bea)
		{
			if (this.Recieved != null)
			{
				this.Recieved(this, bea);
			}
		}

		public bool Broadcast(T data)
		{
			try
			{
				foreach (IPEndPoint localEndpoint in LocalEndpoints)
				{
					using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
					{
						socket.Bind(localEndpoint);
						socket.EnableBroadcast = true;
						socket.SendTo(XmlUtility.Store(data, compressed: true), new IPEndPoint(IPAddress.Broadcast, port));
					}
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
