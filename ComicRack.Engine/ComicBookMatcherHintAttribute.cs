using System;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ComicBookMatcherHintAttribute : Attribute
	{
		public ISet<string> Properties
		{
			get;
			private set;
		}

		public bool DisableOptimizedUpdate
		{
			get;
			set;
		}

		private ComicBookMatcherHintAttribute(IEnumerable<string> names)
		{
			Properties = new HashSet<string>(names.SelectMany(SplitName));
		}

		public ComicBookMatcherHintAttribute(bool disable)
		{
			DisableOptimizedUpdate = disable;
		}

		public ComicBookMatcherHintAttribute(string name)
			: this(ListExtensions.AsEnumerable<string>(name))
		{
		}

		public ComicBookMatcherHintAttribute(string name1, string name2)
			: this(ListExtensions.AsEnumerable<string>(name1, name2))
		{
		}

		public ComicBookMatcherHintAttribute(string name1, string name2, string name3)
			: this(ListExtensions.AsEnumerable<string>(name1, name2, name3))
		{
		}

		public ComicBookMatcherHintAttribute(string name1, string name2, string name3, string name4)
			: this(ListExtensions.AsEnumerable<string>(name1, name2, name3, name4))
		{
		}

		public ComicBookMatcherHintAttribute(string name1, string name2, string name3, string name4, string name5)
			: this(ListExtensions.AsEnumerable<string>(name1, name2, name3, name4, name5))
		{
		}

		public ComicBookMatcherHintAttribute(string name1, string name2, string name3, string name4, string name5, string name6)
			: this(ListExtensions.AsEnumerable<string>(name1, name2, name3, name4, name5, name6))
		{
		}

		public ComicBookMatcherHintAttribute(string name1, string name2, string name3, string name4, string name5, string name6, string name7)
			: this(ListExtensions.AsEnumerable<string>(name1, name2, name3, name4, name5, name6, name7))
		{
		}

		private static IEnumerable<string> SplitName(string name)
		{
			return name.Split(',').TrimStrings().RemoveEmpty();
		}
	}
}
