using System;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class WatchFolderCollection : SmartList<WatchFolder>
	{
		public IEnumerable<string> Folders => this.Select((WatchFolder wf) => wf.Folder);

		public WatchFolderCollection()
		{
			base.Flags |= SmartListOptions.DisposeOnRemove;
		}
	}
}
