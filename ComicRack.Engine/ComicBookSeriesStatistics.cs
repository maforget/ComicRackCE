using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using cYo.Common;
using cYo.Common.Collections;
using cYo.Common.Localize;
using cYo.Common.Reflection;

namespace cYo.Projects.ComicRack.Engine
{
    public class ComicBookSeriesStatistics
    {
        public class Key : IEquatable<Key>
        {
            public string Series
            {
                get;
                private set;
            }

            public int Volume
            {
                get;
                private set;
            }

            public Key(ComicBook book)
            {
                Series = book.ShadowSeries ?? string.Empty;
                Volume = book.ShadowVolume;
            }

            public override int GetHashCode()
            {
                return Series.GetHashCode() ^ Volume.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Key key = obj as Key;
                if (key != null)
                {
                    return Equals(key);
                }
                return false;
            }

            public bool Equals(Key other)
            {
                if (other != null && Series == other.Series)
                {
                    return Volume == other.Volume;
                }
                return false;
            }
        }

        private static TR tr;

        private static readonly string noneText = TR["None", "None"];

        private readonly Lazy<int> count;

        private readonly Lazy<int> readCount;

        private readonly Lazy<int> pageCount;

        private readonly Lazy<int> pageReadCount;

        private readonly Lazy<int> readPercentage;

        private readonly Lazy<int> ratingCount;

        private readonly Lazy<int> communityRatingCount;

        private readonly Lazy<float> averageRating;

        private readonly Lazy<float> averageCommunityRating;

        private readonly Lazy<float> firstNumber;

        private readonly Lazy<float> lastNumber;

        private readonly Lazy<int> minYear;

        private readonly Lazy<int> maxYear;

        private readonly Lazy<int> runningTimeYears;

        private readonly Lazy<IEnumerable<RangeF>> gaps;

        private readonly Lazy<int> maxGapSize;

        private readonly Lazy<int> gapCount;

        private readonly Lazy<YesNo> complete;

        private readonly Lazy<DateTime> lastOpenedTime;

        private readonly Lazy<DateTime> lastAddedTime;

        private readonly Lazy<DateTime> lastPublishedTime;

        private readonly Lazy<DateTime> lastReleasedTime;

        private readonly Lazy<int> maxCount;

        private readonly Lazy<int> minCount;

        private static readonly HashSet<string> statisticProperties = new HashSet<string>(new string[15]
        {
            "Series",
            "Volume",
            "FilePath",
            "Number",
            "Count",
            "Year",
            "Month",
            "AddedTime",
            "OpenedTime",
            "ReleasedTime",
            "SeriesComplete",
            "PageCount",
            "LastPageRead",
            "Rating",
            "CommunityRating"
        });

        public static TR TR
        {
            get
            {
                if (tr == null)
                {
                    tr = TR.Load("ComicBook");
                }
                return tr;
            }
        }

        public IEnumerable<ComicBook> Books
        {
            get;
            private set;
        }

        public int Count => count.Value;

        public int ReadCount => readCount.Value;

        public int PageCount => pageCount.Value;

        public int PageReadCount => pageReadCount.Value;

        public int ReadPercentage => readPercentage.Value;

        public int RatingCount => ratingCount.Value;

        public int CommunityRatingCount => communityRatingCount.Value;

        public float AverageRating => averageRating.Value;

        public float AverageCommunityRating => averageCommunityRating.Value;

        public float FirstNumber => firstNumber.Value;

        public float LastNumber => lastNumber.Value;

        public int FirstYear => minYear.Value;

        public int LastYear => maxYear.Value;

        public int RunningTimeYears => runningTimeYears.Value;

        public IEnumerable<RangeF> Gaps => gaps.Value;

        public int MaxGapSize => maxGapSize.Value;

        public int GapCount => gapCount.Value;

        public YesNo AllComplete => complete.Value;

        public DateTime LastOpenedTime => lastOpenedTime.Value;

        public DateTime LastAddedTime => lastAddedTime.Value;

        public DateTime LastPublishedTime => lastPublishedTime.Value;

        public DateTime LastReleasedTime => lastReleasedTime.Value;

        public int MaxCount => maxCount.Value;

        public int MinCount => minCount.Value;

        public string CountAsText => Count.ToString();

        public string PageCountAsText => ComicBook.FormatPages(PageCount);

        public string PageReadCountAsText => ComicBook.FormatPages(PageReadCount);

        public string ReadPercentageAsText => $"{ReadPercentage}%";

        public string MinYearAsText => ComicBook.FormatYear(FirstYear);

        public string MaxYearAsText => ComicBook.FormatYear(LastYear);

        public string RunningTimeYearsAsText => ComicBook.FormatYear(RunningTimeYears);

        public string GapCountAsText
        {
            get
            {
                if (GapCount > 0)
                {
                    return GapCount.ToString();
                }
                return noneText;
            }
        }

        public string MinNumberAsText
        {
            get
            {
                if (!(FirstNumber < 0f))
                {
                    return FirstNumber.ToString();
                }
                return string.Empty;
            }
        }

        public string MaxNumberAsText
        {
            get
            {
                if (!(LastNumber < 0f))
                {
                    return LastNumber.ToString();
                }
                return string.Empty;
            }
        }

        public static ISet<string> StatisticProperties => statisticProperties;

