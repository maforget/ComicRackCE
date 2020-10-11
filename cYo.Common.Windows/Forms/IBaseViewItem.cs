using System.ComponentModel;
using System.Drawing;

namespace cYo.Common.Windows.Forms
{
	public interface IBaseViewItem : INotifyPropertyChanged
	{
		string Text
		{
			get;
		}

		string Name
		{
			get;
		}

		string TooltipText
		{
			get;
		}

		object Tag
		{
			get;
			set;
		}

		object Data
		{
			get;
			set;
		}

		ItemView View
		{
			get;
			set;
		}

		int HitTest(Point pt);
	}
}
