using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Plugins.Automation;
using cYo.Projects.ComicRack.Plugins.Theme;

namespace cYo.Projects.ComicRack.Plugins
{
	[ComVisible(true)]
	public class PluginEnvironment : IPluginEnvironment, IPluginConfig, ICloneable
	{
		private TRDictionary localization;

		private readonly IPluginConfig config;

		public IWin32Window MainWindow
		{
			get;
			private set;
		}

		public IApplication App
		{
			get;
			private set;
		}

		public IBrowser Browser
		{
			get;
			private set;
		}

		public IOpenBooksManager OpenBooks
		{
			get;
			set;
		}

		public IComicDisplay ComicDisplay
		{
			get;
			private set;
		}

		public string CommandPath
		{
			get;
			set;
		}

		public IThemePlugin Theme
		{
			get;
			set;
		}

		public IEnumerable<string> LibraryPaths => config.LibraryPaths;

		public PluginEnvironment(IWin32Window mainWindow, IApplication app, IBrowser browser, IComicDisplay comicDisplay, IPluginConfig config, IOpenBooksManager openBooksManager, IThemePlugin themePlugin = default)
		{
			App = app;
			Browser = browser;
			ComicDisplay = comicDisplay;
			MainWindow = mainWindow;
			OpenBooks = openBooksManager;
			Theme = themePlugin;
			this.config = config;
		}

		public IEnumerable<ComicBook> ReadDatabaseBooks(string file)
		{
			return ComicDatabase.LoadXml(file).Books;
		}

		public string Localize(string resourceKey, string nameKey, string text)
		{
			if (string.IsNullOrEmpty(resourceKey))
			{
				return text;
			}
			if (localization == null)
			{
				localization = new TRDictionary
				{
					ResourceFolder = new PackedLocalize(new VirtualFileFolder(CommandPath))
				};
			}
			TR tR = localization.Load(resourceKey);
			if (tR.IsEmpty)
			{
				tR = TR.Load("Script." + resourceKey);
				if (tR.IsEmpty)
				{
					tR = TR.Load(resourceKey);
				}
			}
			return tR[nameKey, text];
		}

		public object Clone()
		{
			return new PluginEnvironment(MainWindow, App, Browser, ComicDisplay, config, OpenBooks, Theme);
		}
	}
}
