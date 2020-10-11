using System.ServiceModel;

namespace cYo.Common.Runtime
{
	[ServiceContract]
	internal interface ISingleInstance
	{
		[OperationContract]
		void InvokeLast(string[] args);
	}
}
