using System;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Reflection;
using static cYo.Common.Win32.ExecuteProcess;
using System.Collections.Generic;
using System.Linq;
using cYo.Common.Reflection;

namespace cYo.Projects.ComicRack.Engine
{
    public abstract class ComicBookVirtualTagMatcher : ComicBookStringMatcher
    {
		public string VirtualDescription => GetType().Description() ?? string.Empty;

		private IVirtualTag GetVirtualTag()
		{
			var matcherHintAttribute = GetAttribute();
			IVirtualTag vtag = VirtualTagsCollection.Tags.Values.FirstOrDefault(x => x.IsEnabled && x.PropertyName == matcherHintAttribute?.Properties.First());
			return vtag;
		}

		public ComicBookMatcherHintAttribute GetAttribute()
        {
            return GetAttribute(GetType());
		}

		public static ComicBookMatcherHintAttribute GetAttribute(Type type)
		{
			return Attribute.GetCustomAttribute(type, typeof(ComicBookMatcherHintAttribute)) as ComicBookMatcherHintAttribute;
		}

		public static Type GetMatcher(IVirtualTag tag)
		{
            if (tag is null || string.IsNullOrEmpty(tag.PropertyName))
                return default;

            return GetAvailableMatcherTypes().FirstOrDefault(x => typeof(ComicBookVirtualTagMatcher).IsAssignableFrom(x) 
                && GetAttribute(x).Properties.FirstOrDefault() == tag.PropertyName);
		}

		/// <summary>
		/// Finds the IVirtualTag based on the Hint Attribute & the corresponding IVirtualTag.
		/// Returns an alternate string for the Description used in SmartLists
		/// </summary>
		public override string DescriptionNeutral
		{
			get
			{
				if (descriptionNeutral == null)
				{
					IVirtualTag vtag = GetVirtualTag();
					descriptionNeutral = vtag?.Name ?? string.Empty;
				}
				return descriptionNeutral;
			}
		}
		private string descriptionNeutral;
	}

	[Serializable]
    [Description("Virtual Tags #01")]
    [ComicBookMatcherHint("VirtualTag01", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag1Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag01;
        }
    }

    [Serializable]
    [Description("Virtual Tags #02")]
    [ComicBookMatcherHint("VirtualTag02", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag2Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag02;
        }
    }

    [Serializable]
    [Description("Virtual Tags #03")]
    [ComicBookMatcherHint("VirtualTag03", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag3Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag03;
        }
    }

    [Serializable]
    [Description("Virtual Tags #04")]
    [ComicBookMatcherHint("VirtualTag04", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag4Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag04;
        }
    }

    [Serializable]
    [Description("Virtual Tags #05")]
    [ComicBookMatcherHint("VirtualTag05", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag5Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag05;
        }
    }

    [Serializable]
    [Description("Virtual Tags #06")]
    [ComicBookMatcherHint("VirtualTag06", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag6Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag06;
        }
    }

    [Serializable]
    [Description("Virtual Tags #07")]
    [ComicBookMatcherHint("VirtualTag07", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag7Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag07;
        }
    }

    [Serializable]
    [Description("Virtual Tags #08")]
    [ComicBookMatcherHint("VirtualTag08", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag8Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag08;
        }
    }

    [Serializable]
    [Description("Virtual Tags #09")]
    [ComicBookMatcherHint("VirtualTag09", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag9Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag09;
        }
    }

    [Serializable]
    [Description("Virtual Tags #10")]
    [ComicBookMatcherHint("VirtualTag10", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag10Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag10;
        }
    }

    [Serializable]
    [Description("Virtual Tags #11")]
    [ComicBookMatcherHint("VirtualTag11", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag11Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag11;
        }
    }

    [Serializable]
    [Description("Virtual Tags #12")]
    [ComicBookMatcherHint("VirtualTag12", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag12Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag12;
        }
    }

    [Serializable]
    [Description("Virtual Tags #13")]
    [ComicBookMatcherHint("VirtualTag13", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag13Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag13;
        }
    }

    [Serializable]
    [Description("Virtual Tags #14")]
    [ComicBookMatcherHint("VirtualTag14", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag14Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag14;
        }
    }

    [Serializable]
    [Description("Virtual Tags #15")]
    [ComicBookMatcherHint("VirtualTag15", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag15Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag15;
        }
    }

    [Serializable]
    [Description("Virtual Tags #16")]
    [ComicBookMatcherHint("VirtualTag16", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag16Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag16;
        }
    }

    [Serializable]
    [Description("Virtual Tags #17")]
    [ComicBookMatcherHint("VirtualTag17", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag17Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag17;
        }
    }

    [Serializable]
    [Description("Virtual Tags #18")]
    [ComicBookMatcherHint("VirtualTag18", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag18Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag18;
        }
    }

    [Serializable]
    [Description("Virtual Tags #19")]
    [ComicBookMatcherHint("VirtualTag19", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag19Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag19;
        }
    }

    [Serializable]
    [Description("Virtual Tags #20")]
    [ComicBookMatcherHint("VirtualTag20", DisableOptimizedUpdate = true)]
    public class ComicBookVirtualTag20Matcher : ComicBookVirtualTagMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.VirtualTag20;
        }
    }
}
