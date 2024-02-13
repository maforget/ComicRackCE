using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading;
using cYo.Common;
using cYo.Common.Text;
using cYo.Common.Threading;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public class WirelessSyncProvider : SyncProviderBase
	{
		public class ClientSyncRequestArgs : EventArgs
		{
			public bool IsPaired
			{
				get;
				set;
			}

			public string Key
			{
				get;
				private set;
			}

			public ClientSyncRequestArgs(string key)
			{
				Key = key;
			}
		}

		private const string AndroidDebugKey = "3082030d308201f5a0030201020204494d03a7300d06092a864886f70d01010b05003037310b30090603550406130255533110300e060355040a1307416e64726f6964311630140603550403130d416e64726f6964204465627567301e170d3135303732323134353830375a170d3435303731343134353830375a3037310b30090603550406130255533110300e060355040a1307416e64726f6964311630140603550403130d416e64726f696420446562756730820122300d06092a864886f70d01010105000382010f003082010a02820101009ebd1f327aa7fd9d5c556df9e09ce4d7f091b04ffe649bf0c286fcd7d2efb24c485f02b4518d08227285d1758f0e6ba44ec3d2dd16a53d34f790452c2b25166db2488ac8de275cbe575325a4f19a476e23cd0831e7b05bb728525500e516bb24b20444ce79ec5625cf5963e4b792f8c5017fc36b880b8b78750bdcace2d4e25aee155aab3a4bb1c1c9a539a73edfc1057d77080e85c9506b033ffc72efe2c418f91171b78899be0d9fb04e5befd57cae955d7a81aff4573362d74571bd84d8ca5502cf99ad3124a6dfe45428b38c50300af28c13006803feadfc3aed84027b5fc4d0350ff41612652368f49b49c0461ff845b9c1e1bea440df4f65805f582b150203010001a321301f301d0603551d0e0416041490edd92d5d54c07225eb32807acdd2bd57a169a4300d06092a864886f70d01010b050003820101000932afee5e88d0b1252c84d1d9b8533c5d57fbd0e4766c53ce0f6565e04fe06c4c23687e109d9e23569736f3ee2105c57630b3b66229d25d910293c3d615e81290e932ecf321cc59a2dbecb4acf89a811cd63f10611b01d546f2aea1a23f259bb7f4e833117396e2e62c28a331d5d9a3fb625e199438635dd65e2da46fd4687aea161e9a490a597e264573be8821e2f3e7826df68dfee333301d968d154636497e76851f838df16d9d428b390b5b19f7b6ddbdbb1e19395f349f169764f38c114a91eaa831195a8e2c6217d1ac385ca4f6301f385fe44bdb82bbf6cd64033cd54e334500cec523e6d5abcc3f6bed5538ad7830ec45a897b1752f3695063e4853";

		private const string AndroidKey = "3082019d30820106a00302010202044e7b1922300d06092a864886f70d010105050030133111300f060355040a130863596f20536f6674301e170d3131303932323131313635305a170d3336303931353131313635305a30133111300f060355040a130863596f20536f667430819f300d06092a864886f70d010101050003818d00308189028181008d81ffb74008a048c517275a464db26461df06a3c85675b6ffa8bea15ec9288eec1ef1bf7616d09b7265bf1c5666473342c2a96ca385769592d73a21595335e5173c69ae5bb7aebd29387e9635ce30bdf11afff71145570b6577799ecac6100bcf0b2c4df6fe34fb8a418b5511c6a56c97b15c544269e91478ee24633ef063090203010001300d06092a864886f70d010105050003818100802cf8770c7af0744f9680b54da88b56eb1d6a48e8d446ce746817fe959991dc1f882323c6015edd4d48f28cfe3a94e30b75c92855b01a8c48354aae8e13a0b949390133c07b09419ac73d0b0b3dc13b2838fe9eae4b171c8022cb47ead602771560277cde7ad61e1a9ce5dee880d0226ed8cc71f36fb376d271a3cb61f1128b";

		private const int CurrentSyncVersion = 1;

		private const int DeviceClientPort = 7614;

		private const int BroadcastListenPort = 7615;

		private const int FirstServerControlPort = 7620;

		private const int CommandListFiles = 0;

		private const int CommandReadFile = 1;

		private const int CommandFreeSpace = 2;

		private const int CommandFileExists = 3;

		private const int CommandDeleteFile = 4;

		private const int CommandWriteFile = 5;

		private const int CommandStart = 6;

		private const int CommandCompleted = 7;

		private const int CommandProgressUpdate = 8;

		private const int CommandInfo = 9;

		private const int CommandReadMultiFile = 10;

		private const int CommandCheckAbort = 11;

		private const int CommandClientPong = 12;

		private const int CommandServerAvailable = 13;

		private readonly IPAddress address;

		private static readonly HashSet<IPAddress> foundDevices = new HashSet<IPAddress>();

		private static UdpClient listener;

		private static IPEndPoint listenerEP;

		private static Socket controlSocket;

		private static int controlPort;

		private static Timer serverAvailableTimer;

		public static event EventHandler<ClientSyncRequestArgs> ClientSyncRequest;

		public WirelessSyncProvider(IPAddress address, string deviceKey = null)
		{
			this.address = address;
			if (!ReadMarkerFile(deviceKey))
			{
				throw new DriveNotFoundException();
			}
		}

		protected override void OnStart()
		{
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandStart);
				SendString(s, "Start Synchronizing");
			});
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandInfo);
				SendInteger(s, CurrentSyncVersion);
				bool licensed = ReadBool(s);
				int versionCode = ReadInteger(s);
				string key = ReadString(s);
				bool flag = ReadBool(s);
				ValidateWifi(versionCode, licensed, key);
			});
		}

		protected override bool OnProgress(int percent)
		{
			Communicate(delegate(Socket s)
			{
				s.Send(new byte[2]
				{
                    CommandProgressUpdate,
					(byte)percent
				});
			});
			bool abort = false;
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandCheckAbort);
				abort = ReadBool(s);
			});
			return !abort;
		}

		protected override void OnCompleted()
		{
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandCompleted);
				SendString(s, "Synchronization completed");
			});
		}

		protected override bool FileExists(string file)
		{
			bool fileExists = false;
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandFileExists);
				SendString(s, file);
				fileExists = ReadBool(s);
			});
			return fileExists;
		}

		protected override void WriteFile(string file, Stream data)
		{
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandWriteFile);
				SendString(s, file);
				SendLong(s, data.Length);
				byte[] array = new byte[100000];
				int size;
				while ((size = data.Read(array, 0, array.Length)) > 0)
				{
					s.Send(array, size, SocketFlags.None);
				}
				if (!ReadBool(s))
				{
					throw new IOException();
				}
			});
		}

		protected override Stream ReadFile(string file)
		{
			MemoryStream ms = null;
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandReadFile);
				SendString(s, file);
				ms = new MemoryStream(ReadSocketData(s));
			});
			return ms;
		}

		protected override void DeleteFile(string file)
		{
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandDeleteFile);
				SendString(s, file);
				ReadBool(s);
			});
		}

		protected override long GetFreeSpace()
		{
			long freeSpace = 0L;
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandFreeSpace);
				freeSpace = ReadLong(s);
			});
			return freeSpace;
		}

		protected override IEnumerable<string> GetFileList()
		{
			string[] fileList = null;
			Communicate(delegate(Socket s)
			{
				SendByte(s, CommandListFiles);
				fileList = ReadString(s).Split("\n", StringSplitOptions.RemoveEmptyEntries).TrimStrings().ToArray();
			});
			return fileList;
		}

		private void ValidateWifi(int versionCode, bool licensed, string key)
		{
			bool flag = licensed && base.Device.Version == versionCode;
			switch (base.Device.Edition)
			{
			case SyncAppEdition.AndroidFull:
				flag &= key == AndroidDebugKey || key == AndroidKey;
					flag = true;
				break;
			default:
				flag = false;
				break;
			case SyncAppEdition.iOS:
				break;
			}
			if (!flag)
			{
				throw new StorageSync.FatalSyncException("Invalid device");
			}
		}

		public override IEnumerable<ComicBook> GetBooks()
		{
			List<string> deleteFiles = new List<string>();
			string[] files = GetFileList().Where(SyncProviderBase.IsValidSyncFile).ToArray();
			Communicate(delegate(Socket socket)
			{
				ComicBookCollection comicBookCollection = new ComicBookCollection();
				SendByte(socket, CommandReadMultiFile);
				SendInteger(socket, files.Length);
				for (int i = 0; i < files.Length; i++)
				{
					SendString(socket, SyncProviderBase.MakeSidecar(files[i]));
				}
				for (int j = 0; j < files.Length; j++)
				{
					Stream inputStream = new MemoryStream(ReadSocketData(socket));
					ComicBook comicBook = DeserializeBook(files[j], inputStream);
					if (comicBook != null && comicBookCollection.FindItemById(comicBook.Id) == null)
					{
						comicBookCollection.Add(comicBook);
					}
					else
					{
						deleteFiles.Add(files[j]);
					}
				}
				base.BooksOnDevice = comicBookCollection;
			});
			foreach (string item in deleteFiles)
			{
				DeleteFile(item);
				DeleteFile(SyncProviderBase.MakeSidecar(item));
			}
			return base.BooksOnDevice;
		}

		public void Communicate(Action<Socket> action, int retry = -1)
		{
			Communicate(address, action, retry);
		}

		public static void Communicate(IPAddress address, Action<Socket> action, int retry = -1)
		{
			int wifiSyncReceiveTimeout = EngineConfiguration.Default.WifiSyncReceiveTimeout;
			int wifiSyncSendTimeout = EngineConfiguration.Default.WifiSyncSendTimeout;
			int wifiSyncConnectionTimeout = EngineConfiguration.Default.WifiSyncConnectionTimeout;
			int wifiSyncConnectionRetries = EngineConfiguration.Default.WifiSyncConnectionRetries;
			if (retry < 0)
			{
				retry = wifiSyncConnectionRetries;
			}
			while (true)
			{
				Socket socket = null;
				try
				{
					socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP)
					{
						ReceiveTimeout = wifiSyncReceiveTimeout,
						SendTimeout = wifiSyncSendTimeout
					};
					IAsyncResult asyncResult = socket.BeginConnect(address, DeviceClientPort, null, null);
					asyncResult.AsyncWaitHandle.WaitOne(wifiSyncConnectionTimeout, exitContext: true);
					if (!socket.Connected)
					{
						socket.Close();
						throw new CommunicationException("Failed to connect to device");
					}
					action(socket);
					return;
				}
				catch (Exception)
				{
					if (--retry < 0)
					{
						throw;
					}
				}
				finally
				{
					socket?.Close();
				}
			}
		}

		public static void SendString(Socket socket, string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			SendInteger(socket, bytes.Length);
			socket.Send(bytes);
		}

		public static byte[] ReadBytes(Socket socket)
		{
			int num = ReadInteger(socket);
			byte[] array = new byte[num];
			ReadBlocking(socket, array);
			return array;
		}

		public static string ReadString(Socket socket)
		{
			return Encoding.UTF8.GetString(ReadBytes(socket));
		}

		public static void SendInteger(Socket socket, int data)
		{
			if (BitConverter.IsLittleEndian)
			{
				data = data.EndianSwap();
			}
			byte[] bytes = BitConverter.GetBytes(data);
			socket.Send(bytes);
		}

		public static void SendLong(Socket socket, long data)
		{
			if (BitConverter.IsLittleEndian)
			{
				data = data.EndianSwap();
			}
			socket.Send(BitConverter.GetBytes(data));
		}

		public static int ReadInteger(Socket socket)
		{
			byte[] array = new byte[4];
			ReadBlocking(socket, array);
			int num = BitConverter.ToInt32(array, 0);
			if (BitConverter.IsLittleEndian)
			{
				num = num.EndianSwap();
			}
			return num;
		}

		public static void SendByte(Socket socket, byte data)
		{
			socket.Send(new byte[1]
			{
				data
			});
		}

		public static bool ReadBool(Socket socket)
		{
			byte[] array = new byte[1];
			ReadBlocking(socket, array);
			return array[0] != 0;
		}

		public static long ReadLong(Socket socket)
		{
			byte[] array = new byte[8];
			ReadBlocking(socket, array);
			long num = BitConverter.ToInt64(array, 0);
			if (BitConverter.IsLittleEndian)
			{
				num = num.EndianSwap();
			}
			return num;
		}

		public static byte[] ReadSocketData(Socket socket)
		{
			int num = (int)ReadLong(socket);
			byte[] array = new byte[num];
			int num2 = 0;
			while (num2 < num)
			{
				int num3 = socket.Receive(array, num2, num - num2, SocketFlags.None);
				if (num3 == -1)
				{
					break;
				}
				if (num3 > 0)
				{
					num2 += num3;
				}
			}
			if (num2 != num)
			{
				throw new IOException();
			}
			return array;
		}

		public static void ReadBlocking(Socket socket, byte[] data, int offset, int length)
		{
			int num = 0;
			try
			{
				int num2;
				while (length > 0 && (num2 = socket.Receive(data, offset + num, length, SocketFlags.None)) != 0)
				{
					num += num2;
					length -= num2;
				}
			}
			catch (Exception inner)
			{
				throw new StorageSync.FatalSyncException("Error during Read", inner);
			}
			if (length != 0)
			{
				throw new StorageSync.FatalSyncException("Wrong length");
			}
		}

		public static void ReadBlocking(Socket socket, byte[] data)
		{
			ReadBlocking(socket, data, 0, data.Length);
		}

		public static void StartListen()
		{
			try
			{
				IPAddress group = IPAddress.Parse("224.34.123.90");
				listener = new UdpClient();
				Socket client = listener.Client;
				listenerEP = new IPEndPoint(IPAddress.Any, BroadcastListenPort);
				client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
				client.Bind(listenerEP);
				client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(group, IPAddress.Any));
				listener.BeginReceive(OnReceivedBroadcastData, null);
			}
			catch (Exception)
			{
			}
			controlPort = FirstServerControlPort;
			controlSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			while (true)
			{
				try
				{
					IPEndPoint localEP = new IPEndPoint(IPAddress.Any, controlPort);
					controlSocket.Bind(localEP);
					controlSocket.Listen(25);
					controlSocket.BeginAccept(OnAcceptControl, null);
				}
				catch (SocketException ex2)
				{
					if (ex2.ErrorCode == 10048)
					{
						controlPort++;
						continue;
					}
				}
				catch (Exception)
				{
				}
				break;
			}
			serverAvailableTimer = new Timer(NotifyDevicesServerAvailable, null, 10000, 10000);
		}

		private static void NotifyDevicesServerAvailable(object state)
		{
			foreach (IPAddress extraWifiDeviceAddress in DeviceSyncFactory.ExtraWifiDeviceAddresses)
			{
				try
				{
					lock (foundDevices)
					{
						if (foundDevices.Contains(extraWifiDeviceAddress))
						{
							continue;
						}
					}
					Communicate(extraWifiDeviceAddress, delegate(Socket s)
					{
						SendByte(s, CommandServerAvailable);
						SendInteger(s, controlPort);
					}, 0);
				}
				catch (Exception)
				{
				}
			}
		}

		public static IEnumerable<IPAddress> GetWirelessDevices()
		{
			using (ItemMonitor.Lock(foundDevices))
			{
				return foundDevices.ToArray();
			}
		}

		private static void OnAcceptControl(IAsyncResult ar)
		{
			try
			{
				using (Socket socket = controlSocket.EndAccept(ar))
				{
					byte[] array = new byte[2048];
					int count = socket.Receive(array);
					IPEndPoint iPEndPoint = socket.RemoteEndPoint as IPEndPoint;
					HandleDeviceMessage(iPEndPoint.Address, array.Take(count).ToArray(), addAddress: false);
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				try
				{
					controlSocket.BeginAccept(OnAcceptControl, null);
				}
				catch (Exception)
				{
				}
			}
		}

		private static void OnReceivedBroadcastData(IAsyncResult ar)
		{
			try
			{
				byte[] bytes = listener.EndReceive(ar, ref listenerEP);
				HandleDeviceMessage(listenerEP.Address, bytes, addAddress: true);
			}
			catch
			{
			}
			finally
			{
				try
				{
					listener.BeginReceive(OnReceivedBroadcastData, null);
				}
				catch (SocketException)
				{
				}
			}
		}

		private static bool HandleDeviceMessage(IPAddress address, byte[] bytes, bool addAddress)
		{
			string @string = Encoding.UTF8.GetString(bytes);
			string[] array = @string.Split(':');
			if (!array[0].StartsWith("ComicRack"))
			{
				return true;
			}
			if (addAddress)
			{
				using (ItemMonitor.Lock(foundDevices))
				{
					foundDevices.Add(address);
				}
			}
			if (array.Length > 1)
			{
				string key = array[1];
				if (array.Length > 2 && array[2] == "Sync")
				{
					WirelessSyncProvider.ClientSyncRequest(address, new ClientSyncRequestArgs(key));
				}
				else
				{
					ClientSyncRequestArgs clientSyncRequestArgs = new ClientSyncRequestArgs(key);
					WirelessSyncProvider.ClientSyncRequest(null, clientSyncRequestArgs);
					if (clientSyncRequestArgs.IsPaired)
					{
						Communicate(address, delegate(Socket s)
						{
							SendByte(s, CommandClientPong);
						}, 0);
					}
				}
			}
			return false;
		}
	}
}
