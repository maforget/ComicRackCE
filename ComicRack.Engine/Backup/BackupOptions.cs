using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Backup
{
	[Flags]
	public enum BackupOptions
	{
		None = 0,
		Database = 1 << 0,
		Config = 1 << 1,
		ComicRackINI = 1 << 2,
		DefaultLists = 1 << 3,
		Scripts = 1 << 4,
		Resources = 1 << 5,
		CustomThumbnails = 1 << 6,
		Cache = 1 << 7,
		Full = 0x7F,
		FullWithCache = 0xFF,
	}

	/// <summary>
	/// Options saved in the Settings
	/// </summary>
	public class BackupManagerOptions
	{
		public string Location { get; set; } = null;

		[DefaultValue(BackupOptions.Full)]
		public BackupOptions Options { get; set; } = BackupOptions.Full;

		/// <summary>
		/// Includes the <see cref="BackupOptions"/> from Alternate Configurations<br/>
		/// Otherwise will only backup the current config
		/// </summary>
		[DefaultValue(false)]
		public bool IncludeAllAlternateConfigs { get; set; } = false;

		[DefaultValue(5)]
		public int BackupsToKeep { get; set; } = 5;

		[DefaultValue (false)]
		public bool OnStartup { get; set; } = false;

		[DefaultValue(false)]
		public bool OnExit { get; set; } = false;
	}

}
