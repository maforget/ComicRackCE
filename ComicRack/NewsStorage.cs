using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using cYo.Common.Net.News;
using cYo.Common.Xml;

namespace cYo.Projects.ComicRack.Viewer
{
	[Serializable]
	public class NewsStorage
	{
		[Serializable]
		public class Subscription
		{
			private string url = string.Empty;

			private string comment = string.Empty;

			private readonly NewsChannelCollection channels = new NewsChannelCollection();

			private DateTime lastTimeRead = DateTime.MinValue;

			[XmlAttribute]
			public string Url
			{
				get
				{
					return url;
				}
				set
				{
					url = value;
				}
			}

			[DefaultValue("")]
			public string Comment
			{
				get
				{
					return comment;
				}
				set
				{
					comment = value;
				}
			}

			public NewsChannelCollection Channels => channels;

			public DateTime LastUpdate
			{
				get
				{
					return lastTimeRead;
				}
				set
				{
					lastTimeRead = value;
				}
			}

			public Subscription()
			{
			}

			public Subscription(string url, string comment)
			{
				this.url = url;
				this.comment = comment;
			}
		}

		[Serializable]
		public class SubscriptionCollection : List<Subscription>
		{
        }

		[Serializable]
		public class NewsChannelItemInfo
		{
			[XmlAttribute]
			public string Guid
			{
				get;
				set;
			}

			[XmlAttribute]
			[DefaultValue(false)]
			public bool IsRead
			{
				get;
				set;
			}

			public NewsChannelItemInfo()
			{
			}

			public NewsChannelItemInfo(NewsChannelItem nci)
			{
				Guid = nci.Guid;
			}
		}

		[Serializable]
		public class NewsChannelItemInfoCollection : List<NewsChannelItemInfo>
		{
			public NewsChannelItemInfo this[NewsChannelItem item]
			{
				get
				{
					using (Enumerator enumerator = GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NewsChannelItemInfo current = enumerator.Current;
							if (current.Guid == item.Guid)
							{
								return current;
							}
						}
					}
					NewsChannelItemInfo newsChannelItemInfo = new NewsChannelItemInfo(item);
					Add(newsChannelItemInfo);
					return newsChannelItemInfo;
				}
			}
		}

		private readonly SubscriptionCollection subscriptions = new SubscriptionCollection();

		private readonly NewsChannelItemInfoCollection newsChannelItemInfos = new NewsChannelItemInfoCollection();

		public bool HasUnread => Items.Find((NewsChannelItem item) => !NewsChannelItemInfos[item].IsRead) != null;

		public SubscriptionCollection Subscriptions => subscriptions;

		public NewsChannelItemInfoCollection NewsChannelItemInfos => newsChannelItemInfos;

		[XmlIgnore]
		public NewsChannelCollection Channels
		{
			get
			{
				NewsChannelCollection channels = new NewsChannelCollection();
				subscriptions.ForEach(delegate(Subscription s)
				{
					channels.AddRange(s.Channels);
				});
				return channels;
			}
		}

		[XmlIgnore]
		public NewsChannelItemCollection Items
		{
			get
			{
				NewsChannelItemCollection items = new NewsChannelItemCollection();
				Channels.ForEach(delegate(NewsChannel nc)
				{
					items.AddRange(nc.Items);
				});
				return items;
			}
		}

		public void UpdateFeeds(int minutes)
		{
			DateTime d = DateTime.Now.ToUniversalTime();
			try
			{
				foreach (Subscription subscription in subscriptions)
				{
					if (TimeSpan.FromMinutes(minutes) > d - subscription.LastUpdate)
					{
						continue;
					}
					AtomNewsFeed rssNewsFeed = new AtomNewsFeed();
					try
					{
						rssNewsFeed.ReadFeed(NewsFeed.LoadFeed(subscription.Url));
					}
					catch
					{
					}
					finally
					{
						subscription.Channels.Clear();
						subscription.Channels.AddRange(rssNewsFeed.Channels);
						subscription.LastUpdate = DateTime.UtcNow;
					}
				}
			}
			finally
			{
				UpdateChannelInfo();
			}
		}

		public void MarkAllRead()
		{
			Items.ForEach(delegate(NewsChannelItem item)
			{
				NewsChannelItemInfos[item].IsRead = true;
			});
		}

		private void UpdateChannelInfo()
		{
			NewsChannelItemCollection items = Items;
			newsChannelItemInfos.RemoveAll((NewsChannelItemInfo ci) => items[ci.Guid] == null);
		}

		public static NewsStorage Load(string file)
		{
			try
			{
				return XmlUtility.Load<NewsStorage>(file);
			}
			catch (Exception)
			{
				return new NewsStorage();
			}
		}

		public void Save(string file)
		{
			XmlUtility.Store(file, this);
		}
	}
}
