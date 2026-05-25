using System;
using System.IO;
using cYo.Common.Win32;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public static class NtfsInfoStorage
	{
		public const string ComicBookInfoStream = "ComicRackInfo";
		public const string ComicBookStream = "ComicRackBook";

		private static Func<Stream, ComicInfo> comicInfoDeserializationDelegate => ComicInfo.Deserialize;
		private static Func<Stream, ComicBook> comicBookDeserializationDelegate => ComicBook.DeserializeFull;

		public static bool StoreInfo(string file, ComicInfo comicInfo) => Store(file, comicInfo, ComicBookInfoStream, comicInfoDeserializationDelegate);
        public static bool StoreBook(string file, ComicBook comicbook) => Store(file, comicbook, ComicBookStream, comicBookDeserializationDelegate);
		private static bool Store<T>(string file, T comicInfo, string stream, Func<Stream, T> deserializationDelegate, bool append = false) where T: ComicInfo
		{
			T ci = Load(file, stream, deserializationDelegate);
			if (comicInfo.IsSameContent(ci))
				return false;

			try
			{
				FileInfo fileInfo = new FileInfo(file);
				using (StreamWriter streamWriter = AlternateDataStreamFile.CreateText(file, stream))
				{
                    comicInfo.Serialize(streamWriter.BaseStream);
                    return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}


		public static T LoadInfo<T>(string file) where T : ComicInfo
		{
			return typeof(T) switch
			{
				Type t when t == typeof(ComicInfo) => Load(file, ComicBookInfoStream, comicInfoDeserializationDelegate) as T,
				Type t when t == typeof(ComicBook) => Load(file, ComicBookStream, comicBookDeserializationDelegate) as T,
				_ => throw new NotSupportedException($"Type {typeof(T).FullName} is not supported for loading.")
			};
		}

		public static T Load<T>(string file, string stream, Func<Stream, T> deserializationDelegate)
		{
			try
			{
				if (!AlternateDataStreamFile.Exists(file, stream))
					return default;

				using (StreamReader streamReader = AlternateDataStreamFile.OpenText(file, stream))
				{
					return deserializationDelegate(streamReader.BaseStream);
				}
			}
			catch (Exception)
			{
				return default;
			}
		}


		public static void ClearInfo(string file) => Clear(file, ComicBookInfoStream);
		public static void ClearBook(string file) => Clear(file, ComicBookStream);
		public static void Clear(string file, string stream)
		{
			try
			{
				if (AlternateDataStreamFile.Exists(file, stream))
					AlternateDataStreamFile.Delete(file, stream);
			}
			catch (Exception)
			{
			}
		}
	}
}
