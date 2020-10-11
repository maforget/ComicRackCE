using System;
using System.IO;

namespace cYo.Common.IO
{
	public class FileKey
	{
		private string File
		{
			get;
			set;
		}

		private long Size
		{
			get;
			set;
		}

		private DateTime Modified
		{
			get;
			set;
		}

		public FileKey(string file)
		{
			FileInfo fileInfo = new FileInfo(file);
			File = file;
			Modified = fileInfo.LastWriteTimeUtc;
			Size = fileInfo.Length;
		}

		public override bool Equals(object obj)
		{
			FileKey fileKey = obj as FileKey;
			if (fileKey != null && File == fileKey.File && Size == fileKey.Size)
			{
				return Modified == fileKey.Modified;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return File.GetHashCode() ^ Size.GetHashCode() ^ Modified.GetHashCode();
		}
	}
}
