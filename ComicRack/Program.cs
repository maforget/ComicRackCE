using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Compression;
using cYo.Common.Drawing;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Common.Mathematics;
using cYo.Common.Net;
using cYo.Common.Presentation.Tao;
using cYo.Common.Runtime;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.Display.Forms;
using cYo.Projects.ComicRack.Engine.Drawing;
using cYo.Projects.ComicRack.Engine.IO;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using cYo.Projects.ComicRack.Engine.IO.Provider;
using cYo.Projects.ComicRack.Engine.Sync;
using cYo.Projects.ComicRack.Plugins;
using cYo.Projects.ComicRack.Viewer.Config;
using cYo.Projects.ComicRack.Viewer.Dialogs;
using cYo.Projects.ComicRack.Viewer.Properties;
using Microsoft.Win32;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace cYo.Projects.ComicRack.Viewer
{
	public static class Program
	{
		private class MouseWheelDelegater : IMessageFilter
		{
			private IntPtr lastHandle;

			[DllImport("user32.dll")]
			private static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

			public bool PreFilterMessage(ref Message m)
			{
				switch (m.Msg)
				{
				case 512:
					lastHandle = m.HWnd;
					break;
				case 522:
				case 526:
					if (!(m.HWnd == lastHandle))
					{
						SendMessage(lastHandle, m.Msg, m.WParam, m.LParam);
						return true;
					}
					break;
				}
				return false;
			}
		}

		public const int DeadLockTestTime = 0;

		public const int MinThumbHeight = 96;

		public const int MaxThumbHeight = 512;

		public const int MinTileHeight = 64;

		public const int MaxTileHeight = 256;

		public const int MinRowHeight = 12;

		public const int MaxRowHeight = 48;

		public const string DefaultWiki = "https://web.archive.org/web/20161013095840/http://comicrack.cyolito.com:80/documentation/wiki";

		public const string DefaultWebSite = "https://github.com/maforget/ComicRackCE";

		public const string DefaultNewsFeed = "http://feeds.feedburner.com/ComicRackNews";

		public const string DefaultUserForm = "https://github.com/maforget/ComicRackCE/discussions";

		public const string DefaultLocalizePage = "https://web.archive.org/web/20170528182733/http://comicrack.cyolito.com/faqs/12-how-to-create-language-packs";

		public const string ComicRackTypeId = "cYo.ComicRack";

		public const string ComicRackDocumentName = "eComic";

		public const int NewsIntervalMinutes = 60;

		private static ExtendedSettings extendedSettings;

		public static readonly SystemPaths Paths = new SystemPaths(UseLocalSettings, ExtendedSettings.AlternateConfig, ExtendedSettings.DatabasePath, ExtendedSettings.CachePath);

		public static readonly ContextHelp Help = new ContextHelp(Path.Combine(Application.StartupPath, "Help"));

		public static readonly string QuickHelpManualFile = Path.Combine(Application.StartupPath, "Help\\ComicRack Introduction.djvu");

		private const string DefaultListsFile = "DefaultLists.txt";

		private static readonly string defaultSettingsFile = Path.Combine(Paths.ApplicationDataPath, "Config.xml");

		private static readonly string defaultNewsFile = Path.Combine(Paths.ApplicationDataPath, "NewsFeeds.xml");

		private const string DefaultBackgroundTexturesPath = "Resources\\Textures\\Backgrounds";

		private const string DefaultPaperTexturesPath = "Resources\\Textures\\Papers";

		private const string DefaultIconPackagesPath = "Resources\\Icons";

		public static readonly PackageManager ScriptPackages = new PackageManager(Paths.ScriptPathSecondary, Paths.PendingScriptsPath, commit: true);

		public static readonly DatabaseManager DatabaseManager = new DatabaseManager();

		private static readonly object installedLanguagesLock = new object();

		private static List<TRInfo> installedLanguages;

		private static Splash splash;

		private static readonly Regex yearRegex = new Regex("\\((?<start>\\d{4})-(?<end>\\d{4})\\)", RegexOptions.Compiled);

		public static ExtendedSettings ExtendedSettings
		{
			get
			{
				if (extendedSettings == null)
				{
					extendedSettings = new ExtendedSettings();
					CommandLineParser.Parse(extendedSettings, CommandLineParserOptions.None);
					if (!string.IsNullOrEmpty(extendedSettings.AlternateConfig))
					{
						IniFile.AddDefaultLocation(SystemPaths.MakeApplicationPath(extendedSettings.AlternateConfig, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
					}
					extendedSettings = new ExtendedSettings();
					CommandLineParser.Parse(extendedSettings);
					if (extendedSettings.Restart)
					{
						extendedSettings.InstallPlugin = null;
						extendedSettings.ImportList = null;
						extendedSettings.Files = Enumerable.Empty<string>();
					}
				}
				return extendedSettings;
			}
		}

		public static MainForm MainForm
		{
			get;
			private set;
		}

		public static DefaultLists Lists
		{
			get;
			private set;
		}

		public static bool Restart
		{
			get;
			set;
		}

		public static ScriptOutputForm ScriptConsole
		{
			get;
			set;
		}

		public static bool UseLocalSettings => IniFile.Default.GetValue("UseLocalSettings", def: false);

		public static Settings Settings
		{
			get;
			private set;
		}

		public static NewsStorage News
		{
			get;
			private set;
		}

		public static CacheManager CacheManager
		{
			get;
			private set;
		}

		public static ImagePool ImagePool => CacheManager.ImagePool;

		public static NetworkManager NetworkManager
		{
			get;
			private set;
		}

		public static QueueManager QueueManager
		{
			get;
			private set;
		}

		public static ComicScanner Scanner => QueueManager.Scanner;

		public static ComicDatabase Database => DatabaseManager.Database;

		public static ComicBookFactory BookFactory => DatabaseManager.BookFactory;

		public static FileCache InternetCache => CacheManager.InternetCache;

		public static IEnumerable<string> CommandLineFiles => ExtendedSettings.Files ?? Enumerable.Empty<string>();

		public static ExportSettingCollection ExportComicRackPresets => new ExportSettingCollection
		{
			ExportSetting.ConvertToCBZ,
			ExportSetting.ConvertToCB7
		};

		public static IEnumerable<StringPair> DefaultKeyboardMapping
		{
			get;
			set;
		}

		public static TRInfo[] InstalledLanguages
		{
			get
			{
				using (ItemMonitor.Lock(installedLanguagesLock))
				{
					if (installedLanguages == null)
					{
						installedLanguages = new List<TRInfo>();
						TRDictionary tRDictionary = null;
						try
						{
							tRDictionary = new TRDictionary(TR.ResourceFolder, "de");
						}
						catch (Exception)
						{
						}
						foreach (TRInfo languageInfo in TR.GetLanguageInfos())
						{
							TRDictionary tRDictionary2 = new TRDictionary(TR.ResourceFolder, languageInfo.CultureName);
							if (tRDictionary != null)
							{
								languageInfo.CompletionPercent = tRDictionary2.CompletionPercent(tRDictionary);
							}
							installedLanguages.Add(languageInfo);
						}
						installedLanguages.Sort((TRInfo a, TRInfo b) =>
                        {
                            int num = b.CompletionPercent.CompareTo(a.CompletionPercent);
                            return (num == 0) ? string.Compare(a.CultureName, b.CultureName) : num;
                        });
					}
					return installedLanguages.ToArray();
				}
			}
		}

		public static IEnumerable<string> HelpSystems => Help.HelpSystems;

		public static string HelpSystem
		{
			get
			{
				return Help.HelpName;
			}
			set
			{
				if (!(Help.HelpName == value))
				{
					if (!Help.Load(value))
					{
						Help.Load("ComicRack Wiki");
					}
					Help.Variables["APPEXE"] = Application.ExecutablePath;
					Help.Variables["APPPATH"] = Path.GetDirectoryName(Application.ExecutablePath);
					Help.Variables["APPDATA"] = Paths.ApplicationDataPath;
					Help.Variables["USERPATH"] = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
					Help.ShowKey = ExtendedSettings.ShowContextHelpKey;
					Help.Initialize();
				}
			}
		}

		public static void RefreshAllWindows()
		{
			ForAllForms((Form f) =>
            {
                f.Refresh();
            });
		}

		public static void ForAllForms(Action<Form> action)
		{
			if (action == null)
			{
				return;
			}
			foreach (Form openForm in Application.OpenForms)
			{
				action(openForm);
			}
		}

		public static string ShowComicOpenDialog(IWin32Window parent, string title, bool includeReadingLists)
		{
			string result = null;
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				IEnumerable<FileFormat> enumerable = from f in Providers.Readers.GetSourceFormats()
					orderby f
					select f;
				if (includeReadingLists)
				{
					enumerable = enumerable.AddLast(new FileFormat(TR.Load("FileFilter")["FormatReadingList", "ComicRack Reading List"], 10089, ".cbl"));
				}
				openFileDialog.Title = title;
				openFileDialog.Filter = enumerable.GetDialogFilter(withAllFilter: true);
				openFileDialog.FilterIndex = Settings.LastOpenFilterIndex;
				openFileDialog.CheckFileExists = true;
				foreach (string favoritePath in GetFavoritePaths())
				{
					openFileDialog.CustomPlaces.Add(favoritePath);
				}
				if (openFileDialog.ShowDialog(parent) == DialogResult.OK)
				{
					result = openFileDialog.FileName;
				}
				Settings.LastOpenFilterIndex = openFileDialog.FilterIndex;
				return result;
			}
		}

		public static bool AskQuestion(IWin32Window parent, string question, string okButton, HiddenMessageBoxes hmb, string askAgainText = null, string cancelButton = null)
		{
			if ((Settings.HiddenMessageBoxes & hmb) != 0)
			{
				return true;
			}
			if (string.IsNullOrEmpty(askAgainText))
			{
				askAgainText = TR.Messages["NotAskAgain", "&Do not ask me again"];
			}
			switch (QuestionDialog.AskQuestion(parent, question, okButton, askAgainText, null, showCancel: true, cancelButton))
			{
			case QuestionResult.Cancel:
				return false;
			case QuestionResult.OkWithOption:
				Settings.HiddenMessageBoxes |= hmb;
				break;
			}
			return true;
		}

		public static void Collect()
		{
			GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
		}

		public static void StartDocument(string document, string path = null)
		{
			try
			{
				ProcessStartInfo processStartInfo = new ProcessStartInfo(document);
				if (path != null && Directory.Exists(path))
				{
					processStartInfo.WorkingDirectory = path;
				}
				Process.Start(processStartInfo);
			}
			catch (Exception)
			{
			}
		}

		public static void StartProgram(string exe, string commandLine)
		{
			try
			{
				Process.Start(exe, commandLine);
			}
			catch (Exception)
			{
			}
		}

		public static IEnumerable<ComicBookValueMatcher> GetAvailableComicBookMatchers()
		{
			return ComicBookValueMatcher.GetAvailableMatchers().OfType<ComicBookValueMatcher>();
		}

		public static IEnumerable<ComicBookValueMatcher> GetUsedComicBookMatchers(int minUsage)
		{
			return from n in Database.ComicLists.GetItems<ComicSmartListItem>().SelectMany((ComicSmartListItem n) => n.Matchers.Recurse<ComicBookValueMatcher>((object o) => (!(o is ComicBookGroupMatcher)) ? null : ((ComicBookGroupMatcher)o).Matchers))
				select n.GetType() into n
				group n by n into g
				where g.Count() >= minUsage
				select g into t
				select Activator.CreateInstance(t.First()) as ComicBookValueMatcher into n
				orderby n.Description
				select n;
		}

		public static ContextMenuStrip CreateComicBookMatchersMenu(Action<ComicBookValueMatcher> action, int minUsage = 20)
		{
			ContextMenuBuilder contextMenuBuilder = new ContextMenuBuilder();
			Type[] source = (from m in GetUsedComicBookMatchers(5)
				select m.GetType()).ToArray();
			foreach (ComicBookValueMatcher availableComicBookMatcher in GetAvailableComicBookMatchers())
			{
				ComicBookValueMatcher i = availableComicBookMatcher;
				contextMenuBuilder.Add(availableComicBookMatcher.Description, topLevel: false, chk: false, delegate
				{
					action(i);
				}, null, source.Contains(availableComicBookMatcher.GetType()) ? DateTime.MaxValue : DateTime.MinValue);
			}
			ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
			contextMenuStrip.Items.AddRange(contextMenuBuilder.Create(20));
			return contextMenuStrip;
		}

		public static string[] LoadDefaultBackgroundTextures()
		{
			return (from s in FileUtility.GetFiles(IniFile.GetDefaultLocations("Resources\\Textures\\Backgrounds"), SearchOption.AllDirectories, ".jpg", ".gif", ".png")
				orderby s
				select s).ToArray();
		}

		public static string[] LoadDefaultPaperTextures()
		{
			return (from s in FileUtility.GetFiles(IniFile.GetDefaultLocations("Resources\\Textures\\Papers"), SearchOption.AllDirectories, ".jpg", ".gif", ".png")
				orderby s
				select s).ToArray();
		}

		public static void StartupProgress(string message, int progress)
		{
			Splash splash = Program.splash;
			if (splash != null)
			{
				splash.Message = splash.Message.AppendWithSeparator("\n", message);
				if (progress >= 0)
				{
					splash.Progress = progress;
				}
			}
		}

		public static IEnumerable<string> GetFavoritePaths()
		{
			return Settings.FavoriteFolders.Concat(Database.WatchFolders.Select((WatchFolder wf) => wf.Folder)).Distinct();
		}

		public static Image MakeBooksImage(IEnumerable<ComicBook> books, Size size, int maxImages, bool onlyMemory)
		{
			int num = books.Count();
			int num2 = Math.Min(maxImages, num);
			int num3 = size.Width / (num2 + 1);
			int height = size.Height - (num2 - 1) * 3;
			Bitmap bitmap = new Bitmap(size.Width, size.Height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				int num4 = 0;
				foreach (ComicBook item in books.Take(num2))
				{
					int num5 = num3 * num4;
					int num6 = num3 * (num4 + 2);
					ThumbnailKey frontCoverThumbnailKey = item.GetFrontCoverThumbnailKey();
					using (IItemLock<ThumbnailImage> itemLock = ImagePool.Thumbs.GetImage(frontCoverThumbnailKey, onlyMemory))
					{
						Image image = itemLock?.Item.Bitmap;
						if (image != null)
						{
							ThumbRenderer.DrawThumbnail(graphics, image, new Rectangle(num5, 3 * num4, num6 - num5, height), ThumbnailDrawingOptions.Default | ThumbnailDrawingOptions.KeepAspect, item);
						}
					}
					num4++;
				}
				if (num2 != num)
				{
					Color color = Color.FromArgb(192, SystemColors.Highlight);
					Font iconTitleFont = SystemFonts.IconTitleFont;
					string text = StringUtility.Format("{0} {1}", num, TR.Default["Books", "books"]);
					Rectangle rectangle = new Rectangle(Point.Empty, graphics.MeasureString(text, iconTitleFont).ToSize());
					rectangle.Inflate(4, 4);
					rectangle = rectangle.Align(new Rectangle(Point.Empty, size), ContentAlignment.MiddleCenter);
					using (GraphicsPath path = rectangle.ConvertToPath(5, 5))
					{
						using (Brush brush = new SolidBrush(color))
						{
							graphics.FillPath(brush, path);
						}
					}
					using (StringFormat format = new StringFormat
					{
						LineAlignment = StringAlignment.Center,
						Alignment = StringAlignment.Center
					})
					{
						graphics.DrawString(text, iconTitleFont, SystemBrushes.HighlightText, rectangle, format);
						return bitmap;
					}
				}
				return bitmap;
			}
		}

		public static bool ShowExplorer(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			try
			{
				if (File.Exists(path))
				{
					Process.Start("explorer.exe", $"/n,/select,\"{path}\"");
					return true;
				}
				if (Directory.Exists(path))
				{
					Process.Start("explorer.exe", $"\"{path}\"");
					return true;
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		public static bool RegisterFormats(string formats)
		{
			try
			{
				bool overwrite = formats.Contains("!");
				foreach (string item in formats.Remove("!").Split(',').RemoveEmpty())
				{
					bool flag = !item.Contains("-");
					string name = item.Remove("-");
					FileFormat fileFormat = Providers.Readers.GetSourceFormats().FirstOrDefault((FileFormat sf) => sf.Name == name);
					if (fileFormat != null)
					{
						if (flag)
						{
							fileFormat.RegisterShell("cYo.ComicRack", "eComic", overwrite);
						}
						else
						{
							fileFormat.UnregisterShell("cYo.ComicRack");
						}
					}
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
			finally
			{
				ShellRegister.RefreshShell();
			}
		}

		public static string LoadCustomThumbnail(string file, IWin32Window parent = null, string title = null)
		{
			string result = null;
			if (string.IsNullOrEmpty(file))
			{
				using (OpenFileDialog openFileDialog = new OpenFileDialog())
				{
					if (!string.IsNullOrEmpty(title))
					{
						openFileDialog.Title = title;
					}
					openFileDialog.Filter = TR.Load("FileFilter")["LoadThumbnail", "JPEG Image|*.jpg|Windows Bitmap Image|*.bmp|PNG Image|*.png|GIF Image|*.gif|TIFF Image|*.tif|Icon Image|*.ico"];
					openFileDialog.CheckFileExists = true;
					if (openFileDialog.ShowDialog(parent) == DialogResult.OK)
					{
						file = openFileDialog.FileName;
					}
				}
			}
			if (!string.IsNullOrEmpty(file))
			{
				string item = file;
				string text = null;
				try
				{
					if (Path.GetExtension(file).Equals(".ico", StringComparison.OrdinalIgnoreCase))
					{
						using (Bitmap bitmap = BitmapExtensions.LoadIcon(file, Color.Transparent))
						{
							file = (text = Path.GetTempFileName());
							bitmap.Save(text, ImageFormat.Png);
						}
					}
					using (Bitmap bmp = BitmapExtensions.BitmapFromFile(file))
					{
						result = ImagePool.AddCustomThumbnail(bmp);
					}
					Settings.ThumbnailFiles.UpdateMostRecent(item);
					return result;
				}
				catch (Exception ex)
				{
					Settings.ThumbnailFiles.Remove(file);
					MessageBox.Show(parent, string.Format(TR.Messages["CouldNotLoadThumbnail", "Could not load thumbnail!\nReason: {0}"], ex.Message), TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return result;
				}
				finally
				{
					if (text != null)
					{
						FileUtility.SafeDelete(text);
					}
				}
			}
			return result;
		}

        private static void RemoteServerStarted(object sender, NetworkManager.RemoteServerStartedEventArgs e)
		{
			CallMainForm("Remote Server Started", delegate
			{
				MainForm.OnRemoteServerStarted(e.Information);
			});
		}

		private static void RemoteServerStopped(object sender, NetworkManager.RemoteServerStoppedEventArgs e)
		{
			CallMainForm("Remote Server Stopped", delegate
			{
				MainForm.OnRemoteServerStopped(e.Address);
			});
		}

		private static void CallMainForm(string actionName, Action action)
		{
			ThreadUtility.RunInBackground(actionName, delegate
			{
				while (MainForm == null || !MainForm.IsInitialized)
				{
					if (MainForm != null && MainForm.IsDisposed)
					{
						return;
					}
					Thread.Sleep(1000);
				}
				if (!MainForm.InvokeIfRequired(action))
				{
					action();
				}
			});
		}

		private static void ScannerCheckFileIgnore(object sender, ComicScanNotifyEventArgs e)
		{
			if (Settings.DontAddRemoveFiles && Database.IsBlacklisted(e.File))
			{
				e.IgnoreFile = true;
			}
			if (ExtendedSettings.MacCompatibleScanning && Path.GetFileName(e.File).StartsWith("._"))
			{
				e.IgnoreFile = true;
			}
		}

		private static void IgnoredCoverImagesChanged(object sender, EventArgs e)
		{
			ComicInfo.CoverKeyFilter = Settings.IgnoredCoverImages;
		}

		private static void SystemEventsPowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			switch (e.Mode)
			{
			case PowerModes.Resume:
				NetworkManager.Start();
				break;
			case PowerModes.Suspend:
				NetworkManager.Stop();
				break;
			case PowerModes.StatusChange:
				break;
			}
		}

		private static void SetUICulture(string culture)
		{
			if (!string.IsNullOrEmpty(culture))
			{
				try
				{
					Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
				}
				catch (Exception)
				{
				}
			}
		}

		private static void StartNew(string[] args)
		{
			Thread.CurrentThread.Name = "GUI Thread";
			Diagnostic.StartWatchDog(CrashDialog.OnBark);
			ComicBookValueMatcher.RegisterMatcherType(typeof(ComicBookPluginMatcher));
			ComicBookValueMatcher.RegisterMatcherType(typeof(ComicBookExpressionMatcher));
			Settings = Settings.Load(defaultSettingsFile);
			Settings.RunCount++;
			CommandLineParser.Parse(ImageDisplayControl.HardwareSettings);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			DatabaseManager.FirstDatabaseAccess += delegate
			{
				StartupProgress(TR.Messages["OpenDatabase", "Opening Database"], -1);
			};
			DatabaseManager.BackgroundSaveInterval = ExtendedSettings.DatabaseBackgroundSaving;
			WirelessSyncProvider.StartListen();
			WirelessSyncProvider.ClientSyncRequest += (object s, WirelessSyncProvider.ClientSyncRequestArgs e) =>
            {
                if (MainForm != null)
                {
                    IPAddress address = s as IPAddress;
                    e.IsPaired = QueueManager.Devices.Any((DeviceSyncSettings d) => d.DeviceKey == e.Key);
                    if (e.IsPaired && address != null)
                    {
                        MainForm.BeginInvoke(delegate
                        {
                            QueueManager.SynchronizeDevice(e.Key, address);
                        });
                    }
                }
            };
			if (!ExtendedSettings.DisableAutoTuneSystem)
			{
				AutoTuneSystem();
			}
			ListExtensions.ParallelEnabled = EngineConfiguration.Default.EnableParallelQueries;
			if (EngineConfiguration.Default.IgnoredArticles != null)
			{
				StringUtility.Articles = EngineConfiguration.Default.IgnoredArticles;
			}
			ComicLibrary.QueryCacheMode = ExtendedSettings.QueryCacheMode;
			ComicLibrary.BackgroundQueryCacheUpdate = !ExtendedSettings.DisableBackgroundQueryCacheUpdate;
			ComicBook.EnableGroupNameCompression = ExtendedSettings.EnableGroupNameCompression;
			try
			{
				string text = ExtendedSettings.Language ?? Settings.CultureName;
				if (!string.IsNullOrEmpty(text))
				{
					SetUICulture(text);
					TR.DefaultCulture = new CultureInfo(text);
				}
			}
			catch (Exception)
			{
			}
			if (!ExtendedSettings.LoadDatabaseInForeground)
			{
				ThreadUtility.RunInBackground("Loading Database", delegate
				{
					InitializeDatabase(0, null);
				});
			}
			if (Settings.ShowSplash)
			{
				ManualResetEvent mre = new ManualResetEvent(initialState: false);
				ThreadUtility.RunInBackground("Splash Thread", delegate
				{
					splash = new Splash
					{
						Fade = true
					};
					splash.Location = splash.Bounds.Align(Screen.FromPoint(Settings.CurrentWorkspace.FormBounds.Location).Bounds, ContentAlignment.MiddleCenter).Location;
					splash.VisibleChanged += delegate
					{
						mre.Set();
					};
					splash.Closed += delegate
					{
						splash = null;
					};
					splash.ShowDialog();
				});
				mre.WaitOne(5000, exitContext: false);
			}
			try
			{
				if (ExtendedSettings.LoadDatabaseInForeground)
				{
					string text2 = TR.Messages["OpenDatabase", "Opening Database"];
					StartupProgress(text2, 0);
					InitializeDatabase(0, text2);
				}
				StartupProgress(TR.Messages["LoadCustomSettings", "Loading custom settings"], 20);
				IEnumerable<string> defaultLocations = IniFile.GetDefaultLocations("Resources\\Icons");
				ComicBook.PublisherIcons.AddRange(ZipFileFolder.CreateFromFiles(defaultLocations, "Publishers*.zip"), SplitIconKeysWithYear);
				ComicBook.AgeRatingIcons.AddRange(ZipFileFolder.CreateFromFiles(defaultLocations, "AgeRatings*.zip"), SplitIconKeys);
				ComicBook.FormatIcons.AddRange(ZipFileFolder.CreateFromFiles(defaultLocations, "Formats*.zip"), SplitIconKeys);
				ComicBook.SpecialIcons.AddRange(ZipFileFolder.CreateFromFiles(defaultLocations, "Special*.zip"), SplitIconKeys);
				ToolStripRenderer renderer;
				if (ExtendedSettings.SystemToolBars)
				{
					renderer = new ToolStripSystemRenderer();
				}
				else
				{
					bool flag = Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 5;
					ProfessionalColorTable professionalColorTable = ((!(ExtendedSettings.ForceTanColorSchema || flag)) ? ((ProfessionalColorTable)new OptimizedProfessionalColorTable()) : ((ProfessionalColorTable)new OptimizedTanColorTable()));
					renderer = new ToolStripProfessionalRenderer(professionalColorTable)
					{
						RoundedEdges = false
					};
				}
				ToolStripManager.Renderer = renderer;
				if (ExtendedSettings.DisableHardware)
				{
					ImageDisplayControl.HardwareAcceleration = ImageDisplayControl.HardwareAccelerationType.Disabled;
				}
				else
				{
					ImageDisplayControl.HardwareAcceleration = ((!ExtendedSettings.ForceHardware) ? ImageDisplayControl.HardwareAccelerationType.Enabled : ImageDisplayControl.HardwareAccelerationType.Forced);
				}
				if (ExtendedSettings.DisableMipMapping)
				{
					ImageDisplayControl.HardwareSettings.MipMapping = false;
				}
				Lists = new DefaultLists(() => Database.Books, IniFile.GetDefaultLocations("DefaultLists.txt"));
				StartupProgress(TR.Messages["InitCache", "Initialize Disk Caches"], 30);
				CacheManager = new CacheManager(DatabaseManager, Paths, Settings, Resources.ResourceManager);
				QueueManager = new QueueManager(DatabaseManager, CacheManager, Settings, Settings.Devices);
				QueueManager.ComicScanned += ScannerCheckFileIgnore;
				Settings.IgnoredCoverImagesChanged += IgnoredCoverImagesChanged;
				IgnoredCoverImagesChanged(null, EventArgs.Empty);
				SystemEvents.PowerModeChanged += SystemEventsPowerModeChanged;
			}
			catch (Exception ex2)
			{
				MessageBox.Show(StringUtility.Format(TR.Messages["FailedToInitialize", "Failed to initialize ComicRack: {0}"], ex2.Message));
				return;
			}
			StartupProgress(TR.Messages["ReadNewsFeed", "Reading News Feed"], 40);
			News = NewsStorage.Load(defaultNewsFile);
			if (News.Subscriptions.Count == 0)
			{
				News.Subscriptions.Add(new NewsStorage.Subscription(DefaultNewsFeed, "ComicRack News"));
			}
			StartupProgress(TR.Messages["CreateMainWindow", "Creating Main Window"], 50);
			if (ExtendedSettings.DisableScriptOptimization)
			{
				PythonCommand.Optimized = false;
			}
			if (ExtendedSettings.ShowScriptConsole)
			{
				ScriptConsole = new ScriptOutputForm();
				TextBoxStream logOutput = (TextBoxStream)(PythonCommand.Output = new TextBoxStream(ScriptConsole.Log));
				PythonCommand.EnableLog = true;
				WebComic.SetLogOutput(logOutput);
				ScriptConsole.Show();
			}
			NetworkManager = new NetworkManager(DatabaseManager, CacheManager, Settings, ExtendedSettings.PrivateServerPort, ExtendedSettings.InternetServerPort, ExtendedSettings.DisableBroadcast);
			MainForm = new MainForm();
			MainForm.FormClosed += MainFormFormClosed;
			MainForm.FormClosing += (object s, FormClosingEventArgs e) =>
            {
                bool flag2 = e.CloseReason == CloseReason.UserClosing;
                foreach (Command command in ScriptUtility.GetCommands("Shutdown"))
                {
                    try
                    {
                        if (!(bool)command.Invoke(new object[1]
                        {
                            flag2
                        }) && flag2)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            };
			Application.AddMessageFilter(new MouseWheelDelegater());
			MainForm.Show();
			MainForm.Update();
			MainForm.Activate();
			if (splash != null)
			{
				splash.Invoke(splash.Close);
			}
			ThreadUtility.RunInBackground("Starting Network", NetworkManager.Start);
			ThreadUtility.RunInBackground("Generate Language Pack Info", delegate
			{
				int num = InstalledLanguages.Length;
			});
			if (!string.IsNullOrEmpty(DatabaseManager.OpenMessage))
			{
				MessageBox.Show(MainForm, DatabaseManager.OpenMessage, TR.Messages["Attention", "Attention"], MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			if (Settings.NewsStartup)
			{
				MainForm.ShowNews(always: false);
			}
			Application.Run(MainForm);
		}

		private static IEnumerable<string> SplitIconKeys(string value)
		{
			return value.Split(',', '#');
		}

		private static IEnumerable<string> SplitIconKeysWithYear(string value)
		{
			foreach (string key in SplitIconKeys(value))
			{
				Match j = yearRegex.Match(key);
				if (!j.Success)
				{
					yield return key;
					continue;
				}
				int num = int.Parse(j.Groups["start"].Value);
				int end = int.Parse(j.Groups["end"].Value);
				string key2 = yearRegex.Replace(key, string.Empty);
				for (int i = num; i <= end; i++)
				{
					yield return key2 + "(" + i + ")";
				}
			}
		}

		private static bool InitializeDatabase(int startPercent, string readDbMessage)
		{
			return DatabaseManager.Open(Paths.DatabasePath, ExtendedSettings.DataSource, ExtendedSettings.DoNotLoadQueryCaches, string.IsNullOrEmpty(readDbMessage) ? null : ((Action<int>)((int percent) =>
            {
                StartupProgress(readDbMessage, startPercent + percent / 5);
            })));
		}

		private static void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
			AutomaticProgressDialog.Process(null, TR.Messages["ClosingComicRack", "Closing ComicRack"], TR.Messages["SaveAllData", "Saving all Data to Disk..."], 1500, CleanUp, AutomaticProgressDialogOptions.None);
		}

		private static void CleanUp()
		{
			try
			{
				NetworkManager.Dispose();
				SystemEvents.PowerModeChanged -= SystemEventsPowerModeChanged;
				QueueManager.Dispose();
				News.Save(defaultNewsFile);
				Settings.Save(defaultSettingsFile);
				ImagePool.Dispose();
				DatabaseManager.Dispose();
			}
			catch (Exception ex)
			{
				MessageBox.Show(StringUtility.Format(TR.Messages["ErrorShutDown", "There was an error shutting down the application:\r\n{0}"], ex.Message), TR.Messages["Error", "Error"], MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private static void StartLast(string[] args)
		{
			ExtendedSettings sw = default(ExtendedSettings);
			MainForm.BeginInvoke(delegate
			{
				MainForm.RestoreToFront();
				try
				{
					sw = new ExtendedSettings();
					IEnumerable<string> enumerable = CommandLineParser.Parse(sw, args);
					if (!string.IsNullOrEmpty(sw.ImportList))
					{
						MainForm.ImportComicList(sw.ImportList);
					}
					if (!string.IsNullOrEmpty(sw.InstallPlugin))
					{
						MainForm.ShowPreferences(sw.InstallPlugin);
					}
					if (enumerable.Any())
					{
						enumerable.ForEach((string file) =>
                        {
                            MainForm.OpenSupportedFile(file, newSlot: true, sw.Page, fromShell: true);
                        });
					}
				}
				catch (Exception)
				{
				}
			});
		}

		private static void AutoTuneSystem()
		{
			if (ExtendedSettings.IsQueryCacheModeDefault && EngineConfiguration.Default.IsEnableParallelQueriesDefault && ImageDisplayControl.HardwareSettings.IsMaxTextureMemoryMBDefault && ImageDisplayControl.HardwareSettings.IsTextureManagerOptionsDefault)
			{
				int processorCount = Environment.ProcessorCount;
				int num = (int)(MemoryInfo.InstalledPhysicalMemory / 1024 / 1024);
				int cpuSpeedInHz = MemoryInfo.CpuSpeedInHz;
				if (num <= 512)
				{
					ExtendedSettings.QueryCacheMode = QueryCacheMode.Disabled;
				}
				EngineConfiguration.Default.EnableParallelQueries = processorCount > 1;
				if (cpuSpeedInHz > 2000)
				{
					ExtendedSettings.OptimizedListScrolling = false;
				}
				ImageDisplayControl.HardwareSettings.MaxTextureMemoryMB = (num / 8).Clamp(32, 256);
				if (ImageDisplayControl.HardwareSettings.MaxTextureMemoryMB <= 64)
				{
					ImageDisplayControl.HardwareSettings.TextureManagerOptions |= TextureManagerOptions.BigTexturesAs16Bit;
					ImageDisplayControl.HardwareSettings.TextureManagerOptions &= ~TextureManagerOptions.MipMapFilter;
				}
			}
		}

		[DllImport("user32.dll")]
		private static extern bool SetProcessDPIAware();

		[STAThread]
		private static int Main(string[] args)
		{
			SetProcessDPIAware();
			ServicePointManager.Expect100Continue = false;
			if (ExtendedSettings.WaitPid != 0)
			{
				try
				{
					Process.GetProcessById(ExtendedSettings.WaitPid).WaitForExit(30000);
				}
				catch
				{
				}
			}
			if (!string.IsNullOrEmpty(ExtendedSettings.RegisterFormats))
			{
				if (!RegisterFormats(ExtendedSettings.RegisterFormats))
				{
					return 1;
				}
				return 0;
			}
			TR.ResourceFolder = new PackedLocalize(TR.ResourceFolder);
			Control.CheckForIllegalCrossThreadCalls = false;
			ItemMonitor.CatchThreadInterruptException = true;
			SingleInstance singleInstance = new SingleInstance("ComicRackSingleInstance", StartNew, StartLast);
			singleInstance.Run(args);
			if (Restart)
			{
				Application.Exit();
				Process.Start(Application.ExecutablePath, "-restart -waitpid " + Process.GetCurrentProcess().Id + " " + Environment.CommandLine);
			}
			return 0;
		}
	}
}
