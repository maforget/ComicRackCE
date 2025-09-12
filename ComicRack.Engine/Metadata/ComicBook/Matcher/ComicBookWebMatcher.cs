using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
    [Serializable]
    [Description("Web")]
    [ComicBookMatcherHint("Web")]
    public class ComicBookWebMatcher : ComicBookStringMatcher
    {
        protected override string GetValue(ComicBook comicBook)
        {
            return comicBook.Web;
        }
    }
}


