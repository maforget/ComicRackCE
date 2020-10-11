using System.Collections.Generic;

namespace cYo.Common.Net.News
{
	public class NewsChannelItemCollection : List<NewsChannelItem>
	{
		public NewsChannelItem this[string guid] => Find((NewsChannelItem nci) => nci.Guid == guid);
	}
}
