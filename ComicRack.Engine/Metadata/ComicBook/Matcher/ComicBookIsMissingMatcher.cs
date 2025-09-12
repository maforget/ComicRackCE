using System;
using System.ComponentModel;
using System.IO;

namespace cYo.Projects.ComicRack.Engine
{
	[Serializable]
	[Description("Is Missing")]
	[ComicBookMatcherHint("FileIsMissing", DisableOptimizedUpdate = true)]
	public class ComicBookIsMissingMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
            if (comicBook.IsLinked && comicBook.FileIsMissing)
                return YesNo.Yes;

            return YesNo.No;
        }
	}
}
