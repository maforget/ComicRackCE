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
	/// <see cref="ComicRack"/> and <see cref="ScriptPath"/> are not set in the constructor. Do the real initialization in <see cref="OnLoad"/>.
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
		/// Called once, after <see cref="ComicRack"/> and <see cref="ScriptPath"/> are set and before any hook runs.
		/// If it throws, none of the hooks of the class are registered.
		/// </summary>
		protected virtual void OnLoad()
		{
		}

		// Every hook command of the class initializes the same instance; a failed OnLoad has to fail all of them.
		internal void Load(IPluginEnvironment env, string folder)
		{
			loadFailure?.Throw();
			if (loaded)
			{
				return;
			}
			loaded = true;
			ComicRack = env;
			ScriptPath = folder;
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