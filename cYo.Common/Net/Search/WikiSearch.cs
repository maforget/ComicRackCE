using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using cYo.Common.Properties;

namespace cYo.Common.Net.Search
{
	public class WikiSearch : CachedSearch
	{
		private const string Wiki = "http://en.wikipedia.org";

		private static readonly Image image = Resources.Wikipedia;

		public override string Name => "Wikipedia";

		public override Image Image => image;

		protected override IEnumerable<SearchResult> OnSearch(string hint, string text, int limit)
		{
			string uri = $"{Wiki}/w/api.php?action=opensearch&limit={limit}&search={WebUtility.UrlEncode(text)}";
			return ParseOpenSearch(HttpAccess.ReadText(uri));
		}

		// OpenSearch answers [search text, [titles], [descriptions], [links]]. The descriptions are always empty now.
		private static List<SearchResult> ParseOpenSearch(string reply)
		{
			using (JsonDocument document = JsonDocument.Parse(reply))
			{
				JsonElement root = document.RootElement;
				if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() < 2)
				{
					return new List<SearchResult>();
				}
				string[] titles = root[1].EnumerateArray().Select(title => title.GetString()).ToArray();
				string[] links = root.GetArrayLength() > 3 ? root[3].EnumerateArray().Select(link => link.GetString()).ToArray() : new string[0];
				return titles.Select((title, i) => new SearchResult
				{
					Name = title,
					Result = i < links.Length ? links[i] : $"{Wiki}/wiki/{title.Replace(" ", "_")}"
				}).ToList();
			}
		}

		protected override string OnGenericSearchLink(string hint, string text)
		{
			return $"{Wiki}/w/index.php?title=Special:Search&fulltext=Search&search={WebUtility.UrlEncode(text)}";
		}
	}
}
