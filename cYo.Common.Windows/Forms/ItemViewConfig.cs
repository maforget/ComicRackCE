using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace cYo.Common.Windows.Forms
{
	[Serializable]
	public class ItemViewConfig
	{
		private List<ItemViewColumnInfo> columns = new List<ItemViewColumnInfo>();

		private ItemViewMode itemViewMode = ItemViewMode.Detail;

		private SortOrder itemSortOrder = SortOrder.Ascending;

		private SortOrder groupSortOrder = SortOrder.Ascending;

		[XmlArrayItem("Column")]
		public List<ItemViewColumnInfo> Columns
		{
			get
			{
				return columns;
			}
			set
			{
				columns = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(ItemViewMode.Detail)]
		public ItemViewMode ItemViewMode
		{
			get
			{
				return itemViewMode;
			}
			set
			{
				itemViewMode = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(false)]
		public bool Grouping
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(null)]
		public string SortKey
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(null)]
		public string GrouperId
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(null)]
		public string StackerId
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(SortOrder.Ascending)]
		public SortOrder ItemSortOrder
		{
			get
			{
				return itemSortOrder;
			}
			set
			{
				itemSortOrder = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(SortOrder.Ascending)]
		public SortOrder GroupSortOrder
		{
			get
			{
				return groupSortOrder;
			}
			set
			{
				groupSortOrder = value;
			}
		}

		[DefaultValue(null)]
		public ItemViewGroupsStatus GroupsStatus
		{
			get;
			set;
		}

		public Size ThumbnailSize
		{
			get;
			set;
		}

		public Size TileSize
		{
			get;
			set;
		}

		public int ItemRowHeight
		{
			get;
			set;
		}
	}
}
