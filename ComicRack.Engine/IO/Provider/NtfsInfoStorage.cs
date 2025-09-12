using System;
using System.IO;
using cYo.Common.Win32;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public static class NtfsInfoStorage
	{
		public const string ComicBookInfoStream = "ComicRackInfo";

		public static bool StoreInfo(string file, ComicInfo comicInfo)
		{
			ComicInfo ci = LoadInfo(file);
			if (comicInfo.IsSameContent(ci))
			{
				return false;
			}
			try
			{
				FileInfo fileInfo = new FileInfo(file);
				using (StreamWriter streamWriter = AlternateDataStreamFile.CreateText(file, ComicBookInfoStream))
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

		public static ComicInfo LoadInfo(string file)
		{
			try
			{
				if (!AlternateDataStreamFile.Exists(file, ComicBookInfoStream))
				{
					return null;
				}
				using (StreamReader streamReader = AlternateDataStreamFile.OpenText(file, ComicBookInfoStream))
				{
					return ComicInfo.Deserialize(streamReader.BaseStream);
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void ClearInfo(string file)
		{
			try
			{
				if (AlternateDataStreamFile.Exists(file, ComicBookInfoStream))
				{
					AlternateDataStreamFile.Delete(file, ComicBookInfoStream);
				}
			}
			catch (Exception)
			{
			}
		}
	}
}
