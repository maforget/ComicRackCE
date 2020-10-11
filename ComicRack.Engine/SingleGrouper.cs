using System;
using System.Collections.Generic;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public abstract class SingleGrouper<T> : IGrouper<T>
	{
		public bool IsMultiGroup => false;

		public abstract IGroupInfo GetGroup(T item);

		public IEnumerable<IGroupInfo> GetGroups(T item)
		{
			throw new NotImplementedException();
		}
	}
}
