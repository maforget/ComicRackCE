using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using cYo.Common.Text;
using cYo.Common.Win32.PortableDevices;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public static class DeviceSyncFactory
	{
		private static List<IPAddress> extraWifiDeviceAddresses = new List<IPAddress>();

		public static IEnumerable<IPAddress> ExtraWifiDeviceAddresses => extraWifiDeviceAddresses;

		public static IEnumerable<ISyncProvider> Discover()
		{
			Dictionary<string, ISyncProvider> dictionary = new Dictionary<string, ISyncProvider>();
			try
			{
				foreach (Device device in DeviceFactory.GetDevices())
				{
					try
					{
						PortableDeviceSyncProvider portableDeviceSyncProvider = new PortableDeviceSyncProvider(device.Key);
						dictionary[portableDeviceSyncProvider.Device.Key] = portableDeviceSyncProvider;
					}
					catch (Exception)
					{
					}
				}
			}
			catch (Exception)
			{
			}
			foreach (string removeableDrife in GetRemoveableDrives())
			{
				try
				{
					DiskDriveSyncProvider diskDriveSyncProvider = new DiskDriveSyncProvider(removeableDrife);
					dictionary[diskDriveSyncProvider.Device.Key] = diskDriveSyncProvider;
				}
				catch (Exception)
				{
				}
			}
			foreach (ISyncProvider item in Discover(WirelessSyncProvider.GetWirelessDevices().Concat(ExtraWifiDeviceAddresses)))
			{
				dictionary[item.Device.Key] = item;
			}
			return dictionary.Values;
		}

		public static IEnumerable<ISyncProvider> Discover(IEnumerable<IPAddress> adresses)
		{
			Dictionary<string, ISyncProvider> dictionary = new Dictionary<string, ISyncProvider>();
			foreach (IPAddress item in adresses.Distinct())
			{
				try
				{
					WirelessSyncProvider wirelessSyncProvider = new WirelessSyncProvider(item);
					dictionary[wirelessSyncProvider.Device.Key] = wirelessSyncProvider;
				}
				catch (Exception)
				{
				}
			}
			return dictionary.Values;
		}

		public static ISyncProvider Create(string deviceKey)
		{
			try
			{
				foreach (Device device in DeviceFactory.GetDevices())
				{
					try
					{
						return new PortableDeviceSyncProvider(device.Key, deviceKey);
					}
					catch (Exception)
					{
					}
				}
			}
			catch (Exception)
			{
			}
			foreach (string removeableDrife in GetRemoveableDrives())
			{
				try
				{
					return new DiskDriveSyncProvider(removeableDrife, deviceKey);
				}
				catch (Exception)
				{
				}
			}
			foreach (IPAddress item in WirelessSyncProvider.GetWirelessDevices().Concat(ExtraWifiDeviceAddresses))
			{
				try
				{
					return new WirelessSyncProvider(item, deviceKey);
				}
				catch (Exception)
				{
				}
			}
			return null;
		}

		private static IEnumerable<string> GetRemoveableDrives()
		{
			return from d in DiskDriveSyncProvider.GetRemoveableDrives()
				select d.RootDirectory.FullName;
		}

		public static void SetExtraWifiDeviceAddresses(string addresses)
		{
			extraWifiDeviceAddresses = new List<IPAddress>(ParseWifiAddressList(addresses));
		}

		public static IEnumerable<IPAddress> ParseWifiAddressList(string addresses)
		{
			foreach (string item in addresses.Split(',', ';').TrimStrings().RemoveEmpty()
				.Distinct())
			{
				if (IPAddress.TryParse(item, out var address))
				{
					yield return address;
				}
			}
		}
	}
}
