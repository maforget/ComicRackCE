using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public class ExtendedToolStripRenderer : ToolStripRenderer
	{
		private readonly ToolStripRenderer currentRenderer;

		public ExtendedToolStripRenderer(ToolStripRenderer renderer)
		{
			currentRenderer = renderer;
		}

		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			currentRenderer.DrawToolStripBorder(e);
		}

		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			currentRenderer.DrawToolStripBackground(e);
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawButtonBackground(e);
		}

		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			currentRenderer.DrawItemImage(e);
		}

		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			currentRenderer.DrawItemText(e);
		}

		protected override void OnRenderGrip(ToolStripGripRenderEventArgs e)
		{
			currentRenderer.DrawGrip(e);
		}

		protected override void OnRenderLabelBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawLabelBackground(e);
		}

		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			currentRenderer.DrawArrow(e);
		}

		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawDropDownButtonBackground(e);
		}

		protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
		{
			currentRenderer.DrawImageMargin(e);
		}

		protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawItemBackground(e);
		}

		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
		{
			currentRenderer.DrawItemCheck(e);
		}

		protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawMenuItemBackground(e);
		}

		protected override void OnRenderOverflowButtonBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawOverflowButtonBackground(e);
		}

		protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		{
			currentRenderer.DrawSeparator(e);
		}

		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawSplitButton(e);
		}

		protected override void OnRenderStatusStripSizingGrip(ToolStripRenderEventArgs e)
		{
			currentRenderer.DrawStatusStripSizingGrip(e);
		}

		protected override void OnRenderToolStripContentPanelBackground(ToolStripContentPanelRenderEventArgs e)
		{
			currentRenderer.DrawToolStripContentPanelBackground(e);
		}

		protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
		{
			currentRenderer.DrawToolStripPanelBackground(e);
		}

		protected override void OnRenderToolStripStatusLabelBackground(ToolStripItemRenderEventArgs e)
		{
			currentRenderer.DrawToolStripStatusLabelBackground(e);
		}
	}
}
