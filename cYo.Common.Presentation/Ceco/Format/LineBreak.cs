namespace cYo.Common.Presentation.Ceco.Format
{
	public class LineBreak : Span
	{
		private float lineScale = 1f;

		public float LineScale
		{
			get
			{
				return lineScale;
			}
			set
			{
				lineScale = value;
			}
		}

		public bool Clear
		{
			get;
			set;
		}

		public override int FlowBreakOffset => (int)((float)Font.Height * lineScale);

		public override FlowBreak FlowBreak => FlowBreak.After;

		public LineBreak()
		{
		}

		public LineBreak(float lineScale)
		{
			this.lineScale = lineScale;
		}
	}
}
