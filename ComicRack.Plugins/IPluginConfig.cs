using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Plugins
{
	public interface IPluginConfig
	{
		IEnumerable<string> LibraryPaths
		{
			get;
		}
	}
}
