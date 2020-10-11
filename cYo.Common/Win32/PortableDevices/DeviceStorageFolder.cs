namespace cYo.Common.Win32.PortableDevices
{
	public class DeviceStorageFolder : DeviceFolder
	{
		public string FileSystem
		{
			get;
			private set;
		}

		public DeviceStorageFolder(DeviceFolder parent, string id, string name, string fileSystem)
			: base(parent, id, name)
		{
			FileSystem = fileSystem;
		}
	}
}
