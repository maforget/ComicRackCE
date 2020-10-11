using System.Drawing;

namespace cYo.Common.Presentation.Ceco.Format
{
	public class HorizontalRule : Block
	{
		private int thickness = 2;

		private bool noshade;

		public override FlowBreak FlowBreak => FlowBreak.BreakLine | FlowBreak.Before | FlowBreak.After;

		public override int FlowBreakOffset => Font.Height;

		public int Thickness
		{
			get
			{
				return thickness;
			}
			set
			{
				if (thickness != value)
				{
					thickness = value;
					OnThicknessChanged();
				}
			}
		}

		public bool Noshade
		{
			get
			{
				return noshade;
			}
			set
			{
				if (noshade != value)
				{
					noshade = value;
					OnNoShadeChanged();
				}
			}
		}

		protected override void CoreMeasure(Graphics gr, int maxWidth, LayoutType tbl)
		{
			base.Width = base.BlockWidth.GetSize(maxWidth);
			base.MinimumWidth = (base.BlockWidth.IsFixed ? base.Width : 0);
			base.Height = Thickness + ((!Noshade) ? 1 : 0);
		}

		public override void Draw(Graphics gr, Point location)
		{
			base.Draw(gr, location);
			Rectangle bounds = base.Bounds;
			bounds.Offset(location);
			if (!noshade)
			{
				bounds.Width--;
				bounds.Height--;
			}
			if (!noshade)
			{
				bounds.Offset(1, 1);
				gr.FillRectangle(Brushes.DarkGray, bounds);
				bounds.Offset(-1, -1);
			}
			using (Brush brush = new SolidBrush(ForeColor))
			{
				gr.FillRectangle(brush, bounds);
			}
		}

		protected virtual void OnThicknessChanged()
		{
			InvokeLayout(LayoutType.Full);
		}

		protected virtual void OnNoShadeChanged()
		{
			InvokeLayout(LayoutType.Full);
		}
	}
}
