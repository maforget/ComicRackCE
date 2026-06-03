using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cYo.Common.Xml;
using cYo.Projects.ComicRack.Engine.IO.Provider.XmlInfo;
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
                using (IArchive archive = ArchiveFactory.OpenArchive(source))
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
            using (IArchive archive = ArchiveFactory.OpenArchive(source))
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
            using (IArchive archive = ArchiveFactory.OpenArchive(source))
            {
                IArchiveEntry archiveEntry = archive.Entries.Skip(info.Index).First();
                MemoryStream memoryStream = new MemoryStream((int)archiveEntry.Size);
                archiveEntry.WriteTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private T Read<T>(string source) where T : class
        {
            using (IArchive archive = ArchiveFactory.OpenArchive(source))
            {
                return XmlInfoProviders.Readers.DeserializeAll<T>(s =>
                {
                    IArchiveEntry archiveEntry = archive.Entries.FirstOrDefault((IArchiveEntry e) => Path.GetFileName(e.Key).Equals(s, StringComparison.OrdinalIgnoreCase));

                    if (archiveEntry == null)
                        return null;

                    return archiveEntry.OpenEntryStream();
                }) as T;
            }
        }

        public override T ReadInfo<T>(string source) => Read<T>(source);
    }
}
