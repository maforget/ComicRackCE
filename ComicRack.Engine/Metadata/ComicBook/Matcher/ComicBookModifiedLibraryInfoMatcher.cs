using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Modified Library Info")]
	[ComicBookMatcherHint("ComicBookIsDirty")]
	public class ComicBookModifiedLibraryInfoMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (!comicBook.ComicBookIsDirty)
			{
				return YesNo.No;
			}
			return YesNo.Yes;
		}
	}
}
