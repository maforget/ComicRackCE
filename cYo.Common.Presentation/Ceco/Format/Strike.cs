using System.Drawing;

namespace cYo.Common.Presentation.Ceco.Format
{
	public class Strike : Span
	{
		public override FontStyle FontStyle
		{
			get
			{
				return base.FontStyle | FontStyle.Strikeout;
			}
			set
			{
				base.FontStyle = value;
			}
		}

		public Strike()
		{
		}

		public Strike(params Inline[] inlines)
			: base(inlines)
		{
		}
	}
}
