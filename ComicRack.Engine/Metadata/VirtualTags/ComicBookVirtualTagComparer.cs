using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cYo.Common.Text;
using cYo.Common.Windows.Forms;

namespace cYo.Projects.ComicRack.Engine.Metadata.VirtualTags
{
	public class ComicBookVirtualTagComparer : ComicBookSeriesComparer
	{
		private readonly string _property;

		public ComicBookVirtualTagComparer(string property)
		{
			_property = property;
		}

		public override int Compare(ComicBook x, ComicBook y)
		{
			int num = ExtendedStringComparer.Compare(x.GetStringPropertyValue(_property), y.GetStringPropertyValue(_property), ExtendedStringComparison.IgnoreCase | ExtendedStringComparison.IgnoreArticles);

			return num != 0 ? num : base.Compare(x, y);
		}
	}
}
