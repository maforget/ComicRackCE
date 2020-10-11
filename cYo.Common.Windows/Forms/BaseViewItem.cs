using System;
using System.ComponentModel;
using System.Drawing;

namespace cYo.Common.Windows.Forms
{
	public abstract class BaseViewItem : IBaseViewItem, INotifyPropertyChanged
	{
		private string name = string.Empty;

		private string text = string.Empty;

		private string tooltipText = string.Empty;

		private object tag;

		private object data;

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (!(name == value))
				{
					name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		public virtual string Text
		{
			get
			{
				return text;
			}
			set
			{
				if (!(text == value))
				{
					text = value;
					OnPropertyChanged("Text");
				}
			}
		}

		public string TooltipText
		{
			get
			{
				return tooltipText;
			}
			set
			{
				if (!(tooltipText == value))
				{
					tooltipText = value;
					OnPropertyChanged("TooltipText");
				}
			}
		}

		public object Tag
		{
			get
			{
				return tag;
			}
			set
			{
				if (tag != value)
				{
					tag = value;
					OnPropertyChanged("Tag");
				}
			}
		}

		public object Data
		{
			get
			{
				return data;
			}
			set
			{
				if (data != value)
				{
					data = value;
					OnPropertyChanged("Data");
				}
			}
		}

		public ItemView View
		{
			get;
			set;
		}

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		protected BaseViewItem()
		{
		}

		protected BaseViewItem(string text)
		{
			this.text = text;
		}

		protected BaseViewItem(string text, object tag)
			: this(text)
		{
			this.tag = tag;
		}

		protected BaseViewItem(string text, object tag, object data)
			: this(text, tag)
		{
			this.data = data;
		}

		protected virtual void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public virtual int HitTest(Point pt)
		{
			return -1;
		}
	}
}
