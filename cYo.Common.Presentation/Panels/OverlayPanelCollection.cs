using cYo.Common.Collections;

namespace cYo.Common.Presentation.Panels
{
	public class OverlayPanelCollection : SmartList<OverlayPanel>
	{
		public OverlayPanelCollection()
		{
			base.Flags |= SmartListOptions.DisposeOnRemove;
		}
	}
}
