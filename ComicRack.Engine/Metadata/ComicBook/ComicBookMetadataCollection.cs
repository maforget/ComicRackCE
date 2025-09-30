using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookMetadataCollection : SmartList<ComicBookMetadata>
	{
		public ComicBookMetadata FindById(int id)
		{
			return Find(h => h.Id == id);
		}

		public ComicBookMetadata FindBySorter(IComparer<ComicBook> comp)
		{
			if (comp != null)
				return Find(h => h.GetComparer<ComicBook>() == comp);

			return default;
		}

		public ComicBookMetadata FindByGrouper(IGrouper<ComicBook> comp)
		{
			if (comp != null)
				return Find(h => h.GetGrouper<ComicBook>() == comp);

			return default;
		}
	}
}
