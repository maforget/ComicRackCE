using System;
using System.Collections.Generic;
using System.Linq;

namespace cYo.Common.Win32.PortableDevices
{
	public static class DeviceFactory
	{
		public static IEnumerable<Device> GetDevices()
		{
			try
			{
				PortableDeviceApi.IPortableDeviceManager portableDeviceManager = (PortableDeviceApi.IPortableDeviceManager)new PortableDeviceApi.PortableDeviceManager();
				uint pcPnPDeviceIDs = 0u;
				portableDeviceManager.GetDevices(null, ref pcPnPDeviceIDs);
				string[] array = new string[pcPnPDeviceIDs];
				portableDeviceManager.GetDevices(array, ref pcPnPDeviceIDs);
				return array.Select((string t) => new Device(t));
			}
			catch (Exception)
			{
				return Enumerable.Empty<Device>();
			}
		}

		public static Device GetDevice(string deviceKey)
		{
			return GetDevices().FirstOrDefault((Device d) => d.Key == deviceKey);
		}
	}
}
