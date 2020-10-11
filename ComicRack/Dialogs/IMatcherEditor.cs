namespace cYo.Projects.ComicRack.Viewer.Dialogs
{
	public interface IMatcherEditor
	{
		void AddRule();

		void AddGroup();

		void DeleteRuleOrGroup();

		void CopyClipboard();

		void CutClipboard();

		void MoveDown();

		void MoveUp();

		void PasteClipboard();

		void SetFocus();
	}
}
