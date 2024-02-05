using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cYo.Common.Net.News
{
    public static class Atom
    {
        public static string TrimEndLine(this string text)
        {
            return text.Trim('\n').Trim();
        }

        [Serializable()]
        [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        [XmlRoot(Namespace = "http://www.w3.org/2005/Atom", IsNullable = false)]
        public class feed
        {
            [XmlElement("entry")]
            public List<feedEntry> entries { get; set; }

            public string id { get; set; }

            [XmlElement("link")]
            public List<feedLink> links { get; set; }

            public string title { get; set; }

            public DateTime updated { get; set; }
        }

        [Serializable()]
        [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public class feedEntry
        {
            private string _title;

            public feedEntryAuthor author { get; set; }
            public feedEntryContent content { get; set; }
            public string id { get; set; }

            public feedEntryLink link { get; set; }

            [XmlElement(Namespace = "http://search.yahoo.com/mrss/")]
            public thumbnail thumbnail { get; set; }

            public string title { get => _title.TrimEndLine(); set => _title = value; }

            public DateTime updated { get; set; }
        }

        [Serializable()]
        [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public class feedEntryAuthor
        {
            public string name { get; set; }

            public string uri { get; set; }
        }

        [Serializable()]
        [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public class feedEntryContent
        {
            private string _value;

            [XmlAttribute()]
            public string type { get; set; }

            [XmlText()]
            public string Value { get => _value.TrimEndLine(); set => _value = value; }
        }

        [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public class feedEntryLink
        {
            [XmlAttribute()]
            public string href { get; set; }

            [XmlAttribute()]
            public string rel { get; set; }

            [XmlAttribute()]
            public string type { get; set; }
        }

        [Serializable()]
        [XmlType(AnonymousType = true, Namespace = "http://www.w3.org/2005/Atom")]
        public class feedLink
        {
            [XmlAttribute()]
            public string href { get; set; }

            [XmlAttribute()]
            public string rel { get; set; }

            [XmlAttribute()]
            public string type { get; set; }
        }

        [Serializable()]
        [XmlType(AnonymousType = true, Namespace = "http://search.yahoo.com/mrss/")]
        [XmlRoot(Namespace = "http://search.yahoo.com/mrss/", IsNullable = false)]
        public class thumbnail
        {
            [XmlAttribute()]
            public byte height { get; set; }

            [XmlAttribute()]
            public string url { get; set; }

            [XmlAttribute()]
            public byte width { get; set; }
        }
    }
}