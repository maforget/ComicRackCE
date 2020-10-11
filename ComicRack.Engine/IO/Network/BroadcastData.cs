using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	public class BroadcastData
	{
		[DefaultValue(null)]
		public string ServerName
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public int ServerPort
		{
			get;
			set;
		}

		[DefaultValue(BroadcastType.ServerStarted)]
		public BroadcastType BroadcastType
		{
			get;
			set;
		}

		public BroadcastData()
			: this(BroadcastType.ServerStarted)
		{
		}

		public BroadcastData(BroadcastType broadcastType, string serverName = null, int serverPort = 0)
		{
			BroadcastType = broadcastType;
			ServerName = serverName;
			ServerPort = serverPort;
		}
	}
}
