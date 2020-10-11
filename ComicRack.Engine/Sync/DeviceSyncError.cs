namespace cYo.Projects.ComicRack.Engine.Sync
{
	public class DeviceSyncError
	{
		public string Name
		{
			get;
			private set;
		}

		public string Message
		{
			get;
			private set;
		}

		public DeviceSyncError()
		{
		}

		public DeviceSyncError(string name, string message)
		{
			Name = name;
			Message = message;
		}
	}
}
