using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public class ComicNameInfo
	{
		private static class NewParser
		{
			private const RegexOptions RxOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline;

			private static readonly Regex rxDotReplace = new Regex("((?<!\\d)\\.|\\.(?!\\d)|_)", RxOptions);

			private static readonly Regex rxRemove = new Regex("\\b(ctc|c2c|\\d+p|\\d{1,2}\\-\\d{1,2}|\\d{1,2}\\-(?=\\d{4}))\\b", RxOptions);

			private static readonly Regex rxBrackets = new Regex("\\(.*?\\)|\\[.*?\\]", RxOptions);

			private static Regex rxCount;

			private static readonly Regex rxNumber = new Regex("(?<!part\\s+)(\\b|#|(c\\w*\\s*))\\d[\\d\\.]*\\b(?!\\s*(pa|cov))", RxOptions | RegexOptions.RightToLeft);

			private static readonly Regex rxVolume = new Regex("\\b(v|vol\\.?|volume)\\s*\\d+\\b", RxOptions);

			private static readonly Regex rxYear = new Regex("\\b(?<!#)(19|2[0-3])\\d\\d\\b(?!\\spa)", RxOptions | RegexOptions.RightToLeft);

			private static readonly Regex rxYearWithMonth = new Regex("(19|2[0-3])\\d\\d[-/\\\\\\s]\\d{1,2}\\b", RxOptions | RegexOptions.RightToLeft);

			private static readonly Regex rxFormat = new Regex("\\b(annual|director's cut|preview|b(lack)?\\s*&\\s*w(hite)?|king\\s*size|giant\\s*size)|sketch\\b", RxOptions);

			private static readonly Regex rxCoverCount = new Regex("(?<covers>\\d+)\\s+cover", RxOptions);

			private static readonly Regex rxnum = new Regex("\\d+\\.?\\d*", RxOptions | RegexOptions.RightToLeft);

			public static ComicNameInfo FromFilePath(string path)
			{
				ComicNameInfo comicNameInfo = new ComicNameInfo();
				try
				{
					string text = Path.GetFileNameWithoutExtension(path);
					if (!text.Contains(" ") || text.Contains("_"))
					{
						text = rxDotReplace.Replace(text, " ");
					}
					text = rxRemove.Replace(text, string.Empty);
					if (rxCount == null)
					{
						string text2 = EngineConfiguration.Default.OfValues ?? "of,von,de";
						string arg = text2.Split(',').TrimStrings().ToListString("|");
						rxCount = new Regex($"\\b({arg})\\s*\\d+\\b", RxOptions);
					}
					Match match = rxCount.Match(text);
					if (match.Success)
					{
						text = text.Remove(match.Index, match.Length);
						if (!int.TryParse(GetNumber(match.Value), out comicNameInfo.count))
						{
							comicNameInfo.count = -1;
						}
					}
					match = rxVolume.Match(text);
					if (match.Success)
					{
						text = text.Remove(match.Index, match.Length);
						if (!int.TryParse(GetNumber(match.Value), out comicNameInfo.volume))
						{
							comicNameInfo.volume = -1;
						}
					}
					match = rxYearWithMonth.Match(text);
					if (!match.Success)
					{
						match = rxYear.Match(text);
					}
					if (match.Success)
					{
						text = text.Remove(match.Index, match.Length);
						if (!int.TryParse(GetNumber(match.Value.Substring(0, 4)), out comicNameInfo.year))
						{
							comicNameInfo.year = -1;
						}
					}
					match = rxFormat.Match(text);
					if (match.Success)
					{
						text = text.Remove(match.Index, match.Length);
						comicNameInfo.Format = match.Value;
					}
					match = rxNumber.Match(text);
					if (match.Success)
					{
						text = text.Remove(match.Index, match.Length);
					}
					comicNameInfo.number = GetNumber(match.Value);
					if (float.TryParse(comicNameInfo.number, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var result))
					{
						comicNameInfo.number = result.ToString(CultureInfo.InvariantCulture);
					}
					match = rxCoverCount.Match(text);
					if (match.Success)
					{
						comicNameInfo.CoverCount = int.Parse(match.Groups["covers"].Value);
					}
					else if (text.Contains("two cove", StringComparison.OrdinalIgnoreCase))
					{
						comicNameInfo.CoverCount = 2;
					}
					else if (text.Contains("three cove", StringComparison.OrdinalIgnoreCase))
					{
						comicNameInfo.CoverCount = 3;
					}
					else if (text.Contains("four cove", StringComparison.OrdinalIgnoreCase))
					{
						comicNameInfo.CoverCount = 4;
					}
					text = text.CutOff('(', '[', '#', ',').Trim();
					comicNameInfo.series = text.Trim(' ', '-', '.');
					if (string.IsNullOrEmpty(comicNameInfo.series) || comicNameInfo.Series.IsNumber())
					{
						comicNameInfo.series = rxBrackets.Replace(Path.GetFileName(Path.GetDirectoryName(path)), string.Empty);
						return comicNameInfo;
					}
					return comicNameInfo;
				}
				catch (Exception)
				{
					return comicNameInfo;
				}
			}

			private static string GetNumber(string text)
			{
				if (!string.IsNullOrEmpty(text))
				{
					return rxnum.Match(text).Value;
				}
				return string.Empty;
			}
		}

		private static class LegacyParser
		{
			private const RegexOptions RxOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline;

			private static readonly Regex rxSeries = new Regex("^(\\d+)?(([&\\w\\s'-])(?!v\\d|(?<=[ #])(\\d(?!\\d*\\s[#\\d]))+(?=(\\W|$))(?!\\))))*", RxOptions);

			private static readonly Regex rxVolume = new Regex("(?<=\\bv)\\d(?=\\b)", RxOptions);

			private static readonly Regex rxNumber = new Regex("(?<=[ #]|c|ch)(\\d(?!\\d*\\s[#\\d]))+(?=(\\W|$))(?!\\))", RxOptions);

			private static readonly Regex rxCount = new Regex("(?<=[\\(\\[\\s]of\\s)\\d+", RxOptions);

			private static readonly Regex rxYear = new Regex("(?<=[\\(\\[])\\d{4}\\b", RxOptions);

			public static ComicNameInfo FromFilePath(string path)
			{
				ComicNameInfo comicNameInfo = new ComicNameInfo();
				try
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
					fileNameWithoutExtension = fileNameWithoutExtension.Replace('.', ' ').Replace('_', ' ');
					comicNameInfo.series = rxSeries.Match(fileNameWithoutExtension).Value;
					if (string.IsNullOrEmpty(comicNameInfo.series) || (char.IsDigit(comicNameInfo.series[0]) && string.IsNullOrEmpty(rxNumber.Match(fileNameWithoutExtension).Value)))
					{
						fileNameWithoutExtension = Path.GetFileName(Path.GetDirectoryName(path)) + " " + fileNameWithoutExtension;
						comicNameInfo.series = rxSeries.Match(fileNameWithoutExtension).Value;
					}
					if (string.IsNullOrEmpty(comicNameInfo.series))
					{
						comicNameInfo.series = fileNameWithoutExtension;
					}
					comicNameInfo.Series = comicNameInfo.Series.Trim();
					string value = rxNumber.Match(fileNameWithoutExtension).Value;
					comicNameInfo.number = (value.TryParse(out float f, invariant: true) ? f.ToString() : value);
					if (!int.TryParse(rxCount.Match(fileNameWithoutExtension).Value, out comicNameInfo.count))
					{
						comicNameInfo.count = -1;
					}
					if (!int.TryParse(rxVolume.Match(fileNameWithoutExtension).Value, out comicNameInfo.volume))
					{
						comicNameInfo.volume = -1;
					}
					if (!int.TryParse(rxYear.Match(fileNameWithoutExtension).Value, out comicNameInfo.year))
					{
						comicNameInfo.year = -1;
						return comicNameInfo;
					}
					return comicNameInfo;
				}
				catch (Exception)
				{
					return comicNameInfo;
				}
			}
		}

		private string series = string.Empty;

		private string title = string.Empty;

		private string number = string.Empty;

		private string format = string.Empty;

		private int volume = -1;

		private int count = -1;

		private int year = -1;

		private int coverCount = 1;

		public string Series
		{
			get
			{
				return series;
			}
			set
			{
				series = value;
			}
		}

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		public string Number
		{
			get
			{
				return number;
			}
			set
			{
				number = value;
			}
		}

		public string Format
		{
			get
			{
				return format;
			}
			set
			{
				format = value;
			}
		}

		public int Volume
		{
			get
			{
				return volume;
			}
			set
			{
				volume = value;
			}
		}

		public int Count
		{
			get
			{
				return count;
			}
			set
			{
				count = value;
			}
		}

		public int Year
		{
			get
			{
				return year;
			}
			set
			{
				year = value;
			}
		}

		public int CoverCount
		{
			get
			{
				return coverCount;
			}
			set
			{
				coverCount = value;
			}
		}

		public ComicNameInfo(string series, string title, string number, int count, int volume, int year, string format)
		{
			Series = series;
			Number = number;
			Year = year;
			Count = count;
			Volume = volume;
			Format = format;
			Title = title;
		}

		public ComicNameInfo()
			: this(string.Empty, string.Empty, string.Empty, -1, -1, -1, string.Empty)
		{
		}

		public override int GetHashCode()
		{
			return Series.GetHashCode() ^ Number.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			ComicNameInfo comicNameInfo = obj as ComicNameInfo;
			if (comicNameInfo == null)
			{
				return false;
			}
			if (comicNameInfo.Series == Series && comicNameInfo.Number == Number && comicNameInfo.Count == Count && comicNameInfo.Volume == Volume && comicNameInfo.Title == Title && comicNameInfo.Format == Format)
			{
				return comicNameInfo.Year == Year;
			}
			return false;
		}

		public static ComicNameInfo FromFilePath(string path, bool legacy)
		{
			if (!legacy)
			{
				return NewParser.FromFilePath(path);
			}
			return LegacyParser.FromFilePath(path);
		}

		public static ComicNameInfo FromFilePath(string path)
		{
			return FromFilePath(path, EngineConfiguration.Default.LegacyFilenameParser);
		}
	}
}
