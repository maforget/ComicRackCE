using System.Collections.Generic;
using System.ComponentModel;
using cYo.Common.Mathematics;
using cYo.Common.Runtime;
using cYo.Projects.ComicRack.Engine.Database;
using cYo.Projects.ComicRack.Engine.IO.Network;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	public class ExtendedSettings
	{
		private int comicCountAlpha = 64;

		[CommandLineSwitch(ShortName = "rf")]
		[DefaultValue(null)]
		public string RegisterFormats
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "restart")]
		[DefaultValue(false)]
		public bool Restart
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "waitpid")]
		[DefaultValue(0)]
		public int WaitPid
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "dats")]
		[DefaultValue(false)]
		public bool DisableAutoTuneSystem
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "dfv")]
		[DefaultValue(false)]
		public bool DisableFoldersView
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "qcm")]
		[DefaultValue(QueryCacheMode.InstantUpdate)]
		public QueryCacheMode QueryCacheMode
		{
			get;
			set;
		}

		public bool IsQueryCacheModeDefault => QueryCacheMode == QueryCacheMode.InstantUpdate;

		[CommandLineSwitch(ShortName = "dnlqc")]
		[DefaultValue(false)]
		public bool DoNotLoadQueryCaches
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "dbqcu")]
		[DefaultValue(true)]
		public bool DisableBackgroundQueryCacheUpdate
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool EnableGroupNameCompression
		{
			get;
			set;
		}

		[DefaultValue(false)]
		[CommandLineSwitch(ShortName = "stb")]
		public bool SystemToolBars
		{
			get;
			set;
		}

		[DefaultValue(false)]
		[CommandLineSwitch(ShortName = "ftcs")]
		public bool ForceTanColorSchema
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool MacCompatibleScanning
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "ssc")]
		[DefaultValue(false)]
		public bool ShowScriptConsole
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "dso")]
		[DefaultValue(false)]
		public bool DisableScriptOptimization
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "schk")]
		[DefaultValue(false)]
		public bool ShowContextHelpKey
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "ds")]
		public string DataSource
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "dbs")]
		[DefaultValue(600)]
		public int DatabaseBackgroundSaving
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "ldif")]
		[DefaultValue(false)]
		public bool LoadDatabaseInForeground
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "ac")]
		[IniFile(false)]
		public string AlternateConfig
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "l")]
		[IniFile(false)]
		public string Language
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "db")]
		public string DatabasePath
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "cp")]
		public string CachePath
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "lm")]
		public int LimitMemory
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "cdb")]
		public bool ConsolidateDatabase
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "il")]
		[IniFile(false)]
		[DefaultValue(null)]
		public string ImportList
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "ip")]
		[IniFile(false)]
		[DefaultValue(null)]
		public string InstallPlugin
		{
			get;
			set;
		}

		[CommandLineFiles]
		[IniFile(false)]
		public IEnumerable<string> Files
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "ws")]
		[IniFile(false)]
		[DefaultValue(null)]
		public string Workspace
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "p")]
		[IniFile(false)]
		[DefaultValue(0)]
		public int Page
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "hwd")]
		public bool DisableHardware
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "hwf")]
		public bool ForceHardware
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "hwdmm")]
		public bool DisableMipMapping
		{
			get;
			set;
		}

		[DefaultValue(0.5f)]
		public float KeyboardZoomStepping
		{
			get;
			set;
		}

		[DefaultValue(0.25f)]
		public float AnamorphicScalingTolerance
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "dbr")]
		public bool DisableBroadcast
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "isp")]
		public int InternetServerPort
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "psp")]
		public int PrivateServerPort
		{
			get;
			set;
		}

		[CommandLineSwitch(ShortName = "orc")]
		[IniFile(false)]
		public bool OwnRemoteConnect
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool DisableMenuHideShowAnimation
		{
			get;
			set;
		}

		[DefaultValue(25)]
		public int ListMenuSize
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool MouseSwitchesToFullLibrary
		{
			get;
			set;
		}

		[DefaultValue(0.6f)]
		public float DragDropCursorAlpha
		{
			get;
			set;
		}

		[DefaultValue(5000)]
		public int AutoHideCursorDuration
		{
			get;
			set;
		}

		[DefaultValue(64)]
		public int ComicCountAlpha
		{
			get
			{
				return comicCountAlpha;
			}
			set
			{
				comicCountAlpha = value.Clamp(0, 255);
			}
		}

		[DefaultValue(10)]
		public int QuickOpenListSize
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool ReplaceDefaultListsInQuickOpen
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool RemoteLibrariesInQuickOpen
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool OnlyLocalRemoteLibrariesInQuickOpen
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool HideBrowserIfShellOpen
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool DisableListSpinButtons
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool OptimizedListScrolling
		{
			get;
			set;
		}

		[DefaultValue(false)]
		[CommandLineSwitch(ShortName = "aclf")]
		public bool AllowCopyListFolders
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool DoNotResetZoomOnBookOpen
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool ShowCustomScriptValues
		{
			get;
			set;
		}

		[DefaultValue(true)]
		public bool SortNetworkFolders
		{
			get;
			set;
		}

        [DefaultValue(false)]
        [CommandLineSwitch(ShortName = "local")]
        public bool UseLocalSettings 
		{ 
			get; 
			set; 
		}

		[DefaultValue(false)]
		[CommandLineSwitch(ShortName = "hidden")]
		public bool StartHidden
		{
			get;
			set;
		}

		public ExtendedSettings()
		{
			AnamorphicScalingTolerance = 0.25f;
			KeyboardZoomStepping = 0.5f;
			DatabaseBackgroundSaving = 600;
			InternetServerPort = ComicLibraryServerConfig.DefaultPublicServicePort;
			PrivateServerPort = ComicLibraryServerConfig.DefaultPrivateServicePort;
			AutoHideCursorDuration = 5000;
			HideBrowserIfShellOpen = true;
			DragDropCursorAlpha = 0.6f;
			ListMenuSize = 25;
			OptimizedListScrolling = true;
			QuickOpenListSize = 10;
			RemoteLibrariesInQuickOpen = true;
			OnlyLocalRemoteLibrariesInQuickOpen = true;
			QueryCacheMode = QueryCacheMode.InstantUpdate;
			MacCompatibleScanning = true;
			SortNetworkFolders = true;
			StartHidden = false;
        }
	}
}
