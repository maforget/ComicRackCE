using cYo.Common.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace cYo.Projects.ComicRack.Plugins
{
	public class DotNetPluginInitializer : PluginInitializer
	{
		private const string Method = "Execute";

		public override IEnumerable<Command> GetCommands(string file)
		{
			List<Command> list = new List<Command>();
			try
			{
				// TODO: Add support for compiling .cs files on the fly and loading them as plugins. For now, we only support precompiled .dll files.
				if (!".dll".Equals(Path.GetExtension(file), StringComparison.OrdinalIgnoreCase))
					return list.ToArray();

				// Load .NET commands that implement the DotNetPlugin class. The class must also implement an IDotNetCommand interface that corresponds to it's HookType.
				var assembly = Assembly.LoadFrom(file);
				assembly?.GetTypes().Where(t => t.IsSubclassOf(typeof(DotNetPlugin))).ForEach(t =>
				{
					DotNetPlugin plugin = (DotNetPlugin)Activator.CreateInstance(t);
					IEnumerable<Type> hooks = plugin.GetType().GetInterfaces()
						.Where(i => i != typeof(IDotNetCommand) && typeof(IDotNetCommand).IsAssignableFrom(i));

					foreach (Type interfaceType in hooks)
					{
						IDotNetHook hook = DotNetHookRegistry.GetHookFromType(interfaceType);
						var executeMethod = interfaceType.GetMethod(Method);

						if (hook == null || executeMethod == null)
							continue;

						list.Add(new DotNetCommand
						{
							Key = plugin.Key,
							Name = plugin.Name,
							ScriptFile = Path.GetFileName(file),
							Description = plugin.Description,
							Image = plugin.Image,
							Enabled = plugin.Enabled,
							PCount = interfaceType.GetProperty("PCount")?.GetValue(plugin) as int? ?? 0,
							Hook = hook.Hook,
							ScriptHandler = args => executeMethod.Invoke(plugin, args),
							Plugin = plugin
						});

					}
				});
				return list;
			}
			catch (Exception)
			{
				return list;
			}
		}
	}
}