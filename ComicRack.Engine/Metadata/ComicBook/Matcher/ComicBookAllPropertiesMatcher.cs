using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
    [Serializable]
    [Description("All")]
    public class ComicBookAllPropertiesMatcher : ComicBookStringMatcher
    {
        public enum MatcherOption
        {
            All,
            Series,
            Writer,
            Artists,
            Descriptive,
            File,
            Catalog
        }

        public enum ShowOptionType
        {
            All,
            Read,
            Reading,
            Unread
        }

        public enum ShowComicType
        {
            All,
            Comics,
            FilelessComics
        }

        private MatcherOption option;

        public MatcherOption Option
        {
            get
            {
                return option;
            }
            set
            {
                option = value;
            }
        }

        private bool MatchOption(ComicBook comicBook, MatcherOption option)
        {
            return GetOptionValueSet(comicBook, option).Any((string t) => MatchBook(comicBook, t));
        }

        public override bool Match(ComicBook comicBook)
        {
            return MatchOption(comicBook, option);
        }

        protected override string GetValue(ComicBook comicBook)
        {
            throw new InvalidOperationException("This is not valid for this comparer");
        }

        public static ComicBookMatcher Create(string query, int matchOperator, MatcherOption searchOption, ShowOptionType showOption, ShowComicType showComic, params ComicBookMatcher[] additonalMatchers)
        {
            List<ComicBookMatcher> list = new List<ComicBookMatcher>();
            switch (showOption)
            {
                case ShowOptionType.Read:
                    list.Add(new ComicBookReadPercentageMatcher
                    {
                        MatchOperator = ComicBookNumericMatcher.Greater,
                        MatchValue = EngineConfiguration.Default.IsReadCompletionPercentage.ToString()
                    });
                    break;
                case ShowOptionType.Reading:
                    list.Add(new ComicBookReadPercentageMatcher
                    {
                        MatchOperator = ComicBookNumericMatcher.InRange,
                        MatchValue = (EngineConfiguration.Default.IsNotReadCompletionPercentage + 1).ToString(),
                        MatchValue2 = (EngineConfiguration.Default.IsReadCompletionPercentage - 1).ToString()
                    });
                    break;
                case ShowOptionType.Unread:
                    list.Add(new ComicBookReadPercentageMatcher
                    {
                        MatchOperator = ComicBookNumericMatcher.Lesser,
                        MatchValue = EngineConfiguration.Default.IsNotReadCompletionPercentage.ToString()
                    });
                    break;
            }
            switch (showComic)
            {
                case ShowComicType.Comics:
                    list.Add(new ComicBookFileMatcher
                    {
                        MatchOperator = ComicBookStringMatcher.OperatorEquals,
                        MatchValue = string.Empty,
                        Not = true
                    });
                    break;
                case ShowComicType.FilelessComics:
                    list.Add(new ComicBookFileMatcher
                    {
                        MatchOperator = ComicBookStringMatcher.OperatorEquals,
                        MatchValue = string.Empty
                    });
                    break;
            }
            if (!string.IsNullOrEmpty(query))
            {
                list.Add(new ComicBookAllPropertiesMatcher
                {
                    MatchValue = query,
                    Option = searchOption,
                    MatchOperator = matchOperator
                });
            }
            list.AddRange(additonalMatchers.Where((ComicBookMatcher m) => m != null));
            if (list.Count == 0)
            {
                return null;
            }
            if (list.Count == 1)
            {
                return list[0];
            }
            ComicBookGroupMatcher comicBookGroupMatcher = new ComicBookGroupMatcher();
            comicBookGroupMatcher.Matchers.AddRange(list);
            comicBookGroupMatcher.MatcherMode = MatcherMode.And;
            return comicBookGroupMatcher;
        }

        public static IEnumerable<string> GetOptionValueSet(ComicBook comicBook, MatcherOption option)
        {
            switch (option)
            {
                case MatcherOption.Series:
                    yield return comicBook.ShadowSeries;
                    yield return comicBook.AlternateSeries;
                    yield return comicBook.ShadowFormat;
                    yield return comicBook.SeriesGroup;
                    yield return comicBook.StoryArc;
                    yield break;
                case MatcherOption.Writer:
                    yield return comicBook.Writer;
                    yield break;
                case MatcherOption.Artists:
                    yield return comicBook.Writer;
                    yield return comicBook.Penciller;
                    yield return comicBook.Inker;
                    yield return comicBook.Colorist;
                    yield return comicBook.Editor;
                    yield return comicBook.Letterer;
                    yield return comicBook.CoverArtist;
                    yield break;
                case MatcherOption.File:
                    yield return comicBook.FilePath;
                    yield break;
                case MatcherOption.Descriptive:
                    yield return comicBook.Notes;
                    yield return comicBook.Summary;
                    yield return comicBook.Review;
                    yield return comicBook.Tags;
                    yield return comicBook.Characters;
                    yield return comicBook.Teams;
                    yield return comicBook.MainCharacterOrTeam;
                    yield return comicBook.Locations;
                    yield return comicBook.ScanInformation;
                    yield break;
                case MatcherOption.Catalog:
                    yield return comicBook.BookAge;
                    yield return comicBook.BookCollectionStatus;
                    yield return comicBook.BookNotes;
                    yield return comicBook.BookOwner;
                    yield return comicBook.BookStore;
                    yield return comicBook.BookLocation;
                    yield return comicBook.ISBN;
                    yield break;
            }
            yield return comicBook.ShadowSeries;
            yield return comicBook.AlternateSeries;
            yield return comicBook.ShadowTitle;
            yield return comicBook.SeriesGroup;
            yield return comicBook.StoryArc;
            yield return comicBook.Writer;
            yield return comicBook.Penciller;
            yield return comicBook.Inker;
            yield return comicBook.Colorist;
            yield return comicBook.Letterer;
            yield return comicBook.Editor;
            yield return comicBook.CoverArtist;
            yield return comicBook.Summary;
            yield return comicBook.FilePath;
            yield return comicBook.Genre;
            yield return comicBook.Notes;
            yield return comicBook.Review;
            yield return comicBook.Publisher;
            yield return comicBook.Imprint;
            yield return comicBook.ShadowVolumeAsText;
            yield return comicBook.ShadowNumberAsText;
            yield return comicBook.AlternateNumberAsText;
            yield return comicBook.ShadowYearAsText;
            yield return comicBook.ShadowFormat;
            yield return comicBook.AgeRating;
            yield return comicBook.Tags;
            yield return comicBook.Characters;
            yield return comicBook.Teams;
            yield return comicBook.MainCharacterOrTeam;
            yield return comicBook.Locations;
            yield return comicBook.BookAge;
            yield return comicBook.BookCollectionStatus;
            yield return comicBook.BookNotes;
            yield return comicBook.BookOwner;
            yield return comicBook.BookStore;
            yield return comicBook.BookLocation;
            yield return comicBook.ISBN;
            yield return comicBook.ScanInformation;
        }
    }
}
