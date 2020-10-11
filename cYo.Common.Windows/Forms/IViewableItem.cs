using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace cYo.Common.Windows.Forms
{
	public interface IViewableItem : IBaseViewItem, INotifyPropertyChanged
	{
		void OnDraw(ItemDrawInformation drawInfo);

		void OnMeasure(ItemSizeInformation sizeInfo);

		bool OnClick(Point pt);

		Control GetEditControl(int subItem);

		ItemViewStates GetOwnerDrawnStates(ItemViewMode mode);
	}
}
