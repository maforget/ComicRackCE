using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[XmlRoot("ReadingList")]
	public class ComicReadingListContainer
	{
		private readonly ComicReadingList items = new ComicReadingList();

		private ComicBookMatcherCollection matchers = new ComicBookMatcherCollection();

		private static Type[] extraTypes;

		public string Name
		{
			get;
			set;
		}

		[XmlArray("Books")]
		[XmlArrayItem("Book")]
		public ComicReadingList Items => items;

		[XmlAttribute]
		[DefaultValue(MatcherMode.And)]
		public MatcherMode MatcherMode
		{
			get;
			set;
		}

		public ComicBookMatcherCollection Matchers => matchers;

		public ComicReadingListContainer()
		{
			MatcherMode = MatcherMode.And;
		}

		public ComicReadingListContainer(ComicListItem list, bool withFilenames, bool alwaysList = false, Func<string, IComparer<ComicBook>> sortHandler = null) : this()
		{
			Name = list.Name;
			ComicSmartListItem comicSmartListItem = list as ComicSmartListItem; // Check if it's a smart list
			if (alwaysList || comicSmartListItem == null)
			{
				List<ComicBook> bookList = list.GetBooks().ToList();// Get the list of books
				string sortKey = comicSmartListItem?.Display.View.SortKey;
				if (comicSmartListItem != null && sortHandler != null && !string.IsNullOrEmpty(sortKey) && (sortHandler(sortKey) is IComparer<ComicBook> comparer) && comparer != null)
					bookList.Sort(comparer);  // sort the book list using the sort key;

				bookList.ForEach((ComicBook b) => this.items.Add(new ComicReadingListItem(b, withFilenames))); // add the individual list of books
				return;
			}
			MatcherMode = comicSmartListItem.MatcherMode;
			comicSmartListItem.Matchers.ForEach((ComicBookMatcher m) => Matchers.Add(m.Clone() as ComicBookMatcher));
		}

		public void Serialize(Stream outStream)
		{
			XmlUtility.GetSerializer<ComicReadingListContainer>().Serialize(outStream, this);
		}

		public void Serialize(string file)
		{
			using (FileStream outStream = File.Create(file))
			{
				Serialize(outStream);
			}
		}

		public static ComicReadingListContainer Deserialize(Stream inStream)
		{
			return XmlUtility.GetSerializer<ComicReadingListContainer>().Deserialize(inStream) as ComicReadingListContainer;
		}

		public static ComicReadingListContainer Deserialize(string file)
		{
			using (FileStream inStream = File.OpenRead(file))
			{
				return Deserialize(inStream);
			}
		}

		public static Type[] GetExtraXmlSerializationTypes()
		{
			if (extraTypes == null)
			{
				List<Type> list = new List<Type>();
				list.AddRange(ComicBookValueMatcher.GetAvailableMatcherTypes());
				list.Add(typeof(ComicBookGroupMatcher));
				extraTypes = list.ToArray();
			}
			return extraTypes;
		}
	}
}
