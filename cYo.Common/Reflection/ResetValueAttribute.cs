using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using cYo.Common.Collections;

namespace cYo.Common.Reflection
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	public class ResetValueAttribute : Attribute
	{
		public int Level
		{
			get;
			private set;
		}

		public ResetValueAttribute(int level = 0)
		{
			Level = level;
		}

		public static void ResetProperties(object data, int level = 0)
		{
			if (data != null)
			{
				(from pi in data.GetType().GetProperties()
					where pi.CanWrite && pi.HasAttribute<DefaultValueAttribute>() && pi.HasAttribute<ResetValueAttribute>()
					where pi.GetAttribute<ResetValueAttribute>().Level <= level
					select pi).ForEach(delegate(PropertyInfo pi)
				{
					pi.SetValue(data, pi.DefaultValue(), null);
				});
			}
		}
	}
}
