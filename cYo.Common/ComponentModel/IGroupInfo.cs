using System;

namespace cYo.Common.ComponentModel
{
	public interface IGroupInfo : IComparable<IGroupInfo>
	{
		object Key
		{
			get;
		}

		string Caption
		{
			get;
		}

		int Index
		{
			get;
		}
	}
}
