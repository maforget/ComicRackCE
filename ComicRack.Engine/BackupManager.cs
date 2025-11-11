using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
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
		Cache = 1 << 6,
		Full = 0x3F, // all except for cache
		FullWithCache = 0x7F, // includes all the above, same as above but includes cache

	public class BackupManager: DisposableObject
	{
		public BackupManagerOptions Options { get; }

		public BackupManager(BackupManagerOptions options, SystemPaths paths)
		{
			this.Options = options;
		}
	}

	public class BackupManagerOptions
	{
		public string Location { get; set; } = null;

		[DefaultValue(BackupOptions.None)]
		public BackupOptions Options { get; set; } = BackupOptions.None;

		/// <summary>
		/// Includes the <see cref="BackupOptions"/> from Alternate Configurations<br/>
		/// Otherwise will only backup the current config
		/// </summary>
		/// 
		[DefaultValue(true)]
		public bool IncludeAlternateConfig { get; set; } = true;

		[DefaultValue(5)]
		public int BackupsToKeep { get; set; } = 5;

		public TimeSpan Interval { get; set; } = TimeSpan.FromDays(7);

		public DateTime LastBackupDate { get; set; } = DateTime.MinValue;
	}
}
