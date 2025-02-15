using System.Collections.Generic;

namespace cYo.Projects.ComicRack.Engine
{
	public abstract class ScanItem
	{
		public bool AutoRemove
		{
			get;
			set;
		}

		public bool ForceRefreshInfo { get; set; }

		public abstract IEnumerable<string> GetScanFiles();
	}
}
