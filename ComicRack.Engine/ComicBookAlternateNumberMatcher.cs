using System;
using System.ComponentModel;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Alternate Number")]
	[ComicBookMatcherHint("AlternateNumber")]
	public class ComicBookAlternateNumberMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			if (!comicBook.AlternateNumber.TryParse(out float f, invariant: true))
			{
				return -1f;
			}
			return f;
		}
	}
}
