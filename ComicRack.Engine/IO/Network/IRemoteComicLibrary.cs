using System;
using System.ServiceModel;
using cYo.Common.Drawing;

namespace cYo.Projects.ComicRack.Engine.IO.Network
{
	[ServiceContract]
	public interface IRemoteComicLibrary
	{
		bool IsValid
		{
			[OperationContract]
			get;
		}

		[OperationContract]
		byte[] GetLibraryData();

		[OperationContract]
		int GetImageCount(Guid comicGuid);

		[OperationContract]
		byte[] GetImage(Guid comicGuid, int index);

		[OperationContract]
		byte[] GetThumbnailImage(Guid comicGuid, int index);

		[OperationContract]
		[ServiceKnownType(typeof(BitmapAdjustment))]
		void UpdateComic(Guid comicGuid, string propertyName, object value);
	}
}
