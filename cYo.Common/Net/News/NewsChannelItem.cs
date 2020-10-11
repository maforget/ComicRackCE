using System;
using System.ComponentModel;

namespace cYo.Common.Net.News
{
	public class NewsChannelItem
	{
		[DefaultValue(null)]
		public string Guid
		{
			get;
			set;
		}

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

		[DefaultValue(null)]
		public string Author
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string Category
		{
			get;
			set;
		}

		public DateTime Published
		{
			get;
			set;
		}
	}
}
