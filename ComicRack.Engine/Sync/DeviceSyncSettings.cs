using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Engine.Sync
{
	[Serializable]
	public class DeviceSyncSettings
	{
		[Serializable]
		public enum LimitType
		{
			Books,
			MB,
			GB
		}

		[Serializable]
		public enum ListSort
		{
			Random,
			Series,
			AlternateSeries,
			Published,
			Added,
			StoryArc,
			ListOrder
		}

		[Serializable]
		public class SharedListSettings
		{
			[XmlAttribute]
			[DefaultValue(false)]
			public bool OptimizePortable
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool OnlyUnread
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool KeepLastRead
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool OnlyChecked
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool Limit
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(true)]
			public bool Sort
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(50)]
			public int LimitValue
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(LimitType.Books)]
			public LimitType LimitValueType
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(ListSort.Series)]
			public ListSort ListSortType
			{
				get;
				set;
			}

			public SharedListSettings()
			{
				LimitValue = 50;
				LimitValueType = LimitType.Books;
				ListSortType = ListSort.Series;
				Sort = true;
			}

			public SharedListSettings(SharedListSettings other)
			{
				OptimizePortable = other.OptimizePortable;
				OnlyUnread = other.OnlyUnread;
				OnlyChecked = other.OnlyChecked;
				KeepLastRead = other.KeepLastRead;
				Limit = other.Limit;
				Sort = other.Sort;
				LimitValue = other.LimitValue;
				LimitValueType = other.LimitValueType;
				ListSortType = other.ListSortType;
			}
		}

		[Serializable]
		public class SharedList : SharedListSettings
		{
			[XmlAttribute]
			public Guid ListId
			{
				get;
				set;
			}

			public SharedList()
			{
			}

			public SharedList(SharedList other)
				: base(other)
			{
				ListId = other.ListId;
			}

			public SharedList(Guid listId, SharedListSettings other)
				: base(other)
			{
				ListId = listId;
			}
		}

		public const string ClipboardFormat = "DeviceSyncSettings";

		[XmlAttribute("Name")]
		public string DeviceName
		{
			get;
			set;
		}

		[XmlAttribute("Key")]
		public string DeviceKey
		{
			get;
			set;
		}

		public SmartList<SharedList> Lists
		{
			get;
		} = new SmartList<SharedList>();


		public SharedListSettings DefaultListSettings
		{
			get;
			set;
		} = new SharedListSettings
		{
			OnlyUnread = true
		};


		public DeviceSyncSettings()
		{
		}

		public DeviceSyncSettings(DeviceSyncSettings sds)
		{
			DeviceName = sds.DeviceName;
			DeviceKey = sds.DeviceKey;
			Lists.AddRange(sds.Lists.Select((SharedList sl) => new SharedList(sl)));
		}

		public override int GetHashCode()
		{
			return DeviceKey.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			DeviceSyncSettings deviceSyncSettings = obj as DeviceSyncSettings;
			if (deviceSyncSettings != null)
			{
				return deviceSyncSettings.DeviceKey == DeviceKey;
			}
			return false;
		}
	}
}
