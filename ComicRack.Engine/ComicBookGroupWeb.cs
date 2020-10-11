using System;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupWeb : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			string text = item.Web;
			try
			{
				text = new Uri(item.Web).Host;
			}
			catch (Exception)
			{
			}
			return SingleComicGrouper.GetNameGroup(text);
		}
	}
}
