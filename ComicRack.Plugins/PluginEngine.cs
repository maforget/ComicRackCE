using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Plugins
{
	public class PluginEngine : DisposableObject
	{
		private readonly PluginInitializer[] initializers;

		private readonly CSharpSourcePluginInitializer sourceInitializer;

		public const string ScriptTypeCreateBookList = "CreateBookList";

		public const string ScriptTypeParseComicPath = "ParseComicPath";

		public const string ScriptTypeLibrary = "Library";

		public const string ScriptTypeEditor = "Editor";

		public const string ScriptTypeBooks = "Books";

		public const string ScriptTypeNewBooks = "NewBooks";

		public const string ScriptTypeBookOpened = "BookOpened";

		public const string ScriptTypeReaderResized = "ReaderResized";

		public const string ScriptTypeSearch = "NetSearch";

		public const string ScriptTypeStartup = "Startup";

		public const string ScriptTypeShutdown = "Shutdown";

		public const string ScriptTypeConfig = "ConfigScript";

		public const string ScriptTypeComicInfoHtml = "ComicInfoHtml";

		public const string ScriptTypeComicInfoUI = "ComicInfoUI";

		public const string ScriptTypeQuickOpenHtml = "QuickOpenHtml";

		public const string ScriptTypeQuickOpenUI = "QuickOpenUI";

		public const string ScriptTypeDrawThumbnailOverlay = "DrawThumbnailOverlay";

		public const string ScriptDescEditBooks = "Edit/Update Books Commands";

		public const string ScriptDescNewBooks = "Create New Books Commands";

		public const string ScriptDescParsePath = "Book Path Parsers";

		public const string ScriptDescBookOpened = "Actions when Books are opened";

		public const string ScriptDescReaderResized = "Actions when Reader is resized";

		public const string ScriptDescSearch = "Additional Search Providers";

		public const string ScriptDescInfo = "Book Information Panels";

		public const string ScriptDescStartup = "Actions when ComicRack starts";

		public const string ScriptDescShutdown = "Actions when ComicRack shuts down";

		public const string ScriptDescQuickOpen = "Quick Open Panels";

		public const string ScriptDescThumbOverlay = "Custom Book Thumbnail Overlays";

		public static readonly Dictionary<string, string> ValidHooks = new Dictionary<string, string>
		{
			{
				ScriptTypeCreateBookList,
				ScriptDescEditBooks
			},
			{
				ScriptTypeParseComicPath,
				ScriptDescParsePath
			},
			{
				ScriptTypeLibrary,
				ScriptDescEditBooks
			},
			{
				ScriptTypeEditor,
				ScriptDescEditBooks
			},
			{
				ScriptTypeBooks,
				ScriptDescEditBooks
			},
			{
				ScriptTypeNewBooks,
				ScriptDescNewBooks
			},
			{
				ScriptTypeBookOpened,
				ScriptDescBookOpened
			},
			{
				ScriptTypeReaderResized,
				ScriptDescReaderResized
			},
			{
				ScriptTypeSearch,
				ScriptDescSearch
			},
			{
				ScriptTypeConfig,
				string.Empty
			},
			{
				ScriptTypeStartup,
				ScriptDescStartup
			},
			{
				ScriptTypeShutdown,
				ScriptDescShutdown
			},
			{
				ScriptTypeComicInfoHtml,
				ScriptDescInfo
			},
			{
				ScriptTypeComicInfoUI,
				ScriptDescInfo
			},
			{
				ScriptTypeQuickOpenHtml,
				ScriptDescQuickOpen
			},
			{
				ScriptTypeQuickOpenUI,
				ScriptDescQuickOpen
			},
			{
				ScriptTypeDrawThumbnailOverlay,
				ScriptDescThumbOverlay
			}
		};

		/// <param name="sourcePluginCachePath">The folder that keeps the compiled assemblies of source plugins (.csplugin). The default is a folder in the temporary files of the user.</param>
		public PluginEngine(string sourcePluginCachePath = null)
		{
			sourceInitializer = new CSharpSourcePluginInitializer(sourcePluginCachePath ?? Path.Combine(Path.GetTempPath(), "ComicRack", "PluginCache"));
			initializers =
			[
				new XmlPluginInitializer(),
				new PythonPluginInitializer(),
				new DotNetPluginInitializer(),
				sourceInitializer
			];
		}

		private readonly CommandCollection commands = new CommandCollection();

		private CommandCollection Commands => commands;

		public string CommandStates
		{
			get
			{
				// The hook commands of one .NET plugin class share a Key, and the state belongs to the Key.
				return Commands.GroupBy((Command cmd) => cmd.Key).Select((IGrouping<string, Command> g) => (g.First().Enabled ? "+" : "-") + g.Key).ToListString(",");
			}
			set
			{
				value.FromListString(',').SafeForEach((string s) => SetEnabled(s.Substring(1), s[0] == '+'));
			}
		}

		public void SetEnabled(string key, bool enabled)
		{
			foreach (Command command in Commands.Where((Command cmd) => cmd.Key == key))
			{
				command.Enabled = enabled;
			}
		}

		public IEnumerable<Command> GetAllCommands()
		{
			return Commands;
		}

		public IEnumerable<Command> GetCommands(string hook)
		{
			return Commands.Where((Command cmd) => cmd.Enabled && cmd.IsHook(hook));
		}

		public void Initialize(IPluginEnvironment env, string path)
		{
			List<Command> list = new List<Command>();
			string[] hooks = ValidHooks.Keys.ToArray();
			List<string> files = FileUtility.GetFiles(path, SearchOption.AllDirectories).ToList();
			List<string> sourcePluginFolders = files.Where(CSharpSourcePluginInitializer.IsMarker).Select(Path.GetDirectoryName).ToList();
			foreach (string file in files.Where((string f) => !CSharpSourcePluginInitializer.IsShadowedBySource(f, sourcePluginFolders)))
			{
				foreach (Command cmd in initializers.SelectMany((PluginInitializer si) => si.GetCommands(file, env)))
				{
					try
					{
						// The Key of a .NET command is known before Initialize. Checking it first keeps OnLoad from running in a plugin that is skipped.
						if (cmd is DotNetCommand && IsKeyTakenByAnotherPlugin(cmd))
						{
							Command.LogDefault("Skipped .NET plugin command '{0}.{1}': the Key is already used by another plugin", cmd.Key, cmd.Hook);
							continue;
						}
						if (!cmd.IsHook(hooks) || !cmd.Initialize(env, Path.GetDirectoryName(file)))
							continue;

						if (cmd.Hook == ScriptTypeConfig)
						{
							list.Add(cmd);
						}
						else
						{
							if (IsKeyTakenByAnotherPlugin(cmd))
								continue;

							if (cmd.Enabled)
							{
								int num = commands.Count((Command c) => c.Enabled);
								cmd.ShortCutKeys = (Keys)((num < 12) ? (0x30000 | (112 + num)) : 0);
							}
							commands.Add(cmd);
							continue;
						}
					}
					catch
					{
					}
				}
				foreach (Command cfg in list)
				{
					if (cfg is DotNetCommand)
					{
						foreach (Command command in commands.Where((Command c) => IsSamePlugin(c, cfg)))
						{
							command.Configure = cfg;
						}
						continue;
					}
					Command first = commands.FirstOrDefault((Command c) => c.Key == cfg.Key && !(c is DotNetCommand));
					if (first != null)
					{
						first.Configure = cfg;
					}
				}
			}
		}

		/// <summary>Call once after Initialize has scanned every scripts folder. Deletes the compiled assemblies of source plugins that no folder uses any more.</summary>
		public void CompleteInitialization()
		{
			sourceInitializer.SweepCache();
		}

		// A Key belongs to the first plugin that registers it. Only the hook commands of one .NET plugin class share their Key.
		private bool IsKeyTakenByAnotherPlugin(Command cmd) => commands.Exists((Command c) => c.Key == cmd.Key && !IsSamePlugin(c, cmd));

		private static bool IsSamePlugin(Command a, Command b) => a is DotNetCommand x && b is DotNetCommand y && x.Plugin == y.Plugin;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				IEnumerable<DotNetPlugin> plugins = commands
					.SelectMany((Command c) => new[] { c, c.Configure })
					.OfType<DotNetCommand>()
					.Select((DotNetCommand c) => c.Plugin)
					.Distinct();

				foreach (DotNetPlugin plugin in plugins)
				{
					try
					{
						(plugin as IDisposable)?.Dispose();
					}
					catch (Exception e)
					{
						Command.LogDefault("Failed to dispose .NET plugin '{0}': {1}", plugin.Key, e.Message);
					}
				}
			}

			base.Dispose(disposing);
		}

		public void Invoke(string hook, object[] data)
		{
			GetCommands(hook).ForEach((Command c) => c.Invoke(data));
		}

		public static string GetHookDescription(string hook)
		{
			string text = hook.Split(',').TrimStrings().RemoveEmpty()
				.FirstOrDefault();
			if (string.IsNullOrEmpty(text) || !ValidHooks.TryGetValue(text, out var value))
			{
				return string.Empty;
			}
			return TR.Load("PluginEngine")[value.ReplaceAny("/ ", string.Empty), value];
		}
	}
}