using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;

namespace cYo.Projects.ComicRack.Plugins.Controls
{
	public partial class HtmlComicPageControl : ComicPageControl
	{
		[ComVisible(true)]
		public class ScriptProvider
		{
			public object ComicRack
			{
				get;
				set;
			}

			public string Config
			{
				get;
				set;
			}
		}

		private string lastResult;

		private string originalConfig;

		public Func<ComicBook[], string> InfoFunction
		{
			get;
			set;
		}

		public Action<string> SaveConfigFunction
		{
			get;
			set;
		}

		public ScriptProvider Script
		{
			get;
			private set;
		}

		public object ScriptEngine
		{
			get
			{
				return Script.ComicRack;
			}
			set
			{
				Script.ComicRack = value;
			}
		}

		public string ScriptConfig
		{
			get
			{
				return Script.Config;
			}
			set
			{
				string text2 = (originalConfig = (Script.Config = value));
			}
		}

		public HtmlComicPageControl()
		{
			Script = new ScriptProvider();
			InitializeComponent();
		}

		protected override void OnShowInfo(IEnumerable<ComicBook> books)
		{
			base.OnShowInfo(books);
			if (InfoFunction == null)
			{
				return;
			}
			try
			{
				string text = InfoFunction(books.ToArray());
				if (!(text == lastResult))
				{
					webBrowser.ObjectForScripting = Script;
					webBrowser.ScriptErrorsSuppressed = !EngineConfiguration.Default.EnableHtmlScriptErrors;
					webBrowser.IsWebBrowserContextMenuEnabled = EngineConfiguration.Default.HtmlInfoContextMenu;
					if (text.StartsWith("!"))
					{
						webBrowser.Url = new Uri(text.Substring(1));
					}
					else
					{
						webBrowser.DocumentText = text;
					}
					lastResult = text;
				}
			}
			catch (Exception)
			{
				webBrowser.DocumentText = string.Empty;
				lastResult = null;
			}
		}
	}
}
