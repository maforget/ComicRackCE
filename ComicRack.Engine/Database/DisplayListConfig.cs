using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class DisplayListConfig
	{
		[DefaultValue(null)]
		public ItemViewConfig View
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public ThumbnailConfig Thumbnail
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public TileConfig Tile
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public StacksConfig StackConfig
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string BackgroundImageSource
		{
			get;
			set;
		}

		[DefaultValue(ComicBookAllPropertiesMatcher.ShowOptionType.All)]
		public ComicBookAllPropertiesMatcher.ShowOptionType ShowOptionType
		{
			get;
			set;
		}

		[DefaultValue(ComicBookAllPropertiesMatcher.ShowComicType.All)]
		public ComicBookAllPropertiesMatcher.ShowComicType ShowComicType
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool ShowOnlyDuplicates
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool ShowGroupHeaders
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public int ShowGroupHeadersWidth
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string QuickSearch
		{
			get;
			set;
		}

		[DefaultValue(ComicBookAllPropertiesMatcher.MatcherOption.All)]
		public ComicBookAllPropertiesMatcher.MatcherOption QuickSearchType
		{
			get;
			set;
		}

		[XmlIgnore]
		public Point ScrollPosition
		{
			get;
			set;
		}

		[XmlIgnore]
		public Guid FocusedComicId
		{
			get;
			set;
		}

		[XmlIgnore]
		public Point StackScrollPosition
		{
			get;
			set;
		}

		[XmlIgnore]
		public Guid StackFocusedComicId
		{
			get;
			set;
		}

		[XmlIgnore]
		public Guid StackedComicId
		{
			get;
			set;
		}

		public DisplayListConfig()
		{
		}

		public DisplayListConfig(ItemViewConfig view, ThumbnailConfig thumbnail, TileConfig tile, StacksConfig stackConfig, string background)
		{
			View = view;
			Thumbnail = thumbnail;
			Tile = tile;
			StackConfig = stackConfig;
			BackgroundImageSource = background;
		}
	}
}
