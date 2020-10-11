using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	public abstract class ShareableComicListItem : ComicListItem, ICloneable
	{
		public const string ClipboardFormat = "ComicList";

		private bool quickOpen;

		[XmlAttribute]
		[DefaultValue(false)]
		public virtual bool QuickOpen
		{
			get
			{
				return quickOpen;
			}
			set
			{
				if (quickOpen != value)
				{
					quickOpen = value;
					OnChanged(ComicListItemChange.Other);
				}
			}
		}

		public abstract object Clone();
	}
}
