using System;
using System.Collections.Generic;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public static class ComicBookDuplicateComparer
	{
		public class EqualityComparer : IEqualityComparer<ComicBook>
		{
			public bool Equals(ComicBook x, ComicBook y)
			{
				if (x == null || y == null)
					return false;

				// Check metadata match
				bool metaEqual =
					string.Equals(GroupInfo.CompressedName(x.ShadowSeries), GroupInfo.CompressedName(y.ShadowSeries), StringComparison.OrdinalIgnoreCase) &&
					string.Equals(x.ShadowFormat, y.ShadowFormat) &&
					x.ShadowVolume == y.ShadowVolume &&
					string.Equals(x.ShadowNumber, y.ShadowNumber) &&
					string.Equals(x.LanguageISO, y.LanguageISO) &&
					(x.ShadowYear >= 0 || y.ShadowYear >= 0) ? x.ShadowYear == y.ShadowYear : true &&
					(x.Month >= 0 || y.Month >= 0) ? x.Month == y.Month : true &&
					(x.Day >= 0 || y.Day >= 0) ? x.Day == y.Day : true &&
					x.BlackAndWhite == y.BlackAndWhite;

				return metaEqual;
			}

			public int GetHashCode(ComicBook obj)
			{
				if (obj == null)
					return 0;

				int hash = 17;
				hash = hash * 23 + GroupInfo.CompressedName(obj.ShadowSeries)?.GetHashCode() ?? 0;
				hash = hash * 23 + (obj.ShadowFormat?.GetHashCode() ?? 0);
				hash = hash * 23 + obj.ShadowVolume.GetHashCode();
				hash = hash * 23 + (obj.ShadowNumber?.GetHashCode() ?? 0);
				hash = hash * 23 + (obj.LanguageISO?.GetHashCode() ?? 0);
				hash = hash * 23 + (obj.ShadowYear >= 0 ? obj.ShadowYear.GetHashCode() : 0);
				hash = hash * 23 + (obj.Month >= 0 ? obj.Month.GetHashCode() : 0);
				hash = hash * 23 + (obj.Day >= 0 ? obj.Day.GetHashCode() : 0);
				hash = hash * 23 + obj.BlackAndWhite.GetHashCode();
				return hash;
			}
		}

		public class FilePathComparer : IEqualityComparer<ComicBook>
		{
			public bool Equals(ComicBook x, ComicBook y)
			{
				if (x == null || y == null)
					return false;

				// Don't compare file paths for fileless comics
				return x.IsLinked && y.IsLinked && string.Equals(x.FilePath, y.FilePath, StringComparison.OrdinalIgnoreCase);
			}

			public int GetHashCode(ComicBook obj)
			{
				if (obj == null || !obj.IsLinked)
					return 0;

				return obj.FilePath?.GetHashCode() ?? 0;
			}
		}
	}
}
