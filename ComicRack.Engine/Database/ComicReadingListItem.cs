using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine.Database
{
	public class ComicReadingListItem
	{
		[XmlAttribute]
		[DefaultValue("")]
		public string Series
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue("")]
		public string Number
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(-1)]
		public int Volume
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue(-1)]
		public int Year
		{
			get;
			set;
		}

		[XmlAttribute]
		[DefaultValue("")]
		public string Format
		{
			get;
			set;
		}

		public Guid Id
		{
			get;
			set;
		}

		[DefaultValue("")]
		public string FileName
		{
			get;
			set;
		}

		public ComicReadingListItem()
		{
			Id = Guid.Empty;
			Volume = -1;
			Year = -1;
			Series = string.Empty;
			FileName = string.Empty;
			Format = string.Empty;
			Number = string.Empty;
		}

		public ComicReadingListItem(ComicBook cb, bool withFilename)
		{
			Series = cb.ShadowSeries;
			Number = cb.ShadowNumber;
			Volume = cb.ShadowVolume;
			Year = cb.ShadowYear;
			Format = cb.ShadowFormat;
			Id = cb.Id;
			FileName = (withFilename ? cb.FileName : string.Empty);
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(Series))
			{
				return ComicBook.FormatTitle(ComicBook.DefaultCaptionFormat, Series, null, ComicBook.FormatVolume(Volume), Number, ComicBook.FormatYear(Year), null, null, Format, FileName);
			}
			return FileName;
		}

		public void SetInfo(ComicNameInfo cni, bool onlyEmpty = false)
		{
			if (!onlyEmpty || string.IsNullOrEmpty(Series))
			{
				Series = cni.Series;
			}
			if (!onlyEmpty || string.IsNullOrEmpty(Number))
			{
				Number = cni.Number;
			}
			if (!onlyEmpty || Volume == -1)
			{
				Volume = cni.Volume;
			}
			if (!onlyEmpty || Year == -1)
			{
				Year = cni.Year;
			}
			if (!onlyEmpty || string.IsNullOrEmpty(Format))
			{
				Format = cni.Format;
			}
		}

		public void SetFileNameInfo()
		{
			if (!string.IsNullOrEmpty(FileName))
			{
				SetInfo(ComicNameInfo.FromFilePath(FileName));
			}
		}
	}
}
