using System.Collections.Generic;
using System.Drawing;

namespace cYo.Common.Net.Search
{
	public interface INetSearch
	{
		string Name
		{
			get;
		}

		Image Image
		{
			get;
		}

		IEnumerable<SearchResult> Search(string hint, string text, int limit);

		string GenericSearchLink(string hint, string text);
	}
}
