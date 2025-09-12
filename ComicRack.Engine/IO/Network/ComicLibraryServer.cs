using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Net;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.Properties;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true, ConcurrencyMode = ConcurrencyMode.Multiple, AddressFilterMode = AddressFilterMode.Prefix)]
	public class ComicLibraryServer : IRemoteComicLibrary, IRemoteServerInfo, IDisposable
	{
		private class PasswordValidator : UserNamePasswordValidator
		{
			private readonly string password;

			public PasswordValidator(string password)
			{
				this.password = password;
			}

			public override void Validate(string userName, string password)
			{
				if (!string.IsNullOrEmpty(this.password) && this.password != password)
				{
					throw new SecurityTokenException("Validation Failed!");
				}
			}
		}

		public const string InfoPoint = "Info";

		public const string LibraryPoint = "Library";

		public const int ServerPingTime = 10000;

		public const int ServerAnnounceTime = 300000;

		public static string ExternalServerAddress;

		private static X509Certificate2 certificate;

		private Timer pingTimer;

		private Timer announceTimer;

		private readonly Func<ComicLibrary> getComicLibrary;

		private bool serverHasBeenAnnounced;

		private bool serverHasBeenValidated;

		private bool serverValidationFailed;

		private ServiceHost serviceHost;

		private readonly Cache<Guid, IImageProvider> providerCache = new Cache<Guid, IImageProvider>(EngineConfiguration.Default.ServerProviderCacheSize);

		//private static readonly ServerRegistration serverRegistration = new ServerRegistration();

		private static readonly Dictionary<int, int> shareCounts = new Dictionary<int, int>();

		public static X509Certificate2 Certificate
		{
			get
			{
				if (certificate == null)
				{
					certificate = new X509Certificate2(Resources.Certificate2, string.Empty);
                }
				return certificate;
			}
		}

		public string Id
		{
			get;
			private set;
		}

		public ComicLibraryServerConfig Config
		{
			get;
			private set;
		}

		public IBroadcast<BroadcastData> Broadcaster
		{
			get;
			private set;
		}

		public bool PingEnabled
		{
			get;
			set;
		}

		public IPagePool PagePool
		{
			get;
			set;
		}

		public IThumbnailPool ThumbPool
		{
			get;
			set;
		}

		public ComicLibrary ComicLibrary => getComicLibrary();

		public ServerStatistics Statistics
		{
			get;
			private set;
		}

		public bool IsRunning => serviceHost != null;

		public bool IsAnnounced => serverHasBeenAnnounced;

		string IRemoteServerInfo.Id
		{
			get
			{
				CheckPrivateNetwork();
				AddStats(ServerStatistics.StatisticType.InfoRequest);
				return Id;
			}
		}

		string IRemoteServerInfo.Name => Config.Name;

		string IRemoteServerInfo.Description => Config.Description;

		ServerOptions IRemoteServerInfo.Options => Config.Options;

		public bool IsValid
		{
			get
			{
				try
				{
					CheckPrivateNetwork();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}

		public ComicLibraryServer(ComicLibraryServerConfig config, Func<ComicLibrary> getComicLibrary, IPagePool pagePool, IThumbnailPool thumbPool, IBroadcast<BroadcastData> broadcaster)
		{
			Id = Guid.NewGuid().ToString();
			Config = CloneUtility.Clone(config);
			Statistics = new ServerStatistics();
			this.getComicLibrary = getComicLibrary;
			PagePool = pagePool;
			ThumbPool = thumbPool;
			Broadcaster = broadcaster;
			PingEnabled = true;
			providerCache.ItemRemoved += providerCache_ItemRemoved;
		}

		public void Dispose()
		{
			Stop();
			providerCache.Dispose();
		}

		byte[] IRemoteComicLibrary.GetLibraryData()
		{
			CheckPrivateNetwork();
			try
			{
				byte[] array = GetSharedComicLibrary().ToByteArray();
				AddStats(ServerStatistics.StatisticType.LibraryRequest, array.Length);
				return array;
			}
			catch (Exception)
			{
				return null;
			}
		}

		int IRemoteComicLibrary.GetImageCount(Guid comicGuid)
		{
			ComicBook comicBook = ComicLibrary.Books[comicGuid];
			try
			{
				if (comicBook.PageCount > 0)
				{
					return comicBook.PageCount;
				}
				using (IItemLock<IImageProvider> itemLock = providerCache.LockItem(comicGuid, CreateProvider))
				{
					comicBook.PageCount = itemLock.Item.Count;
					return comicBook.PageCount;
				}
			}
			catch (Exception)
			{
				return 0;
			}
		}

		byte[] IRemoteComicLibrary.GetImage(Guid comicGuid, int index)
		{
			CheckPrivateNetwork();
			ComicBook comicBook = ComicLibrary.Books[comicGuid];
			try
			{
				index = comicBook.TranslateImageIndexToPage(index);
				using (IItemLock<IImageProvider> itemLock = providerCache.LockItem(comicGuid, CreateProvider))
				{
					using (IItemLock<PageImage> itemLock2 = PagePool.GetPage(comicBook.GetPageKey(index, BitmapAdjustment.Empty), itemLock.Item, onErrorThrowException: true))
					{
						int pageQuality = Config.PageQuality;
						byte[] array = ((pageQuality != 100) ? itemLock2.Item.Bitmap.ImageToJpegBytes(75 * pageQuality / 100) : ((byte[])itemLock2.Item.Data.Clone()));
						AddStats(ServerStatistics.StatisticType.PageRequest, array.Length);
						return array;
					}
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		byte[] IRemoteComicLibrary.GetThumbnailImage(Guid comicGuid, int index)
		{
			ComicBook comicBook = ComicLibrary.Books[comicGuid];
			try
			{
				index = comicBook.TranslateImageIndexToPage(index);
				using (IItemLock<IImageProvider> itemLock = providerCache.LockItem(comicGuid, CreateProvider))
				{
					using (IItemLock<ThumbnailImage> itemLock2 = ThumbPool.GetThumbnail(comicBook.GetThumbnailKey(index), itemLock.Item, onErrorThrowException: true))
					{
						int thumbnailQuality = Config.ThumbnailQuality;
						byte[] array = ((thumbnailQuality != 100) ? new ThumbnailImage(itemLock2.Item.Bitmap.ImageToJpegBytes(75 * thumbnailQuality / 100), itemLock2.Item.Size, itemLock2.Item.OriginalSize).ToBytes() : itemLock2.Item.ToBytes());
						AddStats(ServerStatistics.StatisticType.ThumbnailRequest, array.Length);
						return array;
					}
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		void IRemoteComicLibrary.UpdateComic(Guid comicGuid, string propertyName, object value)
		{
			if (Config.IsEditable)
			{
				try
				{
					ComicLibrary.Books[comicGuid]?.SetValue(propertyName, value);
				}
				catch (Exception)
				{
				}
			}
		}

		public string GetAnnouncementUri()
		{
			if (!Config.IsInternet)
			{
				return null;
			}
			return ServiceAddress.CompletePortAndPath(GetExternalServiceAddress(), (Config.ServicePort == ComicLibraryServerConfig.DefaultPublicServicePort) ? null : Config.ServicePort.ToString(), (Config.ServiceName == ComicLibraryServerConfig.DefaultServiceName) ? null : Config.ServiceName);
		}

		public void AnnounceServer()
		{
			string uri = GetAnnouncementUri();
			if (string.IsNullOrEmpty(uri) || serverValidationFailed)
			{
				return;
			}
			try
			{
				ThreadUtility.RunInBackground("Announce Server", delegate
				{
					if (!serverHasBeenValidated)
					{
						serverHasBeenValidated = true;
						serverValidationFailed = ComicLibraryClient.GetServerId(uri) != Id;
						if (serverValidationFailed)
						{
							return;
						}
					}
					try
					{
						//serverRegistration.Register(uri, Config.Name, Config.Description ?? string.Empty, (int)Config.Options, Config.PrivateListPassword);
						//serverHasBeenAnnounced = true;
					}
					catch (Exception)
					{
					}
				});
			}
			catch (Exception)
			{
			}
		}

		public void AnnouncedServerRefresh()
		{
			AnnounceServer();
		}

		public void AnnouncedServerRemove()
		{
			if (!serverHasBeenAnnounced)
			{
				return;
			}
			string announcementUri = GetAnnouncementUri();
			try
			{
				//serverRegistration.Unregister(announcementUri);
			}
			catch (Exception)
			{
			}
			finally
			{
				serverHasBeenAnnounced = false;
			}
		}

		public void CheckPrivateNetwork()
		{
			if (!Config.OnlyPrivateConnections || GetClientIp().IsPrivate())
			{
				return;
			}
			throw new AuthenticationException("Only clients in private network can connect");
		}

		public void AddStats(ServerStatistics.StatisticType type, int size = 0)
		{
			Statistics.Add(GetClientIp().ToString(), type, size);
		}

		public bool Start()
		{
			try
			{
				if (IsRunning)
				{
					Stop();
				}
				if (!Config.IsValidShare)
				{
					return false;
				}
				string uriString = $"net.tcp://localhost:{Config.ServicePort}/{Config.ServiceName}";
				serviceHost = new ServiceHost(this, new Uri(uriString));
				serviceHost.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom;
				serviceHost.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new PasswordValidator(Config.ProtectionPassword);
				serviceHost.Credentials.ServiceCertificate.Certificate = new X509Certificate2(Certificate);// New Cert (sha256)
                serviceHost.Credentials.IssuedTokenAuthentication.KnownCertificates.Add(new X509Certificate2(Certificate));// New Cert (sha256)
				serviceHost.Credentials.IssuedTokenAuthentication.KnownCertificates.Add(new X509Certificate2(Resources.Certificate, string.Empty));//Old Cert (md5)
                serviceHost.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None;
				ServiceEndpoint serviceEndpoint = serviceHost.AddServiceEndpoint(typeof(IRemoteServerInfo), CreateChannel(secure: false), InfoPoint);
				serviceEndpoint.Binding.SendTimeout = TimeSpan.FromSeconds(EngineConfiguration.Default.OperationTimeout);
				serviceEndpoint = serviceHost.AddServiceEndpoint(typeof(IRemoteComicLibrary), CreateChannel(secure: true), LibraryPoint);
				serviceEndpoint.Binding.SendTimeout = TimeSpan.FromSeconds(EngineConfiguration.Default.OperationTimeout);
				serviceHost.Open();
				if (Config.IsInternet)
				{
					AnnounceServer();
					announceTimer = new Timer(ServerAnnounce, null, 300000, 300000);
				}
				else
				{
					if (Broadcaster != null)
					{
						Broadcaster.Broadcast(new BroadcastData(BroadcastType.ServerStarted, Config.ServiceName, Config.ServicePort));
					}
					pingTimer = new Timer(ServerPing, null, 10000, 10000);
				}
				return true;
			}
			catch (Exception)
			{
				Stop();
				return false;
			}
		}

		public void Stop()
		{
			if (!IsRunning)
			{
				return;
			}
			AnnouncedServerRemove();
			try
			{
				announceTimer.SafeDispose();
				announceTimer = null;
				pingTimer.SafeDispose();
				pingTimer = null;
				if (Broadcaster != null)
				{
					Broadcaster.Broadcast(new BroadcastData(BroadcastType.ServerStopped, Config.ServiceName, Config.ServicePort));
				}
				serviceHost.Close();
			}
			catch
			{
			}
			finally
			{
				serviceHost = null;
			}
		}

		private void ServerPing(object state)
		{
			if (PingEnabled && Broadcaster != null)
			{
				Broadcaster.Broadcast(new BroadcastData(BroadcastType.ServerStarted, Config.ServiceName, Config.ServicePort));
			}
		}

		private void ServerAnnounce(object state)
		{
			AnnouncedServerRefresh();
		}

		private ComicLibrary GetSharedComicLibrary()
		{
			switch (Config.LibraryShareMode)
			{
			case LibraryShareMode.Selected:
			{
				ComicLibrary comicLibrary = new ComicLibrary
				{
					Name = ComicLibrary.Name,
					Id = ComicLibrary.Id
				};
				HashSet<ComicBook> hashSet = new HashSet<ComicBook>();
				IEnumerable<ShareableComicListItem> source = from scli in ComicLibrary.ComicLists.GetItems<ShareableComicListItem>()
					where Config.SharedItems.Contains(scli.Id)
					select scli;
				comicLibrary.ComicLists.AddRange(source.Select((ShareableComicListItem scli) => new ComicIdListItem(scli)));
				hashSet.AddRange(source.SelectMany((ShareableComicListItem scli) => scli.GetBooks()));
				comicLibrary.Books.AddRange(hashSet.Select((ComicBook cb) => new ComicBook(cb)));
				return comicLibrary;
			}
			case LibraryShareMode.All:
				return ComicLibrary.Attach(ComicLibrary);
			default:
				return new ComicLibrary();
			}
		}

		private void providerCache_ItemRemoved(object sender, CacheItemEventArgs<Guid, IImageProvider> e)
		{
			e.Item.Dispose();
		}

		private IImageProvider CreateProvider(Guid comicGuid)
		{
			ComicBook comicBook = ComicLibrary.Books[comicGuid];
			return comicBook.OpenProvider();
		}

		public static Binding CreateChannel(bool secure)
		{
			NetTcpBinding netTcpBinding = new NetTcpBinding();
			netTcpBinding.Security.Mode = (secure ? SecurityMode.Message : SecurityMode.None);
			if (secure)
			{
				netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
			}
			netTcpBinding.MaxReceivedMessageSize = 100000000L;
			netTcpBinding.ReaderQuotas.MaxArrayLength = 100000000;
			return netTcpBinding;
		}

		public static IEnumerable<ShareInformation> GetPublicServers(ServerOptions optionsMask, string password)
		{
			//ServerInfo[] source = HttpAccess.CallSoap(serverRegistration, (ServerRegistration s) => s.GetList((int)optionsMask, password));
			//return ((IEnumerable<ServerInfo>)source).Select((Func<ServerInfo, ShareInformation>)((ServerInfo s) => s));
			return Enumerable.Empty<ShareInformation>();
		}

		public static string GetExternalServiceAddress()
		{
			string text = ExternalServerAddress ?? string.Empty;
			text = text.Trim();
			try
			{
				if (string.IsNullOrEmpty(text))
				{
					return ServiceAddress.GetWanAddress();
				}
				return text;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static IEnumerable<ComicLibraryServer> Start(IEnumerable<ComicLibraryServerConfig> servers, int port, Func<ComicLibrary> getComicLibrary, IPagePool pagePool, IThumbnailPool thumbPool, IBroadcast<BroadcastData> broadcaster)
		{
			foreach (ComicLibraryServerConfig item in servers.Where((ComicLibraryServerConfig c) => c.IsValidShare))
			{
				int freeShareNumber = GetFreeShareNumber(port);
				item.ServicePort = port;
				item.ServiceName = "Share" + ((freeShareNumber > 0) ? (freeShareNumber + 1).ToString() : string.Empty);
				ComicLibraryServer comicLibraryServer = new ComicLibraryServer(item, getComicLibrary, pagePool, thumbPool, broadcaster);
				if (comicLibraryServer.Start())
				{
					yield return comicLibraryServer;
				}
			}
		}

		private static int GetFreeShareNumber(int port)
		{
			if (!shareCounts.TryGetValue(port, out var value))
			{
				value = -1;
			}
			return shareCounts[port] = value + 1;
		}

		public static IPAddress GetClientIp()
		{
			OperationContext current = OperationContext.Current;
			MessageProperties incomingMessageProperties = current.IncomingMessageProperties;
			RemoteEndpointMessageProperty remoteEndpointMessageProperty = incomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
			if (!IPAddress.TryParse(remoteEndpointMessageProperty.Address, out var address))
			{
				return null;
			}
			return address;
		}
	}
}
