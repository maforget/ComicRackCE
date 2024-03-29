using System;
using System.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Plugins.Automation;

namespace cYo.Projects.ComicRack.Plugins
{
	public interface IPluginEnvironment : IPluginConfig, ICloneable
	{
		IWin32Window MainWindow
		{
			get;
		}

		IApplication App
		{
			get;
		}

		IOpenBooksManager OpenBooks
		{
			get;
		}

		IBrowser Browser
		{
			get;
		}

		IComicDisplay ComicDisplay
		{
			get;
		}

		string CommandPath
		{
			get;
			set;
		}

		string Localize(string resourceKey, string elementKey, string text);
	}
}
