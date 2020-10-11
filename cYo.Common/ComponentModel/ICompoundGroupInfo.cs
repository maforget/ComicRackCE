using System;

namespace cYo.Common.ComponentModel
{
	public interface ICompoundGroupInfo : IGroupInfo, IComparable<IGroupInfo>
	{
		IGroupInfo[] Infos
		{
			get;
		}
	}
}
