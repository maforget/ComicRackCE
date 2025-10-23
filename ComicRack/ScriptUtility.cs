using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Localize;
using cYo.Common.Net.Search;
using cYo.Common.Text;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Controls;
using cYo.Projects.ComicRack.Engine.Display;
using cYo.Projects.ComicRack.Plugins;
using cYo.Projects.ComicRack.Plugins.Automation;
using cYo.Projects.ComicRack.Plugins.Controls;
using cYo.Projects.ComicRack.Plugins.Theme;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Common.Windows.Forms.Theme;

namespace cYo.Projects.ComicRack.Viewer
{
	public static class ScriptUtility
	{
		private class ScriptSearch : CachedSearch
		{
			public Command Command
			{
				get;
				set;
			}

			public override string Name => Command.GetLocalizedName();

			public override Image Image => Command.CommandImage;

			protected override IEnumerable<SearchResult> OnSearch(string hint, string text, int limit)
			{
				Dictionary<string, string> source = Command.Invoke(new object[3]
				{
					hint,
					text,
					limit
				}) as Dictionary<string, string>;
				return source.Select((KeyValuePair<string, string> kvp) => new SearchResult
				{
					Name = kvp.Key,
					Result = kvp.Value
				});
			}

			protected override string OnGenericSearchLink(string hint, string text)
			{
				Dictionary<string, string> source = Command.Invoke(new object[3]
				{
					hint,
					text,
					-1
				}) as Dictionary<string, string>;
				return source.Select((KeyValuePair<string, string> kvp) => kvp.Value).FirstOrDefault();
			}
		}

		public static PluginEngine Scripts
		{
			get;
			private set;
		}

		public static bool Enabled => Program.Settings.Scripting;

		public static bool Initialize(IWin32Window mainWindow, IApplication app, IBrowser browser, IComicDisplay comicDisplay, IPluginConfig config, IOpenBooksManager openBooks)
		{
			Scripts = new PluginEngine();
			if (!Enabled)
				return false;

			PluginEnvironment env = new PluginEnvironment(mainWindow, app, browser, comicDisplay, config, openBooks, ThemePlugin.Default);
			Scripts.Initialize(env, Program.Paths.ScriptPath);
			Scripts.Initialize(env, Program.Paths.ScriptPathSecondary);
			Scripts.CommandStates = Program.Settings.PluginsStates;
			ComicBookDialog.ScriptEngine = Scripts;
			ComicBookPluginMatcher.PluginEngine = Scripts;
			foreach (Command command in Scripts.GetCommands(PluginEngine.ScriptTypeParseComicPath))
			{
				Command c = command;
				ComicBook.ParseFilePath += (sender, e) => c.Invoke([e.Path, e.NameInfo], catchErrors: true);
			}
			foreach (Command command2 in Scripts.GetCommands(PluginEngine.ScriptTypeSearch))
			{
				SearchEngines.Engines.Add(new ScriptSearch { Command = command2 });
			}
			return true;
		}

		public static void CreateBookCode(Control parent, Command command, Func<IEnumerable<ComicBook>> books)
		{
			Program.Database.Undo.SetMarker(StringUtility.Format(TR.Messages["UndoScript", "Automation '{0}'"], command.GetLocalizedName()));
			try
			{
				ComicBook[] array = books().ToArray();
				using (new WaitCursor(parent))
				{
					command.Invoke(new object[1]
					{
						books().ToArray()
					});
				}
			}
			catch (Exception ex)
			{
				ShowError(parent, ex);
			}
		}

