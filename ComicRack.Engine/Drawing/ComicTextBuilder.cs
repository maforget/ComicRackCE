using System;
using System.Collections.Generic;
using System.Drawing;
using cYo.Common.Drawing;
using cYo.Common.Localize;
using cYo.Common.Text;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	public static class ComicTextBuilder
	{
		private static readonly TR TRComic = TR.Load("ComicBook");

		private static readonly string NoInformationAvailable = TRComic["NoInformation", "No information available"];

		private static readonly string SizeText = TRComic["Size", "Size:\t{0}/{1}"];

		private static readonly string OpenedText = TRComic["Opened", "Opened:\t{0}"];

		private static readonly string AddedText = TRComic["Added", "Added:\t{0}"];

		private static readonly string ReleasedText = TRComic["Released", "Released:\t{0}"];

		private static readonly string FormatText = TRComic["Format", "Format:\t{0}"];

		private static readonly string FileNameText = TRComic["FileName", "File:\t{0}"];

		private static readonly string PageText = TRComic["PageText", "Page #{0}"];

		private static readonly string PageSizeText = TRComic["PageSize", "Size:\t{0}"];

		private static readonly string UnknownSizeText = TRComic["UnknownSize", "Unknown Size"];

		private static readonly string ResolutionText = TRComic["Resolution", "Resolution:\t{0} x {1}"];

		private static readonly string RotationText = TRComic["Rotation", "Rotation:\t{0}"];

		private static readonly string BookmarkText = TRComic["Bookmark", "Bookmark:\t{0}"];

		private static readonly string BookCountText = TRComic["BookCount", "Books:\t{0}"];

		public static IEnumerable<TextLine> GetTextBlocks(ComicBook comicBook, Font font, Color foreColor, ComicTextElements flags)
		{
			if (comicBook == null)
			{
				yield return new TextLine(NoInformationAvailable, GetCaptionFont(font), foreColor);
				yield break;
			}
			if (!comicBook.IsLinked)
			{
				flags &= ~ComicTextElements.LinkedElements;
			}
			if (flags.HasFlag(ComicTextElements.StackTitle))
			{
				yield return new TextLine(comicBook.Series, GetCaptionFont(font), foreColor);
			}
			if (flags.HasFlag(ComicTextElements.Caption))
			{
				yield return new TextLine(comicBook.Caption, GetCaptionFont(font), foreColor);
			}
			if (flags.HasFlag(ComicTextElements.CaptionWithoutTitle))
			{
				yield return new TextLine(comicBook.CaptionWithoutTitle, GetCaptionFont(font), foreColor);
			}
			if (flags.HasFlag(ComicTextElements.Title))
			{
				yield return new TextLine(comicBook.ShadowTitle, GetCaptionFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.AlternateCaption))
			{
				yield return new TextLine(comicBook.AlternateCaption, GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.PublisherAndImprint))
			{
				yield return new TextLine(comicBook.Publisher.AppendWithSeparator("/", comicBook.Imprint, comicBook.ISBN), GetSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.AgeRating))
			{
				yield return new TextLine(comicBook.AgeRating, GetSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.ArtistInfo))
			{
				yield return new TextLine(comicBook.ArtistInfo, GetSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			yield return new TextLine(4);
			yield return new TextLine(0)
			{
				ScrollStart = true
			};
			if (flags.HasFlag(ComicTextElements.Genre))
			{
				yield return new TextLine(comicBook.Genre, GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.CharactersAndTeams))
			{
				yield return new TextLine(comicBook.Characters.AppendWithSeparator(", ", comicBook.Teams), GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.Locations))
			{
				yield return new TextLine(comicBook.Locations, GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			yield return new TextLine(4);
			if (flags.HasFlag(ComicTextElements.PurchaseInformation))
			{
				yield return new TextLine(comicBook.BookStore.AppendWithSeparator("/", comicBook.BookPriceAsText), GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.StorageInformation))
			{
				yield return new TextLine(comicBook.BookLocation.AppendWithSeparator("/", comicBook.BookAge, comicBook.BookCondition), GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.CollectionStatus))
			{
				yield return new TextLine(comicBook.BookCollectionStatus, GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			yield return new TextLine(6);
			if (flags.HasFlag(ComicTextElements.Summary))
			{
				StringFormat format = new StringFormat
				{
					Trimming = StringTrimming.EllipsisWord
				};
				yield return new TextLine(comicBook.Summary.Replace("\t", string.Empty), GetSmallFont(font), foreColor, format);
			}
			if (flags.HasFlag(ComicTextElements.Notes))
			{
				StringFormat format2 = new StringFormat
				{
					Trimming = StringTrimming.EllipsisWord
				};
				yield return new TextLine(comicBook.Notes.AppendWithSeparator("\n", comicBook.BookNotes).Replace("\t", string.Empty), GetSmallFont(font), foreColor, format2);
			}
			if (flags.HasFlag(ComicTextElements.ScanInformation))
			{
				yield return new TextLine(comicBook.ScanInformation, GetNormalFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			yield return new TextLine(6);
			if (flags.HasFlag(ComicTextElements.StackBookCount))
			{
				yield return new TextLine(StringUtility.Format(BookCountText, comicBook.CountAsText), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.StackBooksOpened))
			{
				yield return new TextLine(StringUtility.Format(OpenedText, comicBook.LastPageRead), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.FileSize))
			{
				yield return new TextLine(StringUtility.Format(SizeText, comicBook.FileSizeAsText, comicBook.PagesAsText), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.Released) && (!flags.HasFlag(ComicTextElements.NoEmptyDates) || comicBook.ReleasedTime != DateTime.MinValue))
			{
				yield return new TextLine(StringUtility.Format(ReleasedText, comicBook.ReleasedTimeAsText), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.Opened) && (!flags.HasFlag(ComicTextElements.NoEmptyDates) || comicBook.OpenedTime != DateTime.MinValue))
			{
				yield return new TextLine(StringUtility.Format(OpenedText, comicBook.OpenedTimeAsText), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.Added) && (!flags.HasFlag(ComicTextElements.NoEmptyDates) || comicBook.AddedTime != DateTime.MinValue))
			{
				yield return new TextLine(StringUtility.Format(AddedText, comicBook.AddedTimeAsText), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.FileFormat))
			{
				yield return new TextLine(StringUtility.Format(FormatText, comicBook.FileFormat), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
			if (flags.HasFlag(ComicTextElements.FileName))
			{
				yield return new TextLine(StringUtility.Format(FileNameText, comicBook.FileNameWithExtension), GetSmallSmallFont(font), foreColor, StringFormatFlags.NoWrap);
			}
		}

		public static IEnumerable<TextLine> GetTextBlocks(ComicPageInfo cpi, int page, Font font, Color foreColor, ComicTextElements flags)
		{
			if (cpi.IsEmpty)
			{
				yield return new TextLine(NoInformationAvailable, GetCaptionFont(font), foreColor);
				yield break;
			}
			yield return new TextLine(string.Format(PageText, page + 1), GetCaptionFont(font), foreColor);
			yield return new TextLine(cpi.PageTypeAsText, GetSmallFont(font), foreColor);
			yield return new TextLine(10);
			if (cpi.ImageFileSize != 0)
			{
				yield return new TextLine(StringUtility.Format(PageSizeText, cpi.ImageFileSizeAsText), GetSmallFont(font), foreColor);
			}
			else
			{
				yield return new TextLine(UnknownSizeText, GetSmallFont(font), foreColor);
			}
			yield return new TextLine(StringUtility.Format(ResolutionText, cpi.ImageWidthAsText, cpi.ImageHeightAsText), GetSmallFont(font), foreColor);
			if (cpi.Rotation != 0)
			{
				yield return new TextLine(StringUtility.Format(RotationText, cpi.RotationAsText), GetSmallFont(font), foreColor);
			}
			yield return new TextLine(6);
			if (cpi.IsBookmark)
			{
				yield return new TextLine(StringUtility.Format(BookmarkText, cpi.Bookmark), GetSmallFont(font), foreColor);
			}
		}

		private static Font GetCaptionFont(Font font)
		{
			return FC.Get(font, FontStyle.Bold);
		}

		private static Font GetNormalFont(Font font)
		{
			return font;
		}

		private static Font GetSmallFont(Font font)
		{
			return FC.GetRelative(font, 0.95f);
		}

		private static Font GetSmallSmallFont(Font font)
		{
			return FC.GetRelative(font, 0.9f);
		}
	}
}
