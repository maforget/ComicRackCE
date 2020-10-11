using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace cYo.Common.Windows.Forms
{
	[Serializable]
	public class ItemViewColumnInfo
	{
		private bool visible = true;

		private int width = 80;

		private readonly string name;

		[NonSerialized]
		private readonly object tag;

		private DateTime lastTimeVisible = DateTime.MinValue;

		[XmlAttribute]
		[DefaultValue(0)]
		public int Id
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(0)]
		public int FormatId
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(true)]
		public bool Visible
		{
			get
			{
				return visible;
			}
			set
			{
				visible = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(80)]
		public int Width
		{
			get
			{
				return width;
			}
			set
			{
				width = value;
			}
		}

		public string Name => name;

		public object Tag => tag;

		[DefaultValue(typeof(DateTime), "0001-01-01T00:00:00")]
		public DateTime LastTimeVisible
		{
			get
			{
				return lastTimeVisible;
			}
			set
			{
				lastTimeVisible = value;
			}
		}

		public ItemViewColumnInfo()
		{
		}

		public ItemViewColumnInfo(IColumn header)
		{
			Id = header.Id;
			FormatId = header.FormatId;
			Visible = header.Visible;
			Width = header.Width;
			name = header.Text;
			tag = header.Tag;
			lastTimeVisible = header.LastTimeVisible;
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}
			return string.Empty;
		}
	}
}
