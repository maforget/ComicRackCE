using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Start of Gap")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Number", DisableOptimizedUpdate = true)]
	public class SmartListSeriesStartOfGapMatcher : ComicBookYesNoMatcher
	{
		protected override YesNo GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				if (!base.StatsProvider.GetSeriesStats(comicBook).IsGapStart(comicBook))
				{
					return YesNo.No;
				}
				return YesNo.Yes;
			}
			return YesNo.Unknown;
		}
	}
}
