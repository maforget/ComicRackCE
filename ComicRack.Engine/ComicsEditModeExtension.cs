namespace cYo.Projects.ComicRack.Engine
{
	public static class ComicsEditModeExtension
	{
		public static bool IsLocalComic(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.Local) != 0;
		}

		public static bool CanEditProperties(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.EditProperties) != 0;
		}

		public static bool CanEditPages(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.EditPages) != 0;
		}

		public static bool CanExport(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.ExportComic) != 0;
		}

		public static bool CanDeleteComics(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.DeleteComics) != 0;
		}

		public static bool CanShowComics(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.Local) != 0;
		}

		public static bool CanEditList(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.EditComicList) != 0;
		}

		public static bool CanScan(this ComicsEditModes em)
		{
			return (em & ComicsEditModes.Rescan) != 0;
		}
	}
}
