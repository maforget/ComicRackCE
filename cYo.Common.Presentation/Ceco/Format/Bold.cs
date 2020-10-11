using System.Drawing;

namespace cYo.Common.Presentation.Ceco.Format
{
	public class Bold : Span
	{
		public override FontStyle FontStyle
		{
			get
			{
				return base.FontStyle | FontStyle.Bold;
			}
			set
			{
				base.FontStyle = value;
			}
		}

		public Bold()
		{
		}

		public Bold(params Inline[] inlines)
			: base(inlines)
		{
		}
	}
}
