using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace cYo.Projects.ComicRack.Plugins
{
	// The one place that turns a plugin assembly into commands. Compiled plugins and source plugins both end here.
	internal static class DotNetPluginLoader
	{
		// pluginFile is the file that stands for the plugin on disk: the DLL of a compiled plugin, the .csplugin file of a source plugin.
		public static IEnumerable<Command> CreateCommands(Assembly assembly, string pluginFile)
		{
			var commands = new List<Command>();
			foreach (Type type in GetLoadableTypes(assembly, pluginFile).Where(IsPluginClass))
			{
				try
				{
					DotNetPlugin plugin = (DotNetPlugin)Activator.CreateInstance(type);
					if (string.IsNullOrEmpty(plugin.Key))
					{
						Command.LogDefault($"Skipped .NET plugin class '{type.FullName}' in '{pluginFile}': the Key is empty");
						continue;
					}
					commands.AddRange(DotNetCommand.CreateAll(plugin, pluginFile));
				}
				catch (Exception e)
				{
					Command.LogDefault($"Skipped .NET plugin class '{type.FullName}' in '{pluginFile}': {((e as TargetInvocationException)?.InnerException ?? e).Message}");
				}
			}
			return commands;
		}


		public static PluginKind KindOf(string pluginFile) => CSharpSourcePluginInitializer.IsMarker(pluginFile) ? PluginKind.CSharpSource : PluginKind.CompiledDotNet;

		// Abstract and open generic classes are base classes for other plugins, not plugins.
		private static bool IsPluginClass(Type type)
		{
			return typeof(DotNetPlugin).IsAssignableFrom(type) && !type.IsAbstract && !type.ContainsGenericParameters;
		}

		private static IEnumerable<Type> GetLoadableTypes(Assembly assembly, string pluginFile)
		{
			try
			{
				return assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				foreach (string message in e.LoaderExceptions.Select(le => le.Message).Distinct())
				{
					Command.LogDefault($"Some types in .NET plugin '{pluginFile}' could not be loaded: {message}");
				}
				return e.Types.Where(type => type != null);
			}
		}
	}
}
