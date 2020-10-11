using System;

namespace cYo.Common.Runtime
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class CommandLineSwitchAttribute : Attribute
	{
		public string Name
		{
			get;
			set;
		}

		public string ShortName
		{
			get;
			set;
		}

		public CommandLineSwitchAttribute(string name, string shortName)
		{
			Name = name;
			ShortName = shortName;
		}

		public CommandLineSwitchAttribute(string name)
			: this(name, null)
		{
		}

		public CommandLineSwitchAttribute()
			: this(null)
		{
		}
	}
}
