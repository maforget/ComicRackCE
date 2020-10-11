namespace cYo.Common.Net.News
{
	public class NewsChannel
	{
		private readonly NewsChannelItemCollection items = new NewsChannelItemCollection();

		public string Title
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Link
		{
			get;
			set;
		}

		public string Editor
		{
			get;
			set;
		}

		public NewsChannelItemCollection Items => items;
	}
}
