using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.ComponentModel;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookMetadata
	{
		private readonly IComparer<IViewableItem> comparer;
		private readonly IGrouper<IViewableItem> grouper;
		public ComicBookMetadata(int id, string name, IComparer<IViewableItem> comparer = null, IGrouper<IViewableItem> grouper = null)
		{
			this.Id = id;
			this.Name = name;
			this.comparer = comparer;
			this.grouper = grouper;
		}

		public int Id { get; }
		public string Name { get; }

		public IComparer<T> GetComparer<T>()
		{
			return typeof(T) switch
			{
				Type comicBookType when comicBookType == typeof(ComicBook) => (comparer as IComicBookComparer)?.Comparer as IComparer<T>,
				Type viewableItem when viewableItem == typeof(IViewableItem) => comparer as IComparer<T>,
				_ => null,
			};
		}

		public IGrouper<T> GetGrouper<T>()
		{
			return typeof(T) switch
			{
				Type comicBookType when comicBookType == typeof(ComicBook) => (grouper as IBookGrouper)?.BookGrouper as IGrouper<T>,
				Type viewableItem when viewableItem == typeof(IViewableItem) => grouper as IGrouper<T>,
				_ => null,
			};
		}
	}
}
