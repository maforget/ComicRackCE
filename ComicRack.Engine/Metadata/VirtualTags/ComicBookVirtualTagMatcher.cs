using System;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Reflection;
using static cYo.Common.Win32.ExecuteProcess;
using System.Collections.Generic;
using System.Linq;

namespace cYo.Projects.ComicRack.Engine
{
    public abstract class ComicBookVirtualTagMatcher : ComicBookStringMatcher, IVirtualDescription
    {
        /// <summary>
        /// Finds the IVirtualTag based on the Hint Attribute & the corresponding IVirtualTag.
        /// Returns an alternate string for the Description used in SmartLists
        /// </summary>
        public string VirtualDescription
        {
            get
            {
                var matcherHintAttribute = Attribute.GetCustomAttribute(GetType(), typeof(ComicBookMatcherHintAttribute)) as ComicBookMatcherHintAttribute;
                IVirtualTag vtag = VirtualTagsCollection.Tags.Values.FirstOrDefault(x => x.IsEnabled && x.PropertyName == matcherHintAttribute?.Properties.First());
                return vtag?.Name ?? string.Empty;
            }
        }
    }

    [Serializable]
    [Description("Virtual Tags #01")]
    [ComicBookMatcherHint("VirtualTag01")]
    public class ComicBookVirtualTag1Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag01;
        }
    }

    [Serializable]
    [Description("Virtual Tags #02")]
    [ComicBookMatcherHint("VirtualTag02")]
    public class ComicBookVirtualTag2Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag02;
        }
    }

    [Serializable]
    [Description("Virtual Tags #03")]
    [ComicBookMatcherHint("VirtualTag03")]
    public class ComicBookVirtualTag3Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag03;
        }
    }

    [Serializable]
    [Description("Virtual Tags #04")]
    [ComicBookMatcherHint("VirtualTag04")]
    public class ComicBookVirtualTag4Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag04;
        }
    }

    [Serializable]
    [Description("Virtual Tags #05")]
    [ComicBookMatcherHint("VirtualTag05")]
    public class ComicBookVirtualTag5Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag05;
        }
    }

    [Serializable]
    [Description("Virtual Tags #06")]
    [ComicBookMatcherHint("VirtualTag06")]
    public class ComicBookVirtualTag6Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag06;
        }
    }

    [Serializable]
    [Description("Virtual Tags #07")]
    [ComicBookMatcherHint("VirtualTag07")]
    public class ComicBookVirtualTag7Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag07;
        }
    }

    [Serializable]
    [Description("Virtual Tags #08")]
    [ComicBookMatcherHint("VirtualTag08")]
    public class ComicBookVirtualTag8Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag08;
        }
    }

    [Serializable]
    [Description("Virtual Tags #09")]
    [ComicBookMatcherHint("VirtualTag09")]
    public class ComicBookVirtualTag9Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag09;
        }
    }

    [Serializable]
    [Description("Virtual Tags #10")]
    [ComicBookMatcherHint("VirtualTag10")]
    public class ComicBookVirtualTag10Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag10;
        }
    }
}
