using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookIdComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			return x.Id.CompareTo(y.Id);
		}
	}
}
