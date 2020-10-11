using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.IO;
using cYo.Common.Reflection;
using cYo.Common.Xml;

namespace cYo.Common.Localize
{
	public class TR
	{
		private readonly TREntryList texts;

		private CultureInfo culture = CultureInfo.CurrentUICulture;

		private static readonly TRDictionary languageResources = new TRDictionary();

		private static TR defaultTR;

		private static TR messagesTR;

		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		[XmlAttribute]
		public string CultureName
		{
			get
			{
				return culture.Name;
			}
			set
			{
				culture = new CultureInfo(value);
			}
		}

		[XmlArray("Texts")]
		[XmlArrayItem("Text")]
		public TREntryList Texts => texts;

		[XmlIgnore]
		public CultureInfo Culture
		{
			get
			{
				return culture;
			}
			set
			{
				culture = value;
			}
		}

		[XmlIgnore]
		public string File
		{
			get;
			set;
		}

		public string FileName => Path.GetFileName(File);

		public bool IsEmpty => texts.Count == 0;

		public string this[string key, string value] => texts.GetText(key, value);

		public string this[string key] => texts.GetText(key, key);

		public static TRDictionary LanguageResources => languageResources;

		public static TRInfo Info => LanguageResources.Info;

		public static CultureInfo DefaultCulture
		{
			get
			{
				return LanguageResources.DefaultCulture;
			}
			set
			{
				LanguageResources.DefaultCulture = value;
			}
		}

		public static IVirtualFolder ResourceFolder
		{
			get
			{
				return LanguageResources.ResourceFolder;
			}
			set
			{
				LanguageResources.ResourceFolder = value;
			}
		}

		public static TR Default
		{
			get
			{
				if (defaultTR == null)
				{
					defaultTR = Load("Default");
				}
				return defaultTR;
			}
		}

		public static TR Messages
		{
			get
			{
				if (messagesTR == null)
				{
					messagesTR = Load("Messages");
				}
				return messagesTR;
			}
		}

		public TR()
		{
			texts = new TREntryList(this);
		}

		public TR(string name, CultureInfo culture)
			: this()
		{
			Name = name;
			this.culture = culture;
		}

		public string[] GetStrings(string key, string array, char sep)
		{
			string[] array2 = array.Split(sep);
			try
			{
				string[] array3 = this[key, array].Split(sep);
				if (array2.Length == array3.Length)
				{
					return array3;
				}
				return array2;
			}
			catch (Exception)
			{
				return array2;
			}
		}

		public void Save(IVirtualFolder folder, string path)
		{
			Texts.Sort();
			byte[] second = XmlUtility.Store(this, compressed: false);
			try
			{
				using (Stream stream = folder.OpenRead(path))
				{
					byte[] array = new byte[stream.Length];
					stream.Read(array, 0, (int)stream.Length);
					if (array.SequenceEqual(second))
					{
						return;
					}
				}
			}
			catch (Exception)
			{
			}
			try
			{
				folder.CreateFolder(Path.GetDirectoryName(path));
				XmlUtility.Store(folder.Create(path), this, compressed: false);
			}
			catch (Exception)
			{
			}
		}

		public static string Translate(Enum e)
		{
			return Load(e.GetType().Name)[e.Name(), e.Description()];
		}

		public static TR Load(string name)
		{
			return LanguageResources.Load(name);
		}

		public static IEnumerable<TRInfo> GetLanguageInfos()
		{
			return LanguageResources.GetLanguageInfos();
		}
	}
}
