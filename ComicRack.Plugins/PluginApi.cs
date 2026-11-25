using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace cYo.Projects.ComicRack.Plugins
{
	/// <summary>
	/// The version of the plugin API: the attributes and interfaces in this assembly that plugins use.
	/// The version of the assembly changes with every build, so it cannot tell a plugin whether it fits. This number can.
	/// </summary>
	/// <remarks>
	/// ComicRack raises <see cref="Version"/> when a change breaks existing plugins, and raises <see cref="MinimumVersion"/> when it stops supporting old plugins.
	/// A plugin declares the version it was written for with <c>[assembly: PluginApiVersion(1)]</c>.
	/// </remarks>
	public static class PluginApi
	{
		/// <summary>The newest version of the plugin API that this ComicRack provides.</summary>
		public const int Version = 1;

		/// <summary>The oldest version of the plugin API that this ComicRack still supports.</summary>
		public const int MinimumVersion = 1;

		/// <summary>The version of a plugin that does not declare one.</summary>
		internal const int UndeclaredVersion = 1;
	}

	/// <summary>
	/// Declares the version of the plugin API (<see cref="PluginApi.Version"/>) that the plugin was written for. Put it in the code of the plugin:
	/// <code>[assembly: PluginApiVersion(1)]</code>
	/// </summary>
	/// <remarks>
	/// ComicRack skips a plugin that declares a version above <see cref="PluginApi.Version"/> or below <see cref="PluginApi.MinimumVersion"/>,
	/// and writes both versions to the script console. It reads the attribute without running any code of the plugin.
	/// A plugin without this attribute counts as version 1, so plugins that were written before the attribute existed still load.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public sealed class PluginApiVersionAttribute : Attribute
	{
		public PluginApiVersionAttribute(int version)
		{
			Version = version;
		}

		public int Version { get; }
	}

	// Decides from the metadata of a plugin assembly whether this ComicRack can run it. The assembly is loaded for reflection only.
	internal static class PluginApiCheck
	{
		private static readonly AssemblyName hostApi = typeof(PluginApiVersionAttribute).Assembly.GetName();

		static PluginApiCheck()
		{
			// Reading an attribute of the plugin resolves its type in the plugin API. The plugin may have been built against another build of it.
			AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, args) =>
				new AssemblyName(args.Name).Name == hostApi.Name ? LoadReflectionOnly(typeof(PluginApiVersionAttribute).Assembly.Location) : null;
		}

		// A second ReflectionOnlyLoadFrom of an identity already loaded from another path throws, so reuse the first one.
		public static Assembly LoadReflectionOnly(string file)
		{
			AssemblyName name = AssemblyName.GetAssemblyName(file);
			return AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies().FirstOrDefault(a => a.FullName == name.FullName) ?? Assembly.ReflectionOnlyLoadFrom(file);
		}

		// Returns null when the plugin fits, otherwise the reason for the script console.
		public static string FindProblem(Assembly reflectionOnlyAssembly)
		{
			int declared = ReadDeclaredVersion(reflectionOnlyAssembly);
			if (declared >= PluginApi.MinimumVersion && declared <= PluginApi.Version)
			{
				return null;
			}
			return $"it was written for plugin API version {declared}, but this ComicRack supports versions {PluginApi.MinimumVersion} to {PluginApi.Version}";
		}

		private static int ReadDeclaredVersion(Assembly assembly)
		{
			try
			{
				foreach (CustomAttributeData data in CustomAttributeData.GetCustomAttributes(assembly))
				{
					if (data.AttributeType.FullName == typeof(PluginApiVersionAttribute).FullName && data.ConstructorArguments.Count == 1 && data.ConstructorArguments[0].Value is int version)
					{
						return version;
					}
				}
			}
			catch (Exception)
			{
				// Metadata that cannot be read has no declaration, and the load that follows reports what is wrong with the file.
			}
			return PluginApi.UndeclaredVersion;
		}
	}
}
