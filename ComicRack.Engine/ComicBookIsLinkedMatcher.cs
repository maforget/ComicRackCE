using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Is Linked")]
	[ComicBookMatcherHint("FilePath")]
	public class ComicBookIsLinkedMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (!comicBook.IsLinked)
			{
				return YesNo.No;
			}
			return YesNo.Yes;
		}
	}
}
