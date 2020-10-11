using System;
using System.Collections.Generic;

namespace cYo.Common.ComponentModel
{
	public interface IGroupContainer<T> : IGroupInfo, IComparable<IGroupInfo>
	{
		IGroupInfo Info
		{
			get;
			set;
		}

		List<T> Items
		{
			get;
		}
	}
}
