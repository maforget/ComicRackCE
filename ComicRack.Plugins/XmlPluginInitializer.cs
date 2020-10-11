using System;
using System.Collections.Generic;
using System.IO;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Plugins
{
	public class XmlPluginInitializer : PluginInitializer
	{
		public override IEnumerable<Command> GetCommands(string file)
		{
			try
			{
				if (".xml".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase))
				{
					return XmlUtility.Load<CommandCollection>(file, compressed: false);
				}
			}
			catch (Exception)
			{
			}
			return new Command[0];
		}
	}
}
