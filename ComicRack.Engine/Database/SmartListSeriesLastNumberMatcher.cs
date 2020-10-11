using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace cYo.Projects.ComicRack.Engine.Database
{
	[Serializable]
	[Description("Series: Last Number")]
	[ComicBookMatcherHint("Series", "Volume", "FilePath", "EnableProposed", "Number", DisableOptimizedUpdate = true)]
	[XmlType("SmartListSeriesMaxNumbertMatcher")]
	public class SmartListSeriesLastNumberMatcher : ComicBookNumericMatcher
	{
		protected override float GetValue(ComicBook comicBook)
		{
			if (base.StatsProvider != null)
			{
				return base.StatsProvider.GetSeriesStats(comicBook).LastNumber;
			}
			return 0f;
		}
	}
}
