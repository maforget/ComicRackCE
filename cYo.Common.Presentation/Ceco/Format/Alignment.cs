namespace cYo.Common.Presentation.Ceco.Format
{
	public class Alignment : Span
	{
		public Alignment(HorizontalAlignment lineAlign)
		{
			Align = lineAlign;
		}

		public Alignment(HorizontalAlignment lineAlign, params Inline[] inlines)
			: base(inlines)
		{
			Align = lineAlign;
		}
	}
}
