using System;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	public class ServerStatistics
	{
		public enum StatisticType
		{
			InfoRequest,
			LibraryRequest,
			PageRequest,
			ThumbnailRequest,
			FailedAuthentication
		}

		public class StatisticItem
		{
			public DateTime TimeStamp
			{
				get;
				private set;
			}

			public StatisticType Type
			{
				get;
				set;
			}

			public string Client
			{
				get;
				set;
			}

			public int Size
			{
				get;
				set;
			}

			public StatisticItem(string client, StatisticType type, int size = 0)
			{
				TimeStamp = DateTime.Now;
				Client = client;
				Type = type;
				Size = size;
			}
		}

		public class StatisticResult
		{
			public int ClientCount
			{
				get;
				private set;
			}

			public int InfoRequestCount
			{
				get;
				private set;
			}

			public int LibraryRequestCount
			{
				get;
				private set;
			}

			public int PageRequestCount
			{
				get;
				private set;
			}

			public int ThumbnailRequestCount
			{
				get;
				private set;
			}

			public long PageRequestSize
			{
				get;
				private set;
			}

			public long ThumbnailRequestSize
			{
				get;
				private set;
			}

			public long LibraryRequestSize
			{
				get;
				private set;
			}

			public int FailedAuthenticationCount
			{
				get;
				private set;
			}

			public long TotalRequestSize => PageRequestSize + ThumbnailRequestSize + LibraryRequestSize;

			public StatisticResult()
			{
			}

			public StatisticResult(IEnumerable<StatisticItem> items, TimeSpan timeSpan)
			{
				DateTime now = DateTime.Now;
				IEnumerable<StatisticItem> source = items.Reverse().TakeWhile((StatisticItem n) => now - n.TimeStamp < timeSpan);
				ClientCount = source.Select((StatisticItem n) => n.Client).Distinct().Count();
				InfoRequestCount = source.Count((StatisticItem n) => n.Type == StatisticType.InfoRequest);
				LibraryRequestCount = source.Count((StatisticItem n) => n.Type == StatisticType.LibraryRequest);
				PageRequestCount = source.Count((StatisticItem n) => n.Type == StatisticType.PageRequest);
				ThumbnailRequestCount = source.Count((StatisticItem n) => n.Type == StatisticType.ThumbnailRequest);
				FailedAuthenticationCount = source.Count((StatisticItem n) => n.Type == StatisticType.FailedAuthentication);
				PageRequestSize = source.Where((StatisticItem n) => n.Type == StatisticType.PageRequest).Sum((Func<StatisticItem, long>)((StatisticItem n) => n.Size));
				LibraryRequestSize = source.Where((StatisticItem n) => n.Type == StatisticType.LibraryRequest).Sum((Func<StatisticItem, long>)((StatisticItem n) => n.Size));
				ThumbnailRequestSize = source.Where((StatisticItem n) => n.Type == StatisticType.ThumbnailRequest).Sum((Func<StatisticItem, long>)((StatisticItem n) => n.Size));
			}

			public void Add(StatisticResult sr)
			{
				InfoRequestCount += sr.InfoRequestCount;
				LibraryRequestCount += sr.LibraryRequestCount;
				PageRequestCount += sr.PageRequestCount;
				ThumbnailRequestCount += sr.ThumbnailRequestCount;
				FailedAuthenticationCount += sr.FailedAuthenticationCount;
				PageRequestSize += sr.PageRequestSize;
				LibraryRequestSize += sr.LibraryRequestSize;
				ThumbnailRequestSize += sr.ThumbnailRequestSize;
			}
		}

		private SmartList<StatisticItem> items = new SmartList<StatisticItem>();

		public IEnumerable<StatisticItem> Items => items;

		public void Add(string client, StatisticType type, int size = 0)
		{
			items.Add(new StatisticItem(client, type, size));
		}

		public StatisticResult GetResult(TimeSpan timeSpan)
		{
			return new StatisticResult(Items, timeSpan);
		}

		public StatisticResult GetResult(int seconds = int.MaxValue)
		{
			return new StatisticResult(Items, TimeSpan.FromSeconds(seconds));
		}

		public bool WasActive(int seconds = 10)
		{
			return WasActive(TimeSpan.FromSeconds(seconds));
		}

		public bool WasActive(TimeSpan timeSpan)
		{
			DateTime now = DateTime.Now;
			return items.Reverse().Any((StatisticItem n) => now - n.TimeStamp < timeSpan);
		}
	}
}
