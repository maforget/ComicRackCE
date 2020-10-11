using System.Drawing;

namespace cYo.Common.Presentation.Ceco.Format
{
	public class TextFont : Span
	{
		public TextFont()
		{
		}

		public TextFont(string face, float scale, FontSize fontSize, Color color, params Inline[] inlines)
			: base(inlines)
		{
			FontScale = scale;
			FontSize = fontSize;
			FontFamily = face;
			ForeColor = color;
		}

		public TextFont(int size, params Inline[] inlines)
			: this(null, 1f, new FontSize(size, relative: false), Color.Empty, inlines)
		{
		}

		public TextFont(int size, FontStyle fontStyle, params Inline[] inlines)
			: this(null, 1f, new FontSize(size, relative: false), Color.Empty, inlines)
		{
			FontStyle = fontStyle;
		}

		public TextFont(FontSize fontSize, params Inline[] inlines)
			: this(null, 1f, fontSize, Color.Empty, inlines)
		{
		}

		public TextFont(float scale, BaseAlignment align, params Inline[] inlines)
			: this(null, scale, new FontSize(0, relative: true), Color.Empty, inlines)
		{
			base.BaseAlign = align;
		}
	}
}
