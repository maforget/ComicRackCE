using System;
using System.IO;
using System.Linq;
using cYo.Common.Xml;

namespace cYo.Common.Net.News
{
    public class AtomNewsFeed : NewsFeed
    {
        protected override NewsChannelCollection ParseFeed(string xmlFeed)
        {
            NewsChannelCollection newsChannelCollection = new NewsChannelCollection();
            var feed = XmlUtility.FromString<Atom.feed>(xmlFeed);

            NewsChannel newsChannel = new NewsChannel();
            newsChannel.Title = feed.title;
            newsChannel.Link = feed.links.FirstOrDefault(x => x.rel == "alternate")?.href;

            foreach (Atom.feedEntry item in feed.entries.OrderByDescending(dt => dt.updated))
            {
                NewsChannelItem newsChannelItem = new NewsChannelItem()
                {
                    Author = item.author.name,
                    Title = item.title,
                    Description = item.content.Value,
                    Guid = item.id,
                    Link = item.link.href,
                    Published = item.updated.SafeToLocalTime(),
                    Category = "Commit"
                };

                newsChannel.Items.Add(newsChannelItem);

            }

            newsChannelCollection.Add(newsChannel);
            return newsChannelCollection;
        }


    }
}
