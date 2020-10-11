using System.Drawing;
using cYo.Common.ComponentModel;

namespace cYo.Common.Drawing
{
	public class TextLine : DisposableObject
	{
		public bool Separator
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public Color ForeColor
		{
			get;
			set;
		}

		public Font Font
		{
			get;
			set;
		}

		public int BeforeSpacing
		{
			get;
			set;
		}

		public int AfterSpacing
		{
			get;
			set;
		}

		public StringFormat Format
		{
			get;
			set;
		}

		public bool FontOwned
		{
			get;
			set;
		}

		public bool ScrollStart
		{
			get;
			set;
		}

		public TextLine(string text, Font font, Color foreColor, StringFormat format, int beforeSpacing = 0, int afterSpacing = 0)
		{
			Text = text;
			Font = font;
			ForeColor = foreColor;
			BeforeSpacing = beforeSpacing;
			AfterSpacing = afterSpacing;
			Format = format;
		}

		public TextLine(string text, Font font, Color foreColor, StringFormatFlags options = (StringFormatFlags)0, StringAlignment alignment = StringAlignment.Near, int beforeSpacing = 0, int afterSpacing = 0)
			: this(text, font, foreColor, CreateStringFormat(alignment, options), beforeSpacing, afterSpacing)
		{
		}

		public TextLine(int spacing)
		{
			Separator = true;
			BeforeSpacing = spacing;
			Format = new StringFormat();
		}

		private static StringFormat CreateStringFormat(StringAlignment align, StringFormatFlags options)
		{
			StringFormat stringFormat = new StringFormat(options)
			{
				Alignment = align
			};
			if ((options & StringFormatFlags.NoWrap) != 0)
			{
				stringFormat.Trimming = StringTrimming.EllipsisCharacter;
			}
			return stringFormat;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (Format != null)
				{
					Format.Dispose();
				}
				if (FontOwned && Font != null && !Font.IsSystemFont)
				{
					Font.Dispose();
				}
			}
			base.Dispose(disposing);
		}
	}
}
