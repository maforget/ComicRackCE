using System.Collections.Generic;
using cYo.Common.Net.Search;

namespace cYo.Projects.ComicRack.Viewer
{
	public static class SearchEngines
	{
		public static readonly IList<INetSearch> Engines = new List<INetSearch>(new WikiSearch[1]
		{
			new WikiSearch()
		});
	}
}
