using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using cYo.Common.Win32.PortableDevices;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	public class PortableDeviceSyncProvider : SyncProviderBase
	{
		private const int maxFolderLevel = 10;

		private readonly Device device;

		private readonly DeviceFolder syncFolder;

		public PortableDeviceSyncProvider(string deviceNode, string deviceKey = null)
		{
			device = DeviceFactory.GetDevice(deviceNode);
			syncFolder = device.Find(Regex.Escape(MarkerFile), maxFolderLevel).Parent;
			if (syncFolder == null)
			{
				throw new DriveNotFoundException();
			}
			if (!ReadMarkerFile(deviceKey))
			{
				throw new DriveNotFoundException();
			}
		}

		protected override void OnStart()
		{
		}

		protected override void OnCompleted()
		{
		}

		protected override bool FileExists(string file)
		{
			return syncFolder.FileExists(file);
		}

		protected override IEnumerable<string> GetFileList()
		{
			return from item in syncFolder.Items.OfType<DeviceFile>()
				select item.Name;
		}

		protected override void WriteFile(string fileName, Stream data)
		{
			syncFolder.WriteFile(fileName, data);
		}

		protected override Stream ReadFile(string fileName)
		{
			return syncFolder.GetFile(fileName).ReadFile();
		}

		protected override void DeleteFile(string fileName)
		{
			syncFolder.GetFile(fileName)?.Delete();
		}

		protected override long GetFreeSpace()
		{
			return syncFolder.FreeSpace;
		}
	}
}
