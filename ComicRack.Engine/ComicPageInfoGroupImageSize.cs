using System;
using System.Collections.Generic;
using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicPageInfoGroupImageSize : IGrouper<ComicPageInfo>
	{
		public bool IsMultiGroup => false;

		public IGroupInfo GetGroup(ComicPageInfo item)
		{
			return GroupInfo.GetFileSizeGroup(item.ImageFileSize);
		}

		public IEnumerable<IGroupInfo> GetGroups(ComicPageInfo item)
		{
			throw new NotImplementedException();
		}
	}
}
