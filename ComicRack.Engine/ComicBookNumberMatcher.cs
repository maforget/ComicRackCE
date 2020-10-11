using System;
using System.ComponentModel;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Number")]
	[ComicBookMatcherHint("Number", "FilePath", "EnableProposed")]
	public class ComicBookNumberMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			if (!comicBook.ShadowNumber.TryParse(out float f, invariant: true))
			{
				return -1f;
			}
			return f;
		}
	}
}
