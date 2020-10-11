using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: End of Gap")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Number", DisableOptimizedUpdate = true)]
	public class SmartListSeriesEndOfGapMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				if (!base.StatsProvider.GetSeriesStats(comicBook).IsGapEnd(comicBook))
				{
					return YesNo.No;
				}
				return YesNo.Yes;
			}
			return YesNo.Unknown;
		}
	}
}
