using System.Globalization;
using System.Xml.Serialization;

namespace cYo.Common.Localize
{
	public class TRInfo
	{
		private float completionPercent = 100f;

		public string ApplicationName
		{
			get;
			set;
		}

		public string ApplicationVersion
		{
			get;
			set;
		}

		public string Author
		{
			get;
			set;
		}

		public string Notes
		{
			get;
			set;
		}

		public bool RightToLeft
		{
			get;
			set;
		}

		[XmlElement("Language")]
		public string CultureName
		{
			get;
			set;
		}

		public string DisplayLanguage
		{
			get
			{
				string cultureName = CultureName;
				if (cultureName == null)
				{
					return TR.Default["System", "System"];
				}
				return new CultureInfo(CultureName).DisplayName;
			}
		}

		[XmlIgnore]
		public float CompletionPercent
		{
			get
			{
				return completionPercent;
			}
			set
			{
				completionPercent = value;
			}
		}

		public TRInfo()
		{
		}

		public TRInfo(string language)
		{
			CultureName = language;
		}

		public override string ToString()
		{
			string text = DisplayLanguage;
			if (!string.IsNullOrEmpty(Author))
			{
				text = $"{text} ({Author})";
			}
			int num = (int)completionPercent;
			if (num != 100)
			{
				text += $"\t[{num:###}%]";
			}
			return text;
		}
	}
}
