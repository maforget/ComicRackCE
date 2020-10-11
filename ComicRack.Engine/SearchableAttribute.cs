using System;
using System.Linq;
using System.Reflection;

namespace cYo.Projects.ComicRack.Engine
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class SearchableAttribute : Attribute
	{
		public bool Searchable
		{
			get;
			private set;
		}

		public SearchableAttribute(bool searchable)
		{
			Searchable = searchable;
		}

		public SearchableAttribute()
			: this(searchable: true)
		{
		}

		public static bool IsSearchable(PropertyInfo pi)
		{
			return pi.GetCustomAttributes(inherit: true).OfType<SearchableAttribute>().FirstOrDefault((SearchableAttribute a) => a.Searchable) != null;
		}
	}
}
