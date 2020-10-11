using System;

namespace cYo.Common.ComponentModel
{
	public interface ILiteComponent : IDisposable
	{
		event EventHandler<ServiceRequestEventArgs> ServiceRequest;

		T QueryService<T>() where T : class;
	}
}