        public ComicBookSeriesStatistics(IEnumerable<ComicBook> books)
        {
            Books = books;
            count = new Lazy<int>(() => Books.Count());
            minCount = new Lazy<int>(() => Books.Min((ComicBook cb) => cb.ShadowCount));
            maxCount = new Lazy<int>(() => Books.Max((ComicBook cb) => cb.ShadowCount));
            readCount = new Lazy<int>(() => Books.Count((ComicBook cb) => cb.HasBeenRead));
            pageCount = new Lazy<int>(() => Books.Sum((ComicBook cb) => cb.PageCount));
            pageReadCount = new Lazy<int>(() => Books.Sum((ComicBook cb) => cb.LastPageRead));
            readPercentage = new Lazy<int>(() => (Count != 0) ? (ReadCount * 100 / Count) : 0);
            firstNumber = new Lazy<float>(() => Books.Min((ComicBook cb) => GetSafeNumber(cb)));
            lastNumber = new Lazy<float>(() => Books.Max((ComicBook cb) => GetSafeNumber(cb)));
            ratingCount = new Lazy<int>(() => Books.Count((ComicBook cb) => cb.Rating > 0f));
            communityRatingCount = new Lazy<int>(() => Books.Count((ComicBook cb) => cb.CommunityRating > 0f));
            averageRating = new Lazy<float>(() => (RatingCount != 0) ? Books.Where((ComicBook cb) => cb.Rating > 0f).Average((ComicBook cb) => cb.Rating) : 0f);
            averageCommunityRating = new Lazy<float>(() => (CommunityRatingCount != 0) ? Books.Where((ComicBook cb) => cb.CommunityRating > 0f).Average((ComicBook cb) => cb.CommunityRating) : 0f);
            minYear = new Lazy<int>(() => Books.Min((ComicBook cb) => cb.ShadowYear));
            maxYear = new Lazy<int>(() => Books.Max((ComicBook cb) => cb.ShadowYear));
            runningTimeYears = new Lazy<int>(() => (LastYear >= 0) ? (LastYear - FirstYear) : 0);
            gaps = new Lazy<IEnumerable<RangeF>>(() => GetGaps(Books).ToArray());
            gapCount = new Lazy<int>(() => Gaps.Count());
            maxGapSize = new Lazy<int>(() => (GapCount != 0) ? Gaps.Max((RangeF g) => (int)g.Length) : 0);
            complete = new Lazy<YesNo>(() => SumComplete(Books));
            lastAddedTime = new Lazy<DateTime>(() => Books.Max((ComicBook cb) => cb.AddedTime));
            lastOpenedTime = new Lazy<DateTime>(() => Books.Max((ComicBook cb) => cb.OpenedTime));
            lastPublishedTime = new Lazy<DateTime>(() => Books.Max((ComicBook cb) => cb.Published));
            lastReleasedTime = new Lazy<DateTime>(() => Books.Max((ComicBook cb) => cb.ReleasedTime));
        }

        public bool IsGapStart(ComicBook book)
        {
            float i = GetSafeNumber(book);
            return Gaps.Any((RangeF g) => g.Start == i);
        }

        public bool IsGapEnd(ComicBook book)
        {
            float i = GetSafeNumber(book);
            return Gaps.Any((RangeF g) => g.End == i);
        }

        public T GetTypedValue<T>(string propName)
        {
            try
            {
                return PropertyCaller.CreateGetMethod<ComicBookSeriesStatistics, T>(propName)(this);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private static YesNo SumComplete(IEnumerable<ComicBook> books)
        {
            bool flag = true;
            YesNo yesNo = YesNo.Unknown;
            foreach (ComicBook book in books)
            {
                if (flag)
                {
                    flag = false;
                    yesNo = book.SeriesComplete;
                }
                else if (yesNo != book.SeriesComplete)
                {
                    return YesNo.Unknown;
                }
            }
            return yesNo;
        }

        public static IEnumerable<string> GetProperties()
        {
            return from pi in typeof(ComicBookSeriesStatistics).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                   where pi.Browsable()
                   select pi.Name;
        }

        public static Dictionary<Key, ComicBookSeriesStatistics> Create(IEnumerable<ComicBook> books)
        {
            return (from cb in books.Lock()
                    group cb by new Key(cb)).ToDictionary((IGrouping<Key, ComicBook> gr) => gr.Key, (IGrouping<Key, ComicBook> gr) => new ComicBookSeriesStatistics(gr));
        }

        public static IEnumerable<RangeF> GetGaps(IEnumerable<ComicBook> books)
        {
            IOrderedEnumerable<float> source = from n in books.SelectMany(book => new[] { GetSafeNumber(book) }.Concat(GetRangeNumber(book)))
                                               where n > 0f
                                               orderby n
                                               select n;
            if (!source.Any())
            {
                yield break;
            }
            float num = source.First();
            foreach (float i in source.Skip(1))
            {
                float num2 = i - num;
                if (num2 > 1f)
                {
                    yield return new RangeF(num, num2);
                }
                num = i;
            }
        }

        private static float GetSafeNumber(ComicBook cb)
        {
            if (!cb.CompareNumber.IsNumber)
            {
                return -1f;
            }
            return cb.CompareNumber.Number;
        }

        private static IEnumerable<float> GetRangeNumber(ComicBook cb)
        {
            return cb.CompareNumber.GetRange().Skip(1);//Skip the first number, because it is added to the list by GetSafeNumber
        }
    }
}
