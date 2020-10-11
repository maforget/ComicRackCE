using System;
using System.ComponentModel;
using cYo.Common.ComponentModel;
using cYo.Common.Windows;
using cYo.Common.Windows.Forms;
using cYo.Projects.ComicRack.Engine.Database;

namespace cYo.Projects.ComicRack.Viewer.Config
{
	[Serializable]
	public class ListConfiguration : IComparable<ListConfiguration>, INamed, IDescription
	{
		[DefaultValue("")]
		public string Name
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public DisplayListConfig Config
		{
			get;
			set;
		}

		public string Description => LocalizeUtility.LocalizeEnum(typeof(ItemViewMode), (int)Config.View.ItemViewMode);

		public ListConfiguration()
			: this(string.Empty)
		{
		}

		public ListConfiguration(string name)
		{
			Name = name;
		}

		public int CompareTo(ListConfiguration other)
		{
			return string.Compare(Name, other.Name);
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
