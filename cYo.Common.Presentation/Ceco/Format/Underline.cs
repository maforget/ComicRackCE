using System.Drawing;

namespace cYo.Common.Presentation.Ceco.Format
{
	public class Underline : Span
	{
		public override FontStyle FontStyle
		{
			get
			{
				return base.FontStyle | FontStyle.Underline;
			}
			set
			{
				base.FontStyle = value;
			}
		}

		public Underline()
		{
		}

		public Underline(params Inline[] inlines)
			: base(inlines)
		{
		}
	}
}
