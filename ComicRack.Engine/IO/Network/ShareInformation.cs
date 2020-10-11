namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	public class ShareInformation
	{
		public string Id
		{
			get;
			set;
		}

		public string Uri
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Comment
		{
			get;
			set;
		}

		public ServerOptions Options
		{
			get;
			set;
		}

		public bool IsLocal
		{
			get;
			set;
		}

		public bool IsProtected => (Options & ServerOptions.ShareNeedsPassword) != 0;

		public bool IsEditable => (Options & ServerOptions.ShareIsEditable) != 0;

		public bool IsExportable => (Options & ServerOptions.ShareIsExportable) != 0;

		public static implicit operator ServerInfo(ShareInformation info)
		{
			return new ServerInfo
			{
				Name = info.Name,
				Comment = info.Comment,
				Uri = info.Uri,
				Options = (int)info.Options
			};
		}

		public static implicit operator ShareInformation(ServerInfo info)
		{
			return new ShareInformation
			{
				Name = info.Name,
				Comment = info.Comment,
				Uri = info.Uri,
				Options = (ServerOptions)info.Options
			};
		}
	}
}
