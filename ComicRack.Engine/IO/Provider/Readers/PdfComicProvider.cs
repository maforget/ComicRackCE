using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using cYo.Common.Text;
using cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Pdf;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers
{
	[FileFormat("PDF Document (PDF)", KnownFileFormats.PDF, ".pdf")]
	public class PdfComicProvider : ComicProvider
	{
		private IComicAccessor pdfReader;

		private List<ProviderImageInfo> infos = new List<ProviderImageInfo>();

		public override ImageProviderCapabilities Capabilities => ImageProviderCapabilities.FastFormatCheck;

		public PdfComicProvider()
		{
			PdfGhostScript pdfGhostScript = new PdfGhostScript();
			if (pdfGhostScript.IsAvailable())
			{
				pdfReader = pdfGhostScript;
			}
			else
			{
				if (EngineConfiguration.Default.PdfEngineToUse == EngineConfiguration.PdfEngine.Native)
                    pdfReader = new PdfNative();
				else
					pdfReader = new PdfiumReaderEngine();
            }
		}

		protected override bool OnFastFormatCheck(string source)
		{
			try
			{
				using (FileStream fileStream = File.OpenRead(source))
				{
					byte[] array = new byte[4];
					fileStream.Read(array, 0, array.Length);
					return array[0] == 37 && array[1] == 80 && array[2] == 68 && array[3] == 70;
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
			foreach (ProviderImageInfo entry in pdfReader.GetEntryList(base.Source))
			{
				infos.Add(entry);
				if (!FireIndexReady(entry))
				{
					break;
				}
			}
		}

		protected override byte[] OnRetrieveSourceByteImage(int index)
		{
			return pdfReader.ReadByteImage(base.Source, infos[index]);
		}
	}
}
