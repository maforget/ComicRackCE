using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[XmlInclude(typeof(ComicIdListItem))]
	[XmlInclude(typeof(ComicLibraryListItem))]
	[XmlInclude(typeof(ComicSmartListItem))]
	[XmlInclude(typeof(ComicListItemFolder))]
	public class ComicListItemCollection : SmartList<ComicListItem>
	{
		public ComicListItemCollection()
			: base(SmartListOptions.Default | SmartListOptions.DisposeOnRemove)
		{
		}

		public IEnumerable<T> GetItems<T>(bool bottomUp = false) where T : ComicListItem
		{
			return this.Recurse<T>((object o) => (!(o is ComicListItemFolder)) ? null : ((ComicListItemFolder)o).Items, bottomUp);
		}

		public int GetChildLevel<T>(T cli) where T : ComicListItem
		{
			return this.GetChildLevel(cli, (object o) => (!(o is ComicListItemFolder)) ? null : ((ComicListItemFolder)o).Items, 0);
		}

		public ComicListItem FindItem(Guid id)
		{
			return GetItems<ComicListItem>().FirstOrDefault((ComicListItem cli) => cli.Id == id);
		}
	}
}
