using System;

namespace cYo.Common.Runtime
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class IniFileAttribute : Attribute
	{
		private bool enabled = true;

		public string Name
		{
			get;
			set;
		}

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
			}
		}

		public IniFileAttribute(bool enabled)
		{
			this.enabled = enabled;
		}
	}
}
