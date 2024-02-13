using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using cYo.Common.IO;
using cYo.Common.Text;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	public class DefaultLists
	{
		private const string DefaultGenresSection = "Book Genres";

		private const string DefaultFormatsSection = "Book Formats";

		private const string DefaultAgeRatingsSection = "Age Ratings";

		private const string DefaultBookAgesSection = "Book Ages";

		private const string DefaultBookConditionsSection = "Book Conditions";

		private const string DefaultBookCollectionStatusSection = "Book Collection Status";

		private Func<IEnumerable<ComicBook>> getBooks;

		public string[] DefaultGenres
		{
			get;
			private set;
		}

		public string[] DefaultFormats
		{
			get;
			private set;
		}

		public string[] DefaultAgeRatings
		{
			get;
			private set;
		}

		public string[] DefaultBookAges
		{
			get;
			private set;
		}

		public string[] DefaultBookConditions
		{
			get;
			private set;
		}

		public string[] DefaultBookCollectionStatus
		{
			get;
			private set;
		}

		public DefaultLists(Func<IEnumerable<ComicBook>> getBooks, IEnumerable<string> initPaths)
		{
			this.getBooks = getBooks;
			DefaultGenres = LoadDefaultTextList(initPaths, DefaultGenresSection).ToArray();
			DefaultFormats = (from s in LoadDefaultTextList(initPaths, DefaultFormatsSection).Concat(ComicBook.FormatIcons.Keys).Distinct()
				orderby s
				select s).ToArray();
			DefaultAgeRatings = (from s in LoadDefaultTextList(initPaths, DefaultAgeRatingsSection).Concat(ComicBook.AgeRatingIcons.Keys)
				orderby s
				select s).Distinct().ToArray();
			DefaultBookAges = LoadDefaultTextList(initPaths, DefaultBookAgesSection).ToArray();
			DefaultBookConditions = LoadDefaultTextList(initPaths, DefaultBookConditionsSection).ToArray();
			DefaultBookCollectionStatus = LoadDefaultTextList(initPaths, DefaultBookCollectionStatusSection).ToArray();
		}

		public AutoCompleteStringCollection GetComicFieldList(Func<ComicBook, string> autoCompleteHandler, bool sort = false)
		{
			AutoCompleteStringCollection autoCompleteStringCollection = new AutoCompleteStringCollection();
			foreach (ComicBook item in getBooks())
			{
				autoCompleteStringCollection.Add(autoCompleteHandler(item));
			}
			if (sort)
			{
				ArrayList.Adapter(autoCompleteStringCollection).Sort();
			}
			return autoCompleteStringCollection;
		}

		public AutoCompleteStringCollection GetGenreList(bool withSeparator)
		{
			AutoCompleteStringCollection comicFieldList = GetComicFieldList((ComicBook cb) => cb.Genre);
			comicFieldList.Remove("");
			if (withSeparator)
			{
				comicFieldList.Remove("-");
				if (comicFieldList.Count > 0)
				{
					comicFieldList.Add("-");
				}
			}
			comicFieldList.AddRange(DefaultGenres);
			return comicFieldList;
		}

		public AutoCompleteStringCollection GetFormatList()
		{
			AutoCompleteStringCollection comicFieldList = GetComicFieldList((ComicBook cb) => cb.ShadowFormat, sort: true);
			comicFieldList.Remove("");
			comicFieldList.Remove("-");
			if (comicFieldList.Count > 0)
			{
				comicFieldList.Add("-");
			}
			comicFieldList.AddRange(DefaultFormats);
			return comicFieldList;
		}

		public AutoCompleteStringCollection GetAgeRatingList()
		{
			AutoCompleteStringCollection comicFieldList = GetComicFieldList((ComicBook cb) => cb.AgeRating);
			comicFieldList.Remove("");
			comicFieldList.Remove("-");
			if (comicFieldList.Count > 0)
			{
				comicFieldList.Add("-");
			}
			comicFieldList.AddRange(DefaultAgeRatings);
			return comicFieldList;
		}

		public AutoCompleteStringCollection GetBookAgeList()
		{
			AutoCompleteStringCollection comicFieldList = GetComicFieldList((ComicBook cb) => cb.BookAge);
			comicFieldList.Remove("");
			comicFieldList.Remove("-");
			if (comicFieldList.Count > 0)
			{
				comicFieldList.Add("-");
			}
			comicFieldList.AddRange(DefaultBookAges);
			return comicFieldList;
		}

		public AutoCompleteStringCollection GetBookConditionList()
		{
			AutoCompleteStringCollection comicFieldList = GetComicFieldList((ComicBook cb) => cb.BookCondition);
			comicFieldList.Remove("");
			comicFieldList.Remove("-");
			if (comicFieldList.Count > 0)
			{
				comicFieldList.Add("-");
			}
			comicFieldList.AddRange(DefaultBookConditions);
			return comicFieldList;
		}

		public AutoCompleteStringCollection GetBookCollectionStatusList()
		{
			AutoCompleteStringCollection comicFieldList = GetComicFieldList((ComicBook cb) => cb.BookCollectionStatus);
			comicFieldList.Remove("");
			comicFieldList.Remove("-");
			comicFieldList.AddRange(DefaultBookCollectionStatus);
			return comicFieldList;
		}

		private static IEnumerable<string> LoadDefaultTextList(IEnumerable<string> files, string section)
		{
			List<string> list = new List<string>();
			foreach (string file in files)
			{
				try
				{
					list.AddRange(LoadTextList(file, section));
				}
				catch (Exception)
				{
				}
			}
			return list;
		}

		private static IEnumerable<string> LoadTextList(string file, string section)
		{
			if (!File.Exists(file))
			{
				yield break;
			}
			string sectionHeader = $"[{section}]";
			bool sectionFound = false;
			string prefix = null;
			foreach (string line in FileUtility.ReadLines(file).TrimEndStrings().RemoveEmpty())
			{
				if (line.StartsWith(";"))
				{
					continue;
				}
				if (line.StartsWith(sectionHeader))
				{
					sectionFound = true;
					continue;
				}
				if (sectionFound && line.StartsWith("["))
				{
					break;
				}
				if (sectionFound)
				{
					if (!char.IsWhiteSpace(line[0]) || string.IsNullOrEmpty(prefix))
					{
						prefix = line;
						yield return line;
					}
					else
					{
						yield return prefix + ": " + line.Trim();
					}
				}
			}
		}
	}
}
