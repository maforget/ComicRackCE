using cYo.Projects.ComicRack.Engine;
using System.Collections.Generic;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Plugins
{
	public interface IDotNetCommand;

	public interface ICreateBookList : IDotNetCommand
	{
		// Uses PCount to determine how many parameters are needed for the command.
		// Only used by ICreateBookList so adding this here for now, but could be moved to IDotNetCommand if needed for other commands in the future.
		public int PCount { get; set; }

		// a is the first paramter from the smartlist editor
		// b is the second parameter
		IEnumerable<ComicBook> Execute(ComicBook[] books, string a, string b);
	}

	public interface IParseComicPath : IDotNetCommand
	{
		public string Execute(string path, ComicNameInfo proposed);
	}

	public interface ILibrary : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	public interface IEditor : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	public interface IBooks : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	public interface INewBooks : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	public interface IBookOpened : IDotNetCommand
	{
		public void Execute(ComicBook book);
	}

	public interface IReaderResized : IDotNetCommand
	{
		public void Execute(int width, int height);
	}

	public interface ISearch : IDotNetCommand
	{
		public Dictionary<string, string> Execute(string hint, string text, int limit);
	}

	public interface IConfig : IDotNetCommand
	{
		public void Execute();
	}

	public interface IStartup : IDotNetCommand
	{
		public void Execute();
	}

	public interface IShutdown : IDotNetCommand
	{
		public bool Execute(bool user_is_closing);
	}

	public interface IComicInfoHtml : IDotNetCommand
	{
		public string Execute(ComicBook[] books);
	}

	public interface IComicInfoUI : IDotNetCommand
	{
		public Control Execute(ComicBook[] books);
	}

	public interface IQuickOpenHtml : IDotNetCommand
	{
		public string Execute(ComicBook[] books);
	}

	public interface IQuickOpenUI : IDotNetCommand
	{
		public Control Execute(ComicBook[] books);
	}

	public interface IDrawThumbnailOverlay : IDotNetCommand
	{
		// flags is a bit-mask with 1: Stacked // 2: Selected // 4: Hot
		public void Execute(ComicBook book, System.Drawing.Graphics graphics, System.Drawing.Rectangle bounds, int flags);
	}
}