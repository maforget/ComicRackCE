using System;

namespace cYo.Common.ComponentModel
{
	public class ServiceRequestEventArgs : EventArgs
	{
		private readonly Type serviceType;

		public Type ServiceType => serviceType;

		public object Service
		{
			get;
			set;
		}

		public ServiceRequestEventArgs(Type serviceType, object service)
		{
			this.serviceType = serviceType;
			Service = service;
		}
	}
}
