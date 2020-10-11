using System;

namespace cYo.Common.ComponentModel
{
	[Serializable]
	public class LiteComponent : DisposableObject, ILiteComponent, IDisposable
	{
		[field: NonSerialized]
		public event EventHandler<ServiceRequestEventArgs> ServiceRequest;

		public virtual T QueryService<T>() where T : class
		{
			ServiceRequestEventArgs serviceRequestEventArgs = new ServiceRequestEventArgs(typeof(T), this as T);
			OnServiceRequest(serviceRequestEventArgs);
			return serviceRequestEventArgs.Service as T;
		}

		protected virtual void OnServiceRequest(ServiceRequestEventArgs e)
		{
			if (this.ServiceRequest != null)
			{
				this.ServiceRequest(this, e);
			}
		}
	}
}
