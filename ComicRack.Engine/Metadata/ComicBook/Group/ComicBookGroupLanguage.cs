using cYo.Common.ComponentModel;

namespace cYo.Projects.ComicRack.Engine
{
	public class ComicBookGroupLanguage : SingleComicGrouper
	{
		public override IGroupInfo GetGroup(ComicBook item)
		{
			string languageAsText = item.LanguageAsText;
			return new GroupInfo(string.IsNullOrEmpty(languageAsText) ? GroupInfo.Unspecified : languageAsText);
		}
	}
}
