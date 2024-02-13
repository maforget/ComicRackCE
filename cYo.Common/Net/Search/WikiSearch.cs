using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using cYo.Common.Properties;

namespace cYo.Common.Net.Search
{
	public class WikiSearch : CachedSearch
	{
		private const string Wiki = "http://en.wikipedia.org";

		private static readonly Regex rx = new Regex("\\\"(?<text>.*?)\\\"", RegexOptions.Compiled);

		private static readonly Image image = Resources.Wikipedia;

		public override string Name => "Wikipedia";

		public override Image Image => image;

		protected override IEnumerable<SearchResult> OnSearch(string hint, string text, int limit)
		{
			string uri = $"{Wiki}/w/api.php?action=opensearch&limit={limit}&search={WebUtility.UrlEncode(text)}";
			return from m in rx.Matches(HttpAccess.ReadText(uri)).OfType<Match>().Skip(1)
				select m.Groups["text"].Value into t
				select new SearchResult
				{
					Name = t,
					Result = $"{Wiki}/wiki/{t.Replace(" ", "_")}"
                };
		}

		protected override string OnGenericSearchLink(string hint, string text)
		{
			return $"{Wiki}/w/index.php?title=Special:Search&fulltext=Search&search={WebUtility.UrlEncode(text)}";
		}
	}
}
