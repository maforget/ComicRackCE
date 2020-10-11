using System.Drawing;

namespace cYo.Common.Presentation.Ceco.Format
{
	public class Italic : Span
	{
		public override FontStyle FontStyle
		{
			get
			{
				return base.FontStyle | FontStyle.Italic;
			}
			set
			{
				base.FontStyle = value;
			}
		}

		public Italic()
		{
		}

		public Italic(params Inline[] inlines)
			: base(inlines)
		{
		}
	}
}