		public static T CreateToolItem<T>(Control parent, Command command, Func<IEnumerable<ComicBook>> books) where T : ToolStripItem, new()
		{
			T val = new T();
			val.Text = command.GetLocalizedName();
			T val2 = val;
			val2.Image = command.CommandImage;
			val2.ToolTipText = command.GetLocalizedDescription();
			if (val2 is ToolStripSplitButton)
			{
				ToolStripSplitButton tssb = (ToolStripSplitButton)(object)val2;
				tssb.ButtonClick += delegate
				{
					CreateBookCode(parent, command, books);
				};
				tssb.MouseDown += delegate(object s, MouseEventArgs e)
				{
					if (e.Button == MouseButtons.Right)
					{
						tssb.ShowDropDown();
					}
				};
				if (command.Configure != null)
				{
					tssb.DropDownItems.Add(TR.Default["Configure"] + "...", null, delegate
					{
						command.Configure.Invoke(null, catchErrors: true);
					});
				}
			}
			else
			{
				val2.Click += delegate
				{
					CreateBookCode(parent, command, books);
				};
			}
			if (val2 is ToolStripMenuItem)
			{
				((ToolStripMenuItem)(object)val2).ShortcutKeys = command.ShortCutKeys;
			}
			return val2;
		}

		public static IEnumerable<T> CreateToolItems<T>(Control parent, string scriptType, Func<IEnumerable<ComicBook>> books, Func<Command, bool> predicate = null) where T : ToolStripItem, new()
		{
			if (Scripts == null)
			{
				return Enumerable.Empty<T>();
			}
			return from command in Scripts.GetCommands(scriptType)
				where predicate == null || predicate(command)
				select CreateToolItem<T>(parent, command, books);
		}

		public static void ShowError(IWin32Window parent, Exception ex)
		{
			MessageBox.Show(parent, ex.Message, TR.Messages["ScriptFailed", "Execution of the script failed!"], MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}

		public static void Invoke(string hook, params object[] p)
		{
			try
			{
				if (Scripts != null)
				{
					Scripts.Invoke(hook, p);
				}
			}
			catch (Exception)
			{
			}
		}

		public static IEnumerable<Command> GetCommands(string hook)
		{
			try
			{
				return Scripts.GetCommands(hook);
			}
			catch (Exception)
			{
				return Enumerable.Empty<Command>();
			}
		}

		public static IEnumerable<ComicPageControl> CreatePagesHtml(string type)
		{
			if (Scripts == null)
			{
				yield break;
			}
			foreach (Command command2 in Scripts.GetCommands(type))
			{
				Command command = command2;
				yield return new HtmlComicPageControl
				{
					Text = command.Name,
					Icon = command.CommandImage,
					ScriptEngine = command.Environment,
					ScriptConfig = command.LoadConfig(),
					InfoFunction = (ComicBook[] b) =>
					{
						try
						{
							//TODO: Add a way to disable ReplaceWebColors if the plugin supports theme and doesn't need you to replace the colors
							return (command.Invoke([b]) as string)?.ReplaceWebColors();
						}
						catch (Exception ex)
						{
							return ex.ToString();
						}
					},
					SaveConfigFunction = cfg => command.SaveConfig(cfg)
				};
			}
		}

		public static IEnumerable<ComicPageControl> CreatePagesUI(string type)
		{
			if (Scripts == null)
			{
				yield break;
			}
			foreach (Command command2 in Scripts.GetCommands(type))
			{
				Command command = command2;
				Control control = null;
				Func<Control> func = () => command.Invoke(null) as Control;
				try
				{
					control = func();
				}
				catch (Exception)
				{
				}
				if (control != null)
				{
					yield return new UIComicPageControl(control)
					{
						Text = command2.Name,
						Icon = command2.CommandImage,
						CreatePlugin = func
					};
				}
			}
		}

		public static IEnumerable<ComicPageControl> CreateComicInfoPages()
		{
			foreach (ComicPageControl item in CreatePagesHtml(PluginEngine.ScriptTypeComicInfoHtml))
			{
				yield return item;
			}
			foreach (ComicPageControl item2 in CreatePagesUI(PluginEngine.ScriptTypeComicInfoUI))
			{
				yield return item2;
			}
		}

		public static IEnumerable<ComicPageControl> CreateQuickOpenPages()
		{
			foreach (ComicPageControl item in CreatePagesHtml(PluginEngine.ScriptTypeQuickOpenHtml))
			{
				yield return item;
			}
			foreach (ComicPageControl item2 in CreatePagesUI(PluginEngine.ScriptTypeQuickOpenUI))
			{
				yield return item2;
			}
		}
	}
}
