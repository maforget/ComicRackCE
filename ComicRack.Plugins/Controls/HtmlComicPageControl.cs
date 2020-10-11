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
	public class HtmlComicPageControl : ComicPageControl
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

		private IContainer components;

		private WebBrowser webBrowser;

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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (SaveConfigFunction != null && originalConfig != ScriptConfig)
				{
					SaveConfigFunction(ScriptConfig);
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
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

		private void InitializeComponent()
		{
			webBrowser = new System.Windows.Forms.WebBrowser();
			SuspendLayout();
			webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			webBrowser.Location = new System.Drawing.Point(0, 0);
			webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			webBrowser.Name = "webBrowser";
			webBrowser.Size = new System.Drawing.Size(540, 402);
			webBrowser.TabIndex = 1;
			webBrowser.WebBrowserShortcutsEnabled = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(webBrowser);
			base.Name = "HtmlInfoControl";
			base.Size = new System.Drawing.Size(540, 402);
			ResumeLayout(false);
		}
	}
}
