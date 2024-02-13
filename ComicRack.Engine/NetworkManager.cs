using System;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Net;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.IO.Network;

namespace cYo.Projects.ComicRack.Engine
{
	public class NetworkManager : DisposableObject
	{
		public class RemoteServerStartedEventArgs : EventArgs
		{
			public ShareInformation Information
			{
				get;
				private set;
			}

			public RemoteServerStartedEventArgs(ShareInformation information)
			{
				Information = information;
			}
		}

		public class RemoteServerStoppedEventArgs : EventArgs
		{
			public string Address
			{
				get;
				private set;
			}

			public RemoteServerStoppedEventArgs(string address)
			{
				Address = address;
			}
		}

		public const int BroadcastPort = 7613;

		private Broadcaster<BroadcastData> broadcaster;

		private readonly SmartList<ComicLibraryServer> runningServers = new SmartList<ComicLibraryServer>();

		private readonly Dictionary<string, ShareInformation> localShares = new Dictionary<string, ShareInformation>();

		public DatabaseManager DatabaseManager
		{
			get;
			private set;
		}

		public CacheManager CacheManager
		{
			get;
			private set;
		}

		public int PrivatePort
		{
			get;
			set;
		}

		public int PublicPort
		{
			get;
			set;
		}

		public bool DisableBroadcast
		{
			get;
			set;
		}

		public ISharesSettings Settings
		{
			get;
			private set;
		}

		public Broadcaster<BroadcastData> Broadcaster
		{
			get
			{
				if (broadcaster == null && !DisableBroadcast)
				{
					broadcaster = new Broadcaster<BroadcastData>(BroadcastPort);
				}
				return broadcaster;
			}
		}

		public SmartList<ComicLibraryServer> RunningServers => runningServers;

		public Dictionary<string, ShareInformation> LocalShares => localShares;

		public event EventHandler<RemoteServerStartedEventArgs> RemoteServerStarted;

		public event EventHandler<RemoteServerStoppedEventArgs> RemoteServerStopped;

		public NetworkManager(DatabaseManager databaseManager, CacheManager cacheManager, ISharesSettings settings, int privatePort, int publicPort, bool disableBroadcast)
		{
			DatabaseManager = databaseManager;
			CacheManager = cacheManager;
			Settings = settings;
			PrivatePort = privatePort;
			PublicPort = publicPort;
			DisableBroadcast = disableBroadcast;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stop();
				Broadcaster.SafeDispose();
			}
			base.Dispose(disposing);
		}

		public bool IsOwnServer(string serverAddress)
		{
			return runningServers.Any((ComicLibraryServer rs) => rs.GetAnnouncementUri() == serverAddress);
		}

		public bool HasActiveServers()
		{
			return runningServers.Count > 0;
		}

		public bool RecentServerActivity(int seconds = 10)
		{
			return runningServers.Any((ComicLibraryServer s) => s.Statistics.WasActive(seconds));
		}

		public void BroadcastStart()
		{
			if (Broadcaster != null)
			{
				Broadcaster.Broadcast(new BroadcastData(BroadcastType.ClientStarted));
			}
		}

		public void BroadcastStop()
		{
			if (Broadcaster != null)
			{
				Broadcaster.Broadcast(new BroadcastData(BroadcastType.ClientStopped));
			}
		}

		public void Start()
		{
			ComicLibraryServer.ExternalServerAddress = Settings.ExternalServerAddress;
			if (Broadcaster != null)
			{
				Broadcaster.Listen = true;
				Broadcaster.Recieved += BroadcasterRecieved;
			}
			foreach (ComicLibraryServerConfig share in Settings.Shares)
			{
				share.OnlyPrivateConnections = !share.IsInternet && PrivatePort == PublicPort;
				share.PrivateListPassword = ((share.IsInternet && share.IsPrivate) ? Settings.PrivateListingPassword : string.Empty);
			}
			runningServers.AddRange(ComicLibraryServer.Start(Settings.Shares.Where((ComicLibraryServerConfig sc) => !sc.IsInternet), PrivatePort, () => DatabaseManager.Database, CacheManager.ImagePool, CacheManager.ImagePool, Broadcaster));
			runningServers.AddRange(ComicLibraryServer.Start(Settings.Shares.Where((ComicLibraryServerConfig sc) => sc.IsInternet), PublicPort, () => DatabaseManager.Database, CacheManager.ImagePool, CacheManager.ImagePool, Broadcaster));
		}

		public void Stop()
		{
			if (Broadcaster != null)
			{
				Broadcaster.Recieved -= BroadcasterRecieved;
				Broadcaster.Listen = false;
			}
			runningServers.ForEach(delegate(ComicLibraryServer s)
			{
				s.Stop();
			});
			runningServers.Clear();
		}

		private void BroadcasterRecieved(object sender, BroadcastEventArgs<BroadcastData> e)
		{
			if (Broadcaster == null)
			{
				return;
			}
			switch (e.Data.BroadcastType)
			{
			case BroadcastType.ClientStarted:
				foreach (ComicLibraryServer item in runningServers.Where((ComicLibraryServer si) => !si.Config.IsInternet))
				{
					Broadcaster.Broadcast(new BroadcastData(BroadcastType.ServerStarted, item.Config.ServiceName, item.Config.ServicePort));
				}
				break;
			case BroadcastType.ServerStarted:
			{
				string text2 = ServiceAddress.Append(e.Address, e.Data.ServerPort.ToString(), e.Data.ServerName);
				using (ItemMonitor.Lock(localShares))
				{
					if (localShares.ContainsKey(text2))
					{
						return;
					}
				}
				ShareInformation serverInfo = ComicLibraryClient.GetServerInfo(text2);
				if (serverInfo != null)
				{
					using (ItemMonitor.Lock(localShares))
					{
						serverInfo.IsLocal = true;
						localShares[text2] = serverInfo;
					}
					if (Settings.LookForShared && this.RemoteServerStarted != null)
					{
						this.RemoteServerStarted(this, new RemoteServerStartedEventArgs(serverInfo));
					}
				}
				break;
			}
			case BroadcastType.ServerStopped:
			{
				string text = ServiceAddress.Append(e.Address, e.Data.ServerPort.ToString(), e.Data.ServerName);
				using (ItemMonitor.Lock(localShares))
				{
					localShares.Remove(text);
				}
				if (this.RemoteServerStopped != null)
				{
					this.RemoteServerStopped(this, new RemoteServerStoppedEventArgs(text));
				}
				break;
			}
			case BroadcastType.ClientStopped:
				break;
			}
		}
	}
}
