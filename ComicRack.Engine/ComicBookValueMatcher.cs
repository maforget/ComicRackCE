using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using cYo.Common.Collections;
using cYo.Common.ComponentModel;
using cYo.Common.Reflection;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	public abstract class ComicBookValueMatcher : ComicBookMatcher, IComicBookValueMatcher, IComicBookMatcher, IMatcher<ComicBook>, ICloneable, IComparable
	{
		private string description;

		private string descriptionNeutral;

		private string matchValue;

		private string matchValue2;

		private int matchOperator;

		private static HashSet<Type> cachedTypeList;

		[XmlAttribute]
		[DefaultValue("")]
		public string Name
		{
			get;
			set;
		}

		public string Description => description ?? (description = ComicBookMatcher.TRMatcher[GetType().Name, DescriptionNeutral]);

		public virtual string DescriptionNeutral
		{
			get
			{
				if (descriptionNeutral == null)
				{
                    DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(GetType(), typeof(DescriptionAttribute));
                    descriptionNeutral = descriptionAttribute.Description ?? string.Empty;
				}
				return descriptionNeutral;
			}
		}

		public virtual string MatchValue
		{
			get
			{
				return matchValue ?? string.Empty;
			}
			set
			{
				if (!(matchValue == value))
				{
					matchValue = value;
					OnMatchValueChanged();
				}
			}
		}

		[DefaultValue("")]
		public virtual string MatchValue2
		{
			get
			{
				return matchValue2 ?? string.Empty;
			}
			set
			{
				if (!(matchValue2 == value))
				{
					matchValue2 = value;
					OnMatchValue2Changed();
				}
			}
		}

		[XmlAttribute]
		[DefaultValue(0)]
		public virtual int MatchOperator
		{
			get
			{
				return matchOperator;
			}
			set
			{
				if (matchOperator != value)
				{
					matchOperator = value;
					OnMatchOperatorChanged();
				}
			}
		}

		public abstract string[] OperatorsListNeutral
		{
			get;
		}

		public abstract string[] OperatorsList
		{
			get;
		}

		public abstract int ArgumentCount
		{
			get;
		}

		public virtual bool SwapOperatorArgument => false;

		public virtual string UnitDescription => string.Empty;

		protected ComicBookValueMatcher()
		{
			Name = string.Empty;
		}

		public override IEnumerable<ComicBook> Match(IEnumerable<ComicBook> items)
		{
			OnInitializeMatch();
			if (ListExtensions.ParallelEnabled && items.Count() > 100)
			{
				return items.Lock().ToArray().AsParallelSafe()
					.Where(Match);
			}
			return items.Where(Match);
		}

		public override object Clone()
		{
			IComicBookValueMatcher comicBookValueMatcher = (IComicBookValueMatcher)Activator.CreateInstance(GetType());
			comicBookValueMatcher.Not = base.Not;
			comicBookValueMatcher.MatchOperator = MatchOperator;
			comicBookValueMatcher.MatchValue = MatchValue;
			comicBookValueMatcher.MatchValue2 = MatchValue2;
			return comicBookValueMatcher;
		}

		public override bool IsSame(ComicBookMatcher cbm)
		{
			ComicBookValueMatcher comicBookValueMatcher = cbm as ComicBookValueMatcher;
			if (comicBookValueMatcher != null && base.IsSame(cbm) && comicBookValueMatcher.MatchOperator == MatchOperator && comicBookValueMatcher.MatchValue == MatchValue)
			{
				return comicBookValueMatcher.MatchValue2 == MatchValue2;
			}
			return false;
		}

		public override string ToString()
		{
			return base.ToString() + " " + ConvertParametersToString(this);
		}

		protected virtual void OnInitializeMatch()
		{
		}

		protected virtual void OnMatchValueChanged()
		{
		}

		protected virtual void OnMatchValue2Changed()
		{
		}

		protected virtual void OnMatchOperatorChanged()
		{
		}

		public virtual bool Set(ComicBookValueMatcher matcher)
		{
			Type type = GetType();
			Type type2 = matcher.GetType();
			if (type2 != type && type2.BaseType != type.BaseType)
			{
				return false;
			}
			matchOperator = matcher.matchOperator;
			matchValue = matcher.matchValue;
			matchValue2 = matcher.matchValue2;
			return true;
		}

		public virtual bool Match(ComicBook item)
		{
			return true;
		}

		public int CompareTo(object obj)
		{
			IComicBookValueMatcher comicBookValueMatcher = obj as IComicBookValueMatcher;
			if (comicBookValueMatcher != null)
			{
				return string.Compare(Description, comicBookValueMatcher.Description, ignoreCase: true);
			}
			return 1;
		}

		private static string Escape(string value)
		{
			return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
		}

		public static string ConvertParametersToString(ComicBookValueMatcher m)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(m.OperatorsListNeutral[m.MatchOperator]);
			for (int i = 0; i < m.ArgumentCount; i++)
			{
				stringBuilder.Append(' ');
				stringBuilder.Append('"');
				stringBuilder.Append(Escape((i == 0) ? m.MatchValue : m.MatchValue2));
				stringBuilder.Append('"');
			}
			return stringBuilder.ToString();
		}

		public static IEnumerable<Type> GetAvailableMatcherTypes()
		{
			if (cachedTypeList == null)
			{
				cachedTypeList = new HashSet<Type>(from t in typeof(IComicBookValueMatcher).Assembly.GetExportedTypes()
					where t.GetInterface(typeof(IComicBookValueMatcher).Name) != null && !t.IsAbstract
					select t);
			}
			return cachedTypeList;
		}

		public static void RegisterMatcherType(Type type)
		{
			GetAvailableMatcherTypes();
			cachedTypeList.Add(type);
		}

		public static IEnumerable<IComicBookValueMatcher> GetAvailableMatchers()
		{
			IComicBookValueMatcher[] array = (from t in GetAvailableMatcherTypes()
				select Activator.CreateInstance(t) as IComicBookValueMatcher).ToArray();
			Array.Sort(array);
			return array;
		}

		public static ComicBookValueMatcher Create(Type matchType, int matchOperator, string matchValue1, string matchValue2)
		{
			ComicBookValueMatcher comicBookValueMatcher = Activator.CreateInstance(matchType) as ComicBookValueMatcher;
			if (comicBookValueMatcher == null)
			{
				throw new ArgumentException("Must be a ComicBookValueMatcher", "matchType");
			}
			comicBookValueMatcher.MatchOperator = matchOperator;
			comicBookValueMatcher.MatchValue = matchValue1;
			comicBookValueMatcher.MatchValue2 = matchValue2;
			return comicBookValueMatcher;
		}

		public static IComicBookValueMatcher Create(string description)
		{
			Type type = GetAvailableMatcherTypes().FirstOrDefault((Type t) => string.Equals(t.Description(), description, StringComparison.OrdinalIgnoreCase));
			return (IComicBookValueMatcher)((type != null) ? Activator.CreateInstance(type) : null);
		}
	}
	[Serializable]
	public abstract class ComicBookValueMatcher<T> : ComicBookValueMatcher
	{
		private string comicProperty;

		private string comicProperty2;

		private string seriesStatsProperty;

		private string seriesStatsProperty2;

		private T matchValue;

		private T matchValue2;

		protected static readonly Regex FieldExpression = new Regex("{(?<name>[a-z]+)}", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public override bool IsOptimizedCacheUpdateDisabled
		{
			get
			{
				if (!base.IsOptimizedCacheUpdateDisabled && seriesStatsProperty == null)
				{
					return seriesStatsProperty2 != null;
				}
				return true;
			}
		}

		public override IEnumerable<string> GetDependentProperties()
		{
			foreach (string dependentProperty in base.GetDependentProperties())
			{
				yield return dependentProperty;
			}
			if (comicProperty != null)
			{
				yield return comicProperty;
			}
			if (comicProperty2 != null)
			{
				yield return comicProperty2;
			}
		}

		public override bool UsesProperty(string propertyHint)
		{
			if (comicProperty != null && comicProperty == propertyHint)
			{
				return true;
			}
			if (comicProperty2 != null && comicProperty2 == propertyHint)
			{
				return true;
			}
			return base.UsesProperty(propertyHint);
		}

		public override bool Match(ComicBook item)
		{
			return MatchBook(item, PreparseValue(GetValue(item)));
		}

		protected override void OnMatchValueChanged()
		{
			base.OnMatchValueChanged();
			Match match = FieldExpression.Match(MatchValue ?? string.Empty);
			comicProperty = (seriesStatsProperty = null);
			if (!match.Success)
			{
				matchValue = ConvertMatchValue(PreparseMatchValue(MatchValue));
				return;
			}
			string value = match.Groups[1].Value;
			if (ComicBookMatcher.IsComicProperty(value))
			{
				comicProperty = value;
			}
			else if (ComicBookMatcher.IsSeriesStatsProperty(value))
			{
				seriesStatsProperty = ComicBookMatcher.ParseSeriesProperty(value);
			}
		}

		protected override void OnMatchValue2Changed()
		{
			base.OnMatchValue2Changed();
			Match match = FieldExpression.Match(MatchValue2 ?? string.Empty);
			comicProperty2 = (seriesStatsProperty2 = null);
			if (!match.Success)
			{
				matchValue2 = ConvertMatchValue(PreparseMatchValue(MatchValue2));
				return;
			}
			string value = match.Groups[1].Value;
			if (ComicBookMatcher.IsComicProperty(value))
			{
				comicProperty = value;
			}
			else if (ComicBookMatcher.IsSeriesStatsProperty(value))
			{
				seriesStatsProperty = ComicBookMatcher.ParseSeriesProperty(value);
			}
		}

		protected virtual string PreparseMatchValue(string value)
		{
			return value;
		}

		protected virtual T PreparseValue(T value)
		{
			return value;
		}

		protected virtual T GetMatchValue(ComicBook comicBook)
		{
			if (comicProperty != null)
			{
				return comicBook.GetPropertyValue<T>(comicProperty);
			}
			if (base.StatsProvider != null && seriesStatsProperty != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).GetTypedValue<T>(seriesStatsProperty);
			}
			return matchValue;
		}

		protected virtual T GetMatchValue2(ComicBook comicBook)
		{
			if (comicProperty2 != null)
			{
				return comicBook.GetPropertyValue<T>(comicProperty2);
			}
			if (base.StatsProvider != null && seriesStatsProperty2 != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).GetTypedValue<T>(seriesStatsProperty2);
			}
			return matchValue2;
		}

		protected virtual T ConvertMatchValue(string input)
		{
			try
			{
				return (T)Convert.ChangeType(input, typeof(T));
			}
			catch
			{
				return GetInvalidValue();
			}
		}

		protected virtual T GetInvalidValue()
		{
			return default(T);
		}

		protected abstract bool MatchBook(ComicBook book, T value);

		protected abstract T GetValue(ComicBook comicBook);
	}
}
