using System.IO;
using cYo.Common.ComponentModel;

namespace cYo.Common.IO
{
	public class LockFile : DisposableObject
	{
		private readonly string file;

		public bool WasLocked
		{
			get;
			private set;
		}

		public LockFile(string file)
		{
			this.file = file;
			if (File.Exists(file))
			{
				WasLocked = true;
			}
			else
			{
				using (File.Create(file))
				{
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			FileUtility.SafeDelete(file);
			base.Dispose(disposing);
		}
	}
}
