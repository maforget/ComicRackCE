namespace cYo.Common.Net.News
{
	public abstract class NewsFeed
	{
		private string rawFeed;

		private NewsChannelCollection channels = new NewsChannelCollection();

		public string RawFeed => rawFeed;

		public NewsChannelCollection Channels => channels;

		public void ReadFeed(string feed)
		{
			if (!(rawFeed == feed))
			{
				rawFeed = feed;
				channels = ParseFeed(feed);
			}
		}

		protected abstract NewsChannelCollection ParseFeed(string xmlFeed);

		public static string LoadFeed(string url)
		{
			return HttpAccess.ReadText(url);
		}
	}
}
