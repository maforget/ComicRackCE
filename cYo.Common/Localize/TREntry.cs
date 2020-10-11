using System;
using System.Xml.Serialization;

namespace cYo.Common.Localize
{
	public class TREntry : IComparable<TREntry>
	{
		[XmlAttribute]
		public virtual string Key
		{
			get;
			set;
		}

		[XmlAttribute]
		public virtual string Text
		{
			get;
			set;
		}

		[XmlAttribute]
		public virtual string Comment
		{
			get;
			set;
		}

		[XmlIgnore]
		public virtual TR Resource
		{
			get;
			internal set;
		}

		public virtual string ResourceName
		{
			get
			{
				if (Resource != null)
				{
					return Resource.Name;
				}
				return string.Empty;
			}
		}

		public TREntry()
		{
		}

		public TREntry(string key, string text, string comment)
		{
			Key = key;
			Text = text;
			Comment = comment;
		}

		public virtual int CompareTo(TREntry other)
		{
			return string.Compare(Key, other.Key, StringComparison.Ordinal);
		}
	}
}
