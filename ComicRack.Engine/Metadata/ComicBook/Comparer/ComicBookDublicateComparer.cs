using System.Collections.Generic;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookDublicateComparer : Comparer<ComicBook>
	{
		public override int Compare(ComicBook x, ComicBook y)
		{
			int num = string.Compare(x.FilePath, y.FilePath, ignoreCase: true);
			if (num != 0)
			{
				return num;
			}
			num = string.Compare(GroupInfo.CompressedName(x.ShadowSeries), GroupInfo.CompressedName(y.ShadowSeries), ignoreCase: true);
			if (num != 0)
			{
				return num;
			}
			num = string.Compare(x.ShadowFormat, y.ShadowFormat);
			if (num != 0)
			{
				return num;
			}
			num = x.ShadowVolume.CompareTo(y.ShadowVolume);
			if (num != 0)
			{
				return num;
			}
			num = string.Compare(x.ShadowNumber, y.ShadowNumber);
			if (num != 0)
			{
				return num;
			}
			num = string.Compare(x.LanguageISO, y.LanguageISO);
			if (num != 0)
			{
				return num;
			}
			if (x.ShadowYear >= 0 || y.ShadowYear >= 0)
			{
				num = x.ShadowYear.CompareTo(y.ShadowYear);
				if (num != 0)
				{
					return num;
				}
			}
			if (x.Month >= 0 || y.Month >= 0)
			{
				num = x.Month.CompareTo(y.Month);
				if (num != 0)
				{
					return num;
				}
			}
			if (x.Day >= 0 || y.Day >= 0)
			{
				num = x.Day.CompareTo(y.Day);
				if (num != 0)
				{
					return num;
				}
			}
			return x.BlackAndWhite.CompareTo(y.BlackAndWhite);
		}
	}
}
