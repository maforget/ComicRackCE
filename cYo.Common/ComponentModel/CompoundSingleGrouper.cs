using System;
using System.Collections.Generic;
using System.Linq;

namespace cYo.Common.ComponentModel
{
	public class CompoundSingleGrouper<T> : IGrouper<T>
	{
		private class CompoundGroupInfo : ICompoundGroupInfo, IGroupInfo, IComparable<IGroupInfo>
		{
			public IGroupInfo[] Infos
			{
				get;
				private set;
			}

			public object Key
			{
				get;
				private set;
			}

			public string Caption
			{
				get;
				private set;
			}

			public int Index
			{
				get;
				private set;
			}

			public CompoundGroupInfo(IGroupInfo[] infos)
			{
				Infos = infos;
				foreach (IGroupInfo groupInfo in infos)
				{
					if (Key == null)
					{
						Key = groupInfo.Key;
						Caption = groupInfo.Caption;
						Index = groupInfo.Index;
					}
					else
					{
						Key = string.Concat(Key, "/", groupInfo.Key);
						Caption = Caption + " - " + groupInfo.Caption;
					}
				}
			}

			public int CompareTo(IGroupInfo other)
			{
				CompoundGroupInfo compoundGroupInfo = other as CompoundGroupInfo;
				if (compoundGroupInfo == null)
				{
					return GroupInfo.Compare(this, other);
				}
				int num = Infos.Length;
				int num2 = compoundGroupInfo.Infos.Length;
				for (int i = 0; i < Math.Min(num, num2); i++)
				{
					IGroupInfo groupInfo = Infos[i];
					IGroupInfo other2 = compoundGroupInfo.Infos[i];
					int num3 = groupInfo.CompareTo(other2);
					if (num3 != 0)
					{
						return num3;
					}
				}
				return Math.Sign(num - num2);
			}
		}

		public IGrouper<T>[] Groupers
		{
			get;
			private set;
		}

		public bool IsMultiGroup => false;

		public CompoundSingleGrouper(IGrouper<T>[] groupers)
		{
			Groupers = groupers;
			if (Groupers.Any((IGrouper<T> g) => g.IsMultiGroup))
			{
				throw new ArgumentException("Only single groupers are supported");
			}
		}

		public IGroupInfo GetGroup(T item)
		{
			return new CompoundGroupInfo(Groupers.Select((IGrouper<T> g) => g.GetGroup(item)).ToArray());
		}

		public IEnumerable<IGroupInfo> GetGroups(T item)
		{
			throw new NotImplementedException();
		}
	}
}
