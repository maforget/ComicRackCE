using System.IO;

namespace cYo.Common.Win32.PortableDevices
{
	public class DeviceFile : DeviceItem
	{
		public long Size
		{
			get;
			set;
		}

		public DeviceFile(DeviceFolder parent, string id, string name, long size)
			: base(parent, id, name)
		{
			Size = size;
		}

		public Stream ReadFile()
		{
			return base.Device.ReadFile(this);
		}
	}
}
