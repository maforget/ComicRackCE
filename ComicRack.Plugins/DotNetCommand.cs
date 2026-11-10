using cYo.Common.ComponentModel;
using cYo.Common.Text;
using cYo.Projects.ComicRack.Engine;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Plugins
{
	public class DotNetCommand : Command
	{
		public Func<object[], object> ScriptHandler { get; init; }
		public string ScriptFile { get; set; }
		public string LibPath { get; private set; }
		private string ConfigFileName => ScriptFile + "-" + Key + ".config";
		public DotNetPlugin Plugin { get; internal set; }

		protected override void Log(string text, params object[] o)
		{
			base.Log(text, o);
			// TODO: Implement proper logging for plugin command invocation.
			// Need to fix how the script console is wired with python commands, so that we can log to the console for .NET commands as well.
			// Should check Nadiar fork to implement the new script console at the same time.
		}

		protected override object OnInvoke(object[] data)
		{
			try
			{
				Log($"Calling '{Key}.{Hook}'...");
				if (DotNetHookRegistry.TryGetHook(Hook, out IDotNetHook options))
				{
					return ScriptHandler(data);
				}
				throw new NotImplementedException($"No handler found for hook {Hook} with type {options?.HookType?.Name ?? "Unknown"}");
			}
			catch (Exception e)
			{
				HandleException(e);
				throw;
			}
		}

		protected override bool IsValid
		{
			get
			{
				if (base.IsValid && !string.IsNullOrEmpty(Hook))
				{
					return ScriptHandler != null;
				}
				return false;
			}
		}

		protected override void OnInitialize(IPluginEnvironment env, string path)
		{
			base.OnInitialize(env, path);
			try
			{
				LibPath = path;
				Log($"\nInitializing script '{Key}.{Hook}' from '{ScriptFile}'");
				ScriptFile = Command.GetFile(path, ScriptFile);
				Plugin.SetPluginEnvironment(Environment);
				if (!File.Exists(ScriptFile))
				{
					throw new FileNotFoundException("Script file not found");
				}
			}
			catch (Exception e)
			{
				HandleException(e);
				throw;
			}
		}

		protected override void MakeDefaults()
		{
			// TODO: Implement proper default values for plugin command invocation.
			// DotNetPlugin Enforces the Key to be set and the Name to default to the Key if not set.
			base.MakeDefaults();
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
