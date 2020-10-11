using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: All complete")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "SeriesComplete", DisableOptimizedUpdate = true)]
	public class SmartListSeriesAllCompleteMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).AllComplete;
			}
			return YesNo.Unknown;
		}
	}
}
