using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using cYo.Common.IO;
using cYo.Common.Runtime;
using cYo.Common.Win32;
using Microsoft.Win32;

namespace cYo.Common.Drawing
{
	public class PdfImages
	{
		private const string GhostScriptWin32 = "gswin32c.exe";

		private const string GhostScriptWin64 = "gswin64c.exe";

		private string tempPath = Path.GetTempPath();

		private static readonly Regex rxCount = new Regex("file:\\s(?<count>\\d+)", RegexOptions.Compiled);

		private string pdfFile;

		private volatile int pageCount;

		private static string ghostscriptPath;

		private static bool searched;

		public string TempPath
		{
			get
			{
				return tempPath;
			}
			set
			{
				tempPath = value;
			}
		}

		public string PdfFile => pdfFile;

		public int PageCount
		{
			get
			{
				return pageCount;
			}
			set
			{
				pageCount = value;
			}
		}

		public static string GhostscriptPath
		{
			get
			{
				if (ghostscriptPath == null && !searched)
				{
					ghostscriptPath = CheckRegistry("GPL Ghostscript") ?? CheckRegistry("AFPL Ghostscript") ?? CheckProgramPath("gs") ?? CheckProgramPath("Ghostscript") ?? CheckProgramPath("GPL Ghostscript");
					searched = true;
				}
				return ghostscriptPath;
			}
			set
			{
				ghostscriptPath = value;
			}
		}

		public static bool IsGhostscriptAvailable
		{
			get
			{
				try
				{
					return !string.IsNullOrEmpty(GhostscriptPath) && File.Exists(GhostscriptPath);
				}
				catch
				{
					return false;
				}
			}
		}

		public PdfImages()
		{
		}

		public PdfImages(string pdfFile, string tempPath)
		{
			if (!string.IsNullOrEmpty(tempPath))
			{
				this.tempPath = tempPath;
			}
			Open(pdfFile);
		}

		public PdfImages(string pdfFile)
			: this(pdfFile, null)
		{
		}

		public void Open(string pdfFile)
		{
			string input = ExecuteGhostscript("-q -dBATCH -dNOPAUSE -dSAFER -sDEVICE=nullpage -dFirstPage=100000 \"{0}\"", pdfFile);
			Match match = rxCount.Match(input);
			if (match.Success)
			{
				PageCount = int.Parse(match.Groups["count"].Value);
				this.pdfFile = pdfFile;
				return;
			}
			throw new FileLoadException();
		}

		public void Close()
		{
			pdfFile = null;
			PageCount = 0;
		}

		public Bitmap GetPage(int page, int dpi)
		{
			byte[] pageData = GetPageData(page, dpi);
			try
			{
				return (pageData == null) ? null : ((Bitmap)Image.FromStream(new MemoryStream(pageData), useEmbeddedColorManagement: false));
			}
			catch (Exception)
			{
				return null;
			}
		}

		public byte[] GetPageData(int page, int dpi)
		{
			if (page >= PageCount)
			{
				return null;
			}
			string path = TempPath;
			string path2 = string.Concat(Guid.NewGuid(), ".tmp");
			string text = Path.Combine(path, path2);
			try
			{
				ExecuteGhostscript("-r{0} -q -dBATCH -dNOPAUSE -dTextAlphaBits=4 -dGraphicsAlphaBits=4 -dUseCropBox -dUseTrimBox -dFIXEDRESOLUTION -sDEVICE=jpeg -dFirstPage={1} -dLastPage={1} -sOutputFile=\"{2}\" \"{3}\"", dpi, page + 1, text, pdfFile);
				return File.ReadAllBytes(text);
			}
			catch
			{
				try
				{
					ExecuteGhostscript("-r{0} -q -dBATCH -dNOPAUSE -dTextAlphaBits=4 -dGraphicsAlphaBits=4 -dFIXEDRESOLUTION -sDEVICE=jpeg -dFirstPage={1} -dLastPage={1} -sOutputFile=\"{2}\" \"{3}\"", dpi, page + 1, text, pdfFile);
					return File.ReadAllBytes(text);
				}
				catch
				{
					return null;
				}
			}
			finally
			{
				FileUtility.SafeDelete(text);
			}
		}

		private static string CheckRegistry(string key)
		{
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\" + key, writable: false))
				{
					if (registryKey == null)
					{
						return null;
					}
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(registryKey.GetSubKeyNames()[0]))
					{
						if (registryKey2 == null)
						{
							return null;
						}
						string text = Path.Combine(Path.GetDirectoryName(registryKey2.GetValue("GS_DLL").ToString()), GhostScriptWin32);
						if (!File.Exists(text))
						{
							text = Path.Combine(Path.GetDirectoryName(registryKey2.GetValue("GS_DLL").ToString()), GhostScriptWin64);
						}
						if (!File.Exists(text))
						{
							text = null;
						}
						return text;
					}
				}
			}
			catch
			{
				return null;
			}
		}

		private static string CheckPath(string path)
		{
			return FileUtility.GetFiles(path, SearchOption.AllDirectories).FirstOrDefault((string s) => Path.GetFileName(s).Equals(GhostScriptWin32, StringComparison.OrdinalIgnoreCase)) ?? FileUtility.GetFiles(path, SearchOption.AllDirectories).FirstOrDefault((string s) => Path.GetFileName(s).Equals(GhostScriptWin64, StringComparison.OrdinalIgnoreCase));
		}

		private static string CheckProgramPath(string path)
		{
			if (Machine.Is64Bit)
			{
				return CheckPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), path)) ?? CheckPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), path));
			}
			return CheckPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), path));
		}

		private static string ExecuteGhostscript(string arguments, params object[] objs)
		{
			ExecuteProcess.Result result = ExecuteProcess.Execute(GhostscriptPath, string.Format(arguments, objs), ExecuteProcess.Options.StoreOutput);
			if (result.ExitCode != 0)
			{
				throw new FileLoadException();
			}
			return result.ConsoleText;
		}
	}
}
