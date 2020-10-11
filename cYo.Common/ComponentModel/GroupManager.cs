using System;
using System.Collections.Generic;
using cYo.Common.Collections;
using cYo.Common.Threading;

namespace cYo.Common.ComponentModel
{
	public class GroupManager<T, G> where G : IGroupContainer<T>, new()
	{
		private readonly Dictionary<object, G> groups = new Dictionary<object, G>();

		private IGrouper<T> grouper;

		public IGrouper<T> Grouper
		{
			get
			{
				return grouper;
			}
			set
			{
				if (grouper != value)
				{
					grouper = value;
					Reset();
				}
			}
		}

		public GroupManager(IGrouper<T> grouper = null, IEnumerable<T> items = null)
		{
			this.grouper = grouper;
			if (items != null)
			{
				AddRange(items);
			}
		}

		private void AddGroupItem(IGroupInfo gi, T item)
		{
			if (gi == null)
			{
				return;
			}
			using (ItemMonitor.Lock(groups))
			{
				if (!groups.ContainsKey(gi.Key))
				{
					G val = new G();
					val.Info = gi;
					G value = val;
					groups[gi.Key] = value;
				}
				using (ItemMonitor.Lock(groups[gi.Key].Items))
				{
					groups[gi.Key].Items.Add(item);
				}
			}
		}

		private void AddGroupsItems(IEnumerable<IGroupInfo> gis, T item)
		{
			if (gis == null)
			{
				return;
			}
			foreach (IGroupInfo gi in gis)
			{
				AddGroupItem(gi, item);
			}
		}

		public void Add(T item)
		{
			IGroupable groupable = item as IGroupable;
			if (groupable != null)
			{
				if (groupable.IsMultiGroup)
				{
					AddGroupsItems(groupable.GetGroups(), item);
				}
				else
				{
					AddGroupItem(groupable.GetGroup(), item);
				}
				return;
			}
			if (grouper != null)
			{
				if (grouper.IsMultiGroup)
				{
					AddGroupsItems(grouper.GetGroups(item), item);
					return;
				}
				IGroupInfo group = grouper.GetGroup(item);
				AddGroupItem(group, item);
				return;
			}
			throw new ArgumentException("No grouper available");
		}

		public void AddRange(IEnumerable<T> items)
		{
			items.ParallelForEach(Add);
		}

		public void Reset()
		{
			using (ItemMonitor.Lock(groups))
			{
				groups.Clear();
			}
		}

		public IEnumerable<G> GetGroups()
		{
			using (ItemMonitor.Lock(groups))
			{
				return new List<G>(groups.Values);
			}
		}
	}
	public class GroupManager<T> : GroupManager<T, GroupContainer<T>>
	{
		public GroupManager(IGrouper<T> grouper, IEnumerable<T> items)
			: base(grouper, items)
		{
		}

		public GroupManager(IGrouper<T> grouper)
			: base(grouper, (IEnumerable<T>)null)
		{
		}

		public GroupManager()
			: base((IGrouper<T>)null, (IEnumerable<T>)null)
		{
		}
	}
}
