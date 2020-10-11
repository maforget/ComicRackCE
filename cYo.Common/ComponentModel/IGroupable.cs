using System.Collections.Generic;

namespace cYo.Common.ComponentModel
{
	public interface IGroupable
	{
		bool IsMultiGroup
		{
			get;
		}

		IGroupInfo GetGroup();

		IEnumerable<IGroupInfo> GetGroups();
	}
}
