using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace cYo.Projects.ComicRack.Plugins
{
	// Finds the third-party assemblies of .NET plugins. A plugin ships them in its own folder, by convention in "lib".
	//
	// The runtime already finds an assembly that lies next to the DLL that asks for it. This resolver adds the other
	// directories of the plugin, and it serves source plugins, whose compiled assembly lies in the cache folder.
	//
	// The runtime limits what plugins can ship. It loads an assembly without a strong name once per name: when two
	// plugins ship different versions, both use the one that was needed first, and the runtime does not ask this
	// resolver again. NoteLibrary warns about that. Versions of a strong-named assembly load side by side.
	internal static class PluginAssemblyResolver
	{
		private static readonly object gate = new object();

		private static readonly Dictionary<string, HashSet<string>> directoriesByPluginFolder = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

		private static readonly Dictionary<Assembly, string> pluginFolderByAssembly = new Dictionary<Assembly, string>();

		private static readonly Dictionary<string, KeyValuePair<Version, string>> librariesByName = new Dictionary<string, KeyValuePair<Version, string>>(StringComparer.OrdinalIgnoreCase);

		private static bool installed;

		public static void Register(string pluginFolder, IEnumerable<string> moreDirectories = null)
		{
			pluginFolder = Path.GetFullPath(pluginFolder);
			lock (gate)
			{
				if (!directoriesByPluginFolder.TryGetValue(pluginFolder, out HashSet<string> directories))
				{
					directories = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { pluginFolder };
					directoriesByPluginFolder.Add(pluginFolder, directories);
				}
				directories.UnionWith((moreDirectories ?? Enumerable.Empty<string>()).Select(Path.GetFullPath));
				if (!installed)
				{
					AppDomain.CurrentDomain.AssemblyResolve += Resolve;
					installed = true;
				}
			}
		}

		// The assembly of a source plugin lies in the cache folder, so its location does not tell which plugin it belongs to.
		public static void Register(Assembly assembly, string pluginFolder)
		{
			lock (gate)
			{
				pluginFolderByAssembly[assembly] = Path.GetFullPath(pluginFolder);
			}
		}

		// Called for every library that plugins ship, so two versions of one name are reported before one of them silently wins.
		public static void NoteLibrary(string file, AssemblyName name)
		{
			if (name.GetPublicKeyToken()?.Length > 0)
			{
				return;
			}
			lock (gate)
			{
				if (!librariesByName.TryGetValue(name.Name, out KeyValuePair<Version, string> first))
				{
					librariesByName.Add(name.Name, new KeyValuePair<Version, string>(name.Version, file));
				}
				else if (first.Key != name.Version)
				{
					Command.LogDefault("Warning: plugins ship different versions of the assembly '{0}': '{1}' (version {2}) and '{3}' (version {4}). The assembly has no strong name, so only the one that is needed first is loaded and every plugin uses it.", name.Name, first.Value, first.Key, file, name.Version);
				}
			}
		}

		private static Assembly Resolve(object sender, ResolveEventArgs args)
		{
			try
			{
				AssemblyName requested = new AssemblyName(args.Name);
				// The runtime asks for satellite and serializer assemblies all the time; plugins do not ship them in this layout.
				if (requested.Name.EndsWith(".resources", StringComparison.OrdinalIgnoreCase) || requested.Name.EndsWith(".XmlSerializers", StringComparison.OrdinalIgnoreCase))
				{
					return null;
				}
				var candidates = ProbeDirectories(args.RequestingAssembly)
					.Select(directory => Path.Combine(directory, requested.Name + ".dll"))
					.Where(File.Exists)
					.Select(file => new { File = file, Name = ReadName(file) })
					.Where(c => c.Name != null && string.Equals(c.Name.Name, requested.Name, StringComparison.OrdinalIgnoreCase))
					.ToList();
				var best = candidates.FirstOrDefault(c => c.Name.Version == requested.Version) ?? candidates.OrderByDescending(c => c.Name.Version).FirstOrDefault();
				if (best == null)
				{
					return null;
				}
				Assembly assembly = Assembly.LoadFrom(best.File);
				string requester = args.RequestingAssembly?.GetName().Name ?? "a plugin";
				if (requested.Version != null && best.Name.Version != requested.Version)
				{
					Command.LogDefault("Resolved '{0}' for {1} to '{2}'. Warning: version {3} was requested, version {4} was found.", requested.Name, requester, assembly.Location, requested.Version, best.Name.Version);
				}
				else
				{
					Command.LogDefault("Resolved '{0}' for {1} to '{2}'", requested.Name, requester, assembly.Location);
				}
				return assembly;
			}
			catch (Exception e)
			{
				// An exception here would surface in unrelated code that happened to trigger the assembly load.
				Command.LogDefault("Failed to resolve '{0}' for a plugin: {1}", args.Name, e.Message);
				return null;
			}
		}

		// A request without a known requester (Assembly.Load by name, Type.GetType, a serializer) can come from any plugin.
		// A request from an assembly outside every plugin is not for this resolver.
		private static List<string> ProbeDirectories(Assembly requester)
		{
			lock (gate)
			{
				if (requester == null)
				{
					return directoriesByPluginFolder.Values.SelectMany(d => d).ToList();
				}
				if (pluginFolderByAssembly.TryGetValue(requester, out string pluginFolder))
				{
					return directoriesByPluginFolder[pluginFolder].ToList();
				}
				string location = requester.IsDynamic || string.IsNullOrEmpty(requester.Location) ? null : Path.GetDirectoryName(requester.Location);
				return directoriesByPluginFolder.Values.Where(d => location != null && d.Contains(location)).SelectMany(d => d).ToList();
			}
		}

		private static AssemblyName ReadName(string file)
		{
			try
			{
				return AssemblyName.GetAssemblyName(file);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
