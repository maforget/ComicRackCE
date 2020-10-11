using System.Collections.Generic;
using System.Linq;
using cYo.Common.Collections;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine.Database
{
	public static class ComicListItemExtension
	{
		public static ComicListItemFolder GetItemParent(this ComicListItem item)
		{
			if (item.Library == null)
			{
				return null;
			}
			return item.Library.ComicLists.GetItems<ComicListItemFolder>().FirstOrDefault((ComicListItemFolder clif) => clif.Items.Contains(item));
		}

		public static IEnumerable<ComicListItem> GetItemPath(this ComicListItem item)
		{
			yield return item;
			while (true)
			{
				ComicListItemFolder itemParent;
				ComicListItemFolder p = (itemParent = item.GetItemParent());
				if (itemParent != null)
				{
					yield return p;
					item = p;
					continue;
				}
				break;
			}
		}

		public static string GetFullName(this ComicListItem item, string separator = ".")
		{
			string s = string.Empty;
			item.GetItemPath().Reverse().ForEach(delegate(ComicListItem n)
			{
				s = s.AppendWithSeparator(separator, n.Name);
			});
			return s;
		}

		public static int GetLevel(this ComicListItem item)
		{
			if (item.Library == null)
			{
				return 0;
			}
			return item.Library.ComicLists.GetChildLevel(item);
		}
	}
}
