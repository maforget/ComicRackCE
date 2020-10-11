using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Plugins
{
	public abstract class PluginInitializer
	{
		public abstract IEnumerable<Command> GetCommands(string file);
	}
}
