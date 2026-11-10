using cYo.Projects.ComicRack.Engine;
using System.Collections.Generic;
using System.Windows.Forms;

namespace cYo.Projects.ComicRack.Plugins
{
	/// <summary>
	/// Base of the hook interfaces. A <see cref="DotNetPlugin"/> class gets one command for each hook interface it implements.
	/// </summary>
	/// <remarks>
	/// Several hooks have the same Execute signature (for example <see cref="ILibrary"/>, <see cref="IEditor"/>, <see cref="IBooks"/> and <see cref="INewBooks"/>).
	/// A class that implements more than one of them with a public Execute method runs the same code for each hook.
	/// To do something different for each hook, implement the interfaces explicitly:
	/// <code>
	/// void ILibrary.Execute(ComicBook[] books) { ... }
	/// void IBooks.Execute(ComicBook[] books) { ... }
	/// </code>
	/// </remarks
	public interface IDotNetCommand;

	/// <summary>Supplies the books of a smart list rule.</summary>
	public interface ICreateBookList : IDotNetCommand
	{
		/// <summary>The number of parameters (0 to 2) the smart list editor asks for.</summary>
		public int PCount { get; set; }

		/// <param name="books">The books to select from.</param>
		/// <param name="a">The first parameter from the smart list editor.</param>
		/// <param name="b">The second parameter from the smart list editor.</param>
		IEnumerable<ComicBook> Execute(ComicBook[] books, string a, string b);
	}


	/// <summary>Reads book information from a file path. Change <c>proposed</c> to set the result.</summary>
	public interface IParseComicPath : IDotNetCommand
	{
		public void Execute(string path, ComicNameInfo proposed);
	}

	/// <summary>A command in the File &gt; Automation menu. It gets all books of the library.</summary>
	public interface ILibrary : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	/// <summary>A command in the book information dialog. It gets the book that is edited.</summary>
	public interface IEditor : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	/// <summary>A command in the Automation menu of the book list. It gets the selected books.</summary>
	public interface IBooks : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	/// <summary>A command in the File menu, below the New command. It gets all books of the library.</summary>
	public interface INewBooks : IDotNetCommand
	{
		public void Execute(ComicBook[] books);
	}

	/// <summary>Runs when a book is opened in the reader.</summary>
	public interface IBookOpened : IDotNetCommand
	{
		public void Execute(ComicBook book);
	}

	/// <summary>Runs when the size of the reader changes.</summary>
	public interface IReaderResized : IDotNetCommand
	{
		public void Execute(int width, int height);
	}

	/// <summary>A search provider. The result maps a display name to a link. A <c>limit</c> of -1 asks for one generic search link.</summary>
	public interface ISearch : IDotNetCommand
	{
		public Dictionary<string, string> Execute(string hint, string text, int limit);
	}

	/// <summary>The Configure action of the other commands of the same plugin class.</summary>
	public interface IConfig : IDotNetCommand
	{
		public void Execute();
	}

	/// <summary>Runs after ComicRack has started.</summary>
	public interface IStartup : IDotNetCommand
	{
		public void Execute();
	}

	/// <summary>Runs when ComicRack closes. Return false to cancel a close that the user started.</summary>
	public interface IShutdown : IDotNetCommand
	{
		public bool Execute(bool user_is_closing);
	}

	/// <summary>A book information panel that shows the returned HTML.</summary>
	public interface IComicInfoHtml : IDotNetCommand
	{
		public string Execute(ComicBook[] books);
	}

	/// <summary>A book information panel that shows the returned control. The Control needs to implement ShowInfo</summary>
	public interface IComicInfoUI : IDotNetCommand
	{
		public UIControl Execute();
	}

	/// <summary>A Quick Open panel that shows the returned HTML.</summary>
	public interface IQuickOpenHtml : IDotNetCommand
	{
		public string Execute(ComicBook[] books);
	}

	/// <summary>A Quick Open panel that shows the returned control. The Control needs to implement ShowInfo</summary>
	public interface IQuickOpenUI : IDotNetCommand
	{
		public UIControl Execute();
	}

	/// <summary>Draws on top of a book thumbnail.</summary>
	public interface IDrawThumbnailOverlay : IDotNetCommand
	{
		/// <param name="flags">A bit mask: 1 = stacked, 2 = selected, 4 = hot.</param>
		public void Execute(ComicBook book, System.Drawing.Graphics graphics, System.Drawing.Rectangle bounds, int flags);
	}

	/// <summary>A Control that implements <c>ShowInfo</c> to display book information. Returned by <see cref="IComicInfoUI"/> and <see cref="IQuickOpenUI"/>.</summary>
	public abstract class UIControl: Control
	{
		public abstract void ShowInfo(ComicBook[] books);
	}
}