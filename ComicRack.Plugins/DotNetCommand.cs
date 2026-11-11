using cYo.Common.ComponentModel;
using cYo.Common.Drawing;
using cYo.Common.Text;
using cYo.Common.Windows.Forms.Theme;
using cYo.Projects.ComicRack.Engine;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Plugins
{
	internal sealed class DotNetCommand : Command
	{
		private const string Method = "Execute";
		private readonly MethodInfo execute;
		private readonly string pluginFile;
		public DotNetPlugin Plugin { get; }
		public override PluginKind Kind => DotNetPluginLoader.KindOf(pluginFile);
		private string ConfigFileName => pluginFile + "-" + Key + ".config";

		private DotNetCommand(DotNetPlugin plugin, IDotNetHook hook, string pluginFile)
		{
			Plugin = plugin;
			this.pluginFile = pluginFile;
			execute = hook.HookType.GetMethod(Method);
			Key = plugin.Key;
			Name = plugin.Name;
			Description = plugin.Description;
			Image = plugin.Image;
			Enabled = plugin.Enabled;
			PCount = hook.HookType.GetProperty("PCount")?.GetValue(plugin) as int? ?? 0;
			Hook = hook.Hook;
		}

		// pluginFile is the file that stands for the plugin on disk: the DLL of a compiled plugin, the .csplugin file of a source plugin.
		public static IEnumerable<DotNetCommand> CreateAll(DotNetPlugin plugin, string pluginFile)
		{
			return DotNetHookRegistry.GetHooks(plugin).Select(hook => new DotNetCommand(plugin, hook, pluginFile));
		}

		protected override void Log(string text, params object[] o)
		{
			base.Log(text, o);
			LogDefault(text, o);
			// Should check Nadiar fork to implement the new script console at the same time.
		}

		protected override object OnInvoke(object[] data)
		{
			Log($"Calling '{Key}.{Hook}'...");
			try
			{
				return execute.Invoke(Plugin, data);
			}
			catch (TargetInvocationException e) when (e.InnerException != null)
			{
				Log($"{Key}.{Hook} failed: {e.InnerException.Message}");
				ExceptionDispatchInfo.Capture(e.InnerException).Throw();
				throw;
			}
			catch (Exception e)
			{
				Log($"{Key}.{Hook} failed: {e.Message}");
				throw;
			}
		}

		protected override bool IsValid
		{
			get
			{
				if (base.IsValid && !string.IsNullOrEmpty(Hook))
				{
					return execute != null;
				}
				return false;
			}
		}

		protected override void OnInitialize(IPluginEnvironment env, string path)
		{
			base.OnInitialize(env, path);
			Log($"\nInitializing script '{Key}.{Hook}' from '{(Path.GetFileName(pluginFile))}'");
			if (CommandImage == null && !string.IsNullOrEmpty(Plugin.Image))
				CommandImage = LoadEmbeddedImage();

			try
			{
				Plugin.Load(Environment, path);
			}
			catch (Exception e)
			{
				Log($"Skipped '{Key}.{Hook}' from '{pluginFile}': OnLoad failed: {e.Message}");
				throw;
			}
		}

		// The base class loads the image from the plugin folder; this fills in a missing file from the plugin assembly, so a single DLL can carry its image.
		private System.Drawing.Image LoadEmbeddedImage()
		{
			string file = Path.GetFileName(Plugin.Image);
			var candidates = new List<string>();
			if (Environment != null && Environment.Theme.CurrentTheme != Themes.Default)
				candidates.Add(Environment.Theme.CurrentTheme + file);

			candidates.Add(file);
			Assembly assembly = Plugin.GetType().Assembly;
			string[] resources = assembly.GetManifestResourceNames();
			foreach (string candidate in candidates)
			{
				string resource = resources.FirstOrDefault(r => r.Equals(candidate, StringComparison.OrdinalIgnoreCase) || r.EndsWith("." + candidate, StringComparison.OrdinalIgnoreCase));
				if (resource == null)
					continue;

				try
				{
					using (Stream stream = assembly.GetManifestResourceStream(resource))
					using (System.Drawing.Image source = System.Drawing.Image.FromStream(stream))
					{
						return source.CreateCopy(alwaysTrueCopy: true);
					}
				}
				catch
				{
					return null;
				}
			}
			return null;
		}

		public override string LoadConfig()
		{
			try
			{
				return File.ReadAllText(ConfigFileName);
			}
			catch
			{
				return string.Empty;
			}
		}

		public override bool SaveConfig(string config)
		{
			try
			{
				File.WriteAllText(ConfigFileName, config);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void HandleException(Exception e)
		{
			// TODO: Implement proper exception handling/logging for plugin command invocation
			if (e.InnerException != null)
				e = e.InnerException;

			Log(e.Message);
			return;
		}
	}
}
