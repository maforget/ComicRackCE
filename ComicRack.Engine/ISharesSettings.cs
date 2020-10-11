using cYo.Common.Collections;
using cYo.Projects.ComicRack.Engine.IO.Network;

namespace cYo.Projects.ComicRack.Engine
{
	public interface ISharesSettings
	{
		bool LookForShared
		{
			get;
			set;
		}

		SmartList<ComicLibraryServerConfig> Shares
		{
			get;
		}

		string PrivateListingPassword
		{
			get;
		}

		string ExternalServerAddress
		{
			get;
		}
	}
}
