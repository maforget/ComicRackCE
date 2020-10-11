using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public abstract class SingleComicGrouper : SingleGrouper<ComicBook>
	{
		public static IGroupInfo GetNameGroup(string text)
		{
			if (ComicBook.EnableGroupNameCompression)
			{
				return GroupInfo.GetCompressedNameGroup(text);
			}
			return new GroupInfo(string.IsNullOrEmpty(text) ? GroupInfo.Unspecified : text);
		}

		public virtual ComicBookMatcher CreateMatcher(IGroupInfo info)
		{
			return null;
		}
	}
}
