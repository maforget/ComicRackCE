using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using cYo.Common.Collections;

namespace cYo.Common.Win32.PortableDevices
{
	public class DeviceFolder : DeviceItem
	{
		public IEnumerable<DeviceItem> Items => base.Device.EnumerateItems(this).CatchExceptions(ex => 
			Debug.WriteLine($"Error enumerating ItemPath: {this.ItemPath} - {ex.Message}"));

		public bool IsRoot
		{
			get;
			private set;
		}

		public long FreeSpace => base.Device.GetFreeSpace(this);

		public DeviceStorageFolder StorageParent
		{
			get
			{
				for (DeviceFolder deviceFolder = this; deviceFolder != null; deviceFolder = deviceFolder.Parent)
				{
					if (deviceFolder is DeviceStorageFolder)
					{
						return (DeviceStorageFolder)deviceFolder;
					}
				}
				return null;
			}
		}

		public DeviceFolder(DeviceFolder parent, string id, string name)
			: base(parent, id, name)
		{
		}

		public DeviceFolder(Device device)
			: base(null, "DEVICE", "DEVICE")
		{
			base.Device = device;
			IsRoot = true;
		}

		public DeviceFolder GetFolder(string relativePath)
		{
			return base.Device.GetFolder(CombinePath(relativePath));
		}

		public DeviceFile GetFile(string relativePath)
		{
			return base.Device.GetFile(CombinePath(relativePath));
		}

		public bool FileExists(string relativePath)
		{
			return base.Device.FileExists(CombinePath(relativePath));
		}

		public DeviceFile WriteFile(string fileName, Stream inStream)
		{
			return base.Device.WriteFile(this, fileName, inStream);
		}

		public DeviceFolder CreateFolder(string folderName)
		{
			return base.Device.CreateFolder(this, folderName);
		}

		public string CombinePath(string relativePath)
		{
			return CombinePath(base.ItemPath, relativePath);
		}

		public DeviceItem Find(string regEx, int maxLevel = -1)
		{
			Regex rx = new Regex(regEx, RegexOptions.IgnoreCase);
			return Items.Recurse<DeviceItem>((object item) => (!(item is DeviceFolder)) ? null : ((DeviceFolder)item).Items, bottomUp: false, maxLevel).FirstOrDefault((DeviceItem item) => rx.IsMatch(item.Name));
		}

		public static string CombinePath(string pathAbsolute, string pathRelative)
		{
			return Path.Combine(pathAbsolute, pathRelative);
		}
	}
}
