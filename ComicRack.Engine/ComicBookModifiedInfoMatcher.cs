using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Modified Info")]
	[ComicBookMatcherHint("ComicInfoIsDirty")]
	public class ComicBookModifiedInfoMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (!comicBook.ComicInfoIsDirty)
			{
				return YesNo.No;
			}
			return YesNo.Yes;
		}
	}
}
