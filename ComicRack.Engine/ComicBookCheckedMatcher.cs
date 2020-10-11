using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Is Checked")]
	[ComicBookMatcherHint("Checked")]
	public class ComicBookCheckedMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (!comicBook.Checked)
			{
				return YesNo.No;
			}
			return YesNo.Yes;
		}
	}
}
