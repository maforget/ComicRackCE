using System;
using System.ComponentModel;
using System.Xml.Serialization;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Text;
using cYo.Common.Windows;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[TypeConverter(typeof(ComicPageInfoConverter))]
	public struct ComicPageInfo
	{
		private static readonly string UnknownText = TR.Default["Unknown", "Unknown"];

		private volatile short imageIndex;

		private volatile ComicPageType pageType;

		private volatile string bookmark;

		private volatile int imageFileSize;

		private volatile short imageWidth;

		private volatile short imageHeight;

		private volatile ImageRotation rotation;

		private volatile ComicPagePosition pagePosition;

		private volatile string key;

		public static readonly ComicPageInfo Empty;

		[XmlAttribute("Image")]
		public int ImageIndex
		{
			get
			{
				return imageIndex - 1;
			}
			set
			{
				imageIndex = (short)(value + 1);
			}
		}

		[XmlIgnore]
		public ComicPageType PageType
		{
			get
			{
				if (pageType != 0)
				{
					return pageType;
				}
				return ComicPageType.Story;
			}
			set
			{
				pageType = ((value == (ComicPageType)0) ? ComicPageType.Story : value);
			}
		}

		[XmlAttribute]
		[DefaultValue(null)]
		public string Bookmark
		{
			get
			{
				return bookmark;
			}
			set
			{
				bookmark = (string.IsNullOrEmpty(value) ? null : value);
			}
		}

		[DefaultValue(0)]
		[XmlAttribute("ImageSize")]
		public int ImageFileSize
		{
			get
			{
				return imageFileSize;
			}
			set
			{
				imageFileSize = value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int ImageWidth
		{
			get
			{
				return imageWidth;
			}
			set
			{
				imageWidth = (short)value;
			}
		}

		[DefaultValue(0)]
		[XmlAttribute]
		public int ImageHeight
		{
			get
			{
				return imageHeight;
			}
			set
			{
				imageHeight = (short)value;
			}
		}

		[DefaultValue(ImageRotation.None)]
		[XmlAttribute]
		public ImageRotation Rotation
		{
			get
			{
				return rotation;
			}
			set
			{
				rotation = value;
			}
		}

		[DefaultValue(ComicPagePosition.Default)]
		[XmlAttribute]
		public ComicPagePosition PagePosition
		{
			get
			{
				return pagePosition;
			}
			set
			{
				pagePosition = value;
			}
		}

		[DefaultValue(null)]
		[XmlAttribute]
		public string Key
		{
			get
			{
				return key;
			}
			set
			{
				key = value;
			}
		}

		[XmlAttribute("Type")]
		[Browsable(false)]
		[DefaultValue("Story")]
		public string TypeSerialized
		{
			get
			{
				return PageType.ToString();
			}
			set
			{
				string value2 = value.Replace("Advertisment", "Advertisement");
				if (Enum.TryParse<ComicPageType>(value2, out var result))
				{
					pageType = result;
				}
			}
		}

		public string PageTypeAsText => LocalizeUtility.LocalizeEnum(typeof(ComicPageType), (int)PageType);

		public string PagePositionAsText => LocalizeUtility.LocalizeEnum(typeof(ComicPagePosition), (int)PagePosition);

		public string ImageFileSizeAsText
		{
			get
			{
				long num = ImageFileSize;
				if (num > 0)
				{
					return string.Format(new FileLengthFormat(), "{0}", new object[1]
					{
						num
					});
				}
				return UnknownText;
			}
		}

		public string ImageWidthAsText
		{
			get
			{
				if (ImageWidth != 0)
				{
					return ImageWidth.ToString();
				}
				return UnknownText;
			}
		}

		public string ImageHeightAsText
		{
			get
			{
				if (ImageHeight != 0)
				{
					return ImageHeight.ToString();
				}
				return UnknownText;
			}
		}

		public string RotationAsText => $"{rotation.ToDegrees()}°";

		public bool IsBookmark => !string.IsNullOrEmpty(Bookmark);

		public bool IsEmpty => Equals(Empty);

		public bool IsDoublePage => ImageWidth > ImageHeight;

		public bool IsFrontCover => IsTypeOf(ComicPageType.FrontCover);

		public bool IsDeleted => IsTypeOf(ComicPageType.Deleted);

		public bool IsSinglePageType => IsTypeOf(ComicPageType.FrontCover | ComicPageType.BackCover);

		public bool IsSingleRightPageType => IsTypeOf(ComicPageType.FrontCover);

		public ComicPageInfo(int index)
		{
			imageHeight = 0;
			imageWidth = 0;
			bookmark = null;
			imageFileSize = 0;
			rotation = ImageRotation.None;
			pagePosition = ComicPagePosition.Default;
			imageIndex = (short)(index + 1);
			pageType = ((index == 0) ? ComicPageType.FrontCover : ComicPageType.Story);
			key = null;
		}

		public bool IsTypeOf(ComicPageType type)
		{
			return (PageType & type) != 0;
		}

		public bool IsDefaultContent(int index)
		{
			if ((index == -1 || imageIndex == index + 1) && ImageWidth == 0 && ImageHeight == 0 && PageType == ComicPageType.Story && ImageFileSize == 0)
			{
				return Bookmark == null;
			}
			return false;
		}

		public string GetStringValue(string propName)
		{
			try
			{
				return GetType().GetProperty(propName).GetValue(this, null) as string;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is ComicPageInfo))
			{
				return false;
			}
			ComicPageInfo comicPageInfo = (ComicPageInfo)obj;
			if (imageIndex == comicPageInfo.imageIndex && pageType == comicPageInfo.pageType && imageHeight == comicPageInfo.imageHeight && imageWidth == comicPageInfo.imageWidth && bookmark == comicPageInfo.bookmark)
			{
				return pagePosition == comicPageInfo.pagePosition;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return imageIndex.GetHashCode();
		}

		public override string ToString()
		{
			TypeConverter converter = TypeDescriptor.GetConverter(this);
			return converter.ConvertToString(this);
		}

		public static bool operator ==(ComicPageInfo a, ComicPageInfo b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(ComicPageInfo a, ComicPageInfo b)
		{
			return !(a == b);
		}

		public static ComicPageInfo Parse(string text)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(ComicPageInfo));
			return (ComicPageInfo)converter.ConvertFromString(text);
		}
	}
}
