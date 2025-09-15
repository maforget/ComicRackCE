using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using cYo.Common.IO;
using cYo.Common.Localize;
using cYo.Projects.ComicRack.Engine.IO.Provider;

namespace cYo.Projects.ComicRack.Engine.IO
{
	[Serializable]
	public class ExportSetting : StorageSetting
	{
		[XmlAttribute]
		public string Name
		{
			get;
			set;
		}

		public static string DefaultName => TR.Default["New", "New"];

		[DefaultValue(ExportTarget.NewFolder)]
		public ExportTarget Target
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string TargetFolder
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool DeleteOriginal
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool AddToLibrary
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool Overwrite
		{
			get;
			set;
		}

		[DefaultValue(false)]
		public bool Combine
		{
			get;
			set;
		}

		[DefaultValue(ExportNaming.Filename)]
		public ExportNaming Naming
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string CustomName
		{
			get;
			set;
		}

		[DefaultValue(1)]
		public int CustomNamingStart
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string ForcedName
		{
			get;
			set;
		}

		[DefaultValue(null)]
		public string TagsToAppend
		{
			get;
			set;
		}

		[DefaultValue(ExportImageProcessingSource.Custom)]
		public ExportImageProcessingSource ImageProcessingSource
		{
			get;
			set;
		}

		public static ExportSetting ConvertToCBZ => new ExportSetting
		{
			Name = TR.Messages["ConvertToCBZ", "Convert to CBZ"],
			Target = ExportTarget.ReplaceSource,
			FormatId = 2
		};

		public static ExportSetting ConvertToCB7 => new ExportSetting
		{
			Name = TR.Messages["ConvertToCB7", "Convert to CB7"],
			Target = ExportTarget.ReplaceSource,
			FormatId = 6
		};

		public ExportSetting()
		{
			ImageProcessingSource = ExportImageProcessingSource.Custom;
			CustomNamingStart = 1;
			Naming = ExportNaming.Filename;
			Target = ExportTarget.NewFolder;
		}

		public FileFormat GetFileFormat(ComicBook cb)
		{
			try
			{
				return Providers.Writers.GetSourceFormats().First((FileFormat ff) => (base.FormatId != 0) ? (ff.Id == base.FormatId) : (ff.Name == cb.FileFormat));
			}
			catch (Exception)
			{
				return Providers.Writers.GetSourceFormats().First((FileFormat ff) => ff.Id == 2);
			}
		}

		public string GetTargetFilePath(ComicBook cb)
		{
			if (Target != 0)
			{
				return Path.GetDirectoryName(cb.FilePath);
			}
			return TargetFolder;
		}

		public string GetTargetFileName(ComicBook cb, int index)
		{
			if (!string.IsNullOrEmpty(ForcedName))
			{
				return ForcedName;
			}
			FileFormat fileFormat = GetFileFormat(cb);
			string text;
			switch (Naming)
			{
			case ExportNaming.Caption:
				text = (string.IsNullOrEmpty(CustomName) ? cb.TargetFilename : cb.GetFullTitle(CustomName));
				break;
			case ExportNaming.Custom:
				text = (string.IsNullOrEmpty(CustomName) ? cb.FileName : CustomName);
				index += CustomNamingStart;
				if (index > 0)
				{
					text = $"{text} ({index})";
				}
				break;
			default:
				text = cb.FileName;
				break;
			}
			return FileUtility.MakeValidFilename(text) + fileFormat.MainExtension;
		}

		public string GetTargetPath(ComicBook cb, int index)
		{
			return Path.Combine(GetTargetFilePath(cb), GetTargetFileName(cb, index));
		}
	}
}
