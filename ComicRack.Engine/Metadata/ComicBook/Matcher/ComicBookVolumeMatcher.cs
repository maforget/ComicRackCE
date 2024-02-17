using System;
using System.ComponentModel;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Volume")]
	[ComicBookMatcherHint("Volume", "FilePath", "EnableProposed")]
	public class ComicBookVolumeMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			return comicBook.ShadowVolume;
		}

		protected override string PreparseMatchValue(string value)
		{
			return base.PreparseMatchValue(value.OnlyDigits());
		}
	}
}
