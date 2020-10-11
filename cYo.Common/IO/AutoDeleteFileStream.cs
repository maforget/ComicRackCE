using System.IO;

namespace cYo.Common.IO
{
	public class AutoDeleteFileStream : FileStream
	{
		public string File
		{
			get;
			private set;
		}

		public AutoDeleteFileStream(string file)
			: base(file, FileMode.Open, FileAccess.Read, FileShare.Read)
		{
			File = file;
		}

		public override void Close()
		{
			base.Close();
			DeleteFile();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			DeleteFile();
		}

		private void DeleteFile()
		{
			FileUtility.SafeDelete(File);
		}
	}
}
