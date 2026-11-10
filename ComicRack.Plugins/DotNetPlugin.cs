using cYo.Projects.ComicRack.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Plugins
{
	public abstract class DotNetPlugin
	{
		protected IPluginEnvironment ComicRack { get; private set; } // Uses the same name than Python to keep the usage similar
		internal void SetPluginEnvironment(IPluginEnvironment env) => ComicRack = env;

		protected string ScriptPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // Uses the same name than Python to keep the usage similar

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
	}
}