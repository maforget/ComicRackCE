using System;
using System.Collections.Generic;

namespace cYo.Common.ComponentModel
{
	public class GroupContainer<T> : IGroupContainer<T>, IGroupInfo, IComparable<IGroupInfo>
	{
		private readonly List<T> items = new List<T>();

		public IGroupInfo Info
		{
			get;
			set;
		}

		public List<T> Items => items;

		public string Caption => Info.Caption;

		public object Key => Info.Key;

		public int Index => Info.Index;

		public int CompareTo(IGroupInfo other)
		{
			return Info.CompareTo(other);
		}
	}
}
