using System;
using System.IO;
using System.Xml;

namespace cYo.Common.Net.News
{
	public class RssNewsFeed : NewsFeed
	{
		protected override NewsChannelCollection ParseFeed(string xmlFeed)
		{
			NewsChannelCollection newsChannelCollection = new NewsChannelCollection();
			using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xmlFeed)))
			{
				xmlTextReader.MoveToContent();
				xmlTextReader.ReadStartElement("rss");
				while (xmlTextReader.IsStartElement("channel"))
				{
					NewsChannel newsChannel = new NewsChannel();
					xmlTextReader.ReadStartElement();
					while (xmlTextReader.IsStartElement())
					{
						switch (xmlTextReader.Name)
						{
						case "title":
							newsChannel.Title = xmlTextReader.ReadElementContentAsString();
							break;
						case "description":
							newsChannel.Description = xmlTextReader.ReadElementContentAsString();
							break;
						case "link":
							newsChannel.Link = xmlTextReader.ReadElementContentAsString();
							break;
						case "managingEditor":
							newsChannel.Editor = xmlTextReader.ReadElementContentAsString();
							break;
						case "item":
							newsChannel.Items.Add(ParseItem(xmlTextReader));
							break;
						default:
							xmlTextReader.Skip();
							break;
						}
					}
					xmlTextReader.ReadEndElement();
					newsChannelCollection.Add(newsChannel);
				}
				xmlTextReader.ReadEndElement();
				return newsChannelCollection;
			}
		}

		protected static NewsChannelItem ParseItem(XmlReader reader)
		{
			NewsChannelItem newsChannelItem = new NewsChannelItem();
			reader.ReadStartElement("item");
			while (reader.IsStartElement())
			{
				switch (reader.Name)
				{
				case "guid":
					newsChannelItem.Guid = reader.ReadElementContentAsString();
					break;
				case "pubDate":
				{
					string s = reader.ReadElementContentAsString();
					if (DateTime.TryParse(s, out var result))
					{
						newsChannelItem.Published = result;
					}
					else
					{
						newsChannelItem.Published = DateTime.Now;
					}
					break;
				}
				case "title":
					newsChannelItem.Title = reader.ReadElementContentAsString();
					break;
				case "description":
					newsChannelItem.Description = reader.ReadElementContentAsString();
					break;
				case "category":
					newsChannelItem.Category = reader.ReadElementContentAsString();
					break;
				case "link":
					newsChannelItem.Link = reader.ReadElementContentAsString();
					break;
				case "author":
					newsChannelItem.Author = reader.ReadElementContentAsString();
					break;
				default:
					reader.Skip();
					break;
				}
			}
			reader.ReadEndElement();
			if (string.IsNullOrEmpty(newsChannelItem.Guid))
			{
				int num = ((!string.IsNullOrEmpty(newsChannelItem.Title)) ? newsChannelItem.Title.GetHashCode() : 0);
				int hashCode = newsChannelItem.Published.GetHashCode();
				int num2 = ((!string.IsNullOrEmpty(newsChannelItem.Description)) ? newsChannelItem.Description.GetHashCode() : 0);
				newsChannelItem.Guid = (num ^ hashCode ^ num2).ToString();
			}
			return newsChannelItem;
		}
	}
}
