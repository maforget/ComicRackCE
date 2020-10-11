using System.Linq;
using System.Xml.Serialization;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Plugins
{
	[XmlInclude(typeof(PythonCommand))]
	public class CommandCollection : SmartList<Command>
	{
		public Command this[string key] => this.FirstOrDefault((Command cmd) => cmd.Key == key);
	}
}
