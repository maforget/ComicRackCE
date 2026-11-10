using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace cYo.Projects.ComicRack.Plugins
{
	internal sealed class DotNetPluginInitializer : PluginInitializer
	{
		// COR_E_NOTSUPPORTED: .NET Framework refuses LoadFrom of a file that carries the internet zone mark.
		private const int BlockedByZoneHResult = unchecked((int)0x80131515);

		private static readonly string pluginApiName = typeof(DotNetPlugin).Assembly.GetName().Name;

		public override IEnumerable<Command> GetCommands(string file) => GetCommands(file, null);

		public override IEnumerable<Command> GetCommands(string file, IPluginEnvironment env)
		{
			if (!".dll".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase) || !ReferencesPluginApi(file))
				return Enumerable.Empty<Command>();

			try
			{
				Assembly assembly = Assembly.LoadFrom(file);
				PluginAssemblyResolver.Register(Path.GetDirectoryName(file), env?.LibraryPaths);
				if (!string.Equals(Path.GetFullPath(assembly.Location), Path.GetFullPath(file), StringComparison.OrdinalIgnoreCase))
				{
					Command.LogDefault($"Skipped .NET plugin '{file}': an assembly with the same identity is already loaded from '{assembly.Location}'");
					return Enumerable.Empty<Command>();
				}
				return DotNetPluginLoader.CreateCommands(assembly, file);
			}
			catch (FileLoadException e) when (e.HResult == BlockedByZoneHResult)
			{
				Command.LogDefault($"Windows blocked the .NET plugin '{file}' because it came from the internet. Open the Properties of the file and select Unblock, then restart ComicRack.");
				return Enumerable.Empty<Command>();
			}
			catch (Exception e)
			{
				Command.LogDefault($"Failed to load .NET plugin '{file}': {e.Message}");
				return Enumerable.Empty<Command>();
			}
		}

		// A script folder also holds the libraries of plugins. Only a DLL that references the plugin API can contain a plugin, and only such a DLL gets loaded for execution.
		private static bool ReferencesPluginApi(string file)
		{
			try
			{
				AssemblyName name = AssemblyName.GetAssemblyName(file);
				// A second ReflectionOnlyLoadFrom of an identity already loaded from another path throws, so reuse the first one.
				Assembly assembly = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(a => a.FullName == name.FullName) ?? Assembly.ReflectionOnlyLoadFrom(file);
				if (assembly.GetReferencedAssemblies().Any(a => a.Name == pluginApiName))
					return true;

				PluginAssemblyResolver.NoteLibrary(file, name);
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}