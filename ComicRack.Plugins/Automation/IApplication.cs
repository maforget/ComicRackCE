using System.Collections.Generic;
using System.Drawing;
using cYo.Projects.ComicRack.Engine;

namespace cYo.Projects.ComicRack.Plugins.Automation
{
	public interface IApplication
	{
		string ProductVersion
		{
			get;
		}

		void Restart();

		void SynchronizeDevices();

		void ScanFolders();

		IEnumerable<ComicBook> ReadDatabaseBooks(string file);

		IEnumerable<ComicBook> GetLibraryBooks();

		ComicBook AddNewBook(bool showDialog);

		bool RemoveBook(ComicBook cb);

		bool SetCustomBookThumbnail(ComicBook cb, Bitmap bitmap);

		ComicBook GetBook(string file);

		Bitmap GetComicPage(ComicBook cb, int page);

		Bitmap GetComicThumbnail(ComicBook cb, int page);

		IDictionary<string, string> GetComicFields();

		Bitmap GetComicPublisherIcon(ComicBook cb);

		Bitmap GetComicImprintIcon(ComicBook cb);

		Bitmap GetComicAgeRatingIcon(ComicBook cb);

		Bitmap GetComicFormatIcon(ComicBook cb);

		string ReadInternet(string url);

		int AskQuestion(string question, string buttonText, string optionText);

		void ShowComicInfo(IEnumerable<ComicBook> books);
	}
}
