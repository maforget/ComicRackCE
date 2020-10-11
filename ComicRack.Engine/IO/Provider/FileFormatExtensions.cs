using System.Collections.Generic;
using System.Linq;
using System.Text;
using cYo.Common.Localize;

namespace cYo.Projects.ComicRack.Engine.IO.Provider
{
	public static class FileFormatExtensions
	{
		public static string GetDialogFilter(this IEnumerable<FileFormat> formats, bool withAllFilter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = (withAllFilter ? new StringBuilder() : null);
			foreach (FileFormat format in formats)
			{
				if (format == null)
				{
					continue;
				}
				if (stringBuilder.Length != 0)
				{
					stringBuilder.Append("|");
				}
				stringBuilder.Append(format.Name);
				stringBuilder.Append("|");
				stringBuilder.Append(format.ExtensionFilter);
				if (stringBuilder2 != null)
				{
					if (stringBuilder2.Length != 0)
					{
						stringBuilder2.Append(";");
					}
					stringBuilder2.Append(format.ExtensionFilter);
				}
			}
			if (stringBuilder2 != null)
			{
				stringBuilder.Append("|");
				stringBuilder.Append(TR.Load("FileFilter")["AllSupportedFiles", "All supported Files"]);
				stringBuilder.Append("|");
				stringBuilder.Append(stringBuilder2);
				stringBuilder.Append("|");
				stringBuilder.Append(TR.Load("FileFilter")["AllFiles", "All Files"]);
				stringBuilder.Append("|*.*");
			}
			return stringBuilder.ToString();
		}

		public static IEnumerable<string> GetExtensions(this IEnumerable<FileFormat> formats)
		{
			return formats.SelectMany((FileFormat ff) => ff.Extensions);
		}
	}
}
