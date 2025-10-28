using cYo.Common.Windows.Forms.Theme.Resources;
using System.Drawing;

namespace cYo.Common.Windows.Forms
{
	public class ItemDrawInformation : ItemSizeInformation
	{
		public ItemViewStates State
		{
			get;
			set;
		}

		public Color TextColor
		{
			get;
			set;
		}

		public bool ControlFocused
		{
			get;
			set;
		}

		public bool DrawBorder
		{
			get;
			set;
		}

		public bool ExpandedColumn
		{
			get;
			set;
		}

		public ItemDrawInformation()
		{
			TextColor = ThemeColors.ItemDrawInfoText;
			DrawBorder = true;
		}
	}
}
