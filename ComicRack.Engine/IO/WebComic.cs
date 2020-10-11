using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.Net;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Projects.ComicRack.Engine.IO.Cache;
using cYo.Projects.ComicRack.Engine.IO.Provider.Readers;

namespace cYo.Projects.ComicRack.Engine.IO
{
	public class WebComic
	{
		public class PageCompositing
		{
			[XmlAttribute]
			[DefaultValue(0)]
			public int PageWidth
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(0)]
			public int PageHeight
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(1)]
			public int Rows
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(1)]
			public int Columns
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(0)]
			public int BorderWidth
			{
				get;
				set;
			}

			[XmlIgnore]
			[XmlAttribute]
			public string BackgroundColor
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool RightToLeft
			{
				get;
				set;
			}

			public Size PageSize => new Size(PageWidth, PageHeight);

			public bool IsEmpty
			{
				get
				{
					if (Columns >= 1 && Rows >= 1)
					{
						if (Columns == 1 && Rows == 1 && BorderWidth == 0)
						{
							return PageSize.IsEmpty;
						}
						return false;
					}
					return true;
				}
			}

			public Color BackColor
			{
				get
				{
					if (string.IsNullOrEmpty(BackgroundColor))
					{
						return Color.White;
					}
					try
					{
						return ColorTranslator.FromHtml(BackgroundColor);
					}
					catch
					{
						return Color.White;
					}
				}
			}

			public PageCompositing()
			{
				Rows = (Columns = 1);
			}

			public PageCompositing(PageCompositing pc)
			{
				Rows = pc.Rows;
				Columns = pc.Columns;
				PageWidth = pc.PageWidth;
				PageHeight = pc.PageHeight;
				BorderWidth = pc.BorderWidth;
				BackgroundColor = pc.BackgroundColor;
				RightToLeft = pc.RightToLeft;
			}
		}

		public enum PageLinkType
		{
			Url,
			BrowseScraper,
			IndexScraper
		}

		public class PagePart
		{
			[XmlAttribute]
			[DefaultValue(int.MaxValue)]
			public int MaximumMatches
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool AddOwn
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool Reverse
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool Sort
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(null)]
			public string Cut
			{
				get;
				set;
			}

			[XmlText]
			public string Pattern
			{
				get;
				set;
			}

			public PagePart()
			{
				MaximumMatches = int.MaxValue;
			}
		}

		public class PagePartCollection : List<PagePart>
		{
		}

		public class PageLink
		{
			private PagePartCollection parts;

			[XmlAttribute]
			public string Url
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(PageLinkType.Url)]
			public PageLinkType Type
			{
				get;
				set;
			}

			[XmlArrayItem("Part")]
			public PagePartCollection Parts => parts ?? (parts = new PagePartCollection());

			[DefaultValue(null)]
			public PageCompositing Compositing
			{
				get;
				set;
			}

			[XmlIgnore]
			public int Left
			{
				get;
				set;
			}

			[XmlIgnore]
			public int Top
			{
				get;
				set;
			}

			[XmlIgnore]
			public bool PartSpecified
			{
				get
				{
					if (parts != null)
					{
						return parts.Count > 0;
					}
					return false;
				}
			}
		}

		public class PageLinkCollection : List<PageLink>
		{
		}

		public class ValuePairCollection : List<ValuePair<string, string>>
		{
		}

		public class WebComicImage
		{
			public Size PageSize
			{
				get;
				set;
			}

			public int Rows
			{
				get;
				set;
			}

			public int Columns
			{
				get;
				set;
			}

			public List<PageLink> Urls
			{
				get;
				private set;
			}

			public PageCompositing Compositing
			{
				get;
				set;
			}

			public string Name => Path.GetFileName(Urls[0].Url);

			public WebComicImage()
			{
				Urls = new List<PageLink>();
			}
		}

		private static readonly Regex rxNumbering;

		private ValuePairCollection variables;

		private PageLinkCollection images;

		private static readonly Dictionary<string, Regex> rxCache;

		private static readonly Regex rxBaseMatcher;

		private static Stream logStream;

		[DefaultValue(null)]
		public ComicInfo Info
		{
			get;
			set;
		}

		[XmlArray("Variables")]
		[XmlArrayItem("Variable")]
		public ValuePairCollection Variables => variables ?? (variables = new ValuePairCollection());

		[XmlArrayItem("Image")]
		public PageLinkCollection Images => images ?? (images = new PageLinkCollection());

		public PageCompositing Compositing
		{
			get;
			set;
		}

		[XmlIgnore]
		public bool ImagesSpecified
		{
			get
			{
				if (images != null)
				{
					return images.Count > 0;
				}
				return false;
			}
		}

		[XmlIgnore]
		public bool VariablesSpecified
		{
			get
			{
				if (variables != null)
				{
					return variables.Count > 0;
				}
				return false;
			}
		}

		static WebComic()
		{
			rxNumbering = new Regex("\\[(?<format>\\d+):(?<start>\\d+)-(?<end>\\d+)\\]", RegexOptions.Compiled);
			rxCache = new Dictionary<string, Regex>();
			rxBaseMatcher = new Regex("<base\\s+?href\\s*?=\\s*?\"(?<base>.+?)\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			try
			{
				MethodInfo method = typeof(UriParser).GetMethod("GetSyntax", BindingFlags.Static | BindingFlags.NonPublic);
				FieldInfo field = typeof(UriParser).GetField("m_Flags", BindingFlags.Instance | BindingFlags.NonPublic);
				if (!(method != null) || !(field != null))
				{
					return;
				}
				string[] array = new string[2]
				{
					"http",
					"https"
				};
				foreach (string text in array)
				{
					UriParser uriParser = (UriParser)method.Invoke(null, new object[1]
					{
						text
					});
					if (uriParser != null)
					{
						int num = (int)field.GetValue(uriParser);
						if (((uint)num & 0x1000000u) != 0)
						{
							field.SetValue(uriParser, num & -16777217);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public WebComic()
		{
			Compositing = new PageCompositing();
		}

		public IEnumerable<WebComicImage> GetParsedImages(bool refresh)
		{
			WebComicImage webComicImage = new WebComicImage
			{
				Compositing = new PageCompositing(Compositing)
			};
			int num = 0;
			int num2 = 0;
			int rh = 0;
			foreach (PageLink pl in images)
			{
				if (pl.Compositing != null)
				{
					if (webComicImage.Urls.Count != 0)
					{
						yield return webComicImage;
					}
					Compositing = pl.Compositing;
					webComicImage = new WebComicImage
					{
						Compositing = new PageCompositing(Compositing)
					};
					int num3;
					rh = (num3 = 0);
					num = (num2 = num3);
				}
				foreach (PageLink p in GetImages(pl, refresh))
				{
					if (Compositing.PageSize.IsEmpty)
					{
						int num4 = Math.Max(1, Compositing.Rows) * Math.Max(1, Compositing.Columns);
						webComicImage.Urls.Add(p);
						if (webComicImage.Urls.Count >= num4)
						{
							yield return webComicImage;
							webComicImage = new WebComicImage
							{
								Compositing = new PageCompositing(Compositing)
							};
							int num3;
							rh = (num3 = 0);
							num = (num2 = num3);
						}
						continue;
					}
					using (Bitmap bmp = WebComicProvider.BitmapFromBytes(p.Url, refresh))
					{
						if (bmp == null)
						{
							continue;
						}
						webComicImage.Compositing.PageWidth = Math.Max(webComicImage.Compositing.PageWidth, bmp.Width);
						webComicImage.Compositing.PageHeight = Math.Max(webComicImage.Compositing.PageHeight, bmp.Height);
						while (num + bmp.Width > webComicImage.Compositing.PageWidth)
						{
							if (num2 + bmp.Height <= webComicImage.Compositing.PageHeight)
							{
								num2 += bmp.Height;
								num = 0;
								rh = bmp.Height;
							}
							else
							{
								yield return webComicImage;
								webComicImage = new WebComicImage
								{
									Compositing = new PageCompositing(Compositing)
								};
								num = 0;
								num2 = 0;
							}
						}
						p.Left = num;
						p.Top = num2;
						rh = Math.Max(rh, bmp.Height);
						num += bmp.Width;
						webComicImage.Urls.Add(p);
					}
				}
			}
			if (webComicImage.Urls.Count != 0)
			{
				yield return webComicImage;
			}
		}

		protected IEnumerable<PageLink> GetImages(PageLink p, bool refresh)
		{
			LogSeparator();
			Log("Parsing image: {0}", p.Url);
			PagePartCollection parts = new PagePartCollection();
			parts.AddRange(from ip in p.Url.Split('|')
				select new PagePart
				{
					Pattern = ip
				});
			parts.AddRange(p.Parts);
			foreach (PagePart ip2 in parts)
			{
				variables.ForEach(delegate(ValuePair<string, string> v)
				{
					ip2.Pattern = ip2.Pattern.Replace("{" + v.Key + "}", v.Value);
				});
			}
			if (HasOption(parts[0], "!"))
			{
				p.Type = PageLinkType.IndexScraper;
			}
			else if (HasOption(parts[0], "?"))
			{
				p.Type = PageLinkType.BrowseScraper;
			}
			string s = parts[0].Pattern;
			switch (p.Type)
			{
			case PageLinkType.BrowseScraper:
				Log("Browse Scraper: {0}", s);
				foreach (PageLink browseScraperPage in GetBrowseScraperPages(refresh, parts.ToArray()))
				{
					yield return browseScraperPage;
				}
				yield break;
			case PageLinkType.IndexScraper:
				Log("Index Scraper: {0}", s);
				foreach (PageLink indexScraperPage in GetIndexScraperPages(refresh, parts.ToArray()))
				{
					yield return indexScraperPage;
				}
				yield break;
			}
			Match j = rxNumbering.Match(s);
			if (!j.Success)
			{
				Log("Page Link: {0}", s);
				yield return new PageLink
				{
					Url = s
				};
				yield break;
			}
			string format = "{0:" + j.Groups["format"].Value + "}";
			int num = int.Parse(j.Groups["start"].Value);
			int end = int.Parse(j.Groups["end"].Value);
			Log("Multi Page Link: {0}", s);
			for (int i = num; i <= end; i++)
			{
				string text = rxNumbering.Replace(s, string.Format(format, i));
				Log("Link {0}: {1}", i, text);
				yield return new PageLink
				{
					Url = text
				};
			}
		}

		public static IEnumerable<PageLink> GetIndexScraperPages(bool refresh, params PagePart[] patterns)
		{
			return GetIndexScraperPages(refresh, patterns[0].Pattern, ReadText(patterns[0].Pattern, refresh: true), patterns.Skip(1), new HashSet<string>(StringComparer.OrdinalIgnoreCase));
		}

		public static IEnumerable<PageLink> GetIndexScraperPages(bool refresh, string url, string pageText, IEnumerable<PagePart> parts, HashSet<string> trackback)
		{
			if (string.IsNullOrEmpty(pageText))
			{
				yield break;
			}
			Log(delegate(TextWriter t)
			{
				t.WriteLine("Parts: {0} [{1}]", parts.Count(), parts.ToListString(", "));
			});
			int num = parts.Count();
			bool end = num == 1;
			PagePart pagePart = parts.First();
			pagePart.Reverse |= HasOption(pagePart, "!");
			pagePart.AddOwn |= HasOption(pagePart, "+");
			bool reverse = pagePart.Reverse;
			bool flag = pagePart.AddOwn && num > 1;
			bool sort = pagePart.Sort;
			Uri uri = new Uri(url);
			string headerBaseUri = GetHeaderBaseUri(pageText);
			IEnumerable<string> matches = (from m in MatchesRegex(pageText, pagePart)
				select MakeAbsolute(uri, m, headerBaseUri).ToString() into m
				where !trackback.Contains(m)
				select m).Distinct();
			Log(delegate(TextWriter t)
			{
				if (matches.Count() == 0)
				{
					t.WriteLine("No Matches!");
				}
			});
			if (flag)
			{
				matches = matches.AddFirst(url).Distinct();
			}
			if (sort)
			{
				matches = matches.OrderBy((string u) => u, StringComparer.OrdinalIgnoreCase);
			}
			if (reverse)
			{
				matches = matches.Reverse();
			}
			foreach (string u2 in matches)
			{
				trackback.Add(u2);
				if (end)
				{
					Log("Final Match: {0}", u2);
					yield return new PageLink
					{
						Url = u2
					};
					continue;
				}
				Log("Match Next Page: {0}", u2);
				foreach (PageLink indexScraperPage in GetIndexScraperPages(refresh, u2, ReadText(u2, refresh), parts.Skip(1), trackback))
				{
					yield return indexScraperPage;
				}
			}
		}

		public static IEnumerable<PageLink> GetBrowseScraperPages(bool refresh, params PagePart[] patterns)
		{
			return GetBrowseScraperPages(refresh, patterns[0].Pattern, patterns.Last(), patterns.Skip(1).Take(patterns.Length - 2), new HashSet<string>(StringComparer.OrdinalIgnoreCase));
		}

		public static IEnumerable<PageLink> GetBrowseScraperPages(bool refresh, string startPage, PagePart nextLink, IEnumerable<PagePart> imageLinks, HashSet<string> trackback)
		{
			Log("Next Link: {0}", nextLink);
			Log(delegate(TextWriter t)
			{
				t.WriteLine("Image Links: {0} [{1}]", imageLinks.Count(), imageLinks.ToListString(", "));
			});
			Uri current = new Uri(startPage);
			bool pageRefresh = refresh;
			while (true)
			{
				string page = current.ToString();
				Log("Current Page: {0}{1}", page, pageRefresh ? " (refreshing)" : string.Empty);
				if (trackback.Contains(page))
				{
					break;
				}
				string pageText;
				try
				{
					pageText = ReadText(page, pageRefresh);
				}
				catch
				{
					yield break;
				}
				Log("Calling Index scraper on the page");
				IEnumerable<string> enumerable = MatchesRegex(pageText, nextLink);
				if (!enumerable.IsEmpty())
				{
					string headerBaseUri = GetHeaderBaseUri(pageText);
					Uri uri = MakeAbsolute(current, enumerable.First(), headerBaseUri);
					trackback.Add(page);
					if (!pageRefresh && trackback.Contains(uri.ToString()))
					{
						trackback.Remove(page);
						pageRefresh = true;
						continue;
					}
					current = uri;
					foreach (PageLink indexScraperPage in GetIndexScraperPages(pageRefresh, page, pageText, imageLinks, trackback))
					{
						yield return indexScraperPage;
					}
					continue;
				}
				if (!pageRefresh)
				{
					Log("Could not find next link: refreshing the page");
					pageRefresh = true;
					continue;
				}
				foreach (PageLink indexScraperPage2 in GetIndexScraperPages(refresh: true, page, pageText, imageLinks, trackback))
				{
					yield return indexScraperPage2;
				}
				break;
			}
		}

		private static IEnumerable<string> MatchesRegex(string text, PagePart regex)
		{
			if (!string.IsNullOrEmpty(regex.Cut))
			{
				Regex regex2 = CreateRegex(regex.Cut);
				Match match = regex2.Match(text);
				if (!match.Success)
				{
					return Enumerable.Empty<string>();
				}
				text = GetValue(match);
			}
			Regex regex3 = CreateRegex(regex.Pattern);
			return regex3.Matches(text).OfType<Match>().Take(regex.MaximumMatches)
				.Select(GetValue);
		}

		private static Regex CreateRegex(string pattern)
		{
			using (ItemMonitor.Lock(rxCache))
			{
				if (rxCache.TryGetValue(pattern, out var value))
				{
					return value;
				}
				return rxCache[pattern] = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
		}

		private static bool HasOption(ref string s, string option)
		{
			if (!s.StartsWith(option))
			{
				return false;
			}
			s = s.Substring(option.Length);
			return true;
		}

		private static bool HasOption(PagePart part, string option)
		{
			string s = part.Pattern;
			bool result = HasOption(ref s, option);
			part.Pattern = s;
			return result;
		}

		private static string GetValue(Match m)
		{
			string value = m.Groups["link"].Value;
			if (string.IsNullOrEmpty(value))
			{
				value = m.Value;
			}
			return value.Remove("\"").Remove("'").Remove("\r")
				.Remove("\n")
				.Replace("&amp;", "&");
		}

		private static Uri MakeAbsolute(Uri uri, string relative, string baseUri = null)
		{
			if (!string.IsNullOrEmpty(baseUri))
			{
				uri = new Uri(uri, baseUri);
			}
			return new Uri(uri, relative);
		}

		private static string GetHeaderBaseUri(string pageContent)
		{
			Match match = rxBaseMatcher.Match(pageContent);
			if (!match.Success)
			{
				return null;
			}
			return match.Groups["base"].Value;
		}

		private static string ReadText(string uri, bool refresh)
		{
			Log("Reading Page from '{0}'", uri);
			try
			{
				if (FileCache.Default != null && !refresh)
				{
					string text = FileCache.Default.GetText(uri);
					if (text != null)
					{
						return text;
					}
				}
				try
				{
					string text2 = HttpAccess.ReadText(uri);
					if (FileCache.Default != null)
					{
						FileCache.Default.AddText(uri, text2);
					}
					return text2;
				}
				catch
				{
					if (FileCache.Default == null)
					{
						throw;
					}
					string text3 = FileCache.Default.GetText(uri);
					if (text3 != null)
					{
						return text3;
					}
					throw;
				}
			}
			catch
			{
				Log("Failed to read page from '{0}'!", uri);
				throw;
			}
		}

		public static void SetLogOutput(Stream ts)
		{
			logStream = ts;
		}

		protected static void Log(Action<TextWriter> writeAction)
		{
			if (logStream != null && writeAction != null)
			{
				using (StreamWriter obj = new StreamWriter(logStream))
				{
					writeAction(obj);
				}
			}
		}

		protected static void Log(string s)
		{
			Log(delegate(TextWriter t)
			{
				t.WriteLine(s);
			});
		}

		protected static void Log(string s, params object[] data)
		{
			Log(delegate(TextWriter t)
			{
				t.WriteLine(s, data);
			});
		}

		protected static void LogSeparator(char c = '-', int width = 40)
		{
			Log(new string(c, width));
		}
	}
}
