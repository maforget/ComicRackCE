using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using cYo.Common.Drawing;
using cYo.Common.Text;
using cYo.Common.Threading;
using cYo.Common.Win32;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("DjVu Document", KnownFileFormats.DJVU, ".djvu")]
	public class DjvuComicProvider : ComicProvider, IValidateProvider
	{
		private static readonly string ListExe = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources\\djvm.exe");

		private static readonly Regex rxList = new Regex("(?<size>\\d+)\\s+PAGE\\s+#(?<page>\\d+)\\s+(?<name>.*)\\r", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private ProviderImageInfo[] pages;

		public override ImageProviderCapabilities Capabilities => ImageProviderCapabilities.FastFormatCheck;

		public bool IsValid => File.Exists(ListExe);

		protected override bool OnFastFormatCheck(string source)
		{
			try
			{
				using (FileStream fileStream = File.OpenRead(source))
				{
					byte[] array = new byte[10];
					fileStream.Read(array, 0, array.Length);
					return DjVuImage.IsDjvu(array);
				}
			}
			catch
			{
				return true;
			}
		}

		public override string CreateHash()
		{
			using (FileStream inputStream = File.OpenRead(base.Source))
			{
				return Base32.ToBase32String(new SHA1Managed().ComputeHash(inputStream));
			}
		}

		protected override void OnParse()
		{
			foreach (ProviderImageInfo page in GetPages())
			{
				FireIndexReady(page);
			}
		}

		protected override byte[] OnRetrieveSourceByteImage(int index)
		{
			using (Bitmap image = DjVuImage.GetBitmap(base.Source, index))
			{
				return image.ImageToJpegBytes();
			}
		}

		private IEnumerable<ProviderImageInfo> GetPages()
		{
			using (ItemMonitor.Lock(this))
			{
				if (pages == null)
				{
					pages = ReadPages().ToArray();
				}
				return pages;
			}
		}

		private IEnumerable<ProviderImageInfo> ReadPages()
		{
			ExecuteProcess.Result result = ExecuteProcess.Execute(ListExe, $"-l \"{base.Source}\"", ExecuteProcess.Options.StoreOutput);
			return from Match m in rxList.Matches(result.ConsoleText)
				select new ProviderImageInfo(int.Parse(m.Groups["page"].Value) - 1, m.Groups["name"].Value, long.Parse(m.Groups["size"].Value));
		}
	}
}
