using cYo.Projects.ComicRack.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Plugins
{
	/// <summary>
	/// Base class of a .NET plugin. Implement one or more hook interfaces (<see cref="IDotNetCommand"/>) on the class.
	/// </summary>
	/// <remarks>
	/// ComicRack creates one instance of every plugin class when it scans the script folders, also for a plugin that is disabled.
	/// The instance is shared by all hooks of the class and lives until ComicRack closes.
	/// Keep the constructor cheap: set <see cref="Key"/>, <see cref="Name"/> and the other properties there and nothing else.
	/// <see cref="ComicRack"/>, <see cref="ScriptPath"/> and <see cref="ConfigPath"/> are not set in the constructor. Do the real initialization in <see cref="OnLoad"/>.
	/// If the class implements <see cref="IDisposable"/>, ComicRack calls Dispose when it closes.
	/// </remarks>
	public abstract class DotNetPlugin
	{
		private bool loaded;
		private ExceptionDispatchInfo loadFailure;

		/// <summary>The host. Null until <see cref="OnLoad"/>.</summary>
		protected IPluginEnvironment ComicRack { get; private set; } // Uses the same name than Python to keep the usage similar

		/// <summary>The folder of the plugin: the folder of the DLL, or of the .csplugin file for a source plugin. Null until <see cref="OnLoad"/>.</summary>
		protected string ScriptPath { get; private set; } // Uses the same name than Python to keep the usage similar

		/// <summary>
		/// The folder for the settings of the plugin. It is a per-user folder that is kept when the plugin is updated. Null until <see cref="OnLoad"/>.
		/// </summary>
		/// <remarks>
		/// Do not save settings in <see cref="ScriptPath"/>: it can be read-only, and the package installer replaces it on update.
		/// The folder may not exist yet. Create it before you write to it.
		/// The config of a ComicInfoHtml panel is kept in this folder too.
		/// </remarks>
		protected string ConfigPath { get; private set; }

		public abstract string Key { get; set; }

		[DefaultValue(null)]
		public virtual string Name
		{
			get => field ??= Key; // Use the Key as default Name if not set
			set => field = value;
		}

		[DefaultValue(null)]
		public virtual string Description { get; set; }

		[DefaultValue(null)]
		public virtual string Image { get; set; }

		[DefaultValue(true)]
		public virtual bool Enabled { get; set; } = true;

		/// <summary>
		/// Called once, after <see cref="ComicRack"/>, <see cref="ScriptPath"/> and <see cref="ConfigPath"/> are set and before any hook runs.
		/// If it throws, none of the hooks of the class are registered.
		/// </summary>
		protected virtual void OnLoad()
		{
		}

		// Every hook command of the class initializes the same instance; a failed OnLoad has to fail all of them.
		internal void Load(IPluginEnvironment env, string folder, string configFolder)
		{
			loadFailure?.Throw();
			if (loaded)
			{
				return;
			}
			loaded = true;
			ComicRack = env;
			ScriptPath = folder;
			ConfigPath = configFolder;
			try
			{
				OnLoad();
			}
			catch (Exception e)
			{
				loadFailure = ExceptionDispatchInfo.Capture(e);
				throw;
			}
		}

		protected void Log(string message, [CallerMemberName] string caller = "")
		{
			Command.LogDefault($"[{Key}.{caller}] {message}");
		}
	}
}