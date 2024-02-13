using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.ComponentModel;
using cYo.Common.Localize;
using cYo.Common.Reflection;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public abstract class ComicBookMatcher : IComicBookMatcher, IMatcher<ComicBook>, ICloneable
	{
		public const string ClipboardFormat = "ComicBookMatcher";

		private static TR trMatcher;

		public const string SeriesStatsPropertyPrefix = "Stats";

		public static readonly string[] ComicProperties = ComicBook.GetProperties(onlyWritable: true).ToArray();

		public static readonly string[] SeriesStatsProperties = (from name in ComicBookSeriesStatistics.GetProperties()
			select SeriesStatsPropertyPrefix + name).ToArray();

		[NonSerialized]
		private IComicBookStatsProvider statsProvider;

		private bool propertyCheckInitialized;

		private ISet<string> usedProperties;

		private bool isOptimizedCacheUpdateDisabled;

		private readonly string[] wildCardProperty = new string[1]
		{
			"*"
		};

		public static TR TRMatcher
		{
			get
			{
				if (trMatcher == null)
				{
					trMatcher = TR.Load("Matchers");
				}
				return trMatcher;
			}
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool Not
		{
			get;
			set;
		}

		[XmlIgnore]
		public IComicBookStatsProvider StatsProvider
		{
			get
			{
				return statsProvider;
			}
			set
			{
				statsProvider = value;
			}
		}

		public virtual bool IsOptimizedCacheUpdateDisabled
		{
			get
			{
				InitializeFromProperty();
				return isOptimizedCacheUpdateDisabled;
			}
		}

		public virtual bool TimeDependant => false;

		public abstract IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items);

		public abstract object Clone();

		public virtual bool IsSame(ComicBookMatcher cbm)
		{
			if (cbm != null && cbm.GetType() == GetType())
			{
				return cbm.Not == Not;
			}
			return false;
		}

		private void InitializeFromProperty()
		{
			if (!propertyCheckInitialized)
			{
				propertyCheckInitialized = true;
				ComicBookMatcherHintAttribute comicBookMatcherHintAttribute = Attribute.GetCustomAttribute(GetType(), typeof(ComicBookMatcherHintAttribute)) as ComicBookMatcherHintAttribute;
				if (comicBookMatcherHintAttribute != null)
				{
					usedProperties = comicBookMatcherHintAttribute.Properties;
					isOptimizedCacheUpdateDisabled = comicBookMatcherHintAttribute.DisableOptimizedUpdate;
				}
			}
		}

		public virtual IEnumerable<string> GetDependentProperties()
		{
			InitializeFromProperty();
			if (usedProperties == null)
			{
				return wildCardProperty;
			}
			return usedProperties;
		}

		public virtual bool UsesProperty(string propertyHint)
		{
			InitializeFromProperty();
			if (usedProperties != null)
			{
				return usedProperties.Contains(propertyHint);
			}
			return true;
		}

		public override string ToString()
		{
			return ConvertToString(this);
		}

		public static string ConvertToString(IComicBookMatcher matcher)
		{
			return (matcher.Not ? "Not " : string.Empty) + "[" + (matcher.GetType().Description().Escape("[]", '\\') ?? matcher.GetType().Name) + "]";
		}

		public static bool IsComicProperty(string prop)
		{
			return ComicProperties.Contains(prop);
		}

		public static bool IsSeriesStatsProperty(string prop)
		{
			return SeriesStatsProperties.Contains(prop);
		}

		public static string ParseSeriesProperty(string prop)
		{
			return prop.Substring(SeriesStatsPropertyPrefix.Length);
		}
	}
}
