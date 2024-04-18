using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cYo.Common.Xml;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
    public class SharpCompressEngine : FileBasedAccessor
    {
        private int format;

        public SharpCompressEngine(int format)
            : base(format)
        {
            this.format = format;
        }

        public override bool IsFormat(string source)
        {
            if (base.HasSignature)
            {
                return base.IsFormat(source);
            }
            try
            {
                using (IArchive archive = ArchiveFactory.Open(source))
                {
                    switch (archive.Type)
                    {
                        case ArchiveType.Rar:
                            return format == KnownFileFormats.CBR;
                        case ArchiveType.Zip:
                            return format == KnownFileFormats.CBZ;
                        case ArchiveType.Tar:
                            return format == KnownFileFormats.CBT;
                        case ArchiveType.SevenZip:
                            return format == KnownFileFormats.CB7;
                        default:
                            return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public override IEnumerable<ProviderImageInfo> GetEntryList(string source)
        {
            using (IArchive archive = ArchiveFactory.Open(source))
            {
                int i = 0;
                foreach (IArchiveEntry entry in archive.Entries)
                {
                    if (!entry.IsDirectory)
                    {
                        yield return new ProviderImageInfo(i, entry.Key, entry.Size);
                    }
                    i++;
                }
            }
        }

        public override byte[] ReadByteImage(string source, ProviderImageInfo info)
        {
            using (IArchive archive = ArchiveFactory.Open(source))
            {
                IArchiveEntry archiveEntry = archive.Entries.Skip(info.Index).First();
                MemoryStream memoryStream = new MemoryStream((int)archiveEntry.Size);
                archiveEntry.WriteTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public override ComicInfo ReadInfo(string source)
        {
            using (IArchive archive = ArchiveFactory.Open(source))
            {
                IArchiveEntry archiveEntry = archive.Entries.FirstOrDefault((IArchiveEntry e) => Path.GetFileName(e.Key).Equals("ComicInfo.xml", StringComparison.OrdinalIgnoreCase));
                if (archiveEntry == null)
                {
                    return null;
                }
                using (Stream s = archiveEntry.OpenEntryStream())
                {
                    return XmlUtility.Load<ComicInfo>(s, compressed: false);
                }
            }
        }
    }
}
