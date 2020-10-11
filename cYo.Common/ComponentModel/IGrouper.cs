using System.Collections.Generic;

namespace cYo.Common.ComponentModel
{
	public interface IGrouper<T>
	{
		bool IsMultiGroup
		{
			get;
		}

		IGroupInfo GetGroup(T item);

		IEnumerable<IGroupInfo> GetGroups(T item);
	}
}
