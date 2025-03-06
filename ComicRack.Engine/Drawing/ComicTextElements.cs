using System;
using System.ComponentModel;

namespace cYo.Projects.ComicRack.Engine.Drawing
{
	[Flags]
	public enum ComicTextElements
	{
		None = 0x0,
		Caption = 0x1,
		[Description("Caption without Title")]
		CaptionWithoutTitle = 0x2,
		AlternateCaption = 0x4,
		Title = 0x8,
		[Description("Artists")]
		ArtistInfo = 0x10,
		Summary = 0x20,
		FileSize = 0x40,
		Opened = 0x80,
		FileName = 0x100,
		FileFormat = 0x200,
		Added = 0x400,
		[Description("Publisher and Imprint")]
		PublisherAndImprint = 0x800,
		AgeRating = 0x1000,
		Genre = 0x2000,
		[Description("Characters and Teams")]
		CharactersAndTeams = 0x4000,
		Locations = 0x8000,
		Notes = 0x10000,
		PurchaseInformation = 0x20000,
		[Description("Storage Information")]
		StorageInfoformation = 0x40000,
		CollectionStatus = 0x80000,
		ScanInformation = 0x100000,
		Released = 0x200000,
		StackTitle = 0x1000000,
		StackBookCount = 0x2000000,
		StackBooksOpened = 0x4000000,
		NoEmptyDates = 0x10000000,
		DefaultComic = 0x27A,
		DefaultFileComic = 0x37A,
		AllComic = 0x3FFFFF,
		DefaultStack = 0x7000050,
		LinkedElements = 0x340,
		DefaultPage = 0x8000000
	}
}
