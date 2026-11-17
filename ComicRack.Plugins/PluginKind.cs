using System.Collections.Generic;
using System.Linq;

namespace cYo.Projects.ComicRack.Plugins
{
	/// <summary>How a plugin is written and shipped.</summary>
	public enum PluginKind
	{
		/// <summary>A Python script, whether a .py file or a command defined in an XML file.</summary>
		Python,

		/// <summary>A folder with a .csplugin file that ComicRack compiles.</summary>
		CSharpSource,

		/// <summary>A .NET DLL that the author built.</summary>
		CompiledDotNet
	}
}
