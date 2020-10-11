using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using cYo.Projects.ComicRack.Engine.Drawing;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public class ThumbnailConfig : ICloneable
	{
		private readonly List<int> captionIds = new List<int>();

		[XmlAttribute]
		[DefaultValue(false)]
		public bool HideCaptions
		{
			get;
			set;
		}

		[XmlArray("Lines")]
		[XmlArrayItem("Id")]
		public List<int> CaptionIds => captionIds;

		[DefaultValue(ComicTextElements.DefaultFileComic)]
		public ComicTextElements TextElements
		{
			get;
			set;
		}

		public ThumbnailConfig()
		{
			TextElements = ComicTextElements.DefaultFileComic;
		}

		public ThumbnailConfig(ThumbnailConfig cfg)
		{
			CaptionIds.AddRange(cfg.CaptionIds);
			HideCaptions = cfg.HideCaptions;
			TextElements = cfg.TextElements;
		}

		public object Clone()
		{
			return new ThumbnailConfig(this);
		}
	}
}
