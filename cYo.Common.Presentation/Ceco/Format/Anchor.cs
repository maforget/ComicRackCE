using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Presentation.Ceco.Format
{
	public class Anchor : Span
	{
		public string HRef
		{
			get;
			set;
		}

		public Anchor()
		{
			Initialize();
		}

		public Anchor(params Inline[] inlines)
			: base(inlines)
		{
			Initialize();
		}

		private void Initialize()
		{
			ForeColor = Color.Red;
			MouseCursor = Cursors.Hand;
		}

		protected override void OnMouseEnter()
		{
			base.OnMouseEnter();
			FontStyle = FontStyle.Bold | FontStyle.Underline;
		}

		protected override void OnMouseLeave()
		{
			base.OnMouseLeave();
			FontStyle = FontStyle.Regular;
		}
	}
}
