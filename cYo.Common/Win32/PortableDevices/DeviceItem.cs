using System.Text;

namespace cYo.Common.Win32.PortableDevices
{
	public abstract class DeviceItem
	{
		public DeviceFolder Parent
		{
			get;
			private set;
		}

		public string Id
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public Device Device
		{
			get;
			protected set;
		}

		public string ItemPath
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder("\\" + Name);
				for (DeviceFolder parent = Parent; parent != null; parent = parent.Parent)
				{
					if (!parent.IsRoot)
					{
						stringBuilder.Insert(0, parent.Name);
						stringBuilder.Insert(0, "\\");
					}
				}
				return stringBuilder.ToString();
			}
		}

		protected DeviceItem(DeviceFolder parent, string id, string name)
		{
			Parent = parent;
			Id = id;
			Name = name;
			if (Parent != null)
			{
				Device = Parent.Device;
			}
		}

		public void Delete()
		{
			Device.Delete(this);
		}

		public override string ToString()
		{
			return ItemPath;
		}
	}
}
