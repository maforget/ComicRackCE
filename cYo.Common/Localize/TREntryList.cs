using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.Threading;

namespace cYo.Common.Localize
{
	public class TREntryList : SmartList<TREntry>
	{
		private readonly Dictionary<string, TREntry> rs = new Dictionary<string, TREntry>();

		private readonly TR owner;

		public IEnumerable<string> Keys => rs.Keys.Lock();

		public TREntry this[string key]
		{
			get
			{
				if (!rs.TryGetValue(key, out var value))
				{
					return null;
				}
				return value;
			}
		}

		public TREntryList(TR owner)
		{
			this.owner = owner;
		}

		protected override void OnInsertCompleted(int index, TREntry item)
		{
			rs[item.Key] = item;
			item.Resource = owner;
			base.OnInsertCompleted(index, item);
		}

		protected override void OnRemoveCompleted(int index, TREntry item)
		{
			rs.Remove(item.Key);
			item.Resource = null;
			base.OnRemoveCompleted(index, item);
		}

		public string GetText(string key, string value = null)
		{
			if (string.IsNullOrEmpty(key))
			{
				return value;
			}
			using (ItemMonitor.Lock(base.SyncRoot))
			{
				if (rs.TryGetValue(key, out var value2) && !string.IsNullOrEmpty(value2.Text))
				{
					return value2.Text;
				}
				return value;
			}
		}

		public void Merge(IEnumerable<TREntry> list)
		{
			using (ItemMonitor.Lock(base.SyncRoot))
			{
				foreach (TREntry item in list)
				{
					TREntry tREntry = this[item.Key];
					if (tREntry != null)
					{
						Remove(tREntry);
					}
					Add(item);
				}
			}
		}

		public void Update(TREntryList list)
		{
			string[] array = Keys.ToArray();
			foreach (string key in array)
			{
				if (list[key] == null)
				{
					Remove(this[key]);
				}
			}
			foreach (TREntry item in list)
			{
				TREntry tREntry = this[item.Key];
				if (tREntry == null)
				{
					Add(new TREntry(item.Key, item.Comment, item.Comment));
				}
				else if (tREntry.Comment != item.Comment)
				{
					Remove(tREntry);
					Add(new TREntry(item.Key, tREntry.Text, item.Comment));
				}
			}
		}
	}
}
