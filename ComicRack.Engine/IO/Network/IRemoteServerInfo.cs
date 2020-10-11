using System.ServiceModel;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[ServiceContract]
	public interface IRemoteServerInfo
	{
		string Id
		{
			[OperationContract]
			get;
		}

		string Name
		{
			[OperationContract]
			get;
		}

		string Description
		{
			[OperationContract]
			get;
		}

		ServerOptions Options
		{
			[OperationContract]
			get;
		}
	}
}
