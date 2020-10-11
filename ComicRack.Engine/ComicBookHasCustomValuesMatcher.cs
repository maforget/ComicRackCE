using System;
using System.ComponentModel;
using System.Linq;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Has Custom Values")]
	[ComicBookMatcherHint("CustomValuesStore")]
	public class ComicBookHasCustomValuesMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (!comicBook.GetCustomValues().Any())
			{
				return YesNo.No;
			}
			return YesNo.Yes;
		}
	}
}
