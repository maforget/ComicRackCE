using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using cYo.Common.ComponentModel;

namespace cYo.Common.Windows.Forms
{
	public interface IColumn : IBaseViewItem, INotifyPropertyChanged
	{
		int Id
		{
			get;
			set;
		}

		int FormatId
		{
			get;
			set;
		}

		string[] FormatTexts
		{
			get;
		}

		bool Visible
		{
			get;
			set;
		}

		int Width
		{
			get;
			set;
		}

		DateTime LastTimeVisible
		{
			get;
			set;
		}

		IComparer<IViewableItem> ColumnSorter
		{
			get;
			set;
		}

		IGrouper<IViewableItem> ColumnGrouper
		{
			get;
			set;
		}

		StringAlignment Alignment
		{
			get;
			set;
		}

		void DrawHeader(Graphics gr, Rectangle rc, HeaderState style);
	}
}
