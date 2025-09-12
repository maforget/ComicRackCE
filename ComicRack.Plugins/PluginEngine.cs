using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.Collections;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Plugins
{
	public class PluginEngine
	{
		private readonly PluginInitializer[] initializers = new PluginInitializer[2]
		{
			new XmlPluginInitializer(),
			new PythonPluginInitializer()
		};

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

		private readonly CommandCollection commands = new CommandCollection();

		private CommandCollection Commands => commands;

		public string CommandStates
		{
			get
			{
				return Commands.Select((Command cmd) => (cmd.Enabled ? "+" : "-") + cmd.Key).ToListString(",");
			}
			set
			{
				value.FromListString(',').SafeForEach(delegate(string s)
				{
					Commands[s.Substring(1)].Enabled = s[0] == '+';
				});
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
			foreach (string file in FileUtility.GetFiles(path, SearchOption.AllDirectories))
			{
				foreach (Command cmd in initializers.SelectMany((PluginInitializer si) => si.GetCommands(file)))
				{
					try
					{
						if (!cmd.IsHook(hooks) || !cmd.Initialize(env, Path.GetDirectoryName(file)))
						{
							continue;
						}
						if (cmd.Hook == ScriptTypeConfig)
						{
							list.Add(cmd);
						}
						else
						{
							if (commands.Exists((Command c) => c.Key == cmd.Key))
							{
								continue;
							}
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
					Command command = commands.FirstOrDefault((Command c) => c.Key == cfg.Key);
					if (command != null)
					{
						command.Configure = cfg;
					}
				}
			}
		}

		public void Invoke(string hook, object[] data)
		{
			GetCommands(hook).ForEach(delegate(Command c)
			{
				c.Invoke(data);
			});
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
