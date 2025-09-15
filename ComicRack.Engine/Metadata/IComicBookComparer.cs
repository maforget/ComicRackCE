using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Projects.ComicRack.Engine
{
	/// <summary>
	/// Used to expose a comparer for ComicBook objects.
	/// </summary>
	public interface IComicBookComparer
	{
		public IComparer<ComicBook> Comparer { get; }
	}
}
