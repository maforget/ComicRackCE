using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using cYo.Common.Drawing;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageInfoConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			string text = value as string;
			if (text != null)
			{
				if (text == "Empty")
				{
					return ComicPageInfo.Empty;
				}
				string[] array = text.Split(culture.TextInfo.ListSeparator, StringSplitOptions.None).TrimStrings().ToArray();
				ComicPageInfo comicPageInfo = new ComicPageInfo(int.Parse(array[0]));
				comicPageInfo.ImageWidth = int.Parse(array[1]);
				comicPageInfo.ImageHeight = int.Parse(array[2]);
				comicPageInfo.ImageFileSize = int.Parse(array[3]);
				comicPageInfo.Bookmark = array[7];
				ComicPageInfo comicPageInfo2 = comicPageInfo;
				if (Enum.TryParse<ComicPageType>(array[4], out var result))
				{
					comicPageInfo2.PageType = result;
				}
				if (Enum.TryParse<ComicPagePosition>(array[5], out var result2))
				{
					comicPageInfo2.PagePosition = result2;
				}
				if (Enum.TryParse<ImageRotation>(array[6], out var result3))
				{
					comicPageInfo2.Rotation = result3;
				}
				return comicPageInfo2;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				ComicPageInfo comicPageInfo = (ComicPageInfo)value;
				return string.Format(culture, "{1}{0} {2}{0} {3}{0} {4}{0} {5}{0} {6}{0} {7}{0} {8}{0}", culture.TextInfo.ListSeparator, comicPageInfo.ImageIndex, comicPageInfo.ImageWidth, comicPageInfo.ImageHeight, comicPageInfo.ImageFileSize, comicPageInfo.PageType, comicPageInfo.PagePosition, comicPageInfo.Rotation, comicPageInfo.Bookmark);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
