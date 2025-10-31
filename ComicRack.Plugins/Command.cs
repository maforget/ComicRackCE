using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Projects.ComicRack.Plugins
{
	public abstract class Command
	{
		private bool enabled = true;

		private Keys shortCutKeys;

		[XmlAttribute]
		[DefaultValue(null)]
		public string Hook
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(0)]
		public int PCount
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(null)]
		public string Key
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string Description
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string Image
		{
			get => GetThemedImage();
			set => image = value;
		}
		private string image = null;

		[XmlIgnore]
		public Image CommandImage
		{
			get;
			set;
		}

		[XmlIgnore]
		public IPluginEnvironment Environment
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(true)]
		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		[XmlIgnore]
		public Keys ShortCutKeys
		{
			get
			{
				return shortCutKeys;
			}
			set
			{
				shortCutKeys = value;
			}
		}

		[XmlIgnore]
		public Command Configure
		{
			get;
			set;
		}

		protected virtual bool IsValid
		{
			get
			{
				if (!string.IsNullOrEmpty(Hook))
				{
					return !string.IsNullOrEmpty(Name);
				}
				return false;
			}
		}

		public bool Initialize(IPluginEnvironment env, string path)
		{
			try
			{
				MakeDefaults();
				if (!IsValid)
				{
					return false;
				}
				OnInitialize(env, path);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public object Invoke(object[] data, bool catchErrors = false)
		{
			try
			{
				return OnInvoke(data);
			}
			catch (Exception)
			{
				if (!catchErrors)
				{
					throw;
				}
				return null;
			}
		}

		public bool IsHook(params string[] hooks)
		{
			return hooks.Any((string hook) => Hook.Contains(hook));
		}

		public void PreCompile(bool handleException = true)
		{
			OnPreCompile(handleException);
		}

		public string GetLocalizedName()
		{
			if (Environment != null)
			{
				return Environment.Localize(Key, "Name", Name);
			}
			return Name;
		}

		public string GetLocalizedDescription()
		{
			if (Environment != null)
			{
				return Environment.Localize(Key, "Description", Description);
			}
			return Name;
		}

		protected static string GetFile(string basePath, string file)
		{
			if (!File.Exists(file))
			{
				return Path.Combine(basePath, Path.GetFileName(file));
			}
			return file;
		}

		protected virtual void Log(string text, params object[] o)
		{
		}

		protected virtual void OnInitialize(IPluginEnvironment env, string path)
		{
			Environment = env.Clone() as IPluginEnvironment;
			if (Environment != null)
			{
				Environment.CommandPath = path;
			}
			try
			{
				if (!string.IsNullOrEmpty(Image))
				{
					CommandImage = System.Drawing.Image.FromFile(GetFile(path, Image)).CreateCopy(alwaysTrueCopy: true); // Since we change Image it should load the correct themed image instead. 
				}
			}
			catch
			{
				CommandImage = null;
			}
		}

		private string GetThemedImage()
		{
			if (Environment?.Theme.CurrentTheme != Themes.Default)
			{
				string themeName = Environment.Theme.CurrentTheme.ToString();
				string themedImage = !string.IsNullOrEmpty(image) ? $"{themeName}{image}" : null; // Find the name of the themed image

				if (!string.IsNullOrEmpty(themedImage) && FileUtility.SafeFileExists(GetFile(Environment.CommandPath, themedImage)))
					return themedImage; // Return themed image name if it exists
			}

			return image; // Otherwise return regular image
		}

		protected virtual void MakeDefaults()
		{
		}

		public virtual string LoadConfig()
		{
			return string.Empty;
		}

		public virtual bool SaveConfig(string config)
		{
			return false;
		}

		public virtual void OnPreCompile(bool handleException)
		{
		}

		protected abstract object OnInvoke(object[] data);
	}
}
